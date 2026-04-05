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
        public ClaudeMode Mode { get; set; } = ClaudeMode.ClaudeCli;
        public string ApiKey { get; set; } = "";
        public string Model { get; set; } = "claude-sonnet-4-6";
    }

    /// <summary>
    /// Sends prompts to Claude using either:
    ///   - The installed `claude` CLI (uses your claude.ai subscription, no key needed)
    ///   - Direct Anthropic API via HTTP (needs API key in settings)
    /// </summary>
    public class ClaudeClient
    {
        public ClaudeSettings Settings { get; set; } = new();

        // Raised for each chunk of streamed text
        public event Action<string>? OnChunk;
        // Raised when the full response is ready
        public event Action<string>? OnDone;
        // Raised on error
        public event Action<string>? OnError;

        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromMinutes(5) };

        public async Task SendAsync(string userMessage, string systemPrompt, CancellationToken ct = default)
        {
            try
            {
                if (Settings.Mode == ClaudeMode.ApiKey && !string.IsNullOrWhiteSpace(Settings.ApiKey))
                    await SendViaApiAsync(userMessage, systemPrompt, ct);
                else
                    await SendViaCliAsync(userMessage, systemPrompt, ct);
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

        // ── Claude CLI mode (uses subscription) ───────────────────────────────

        private async Task SendViaCliAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            string claudeExe = FindClaudeExe();
            if (claudeExe == null)
            {
                OnError?.Invoke("'claude' CLI not found. Install Claude Code from https://claude.ai/code, or configure an API key in Settings.");
                return;
            }

            // Write system prompt to temp file to avoid shell escaping issues
            string sysFile = Path.GetTempFileName();
            string msgFile = Path.GetTempFileName();
            try
            {
                await File.WriteAllTextAsync(sysFile, systemPrompt, ct);
                await File.WriteAllTextAsync(msgFile, userMessage, ct);

                var psi = new ProcessStartInfo
                {
                    FileName = claudeExe,
                    Arguments = $"-p --model {Settings.Model} --system-prompt-file \"{sysFile}\" --output-format text \"{msgFile}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError  = true,
                    UseShellExecute = false,
                    CreateNoWindow  = true
                };

                // Fallback: if --system-prompt-file isn't supported, use inline
                // We'll detect this from stderr and retry
                using var proc = new Process { StartInfo = psi, EnableRaisingEvents = true };
                var sb = new StringBuilder();
                var errSb = new StringBuilder();

                proc.OutputDataReceived += (_, e) =>
                {
                    if (e.Data != null) { sb.AppendLine(e.Data); OnChunk?.Invoke(e.Data + "\n"); }
                };
                proc.ErrorDataReceived += (_, e) =>
                {
                    if (e.Data != null) errSb.AppendLine(e.Data);
                };

                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();

                await proc.WaitForExitAsync(ct);

                string err = errSb.ToString().Trim();
                string output = sb.ToString().Trim();

                if (proc.ExitCode != 0 && string.IsNullOrWhiteSpace(output))
                {
                    // Retry without --system-prompt-file (older CLI versions)
                    await SendViaCliLegacyAsync(userMessage, systemPrompt, claudeExe, ct);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(err) && string.IsNullOrWhiteSpace(output))
                {
                    OnError?.Invoke(err);
                    return;
                }

                OnDone?.Invoke(output);
            }
            finally
            {
                try { File.Delete(sysFile); File.Delete(msgFile); } catch { }
            }
        }

        private async Task SendViaCliLegacyAsync(string userMessage, string systemPrompt, string claudeExe, CancellationToken ct)
        {
            // Use --append-system-prompt and pass message via stdin
            var psi = new ProcessStartInfo
            {
                FileName = claudeExe,
                Arguments = $"-p --model {Settings.Model}",
                RedirectStandardInput  = true,
                RedirectStandardOutput = true,
                RedirectStandardError  = true,
                UseShellExecute = false,
                CreateNoWindow  = true
            };

            using var proc = new Process { StartInfo = psi };
            var sb    = new StringBuilder();
            var errSb = new StringBuilder();

            proc.OutputDataReceived += (_, e) =>
            {
                if (e.Data != null) { sb.AppendLine(e.Data); OnChunk?.Invoke(e.Data + "\n"); }
            };
            proc.ErrorDataReceived += (_, e) =>
            {
                if (e.Data != null) errSb.AppendLine(e.Data);
            };

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            // Send system context + message via stdin
            await proc.StandardInput.WriteLineAsync($"[SYSTEM CONTEXT]\n{systemPrompt}\n\n[USER]\n{userMessage}");
            proc.StandardInput.Close();

            await proc.WaitForExitAsync(ct);

            string output = sb.ToString().Trim();
            string err    = errSb.ToString().Trim();

            if (!string.IsNullOrWhiteSpace(err) && string.IsNullOrWhiteSpace(output))
                OnError?.Invoke(err);
            else
                OnDone?.Invoke(output);
        }

        // ── API Key mode ───────────────────────────────────────────────────────

        private async Task SendViaApiAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            var body = new
            {
                model = Settings.Model,
                max_tokens = 8096,
                system = systemPrompt,
                messages = new[] { new { role = "user", content = userMessage } },
                stream = true
            };

            var req = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages")
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };
            req.Headers.Add("x-api-key", Settings.ApiKey);
            req.Headers.Add("anthropic-version", "2023-06-01");

            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            resp.EnsureSuccessStatusCode();

            using var stream = await resp.Content.ReadAsStreamAsync(ct);
            using var reader = new StreamReader(stream);

            var full = new StringBuilder();
            while (!reader.EndOfStream)
            {
                string? line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data:")) continue;
                string json = line.Substring(5).Trim();
                if (json == "[DONE]") break;
                try
                {
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;
                    if (root.TryGetProperty("type", out var type) && type.GetString() == "content_block_delta")
                    {
                        if (root.TryGetProperty("delta", out var delta) &&
                            delta.TryGetProperty("text", out var text))
                        {
                            string chunk = text.GetString() ?? "";
                            full.Append(chunk);
                            OnChunk?.Invoke(chunk);
                        }
                    }
                }
                catch { /* ignore malformed lines */ }
            }

            OnDone?.Invoke(full.ToString());
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        private static string? FindClaudeExe()
        {
            // Try PATH first
            foreach (string dir in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(Path.PathSeparator))
            {
                foreach (string name in new[] { "claude.exe", "claude" })
                {
                    string full = Path.Combine(dir.Trim(), name);
                    if (File.Exists(full)) return full;
                }
            }
            // Common install locations
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var candidates = new[]
            {
                Path.Combine(home, "AppData", "Roaming", "npm", "claude.cmd"),
                Path.Combine(home, "AppData", "Roaming", "npm", "claude.exe"),
                Path.Combine(home, ".claude", "local", "claude.exe"),
                @"C:\Program Files\Claude\claude.exe",
                "/usr/local/bin/claude",
                "/usr/bin/claude",
            };
            foreach (string c in candidates) if (File.Exists(c)) return c;
            return null;
        }
    }
}
