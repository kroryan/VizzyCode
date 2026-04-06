using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace VizzyCode
{
    internal enum CliKind
    {
        Claude,
        Gemini,
        Codex,
        OpenCode
    }

    internal static class CliIntegration
    {
        public static string BuildCombinedPrompt(string systemPrompt, string userMessage)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(systemPrompt))
            {
                sb.AppendLine("System instructions:");
                sb.AppendLine(systemPrompt.Trim());
                sb.AppendLine();
            }

            sb.AppendLine("User request:");
            sb.Append(userMessage.Trim());
            return sb.ToString().Trim();
        }

        public static string BuildWorkspacePrompt(string? workspacePath = null)
        {
            if (!string.IsNullOrWhiteSpace(workspacePath))
                return $"Read {workspacePath}/TASK.md and {workspacePath}/SYSTEM_PROMPT.md, inspect {workspacePath}/CurrentProgram.cs, edit {workspacePath}/CurrentProgram.cs when needed, then respond with a concise summary.";
            return "Read TASK.md and SYSTEM_PROMPT.md, inspect CurrentProgram.cs, edit CurrentProgram.cs when needed, then respond with a concise summary.";
        }

        public static string? FindExecutable(CliKind kind)
        {
            string[] names = GetExecutableNames(kind);
            string? preferred = kind switch
            {
                CliKind.Codex => FindPreferredExecutable(names, new[] { ".cmd", ".exe", ".bat", "" }),
                CliKind.Gemini => FindPreferredExecutable(names, new[] { ".cmd", ".exe", ".bat", "" }),
                CliKind.OpenCode => FindPreferredExecutable(names, new[] { ".cmd", ".exe", ".bat", "" }),
                _ => FindPreferredExecutable(names, new[] { ".exe", ".cmd", ".bat", "" })
            };

            return preferred;
        }

        public static string? FindExecutable(params string[] names)
        {
            return FindPreferredExecutable(names, new[] { ".exe", ".cmd", ".bat", "" });
        }

        private static string? FindPreferredExecutable(string[] names, string[] extensionPreference)
        {
            string path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            foreach (string dir in path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
            {
                string trimmed = dir.Trim();
                if (trimmed.Length == 0)
                    continue;

                foreach (string name in names)
                {
                    foreach (string ext in extensionPreference)
                    {
                        string candidate = Path.Combine(trimmed, name + ext);
                        if (File.Exists(candidate))
                            return candidate;
                    }
                }
            }

            return null;
        }

        public static string GetMissingExecutableMessage(CliKind kind)
        {
            return kind switch
            {
                CliKind.Claude => "Claude CLI not found. Install `@anthropic-ai/claude-code` and authenticate with `claude` or `claude auth`.",
                CliKind.Gemini => "Gemini CLI not found. Install `@google/gemini-cli` and authenticate with `gemini`.",
                CliKind.Codex => "Codex CLI not found. Install `@openai/codex` and authenticate with `codex login`.",
                CliKind.OpenCode => "OpenCode CLI not found. Install `opencode-ai` and authenticate with `opencode auth`.",
                _ => "CLI executable not found."
            };
        }

        public static string QuoteForCmd(string value)
        {
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        public static ProcessStartInfo CreateProcessStartInfo(string executablePath, string workingDirectory)
        {
            bool useCmdWrapper = executablePath.EndsWith(".cmd", StringComparison.OrdinalIgnoreCase) ||
                                 executablePath.EndsWith(".bat", StringComparison.OrdinalIgnoreCase);

            return new ProcessStartInfo
            {
                FileName = useCmdWrapper ? "cmd.exe" : executablePath,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };
        }

        public static void SetCommandArguments(ProcessStartInfo psi, string executablePath, string argumentText)
        {
            bool useCmdWrapper = executablePath.EndsWith(".cmd", StringComparison.OrdinalIgnoreCase) ||
                                 executablePath.EndsWith(".bat", StringComparison.OrdinalIgnoreCase);

            string trimmedArguments = argumentText?.Trim() ?? string.Empty;
            if (!useCmdWrapper)
            {
                psi.Arguments = trimmedArguments;
                return;
            }

            string innerCommand = $"{QuoteForCmd(executablePath)}";
            if (!string.IsNullOrWhiteSpace(trimmedArguments))
                innerCommand += " " + trimmedArguments;

            psi.Arguments = "/d /c " + QuoteForCmd(innerCommand);
        }

        public static bool TryLaunchInteractive(AiSettings settings, string workingDirectory, string systemPrompt, string userPrompt, out string error)
        {
            error = string.Empty;

            CliKind kind = settings.Provider switch
            {
                AiProvider.Claude => CliKind.Claude,
                AiProvider.Gemini => CliKind.Gemini,
                AiProvider.OpenAI => CliKind.Codex,
                AiProvider.OpenCode => CliKind.OpenCode,
                _ => CliKind.Claude
            };

            string? exe = FindExecutable(kind);
            if (exe == null)
            {
                error = GetMissingExecutableMessage(kind);
                return false;
            }

            string command = BuildInteractiveCommand(settings, exe, BuildWorkspacePrompt());
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/k cd /d {QuoteForCmd(workingDirectory)} && {command}",
                WorkingDirectory = workingDirectory,
                UseShellExecute = true
            };

            Process.Start(psi);
            return true;
        }

        private static string BuildInteractiveCommand(AiSettings settings, string exe, string combinedPrompt)
        {
            var parts = new List<string> { QuoteForCmd(exe) };

            switch (settings.Provider)
            {
                case AiProvider.Claude:
                    if (!string.IsNullOrWhiteSpace(settings.ClaudeModel))
                    {
                        parts.Add("--model");
                        parts.Add(QuoteForCmd(settings.ClaudeModel));
                    }
                    if (!string.IsNullOrWhiteSpace(combinedPrompt))
                        parts.Add(QuoteForCmd(combinedPrompt));
                    break;

                case AiProvider.Gemini:
                    if (!string.IsNullOrWhiteSpace(settings.GeminiModel))
                    {
                        parts.Add("--model");
                        parts.Add(QuoteForCmd(settings.GeminiModel));
                    }
                    if (!string.IsNullOrWhiteSpace(combinedPrompt))
                    {
                        parts.Add("-p");
                        parts.Add(QuoteForCmd(combinedPrompt));
                    }
                    break;

                case AiProvider.OpenAI:
                    if (!string.IsNullOrWhiteSpace(settings.OpenAiModel))
                    {
                        parts.Add("--model");
                        parts.Add(QuoteForCmd(settings.OpenAiModel));
                    }
                    if (!string.IsNullOrWhiteSpace(combinedPrompt))
                        parts.Add(QuoteForCmd(combinedPrompt));
                    break;

                case AiProvider.OpenCode:
                    if (!string.IsNullOrWhiteSpace(settings.OpenCodeModel))
                    {
                        parts.Add("--model");
                        parts.Add(QuoteForCmd(settings.OpenCodeModel));
                    }
                    if (!string.IsNullOrWhiteSpace(combinedPrompt))
                    {
                        parts.Add("--prompt");
                        parts.Add(QuoteForCmd(combinedPrompt));
                    }
                    break;
            }

            return string.Join(" ", parts);
        }

        private static string[] GetExecutableNames(CliKind kind)
        {
            return kind switch
            {
                CliKind.Claude => new[] { "claude" },
                CliKind.Gemini => new[] { "gemini" },
                CliKind.Codex => new[] { "codex" },
                CliKind.OpenCode => new[] { "opencode" },
                _ => Array.Empty<string>()
            };
        }
    }
}
