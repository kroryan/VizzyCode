using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace VizzyCode
{
    internal static class VizzyCoverageVerifier
    {
        public static string Run(string rootDir)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Vizzy Coverage Report");
            sb.AppendLine($"Root: {rootDir}");
            sb.AppendLine($"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine();

            var builderNodes = GetBuilderNodeNames(rootDir);
            var supportedNodes = GetSupportedNodeNames(rootDir);
            var aliasSatisfied = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["LogMessage"] = "Log",
                ["LogFlight"] = "FlightLog",
                ["SetCameraProperty"] = "SetCamera",
                ["SetCraftProperty"] = "LegacyAliasHandler",
                ["SetList"] = "ListOp",
                ["CustomExpression"] = "CustomExpressionDeclarations",
                ["CustomInstruction"] = "CustomInstructionDeclarations",
                ["FlightProgram"] = "ProgramTraversal",
                ["Unknown"] = "CraftTraversal"
            };

            var missing = builderNodes
                .Where(n => !supportedNodes.Contains(n) && !aliasSatisfied.ContainsKey(n))
                .OrderBy(n => n)
                .ToList();

            sb.AppendLine("Builder nodes:");
            sb.AppendLine(string.Join(", ", builderNodes.OrderBy(x => x)));
            sb.AppendLine();
            sb.AppendLine("Unsupported builder nodes after alias mapping:");
            sb.AppendLine(missing.Count == 0 ? "none" : string.Join(", ", missing));
            sb.AppendLine();

            var xmlFiles = Directory.GetFiles(rootDir, "*.xml", SearchOption.TopDirectoryOnly)
                .Where(path => !Path.GetFileName(path).Contains("roundtrip", StringComparison.OrdinalIgnoreCase))
                .OrderBy(Path.GetFileName)
                .ToList();

            sb.AppendLine("Round-trip samples:");
            foreach (var xmlPath in xmlFiles)
            {
                try
                {
                    var doc = XDocument.Load(xmlPath);
                    var conv = new VizzyXmlConverter();
                    bool isCraft = doc.Root?.Name.LocalName is "Craft" or "Assembly" or "FlightProgram";
                    string code = isCraft ? conv.ConvertCraftToCode(doc) : conv.ConvertProgramToCode(doc);
                    bool hasTodo = code.Contains("[TODO]", StringComparison.OrdinalIgnoreCase);
                    int warningCount = conv.Warnings.Count;

                    string programName = Path.GetFileNameWithoutExtension(xmlPath) + " roundtrip";
                    var xml2 = conv.ConvertCodeToXml(code, programName);
                    bool hasInstructions = xml2.Descendants("Instructions").Any();
                    bool hasProgram = xml2.Root?.Name.LocalName == "Program";

                    sb.AppendLine($"{Path.GetFileName(xmlPath)}");
                    sb.AppendLine($"  import warnings: {warningCount}");
                    sb.AppendLine($"  code contains TODO: {hasTodo}");
                    sb.AppendLine($"  export root program: {hasProgram}");
                    sb.AppendLine($"  export has instructions: {hasInstructions}");
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"{Path.GetFileName(xmlPath)}");
                    sb.AppendLine($"  ERROR: {ex.GetType().Name}: {ex.Message}");
                }
            }

            return sb.ToString();
        }

        private static HashSet<string> GetBuilderNodeNames(string rootDir)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var file in Directory.GetFiles(Path.Combine(rootDir, "VizzyBuilder"), "*.cs", SearchOption.TopDirectoryOnly))
            {
                foreach (Match match in Regex.Matches(File.ReadAllText(file), "new XElement\\(\"([^\"]+)\""))
                    result.Add(match.Groups[1].Value);
            }
            return result;
        }

        private static HashSet<string> GetSupportedNodeNames(string rootDir)
        {
            var path = Path.Combine(rootDir, "VizzyXmlConverter.cs");
            var text = File.ReadAllText(path);
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (Match match in Regex.Matches(text, "case \"([^\"]+)\""))
                result.Add(match.Groups[1].Value);
            foreach (Match match in Regex.Matches(text, "new XElement\\(\"([^\"]+)\""))
                result.Add(match.Groups[1].Value);

            return result;
        }
    }
}
