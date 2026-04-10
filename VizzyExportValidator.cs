using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VizzyCode
{
    internal static class VizzyExportValidator
    {
        public static IReadOnlyList<string> Validate(XDocument doc)
        {
            var errors = new List<string>();

            if (doc.Root == null || doc.Root.Name.LocalName != "Program")
                errors.Add("Root element must be <Program>.");

            if (doc.Root != null && !doc.Root.Elements("Instructions").Any())
                errors.Add("Program must contain at least one top-level <Instructions> block.");

            foreach (var eval in doc.Descendants("EvaluateExpression"))
            {
                if (eval.Attribute("style")?.Value != "evaluate-expression")
                    errors.Add("Every <EvaluateExpression> must include style=\"evaluate-expression\".");
            }

            foreach (var elseNode in doc.Descendants("Else"))
                errors.Add("<Else> nodes are not allowed. Use <ElseIf style=\"else\"> with a true constant.");

            foreach (var elseIf in doc.Descendants("ElseIf").Where(x => x.Attribute("style")?.Value == "else"))
            {
                var condition = elseIf.Elements().FirstOrDefault(e => e.Name.LocalName != "Instructions");
                if (condition == null ||
                    condition.Name.LocalName != "Constant" ||
                    condition.Attribute("bool")?.Value != "true")
                {
                    errors.Add("<ElseIf style=\"else\"> must carry a leading <Constant bool=\"true\" /> condition.");
                }
            }

            if (doc.Root != null)
            {
                foreach (var instructions in doc.Root.Elements("Instructions"))
                {
                    var first = instructions.Elements().FirstOrDefault();
                    if (first == null)
                        continue;

                    string name = first.Name.LocalName;
                    if (name is "Event" or "CustomInstruction" or "CustomExpression" or "Comment")
                    {
                        if (first.Attribute("pos") == null)
                            errors.Add($"Top-level <{name}> nodes must include a pos attribute.");
                    }

                    if (name == "CustomInstruction")
                    {
                        RequireAttr(first, "callFormat", errors, name);
                        RequireAttr(first, "format", errors, name);
                        RequireAttr(first, "name", errors, name);
                    }

                    if (name == "CustomExpression")
                    {
                        RequireAttr(first, "callFormat", errors, name);
                        RequireAttr(first, "format", errors, name);
                        RequireAttr(first, "name", errors, name);
                    }

                    if (name == "Event")
                        RequireAttr(first, "event", errors, name);
                }
            }

            return errors;
        }

        public static string Format(IReadOnlyList<string> errors)
        {
            return string.Join(System.Environment.NewLine, errors.Select(e => $"- {e}"));
        }

        private static void RequireAttr(XElement el, string attr, List<string> errors, string nodeName)
        {
            if (string.IsNullOrWhiteSpace(el.Attribute(attr)?.Value))
                errors.Add($"Top-level <{nodeName}> nodes must include a non-empty {attr} attribute.");
        }
    }
}
