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
    public class OpenCodeClient : IAiClient
    {
        private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromMinutes(10) };
        private readonly SkillManager _skillManager = new();

        public AiSettings Settings { get; set; } = new();
        public string WorkingDirectory { get; set; } = Environment.CurrentDirectory;

        public event Action<string>? OnChunk;
        public event Action<string>? OnDone;
        public event Action<string>? OnError;
        public event Action<string>? OnToolActivity;
        public event Action<ToolCallRequest>? OnApprovalRequired;
        public event Action<ToolCallResponse>? OnToolCall;

        private string _currentAgent = "build";

        public async Task SendAsync(string userMessage, string systemPrompt, CancellationToken ct = default)
        {
            try
            {
                userMessage = ProcessSlashCommands(userMessage);
                if (string.IsNullOrWhiteSpace(userMessage))
                    return;

                if (Settings.OpenCodeMode == AccessMode.ApiKey && !string.IsNullOrWhiteSpace(Settings.OpenCodeBaseUrl))
                    await SendViaApiAsync(userMessage, systemPrompt, ct);
                else
                    await SendViaCliAsync(userMessage, systemPrompt, ct);
            }
            catch (OperationCanceledException)
            {
                OnError?.Invoke("OpenCode request cancelled.");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"OpenCode error: {ex.Message}");
            }
        }

        public void ApproveTool(string callId)
        {
            OnToolActivity?.Invoke("OpenCode approvals are handled in the native interactive CLI. Use Open CLI for consent prompts.");
        }

        public void RejectTool(string callId)
        {
            OnToolActivity?.Invoke("OpenCode approvals are handled in the native interactive CLI. Use Open CLI for consent prompts.");
        }

        public void SwitchAgent(string agentName)
        {
            if (string.IsNullOrWhiteSpace(agentName))
                return;

            agentName = agentName.Trim().ToLowerInvariant();
            if (agentName is "build" or "plan" or "general" or "explore")
                _currentAgent = agentName;
            else
                OnError?.Invoke("Unknown OpenCode agent. Valid values: build, plan, general, explore.");
        }

        public void InvokeSkill(string skillName, string[] arguments)
        {
            var skill = _skillManager.GetSkill(skillName);
            if (skill == null)
            {
                OnError?.Invoke($"Skill not found: {skillName}");
                return;
            }

            OnDone?.Invoke(_skillManager.GetSkillContent(skillName, arguments));
        }

        private string ProcessSlashCommands(string message)
        {
            if (!message.StartsWith("/"))
                return ReplaceSkillMentions(message);

            var parts = message[1..].Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            string command = parts.Length > 0 ? parts[0].Trim().ToLowerInvariant() : string.Empty;
            string args = parts.Length > 1 ? parts[1].Trim() : string.Empty;

            switch (command)
            {
                case "help":
                    OnDone?.Invoke("OpenCode slash commands and agent management are available in the native CLI. Use Open CLI for the full experience.");
                    return string.Empty;
                case "agent":
                    SwitchAgent(args);
                    OnDone?.Invoke($"OpenCode agent set to `{_currentAgent}`.");
                    return string.Empty;
                case "skill":
                    InvokeSkill(args, Array.Empty<string>());
                    return string.Empty;
                default:
                    return message;
            }
        }

        private string ReplaceSkillMentions(string message)
        {
            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(message, @"@(\w+)"))
            {
                string skillName = match.Groups[1].Value;
                if (_skillManager.GetSkill(skillName) != null)
                    message = message.Replace($"@{skillName}", _skillManager.GetSkillContent(skillName, Array.Empty<string>()));
            }

            return message;
        }

        private async Task SendViaCliAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            string? exe = CliIntegration.FindExecutable(CliKind.OpenCode);
            if (exe == null)
            {
                OnError?.Invoke(CliIntegration.GetMissingExecutableMessage(CliKind.OpenCode));
                return;
            }

            string prompt = CliIntegration.BuildWorkspacePrompt();

            var psi = CliIntegration.CreateProcessStartInfo(exe, WorkingDirectory);
            var args = new StringBuilder();
            args.Append("run --format json");
            if (!string.IsNullOrWhiteSpace(Settings.OpenCodeModel))
                args.Append(" --model ").Append(CliIntegration.QuoteForCmd(Settings.OpenCodeModel));
            if (_currentAgent != "build")
                args.Append(" --agent ").Append(CliIntegration.QuoteForCmd(_currentAgent));
            args.Append(' ').Append(CliIntegration.QuoteForCmd(prompt));
            CliIntegration.SetCommandArguments(psi, exe, args.ToString());

            var result = await CliProcessRunner.RunAsync(psi, TimeSpan.FromSeconds(45), ct);
            DebugLog.Write($"OPENCODE stdout={result.StdOut}");
            DebugLog.Write($"OPENCODE stderr={result.StdErr}");
            if (!string.IsNullOrWhiteSpace(result.StdErr))
                OnToolActivity?.Invoke(result.StdErr.Trim());

            var fullText = new StringBuilder();
            foreach (string line in result.StdOut.Replace("\r\n", "\n").Split('\n'))
            {
                if (!string.IsNullOrWhiteSpace(line))
                    ParseOpenCodeEvent(line, fullText);
            }

            if (result.TimedOut)
            {
                OnError?.Invoke("OpenCode CLI timed out waiting for output. The process was stopped.");
                return;
            }

            if (result.ExitCode != 0 && fullText.Length == 0)
                OnError?.Invoke($"OpenCode CLI exited with code {result.ExitCode}.");

            OnDone?.Invoke(fullText.ToString());
        }

        private void ParseOpenCodeEvent(string line, StringBuilder fullText)
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

                if (!appended)
                    OnToolActivity?.Invoke(line);
            }
            catch (JsonException)
            {
                fullText.AppendLine(line);
                OnChunk?.Invoke(line + Environment.NewLine);
            }
        }

        private IEnumerable<string> ExtractTextFragments(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    if (property.Value.ValueKind == JsonValueKind.String &&
                        (property.NameEquals("text") || property.NameEquals("message") || property.NameEquals("content")))
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
                model = Settings.OpenCodeModel,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userMessage }
                },
                stream = true,
                temperature = Settings.Temperature,
                max_tokens = Settings.MaxTokens
            };

            string baseUrl = Settings.OpenCodeBaseUrl.TrimEnd('/');
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrWhiteSpace(Settings.OpenCodeApiKey))
                request.Headers.Add("Authorization", $"Bearer {Settings.OpenCodeApiKey}");

            using var response = await Http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
            if (!response.IsSuccessStatusCode)
            {
                OnError?.Invoke($"OpenCode-compatible API error: {await response.Content.ReadAsStringAsync(ct)}");
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
