using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace VizzyCode;

internal static class Program
{
    private static int Main(string[] args)
    {
        try
        {
            if (args.Length == 0 || IsHelp(args[0]))
            {
                PrintUsage();
                return 0;
            }

            return args[0].ToLowerInvariant() switch
            {
                "import" => RunImport(args.Skip(1).ToArray()),
                "export" => RunExport(args.Skip(1).ToArray()),
                "roundtrip" => RunRoundTrip(args.Skip(1).ToArray()),
                "raw-encode" => RunRawEncode(args.Skip(1).ToArray()),
                "raw-decode" => RunRawDecode(args.Skip(1).ToArray()),
                _ => Fail($"Unknown command: {args[0]}")
            };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
            return 1;
        }
    }

    private static int RunImport(string[] args)
    {
        if (args.Length < 1)
            return Fail("import requires <input.xml>");

        string inputPath = Path.GetFullPath(args[0]);
        string outputPath = GetOption(args, "-o", "--output")
            ?? Path.ChangeExtension(inputPath, ".vizzy.cs");

        var converter = new VizzyXmlConverter();
        string xmlText = File.ReadAllText(inputPath, DetectEncodingWithBom(inputPath));
        var doc = XDocument.Parse(xmlText);
        bool isCraft = doc.Root?.Name.LocalName is "Craft" or "Assembly";
        string exactCode = isCraft ? converter.ConvertCraftToCode(doc) : converter.ConvertProgramToCode(doc);
        var cleanView = CodeCleanView.CreateCleanCode(exactCode);
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputPath))!);
        File.WriteAllText(outputPath, cleanView.CleanCode, new UTF8Encoding(false));
        CodeCleanView.SaveSidecar(outputPath, cleanView.Sidecar);
        Console.WriteLine($"Imported: {inputPath}");
        Console.WriteLine($"Code: {Path.GetFullPath(outputPath)}");
        Console.WriteLine($"Metadata: {Path.GetFullPath(CodeCleanView.GetSidecarPath(outputPath))}");
        return 0;
    }

    private static int RunExport(string[] args)
    {
        if (args.Length < 1)
            return Fail("export requires <input.cs>");

        string inputPath = Path.GetFullPath(args[0]);
        string outputPath = GetOption(args, "-o", "--output")
            ?? Path.ChangeExtension(RemoveVizzyCodeSuffix(inputPath), ".xml");
        string? explicitName = GetOption(args, "-n", "--name");

        string cleanCode = File.ReadAllText(inputPath, new UTF8Encoding(false));
        string programName = explicitName
            ?? InferProgramNameFromCode(cleanCode)
            ?? Path.GetFileNameWithoutExtension(RemoveVizzyCodeSuffix(inputPath));
        string exportCode = CodeCleanView.RestoreExactCode(cleanCode, CodeCleanView.LoadSidecar(inputPath));

        var converter = new VizzyXmlConverter();
        var doc = converter.ConvertCodeToXml(exportCode, programName);
        var validationErrors = VizzyExportValidator.Validate(doc);
        if (validationErrors.Count > 0)
            return Fail("Export validation failed:" + Environment.NewLine + VizzyExportValidator.Format(validationErrors));
        SaveXml(outputPath, doc);
        Console.WriteLine($"Exported: {inputPath}");
        Console.WriteLine($"XML: {Path.GetFullPath(outputPath)}");
        return 0;
    }

    private static int RunRoundTrip(string[] args)
    {
        if (args.Length < 1)
            return Fail("roundtrip requires <input.xml>");

        string inputPath = Path.GetFullPath(args[0]);
        string outputPath = GetOption(args, "-o", "--output")
            ?? Path.Combine(Path.GetDirectoryName(inputPath)!,
                Path.GetFileNameWithoutExtension(inputPath) + ".roundtrip.xml");
        string codePath = GetOption(args, "-c", "--code")
            ?? Path.Combine(Path.GetDirectoryName(inputPath)!,
                Path.GetFileNameWithoutExtension(inputPath) + ".roundtrip.vizzy.cs");

        var converter = new VizzyXmlConverter();
        string xmlText = File.ReadAllText(inputPath, DetectEncodingWithBom(inputPath));
        var xdoc = XDocument.Parse(xmlText);
        string exactCode = converter.ConvertProgramToCode(xdoc);
        var cleanView = CodeCleanView.CreateCleanCode(exactCode);
        File.WriteAllText(codePath, cleanView.CleanCode, new UTF8Encoding(false));
        CodeCleanView.SaveSidecar(codePath, cleanView.Sidecar);
        string exportCode = CodeCleanView.RestoreExactCode(cleanView.CleanCode, cleanView.Sidecar);
        var outputDoc = converter.ConvertCodeToXml(exportCode);
        var validationErrors = VizzyExportValidator.Validate(outputDoc);
        if (validationErrors.Count > 0)
            return Fail("Round-trip export validation failed:" + Environment.NewLine + VizzyExportValidator.Format(validationErrors));
        SaveXml(outputPath, outputDoc);

        bool sameBytes = File.ReadAllBytes(inputPath).SequenceEqual(File.ReadAllBytes(outputPath));
        Console.WriteLine($"Input XML:  {inputPath}");
        Console.WriteLine($"Code file:  {Path.GetFullPath(codePath)}");
        Console.WriteLine($"Output XML: {Path.GetFullPath(outputPath)}");
        Console.WriteLine($"same_bytes={sameBytes}");
        return sameBytes ? 0 : 2;
    }

    private static int RunRawEncode(string[] args)
    {
        if (args.Length < 1)
            return Fail("raw-encode requires <input.xml>");

        string inputPath = Path.GetFullPath(args[0]);
        string? outputPath = GetOption(args, "-o", "--output");
        string xml = File.ReadAllText(inputPath, DetectEncodingWithBom(inputPath));
        var element = XElement.Parse(xml);
        string payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(element.ToString(SaveOptions.DisableFormatting)));
        string kind = element.Name.LocalName switch
        {
            "Constant" => "RawConstant",
            "Variable" => "RawVariable",
            "CraftProperty" => "RawCraftProperty",
            "EvaluateExpression" => "RawEval",
            _ => "RawEval"
        };

        string verbatimXml = ToVerbatimStringLiteral(element.ToString(SaveOptions.DisableFormatting));
        string rawXmlKind = kind switch
        {
            "RawConstant" => "RawXmlConstant",
            "RawVariable" => "RawXmlVariable",
            "RawCraftProperty" => "RawXmlCraftProperty",
            _ => "RawXmlEval"
        };

        var sb = new StringBuilder();
        sb.AppendLine($"Element: {element.Name.LocalName}");
        sb.AppendLine("Base64 payload:");
        sb.AppendLine(payload);
        sb.AppendLine();
        sb.AppendLine("Base64 form:");
        sb.AppendLine($"Vz.{kind}(\"{payload}\")");
        sb.AppendLine();
        sb.AppendLine("Readable XML form:");
        sb.AppendLine($"Vz.{rawXmlKind}({verbatimXml})");

        string output = sb.ToString();
        if (!string.IsNullOrWhiteSpace(outputPath))
        {
            File.WriteAllText(Path.GetFullPath(outputPath), output, new UTF8Encoding(false));
            Console.WriteLine($"Wrote: {Path.GetFullPath(outputPath)}");
        }
        else
        {
            Console.Write(output);
        }

        return 0;
    }

    private static int RunRawDecode(string[] args)
    {
        if (args.Length < 1)
            return Fail("raw-decode requires <input.txt|payload|call>");

        string source = ReadRawSource(args[0]);
        string? outputPath = GetOption(args, "-o", "--output");
        string xml = DecodeRawSourceToXml(source);

        if (!string.IsNullOrWhiteSpace(outputPath))
        {
            File.WriteAllText(Path.GetFullPath(outputPath), xml, new UTF8Encoding(false));
            Console.WriteLine($"Wrote: {Path.GetFullPath(outputPath)}");
        }
        else
        {
            Console.WriteLine(xml);
        }

        return 0;
    }

    private static void SaveXml(string path, XDocument doc)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "  ",
            Encoding = new UTF8Encoding(true),
            OmitXmlDeclaration = false
        };

        using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
        using var xw = XmlWriter.Create(fs, settings);
        doc.WriteTo(xw);
    }

    private static string? InferProgramNameFromCode(string code)
    {
        foreach (var rawLine in code.Split('\n'))
        {
            string line = rawLine.Trim();
            if (!line.StartsWith("Vz.Init(\"", StringComparison.Ordinal))
                continue;

            int firstQuote = line.IndexOf('"');
            int lastQuote = line.LastIndexOf('"');
            if (firstQuote >= 0 && lastQuote > firstQuote)
                return line.Substring(firstQuote + 1, lastQuote - firstQuote - 1);
        }

        return null;
    }

    private static string RemoveVizzyCodeSuffix(string path)
    {
        string fileName = Path.GetFileName(path);
        if (fileName.EndsWith(".vizzy.cs", StringComparison.OrdinalIgnoreCase))
            return Path.Combine(Path.GetDirectoryName(path)!, fileName[..^(".vizzy.cs".Length)]);
        return path;
    }

    private static Encoding DetectEncodingWithBom(string path)
    {
        using var stream = File.OpenRead(path);
        using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
        if (reader.Peek() >= 0) { }
        return reader.CurrentEncoding;
    }

    private static string? GetOption(string[] args, params string[] names)
    {
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (names.Contains(args[i], StringComparer.OrdinalIgnoreCase))
                return args[i + 1];
        }
        return null;
    }

    private static bool IsHelp(string arg)
    {
        return arg is "-h" or "--help" or "help";
    }

    private static int Fail(string message)
    {
        Console.Error.WriteLine(message);
        Console.Error.WriteLine();
        PrintUsage();
        return 1;
    }

    private static void PrintUsage()
    {
        Console.WriteLine("VizzyCode.Cli");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  import <input.xml> [-o output.vizzy.cs]");
        Console.WriteLine("  export <input.vizzy.cs> [-o output.xml] [-n programName]");
        Console.WriteLine("  roundtrip <input.xml> [-o output.xml] [-c output.vizzy.cs]");
        Console.WriteLine("  raw-encode <input.xml> [-o output.txt]");
        Console.WriteLine("  raw-decode <input.txt|payload|call> [-o output.xml]");
    }

    private static string ReadRawSource(string arg)
    {
        string candidatePath = Path.GetFullPath(arg);
        if (File.Exists(candidatePath))
            return File.ReadAllText(candidatePath, DetectEncodingWithBom(candidatePath)).Trim();

        return arg.Trim();
    }

    private static string DecodeRawSourceToXml(string source)
    {
        string trimmed = source.Trim();

        var rawCallMatch = Regex.Match(trimmed,
            @"^Vz\.(Raw(?:Xml)?(?:Eval|Constant|Variable|CraftProperty))\((.*)\)$",
            RegexOptions.Singleline);
        if (rawCallMatch.Success)
        {
            string fn = rawCallMatch.Groups[1].Value;
            string arg = rawCallMatch.Groups[2].Value.Trim();
            return fn.StartsWith("RawXml", StringComparison.Ordinal)
                ? DecodeStringLiteral(arg)
                : Encoding.UTF8.GetString(Convert.FromBase64String(DecodeStringLiteral(arg)));
        }

        if (trimmed.StartsWith("<", StringComparison.Ordinal))
            return trimmed;

        return Encoding.UTF8.GetString(Convert.FromBase64String(DecodeStringLiteral(trimmed)));
    }

    private static string ToVerbatimStringLiteral(string value)
    {
        return "@\"" + value.Replace("\"", "\"\"") + "\"";
    }

    private static string DecodeStringLiteral(string value)
    {
        string trimmed = value.Trim();
        if (trimmed.StartsWith("@\"", StringComparison.Ordinal) &&
            trimmed.EndsWith("\"", StringComparison.Ordinal) &&
            trimmed.Length >= 3)
        {
            return trimmed.Substring(2, trimmed.Length - 3).Replace("\"\"", "\"");
        }

        if (trimmed.StartsWith("\"", StringComparison.Ordinal) &&
            trimmed.EndsWith("\"", StringComparison.Ordinal) &&
            trimmed.Length >= 2)
        {
            return trimmed.Substring(1, trimmed.Length - 2)
                .Replace("\\\"", "\"")
                .Replace("\\\\", "\\")
                .Replace("\\n", "\n")
                .Replace("\\r", "\r")
                .Replace("\\t", "\t");
        }

        return trimmed;
    }
}
