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

        private static readonly Dictionary<string, string> CraftPropertyCallMap = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Vz.Craft.AltitudeAGL()"] = "Altitude.AGL",
            ["Vz.Craft.AltitudeASL()"] = "Altitude.ASL",
            ["Vz.Craft.Altitude()"] = "Altitude.ASL",
            ["Vz.Craft.AltitudeASF()"] = "Altitude.ASF",
            ["Vz.Craft.Orbit.Apoapsis()"] = "Orbit.Apoapsis",
            ["Vz.Craft.Orbit.Periapsis()"] = "Orbit.Periapsis",
            ["Vz.Craft.Orbit.TimeToAp()"] = "Orbit.TimeToApoapsis",
            ["Vz.Craft.Orbit.TimeToApoapsis()"] = "Orbit.TimeToApoapsis",
            ["Vz.Craft.Orbit.TimeToPe()"] = "Orbit.TimeToPeriapsis",
            ["Vz.Craft.Orbit.TimeToPeriapsis()"] = "Orbit.TimeToPeriapsis",
            ["Vz.Craft.Orbit.Eccentricity()"] = "Orbit.Eccentricity",
            ["Vz.Craft.Orbit.Inclination()"] = "Orbit.Inclination",
            ["Vz.Craft.Orbit.Period()"] = "Orbit.Period",
            ["Vz.Craft.Orbit.Planet()"] = "Orbit.Planet",
            ["Vz.Craft.Orbit.SemiMajorAxis()"] = "Orbit.SemiMajorAxis",
            ["Vz.Craft.Orbit.MeanAnomaly()"] = "Orbit.MeanAnomaly",
            ["Vz.Craft.Orbit.TrueAnomaly()"] = "Orbit.TrueAnomaly",
            ["Vz.Craft.Orbit.MeanMotion()"] = "Orbit.MeanMotion",
            ["Vz.Craft.Orbit.PeriapsisArgument()"] = "Orbit.PeriapsisArgument",
            ["Vz.Craft.Orbit.RightAscension()"] = "Orbit.RightAscension",
            ["Vz.Craft.Atmosphere.AirDensity()"] = "Atmosphere.AirDensity",
            ["Vz.Craft.Atmosphere.AirPressure()"] = "Atmosphere.AirPressure",
            ["Vz.Craft.Atmosphere.SpeedOfSound()"] = "Atmosphere.SpeedOfSound",
            ["Vz.Craft.Atmosphere.Temperature()"] = "Atmosphere.Temperature",
            ["Vz.Craft.Performance.Mass()"] = "Performance.Mass",
            ["Vz.Craft.Performance.DryMass()"] = "Performance.DryMass",
            ["Vz.Craft.Performance.FuelMass()"] = "Performance.FuelMass",
            ["Vz.Craft.Performance.Thrust()"] = "Performance.Thrust",
            ["Vz.Craft.Performance.MaxThrust()"] = "Performance.MaxActiveEngineThrust",
            ["Vz.Craft.Performance.CurrentThrust()"] = "Performance.CurrentEngineThrust",
            ["Vz.Craft.Performance.TWR()"] = "Performance.TWR",
            ["Vz.Craft.Performance.ISP()"] = "Performance.CurrentIsp",
            ["Vz.Craft.Performance.StageDeltaV()"] = "Performance.StageDeltaV",
            ["Vz.Craft.Performance.BurnTime()"] = "Performance.BurnTime",
            ["Vz.Craft.Fuel.Battery()"] = "Fuel.Battery",
            ["Vz.Craft.Fuel.FuelInStage()"] = "Fuel.FuelInStage",
            ["Vz.Craft.Fuel.Mono()"] = "Fuel.Mono",
            ["Vz.Craft.Fuel.AllStages()"] = "Fuel.AllStages",
            ["Vz.Craft.Nav.Position()"] = "Nav.Position",
            ["Vz.Craft.Nav.Heading()"] = "Nav.CraftHeading",
            ["Vz.Craft.Nav.Pitch()"] = "Nav.Pitch",
            ["Vz.Craft.Nav.BankAngle()"] = "Nav.BankAngle",
            ["Vz.Craft.Nav.AngleOfAttack()"] = "Nav.AngleOfAttack",
            ["Vz.Craft.Nav.SideSlip()"] = "Nav.SideSlip",
            ["Vz.Craft.Nav.North()"] = "Nav.North",
            ["Vz.Craft.Nav.East()"] = "Nav.East",
            ["Vz.Craft.Nav.Direction()"] = "Nav.CraftDirection",
            ["Vz.Craft.Nav.Right()"] = "Nav.CraftRight",
            ["Vz.Craft.Nav.Up()"] = "Nav.CraftUp",
            ["Vz.Craft.Velocity.Surface()"] = "Vel.SurfaceVelocity",
            ["Vz.Craft.Velocity.Orbital()"] = "Vel.OrbitVelocity",
            ["Vz.Craft.Velocity.Gravity()"] = "Vel.Gravity",
            ["Vz.Craft.Velocity.Drag()"] = "Vel.Drag",
            ["Vz.Craft.Velocity.Acceleration()"] = "Vel.Acceleration",
            ["Vz.Craft.Velocity.Angular()"] = "Vel.AngularVelocity",
            ["Vz.Craft.Velocity.LateralSurface()"] = "Vel.LateralSurfaceVelocity",
            ["Vz.Craft.Velocity.VerticalSurface()"] = "Vel.VerticalSurfaceVelocity",
            ["Vz.Craft.Velocity.MachNumber()"] = "Vel.MachNumber",
            ["Vz.Craft.Target.Position()"] = "Target.Position",
            ["Vz.Craft.Target.Velocity()"] = "Target.Velocity",
            ["Vz.Craft.Target.Name()"] = "Target.Name",
            ["Vz.Craft.Target.Planet()"] = "Target.Planet",
            ["Vz.Craft.Grounded()"] = "Misc.Grounded",
            ["Vz.Craft.Splashed()"] = "Misc.Splashed",
            ["Vz.Craft.NumStages()"] = "Misc.NumStages",
            ["Vz.Craft.SolarRadiation()"] = "Misc.SolarRadiation",
            ["Vz.Craft.Camera.Position()"] = "Misc.CameraPosition",
            ["Vz.Craft.Camera.Pointing()"] = "Misc.CameraPointing",
            ["Vz.Craft.Camera.Up()"] = "Misc.CameraUp",
            ["Vz.Craft.PID.Pitch()"] = "Misc.PidPitch",
            ["Vz.Craft.PID.Roll()"] = "Misc.PidRoll",
            ["Vz.Time.DeltaTime()"] = "Time.FrameDeltaTime",
            ["Vz.Time.MissionTime()"] = "Time.TimeSinceLaunch",
            ["Vz.Time.TotalTime()"] = "Time.TotalTime",
            ["Vz.Time.WarpAmount()"] = "Time.WarpAmount",
            ["Vz.Craft.Planet()"] = "Craft.Planet"
        };

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
                case "For":
                case "ForLoop":         EmitForLoop(el, sb, depth, ind); break;
                case "WaitSeconds":     sb.AppendLine($"{ind}Vz.WaitSeconds({E1(el)});"); break;
                case "WaitUntil":       sb.AppendLine($"{ind}using (new WaitUntil({E1(el)})) {{ }}"); break;
                case "LogMessage":
                case "Log":
                case "Display":
                case "DisplayMessage":  sb.AppendLine($"{ind}Vz.Log({E1(el)});"); break;
                case "LogFlight":
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
                case "SetCameraProperty":
                case "SetCamera":       sb.AppendLine($"{ind}Vz.SetCamera(\"{el.Attribute("property")?.Value ?? "zoom"}\", {E1(el)});"); break;
                case "SetPartProperty": EmitSetPartProperty(el, sb, ind); break;
                case "ListOp":
                case "SetList":
                case "InitList":        EmitListOp(el, sb, ind); break;
                case "UserInput":
                case "SetUserInput":    EmitUserInput(el, sb, ind); break;
                case "CallCustomInstruction": EmitCallCustomInstruction(el, sb, ind); break;
                // MFD
                case "CreateWidget":    EmitCreateWidget(el, sb, ind); break;
                case "DestroyWidget":   sb.AppendLine($"{ind}Vz.DestroyWidget({E1(el)});"); break;
                case "DestroyAllWidgets": sb.AppendLine($"{ind}Vz.DestroyAllWidgets();"); break;
                case "SetCraftProperty": EmitLegacySetCraftProperty(el, sb, ind); break;
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

        private void EmitLegacySetCraftProperty(XElement el, StringBuilder sb, string ind)
        {
            string prop = el.Attribute("property")?.Value ?? "";
            var ch = el.Elements().ToList();

            if (prop.StartsWith("Part.Set", StringComparison.Ordinal))
            {
                string partProp = prop.Substring("Part.Set".Length);
                var normalized = new XElement("SetPartProperty", new XAttribute("property", partProp), ch);
                EmitSetPartProperty(normalized, sb, ind);
                return;
            }

            if (prop == "Sound.Beep")
            {
                string f = ch.Count > 0 ? ConvertExpression(ch[0]) : "440";
                string v = ch.Count > 1 ? ConvertExpression(ch[1]) : "1";
                string t = ch.Count > 2 ? ConvertExpression(ch[2]) : "0.2";
                sb.AppendLine($"{ind}Vz.Beep({f}, {v}, {t});");
                return;
            }

            if (prop.StartsWith("Mfd.Create.", StringComparison.Ordinal))
            {
                string widgetType = prop.Substring("Mfd.Create.".Length);
                string widgetName = ch.Count > 0 ? trimQuotes(ConvertExpression(ch[0])) : "widget";
                sb.AppendLine($"{ind}Vz{Cap(widgetType)} {San(widgetName)} = new Vz{Cap(widgetType)}(\"{Esc(widgetName)}\");");
                return;
            }

            if (prop.StartsWith("Mfd.Set", StringComparison.Ordinal))
            {
                string widgetProp = prop.Substring("Mfd.Set".Length);
                var normalized = new XElement("SetWidget", new XAttribute("property", widgetProp), ch);
                EmitSetWidget(normalized, sb, ind);
                return;
            }

            if (prop.StartsWith("Mfd.Widget.SetAnchor.", StringComparison.Ordinal))
            {
                string anchor = prop.Substring("Mfd.Widget.SetAnchor.".Length);
                var normalized = new XElement("SetWidgetAnchor", new XAttribute("anchor", anchor), ch);
                EmitSetWidgetAnchor(normalized, sb, ind);
                return;
            }

            if (prop.StartsWith("Mfd.Label.SetAlignment.", StringComparison.Ordinal))
            {
                string alignment = prop.Substring("Mfd.Label.SetAlignment.".Length);
                string widget = ch.Count > 0 ? ConvertExpression(ch[0]) : "widget";
                sb.AppendLine($"{ind}{widget}.Alignment = Alignment.{Cap(alignment)};");
                return;
            }

            if (prop.StartsWith("Mfd.Label.Set", StringComparison.Ordinal))
            {
                string labelProp = prop.Substring("Mfd.Label.Set".Length);
                var normalized = new XElement("SetLabel", new XAttribute("property", labelProp), ch);
                EmitSetLabel(normalized, sb, ind);
                return;
            }

            if (prop.StartsWith("Mfd.Texture.Initialize", StringComparison.Ordinal))
            {
                var normalized = new XElement("InitTexture", ch);
                sb.AppendLine($"{ind}Vz.InitTexture({string.Join(", ", normalized.Elements().Select(ConvertExpression))});");
                return;
            }

            if (prop.StartsWith("Mfd.Texture.SetPixel", StringComparison.Ordinal))
            {
                var normalized = new XElement("SetPixel", ch);
                EmitSetPixel(normalized, sb, ind);
                return;
            }

            if (prop.StartsWith("Mfd.Sprite.Set", StringComparison.Ordinal))
            {
                string spriteProp = prop.Substring("Mfd.Sprite.Set".Length);
                var normalized = new XElement("SetSprite", new XAttribute("property", spriteProp), ch);
                EmitSetSprite(normalized, sb, ind);
                return;
            }

            if (prop.StartsWith("Mfd.Gauge.Set", StringComparison.Ordinal))
            {
                string gaugeProp = prop.Substring("Mfd.Gauge.Set".Length);
                var normalized = new XElement("SetGauge", new XAttribute("property", gaugeProp), ch);
                EmitSetGauge(normalized, sb, ind);
                return;
            }

            if (prop.StartsWith("Mfd.Line.SetLinePoints", StringComparison.Ordinal))
            {
                var normalized = new XElement("SetLinePoints", ch);
                EmitSetLinePoints(normalized, sb, ind);
                return;
            }

            if (prop.StartsWith("Mfd.Line.Set", StringComparison.Ordinal))
            {
                string lineProp = prop.Substring("Mfd.Line.Set".Length);
                var normalized = new XElement("SetLine", new XAttribute("property", lineProp), ch);
                EmitSetLine(normalized, sb, ind);
                return;
            }

            if (prop.StartsWith("Mfd.Navball.", StringComparison.Ordinal))
            {
                string navProp = prop.Substring("Mfd.Navball.".Length);
                var normalized = new XElement("SetNavBall", new XAttribute("property", navProp), ch);
                EmitSetNavBall(normalized, sb, ind);
                return;
            }

            if (prop.StartsWith("Mfd.Map.", StringComparison.Ordinal))
            {
                string mapProp = prop.Substring("Mfd.Map.".Length);
                var normalized = new XElement("SetMap", new XAttribute("property", mapProp), ch);
                EmitSetMap(normalized, sb, ind);
                return;
            }

            if (prop == "Mfd.Order.SendToFront")
            {
                var normalized = new XElement("SendWidgetToFront", ch);
                sb.AppendLine($"{ind}Vz.SendWidgetToFront({string.Join(", ", normalized.Elements().Select(ConvertExpression))});");
                return;
            }

            if (prop == "Mfd.Order.SendToBack")
            {
                var normalized = new XElement("SendWidgetToBack", ch);
                sb.AppendLine($"{ind}Vz.SendWidgetToBack({string.Join(", ", normalized.Elements().Select(ConvertExpression))});");
                return;
            }

            if (prop.StartsWith("Mfd.Event.Set", StringComparison.Ordinal))
            {
                string eventType = prop.Substring("Mfd.Event.Set".Length);
                var normalized = new XElement("SetWidgetEvent", new XAttribute("eventType", eventType), ch);
                EmitSetWidgetEvent(normalized, sb, ind);
                return;
            }

            if (prop == "Mfd.Destroy")
            {
                sb.AppendLine($"{ind}Vz.DestroyWidget({string.Join(", ", ch.Select(ConvertExpression))});");
                return;
            }

            if (prop == "Mfd.Destroy.All")
            {
                sb.AppendLine($"{ind}Vz.DestroyAllWidgets();");
                return;
            }

            sb.AppendLine($"{ind}// [TODO] Unsupported SetCraftProperty {prop}");
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
                case "ListExpression":
                case "ListOp":             return ConvertListExpr(el);
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
            var declaredVariables = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i].Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//")) continue;

                if (line.StartsWith("using (new "))
                {
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

                        bool isExpressionAssignment = IsVizzyExpression(varValue);
                        if (isExpressionAssignment || declaredVariables.Contains(varName))
                        {
                            var setVar = ConvertSetVariableToXml(varName, varValue);
                            if (setVar != null)
                            {
                                instructions.Add(setVar);
                            }
                        }
                        else
                        {
                            variables.Add(CreateVariableDeclaration(varName, varValue));
                            declaredVariables.Add(varName);
                        }
                    }
                }
                else if (line.StartsWith("Vz."))
                {
                    var instruction = ConvertInstructionToXml(line);
                    if (instruction != null)
                    {
                        instructions.Add(instruction);
                    }
                }
            }

            program.Add(variables);
            program.Add(instructions);
            program.Add(new XElement("Expressions"));

            return new XDocument(program);
        }

        private XElement? ConvertBlockToXml(string blockType, string blockArgs, List<string> lines, ref int index)
        {
            var body = new XElement("Instructions");
            int start = FindBlockStart(lines, index + 1);
            if (start >= 0)
            {
                int bodyIndex = start;
                ParseInstructionLines(lines, ref bodyIndex, body, stopAtClosingBrace: true);
                index = bodyIndex;
            }

            return BuildBlockElement(blockType, blockArgs, body);
        }

        private XElement? BuildBlockElement(string blockType, string blockArgs, XElement body)
        {
            string condArg = string.IsNullOrWhiteSpace(blockArgs) ? "0" : blockArgs.Trim();
            return blockType.ToLower() switch
            {
                "onstart" => new XElement("Event", new XAttribute("event", "FlightStart"), new XAttribute("style", "flight-start"), body),
                "onend" => new XElement("Event", new XAttribute("event", "FlightEnd"), body),
                "onreceivemessage" => new XElement("Event", new XAttribute("event", "ReceiveMessage"),
                    new XAttribute("style", "receive-msg"), CreateConstant(trimQuotes(condArg), forceText: true, canReplace: false), body),
                "ondocked" => new XElement("Event", new XAttribute("event", "Docked"), new XAttribute("style", "craft-docked"), body),
                "onchangesoi" => new XElement("Event", new XAttribute("event", "ChangeSoi"), new XAttribute("style", "change-soi"), body),
                "onpartcollision" => new XElement("Event", new XAttribute("event", "Collide"),
                    new XAttribute("part", trimQuotes(condArg)), body),
                "onpartexplode" => new XElement("Event", new XAttribute("event", "Explode"),
                    new XAttribute("part", trimQuotes(condArg)), body),
                "if" => new XElement("If", new XAttribute("style", "if"), ConvertValueToXml(condArg), body),
                "elseif" => new XElement("ElseIf", new XAttribute("style", "else-if"), ConvertValueToXml(condArg), body),
                "else" => new XElement("Else", new XAttribute("style", "else"), body),
                "while" => new XElement("While", new XAttribute("style", "while"), ConvertValueToXml(condArg), body),
                "waituntil" => new XElement("WaitUntil", new XAttribute("style", "wait-until"), ConvertValueToXml(condArg), body),
                "repeat" => new XElement("Repeat", new XAttribute("style", "repeat"), ConvertValueToXml(condArg), body),
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
            return new XElement("For", new XAttribute("variable", variable), new XAttribute("style", "for"),
                CreateConstant(from),
                CreateConstant(to),
                CreateConstant(step),
                body);
        }

        private XElement? ConvertSetVariableToXml(string varName, string varValue)
        {
            string trimmedValue = varValue.Trim();
            if (trimmedValue.StartsWith("Vz.UserInput("))
            {
                return new XElement("UserInput",
                    new XAttribute("style", "user-input"),
                    CreateVariableReference(varName),
                    ConvertValueToXml(ExtractParenthesisContent(trimmedValue)));
            }

            var setVar = new XElement("SetVariable", new XAttribute("style", "set-variable"));

            var varRef = CreateVariableReference(varName);
            setVar.Add(varRef);

            setVar.Add(ConvertValueToXml(trimmedValue));

            return setVar;
        }

        private XElement? ConvertInstructionToXml(string line)
        {
            // Strip trailing semicolon for matching
            string l = line.TrimEnd(';', ' ');

            // Zero-argument instructions
            if (l == "Vz.ActivateStage()") return new XElement("ActivateStage", new XAttribute("style", "activate-stage"));
            if (l == "Vz.Break()") return new XElement("Break", new XAttribute("style", "break"));
            if (l == "Vz.Beep()") return new XElement("Beep", new XAttribute("style", "play-beep"));
            if (l == "Vz.DestroyAllWidgets()") return new XElement("DestroyAllWidgets", new XAttribute("style", "destroy-all-mfd-widgets"));

            // One-argument instructions
            string c1 = ExtractParenthesisContent(l);
            if (l.StartsWith("Vz.Log("))
                return new XElement("Log", new XAttribute("style", "log"), MakeConstantArg(c1));
            if (l.StartsWith("Vz.Beep("))
            {
                var parts = SplitArgs(c1);
                if (parts.Count >= 3)
                {
                    return new XElement("SetCraftProperty",
                        new XAttribute("property", "Sound.Beep"),
                        new XAttribute("style", "play-beep"),
                        ConvertValueToXml(parts[0]),
                        ConvertValueToXml(parts[1]),
                        ConvertValueToXml(parts[2]));
                }
            }
            if (l.StartsWith("Vz.Display("))
                return new XElement("DisplayMessage", new XAttribute("style", "display"), MakeConstantArg(c1), CreateConstant("7"));
            if (l.StartsWith("Vz.DisplayMessage("))
            {
                var parts = SplitArgs(c1);
                return new XElement("DisplayMessage", new XAttribute("style", "display"),
                    MakeConstantArg(parts.Count > 0 ? parts[0] : "\"\""),
                    MakeConstantArg(parts.Count > 1 ? parts[1] : "7"));
            }
            if (l.StartsWith("Vz.FlightLog("))
            {
                var parts = SplitArgs(c1);
                return new XElement("FlightLog", new XAttribute("style", "flightlog"),
                    MakeConstantArg(parts.Count > 0 ? parts[0] : "\"\""),
                    MakeConstantArg(parts.Count > 1 ? parts[1] : "false"));
            }
            if (l.StartsWith("Vz.WaitSeconds("))
                return new XElement("WaitSeconds", new XAttribute("style", "wait-seconds"), MakeConstantArg(c1));
            if (l.StartsWith("Vz.SetThrottle("))
                return new XElement("SetThrottle", MakeConstantArg(c1));
            if (l.StartsWith("Vz.SetTimeMode("))
                return new XElement("SetTimeMode", MakeConstantArg(c1));
            if (l.StartsWith("Vz.SwitchCraft("))
                return new XElement("SwitchCraft", new XAttribute("style", "switch-craft"), MakeConstantArg(c1));
            if (l.StartsWith("Vz.TargetNode("))
                return new XElement("SetTarget", new XAttribute("style", "set-target"), MakeConstantArg(c1));
            if (l.StartsWith("Vz.DestroyWidget("))
                return new XElement("DestroyWidget", new XAttribute("style", "destroy-mfd-widget"), MakeConstantArg(c1));
            if (l.StartsWith("Vz.SendWidgetToFront("))
                return new XElement("SendWidgetToFront", new XAttribute("style", "set-mfd-order-front"), MakeConstantArg(c1));
            if (l.StartsWith("Vz.SendWidgetToBack("))
                return new XElement("SendWidgetToBack", new XAttribute("style", "set-mfd-order-back"), MakeConstantArg(c1));
            if (l.StartsWith("Vz.InitTexture("))
                return new XElement("InitTexture", new XAttribute("style", "set-mfd-texture-initialize"), MakeConstantArg(c1));
            if (l.StartsWith("Vz.LockNavSphere("))
            {
                var parts = SplitArgs(c1);
                string indicator = parts.Count > 0 ? parts[0].Replace("LockNavSphereIndicatorType.", "").Trim() : "Current";
                var el = new XElement("LockNavSphere",
                    new XAttribute("indicatorType", indicator),
                    new XAttribute("style", indicator.Equals("Vector", StringComparison.OrdinalIgnoreCase) ? "lock-nav-sphere-vector" : "lock-nav-sphere"));
                if (indicator.Equals("Vector", StringComparison.OrdinalIgnoreCase) && parts.Count > 1)
                    el.Add(MakeConstantArg(parts[1]));
                return el;
            }
            if (l.StartsWith("Vz.CMT("))
                return new XElement("Comment", new XAttribute("style", "comment"), CreateConstant(trimQuotes(c1), forceText: true));
            if (l.StartsWith("Vz.SetHeading("))
                return new XElement("SetTargetHeading", new XAttribute("property", "heading"), new XAttribute("style", "set-heading"), MakeConstantArg(c1));
            if (l.StartsWith("Vz.SetPitch("))
                return new XElement("SetTargetHeading", new XAttribute("property", "pitch"), new XAttribute("style", "set-heading"), MakeConstantArg(c1));

            var createWidgetMatch = System.Text.RegularExpressions.Regex.Match(l, @"^Vz(?<type>\w+)\s+(?<name>\w+)\s*=\s*new\s+Vz\w+\(""(?<widgetName>[^""]+)""\)$");
            if (createWidgetMatch.Success)
            {
                return new XElement("CreateWidget",
                    new XAttribute("widgetType", createWidgetMatch.Groups["type"].Value),
                    new XAttribute("name", createWidgetMatch.Groups["widgetName"].Value),
                    new XAttribute("style", "create-mfd-widget"));
            }

            var setWidgetMatch = System.Text.RegularExpressions.Regex.Match(l, @"^(?<widget>\w+)\.(?<property>\w+)\s*=\s*(?<value>.+)$");
            if (setWidgetMatch.Success)
            {
                string property = setWidgetMatch.Groups["property"].Value;
                string widget = setWidgetMatch.Groups["widget"].Value;
                string value = setWidgetMatch.Groups["value"].Value.Trim();
                if (property == "Anchor" && value.StartsWith("WidgetAnchor."))
                {
                    return new XElement("SetWidgetAnchor",
                        new XAttribute("anchor", value.Replace("WidgetAnchor.", "")),
                        CreateVariableReference(widget));
                }

                return new XElement("SetWidget",
                    new XAttribute("property", property),
                    new XAttribute("style", "set-mfd-widget"),
                    CreateVariableReference(widget),
                    ConvertValueToXml(value));
            }

            var subscribeMatch = System.Text.RegularExpressions.Regex.Match(l, @"^(?<widget>\w+)\.Subscribe\(WidgetEventType\.(?<event>\w+),\s*(?<handler>.+?),\s*(?<data>.+?),\s*\(d\)\s*=>\s*\{\s*\}\)$");
            if (subscribeMatch.Success)
            {
                return new XElement("SetWidgetEvent",
                    new XAttribute("eventType", subscribeMatch.Groups["event"].Value),
                    CreateVariableReference(subscribeMatch.Groups["widget"].Value),
                    ConvertValueToXml(subscribeMatch.Groups["handler"].Value),
                    ConvertValueToXml(subscribeMatch.Groups["data"].Value));
            }

            var pixelMatch = System.Text.RegularExpressions.Regex.Match(l, @"^(?<widget>\w+)\.SetPixel\((?<x>.+?),\s*(?<y>.+?),\s*(?<color>.+)\)$");
            if (pixelMatch.Success)
            {
                return new XElement("SetPixel",
                    new XAttribute("style", "set-mfd-texture-setpixel"),
                    CreateVariableReference(pixelMatch.Groups["widget"].Value),
                    ConvertValueToXml(pixelMatch.Groups["x"].Value),
                    ConvertValueToXml(pixelMatch.Groups["y"].Value),
                    ConvertValueToXml(pixelMatch.Groups["color"].Value));
            }

            var pointsMatch = System.Text.RegularExpressions.Regex.Match(l, @"^(?<widget>\w+)\.SetPoints\((?<points>.+)\)$");
            if (pointsMatch.Success)
            {
                return new XElement("SetLinePoints",
                    new XAttribute("style", "set-mfd-line-points"),
                    CreateVariableReference(pointsMatch.Groups["widget"].Value),
                    ConvertValueToXml(pointsMatch.Groups["points"].Value));
            }

            // SetCamera(prop, val)
            if (l.StartsWith("Vz.SetCamera("))
            {
                var parts = SplitArgs(c1);
                string prop = parts.Count > 0 ? parts[0].Trim('"', ' ') : "zoom";
                string val  = parts.Count > 1 ? parts[1] : "0";
                return new XElement("SetCamera", new XAttribute("property", prop), new XAttribute("style", "set-camera"), MakeConstantArg(val));
            }

            // SetActivationGroup(group, val)
            if (l.StartsWith("Vz.SetActivationGroup("))
            {
                var parts = SplitArgs(c1);
                var el = new XElement("SetActivationGroup", new XAttribute("style", "set-ag"));
                el.Add(MakeConstantArg(parts.Count > 0 ? parts[0] : "1"));
                el.Add(MakeConstantArg(parts.Count > 1 ? parts[1] : "true"));
                return el;
            }

            // SetInput(CraftInput.X, val)
            if (l.StartsWith("Vz.SetInput("))
            {
                var parts = SplitArgs(c1);
                string input = parts.Count > 0 ? parts[0].Replace("CraftInput.", "").Trim() : "Throttle";
                var el = new XElement("SetInput", new XAttribute("input", input.ToLowerInvariant()), new XAttribute("style", "set-input"));
                el.Add(MakeConstantArg(parts.Count > 1 ? parts[1] : "0"));
                return el;
            }

            // SetTargetHeading(TargetHeadingProperty.X, val)
            if (l.StartsWith("Vz.SetTargetHeading("))
            {
                var parts = SplitArgs(c1);
                string prop = parts.Count > 0 ? parts[0].Replace("TargetHeadingProperty.", "").Replace('_', '-').Trim() : "Heading";
                var el = new XElement("SetTargetHeading", new XAttribute("property", prop.ToLowerInvariant()), new XAttribute("style", "set-heading"));
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
                var el = new XElement("SetPartProperty", new XAttribute("property", prop), new XAttribute("style", "set-part"));
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
                    new XAttribute("style", "comment"),
                    CreateConstant($"[TODO] {l}", forceText: true));

            return null;
        }

        private XElement MakeConstantArg(string val)
        {
            return ConvertValueToXml(val);
        }

        private XElement ConvertValueToXml(string value)
        {
            string trimmed = value.Trim();
            var expr = ConvertApiCallToXml(trimmed);
            if (expr != null)
                return expr;

            return CreateVariableReference(trimmed);
        }

        private XElement MakeListOp(string op, List<string> parts)
        {
            var el = new XElement("ListOp", new XAttribute("op", op.ToLowerInvariant()), new XAttribute("style", "list-"));
            foreach (var p in parts) el.Add(MakeConstantArg(p));
            return el;
        }

        private List<string> SplitArgs(string argsStr)
        {
            var result = new List<string>();
            int depth = 0;
            bool inString = false;
            bool escaped = false;
            var cur = new StringBuilder();
            foreach (char c in argsStr)
            {
                if (escaped)
                {
                    cur.Append(c);
                    escaped = false;
                    continue;
                }

                if (c == '\\' && inString)
                {
                    cur.Append(c);
                    escaped = true;
                    continue;
                }

                if (c == '"')
                {
                    inString = !inString;
                    cur.Append(c);
                    continue;
                }

                if (!inString)
                {
                    if (c == '(' || c == '[' || c == '{') depth++;
                    else if (c == ')' || c == ']' || c == '}') depth--;
                }

                if (c == ',' && depth == 0 && !inString)
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

            while (HasOuterParentheses(call))
                call = call.Substring(1, call.Length - 2).Trim();

            // Numeric literal
            if (double.TryParse(call, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double numVal))
                return CreateConstant(numVal.ToString(System.Globalization.CultureInfo.InvariantCulture));

            // String literal
            if (call.StartsWith("\"") && call.EndsWith("\""))
                return CreateConstant(trimQuotes(call), forceText: true);

            // Boolean
            if (call == "true" || call == "false") return CreateConstant(call);

            if (call.StartsWith("!"))
            {
                var inner = ConvertApiCallToXml(call.Substring(1));
                if (inner != null)
                    return new XElement("Not", new XAttribute("style", "op-not"), inner);
            }

            if (TryConvertBinaryExpression(call, new[] { "||" }, tuple => CreateBoolOp("or", tuple.left, tuple.right), out var orExpr))
                return orExpr;
            if (TryConvertBinaryExpression(call, new[] { "&&" }, tuple => CreateBoolOp("and", tuple.left, tuple.right), out var andExpr))
                return andExpr;
            if (TryConvertBinaryExpression(call, new[] { ">=", "<=", "==", "!=", ">", "<" }, CreateComparison, out var cmpExpr))
                return cmpExpr;
            if (TryConvertBinaryExpression(call, new[] { "+", "-" }, CreateBinaryOpElement, out var addExpr))
                return addExpr;
            if (TryConvertBinaryExpression(call, new[] { "*", "/", "%" }, CreateBinaryOpElement, out var mulExpr))
                return mulExpr;
            if (TryConvertBinaryExpression(call, new[] { "^" }, CreateBinaryOpElement, out var powExpr))
                return powExpr;

            // Variable reference (plain identifier)
            if (System.Text.RegularExpressions.Regex.IsMatch(call, @"^[A-Za-z_]\w*$"))
                return CreateVariableReference(call);

            if (System.Text.RegularExpressions.Regex.IsMatch(call, @"^[A-Za-z_]\w*(\.[A-Za-z_]\w*)+$"))
                return CreateConstant(call, forceText: true);

            if (TryConvertFunctionCall(call, "Vz.Random", args => CreateBinaryOpElement(("rand", args[0], args[1])), 2, out var randExpr)) return randExpr;
            if (TryConvertFunctionCall(call, "Vz.Min", args => CreateBinaryOpElement(("min", args[0], args[1])), 2, out var minExpr)) return minExpr;
            if (TryConvertFunctionCall(call, "Vz.Max", args => CreateBinaryOpElement(("max", args[0], args[1])), 2, out var maxExpr)) return maxExpr;
            if (TryConvertFunctionCall(call, "Vz.Atan2", args => CreateBinaryOpElement(("atan2", args[0], args[1])), 2, out var atan2Expr)) return atan2Expr;

            // Vz.Vec(x, y, z)
            if (call.StartsWith("Vz.Vec("))
            {
                var parts = SplitArgs(ExtractParenthesisContent(call));
                var el = new XElement("Vector", new XAttribute("style", "vec"));
                foreach (var p in parts) { var c = ConvertApiCallToXml(p); if (c != null) el.Add(c); }
                return el;
            }

            if (TryConvertFunctionCall(call, "Vz.Join", args => CreateStringOp("join", args), 2, out var joinExpr)) return joinExpr;
            if (TryConvertFunctionCall(call, "Vz.LengthOf", args => CreateStringOp("length", args), 1, out var lenExpr)) return lenExpr;
            if (TryConvertFunctionCall(call, "Vz.LetterOf", args => CreateStringOp("letter", args), 2, out var letterExpr)) return letterExpr;
            if (TryConvertFunctionCall(call, "Vz.SubString", args => CreateStringOp("substring", args), 3, out var substringExpr)) return substringExpr;
            if (TryConvertFunctionCall(call, "Vz.Contains", args => CreateStringOp("contains", args), 2, out var containsExpr)) return containsExpr;
            if (TryConvertFunctionCall(call, "Vz.Format", args => CreateStringOp("format", args), 2, out var formatExpr)) return formatExpr;

            if (TryConvertFunctionCall(call, "Vz.ListGet", args => CreateListExpression("get", args), 2, out var listGetExpr)) return listGetExpr;
            if (TryConvertFunctionCall(call, "Vz.ListLength", args => CreateListExpression("length", args), 1, out var listLengthExpr)) return listLengthExpr;
            if (TryConvertFunctionCall(call, "Vz.ListIndex", args => CreateListExpression("index", args), 2, out var listIndexExpr)) return listIndexExpr;
            if (call == "Vz.CreateList()") return new XElement("ListExpression", new XAttribute("op", "create"));

            if (TryConvertFunctionCall(call, "Vz.Length", args => CreateVectorOp("length", args), 1, out var vecLenExpr)) return vecLenExpr;
            if (TryConvertFunctionCall(call, "Vz.Norm", args => CreateVectorOp("norm", args), 1, out var vecNormExpr)) return vecNormExpr;
            if (TryConvertFunctionCall(call, "Vz.Dot", args => CreateVectorOp("dot", args), 2, out var vecDotExpr)) return vecDotExpr;
            if (TryConvertFunctionCall(call, "Vz.Cross", args => CreateVectorOp("cross", args), 2, out var vecCrossExpr)) return vecCrossExpr;
            if (TryConvertFunctionCall(call, "Vz.Angle", args => CreateVectorOp("angle", args), 2, out var vecAngleExpr)) return vecAngleExpr;
            if (TryConvertFunctionCall(call, "Vz.Distance", args => CreateVectorOp("dist", args), 2, out var vecDistExpr)) return vecDistExpr;
            if (TryConvertFunctionCall(call, "Vz.Project", args => CreateVectorOp("project", args), 2, out var vecProjectExpr)) return vecProjectExpr;
            if (TryConvertFunctionCall(call, "Vz.Scale", args => CreateVectorOp("scale", args), 2, out var vecScaleExpr)) return vecScaleExpr;
            if (TryConvertFunctionCall(call, "Vz.VecMin", args => CreateVectorOp("min", args), 2, out var vecMinExpr)) return vecMinExpr;
            if (TryConvertFunctionCall(call, "Vz.VecMax", args => CreateVectorOp("max", args), 2, out var vecMaxExpr)) return vecMaxExpr;
            if (TryConvertFunctionCall(call, "Vz.Clamp", args => CreateVectorOp("clamp", args), 2, out var vecClampExpr)) return vecClampExpr;

            if (TryConvertCraftProperty(call, out var craftPropertyExpr))
                return craftPropertyExpr;

            if (TryConvertFunctionCall(call, "Vz.CraftInput", args =>
                new XElement("CraftInput", new XAttribute("input", trimQuotes(args[0]).Replace("CraftInput.", "")), new XAttribute("style", "prop-input")), 1, out var craftInputExpr))
                return craftInputExpr;

            // Vz.Craft.AltitudeASL() → CraftProperty property="Altitude.ASL"
            if (call.StartsWith("Vz.Craft."))
            {
                return new XElement("CraftProperty", new XAttribute("property", call.Substring(9)));
            }

            // Vz.Sqrt(x) → MathFunction
            var mathFns = new[] { "Abs","Sqrt","Floor","Ceiling","Round","Sin","Cos","Tan",
                "Asin","Acos","Atan","Ln","Log10","Deg2Rad","Rad2Deg" };
            foreach (var fn in mathFns)
            {
                if (call.StartsWith($"Vz.{fn}("))
                {
                    var el = new XElement("MathFunction", new XAttribute("function", fn == "Log10" ? "log" : fn.ToLowerInvariant()), new XAttribute("style", "op-math"));
                    var c = ConvertApiCallToXml(ExtractParenthesisContent(call));
                    if (c != null) el.Add(c);
                    return el;
                }
            }

            return null;
        }

        private void ParseInstructionLines(List<string> lines, ref int index, XElement target, bool stopAtClosingBrace)
        {
            for (; index < lines.Count; index++)
            {
                string line = lines[index].Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                    continue;
                if (line == "{")
                    continue;
                if (line == "}")
                {
                    if (stopAtClosingBrace)
                        return;
                    continue;
                }

                if (line.StartsWith("using (new "))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(line, @"using\s+\(new\s+(\w+)\((.*)\)\)");
                    if (match.Success)
                    {
                        var block = ConvertBlockToXml(match.Groups[1].Value, match.Groups[2].Value, lines, ref index);
                        if (block != null)
                            target.Add(block);
                    }
                    continue;
                }

                if (System.Text.RegularExpressions.Regex.IsMatch(line, @"(?<![=!<>])=(?!=)"))
                {
                    var assignMatch = System.Text.RegularExpressions.Regex.Match(line, @"(\w+)\s*=\s*(.+);");
                    if (assignMatch.Success)
                    {
                        var setVar = ConvertSetVariableToXml(assignMatch.Groups[1].Value, assignMatch.Groups[2].Value);
                        if (setVar != null)
                            target.Add(setVar);
                        continue;
                    }
                }

                var instruction = ConvertInstructionToXml(line);
                if (instruction != null)
                    target.Add(instruction);
            }
        }

        private static int FindBlockStart(List<string> lines, int startIndex)
        {
            for (int i = startIndex; i < lines.Count; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                    continue;
                return line == "{" ? i + 1 : i;
            }
            return -1;
        }

        private XElement CreateVariableDeclaration(string varName, string value)
        {
            string trimmed = value.Trim();
            if (trimmed == "Vz.CreateList()")
                return new XElement("Variable", new XAttribute("name", varName), new XElement("Items"));

            var variable = new XElement("Variable", new XAttribute("name", varName));
            if (bool.TryParse(trimmed, out bool boolVal))
            {
                variable.Add(new XAttribute("style", boolVal ? "true" : "false"));
                variable.Add(new XAttribute("bool", boolVal ? "true" : "false"));
                return variable;
            }

            if (double.TryParse(trimmed, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double num))
            {
                variable.Add(new XAttribute("number", num.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                return variable;
            }

            if (trimmed.StartsWith("\"") && trimmed.EndsWith("\""))
            {
                variable.Add(CreateConstant(trimQuotes(trimmed), forceText: true));
                return variable;
            }

            variable.Add(CreateConstant(trimmed, forceText: true));
            return variable;
        }

        private static XElement CreateVariableReference(string variableName)
        {
            return new XElement("Variable",
                new XAttribute("list", "false"),
                new XAttribute("local", "false"),
                new XAttribute("variableName", variableName.Trim()));
        }

        private XElement CreateVariableOrExpression(string value)
        {
            return ConvertApiCallToXml(value) ?? CreateVariableReference(value);
        }

        private static XElement CreateConstant(string value, bool forceText = false, bool canReplace = true)
        {
            string trimmed = value.Trim();
            var attrs = new List<object>();
            if (!canReplace)
                attrs.Add(new XAttribute("canReplace", "false"));

            if (!forceText && bool.TryParse(trimmed, out bool boolVal))
            {
                attrs.Add(new XAttribute("style", boolVal ? "true" : "false"));
                attrs.Add(new XAttribute("bool", boolVal ? "true" : "false"));
                return new XElement("Constant", attrs);
            }

            if (!forceText && double.TryParse(trimmed, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double numVal))
            {
                attrs.Add(new XAttribute("number", numVal.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                return new XElement("Constant", attrs);
            }

            attrs.Add(new XAttribute("text", trimQuotes(trimmed)));
            return new XElement("Constant", attrs);
        }

        private XElement CreateBinaryOpElement((string op, string left, string right) value)
        {
            string op = NormalizeBinaryOp(value.op);
            return new XElement("BinaryOp",
                new XAttribute("op", op),
                new XAttribute("style", BinaryStyle(op)),
                CreateVariableOrExpression(value.left),
                CreateVariableOrExpression(value.right));
        }

        private XElement CreateComparison((string op, string left, string right) value)
        {
            string op = value.op switch
            {
                "==" => "=",
                "!=" => "ne",
                ">" => "g",
                "<" => "l",
                ">=" => "ge",
                "<=" => "le",
                _ => value.op
            };
            return new XElement("Comparison",
                new XAttribute("op", op),
                new XAttribute("style", ComparisonStyle(op)),
                CreateVariableOrExpression(value.left),
                CreateVariableOrExpression(value.right));
        }

        private XElement CreateBoolOp(string op, string left, string right)
        {
            return new XElement("BoolOp",
                new XAttribute("op", op),
                new XAttribute("style", op == "or" ? "op-or" : "op-and"),
                CreateVariableOrExpression(left),
                CreateVariableOrExpression(right));
        }

        private XElement CreateStringOp(string op, IReadOnlyList<string> args)
        {
            var el = new XElement("StringOp", new XAttribute("op", op), new XAttribute("style", op));
            foreach (var arg in args)
                el.Add(CreateVariableOrExpression(arg));
            return el;
        }

        private XElement CreateListExpression(string op, IReadOnlyList<string> args)
        {
            var el = new XElement("ListExpression", new XAttribute("op", op));
            foreach (var arg in args)
                el.Add(CreateVariableOrExpression(arg));
            return el;
        }

        private XElement CreateVectorOp(string op, IReadOnlyList<string> args)
        {
            var el = new XElement("VectorOp",
                new XAttribute("op", op),
                new XAttribute("style", args.Count > 1 ? "vec-op-2" : "vec-op-1"));
            foreach (var arg in args)
                el.Add(CreateVariableOrExpression(arg));
            return el;
        }

        private bool TryConvertCraftProperty(string call, out XElement? element)
        {
            if (CraftPropertyCallMap.TryGetValue(call, out string? property))
            {
                element = new XElement("CraftProperty",
                    new XAttribute("property", property),
                    new XAttribute("style", CraftPropertyStyle(property)));
                return true;
            }

            element = null;
            return false;
        }

        private bool TryConvertFunctionCall(string call, string functionName, Func<List<string>, XElement> factory, int minArgs, out XElement? element)
        {
            if (!call.StartsWith(functionName + "(", StringComparison.Ordinal))
            {
                element = null;
                return false;
            }

            var args = SplitArgs(ExtractParenthesisContent(call));
            if (args.Count < minArgs)
            {
                element = null;
                return false;
            }

            element = factory(args);
            return true;
        }

        private bool TryConvertBinaryExpression(string call, IReadOnlyList<string> operators, Func<(string op, string left, string right), XElement> factory, out XElement? element)
        {
            foreach (var op in operators)
            {
                int idx = FindTopLevelOperator(call, op);
                if (idx > 0)
                {
                    string left = call.Substring(0, idx).Trim();
                    string right = call.Substring(idx + op.Length).Trim();
                    if (!string.IsNullOrEmpty(left) && !string.IsNullOrEmpty(right))
                    {
                        element = factory((op, left, right));
                        return true;
                    }
                }
            }

            element = null;
            return false;
        }

        private static int FindTopLevelOperator(string expr, string op)
        {
            int depth = 0;
            bool inString = false;
            for (int i = expr.Length - op.Length; i >= 0; i--)
            {
                char c = expr[i];
                if (c == '"' && (i == 0 || expr[i - 1] != '\\'))
                {
                    inString = !inString;
                    continue;
                }
                if (inString)
                    continue;
                if (c == ')' || c == ']' || c == '}') depth++;
                else if (c == '(' || c == '[' || c == '{') depth--;

                if (depth == 0 && expr.Substring(i).StartsWith(op, StringComparison.Ordinal))
                {
                    if ((op == "+" || op == "-") && (i == 0 || "+-*/%^<>=!&|(".Contains(expr[i - 1])))
                        continue;
                    return i;
                }
            }
            return -1;
        }

        private static bool HasOuterParentheses(string expr)
        {
            expr = expr.Trim();
            if (expr.Length < 2 || expr[0] != '(' || expr[^1] != ')')
                return false;

            int depth = 0;
            bool inString = false;
            for (int i = 0; i < expr.Length; i++)
            {
                char c = expr[i];
                if (c == '"' && (i == 0 || expr[i - 1] != '\\'))
                    inString = !inString;
                if (inString)
                    continue;
                if (c == '(') depth++;
                else if (c == ')')
                {
                    depth--;
                    if (depth == 0 && i < expr.Length - 1)
                        return false;
                }
            }
            return depth == 0;
        }

        private static string trimQuotes(string value)
        {
            string trimmed = value.Trim();
            return trimmed.StartsWith("\"") && trimmed.EndsWith("\"") && trimmed.Length >= 2
                ? trimmed.Substring(1, trimmed.Length - 2)
                : trimmed;
        }

        private static bool IsVizzyExpression(string value)
        {
            string trimmed = value.Trim();
            return trimmed.StartsWith("Vz.")
                || trimmed.Contains("&&")
                || trimmed.Contains("||")
                || trimmed.StartsWith("!")
                || trimmed.Contains("==")
                || trimmed.Contains("!=")
                || trimmed.Contains(">=")
                || trimmed.Contains("<=")
                || trimmed.Contains(" > ")
                || trimmed.Contains(" < ")
                || trimmed.Contains(" + ")
                || trimmed.Contains(" - ")
                || trimmed.Contains(" * ")
                || trimmed.Contains(" / ")
                || trimmed.Contains(" ^ ")
                || trimmed.Contains(" % ");
        }

        private static string NormalizeBinaryOp(string op) => op.ToLowerInvariant() switch
        {
            "rand" => "rand",
            "min" => "min",
            "max" => "max",
            "atan2" => "atan2",
            _ => op
        };

        private static string BinaryStyle(string op) => op switch
        {
            "+" => "op-add",
            "-" => "op-sub",
            "*" => "op-mul",
            "/" => "op-div",
            "^" => "op-exp",
            "%" => "op-mod",
            "rand" => "op-rand",
            "min" => "op-min",
            "max" => "op-max",
            "atan2" => "op-atan-2",
            _ => "op-add"
        };

        private static string ComparisonStyle(string op) => op switch
        {
            "=" => "op-eq",
            "ne" => "op-ne",
            "g" => "op-gt",
            "l" => "op-lt",
            "ge" => "op-gte",
            "le" => "op-lte",
            _ => "op-eq"
        };

        private static string CraftPropertyStyle(string property)
        {
            if (property.StartsWith("Altitude.", StringComparison.OrdinalIgnoreCase)) return "prop-altitude";
            if (property.StartsWith("Orbit.", StringComparison.OrdinalIgnoreCase)) return "prop-orbit";
            if (property.StartsWith("Vel.", StringComparison.OrdinalIgnoreCase)) return "prop-velocity";
            if (property.StartsWith("Nav.", StringComparison.OrdinalIgnoreCase)) return "prop-nav";
            if (property.StartsWith("Target.", StringComparison.OrdinalIgnoreCase) || property.StartsWith("Name.", StringComparison.OrdinalIgnoreCase)) return "prop-name";
            if (property.StartsWith("Fuel.", StringComparison.OrdinalIgnoreCase)) return "prop-fuel";
            if (property.StartsWith("Performance.", StringComparison.OrdinalIgnoreCase)) return "prop-performance";
            if (property.StartsWith("Input.", StringComparison.OrdinalIgnoreCase)) return "prop-input";
            if (property.StartsWith("Time.", StringComparison.OrdinalIgnoreCase)) return "prop-time";
            return "craft";
        }

        private string ExtractParenthesisContent(string line)
        {
            int start = line.IndexOf('(') + 1;
            int end = line.LastIndexOf(')');
            if (start > 0 && end > start)
            {
                return line.Substring(start, end - start);
            }
            return "";
        }
    }
}
