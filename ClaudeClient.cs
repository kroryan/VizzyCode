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
    public class ClaudeClient : IAiClient
    {
        private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromMinutes(10) };
        private readonly SkillManager _skillManager = new();

        public AiSettings Settings { get; set; } = new();
        public string WorkingDirectory { get; set; } = Environment.CurrentDirectory;
        public string WorkspaceDirectory { get; set; } = Environment.CurrentDirectory;
        public string? LastSessionId { get; private set; }

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

                if (Settings.ClaudeMode == AccessMode.ApiKey && !string.IsNullOrWhiteSpace(Settings.ClaudeApiKey))
                    await SendViaApiAsync(userMessage, systemPrompt, ct);
                else
                    await SendViaCliAsync(userMessage, systemPrompt, ct);
            }
            catch (OperationCanceledException)
            {
                OnError?.Invoke("Claude request cancelled.");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Claude error: {ex.Message}");
            }
        }

        public void ApproveTool(string callId)
        {
            OnToolActivity?.Invoke("Claude CLI approvals are handled in the native interactive CLI. Use Open CLI for consent prompts.");
        }

        public void RejectTool(string callId)
        {
            OnToolActivity?.Invoke("Claude CLI approvals are handled in the native interactive CLI. Use Open CLI for consent prompts.");
        }

        public void SwitchAgent(string agentName)
        {
            if (string.IsNullOrWhiteSpace(agentName))
                return;

            agentName = agentName.Trim().ToLowerInvariant();
            if (agentName is "build" or "plan" or "general")
                _currentAgent = agentName;
            else
                OnError?.Invoke("Unknown Claude agent. Valid values: build, plan, general.");
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
                    OnDone?.Invoke("Claude Code commands are available in the native CLI. Use Open CLI for full slash command support.");
                    return string.Empty;
                case "agent":
                    SwitchAgent(args);
                    OnDone?.Invoke($"Claude agent set to `{_currentAgent}`.");
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
            string? exe = CliIntegration.FindExecutable(CliKind.Claude);
            if (exe == null)
            {
                OnError?.Invoke(CliIntegration.GetMissingExecutableMessage(CliKind.Claude));
                return;
            }

            string prompt = CliIntegration.BuildWorkspacePrompt(WorkspaceDirectory);

            var psi = CliIntegration.CreateProcessStartInfo(exe, WorkingDirectory);
            var args = new StringBuilder();
            args.Append("-p --output-format stream-json --include-partial-messages");
            if (!string.IsNullOrWhiteSpace(Settings.ClaudeModel))
                args.Append(" --model ").Append(CliIntegration.QuoteForCmd(Settings.ClaudeModel));
            args.Append(" --append-system-prompt ")
                .Append(CliIntegration.QuoteForCmd("When running inside VizzyCode headless mode, prefer concise answers. If a task requires interactive approval, tell the user to use Open CLI."));
            args.Append(' ').Append(CliIntegration.QuoteForCmd(prompt));
            CliIntegration.SetCommandArguments(psi, exe, args.ToString());

            var result = await CliProcessRunner.RunAsync(psi, TimeSpan.FromSeconds(Settings.CliTimeoutSeconds), ct);
            DebugLog.Write($"CLAUDE stdout={result.StdOut}");
            DebugLog.Write($"CLAUDE stderr={result.StdErr}");
            if (!string.IsNullOrWhiteSpace(result.StdErr))
                OnToolActivity?.Invoke(result.StdErr.Trim());

            var fullText = new StringBuilder();
            foreach (string line in result.StdOut.Replace("\r\n", "\n").Split('\n'))
            {
                if (!string.IsNullOrWhiteSpace(line))
                    ParseCliEvent(line, fullText);
            }

            if (result.TimedOut)
            {
                OnError?.Invoke("Claude CLI timed out waiting for output. The process was stopped.");
                return;
            }

            if (result.ExitCode != 0 && fullText.Length == 0)
                OnError?.Invoke($"Claude CLI exited with code {result.ExitCode}.");

            OnDone?.Invoke(fullText.ToString());
        }

        private void ParseCliEvent(string line, StringBuilder fullText)
        {
            try
            {
                using var doc = JsonDocument.Parse(line);
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("session_id", out var sessionId) && sessionId.ValueKind == JsonValueKind.String)
                    LastSessionId = sessionId.GetString();

                string type = root.TryGetProperty("type", out var typeProp) ? typeProp.GetString() ?? string.Empty : string.Empty;

                if (type == "stream_event" && root.TryGetProperty("event", out var ev))
                {
                    string eventType = ev.TryGetProperty("type", out var evType) ? evType.GetString() ?? string.Empty : string.Empty;

                    if (eventType == "content_block_delta" &&
                        ev.TryGetProperty("delta", out var delta) &&
                        delta.TryGetProperty("type", out var deltaType) &&
                        deltaType.GetString() == "text_delta" &&
                        delta.TryGetProperty("text", out var textProp))
                    {
                        AppendChunk(textProp.GetString(), fullText);
                        return;
                    }

                    if (eventType == "tool_use" && ev.TryGetProperty("name", out var toolName))
                    {
                        OnToolActivity?.Invoke($"Claude tool: {toolName.GetString()}");
                        return;
                    }
                }

                if (type == "tool_use" && root.TryGetProperty("name", out var name))
                {
                    OnToolActivity?.Invoke($"Claude tool: {name.GetString()}");
                    return;
                }

                if (type == "result" && root.TryGetProperty("result", out var result))
                {
                    if (result.ValueKind == JsonValueKind.String)
                        AppendChunk(result.GetString(), fullText);
                    return;
                }

                if (type == "error")
                {
                    OnError?.Invoke(line);
                    return;
                }
            }
            catch (JsonException)
            {
                AppendChunk(line, fullText, appendNewLine: true);
            }
        }

        private void AppendChunk(string? text, StringBuilder fullText, bool appendNewLine = false)
        {
            if (string.IsNullOrEmpty(text))
                return;

            fullText.Append(text);
            if (appendNewLine)
                fullText.AppendLine();
            OnChunk?.Invoke(text);
        }

        private async Task SendViaApiAsync(string userMessage, string systemPrompt, CancellationToken ct)
        {
            var body = new
            {
                model = Settings.ClaudeModel,
                max_tokens = Settings.MaxTokens,
                system = systemPrompt,
                messages = new[] { new { role = "user", content = userMessage } },
                stream = true
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages")
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };

            request.Headers.Add("x-api-key", Settings.ClaudeApiKey);
            request.Headers.Add("anthropic-version", "2023-06-01");

            using var response = await Http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
            if (!response.IsSuccessStatusCode)
            {
                OnError?.Invoke($"Claude API error: {await response.Content.ReadAsStringAsync(ct)}");
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
                    JsonElement root = doc.RootElement;
                    string type = root.TryGetProperty("type", out var typeProp) ? typeProp.GetString() ?? string.Empty : string.Empty;

                    if (type == "content_block_delta" &&
                        root.TryGetProperty("delta", out var delta) &&
                        delta.TryGetProperty("text", out var text))
                    {
                        string chunk = text.GetString() ?? string.Empty;
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
    }
}
