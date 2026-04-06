using System;
using System.Collections.Generic;
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
        private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromMinutes(10) };

        public AiSettings Settings { get; set; } = new();
        public string WorkingDirectory { get; set; } = Environment.CurrentDirectory;
        public string WorkspaceDirectory { get; set; } = Environment.CurrentDirectory;

        public event Action<string>? OnChunk;
        public event Action<string>? OnDone;
        public event Action<string>? OnError;
        public event Action<string>? OnToolActivity;
        public event Action<ToolCallRequest>? OnApprovalRequired;
        public event Action<ToolCallResponse>? OnToolCall;

        public async Task SendAsync(string userMessage, string systemPrompt, CancellationToken ct = default)
        {
            try
            {
                if (Settings.GeminiMode == AccessMode.Cli || string.IsNullOrWhiteSpace(Settings.GeminiApiKey))
                    await SendViaCliAsync(userMessage, systemPrompt, ct);
                else
                    await SendViaApiAsync(userMessage, systemPrompt, ct);
            }
            catch (OperationCanceledException)
            {
                OnError?.Invoke("Gemini request cancelled.");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Gemini error: {ex.Message}");
            }
        }

        public void ApproveTool(string callId)
        {
            OnToolActivity?.Invoke("Gemini approvals are handled in the native interactive CLI. Use Open CLI for consent prompts.");
        }

        public void RejectTool(string callId)
        {
            OnToolActivity?.Invoke("Gemini approvals are handled in the native interactive CLI. Use Open CLI for consent prompts.");
        }

        public void SwitchAgent(string agentName)
        {
        }

        public void InvokeSkill(string skillName, string[] arguments)
        {
            OnError?.Invoke("Gemini skills are managed by the Gemini CLI itself. Use Open CLI for `/skills` and `/memory` workflows.");
        }

        private async Task SendViaCliAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            string? exe = CliIntegration.FindExecutable(CliKind.Gemini);
            if (exe == null)
            {
                OnError?.Invoke(CliIntegration.GetMissingExecutableMessage(CliKind.Gemini));
                return;
            }

            string prompt = CliIntegration.BuildWorkspacePrompt(WorkspaceDirectory);

            var psi = CliIntegration.CreateProcessStartInfo(exe, WorkingDirectory);
            var args = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(Settings.GeminiModel))
                args.Append("--model ").Append(CliIntegration.QuoteForCmd(Settings.GeminiModel)).Append(' ');
            args.Append("-p ").Append(CliIntegration.QuoteForCmd(prompt));
            CliIntegration.SetCommandArguments(psi, exe, args.ToString());

            var result = await CliProcessRunner.RunAsync(psi, TimeSpan.FromSeconds(Settings.CliTimeoutSeconds), ct);
            DebugLog.Write($"GEMINI stdout={result.StdOut}");
            DebugLog.Write($"GEMINI stderr={result.StdErr}");
            if (!string.IsNullOrWhiteSpace(result.StdErr))
                OnToolActivity?.Invoke(result.StdErr.Trim());

            var fullText = new StringBuilder();
            foreach (string line in result.StdOut.Replace("\r\n", "\n").Split('\n'))
            {
                if (!string.IsNullOrWhiteSpace(line))
                    ParseGeminiEvent(line, fullText);
            }

            if (result.TimedOut)
            {
                OnError?.Invoke("Gemini CLI timed out waiting for output. The process was stopped.");
                return;
            }

            if (result.ExitCode != 0 && fullText.Length == 0)
                OnError?.Invoke($"Gemini CLI exited with code {result.ExitCode}.");

            OnDone?.Invoke(fullText.ToString());
        }

        private void ParseGeminiEvent(string line, StringBuilder fullText)
        {
            try
            {
                using var doc = JsonDocument.Parse(line);
                bool appended = false;
                foreach (string chunk in ExtractTextFragments(doc.RootElement))
                {
                    if (string.IsNullOrWhiteSpace(chunk))
                        continue;

                    appended = true;
                    fullText.Append(chunk);
                    OnChunk?.Invoke(chunk);
                }

                if (!appended &&
                    doc.RootElement.TryGetProperty("type", out var typeProp) &&
                    typeProp.ValueKind == JsonValueKind.String &&
                    typeProp.GetString()!.Contains("tool", StringComparison.OrdinalIgnoreCase))
                {
                    OnToolActivity?.Invoke(line);
                }
            }
            catch (JsonException)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    fullText.AppendLine(line);
                    OnChunk?.Invoke(line + Environment.NewLine);
                }
            }
        }

        private IEnumerable<string> ExtractTextFragments(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    if (property.Value.ValueKind == JsonValueKind.String &&
                        property.NameEquals("text"))
                    {
                        string? value = property.Value.GetString();
                        if (!string.IsNullOrWhiteSpace(value))
                            yield return value;
                    }

                    foreach (string nested in ExtractTextFragments(property.Value))
                        yield return nested;
                }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement item in element.EnumerateArray())
                {
                    foreach (string nested in ExtractTextFragments(item))
                        yield return nested;
                }
            }
        }

        private async Task SendViaApiAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = CliIntegration.BuildCombinedPrompt(systemPrompt, userMessage) }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = Settings.Temperature,
                    maxOutputTokens = Settings.MaxTokens
                }
            };

            string url = $"https://generativelanguage.googleapis.com/v1beta/models/{Settings.GeminiModel}:streamGenerateContent?key={Settings.GeminiApiKey}";
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            using var response = await Http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
            if (!response.IsSuccessStatusCode)
            {
                OnError?.Invoke($"Gemini API error: {await response.Content.ReadAsStringAsync(ct)}");
                return;
            }

            using var reader = new StreamReader(await response.Content.ReadAsStreamAsync(ct));
            var fullText = new StringBuilder();

            while (!reader.EndOfStream)
            {
                string? line = await reader.ReadLineAsync(ct);
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    using var doc = JsonDocument.Parse(line);
                    foreach (string chunk in ExtractApiText(doc.RootElement))
                    {
                        fullText.Append(chunk);
                        OnChunk?.Invoke(chunk);
                    }
                }
                catch (JsonException)
                {
                }
            }

            OnDone?.Invoke(fullText.ToString());
        }

        private IEnumerable<string> ExtractApiText(JsonElement root)
        {
            if (!root.TryGetProperty("candidates", out var candidates) || candidates.ValueKind != JsonValueKind.Array)
                yield break;

            foreach (JsonElement candidate in candidates.EnumerateArray())
            {
                if (!candidate.TryGetProperty("content", out var content))
                    continue;

                if (!content.TryGetProperty("parts", out var parts) || parts.ValueKind != JsonValueKind.Array)
                    continue;

                foreach (JsonElement part in parts.EnumerateArray())
                {
                    if (part.TryGetProperty("text", out var text) && text.ValueKind == JsonValueKind.String)
                    {
                        string chunk = text.GetString() ?? string.Empty;
                        if (!string.IsNullOrEmpty(chunk))
                            yield return chunk;
                    }
                }
            }
        }
    }
}
