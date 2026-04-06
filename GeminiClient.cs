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
    public class GeminiClient : IAiClient
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
                if (Settings.GeminiMode == AccessMode.ApiKey && !string.IsNullOrWhiteSpace(Settings.GeminiApiKey))
                    await SendViaApiAsync(userMessage, systemPrompt, ct);
                else
                    await SendViaCliAsync(userMessage, systemPrompt, ct);
            }
            catch (Exception ex) { OnError?.Invoke(ex.Message); }
        }

        private async Task SendViaCliAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            string? exe = FindExe("gemini");
            if (exe == null) { OnError?.Invoke("Gemini CLI not found. Run 'npm install -g @google/gemini-cli' and 'gemini login'"); return; }

            // Concatenate system prompt and user message as Gemini CLI doesn't have a separate system prompt flag in many versions
            string fullPrompt = systemPrompt + "\n\n" + userMessage;
            var args = $"-p \"{fullPrompt.Replace("\"", "\\\"")}\" --approval-mode=yolo --model {Settings.GeminiModel}";

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
                if (line == null) continue;
                fullText.AppendLine(line);
                OnChunk?.Invoke(line + "\n");
            }
            await proc.WaitForExitAsync(ct);
            OnDone?.Invoke(fullText.ToString());
        }

        private async Task SendViaApiAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{Settings.GeminiModel}:streamGenerateContent?key={Settings.GeminiApiKey}";
            var body = new {
                contents = new[] { new { role = "user", parts = new[] { new { text = userMessage } } } },
                system_instruction = new { parts = new[] { new { text = systemPrompt } } },
                generationConfig = new { maxOutputTokens = Settings.MaxTokens, temperature = Settings.Temperature }
            };
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json") };
            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            if (!resp.IsSuccessStatusCode) { OnError?.Invoke(await resp.Content.ReadAsStringAsync()); return; }
            using var reader = new StreamReader(await resp.Content.ReadAsStreamAsync());
            var full = new StringBuilder();
            while (!reader.EndOfStream) {
                string? line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var matches = System.Text.RegularExpressions.Regex.Matches(line, @"""text"":\s*""([^""]*)""");
                foreach (System.Text.RegularExpressions.Match match in matches) {
                    string chunk = System.Text.RegularExpressions.Regex.Unescape(match.Groups[1].Value);
                    full.Append(chunk); OnChunk?.Invoke(chunk);
                }
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
