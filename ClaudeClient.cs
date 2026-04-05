using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace VizzyCode
{
    public enum ClaudeMode { ClaudeCli, ApiKey }

    public class ClaudeSettings
    {
        public ClaudeMode Mode   { get; set; } = ClaudeMode.ClaudeCli;
        public string    ApiKey  { get; set; } = "";
        public string    Model   { get; set; } = "claude-sonnet-4-6";
    }

    /// <summary>
    /// Sends prompts to Claude using either:
    ///   - The installed `claude` CLI (uses your claude.ai subscription, no key needed)
    ///     Uses: claude -p --dangerously-skip-permissions --output-format stream-json
    ///           --verbose --include-partial-messages --system-prompt-file &lt;file&gt;
    ///           (message piped via stdin)
    ///   - Direct Anthropic API via HTTP (needs API key in settings)
    /// </summary>
    public class ClaudeClient
    {
        public ClaudeSettings Settings { get; set; } = new();

        // Raised for each streamed text chunk
        public event Action<string>? OnChunk;
        // Raised when full response is ready (full accumulated text)
        public event Action<string>? OnDone;
        // Raised on error
        public event Action<string>? OnError;
        // Raised for tool-use activity (e.g. "🔧 Reading file...  done")
        public event Action<string>? OnToolActivity;

        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromMinutes(10) };

        // Session ID from the last CLI call (for --continue support)
        public string? LastSessionId { get; private set; }

        public async Task SendAsync(string userMessage, string systemPrompt,
                                    CancellationToken ct = default,
                                    bool continueSession = false)
        {
            try
            {
                if (Settings.Mode == ClaudeMode.ApiKey && !string.IsNullOrWhiteSpace(Settings.ApiKey))
                    await SendViaApiAsync(userMessage, systemPrompt, ct);
                else
                    await SendViaCliAsync(userMessage, systemPrompt, ct, continueSession);
            }
            catch (OperationCanceledException)
            {
                OnError?.Invoke("Cancelled.");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Error: {ex.Message}");
            }
        }

        // ── Claude CLI mode (subscription) ────────────────────────────────────

        private async Task SendViaCliAsync(string userMessage, string systemPrompt,
                                           CancellationToken ct, bool continueSession)
        {
            string? claudeExe = FindClaudeExe();
            if (claudeExe == null)
            {
                OnError?.Invoke(
                    "'claude' CLI not found. Install Claude Code from https://claude.ai/code " +
                    "then run: claude login\n\nOr switch to API key mode in Settings (⚙).");
                return;
            }

            // Write system prompt to temp file to avoid shell-escaping issues
            string sysFile = Path.GetTempFileName();
            try
            {
                await File.WriteAllTextAsync(sysFile, systemPrompt, Encoding.UTF8, ct);

                // Build argument list:
                //   -p                          = non-interactive / print mode
                //   --dangerously-skip-permissions = no permission prompts at all
                //   --output-format stream-json  = newline-delimited JSON events
                //   --verbose                   = include tool-use events in output
                //   --include-partial-messages  = stream text chunks as they arrive
                //   --model MODEL               = chosen model
                //   --system-prompt-file FILE   = replace default system prompt
                //   (optional) --resume SESSION = continue previous conversation
                var argsBuilder = new StringBuilder();
                argsBuilder.Append(
                    $"-p --dangerously-skip-permissions" +
                    $" --output-format stream-json --verbose --include-partial-messages" +
                    $" --model {Settings.Model}" +
                    $" --system-prompt-file \"{sysFile}\"");

                if (continueSession && !string.IsNullOrWhiteSpace(LastSessionId))
                    argsBuilder.Append($" --resume \"{LastSessionId}\"");

                string args = argsBuilder.ToString();

                ProcessStartInfo psi;
                // On Windows, claude may be installed as claude.cmd — must invoke via cmd.exe
                if (claudeExe.EndsWith(".cmd", StringComparison.OrdinalIgnoreCase) ||
                    claudeExe.EndsWith(".bat", StringComparison.OrdinalIgnoreCase))
                {
                    psi = new ProcessStartInfo
                    {
                        FileName  = "cmd.exe",
                        Arguments = $"/c \"{claudeExe}\" {args}",
                        RedirectStandardInput  = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError  = true,
                        UseShellExecute  = false,
                        CreateNoWindow   = true,
                        StandardInputEncoding  = Encoding.UTF8,
                        StandardOutputEncoding = Encoding.UTF8,
                    };
                }
                else
                {
                    psi = new ProcessStartInfo
                    {
                        FileName  = claudeExe,
                        Arguments = args,
                        RedirectStandardInput  = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError  = true,
                        UseShellExecute  = false,
                        CreateNoWindow   = true,
                        StandardInputEncoding  = Encoding.UTF8,
                        StandardOutputEncoding = Encoding.UTF8,
                    };
                }

                using var proc = new Process { StartInfo = psi };
                proc.Start();

                // Write message to stdin asynchronously (avoids deadlock)
                var stdinTask = Task.Run(async () =>
                {
                    try
                    {
                        await proc.StandardInput.WriteAsync(userMessage);
                        proc.StandardInput.Close();
                    }
                    catch { /* process may have exited */ }
                }, ct);

                // Drain stderr asynchronously
                var stderrTask = proc.StandardError.ReadToEndAsync();

                // --- Parse stdout stream-json events ---
                var fullText  = new StringBuilder();
                string? currentTool = null;

                while (true)
                {
                    ct.ThrowIfCancellationRequested();

                    string? line = await proc.StandardOutput.ReadLineAsync();
                    if (line == null) break;
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    try
                    {
                        using var doc  = JsonDocument.Parse(line);
                        var root       = doc.RootElement;
                        if (!root.TryGetProperty("type", out var typeEl)) continue;
                        string evType  = typeEl.GetString() ?? "";

                        switch (evType)
                        {
                            // ── session initialised ────────────────────────
                            case "system":
                                if (root.TryGetProperty("subtype", out var sub) &&
                                    sub.GetString() == "init" &&
                                    root.TryGetProperty("session_id", out var sid))
                                    LastSessionId = sid.GetString();
                                break;

                            // ── streaming partial events ───────────────────
                            case "stream_event":
                                ParseStreamEvent(root, fullText, ref currentTool);
                                break;

                            // ── complete assistant turn (no partial msgs) ──
                            case "assistant":
                                // When --include-partial-messages is used these arrive
                                // after the streaming events as a summary — ignore.
                                break;

                            // ── tool_use (verbose, without partial msgs) ───
                            case "tool_use":
                                if (root.TryGetProperty("name", out var tn))
                                    OnToolActivity?.Invoke($"🔧 {tn.GetString()}...");
                                break;

                            // ── final result ───────────────────────────────
                            case "result":
                                if (root.TryGetProperty("result", out var res))
                                {
                                    string finalText = res.GetString() ?? "";
                                    // If we didn't receive streaming text, show the result now
                                    if (fullText.Length == 0 && !string.IsNullOrEmpty(finalText))
                                    {
                                        fullText.Append(finalText);
                                        OnChunk?.Invoke(finalText);
                                    }
                                }
                                break;
                        }
                    }
                    catch (JsonException) { /* skip malformed lines */ }
                }

                await proc.WaitForExitAsync(ct);
                await stdinTask;
                string err = await stderrTask;

                if (fullText.Length == 0 && !string.IsNullOrWhiteSpace(err))
                {
                    // Strip ANSI colour codes that claude sometimes emits on stderr
                    string cleaned = System.Text.RegularExpressions.Regex.Replace(err, @"\x1B\[[^@-~]*[@-~]", "");
                    OnError?.Invoke(cleaned.Trim());
                    return;
                }

                OnDone?.Invoke(fullText.ToString());
            }
            finally
            {
                try { File.Delete(sysFile); } catch { }
            }
        }

        // Parses one stream_event line (Anthropic raw API streaming event)
        private void ParseStreamEvent(JsonElement root, StringBuilder fullText, ref string? currentTool)
        {
            if (!root.TryGetProperty("event", out var ev)) return;
            if (!ev.TryGetProperty("type", out var evTypeEl)) return;
            string evType = evTypeEl.GetString() ?? "";

            switch (evType)
            {
                // Text / tool_use block starting
                case "content_block_start":
                    if (ev.TryGetProperty("content_block", out var cb))
                    {
                        if (cb.TryGetProperty("type", out var cbType) &&
                            cbType.GetString() == "tool_use" &&
                            cb.TryGetProperty("name", out var toolName))
                        {
                            currentTool = toolName.GetString() ?? "tool";
                            OnToolActivity?.Invoke($"🔧 {currentTool}...");
                        }
                    }
                    break;

                // Streaming delta: text or tool input
                case "content_block_delta":
                    if (ev.TryGetProperty("delta", out var delta))
                    {
                        if (delta.TryGetProperty("type", out var dType))
                        {
                            string dt = dType.GetString() ?? "";
                            if (dt == "text_delta" && delta.TryGetProperty("text", out var txt))
                            {
                                string chunk = txt.GetString() ?? "";
                                if (!string.IsNullOrEmpty(chunk))
                                {
                                    fullText.Append(chunk);
                                    OnChunk?.Invoke(chunk);
                                }
                            }
                        }
                    }
                    break;

                // Block finished
                case "content_block_stop":
                    if (currentTool != null)
                    {
                        OnToolActivity?.Invoke($"✓ {currentTool} done");
                        currentTool = null;
                    }
                    break;
            }
        }

        // ── API Key mode (direct HTTP streaming) ──────────────────────────────

        private async Task SendViaApiAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            var body = new
            {
                model      = Settings.Model,
                max_tokens = 8096,
                system     = systemPrompt,
                messages   = new[] { new { role = "user", content = userMessage } },
                stream     = true
            };

            var req = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages")
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };
            req.Headers.Add("x-api-key", Settings.ApiKey);
            req.Headers.Add("anthropic-version", "2023-06-01");

            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            if (!resp.IsSuccessStatusCode)
            {
                string body2 = await resp.Content.ReadAsStringAsync(ct);
                OnError?.Invoke($"API error {(int)resp.StatusCode}: {body2}");
                return;
            }

            using var stream = await resp.Content.ReadAsStreamAsync(ct);
            using var reader = new StreamReader(stream);

            var full = new StringBuilder();
            while (!reader.EndOfStream)
            {
                string? line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data:")) continue;
                string json = line[5..].Trim();
                if (json == "[DONE]") break;
                try
                {
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;
                    if (root.TryGetProperty("type", out var t) &&
                        t.GetString() == "content_block_delta" &&
                        root.TryGetProperty("delta", out var d) &&
                        d.TryGetProperty("text", out var txt))
                    {
                        string chunk = txt.GetString() ?? "";
                        full.Append(chunk);
                        OnChunk?.Invoke(chunk);
                    }
                }
                catch { /* ignore malformed SSE lines */ }
            }

            OnDone?.Invoke(full.ToString());
        }

        // ── Find claude executable ─────────────────────────────────────────────

        private static string? FindClaudeExe()
        {
            // Search PATH first
            string path = Environment.GetEnvironmentVariable("PATH") ?? "";
            foreach (string dir in path.Split(Path.PathSeparator))
            {
                foreach (string name in new[] { "claude.exe", "claude.cmd", "claude" })
                {
                    string full = Path.Combine(dir.Trim(), name);
                    if (File.Exists(full)) return full;
                }
            }

            // Common Windows / macOS install locations
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string[] candidates =
            {
                Path.Combine(home, "AppData", "Roaming", "npm", "claude.cmd"),
                Path.Combine(home, "AppData", "Roaming", "npm", "claude.exe"),
                Path.Combine(home, "AppData", "Local", "Programs", "claude", "claude.exe"),
                Path.Combine(home, ".claude", "local", "claude.exe"),
                @"C:\Program Files\Claude\claude.exe",
                "/usr/local/bin/claude",
                "/usr/bin/claude",
                Path.Combine(home, ".nvm", "current", "bin", "claude"),
            };
            foreach (string c in candidates) if (File.Exists(c)) return c;
            return null;
        }
    }
}
