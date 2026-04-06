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
    public class OpenAiClient : IAiClient
    {
        public AiSettings Settings { get; set; } = new();
        public bool IsOpenCode { get; set; } = false;

        public event Action<string>? OnChunk;
        public event Action<string>? OnDone;
        public event Action<string>? OnError;
        public event Action<string>? OnToolActivity;

        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromMinutes(10) };

        public async Task SendAsync(string userMessage, string systemPrompt, CancellationToken ct = default)
        {
            try
            {
                if (!IsOpenCode && Settings.OpenAiMode == AccessMode.Cli)
                    await SendViaCliAsync(userMessage, systemPrompt, ct);
                else
                    await SendViaApiAsync(userMessage, systemPrompt, ct);
            }
            catch (Exception ex) { OnError?.Invoke(ex.Message); }
        }

        private async Task SendViaCliAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            string? exe = FindExe("codex");
            if (exe == null) { OnError?.Invoke("Codex CLI not found. Run 'npm install -g @openai/codex' and 'codex login'"); return; }

            string fullPrompt = systemPrompt + "\n\n" + userMessage;
            var args = $"exec \"{fullPrompt.Replace("\"", "\\\"")}\" --full-auto --model {Settings.OpenAiModel} --json";

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
                    // OpenAI CLI --json output usually emits JSONL
                    using var doc = JsonDocument.Parse(line);
                    if (doc.RootElement.TryGetProperty("content", out var txt)) {
                        string s = txt.GetString() ?? "";
                        fullText.Append(s); OnChunk?.Invoke(s);
                    }
                } catch { 
                    // Fallback to raw text if not JSON
                    fullText.AppendLine(line); OnChunk?.Invoke(line + "\n");
                }
            }
            await proc.WaitForExitAsync(ct);
            OnDone?.Invoke(fullText.ToString());
        }

        private async Task SendViaApiAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            string baseUrl = IsOpenCode ? Settings.OpenCodeBaseUrl : "https://api.openai.com/v1";
            string apiKey = IsOpenCode ? Settings.OpenCodeApiKey : Settings.OpenAiApiKey;
            string model = IsOpenCode ? Settings.OpenCodeModel : Settings.OpenAiModel;

            if (!IsOpenCode && string.IsNullOrWhiteSpace(apiKey)) { OnError?.Invoke("OpenAI API Key is missing."); return; }

            var body = new {
                model = model,
                messages = new[] { new { role = "system", content = systemPrompt }, new { role = "user", content = userMessage } },
                temperature = Settings.Temperature,
                max_tokens = Settings.MaxTokens,
                stream = true
            };

            var req = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/chat/completions") {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };
            if (!string.IsNullOrWhiteSpace(apiKey)) req.Headers.Add("Authorization", $"Bearer {apiKey}");

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
