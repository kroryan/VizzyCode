using System;
using System.Threading.Tasks;
using System.Threading;

namespace VizzyCode
{
    public enum AiProvider { Claude, Gemini, OpenAI, OpenCode }
    public enum AccessMode { Cli, ApiKey }

    public class AiSettings
    {
        public AiProvider Provider { get; set; } = AiProvider.Claude;

        // Advanced Global
        public double Temperature { get; set; } = 0.7;
        public int    MaxTokens   { get; set; } = 4096;

        // Claude
        public AccessMode ClaudeMode { get; set; } = AccessMode.Cli;
        public string     ClaudeApiKey { get; set; } = "";
        public string     ClaudeModel { get; set; } = "claude-3-7-sonnet-latest";

        // Gemini
        public AccessMode GeminiMode { get; set; } = AccessMode.Cli;
        public string     GeminiApiKey { get; set; } = "";
        public string     GeminiModel { get; set; } = "gemini-2.0-flash-exp";

        // OpenAI (Codex / GPT)
        public AccessMode OpenAiMode { get; set; } = AccessMode.Cli;
        public string     OpenAiApiKey { get; set; } = "";
        public string     OpenAiModel { get; set; } = "gpt-4o";

        // OpenCode (Ollama / CLI)
        public AccessMode OpenCodeMode { get; set; } = AccessMode.Cli;
        public string     OpenCodeBaseUrl { get; set; } = "http://localhost:11434/v1";
        public string     OpenCodeModel { get; set; } = "deepseek-coder-v2";
        public string     OpenCodeApiKey { get; set; } = ""; 
    }

    public interface IAiClient
    {
        event Action<string>? OnChunk;
        event Action<string>? OnDone;
        event Action<string>? OnError;
        event Action<string>? OnToolActivity;
        Task SendAsync(string userMessage, string systemPrompt, CancellationToken ct = default);
    }
}
