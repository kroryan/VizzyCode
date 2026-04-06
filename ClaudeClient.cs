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
    public class ClaudeClient : IAiClient
    {
        public AiSettings Settings { get; set; } = new();

        public event Action<string>? OnChunk;
        public event Action<string>? OnDone;
        public event Action<string>? OnError;
        public event Action<string>? OnToolActivity;

        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromMinutes(10) };
        public string? LastSessionId { get; private set; }

        public async Task SendAsync(string userMessage, string systemPrompt, CancellationToken ct = default)
        {
            try
            {
                if (Settings.ClaudeMode == AccessMode.ApiKey && !string.IsNullOrWhiteSpace(Settings.ClaudeApiKey))
                    await SendViaApiAsync(userMessage, systemPrompt, ct);
                else
                    await SendViaCliAsync(userMessage, systemPrompt, ct);
            }
            catch (Exception ex) { OnError?.Invoke(ex.Message); }
        }

        private async Task SendViaCliAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            string? exe = FindExe("claude");
            if (exe == null) { OnError?.Invoke("Claude CLI not found. Run 'npm install -g @anthropic-ai/claude-code' and 'claude login'"); return; }

            string sysFile = Path.GetTempFileName();
            await File.WriteAllTextAsync(sysFile, systemPrompt, ct);
            
            var args = $"-p --dangerously-skip-permissions --output-format stream-json --verbose --include-partial-messages --model {Settings.ClaudeModel} --system-prompt-file \"{sysFile}\"";
            
            await RunCliAsync(exe, args, userMessage, ct);
            try { File.Delete(sysFile); } catch { }
        }

        private async Task RunCliAsync(string exe, string args, string input, CancellationToken ct)
        {
            var psi = new ProcessStartInfo {
                FileName = exe.EndsWith(".cmd") || exe.EndsWith(".bat") ? "cmd.exe" : exe,
                Arguments = exe.EndsWith(".cmd") || exe.EndsWith(".bat") ? $"/c \"{exe}\" {args}" : args,
                RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true,
                UseShellExecute = false, CreateNoWindow = true,
                StandardInputEncoding = Encoding.UTF8, StandardOutputEncoding = Encoding.UTF8
            };

            using var proc = Process.Start(psi);
            if (proc == null) return;

            _ = Task.Run(async () => { try { await proc.StandardInput.WriteAsync(input); proc.StandardInput.Close(); } catch { } });

            var fullText = new StringBuilder();
            string? currentTool = null;

            while (!proc.StandardOutput.EndOfStream)
            {
                string? line = await proc.StandardOutput.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;
                try
                {
                    using var doc = JsonDocument.Parse(line);
                    var root = doc.RootElement;
                    string type = root.GetProperty("type").GetString() ?? "";
                    if (type == "stream_event") ParseStreamEvent(root, fullText, ref currentTool);
                    else if (type == "tool_use") OnToolActivity?.Invoke($"🔧 {root.GetProperty("name").GetString()}...");
                    else if (type == "result") {
                        string res = root.GetProperty("result").GetString() ?? "";
                        if (fullText.Length == 0) { fullText.Append(res); OnChunk?.Invoke(res); }
                    }
                } catch { }
            }
            await proc.WaitForExitAsync(ct);
            OnDone?.Invoke(fullText.ToString());
        }

        private void ParseStreamEvent(JsonElement root, StringBuilder fullText, ref string? currentTool)
        {
            var ev = root.GetProperty("event");
            string et = ev.GetProperty("type").GetString() ?? "";
            if (et == "content_block_delta") {
                var delta = ev.GetProperty("delta");
                if (delta.GetProperty("type").GetString() == "text_delta") {
                    string txt = delta.GetProperty("text").GetString() ?? "";
                    fullText.Append(txt); OnChunk?.Invoke(txt);
                }
            }
        }

        private async Task SendViaApiAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            var body = new { model = Settings.ClaudeModel, max_tokens = Settings.MaxTokens, system = systemPrompt, messages = new[] { new { role = "user", content = userMessage } }, stream = true };
            var req = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages") { Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json") };
            req.Headers.Add("x-api-key", Settings.ClaudeApiKey);
            req.Headers.Add("anthropic-version", "2023-06-01");
            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            if (!resp.IsSuccessStatusCode) { OnError?.Invoke(await resp.Content.ReadAsStringAsync()); return; }
            using var reader = new StreamReader(await resp.Content.ReadAsStreamAsync());
            var full = new StringBuilder();
            while (!reader.EndOfStream) {
                string? line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data:")) continue;
                string json = line[5..].Trim(); if (json == "[DONE]") break;
                try {
                    using var doc = JsonDocument.Parse(json);
                    var delta = doc.RootElement.GetProperty("delta");
                    if (delta.TryGetProperty("text", out var t)) { string s = t.GetString() ?? ""; full.Append(s); OnChunk?.Invoke(s); }
                } catch { }
            }
            OnDone?.Invoke(full.ToString());
        }

        private string? FindExe(string name)
        {
            string path = Environment.GetEnvironmentVariable("PATH") ?? "";
            foreach (string dir in path.Split(Path.PathSeparator)) {
                foreach (string ext in new[] { ".exe", ".cmd", ".bat", "" }) {
                    string full = Path.Combine(dir.Trim(), name + ext);
                    if (File.Exists(full)) return full;
                }
            }
            return null;
        }
    }
}
