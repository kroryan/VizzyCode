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
        private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromMinutes(10) };

        public AiSettings Settings { get; set; } = new();
        public bool IsOpenCode { get; set; }
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
                if (Settings.OpenAiMode == AccessMode.Cli || string.IsNullOrWhiteSpace(Settings.OpenAiApiKey))
                    await SendViaCodexCliAsync(userMessage, systemPrompt, ct);
                else
                    await SendViaApiAsync(userMessage, systemPrompt, ct);
            }
            catch (OperationCanceledException)
            {
                OnError?.Invoke("Codex request cancelled.");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"OpenAI error: {ex.Message}");
            }
        }

        public void ApproveTool(string callId)
        {
            OnToolActivity?.Invoke("Codex approvals are handled in the native interactive CLI. Use Open CLI for consent prompts.");
        }

        public void RejectTool(string callId)
        {
            OnToolActivity?.Invoke("Codex approvals are handled in the native interactive CLI. Use Open CLI for consent prompts.");
        }

        public void SwitchAgent(string agentName)
        {
            OnToolActivity?.Invoke("Codex CLI does not expose Claude/OpenCode-style named agents through this wrapper.");
        }

        public void InvokeSkill(string skillName, string[] arguments)
        {
            OnError?.Invoke("Codex CLI does not expose Claude/OpenCode skill loading through this VizzyCode headless wrapper.");
        }

        private async Task SendViaCodexCliAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            string? exe = CliIntegration.FindExecutable(CliKind.Codex);
            if (exe == null)
            {
                OnError?.Invoke(CliIntegration.GetMissingExecutableMessage(CliKind.Codex));
                return;
            }

            string cliWorkingDirectory = CliIntegration.GetCliWorkingDirectory(WorkingDirectory, WorkspaceDirectory);
            string prompt = CliIntegration.BuildWorkspacePrompt(cliWorkingDirectory);

            var psi = CliIntegration.CreateProcessStartInfo(exe, cliWorkingDirectory);

            var args = new StringBuilder();
            args.Append("exec --json --full-auto --skip-git-repo-check");
            if (!string.IsNullOrWhiteSpace(Settings.OpenAiModel))
                args.Append(" -m ").Append(CliIntegration.QuoteForCmd(Settings.OpenAiModel));
            args.Append(' ').Append(CliIntegration.QuoteForCmd(prompt));
            CliIntegration.SetCommandArguments(psi, exe, args.ToString());

            var fullText = new StringBuilder();
            var result = await CliProcessRunner.RunAsync(
                psi,
                TimeSpan.FromSeconds(Settings.CliTimeoutSeconds),
                line =>
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        ParseCodexEvent(line, fullText);
                },
                line =>
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        OnToolActivity?.Invoke(line.Trim());
                },
                ct);
            DebugLog.Write($"CODEX stdout={result.StdOut}");
            DebugLog.Write($"CODEX stderr={result.StdErr}");

            if (result.TimedOut)
            {
                OnError?.Invoke("Codex CLI timed out waiting for output. The process was stopped.");
                return;
            }

            if (result.ExitCode != 0 && fullText.Length == 0)
                OnError?.Invoke($"Codex CLI exited with code {result.ExitCode}.");

            OnDone?.Invoke(fullText.ToString());
        }

        private void ParseCodexEvent(string line, StringBuilder fullText)
        {
            try
            {
                using var doc = JsonDocument.Parse(line);
                JsonElement root = doc.RootElement;
                string type = root.TryGetProperty("type", out var typeProp) ? typeProp.GetString() ?? string.Empty : string.Empty;

                if (type == "item.completed" && root.TryGetProperty("item", out var item))
                {
                    string itemType = item.TryGetProperty("type", out var itemTypeProp) ? itemTypeProp.GetString() ?? string.Empty : string.Empty;
                    if (itemType == "agent_message" && item.TryGetProperty("text", out var text))
                    {
                        AppendText(text.GetString(), fullText);
                        return;
                    }
                }

                if (type.Contains("exec_command", StringComparison.OrdinalIgnoreCase) ||
                    type.Contains("mcp", StringComparison.OrdinalIgnoreCase) ||
                    type.Contains("patch", StringComparison.OrdinalIgnoreCase) ||
                    type.Contains("sandbox", StringComparison.OrdinalIgnoreCase))
                {
                    OnToolActivity?.Invoke(line);
                    return;
                }

                if (root.TryGetProperty("text", out var fallbackText) && fallbackText.ValueKind == JsonValueKind.String)
                {
                    AppendText(fallbackText.GetString(), fullText);
                    return;
                }

                if (type == "error")
                {
                    OnError?.Invoke(line);
                }
            }
            catch (JsonException)
            {
                AppendText(line, fullText, appendNewLine: true);
            }
        }

        private void AppendText(string? text, StringBuilder fullText, bool appendNewLine = false)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            fullText.Append(text);
            if (appendNewLine)
                fullText.AppendLine();
            OnChunk?.Invoke(text);
        }

        private async Task SendViaApiAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            var requestBody = new
            {
                model = Settings.OpenAiModel,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userMessage }
                },
                temperature = Settings.Temperature,
                max_tokens = Settings.MaxTokens,
                stream = true
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Authorization", $"Bearer {Settings.OpenAiApiKey}");

            using var response = await Http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
            if (!response.IsSuccessStatusCode)
            {
                OnError?.Invoke($"OpenAI API error: {await response.Content.ReadAsStringAsync(ct)}");
                return;
            }

            using var reader = new StreamReader(await response.Content.ReadAsStreamAsync(ct));
            var fullText = new StringBuilder();

            while (!reader.EndOfStream)
            {
                string? line = await reader.ReadLineAsync(ct);
                if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data:", StringComparison.Ordinal))
                    continue;

                string json = line[5..].Trim();
                if (json == "[DONE]")
                    break;

                try
                {
                    using var doc = JsonDocument.Parse(json);
                    if (!doc.RootElement.TryGetProperty("choices", out var choices))
                        continue;

                    foreach (JsonElement choice in choices.EnumerateArray())
                    {
                        if (!choice.TryGetProperty("delta", out var delta))
                            continue;

                        if (delta.TryGetProperty("content", out var content) && content.ValueKind == JsonValueKind.String)
                        {
                            string chunk = content.GetString() ?? string.Empty;
                            fullText.Append(chunk);
                            OnChunk?.Invoke(chunk);
                        }
                    }
                }
                catch (JsonException)
                {
                }
            }

            OnDone?.Invoke(fullText.ToString());
        }
    }
}
