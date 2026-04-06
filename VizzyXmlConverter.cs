using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace VizzyCode
{
    /// <summary>
    /// Converts Vizzy XML programs to C# VizzyBuilder API code.
    /// Supports all block types from REWVIZZY (REWJUNO.dll).
    /// </summary>
    public class VizzyXmlConverter
    {
        private readonly HashSet<string> _listVariables = new();
        private readonly List<string> _warnings = new();
        public IReadOnlyList<string> Warnings => _warnings;

        // ── Public entry points ────────────────────────────────────────────────

        public string ConvertCraftToCode(XDocument doc)
        {
            var programs = doc.Descendants("Program").ToList();
            if (programs.Count == 0) return "// No Vizzy programs found in this craft file.";
            if (programs.Count == 1) return ConvertProgram(programs[0]);

            var sb = new StringBuilder();
            sb.AppendLine("// This craft contains multiple Vizzy programs.");
            sb.AppendLine();
            for (int i = 0; i < programs.Count; i++)
            {
                sb.AppendLine($"// ═══════════════════════════════════════════");
                sb.AppendLine($"// Program {i + 1}: {programs[i].Attribute("name")?.Value}");
                sb.AppendLine($"// ═══════════════════════════════════════════");
                sb.AppendLine();
                sb.AppendLine(ConvertProgram(programs[i]));
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public string ConvertProgramToCode(XDocument doc)
        {
            var program = doc.Root?.Name.LocalName == "Program"
                ? doc.Root
                : doc.Descendants("Program").FirstOrDefault();
            return program == null ? "// No <Program> element found." : ConvertProgram(program);
        }

        // ── Core conversion ────────────────────────────────────────────────────

        private string ConvertProgram(XElement program)
        {
            _listVariables.Clear();
            _warnings.Clear();

            string name = program.Attribute("name")?.Value ?? "MyProgram";
            var sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using REWJUNO;");
            sb.AppendLine("using REWVIZZY;");
            sb.AppendLine();
            sb.AppendLine($"// ── Program: {name} ──────────────────────────────────");
            sb.AppendLine($"Vz.Init(\"{Esc(name)}\");");
            sb.AppendLine();

            // Collect list variable names first
            var variables = program.Element("Variables");
            if (variables != null)
            {
                foreach (var v in variables.Elements("Variable"))
                    if (v.Element("Items") != null)
                        _listVariables.Add(v.Attribute("name")?.Value ?? "");
            }

            if (variables != null) EmitVariableDeclarations(variables, sb);

            var instructions = program.Element("Instructions");
            if (instructions != null)
            {
                EmitCustomInstructionDeclarations(instructions, sb);
                EmitInstructions(instructions.Elements(), sb, 0);
            }

            var expressions = program.Element("Expressions");
            if (expressions != null) EmitCustomExpressions(expressions, sb);

            sb.AppendLine();
            sb.AppendLine("// ── Serialize output ──");
            sb.AppendLine("Console.WriteLine(Vz.context.currentProgram.Serialize().ToString());");
            return sb.ToString();
        }

        // ── Variables ──────────────────────────────────────────────────────────

        private void EmitVariableDeclarations(XElement variables, StringBuilder sb)
        {
            var vars = variables.Elements("Variable").ToList();
            if (vars.Count == 0) return;
            sb.AppendLine("// ── Variables ────────────────────────────────────────");
            foreach (var v in vars)
            {
                string vname = v.Attribute("name")?.Value ?? "unnamed";
                bool isList = v.Element("Items") != null;
                string num = v.Attribute("number")?.Value ?? "0";
                if (num == "-0") num = "0";
                sb.AppendLine(isList
                    ? $"// var {San(vname)} = [];   // list"
                    : $"// var {San(vname)} = {num};");
            }
            sb.AppendLine();
        }

        // ── Custom Expressions ─────────────────────────────────────────────────

        private void EmitCustomExpressions(XElement expressions, StringBuilder sb)
        {
            var exprs = expressions.Elements("CustomExpression").ToList();
            if (exprs.Count == 0) return;
            sb.AppendLine("// ── Custom Expressions ───────────────────────────────");
            foreach (var expr in exprs)
            {
                string ename = expr.Attribute("name")?.Value ?? "MyExpr";
                var pnames = GetFormatParams(expr.Attribute("format")?.Value ?? "");
                if (pnames.Count == 0)
                    pnames = expr.Elements("Parameter").Select(p => San(p.Attribute("name")?.Value ?? "p")).ToList();
                string pStr = string.Join(", ", pnames.Select(p => $"\"{p}\""));
                string pCall = string.Join(", ", pnames);
                sb.AppendLine($"var {San(ename)} = Vz.DeclareCustomExpression(\"{Esc(ename)}\", {pStr}).SetReturn(({pCall}) =>");
                sb.AppendLine("{");
                var body = expr.Element("Instructions");
                if (body != null) EmitInstructions(body.Elements(), sb, 1);
                sb.AppendLine("    return 0; // TODO: add return expression");
                sb.AppendLine("});");
                sb.AppendLine();
            }
        }

        // ── Custom Instruction declarations ───────────────────────────────────

        private void EmitCustomInstructionDeclarations(XElement instructions, StringBuilder sb)
        {
            var customs = instructions.Elements("CustomInstruction").ToList();
            if (customs.Count == 0) return;
            sb.AppendLine("// ── Custom Instructions ──────────────────────────────");
            foreach (var ci in customs)
            {
                string ciname = ci.Attribute("name")?.Value ?? "MyInstr";
                var pnames = GetFormatParams(ci.Attribute("format")?.Value ?? "");
                if (pnames.Count == 0)
                    pnames = ci.Elements("Parameter").Select(p => San(p.Attribute("name")?.Value ?? "p")).ToList();
                string pStr = string.Join(", ", pnames.Select(p => $"\"{p}\""));
                string pCall = string.Join(", ", pnames);
                sb.AppendLine($"var {San(ciname)} = Vz.DeclareCustomInstruction(\"{Esc(ciname)}\", {pStr}).SetInstructions(({pCall}) =>");
                sb.AppendLine("{");
                var body = ci.Element("Instructions");
                if (body != null) EmitInstructions(body.Elements(), sb, 1);
                sb.AppendLine("});");
                sb.AppendLine();
            }
        }

        // ── Instructions ───────────────────────────────────────────────────────

        private void EmitInstructions(IEnumerable<XElement> elements, StringBuilder sb, int depth)
        {
            foreach (var el in elements) EmitInstruction(el, sb, depth);
        }

        private void EmitInstruction(XElement el, StringBuilder sb, int depth)
        {
            if (el.Name.LocalName == "CustomInstruction" && el.Attribute("pos") != null) return;

            string ind = I(depth);
            switch (el.Name.LocalName)
            {
                case "Comment":         EmitComment(el, sb, ind); break;
                case "Event":           EmitEvent(el, sb, depth, ind); break;
                case "SetVariable":     EmitSetVariable(el, sb, ind); break;
                case "ChangeVariable":  EmitChangeVariable(el, sb, ind); break;
                case "While":           EmitBlock("While", el, sb, depth, ind); break;
                case "If":              EmitBlock("If", el, sb, depth, ind); break;
                case "ElseIf":          EmitBlock("ElseIf", el, sb, depth, ind); break;
                case "Else":            EmitElse(el, sb, depth, ind); break;
                case "Repeat":          EmitBlockN("Repeat", el, sb, depth, ind); break;
                case "ForLoop":         EmitForLoop(el, sb, depth, ind); break;
                case "WaitSeconds":     sb.AppendLine($"{ind}Vz.WaitSeconds({E1(el)});"); break;
                case "WaitUntil":       sb.AppendLine($"{ind}using (new WaitUntil({E1(el)})) {{ }}"); break;
                case "Log":
                case "Display":
                case "DisplayMessage":  sb.AppendLine($"{ind}Vz.Log({E1(el)});"); break;
                case "FlightLog":       sb.AppendLine($"{ind}Vz.FlightLog({E1(el)});"); break;
                case "Break":           sb.AppendLine($"{ind}Vz.Break();"); break;
                case "Beep":            sb.AppendLine($"{ind}Vz.Beep();"); break;
                case "ActivateStage":   sb.AppendLine($"{ind}Vz.ActivateStage();"); break;
                case "SetThrottle":     sb.AppendLine($"{ind}Vz.SetThrottle({E1(el)});"); break;
                case "SetActivateGroup":
                case "SetActivationGroup": EmitSetAG(el, sb, ind); break;
                case "SetInput":        EmitSetInput(el, sb, ind); break;
                case "SetTargetHeading":
                case "LockHeading":     EmitLockHeading(el, sb, ind); break;
                case "SetTimeMode":     sb.AppendLine($"{ind}Vz.SetTimeMode({E1(el)});"); break;
                case "BroadcastMessage":EmitBroadcast(el, sb, ind); break;
                case "SwitchCraft":     sb.AppendLine($"{ind}Vz.SwitchCraft({E1(el)});"); break;
                case "SetTarget":       sb.AppendLine($"{ind}Vz.TargetNode({E1(el)});"); break;
                case "SetCamera":       sb.AppendLine($"{ind}Vz.SetCamera(\"{el.Attribute("property")?.Value ?? "zoom"}\", {E1(el)});"); break;
                case "SetPartProperty": EmitSetPartProperty(el, sb, ind); break;
                case "ListOp":
                case "InitList":        EmitListOp(el, sb, ind); break;
                case "UserInput":
                case "SetUserInput":    EmitUserInput(el, sb, ind); break;
                case "CallCustomInstruction": EmitCallCustomInstruction(el, sb, ind); break;
                // MFD
                case "CreateWidget":    EmitCreateWidget(el, sb, ind); break;
                case "DestroyWidget":   sb.AppendLine($"{ind}Vz.DestroyWidget({E1(el)});"); break;
                case "DestroyAllWidgets": sb.AppendLine($"{ind}Vz.DestroyAllWidgets();"); break;
                case "SetWidget":       EmitSetWidget(el, sb, ind); break;
                case "SetLabel":        EmitSetLabel(el, sb, ind); break;
                case "SetSprite":       EmitSetSprite(el, sb, ind); break;
                case "SetGauge":        EmitSetGauge(el, sb, ind); break;
                case "SetLine":         EmitSetLine(el, sb, ind); break;
                case "SetNavBall":      EmitSetNavBall(el, sb, ind); break;
                case "SetMap":          EmitSetMap(el, sb, ind); break;
                case "SetWidgetAnchor": EmitSetWidgetAnchor(el, sb, ind); break;
                case "SetWidgetEvent":  EmitSetWidgetEvent(el, sb, ind); break;
                case "SendWidgetToFront": sb.AppendLine($"{ind}Vz.SendWidgetToFront({E1(el)});"); break;
                case "SendWidgetToBack":  sb.AppendLine($"{ind}Vz.SendWidgetToBack({E1(el)});"); break;
                case "SetPixel":        EmitSetPixel(el, sb, ind); break;
                case "SetLinePoints":   EmitSetLinePoints(el, sb, ind); break;
                case "LockNavSphere":   EmitLockNavSphere(el, sb, ind); break;
                case "InitTexture":     sb.AppendLine($"{ind}Vz.InitTexture({E1(el)});"); break;
                default:
                    _warnings.Add($"Unknown instruction: {el.Name.LocalName}");
                    sb.AppendLine($"{ind}// [TODO] {el.Name.LocalName} {AttrSummary(el)}");
                    break;
            }
        }

        // ── Instruction emitters ───────────────────────────────────────────────

        private void EmitComment(XElement el, StringBuilder sb, string ind)
        {
            string text = el.Elements("Constant").FirstOrDefault()?.Attribute("text")?.Value ?? "";
            foreach (var line in text.Split('\n'))
                sb.AppendLine($"{ind}// {line.TrimEnd()}");
        }

        private void EmitEvent(XElement el, StringBuilder sb, int depth, string ind)
        {
            string ev = el.Attribute("event")?.Value ?? "";
            string open = ev switch
            {
                "FlightStart"    => $"{ind}using (new OnStart())",
                "FlightEnd"      => $"{ind}using (new OnEnd())",
                "ReceiveMessage" => $"{ind}using (new OnReceiveMessage(\"{Esc(el.Elements("Constant").FirstOrDefault()?.Attribute("text")?.Value ?? "msg")}\"))",
                "Collide"        => $"{ind}using (new OnPartCollision(\"{Esc(el.Attribute("part")?.Value ?? "part")}\"))",
                "Explode"        => $"{ind}using (new OnPartExplode(\"{Esc(el.Attribute("part")?.Value ?? "part")}\"))",
                "Docked"         => $"{ind}using (new OnDocked())",
                "ChangeSoi"      => $"{ind}using (new OnChangeSoi())",
                _                => $"{ind}using (new On(\"{Esc(ev)}\")) // [event: {ev}]"
            };

            var body = GetEventBody(el);
            sb.AppendLine(open);
            sb.AppendLine($"{ind}{{");
            if (body != null) EmitInstructions(body.Elements(), sb, depth + 1);
            sb.AppendLine($"{ind}}}");
            sb.AppendLine();
        }

        private void EmitBlock(string cls, XElement el, StringBuilder sb, int depth, string ind)
        {
            var ch = el.Elements().ToList();
            var cond = ch.FirstOrDefault(c => c.Name.LocalName != "Instructions");
            var body = ch.FirstOrDefault(c => c.Name.LocalName == "Instructions");
            string c = cond != null ? ConvertExpression(cond) : "true";
            sb.AppendLine($"{ind}using (new {cls}({c}))");
            sb.AppendLine($"{ind}{{");
            if (body != null) EmitInstructions(body.Elements(), sb, depth + 1);
            sb.AppendLine($"{ind}}}");
        }

        private void EmitElse(XElement el, StringBuilder sb, int depth, string ind)
        {
            var body = el.Element("Instructions");
            sb.AppendLine($"{ind}using (new Else())");
            sb.AppendLine($"{ind}{{");
            if (body != null) EmitInstructions(body.Elements(), sb, depth + 1);
            sb.AppendLine($"{ind}}}");
        }

        private void EmitBlockN(string cls, XElement el, StringBuilder sb, int depth, string ind)
        {
            var ch = el.Elements().ToList();
            var nEl = ch.FirstOrDefault(c => c.Name.LocalName != "Instructions");
            var body = ch.FirstOrDefault(c => c.Name.LocalName == "Instructions");
            string n = nEl != null ? ConvertExpression(nEl) : "1";
            sb.AppendLine($"{ind}using (new {cls}({n}))");
            sb.AppendLine($"{ind}{{");
            if (body != null) EmitInstructions(body.Elements(), sb, depth + 1);
            sb.AppendLine($"{ind}}}");
        }

        private void EmitForLoop(XElement el, StringBuilder sb, int depth, string ind)
        {
            var exprs = el.Elements().Where(c => c.Name.LocalName != "Instructions").ToList();
            var body = el.Element("Instructions");
            string vname = el.Attribute("variable")?.Value ?? "i";
            string from  = exprs.Count > 0 ? ConvertExpression(exprs[0]) : "0";
            string to    = exprs.Count > 1 ? ConvertExpression(exprs[1]) : "10";
            string step  = exprs.Count > 2 ? ConvertExpression(exprs[2]) : "1";
            sb.AppendLine($"{ind}using (new For(\"{San(vname)}\").From({from}).To({to}).By({step}))");
            sb.AppendLine($"{ind}{{");
            if (body != null) EmitInstructions(body.Elements(), sb, depth + 1);
            sb.AppendLine($"{ind}}}");
        }

        private void EmitSetVariable(XElement el, StringBuilder sb, string ind)
        {
            var ch = el.Elements().ToList();
            if (ch.Count < 2) { sb.AppendLine($"{ind}// [TODO] SetVariable: malformed"); return; }
            string varName = ch[0].Attribute("variableName")?.Value ?? "var";
            string value   = ConvertExpression(ch[1]);
            sb.AppendLine($"{ind}{San(varName)} = {value};");
        }

        private void EmitChangeVariable(XElement el, StringBuilder sb, string ind)
        {
            var ch = el.Elements().ToList();
            if (ch.Count < 2) return;
            string varName = ch[0].Attribute("variableName")?.Value ?? "var";
            sb.AppendLine($"{ind}{San(varName)} += {ConvertExpression(ch[1])};");
        }

        private void EmitSetAG(XElement el, StringBuilder sb, string ind)
        {
            var ch = el.Elements().ToList();
            string g = ch.Count > 0 ? ConvertExpression(ch[0]) : "1";
            string v = ch.Count > 1 ? ConvertExpression(ch[1]) : "true";
            sb.AppendLine($"{ind}Vz.SetActivationGroup({g}, {v});");
        }

        private void EmitSetInput(XElement el, StringBuilder sb, string ind)
        {
            string input = el.Attribute("input")?.Value ?? "Throttle";
            string val   = E1(el);
            sb.AppendLine($"{ind}Vz.SetInput(CraftInput.{Cap(input)}, {val});");
        }

        private void EmitLockHeading(XElement el, StringBuilder sb, string ind)
        {
            string prop = el.Attribute("property")?.Value ?? "Heading";
            string val  = E1(el);
            sb.AppendLine($"{ind}Vz.SetTargetHeading(TargetHeadingProperty.{Cap(prop)}, {val});");
        }

        private void EmitBroadcast(XElement el, StringBuilder sb, string ind)
        {
            bool global = el.Attribute("global")?.Value == "true";
            bool local  = el.Attribute("local")?.Value  == "true";
            var ch = el.Elements().ToList();
            string msg  = ch.Count > 0 ? ConvertExpression(ch[0]) : "\"msg\"";
            string data = ch.Count > 1 ? ConvertExpression(ch[1]) : "0";
            string type = global ? "AllCraft" : (local ? "Local" : "Craft");
            sb.AppendLine($"{ind}Vz.Broadcast(BroadCastType.{type}, {msg}, {data});");
        }

        private void EmitSetPartProperty(XElement el, StringBuilder sb, string ind)
        {
            string prop = el.Attribute("property")?.Value ?? "Activated";
            var ch = el.Elements().ToList();
            string part = ch.Count > 0 ? ConvertExpression(ch[0]) : "0";
            string val  = ch.Count > 1 ? ConvertExpression(ch[1]) : "true";
            sb.AppendLine($"{ind}Vz.SetPartProperty(PartPropertySetType.{Cap(prop)}, {part}, {val});");
        }

        private void EmitListOp(XElement el, StringBuilder sb, string ind)
        {
            string op = el.Attribute("op")?.Value ?? el.Attribute("type")?.Value ?? "Add";
            var ch = el.Elements().ToList();
            string list  = ch.Count > 0 ? ConvertExpression(ch[0]) : "list";
            string arg1  = ch.Count > 1 ? ConvertExpression(ch[1]) : "0";
            string arg2  = ch.Count > 2 ? ConvertExpression(ch[2]) : "0";
            string call = op.ToLower() switch
            {
                "add"     => $"Vz.ListAdd({list}, {arg1})",
                "insert"  => $"Vz.ListInsert({list}, {arg1}, {arg2})",
                "remove"  => $"Vz.ListRemove({list}, {arg1})",
                "set"     => $"Vz.ListSet({list}, {arg1}, {arg2})",
                "clear"   => $"Vz.ListClear({list})",
                "sort"    => $"Vz.ListSort({list})",
                "reverse" => $"Vz.ListReverse({list})",
                _         => $"Vz.List{Cap(op)}({list}, {arg1})"
            };
            sb.AppendLine($"{ind}{call};");
        }

        private void EmitUserInput(XElement el, StringBuilder sb, string ind)
        {
            var ch = el.Elements().ToList();
            string vname  = ch.Count > 0 ? (ch[0].Attribute("variableName")?.Value ?? "var") : "var";
            string prompt = ch.Count > 1 ? ConvertExpression(ch[1]) : "\"Enter value:\"";
            sb.AppendLine($"{ind}{San(vname)} = Vz.UserInput({prompt});");
        }

        private void EmitCallCustomInstruction(XElement el, StringBuilder sb, string ind)
        {
            string call = el.Attribute("call")?.Value ?? el.Attribute("name")?.Value ?? "instr";
            var args = el.Elements().Select(a => ConvertExpression(a));
            sb.AppendLine($"{ind}{San(call)}({string.Join(", ", args)});");
        }

        // MFD
        private void EmitCreateWidget(XElement el, StringBuilder sb, string ind)
        {
            string wtype = el.Attribute("widgetType")?.Value ?? "Rectangle";
            string wname = el.Attribute("name")?.Value ?? "widget1";
            sb.AppendLine($"{ind}Vz{Cap(wtype)} {San(wname)} = new Vz{Cap(wtype)}(\"{Esc(wname)}\");");
        }
        private void EmitSetWidget(XElement el, StringBuilder sb, string ind)
        {
            string prop = el.Attribute("property")?.Value ?? "Position";
            var ch = el.Elements().ToList();
            string widget = ch.Count > 0 ? ConvertExpression(ch[0]) : "widget";
            string val    = ch.Count > 1 ? ConvertExpression(ch[1]) : "0";
            sb.AppendLine($"{ind}{widget}.{Cap(prop)} = {val};");
        }
        private void EmitSetLabel(XElement el, StringBuilder sb, string ind) => EmitSetWidget(el, sb, ind);
        private void EmitSetSprite(XElement el, StringBuilder sb, string ind) => EmitSetWidget(el, sb, ind);
        private void EmitSetGauge(XElement el, StringBuilder sb, string ind) => EmitSetWidget(el, sb, ind);
        private void EmitSetLine(XElement el, StringBuilder sb, string ind) => EmitSetWidget(el, sb, ind);
        private void EmitSetNavBall(XElement el, StringBuilder sb, string ind) => EmitSetWidget(el, sb, ind);
        private void EmitSetMap(XElement el, StringBuilder sb, string ind) => EmitSetWidget(el, sb, ind);
        private void EmitSetWidgetAnchor(XElement el, StringBuilder sb, string ind)
        {
            var ch = el.Elements().ToList();
            string widget = ch.Count > 0 ? ConvertExpression(ch[0]) : "widget";
            string anchor = el.Attribute("anchor")?.Value ?? "Center";
            sb.AppendLine($"{ind}{widget}.Anchor = WidgetAnchor.{Cap(anchor)};");
        }
        private void EmitSetWidgetEvent(XElement el, StringBuilder sb, string ind)
        {
            string evtype = el.Attribute("eventType")?.Value ?? "PointerClick";
            var ch = el.Elements().ToList();
            string widget  = ch.Count > 0 ? ConvertExpression(ch[0]) : "widget";
            string handler = ch.Count > 1 ? ConvertExpression(ch[1]) : "\"handler\"";
            string data    = ch.Count > 2 ? ConvertExpression(ch[2]) : "0";
            sb.AppendLine($"{ind}{widget}.Subscribe(WidgetEventType.{Cap(evtype)}, {handler}, {data}, (d) => {{ }});");
        }
        private void EmitSetPixel(XElement el, StringBuilder sb, string ind)
        {
            var ch = el.Elements().ToList();
            string widget = ch.Count > 0 ? ConvertExpression(ch[0]) : "widget";
            string x = ch.Count > 1 ? ConvertExpression(ch[1]) : "0";
            string y = ch.Count > 2 ? ConvertExpression(ch[2]) : "0";
            string color = ch.Count > 3 ? ConvertExpression(ch[3]) : "\"#FFFFFF\"";
            sb.AppendLine($"{ind}{widget}.SetPixel({x}, {y}, {color});");
        }
        private void EmitSetLinePoints(XElement el, StringBuilder sb, string ind)
        {
            var ch = el.Elements().ToList();
            string widget = ch.Count > 0 ? ConvertExpression(ch[0]) : "widget";
            string pts    = ch.Count > 1 ? ConvertExpression(ch[1]) : "points";
            sb.AppendLine($"{ind}{widget}.SetPoints({pts});");
        }
        private void EmitLockNavSphere(XElement el, StringBuilder sb, string ind)
        {
            string type = el.Attribute("indicatorType")?.Value ?? "Prograde";
            sb.AppendLine($"{ind}Vz.LockNavSphere(LockNavSphereIndicatorType.{Cap(type)}, {E1(el)});");
        }

        // ── Expression converter ───────────────────────────────────────────────

        public string ConvertExpression(XElement el)
        {
            if (el == null) return "null";
            switch (el.Name.LocalName)
            {
                case "Constant":           return ConvertConstant(el);
                case "Variable":           return San(el.Attribute("variableName")?.Value ?? "var");
                case "BinaryOp":           return ConvertBinaryOp(el);
                case "BoolOp":             return ConvertBoolOp(el);
                case "Not":                return $"(!{E1(el)})";
                case "Comparison":         return ConvertComparison(el);
                case "UnaryOp":
                case "MathFunction":       return ConvertMathFunction(el);
                case "VectorOp":           return ConvertVectorOp(el);
                case "Vector":             return ConvertVectorNew(el);
                case "Conditional":        return ConvertConditional(el);
                case "CraftProperty":      return ConvertCraftProperty(el);
                case "CraftOtherProperty": return ConvertCraftOtherProperty(el);
                case "Planet":             return ConvertPlanetOp(el);
                case "EvaluateExpression": return ConvertEvalExpr(el);
                case "ActivationGroup":    return $"Vz.ActivationGroup({E1(el)})";
                case "CraftInput":         return ConvertCraftInputExpr(el);
                case "CallCustomExpression": return ConvertCallCustomExpr(el);
                case "ListExpression":     return ConvertListExpr(el);
                case "StringOp":           return ConvertStringOp(el);
                case "FUNKExpression":
                case "FUNK":               return ConvertFunk(el);
                case "TerrainProperty":    return ConvertTerrainProp(el);
                case "PartProperty":       return ConvertPartProp(el);
                case "PartLocalToPci":     return ConvertPartLocalToPci(el);
                case "PartPciToLocal":     return ConvertPartPciToLocal(el);
                case "PartNameToID":       return $"Vz.PartNameToID({E1(el)})";
                case "Raycast":            return ConvertRaycast(el);
                case "LatLongAglToPosition": return ConvertLatLongToPos(el);
                case "PositionToLatLongAgl": return ConvertPosToLatLong(el);
                case "GetWidgetProperty":  return ConvertGetWidgetProp(el);
                case "GetWidgetEventMsg":  return $"Vz.GetWidgetEventMsg({E1(el)})";
                case "GetPixel":           return ConvertGetPixel(el);
                case "HexColor":           return $"Vz.HexColor({E1(el)})";
                default:
                    _warnings.Add($"Unknown expression: {el.Name.LocalName}");
                    return $"/* {el.Name.LocalName} */0";
            }
        }

        // ── Expression helpers ─────────────────────────────────────────────────

        private string ConvertConstant(XElement el)
        {
            string? style = el.Attribute("style")?.Value;
            string? boolVal = el.Attribute("bool")?.Value;
            if (style == "true" || boolVal == "true")  return "true";
            if (style == "false" || boolVal == "false") return "false";
            var numAttr = el.Attribute("number");
            if (numAttr != null) { string nv = numAttr.Value; return nv == "-0" ? "0" : nv; }
            var textAttr = el.Attribute("text");
            if (textAttr != null)
            {
                string tv = textAttr.Value;
                if (double.TryParse(tv, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out _)) return tv;
                return $"\"{Esc(tv)}\"";
            }
            return "0";
        }

        private string ConvertBinaryOp(XElement el)
        {
            string op = el.Attribute("op")?.Value ?? "+";
            var ch = el.Elements().ToList();
            string l = ch.Count > 0 ? ConvertExpression(ch[0]) : "0";
            string r = ch.Count > 1 ? ConvertExpression(ch[1]) : "0";
            string cs = op switch { "+" => "+", "-" => "-", "*" => "*", "/" => "/",
                "^" => "^", "%" => "%", "Rand" => "/* rand */", "Min" => "/* min */",
                "Max" => "/* max */", "ATan2" => "/* atan2 */", _ => op };
            // Special cases that aren't infix
            if (op == "Rand")  return $"Vz.Random({l}, {r})";
            if (op == "Min")   return $"Vz.Min({l}, {r})";
            if (op == "Max")   return $"Vz.Max({l}, {r})";
            if (op == "ATan2") return $"Vz.Atan2({l}, {r})";
            return $"({l} {cs} {r})";
        }

        private string ConvertBoolOp(XElement el)
        {
            string op = el.Attribute("op")?.Value ?? "and";
            var ch = el.Elements().ToList();
            string l = ch.Count > 0 ? ConvertExpression(ch[0]) : "true";
            string r = ch.Count > 1 ? ConvertExpression(ch[1]) : "true";
            return op == "or" ? $"({l} || {r})" : $"({l} && {r})";
        }

        private string ConvertComparison(XElement el)
        {
            string op = el.Attribute("op")?.Value ?? "=";
            var ch = el.Elements().ToList();
            string l = ch.Count > 0 ? ConvertExpression(ch[0]) : "0";
            string r = ch.Count > 1 ? ConvertExpression(ch[1]) : "0";
            string cs = op switch { "=" => "==", "g" => ">", "l" => "<", "ge" => ">=",
                "le" => "<=", "ne" => "!=", ">" => ">", "<" => "<", ">=" => ">=",
                "<=" => "<=", "!=" => "!=", _ => op };
            return $"({l} {cs} {r})";
        }

        private string ConvertMathFunction(XElement el)
        {
            string fn = el.Attribute("function")?.Value ?? el.Attribute("op")?.Value ?? "abs";
            var ch = el.Elements().ToList();
            string a = ch.Count > 0 ? ConvertExpression(ch[0]) : "0";
            string b = ch.Count > 1 ? ConvertExpression(ch[1]) : "0";
            return fn switch
            {
                "abs" => $"Vz.Abs({a})", "sqrt" => $"Vz.Sqrt({a})",
                "floor" => $"Vz.Floor({a})", "ceiling" or "ceil" => $"Vz.Ceiling({a})",
                "round" => $"Vz.Round({a})", "sin" => $"Vz.Sin({a})",
                "cos" => $"Vz.Cos({a})", "tan" => $"Vz.Tan({a})",
                "asin" => $"Vz.Asin({a})", "acos" => $"Vz.Acos({a})",
                "atan" => $"Vz.Atan({a})", "atan2" => $"Vz.Atan2({a}, {b})",
                "ln" => $"Vz.Ln({a})", "log" => $"Vz.Log10({a})",
                "deg2rad" => $"Vz.Deg2Rad({a})", "rad2deg" => $"Vz.Rad2Deg({a})",
                _ => $"Vz.Math(\"{fn}\", {a})"
            };
        }

        private string ConvertVectorOp(XElement el)
        {
            string op = el.Attribute("op")?.Value ?? "length";
            var ch = el.Elements().ToList();
            string a = ch.Count > 0 ? ConvertExpression(ch[0]) : "v";
            string b = ch.Count > 1 ? ConvertExpression(ch[1]) : "v";
            return op switch
            {
                "length" or "magnitude" => $"Vz.Length({a})",
                "norm"                  => $"Vz.Norm({a})",
                "dot"                   => $"Vz.Dot({a}, {b})",
                "cross"                 => $"Vz.Cross({a}, {b})",
                "angle"                 => $"Vz.Angle({a}, {b})",
                "distance"              => $"Vz.Distance({a}, {b})",
                "project"               => $"Vz.Project({a}, {b})",
                "scale"                 => $"Vz.Scale({a}, {b})",
                "min"                   => $"Vz.VecMin({a}, {b})",
                "max"                   => $"Vz.VecMax({a}, {b})",
                "clamp"                 => $"Vz.Clamp({a}, {b})",
                "x" or "X"              => $"{a}.x",
                "y" or "Y"              => $"{a}.y",
                "z" or "Z"              => $"{a}.z",
                _                       => $"Vz.VecOp(\"{op}\", {a})"
            };
        }

        private string ConvertVectorNew(XElement el)
        {
            var ch = el.Elements().ToList();
            string x = ch.Count > 0 ? ConvertExpression(ch[0]) : "0";
            string y = ch.Count > 1 ? ConvertExpression(ch[1]) : "0";
            string z = ch.Count > 2 ? ConvertExpression(ch[2]) : "0";
            return $"Vz.Vec({x}, {y}, {z})";
        }

        private string ConvertConditional(XElement el)
        {
            var ch = el.Elements().ToList();
            string c = ch.Count > 0 ? ConvertExpression(ch[0]) : "true";
            string t = ch.Count > 1 ? ConvertExpression(ch[1]) : "0";
            string e = ch.Count > 2 ? ConvertExpression(ch[2]) : "0";
            return $"({c} ? {t} : {e})";
        }

        // CraftProperty – maps "Category.Property" dot notation to API calls
        private string ConvertCraftProperty(XElement el)
        {
            string prop = el.Attribute("property")?.Value ?? "";
            var ch = el.Elements().ToList();
            string arg = ch.Count > 0 ? ConvertExpression(ch[0]) : "";
            string craftArg = ch.Count > 0 ? $"({arg})" : "()";

            return prop switch
            {
                // Altitude
                "Altitude.AGL"                    => "Vz.Craft.AltitudeAGL()",
                "Altitude.ASL"                    => "Vz.Craft.AltitudeASL()",
                "Altitude.ASF"                    => "Vz.Craft.AltitudeASF()",
                // Orbit
                "Orbit.Apoapsis"                  => "Vz.Craft.Orbit.Apoapsis()",
                "Orbit.Periapsis"                 => "Vz.Craft.Orbit.Periapsis()",
                "Orbit.TimeToApoapsis"            => "Vz.Craft.Orbit.TimeToAp()",
                "Orbit.TimeToPeriapsis"           => "Vz.Craft.Orbit.TimeToPe()",
                "Orbit.Eccentricity"              => "Vz.Craft.Orbit.Eccentricity()",
                "Orbit.Inclination"               => "Vz.Craft.Orbit.Inclination()",
                "Orbit.Period"                    => "Vz.Craft.Orbit.Period()",
                "Orbit.Planet"                    => "Vz.Craft.Orbit.Planet()",
                "Orbit.SemiMajorAxis"             => "Vz.Craft.Orbit.SemiMajorAxis()",
                "Orbit.MeanAnomaly"               => "Vz.Craft.Orbit.MeanAnomaly()",
                "Orbit.TrueAnomaly"               => "Vz.Craft.Orbit.TrueAnomaly()",
                "Orbit.MeanMotion"                => "Vz.Craft.Orbit.MeanMotion()",
                "Orbit.PeriapsisArgument"         => "Vz.Craft.Orbit.PeriapsisArgument()",
                "Orbit.RightAscension"            => "Vz.Craft.Orbit.RightAscension()",
                "Orbit.ApoapsisTime"              => "Vz.Craft.Orbit.ApoapsisTime()",
                "Orbit.PeriapsisTime"             => "Vz.Craft.Orbit.PeriapsisTime()",
                // Atmosphere
                "Atmosphere.AirDensity"           => "Vz.Craft.Atmosphere.AirDensity()",
                "Atmosphere.AirPressure"          => "Vz.Craft.Atmosphere.AirPressure()",
                "Atmosphere.SpeedOfSound"         => "Vz.Craft.Atmosphere.SpeedOfSound()",
                "Atmosphere.Temperature"          => "Vz.Craft.Atmosphere.Temperature()",
                // Performance
                "Performance.Mass"                => "Vz.Craft.Performance.Mass()",
                "Performance.DryMass"             => "Vz.Craft.Performance.DryMass()",
                "Performance.FuelMass"            => "Vz.Craft.Performance.FuelMass()",
                "Performance.Thrust"              => "Vz.Craft.Performance.Thrust()",
                "Performance.MaxActiveEngineThrust" => "Vz.Craft.Performance.MaxThrust()",
                "Performance.CurrentEngineThrust" => "Vz.Craft.Performance.CurrentThrust()",
                "Performance.TWR"                 => "Vz.Craft.Performance.TWR()",
                "Performance.CurrentIsp"          => "Vz.Craft.Performance.ISP()",
                "Performance.StageDeltaV"         => "Vz.Craft.Performance.StageDeltaV()",
                "Performance.BurnTime"            => "Vz.Craft.Performance.BurnTime()",
                // Fuel
                "Fuel.Battery"                    => "Vz.Craft.Fuel.Battery()",
                "Fuel.FuelInStage"                => "Vz.Craft.Fuel.FuelInStage()",
                "Fuel.Mono"                       => "Vz.Craft.Fuel.Mono()",
                "Fuel.AllStages"                  => "Vz.Craft.Fuel.AllStages()",
                // Navigation
                "Nav.Position"                    => "Vz.Craft.Nav.Position()",
                "Nav.CraftHeading"                => "Vz.Craft.Nav.Heading()",
                "Nav.Pitch"                       => "Vz.Craft.Nav.Pitch()",
                "Nav.BankAngle"                   => "Vz.Craft.Nav.BankAngle()",
                "Nav.AngleOfAttack"               => "Vz.Craft.Nav.AngleOfAttack()",
                "Nav.SideSlip"                    => "Vz.Craft.Nav.SideSlip()",
                "Nav.North"                       => "Vz.Craft.Nav.North()",
                "Nav.East"                        => "Vz.Craft.Nav.East()",
                "Nav.CraftDirection"              => "Vz.Craft.Nav.Direction()",
                "Nav.CraftRight"                  => "Vz.Craft.Nav.Right()",
                "Nav.CraftUp"                     => "Vz.Craft.Nav.Up()",
                // Velocity
                "Vel.SurfaceVelocity"             => "Vz.Craft.Velocity.Surface()",
                "Vel.OrbitVelocity"               => "Vz.Craft.Velocity.Orbital()",
                "Vel.Gravity"                     => "Vz.Craft.Velocity.Gravity()",
                "Vel.Drag"                        => "Vz.Craft.Velocity.Drag()",
                "Vel.Acceleration"                => "Vz.Craft.Velocity.Acceleration()",
                "Vel.AngularVelocity"             => "Vz.Craft.Velocity.Angular()",
                "Vel.LateralSurfaceVelocity"      => "Vz.Craft.Velocity.LateralSurface()",
                "Vel.VerticalSurfaceVelocity"     => "Vz.Craft.Velocity.VerticalSurface()",
                "Vel.MachNumber"                  => "Vz.Craft.Velocity.MachNumber()",
                // Target
                "Target.Position"                 => "Vz.Craft.Target.Position()",
                "Target.Velocity"                 => "Vz.Craft.Target.Velocity()",
                "Target.Name"                     => "Vz.Craft.Target.Name()",
                "Target.Planet"                   => "Vz.Craft.Target.Planet()",
                // Misc
                "Misc.Grounded"                   => "Vz.Craft.Grounded()",
                "Misc.Splashed"                   => "Vz.Craft.Splashed()",
                "Misc.NumStages"                  => "Vz.Craft.NumStages()",
                "Misc.SolarRadiation"             => "Vz.Craft.SolarRadiation()",
                "Misc.CameraPosition"             => "Vz.Craft.Camera.Position()",
                "Misc.CameraPointing"             => "Vz.Craft.Camera.Pointing()",
                "Misc.CameraUp"                   => "Vz.Craft.Camera.Up()",
                "Misc.PidPitch"                   => "Vz.Craft.PID.Pitch()",
                "Misc.PidRoll"                    => "Vz.Craft.PID.Roll()",
                // Time
                "Time.FrameDeltaTime"             => "Vz.Time.DeltaTime()",
                "Time.TimeSinceLaunch"            => "Vz.Time.MissionTime()",
                "Time.TotalTime"                  => "Vz.Time.TotalTime()",
                "Time.WarpAmount"                 => "Vz.Time.WarpAmount()",
                // Name
                "Name.Craft"                      => "Vz.NameOf(Vz.Craft.Self)",
                // Craft queries (take an argument)
                "Craft.NameToID"                  => $"Vz.Craft.NameToID({arg})",
                "Craft.Planet"                    => "Vz.Craft.Planet()",
                // Part queries
                "Part.NameToID"                   => $"Vz.PartNameToID({arg})",
                "Part.PciToLocal"                 => $"Vz.PartPciToLocal({arg})",
                "Part.SetActivated"               => $"Vz.SetPartActivated({arg})",
                // Input
                "Input.Throttle"                  => "Vz.CraftInput(CraftInput.Throttle)",
                "Input.TranslationMode"           => "Vz.CraftInput(CraftInput.TranslateMode)",
                // Legacy/simple
                "heading"                         => "Vz.Craft.Nav.Heading()",
                "pitch"                           => "Vz.Craft.Nav.Pitch()",
                _                                 => $"Vz.Craft.Property(\"{Esc(prop)}\")"
            };
        }

        private string ConvertCraftOtherProperty(XElement el)
        {
            string prop = el.Attribute("property")?.Value ?? "Altitude";
            var ch = el.Elements().ToList();
            string craftId = ch.Count > 0 ? ConvertExpression(ch[0]) : "craftId";
            return $"Vz.OtherCraft({craftId}).{Cap(prop)}()";
        }

        private string ConvertPlanetOp(XElement el)
        {
            string op = el.Attribute("op")?.Value ?? "";
            var ch = el.Elements().ToList();
            string planet = ch.Count > 0 ? ConvertExpression(ch[0]) : "\"planet\"";
            string arg2   = ch.Count > 1 ? ConvertExpression(ch[1]) : "";
            return op switch
            {
                "mass"              => $"Vz.Planet({planet}).Mass()",
                "radius"            => $"Vz.Planet({planet}).Radius()",
                "atmosphereHeight"  => $"Vz.Planet({planet}).AtmosphereDepth()",
                "soiradius"         => $"Vz.Planet({planet}).SOI()",
                "solarPosition"     => $"Vz.Planet({planet}).SolarPosition()",
                "childPlanets"      => $"Vz.Planet({planet}).ChildPlanets()",
                "crafts"            => $"Vz.Planet({planet}).Crafts()",
                "craftids"          => $"Vz.Planet({planet}).CraftIDs()",
                "parent"            => $"Vz.Planet({planet}).Parent()",
                "day"               => $"Vz.Planet({planet}).DayLength()",
                "year"              => $"Vz.Planet({planet}).YearLength()",
                "velocity"          => $"Vz.Planet({planet}).Velocity()",
                "apoapsis"          => $"Vz.Planet({planet}).Apoapsis()",
                "periapsis"         => $"Vz.Planet({planet}).Periapsis()",
                "period"            => $"Vz.Planet({planet}).Period()",
                "inclination"       => $"Vz.Planet({planet}).Inclination()",
                "eccentricity"      => $"Vz.Planet({planet}).Eccentricity()",
                "meananomaly"       => $"Vz.Planet({planet}).MeanAnomaly()",
                "semimajoraxis"     => $"Vz.Planet({planet}).SemiMajorAxis()",
                "trueanomaly"       => $"Vz.Planet({planet}).TrueAnomaly()",
                "toLatLongAgl"      => $"Vz.PosToLatLongAgl({planet}, {(string.IsNullOrEmpty(arg2) ? "Vz.Craft.Nav.Position()" : arg2)})",
                _ => $"Vz.Planet({planet}).Op(\"{Esc(op)}\")"
            };
        }

        private string ConvertEvalExpr(XElement el)
        {
            var child = el.Elements().FirstOrDefault();
            string expr = child?.Attribute("text")?.Value ?? child?.Attribute("number")?.Value ?? "";
            return expr.ToLower() switch { "pi" => "Vz.Pi", "e" => "Vz.E",
                "inf" or "infinity" => "Vz.Infinity", "nan" => "Vz.NaN",
                _ => $"Vz.Eval(\"{Esc(expr)}\")" };
        }

        private string ConvertCraftInputExpr(XElement el)
        {
            string input = el.Attribute("input")?.Value ?? "Throttle";
            return $"Vz.CraftInput(CraftInput.{Cap(input)})";
        }

        private string ConvertCallCustomExpr(XElement el)
        {
            string call = el.Attribute("call")?.Value ?? "expr";
            var args = el.Elements().Select(a => ConvertExpression(a));
            return $"{San(call)}({string.Join(", ", args)})";
        }

        private string ConvertListExpr(XElement el)
        {
            string op = el.Attribute("op")?.Value ?? "Get";
            var ch = el.Elements().ToList();
            string list = ch.Count > 0 ? ConvertExpression(ch[0]) : "list";
            string idx  = ch.Count > 1 ? ConvertExpression(ch[1]) : "0";
            return op.ToLower() switch
            {
                "get"    => $"Vz.ListGet({list}, {idx})",
                "length" => $"Vz.ListLength({list})",
                "index"  => $"Vz.ListIndex({list}, {idx})",
                "create" => $"Vz.CreateList()",
                _        => $"Vz.List{Cap(op)}({list})"
            };
        }

        private string ConvertStringOp(XElement el)
        {
            string op = el.Attribute("op")?.Value ?? "Join";
            var ch = el.Elements().ToList();
            string a = ch.Count > 0 ? ConvertExpression(ch[0]) : "\"\"";
            string b = ch.Count > 1 ? ConvertExpression(ch[1]) : "\"\"";
            string c = ch.Count > 2 ? ConvertExpression(ch[2]) : "0";
            return op.ToLower() switch
            {
                "join"      => $"Vz.Join({a}, {b})",
                "length"    => $"Vz.LengthOf({a})",
                "letter"    => $"Vz.LetterOf({a}, {b})",
                "substring" => $"Vz.SubString({a}, {b}, {c})",
                "contains"  => $"Vz.Contains({a}, {b})",
                "format"    => $"Vz.Format({a}, {b})",
                _           => $"Vz.StringOp(\"{op}\", {a}, {b})"
            };
        }

        private string ConvertFunk(XElement el)
        {
            string text = el.Attribute("expression")?.Value ?? el.Attribute("text")?.Value ?? el.Value;
            return $"(FUNK)\"{Esc(text)}\"";
        }

        private string ConvertTerrainProp(XElement el)
        {
            string prop = el.Attribute("property")?.Value ?? "Height";
            string pos  = E1(el);
            return $"Vz.Terrain(TerrainPropertyType.{Cap(prop)}, {pos})";
        }

        private string ConvertPartProp(XElement el)
        {
            string prop = el.Attribute("property")?.Value ?? "Mass";
            string part = E1(el);
            return $"Vz.Part(PartPropertyGetType.{Cap(prop)}, {part})";
        }

        private string ConvertPartLocalToPci(XElement el)
        {
            var ch = el.Elements().ToList();
            string part = ch.Count > 0 ? ConvertExpression(ch[0]) : "0";
            string pos  = ch.Count > 1 ? ConvertExpression(ch[1]) : "Vz.Vec(0,0,0)";
            return $"Vz.PartLocalToPci({part}, {pos})";
        }

        private string ConvertPartPciToLocal(XElement el)
        {
            var ch = el.Elements().ToList();
            string part = ch.Count > 0 ? ConvertExpression(ch[0]) : "0";
            string pos  = ch.Count > 1 ? ConvertExpression(ch[1]) : "Vz.Vec(0,0,0)";
            return $"Vz.PartPciToLocal({part}, {pos})";
        }

        private string ConvertRaycast(XElement el)
        {
            var ch = el.Elements().ToList();
            string origin = ch.Count > 0 ? ConvertExpression(ch[0]) : "Vz.Vec(0,0,0)";
            string dir    = ch.Count > 1 ? ConvertExpression(ch[1]) : "Vz.Vec(0,0,1)";
            return $"Vz.Raycast({origin}, {dir})";
        }

        private string ConvertLatLongToPos(XElement el)
        {
            var ch = el.Elements().ToList();
            string planet = ch.Count > 0 ? ConvertExpression(ch[0]) : "\"planet\"";
            string latLon = ch.Count > 1 ? ConvertExpression(ch[1]) : "Vz.Vec(0,0,0)";
            return $"Vz.LatLongAglToPosition({planet}, {latLon})";
        }

        private string ConvertPosToLatLong(XElement el)
        {
            var ch = el.Elements().ToList();
            string planet = ch.Count > 0 ? ConvertExpression(ch[0]) : "\"planet\"";
            string pos    = ch.Count > 1 ? ConvertExpression(ch[1]) : "Vz.Craft.Nav.Position()";
            return $"Vz.PositionToLatLongAgl({planet}, {pos})";
        }

        private string ConvertGetWidgetProp(XElement el)
        {
            string prop = el.Attribute("property")?.Value ?? "Position";
            string widget = E1(el);
            return $"{widget}.Get{Cap(prop)}()";
        }

        private string ConvertGetPixel(XElement el)
        {
            var ch = el.Elements().ToList();
            string widget = ch.Count > 0 ? ConvertExpression(ch[0]) : "widget";
            string x = ch.Count > 1 ? ConvertExpression(ch[1]) : "0";
            string y = ch.Count > 2 ? ConvertExpression(ch[2]) : "0";
            return $"{widget}.GetPixel({x}, {y})";
        }

        // ── Event body helper ──────────────────────────────────────────────────

        private XElement? GetEventBody(XElement eventEl)
        {
            var inner = eventEl.Element("Instructions");
            if (inner != null) return inner;
            var siblings = eventEl.Parent?.Elements().ToList();
            if (siblings == null) return null;
            int idx = siblings.IndexOf(eventEl);
            if (idx < 0) return null;
            var body = new List<XElement>();
            for (int i = idx + 1; i < siblings.Count; i++)
            {
                if (siblings[i].Name.LocalName == "Event") break;
                body.Add(siblings[i]);
            }
            return body.Count == 0 ? null : new XElement("Instructions", body);
        }

        // ── Tiny helpers ───────────────────────────────────────────────────────

        private string E1(XElement el)
        {
            var c = el.Elements().FirstOrDefault();
            return c != null ? ConvertExpression(c) : "0";
        }

        private static string I(int depth) => new string(' ', depth * 4);
        private static string Esc(string s) => s.Replace("\\","\\\\").Replace("\"","\\\"").Replace("\n","\\n").Replace("\r","");
        private static string San(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "_var";
            var sb = new StringBuilder();
            foreach (char c in name) sb.Append(char.IsLetterOrDigit(c) || c == '_' ? c : '_');
            string r = sb.ToString();
            return r.Length > 0 && char.IsDigit(r[0]) ? "_" + r : r;
        }
        private static string Cap(string s) => string.IsNullOrEmpty(s) ? s : char.ToUpper(s[0]) + s.Substring(1);
        private static string AttrSummary(XElement el) => string.Join(" ", el.Attributes().Select(a => $"{a.Name.LocalName}=\"{a.Value}\""));
        private static List<string> GetFormatParams(string fmt)
        {
            var r = new List<string>(); int i = 0;
            while (i < fmt.Length)
            {
                int s = fmt.IndexOf('|', i); if (s < 0) break;
                int e = fmt.IndexOf('|', s + 1); if (e < 0) break;
                r.Add(San(fmt.Substring(s + 1, e - s - 1))); i = e + 1;
            }
            return r;
        }

        // ── Convert Code to XML ────────────────────────────────────────────────

        public XDocument ConvertCodeToXml(string code, string programName = "GeneratedProgram")
        {
            var lines = code.Split('\n').Select(l => l.TrimEnd()).ToList();
            var program = new XElement("Program", new XAttribute("name", programName));

            var variables = new XElement("Variables");
            var instructions = new XElement("Instructions");

            string currentVariable = null;
            var currentInstructionElements = new List<XElement>();

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                if (line.StartsWith("//")) continue;

                if (line.StartsWith("using (new "))
                {
                    if (currentInstructionElements.Count > 0)
                    {
                        instructions.Add(currentInstructionElements);
                        currentInstructionElements.Clear();
                    }

                    var blockMatch = System.Text.RegularExpressions.Regex.Match(line, @"using\s+\(new\s+(\w+)\((.*)\)\)");
                    if (blockMatch.Success)
                    {
                        string blockType = blockMatch.Groups[1].Value;
                        string blockArgs = blockMatch.Groups[2].Value;

                        var block = ConvertBlockToXml(blockType, blockArgs, lines, ref i);
                        if (block != null)
                        {
                            instructions.Add(block);
                        }
                    }
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(line, @"(?<![=!<>])=(?!=)"))
                {
                    var assignMatch = System.Text.RegularExpressions.Regex.Match(line, @"(\w+)\s*=\s*(.+);");
                    if (assignMatch.Success)
                    {
                        string varName = assignMatch.Groups[1].Value;
                        string varValue = assignMatch.Groups[2].Value;
                        
                        if (varValue.StartsWith("Vz."))
                        {
                            var setVar = ConvertSetVariableToXml(varName, varValue);
                            if (setVar != null)
                            {
                                currentInstructionElements.Add(setVar);
                            }
                        }
                        else
                        {
                            var varEl = new XElement("Variable", new XAttribute("name", varName));
                            var constant = new XElement("Constant");
                            if (double.TryParse(varValue, out double num))
                            {
                                constant.Add(new XAttribute("number", num));
                            }
                            else
                            {
                                constant.Add(new XAttribute("text", varValue.Trim('"')));
                            }
                            varEl.Add(constant);
                            variables.Add(varEl);
                        }
                    }
                }
                else if (line.StartsWith("Vz."))
                {
                    var instruction = ConvertInstructionToXml(line);
                    if (instruction != null)
                    {
                        currentInstructionElements.Add(instruction);
                    }
                }
            }

            if (currentInstructionElements.Count > 0)
            {
                instructions.Add(currentInstructionElements);
            }

            if (variables.HasElements) program.Add(variables);
            if (instructions.HasElements) program.Add(instructions);

            return new XDocument(new XElement("Assembly", new XAttribute("name", "VizzyAssembly"), program));
        }

        private XElement? ConvertBlockToXml(string blockType, string blockArgs, List<string> lines, ref int index)
        {
            var bodyLines = new List<string>();
            int depth = 0;
            bool inBlock = false;

            for (int i = index + 1; i < lines.Count; i++)
            {
                string line = lines[i].Trim();
                if (line == "{")
                {
                    inBlock = true;
                    depth++;
                    continue;
                }
                if (line == "}")
                {
                    depth--;
                    if (depth == 0)
                    {
                        index = i;
                        break;
                    }
                    continue;
                }
                if (inBlock && !string.IsNullOrWhiteSpace(line))
                {
                    bodyLines.Add(line);
                }
            }

            var body = new XElement("Instructions");
            int bi = 0;
            while (bi < bodyLines.Count)
            {
                string bodyLine = bodyLines[bi];
                var nestedMatch = System.Text.RegularExpressions.Regex.Match(bodyLine, @"using\s+\(new\s+(\w+)\((.*)\)\)");
                if (nestedMatch.Success)
                {
                    string nestedType = nestedMatch.Groups[1].Value;
                    string nestedArgs = nestedMatch.Groups[2].Value;
                    // Collect nested block body lines (look ahead for { ... })
                    var nestedBodyLines = new List<string>();
                    int nd = 0; bool inNested = false;
                    int nEnd = bi;
                    for (int ni = bi + 1; ni < bodyLines.Count; ni++)
                    {
                        string nl = bodyLines[ni].Trim();
                        if (nl == "{") { inNested = true; nd++; continue; }
                        if (nl == "}") { nd--; if (nd == 0) { nEnd = ni; break; } continue; }
                        if (inNested) nestedBodyLines.Add(nl);
                    }
                    // Build nested block using same parsing
                    var allNestedLines = new List<string>(bodyLines);
                    // Re-use ConvertInstructionToXml for simple nested lines
                    var nestedBody = new XElement("Instructions");
                    foreach (var nl2 in nestedBodyLines)
                    {
                        var ni2 = ConvertInstructionToXml(nl2);
                        if (ni2 != null) nestedBody.Add(ni2);
                    }
                    var nestedBlock = BuildBlockElement(nestedType, nestedArgs, nestedBody);
                    if (nestedBlock != null) body.Add(nestedBlock);
                    bi = nEnd + 1;
                    continue;
                }
                var instruction = ConvertInstructionToXml(bodyLine);
                if (instruction != null) body.Add(instruction);
                bi++;
            }

            return BuildBlockElement(blockType, blockArgs, body);
        }

        private XElement? BuildBlockElement(string blockType, string blockArgs, XElement body)
        {
            // Parse "OnReceiveMessage("msg")" style args
            string condArg = string.IsNullOrWhiteSpace(blockArgs) ? "0" : blockArgs.Trim('"', ' ');
            return blockType.ToLower() switch
            {
                "onstart" => new XElement("Event", new XAttribute("event", "FlightStart"), body),
                "onend" => new XElement("Event", new XAttribute("event", "FlightEnd"), body),
                "onreceivemessage" => new XElement("Event", new XAttribute("event", "ReceiveMessage"),
                    new XElement("Constant", new XAttribute("text", condArg)), body),
                "ondocked" => new XElement("Event", new XAttribute("event", "Docked"), body),
                "onchangesoi" => new XElement("Event", new XAttribute("event", "ChangeSoi"), body),
                "onpartcollision" => new XElement("Event", new XAttribute("event", "Collide"),
                    new XAttribute("part", condArg), body),
                "onpartexplode" => new XElement("Event", new XAttribute("event", "Explode"),
                    new XAttribute("part", condArg), body),
                "if" => new XElement("If", new XElement("Constant", new XAttribute("number", condArg)), body),
                "elseif" => new XElement("ElseIf", new XElement("Constant", new XAttribute("number", condArg)), body),
                "else" => new XElement("Else", body),
                "while" => new XElement("While", new XElement("Constant", new XAttribute("number", condArg)), body),
                "waituntil" => new XElement("WaitUntil", new XElement("Constant", new XAttribute("number", condArg)), body),
                "repeat" => new XElement("Repeat", new XElement("Constant", new XAttribute("number", condArg)), body),
                "for" => ParseForBlock(blockArgs, body),
                _ => null
            };
        }

        private XElement ParseForBlock(string blockArgs, XElement body)
        {
            // blockArgs may be "\"i\".From(0).To(10).By(1)" or just the variable name
            string variable = "i";
            string from = "0", to = "10", step = "1";
            var varMatch = System.Text.RegularExpressions.Regex.Match(blockArgs, "\"(\\w+)\"");
            if (varMatch.Success) variable = varMatch.Groups[1].Value;
            var fromMatch = System.Text.RegularExpressions.Regex.Match(blockArgs, @"\.From\(([^)]+)\)");
            if (fromMatch.Success) from = fromMatch.Groups[1].Value;
            var toMatch = System.Text.RegularExpressions.Regex.Match(blockArgs, @"\.To\(([^)]+)\)");
            if (toMatch.Success) to = toMatch.Groups[1].Value;
            var byMatch = System.Text.RegularExpressions.Regex.Match(blockArgs, @"\.By\(([^)]+)\)");
            if (byMatch.Success) step = byMatch.Groups[1].Value;
            return new XElement("ForLoop", new XAttribute("variable", variable),
                new XElement("Constant", new XAttribute("number", from)),
                new XElement("Constant", new XAttribute("number", to)),
                new XElement("Constant", new XAttribute("number", step)),
                body);
        }

        private XElement? ConvertSetVariableToXml(string varName, string varValue)
        {
            var setVar = new XElement("SetVariable");
            
            var varRef = new XElement("Variable", new XAttribute("variableName", varName));
            setVar.Add(varRef);

            if (varValue.StartsWith("Vz."))
            {
                var expr = ConvertApiCallToXml(varValue);
                if (expr != null)
                {
                    setVar.Add(expr);
                }
            }
            else
            {
                var constant = new XElement("Constant");
                if (double.TryParse(varValue, out double num))
                {
                    constant.Add(new XAttribute("number", num));
                }
                else
                {
                    constant.Add(new XAttribute("text", varValue.Trim('"')));
                }
                setVar.Add(constant);
            }

            return setVar;
        }

        private XElement? ConvertInstructionToXml(string line)
        {
            // Strip trailing semicolon for matching
            string l = line.TrimEnd(';', ' ');

            // Zero-argument instructions
            if (l == "Vz.ActivateStage()") return new XElement("ActivateStage");
            if (l == "Vz.Break()") return new XElement("Break");
            if (l == "Vz.Beep()") return new XElement("Beep");
            if (l == "Vz.DestroyAllWidgets()") return new XElement("DestroyAllWidgets");

            // One-argument instructions
            string c1 = ExtractParenthesisContent(l);
            if (l.StartsWith("Vz.Log(") || l.StartsWith("Vz.Display(") || l.StartsWith("Vz.DisplayMessage("))
                return new XElement("Log", MakeConstantArg(c1));
            if (l.StartsWith("Vz.FlightLog("))
                return new XElement("FlightLog", MakeConstantArg(c1));
            if (l.StartsWith("Vz.WaitSeconds("))
                return new XElement("WaitSeconds", MakeConstantArg(c1));
            if (l.StartsWith("Vz.SetThrottle("))
                return new XElement("SetThrottle", MakeConstantArg(c1));
            if (l.StartsWith("Vz.SetTimeMode("))
                return new XElement("SetTimeMode", MakeConstantArg(c1));
            if (l.StartsWith("Vz.SwitchCraft("))
                return new XElement("SwitchCraft", MakeConstantArg(c1));
            if (l.StartsWith("Vz.TargetNode("))
                return new XElement("SetTarget", MakeConstantArg(c1));
            if (l.StartsWith("Vz.DestroyWidget("))
                return new XElement("DestroyWidget", MakeConstantArg(c1));
            if (l.StartsWith("Vz.SendWidgetToFront("))
                return new XElement("SendWidgetToFront", MakeConstantArg(c1));
            if (l.StartsWith("Vz.SendWidgetToBack("))
                return new XElement("SendWidgetToBack", MakeConstantArg(c1));
            if (l.StartsWith("Vz.InitTexture("))
                return new XElement("InitTexture", MakeConstantArg(c1));

            // SetCamera(prop, val)
            if (l.StartsWith("Vz.SetCamera("))
            {
                var parts = SplitArgs(c1);
                string prop = parts.Count > 0 ? parts[0].Trim('"', ' ') : "zoom";
                string val  = parts.Count > 1 ? parts[1] : "0";
                return new XElement("SetCamera", new XAttribute("property", prop), MakeConstantArg(val));
            }

            // SetActivationGroup(group, val)
            if (l.StartsWith("Vz.SetActivationGroup("))
            {
                var parts = SplitArgs(c1);
                var el = new XElement("SetActivationGroup");
                el.Add(MakeConstantArg(parts.Count > 0 ? parts[0] : "1"));
                el.Add(MakeConstantArg(parts.Count > 1 ? parts[1] : "true"));
                return el;
            }

            // SetInput(CraftInput.X, val)
            if (l.StartsWith("Vz.SetInput("))
            {
                var parts = SplitArgs(c1);
                string input = parts.Count > 0 ? parts[0].Replace("CraftInput.", "") : "Throttle";
                var el = new XElement("SetInput", new XAttribute("input", input));
                el.Add(MakeConstantArg(parts.Count > 1 ? parts[1] : "0"));
                return el;
            }

            // SetTargetHeading(TargetHeadingProperty.X, val)
            if (l.StartsWith("Vz.SetTargetHeading("))
            {
                var parts = SplitArgs(c1);
                string prop = parts.Count > 0 ? parts[0].Replace("TargetHeadingProperty.", "") : "Heading";
                var el = new XElement("SetTargetHeading", new XAttribute("property", prop));
                el.Add(MakeConstantArg(parts.Count > 1 ? parts[1] : "0"));
                return el;
            }

            // Broadcast(BroadCastType.X, msg, data)
            if (l.StartsWith("Vz.Broadcast("))
            {
                var parts = SplitArgs(c1);
                string type  = parts.Count > 0 ? parts[0].Replace("BroadCastType.", "") : "Craft";
                bool global  = type == "AllCraft";
                bool local   = type == "Local";
                var el = new XElement("BroadcastMessage");
                if (global) el.Add(new XAttribute("global", "true"));
                if (local)  el.Add(new XAttribute("local", "true"));
                el.Add(MakeConstantArg(parts.Count > 1 ? parts[1] : "\"msg\""));
                el.Add(MakeConstantArg(parts.Count > 2 ? parts[2] : "0"));
                return el;
            }

            // SetPartProperty(PartPropertySetType.X, part, val)
            if (l.StartsWith("Vz.SetPartProperty("))
            {
                var parts = SplitArgs(c1);
                string prop = parts.Count > 0 ? parts[0].Replace("PartPropertySetType.", "") : "Activated";
                var el = new XElement("SetPartProperty", new XAttribute("property", prop));
                el.Add(MakeConstantArg(parts.Count > 1 ? parts[1] : "0"));
                el.Add(MakeConstantArg(parts.Count > 2 ? parts[2] : "true"));
                return el;
            }

            // List operations
            if (l.StartsWith("Vz.ListAdd("))    return MakeListOp("Add",    SplitArgs(c1));
            if (l.StartsWith("Vz.ListInsert(")) return MakeListOp("Insert", SplitArgs(c1));
            if (l.StartsWith("Vz.ListRemove(")) return MakeListOp("Remove", SplitArgs(c1));
            if (l.StartsWith("Vz.ListSet("))    return MakeListOp("Set",    SplitArgs(c1));
            if (l.StartsWith("Vz.ListClear("))  return MakeListOp("Clear",  SplitArgs(c1));
            if (l.StartsWith("Vz.ListSort("))   return MakeListOp("Sort",   SplitArgs(c1));
            if (l.StartsWith("Vz.ListReverse("))return MakeListOp("Reverse",SplitArgs(c1));

            // UserInput: var = Vz.UserInput(prompt) — handled via assignment, but also as statement
            // Generic Vz. call fallback — treat as comment
            if (l.StartsWith("Vz."))
                return new XElement("Comment",
                    new XElement("Constant", new XAttribute("text", $"[TODO] {l}")));

            return null;
        }

        private XElement MakeConstantArg(string val)
        {
            val = val.Trim();
            if (double.TryParse(val, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double num))
                return new XElement("Constant", new XAttribute("number", num));
            if (val.StartsWith("\"") && val.EndsWith("\""))
                return new XElement("Constant", new XAttribute("text", val.Trim('"')));
            if (val == "true" || val == "false")
                return new XElement("Constant", new XAttribute("bool", val));
            // Variable reference
            return new XElement("Variable", new XAttribute("variableName", val));
        }

        private XElement MakeListOp(string op, List<string> parts)
        {
            var el = new XElement("ListOp", new XAttribute("op", op));
            foreach (var p in parts) el.Add(MakeConstantArg(p));
            return el;
        }

        private List<string> SplitArgs(string argsStr)
        {
            var result = new List<string>();
            int depth = 0;
            var cur = new StringBuilder();
            foreach (char c in argsStr)
            {
                if (c == '(' || c == '[') depth++;
                else if (c == ')' || c == ']') depth--;
                else if (c == ',' && depth == 0)
                {
                    result.Add(cur.ToString().Trim());
                    cur.Clear();
                    continue;
                }
                cur.Append(c);
            }
            if (cur.Length > 0) result.Add(cur.ToString().Trim());
            return result;
        }

        private XElement? ConvertApiCallToXml(string call)
        {
            call = call.Trim().TrimEnd(';');

            // Numeric literal
            if (double.TryParse(call, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double numVal))
                return new XElement("Constant", new XAttribute("number", numVal));

            // String literal
            if (call.StartsWith("\"") && call.EndsWith("\""))
                return new XElement("Constant", new XAttribute("text", call.Trim('"')));

            // Boolean
            if (call == "true") return new XElement("Constant", new XAttribute("bool", "true"));
            if (call == "false") return new XElement("Constant", new XAttribute("bool", "false"));

            // Variable reference (plain identifier)
            if (System.Text.RegularExpressions.Regex.IsMatch(call, @"^[A-Za-z_]\w*$"))
                return new XElement("Variable", new XAttribute("variableName", call));

            // BinaryOp: (a op b)
            var binMatch = System.Text.RegularExpressions.Regex.Match(call, @"^\((.+)\s([+\-*/%^])\s(.+)\)$");
            if (binMatch.Success)
            {
                var el = new XElement("BinaryOp", new XAttribute("op", binMatch.Groups[2].Value));
                el.Add(ConvertApiCallToXml(binMatch.Groups[1].Value));
                el.Add(ConvertApiCallToXml(binMatch.Groups[3].Value));
                return el;
            }

            // Vz.Vec(x, y, z)
            if (call.StartsWith("Vz.Vec("))
            {
                var parts = SplitArgs(ExtractParenthesisContent(call));
                var el = new XElement("Vector");
                foreach (var p in parts) { var c = ConvertApiCallToXml(p); if (c != null) el.Add(c); }
                return el;
            }

            // Vz.Craft.AltitudeASL() → CraftProperty property="Altitude.ASL"
            if (call.StartsWith("Vz.Craft."))
            {
                string prop = call.Substring(9).TrimEnd(')').TrimEnd('(');
                // Map known method names back to CraftProperty values
                string xmlProp = prop switch
                {
                    "AltitudeAGL()" => "Altitude.AGL",
                    "AltitudeASL()" => "Altitude.ASL",
                    "AltitudeASF()" => "Altitude.ASF",
                    "Grounded()" => "Misc.Grounded",
                    "Splashed()" => "Misc.Splashed",
                    "NumStages()" => "Misc.NumStages",
                    "SolarRadiation()" => "Misc.SolarRadiation",
                    "Nav.Heading()" => "Nav.CraftHeading",
                    "Nav.Pitch()" => "Nav.Pitch",
                    "Nav.Position()" => "Nav.Position",
                    "Velocity.Surface()" => "Vel.SurfaceVelocity",
                    "Velocity.Orbital()" => "Vel.OrbitVelocity",
                    "Performance.Mass()" => "Performance.Mass",
                    "Orbit.Apoapsis()" => "Orbit.Apoapsis",
                    "Orbit.Periapsis()" => "Orbit.Periapsis",
                    "Fuel.Battery()" => "Fuel.Battery",
                    _ => prop.TrimEnd('(', ')')
                };
                return new XElement("CraftProperty", new XAttribute("property", xmlProp));
            }

            // Vz.Sqrt(x) → MathFunction
            var mathFns = new[] { "Abs","Sqrt","Floor","Ceiling","Round","Sin","Cos","Tan",
                "Asin","Acos","Atan","Ln","Log10","Deg2Rad","Rad2Deg" };
            foreach (var fn in mathFns)
            {
                if (call.StartsWith($"Vz.{fn}("))
                {
                    var el = new XElement("MathFunction", new XAttribute("function", fn.ToLower()));
                    var c = ConvertApiCallToXml(ExtractParenthesisContent(call));
                    if (c != null) el.Add(c);
                    return el;
                }
            }

            // Fallback: constant 0
            return new XElement("Constant", new XAttribute("number", "0"));
        }

        private string ExtractParenthesisContent(string line)
        {
            int start = line.IndexOf('(') + 1;
            int end = line.LastIndexOf(')');
            if (start > 0 && end > start)
            {
                return line.Substring(start, end - start).Trim('"');
            }
            return "";
        }
    }
}
