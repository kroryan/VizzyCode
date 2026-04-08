using System.Text;
using System.Xml;
using System.Xml.Linq;

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
        string code = isCraft ? converter.ConvertCraftToCode(doc) : converter.ConvertProgramToCode(doc);
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputPath))!);
        File.WriteAllText(outputPath, code, new UTF8Encoding(false));
        Console.WriteLine($"Imported: {inputPath}");
        Console.WriteLine($"Code: {Path.GetFullPath(outputPath)}");
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

        string code = File.ReadAllText(inputPath, new UTF8Encoding(false));
        string programName = explicitName
            ?? InferProgramNameFromCode(code)
            ?? Path.GetFileNameWithoutExtension(RemoveVizzyCodeSuffix(inputPath));

        var converter = new VizzyXmlConverter();
        var doc = converter.ConvertCodeToXml(code, programName);
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
        string code = converter.ConvertProgramToCode(xdoc);
        File.WriteAllText(codePath, code, new UTF8Encoding(false));
        var outputDoc = converter.ConvertCodeToXml(code);
        SaveXml(outputPath, outputDoc);

        bool sameBytes = File.ReadAllBytes(inputPath).SequenceEqual(File.ReadAllBytes(outputPath));
        Console.WriteLine($"Input XML:  {inputPath}");
        Console.WriteLine($"Code file:  {Path.GetFullPath(codePath)}");
        Console.WriteLine($"Output XML: {Path.GetFullPath(outputPath)}");
        Console.WriteLine($"same_bytes={sameBytes}");
        return sameBytes ? 0 : 2;
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
    }
}
