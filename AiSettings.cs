using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;

namespace VizzyCode
{
    public enum AiProvider { Claude, Gemini, OpenAI, OpenCode }
    public enum AccessMode { Cli, ApiKey }

    public class AiSettings
    {
        private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

        public AiProvider Provider { get; set; } = AiProvider.Claude;

        // Advanced Global
        public double Temperature { get; set; } = 0.7;
        public int    MaxTokens   { get; set; } = 4096;

        // Claude (CLI only - API key mode not supported for subscription login)
        public AccessMode ClaudeMode { get; set; } = AccessMode.Cli;
        public string     ClaudeApiKey { get; set; } = "";
        public string     ClaudeModel { get; set; } = "sonnet";

        // Gemini
        public AccessMode GeminiMode { get; set; } = AccessMode.Cli;
        public string     GeminiApiKey { get; set; } = "";
        public string     GeminiModel { get; set; } = "gemini-2.5-pro";

        // OpenAI / Codex
        public AccessMode OpenAiMode { get; set; } = AccessMode.Cli;
        public string     OpenAiApiKey { get; set; } = "";
        public string     OpenAiModel { get; set; } = "gpt-5-codex";

        // OpenCode
        public AccessMode OpenCodeMode { get; set; } = AccessMode.Cli;
        public string     OpenCodeBaseUrl { get; set; } = "";
        public string     OpenCodeModel { get; set; } = "";
        public string     OpenCodeApiKey { get; set; } = "";

        public static string SettingsDirectory =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VizzyCode");

        public static string SettingsPath => Path.Combine(SettingsDirectory, "ai-settings.json");

        public static AiSettings Load()
        {
            try
            {
                if (!File.Exists(SettingsPath))
                    return new AiSettings();

                string json = File.ReadAllText(SettingsPath);
                return JsonSerializer.Deserialize<AiSettings>(json, JsonOptions) ?? new AiSettings();
            }
            catch
            {
                return new AiSettings();
            }
        }

        public void Save()
        {
            Directory.CreateDirectory(SettingsDirectory);
            File.WriteAllText(SettingsPath, JsonSerializer.Serialize(this, JsonOptions));
        }
    }

    public class ToolCallRequest
    {
        public string CallId { get; set; }
        public string ToolName { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public string Description { get; set; }
    }

    public class ToolCallResponse
    {
        public string CallId { get; set; }
        public object Result { get; set; }
        public bool Success { get; set; }
    }

    public interface IAiClient
    {
        string WorkingDirectory { get; set; }
        event Action<string>? OnChunk;
        event Action<string>? OnDone;
        event Action<string>? OnError;
        event Action<string>? OnToolActivity;
        event Action<ToolCallRequest>? OnApprovalRequired;
        event Action<ToolCallResponse>? OnToolCall;
        Task SendAsync(string userMessage, string systemPrompt, CancellationToken ct = default);
        void ApproveTool(string callId);
        void RejectTool(string callId);
        void SwitchAgent(string agentName);
        void InvokeSkill(string skillName, string[] arguments);
    }
}
