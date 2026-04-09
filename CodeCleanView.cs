using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace VizzyCode;

internal sealed class CleanViewSidecar
{
    public int Version { get; set; } = 1;
    public List<CleanViewLineMap> Lines { get; set; } = new();
}

internal sealed class CleanViewLineMap
{
    public string CleanLine { get; set; } = string.Empty;
    public string ExactLine { get; set; } = string.Empty;
    public List<string> MetadataLines { get; set; } = new();
}

internal static class CodeCleanView
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private static readonly Regex RawXmlVariableRegex = new(@"Vz\.RawXmlVariable\(@""(?<xml>(?:""""|[^""])*)""\)", RegexOptions.Compiled);
    private static readonly Regex RawXmlConstantRegex = new(@"Vz\.RawXmlConstant\(@""(?<xml>(?:""""|[^""])*)""\)", RegexOptions.Compiled);
    private static readonly Regex RawXmlEvalRegex = new(@"Vz\.RawXmlEval\(@""(?<xml>(?:""""|[^""])*)""\)", RegexOptions.Compiled);

    public static (string CleanCode, CleanViewSidecar Sidecar) CreateCleanCode(string exactCode)
    {
        var sidecar = new CleanViewSidecar();
        var cleanLines = new List<string>();
        var pendingMetadata = new List<string>();

        foreach (string line in SplitLines(NormalizeNewlines(exactCode)))
        {
            if (IsHiddenMetadataLine(line))
            {
                pendingMetadata.Add(line);
                continue;
            }

            string cleanLine = SimplifyVisibleLine(line);
            cleanLines.Add(cleanLine);
            sidecar.Lines.Add(new CleanViewLineMap
            {
                CleanLine = cleanLine,
                ExactLine = line,
                MetadataLines = new List<string>(pendingMetadata)
            });
            pendingMetadata.Clear();
        }

        return (JoinLines(cleanLines), sidecar);
    }

    public static string RestoreExactCode(string cleanCode, CleanViewSidecar? sidecar)
    {
        if (sidecar == null || sidecar.Lines.Count == 0)
            return NormalizeNewlines(cleanCode);

        var currentLines = SplitLines(NormalizeNewlines(cleanCode));
        var output = new List<string>();
        int sidecarIndex = 0;

        foreach (string currentLine in currentLines)
        {
            int matchIndex = FindMatchingIndex(currentLine, sidecar.Lines, sidecarIndex, 32);
            if (matchIndex >= 0)
            {
                var map = sidecar.Lines[matchIndex];
                output.AddRange(map.MetadataLines);
                output.Add(map.ExactLine);
                sidecarIndex = matchIndex + 1;
            }
            else
            {
                output.Add(currentLine);
            }
        }

        return JoinLines(output);
    }

    public static string GetSidecarPath(string codePath)
    {
        string fullPath = Path.GetFullPath(codePath);
        string fileName = Path.GetFileName(fullPath);
        string directory = Path.GetDirectoryName(fullPath)!;

        if (fileName.EndsWith(".vizzy.cs", StringComparison.OrdinalIgnoreCase))
            return Path.Combine(directory, fileName[..^(".vizzy.cs".Length)] + ".vizzy.meta.json");

        return fullPath + ".meta.json";
    }

    public static void SaveSidecar(string codePath, CleanViewSidecar sidecar)
    {
        string sidecarPath = GetSidecarPath(codePath);
        Directory.CreateDirectory(Path.GetDirectoryName(sidecarPath)!);
        File.WriteAllText(sidecarPath, JsonSerializer.Serialize(sidecar, JsonOptions), new UTF8Encoding(false));
    }

    public static CleanViewSidecar? LoadSidecar(string codePath)
    {
        string sidecarPath = GetSidecarPath(codePath);
        if (!File.Exists(sidecarPath))
            return null;

        string json = File.ReadAllText(sidecarPath, new UTF8Encoding(false));
        return JsonSerializer.Deserialize<CleanViewSidecar>(json);
    }

    private static bool IsHiddenMetadataLine(string line)
    {
        string trimmed = line.TrimStart();
        return trimmed.StartsWith("// VZTOPBLOCK", StringComparison.Ordinal)
            || trimmed.StartsWith("// VZBLOCK ", StringComparison.Ordinal)
            || trimmed.StartsWith("// VZEL ", StringComparison.Ordinal);
    }

    private static string SimplifyVisibleLine(string line)
    {
        string simplified = line;
        simplified = RawXmlVariableRegex.Replace(simplified, SimplifyRawXmlVariable);
        simplified = RawXmlConstantRegex.Replace(simplified, SimplifyRawXmlConstant);
        simplified = RawXmlEvalRegex.Replace(simplified, SimplifyRawXmlEval);
        return simplified;
    }

    private static string SimplifyRawXmlVariable(Match match)
    {
        try
        {
            var el = XElement.Parse(UnescapeVerbatimXml(match.Groups["xml"].Value));
            if (el.Name.LocalName != "Variable")
                return match.Value;

            string? variableName = el.Attribute("variableName")?.Value;
            if (!string.IsNullOrWhiteSpace(variableName) && Regex.IsMatch(variableName, @"^[A-Za-z_]\w*$"))
                return variableName;
        }
        catch
        {
        }

        return match.Value;
    }

    private static string SimplifyRawXmlConstant(Match match)
    {
        try
        {
            var el = XElement.Parse(UnescapeVerbatimXml(match.Groups["xml"].Value));
            if (el.Name.LocalName != "Constant")
                return match.Value;

            string? boolValue = el.Attribute("bool")?.Value;
            if (boolValue is "true" or "false")
                return boolValue;

            string? numberValue = el.Attribute("number")?.Value;
            if (!string.IsNullOrWhiteSpace(numberValue))
                return numberValue;

            string? textValue = el.Attribute("text")?.Value;
            if (string.IsNullOrEmpty(textValue))
                return match.Value;

            if (textValue is "true" or "false")
                return textValue;

            if (Regex.IsMatch(textValue, @"^-?(?:\d+(?:\.\d+)?|\.\d+)(?:[eE][+\-]?\d+)?$"))
                return textValue;

            return ToQuotedString(textValue);
        }
        catch
        {
        }

        return match.Value;
    }

    private static string SimplifyRawXmlEval(Match match)
    {
        try
        {
            var el = XElement.Parse(UnescapeVerbatimXml(match.Groups["xml"].Value));
            if (el.Name.LocalName != "EvaluateExpression")
                return match.Value;

            var constant = el.Elements().SingleOrDefault();
            if (constant?.Name.LocalName != "Constant")
                return match.Value;

            string? textValue = constant.Attribute("text")?.Value;
            if (string.IsNullOrWhiteSpace(textValue))
                return match.Value;

            return $"Vz.ExactEval({ToQuotedString(textValue)})";
        }
        catch
        {
        }

        return match.Value;
    }

    private static int FindMatchingIndex(string currentLine, List<CleanViewLineMap> sidecarLines, int startIndex, int lookahead)
    {
        string normalizedCurrent = NormalizeComparableLine(currentLine);
        for (int i = startIndex; i < sidecarLines.Count && i < startIndex + lookahead; i++)
        {
            if (NormalizeComparableLine(sidecarLines[i].CleanLine) == normalizedCurrent)
                return i;
        }

        return -1;
    }

    private static string NormalizeComparableLine(string line)
    {
        return line.Trim();
    }

    private static string NormalizeNewlines(string text)
    {
        return text.Replace("\r\n", "\n").Replace('\r', '\n');
    }

    private static List<string> SplitLines(string text)
    {
        return text.Split('\n').ToList();
    }

    private static string JoinLines(List<string> lines)
    {
        return string.Join("\n", lines);
    }

    private static string UnescapeVerbatimXml(string value)
    {
        return value.Replace("\"\"", "\"");
    }

    private static string ToQuotedString(string value)
    {
        return "\"" + value
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\r", "\\r")
            .Replace("\n", "\\n")
            .Replace("\t", "\\t") + "\"";
    }
}
