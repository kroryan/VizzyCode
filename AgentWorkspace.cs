using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VizzyCode
{
    internal sealed class AgentWorkspace
    {
        public string RootDirectory { get; }
        public string CurrentProgramPath => Path.Combine(RootDirectory, "CurrentProgram.cs");
        public string TaskPath => Path.Combine(RootDirectory, "TASK.md");
        public string SystemPromptPath => Path.Combine(RootDirectory, "SYSTEM_PROMPT.md");
        public string MetadataPath => Path.Combine(RootDirectory, "workspace.json");

        private AgentWorkspace(string rootDirectory)
        {
            RootDirectory = rootDirectory;
        }

        public static AgentWorkspace Create(string? sourceDirectory, string? documentKey)
        {
            string baseRoot = Path.Combine(AiSettings.SettingsDirectory, "agent-workspaces");
            Directory.CreateDirectory(baseRoot);

            string scope = string.IsNullOrWhiteSpace(documentKey)
                ? (string.IsNullOrWhiteSpace(sourceDirectory) ? "unsaved" : sourceDirectory)
                : documentKey;

            string folderName = BuildFolderName(scope);
            string root = Path.Combine(baseRoot, folderName);
            Directory.CreateDirectory(root);

            var workspace = new AgentWorkspace(root);
            workspace.WriteMetadata(sourceDirectory, documentKey);
            return workspace;
        }

        public void WriteInputs(string currentCode, string userMessage, string systemPrompt)
        {
            File.WriteAllText(CurrentProgramPath, currentCode ?? string.Empty, new UTF8Encoding(false));

            var task = new StringBuilder();
            task.AppendLine("# Task");
            task.AppendLine();
            task.AppendLine(userMessage.Trim());
            task.AppendLine();
            task.AppendLine("Rules:");
            task.AppendLine("- Inspect `CurrentProgram.cs` before making changes.");
            task.AppendLine("- Edit `CurrentProgram.cs` directly when code changes are needed.");
            task.AppendLine("- Keep the file compilable C# when possible.");
            task.AppendLine("- Respond with a concise summary of what you changed.");
            File.WriteAllText(TaskPath, task.ToString(), new UTF8Encoding(false));

            File.WriteAllText(SystemPromptPath, systemPrompt ?? string.Empty, new UTF8Encoding(false));
        }

        public string ReadCurrentProgram()
        {
            return File.Exists(CurrentProgramPath)
                ? File.ReadAllText(CurrentProgramPath)
                : string.Empty;
        }

        private void WriteMetadata(string? sourceDirectory, string? documentKey)
        {
            string json =
                "{\n" +
                $"  \"sourceDirectory\": {ToJsonString(sourceDirectory)},\n" +
                $"  \"documentKey\": {ToJsonString(documentKey)},\n" +
                $"  \"updatedAtUtc\": {ToJsonString(DateTime.UtcNow.ToString("O"))}\n" +
                "}\n";

            File.WriteAllText(MetadataPath, json, new UTF8Encoding(false));
        }

        private static string BuildFolderName(string scope)
        {
            string normalized = scope.Trim();
            using var sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(normalized));
            string shortHash = Convert.ToHexString(hash, 0, 8).ToLowerInvariant();

            string prefix = normalized
                .Replace(':', '_')
                .Replace('\\', '_')
                .Replace('/', '_')
                .Replace(' ', '_');

            if (prefix.Length > 32)
                prefix = prefix[..32];

            return $"{prefix}_{shortHash}";
        }

        private static string ToJsonString(string? value)
        {
            if (value == null)
                return "null";

            return "\"" + value.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }
    }
}
