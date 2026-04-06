using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;

namespace VizzyCode
{
    public class Skill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Directory { get; set; }
        public string[] Arguments { get; set; }
    }

    public class SkillManager
    {
        private readonly Dictionary<string, Skill> _skills = new();
        private readonly string _claudeSkillsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".claude", "skills");
        private readonly string _opencodeSkillsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".opencode", "agents");

        public SkillManager()
        {
            LoadSkills();
        }

        public IReadOnlyList<Skill> AllSkills => _skills.Values.ToList();

        public Skill? GetSkill(string name)
        {
            return _skills.TryGetValue(name, out var skill) ? skill : null;
        }

        public string GetSkillContent(string name, string[] arguments)
        {
            var skill = GetSkill(name);
            if (skill == null) return "";

            string content = skill.Content;

            foreach (Match match in Regex.Matches(content, @"!\(`([^`]+)`\)"))
            {
                string command = match.Groups[1].Value;
                try
                {
                    var output = ExecuteShellCommand(command);
                    content = content.Replace(match.Value, output);
                }
                catch { }
            }

            content = ReplaceArguments(content, arguments);

            return content;
        }

        private string ReplaceArguments(string content, string[] arguments)
        {
            for (int i = 0; i < arguments.Length; i++)
            {
                content = content.Replace($"$ARGUMENTS[{i}]", arguments[i]);
                content = content.Replace($"${i}", arguments[i]);
            }
            content = content.Replace("$ARGUMENTS", string.Join(" ", arguments));
            return content;
        }

        private string ExecuteShellCommand(string command)
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = command.Contains(" ") ? command.Split(' ')[0] : command,
                Arguments = command.Contains(" ") ? command.Substring(command.IndexOf(' ') + 1) : "",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var proc = System.Diagnostics.Process.Start(psi);
            if (proc == null) return "";
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            return output.Trim();
        }

        private void LoadSkills()
        {
            _skills.Clear();

            LoadSkillsFromDirectory(_claudeSkillsPath, "claude");
            LoadSkillsFromDirectory(_opencodeSkillsPath, "opencode");
        }

        private void LoadSkillsFromDirectory(string directory, string provider)
        {
            if (!Directory.Exists(directory)) return;

            foreach (var skillDir in Directory.GetDirectories(directory))
            {
                var skillFile = Path.Combine(skillDir, "SKILL.md");
                if (File.Exists(skillFile))
                {
                    var skill = ParseSkillFile(skillFile, provider);
                    if (skill != null)
                    {
                        _skills[skill.Name] = skill;
                    }
                }
            }
        }

        private Skill? ParseSkillFile(string filePath, string provider)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                string? name = null;
                string? description = null;
                var frontmatterLines = new List<string>();
                var contentLines = new List<string>();
                bool inFrontmatter = false;

                foreach (var line in lines)
                {
                    if (line == "---")
                    {
                        if (!inFrontmatter)
                        {
                            inFrontmatter = true;
                            continue;
                        }
                        else
                        {
                            inFrontmatter = false;
                            continue;
                        }
                    }

                    if (inFrontmatter)
                    {
                        frontmatterLines.Add(line);
                    }
                    else
                    {
                        contentLines.Add(line);
                    }
                }

                var frontmatter = string.Join("\n", frontmatterLines);
                name = ExtractFrontmatterValue(frontmatter, "name") ?? Path.GetFileNameWithoutExtension(filePath);
                description = ExtractFrontmatterValue(frontmatter, "description") ?? "";

                var skill = new Skill
                {
                    Name = name,
                    Description = description,
                    Content = string.Join("\n", contentLines),
                    Directory = Path.GetDirectoryName(filePath) ?? ""
                };

                return skill;
            }
            catch
            {
                return null;
            }
        }

        private string? ExtractFrontmatterValue(string frontmatter, string key)
        {
            var match = Regex.Match(frontmatter, @$"{key}:\s*(.+)");
            return match.Success ? match.Groups[1].Value.Trim() : null;
        }
    }
}
