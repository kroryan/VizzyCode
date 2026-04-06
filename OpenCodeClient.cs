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
    public class OpenCodeClient : IAiClient
    {
        public AiSettings Settings { get; set; } = new();

        public event Action<string>? OnChunk;
        public event Action<string>? OnDone;
        public event Action<string>? OnError;
        public event Action<string>? OnToolActivity;

        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromMinutes(10) };

        public async Task SendAsync(string userMessage, string systemPrompt, CancellationToken ct = default)
        {
            try
            {
                if (Settings.OpenCodeMode == AccessMode.ApiKey)
                    await SendViaApiAsync(userMessage, systemPrompt, ct);
                else
                    await SendViaCliAsync(userMessage, systemPrompt, ct);
            }
            catch (Exception ex) { OnError?.Invoke(ex.Message); }
        }

        private async Task SendViaCliAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            string? exe = FindExe("opencode");
            if (exe == null) { OnError?.Invoke("OpenCode CLI not found. Run 'npm install -g opencode-ai'"); return; }

            string fullPrompt = systemPrompt + "\n\n" + userMessage;
            var args = $"-p \"{fullPrompt.Replace("\"", "\\\"")}\" --model {Settings.OpenCodeModel} -f json -q";

            await RunCliAsync(exe, args, ct);
        }

        private async Task RunCliAsync(string exe, string args, CancellationToken ct)
        {
            var psi = new ProcessStartInfo {
                FileName = exe.EndsWith(".cmd") || exe.EndsWith(".bat") ? "cmd.exe" : exe,
                Arguments = exe.EndsWith(".cmd") || exe.EndsWith(".bat") ? $"/c \"{exe}\" {args}" : args,
                RedirectStandardOutput = true, RedirectStandardError = true,
                UseShellExecute = false, CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8
            };

            using var proc = Process.Start(psi);
            if (proc == null) return;

            var fullText = new StringBuilder();
            while (!proc.StandardOutput.EndOfStream)
            {
                string? line = await proc.StandardOutput.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;
                try
                {
                    using var doc = JsonDocument.Parse(line);
                    // opencode JSON format often has a 'content' field in non-interactive mode
                    if (doc.RootElement.TryGetProperty("content", out var txt)) {
                        string s = txt.GetString() ?? "";
                        fullText.Append(s); OnChunk?.Invoke(s);
                    }
                } catch { 
                    fullText.AppendLine(line); OnChunk?.Invoke(line + "\n");
                }
            }
            await proc.WaitForExitAsync(ct);
            OnDone?.Invoke(fullText.ToString());
        }

        private async Task SendViaApiAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            // OpenCode API is usually OpenAI-compatible (Ollama, etc.)
            var body = new {
                model = Settings.OpenCodeModel,
                messages = new[] { new { role = "system", content = systemPrompt }, new { role = "user", content = userMessage } },
                temperature = Settings.Temperature,
                max_tokens = Settings.MaxTokens,
                stream = true
            };

            var req = new HttpRequestMessage(HttpMethod.Post, $"{Settings.OpenCodeBaseUrl}/chat/completions") {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };
            if (!string.IsNullOrWhiteSpace(Settings.OpenCodeApiKey)) req.Headers.Add("Authorization", $"Bearer {Settings.OpenCodeApiKey}");

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
                    var delta = doc.RootElement.GetProperty("choices")[0].GetProperty("delta");
                    if (delta.TryGetProperty("content", out var txt)) { string s = txt.GetString() ?? ""; full.Append(s); OnChunk?.Invoke(s); }
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
