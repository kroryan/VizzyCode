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
        // Variables that are local parameters in the CI body currently being exported.
        // Set before processing a CI body, cleared afterwards. Ordinal (case-sensitive).
        private HashSet<string> _localVariables = new(StringComparer.Ordinal);
        // Maps sanitized C# identifier → original CustomInstruction name for CI call detection.
        private Dictionary<string, string> _ciNameMap = new(StringComparer.Ordinal);
        private XElement? _pendingTopLevelHeader;
        private XElement? _pendingInstructionMetadata;
        private (int X, int Y)? _pendingTopLevelPosition;
        private bool _preferTextConstantsForAuthoring;
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

            // Each <Instructions> block in Vizzy either declares a CustomInstruction
            // (first element is <CustomInstruction pos="..."/>) or is an event handler
            // (first element is <Event .../>) or a standalone block.
            bool ciHeaderPrinted = false;
            foreach (var instrBlock in program.Elements("Instructions"))
            {
                sb.AppendLine("// VZTOPBLOCK");
                var elements = instrBlock.Elements().ToList();
                if (elements.Count == 0) continue;

                var firstEl = elements[0];
                EmitTopLevelBlockMetadata(firstEl, sb);
                if (firstEl.Name.LocalName == "CustomInstruction" &&
                    firstEl.Attribute("pos") != null)
                {
                    // Custom instruction block: header + sibling body elements
                    if (!ciHeaderPrinted)
                    {
                        sb.AppendLine("// ── Custom Instructions ──────────────────────────────");
                        ciHeaderPrinted = true;
                    }
                    EmitCustomInstructionBlock(firstEl, elements.Skip(1), sb);
                }
                else
                {
                    // Event handler or standalone preamble block
                    EmitInstructions(elements, sb, 0);
                }
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
                // Preserve -0 so it can be restored on round-trip
                sb.AppendLine(isList
                    ? $"// var {San(vname)} = [];   // list;"
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
                EmitTopLevelBlockMetadata(expr, sb);
                string ename = expr.Attribute("name")?.Value ?? "MyExpr";
                var pnames = GetFormatParams(expr.Attribute("format")?.Value ?? "");
                if (pnames.Count == 0)
                    pnames = expr.Elements("Parameter").Select(p => San(p.Attribute("name")?.Value ?? "p")).ToList();
                string pStr = string.Join(", ", pnames.Select(p => $"\"{p}\""));
                string pCall = string.Join(", ", pnames);
                // The return expression is the DIRECT child element (not wrapped in <Instructions>)
                _localVariables = new HashSet<string>(pnames, StringComparer.Ordinal);
                var returnEl = expr.Elements().FirstOrDefault(e => e.Name.LocalName != "Parameter");
                string returnCode = returnEl != null ? ConvertExpression(returnEl) : "0";
                _localVariables = new HashSet<string>();
                string pStrArg = pStr.Length > 0 ? ", " + pStr : "";
                sb.AppendLine($"var {San(ename)} = Vz.DeclareCustomExpression(\"{Esc(ename)}\"{pStrArg}).SetReturn(({pCall}) =>");
                sb.AppendLine("{");
                sb.AppendLine($"    return {returnCode};");
                sb.AppendLine("});");
                sb.AppendLine();
            }
        }

        // ── Custom Instruction declarations ───────────────────────────────────

        /// <summary>
        /// Emits one custom instruction whose header is <paramref name="ci"/> and
        /// whose body is <paramref name="bodyElements"/> (siblings in the same
        /// &lt;Instructions&gt; block, NOT children of the CI element).
        /// </summary>
        private void EmitCustomInstructionBlock(XElement ci, IEnumerable<XElement> bodyElements, StringBuilder sb)
        {
            string ciname = ci.Attribute("name")?.Value ?? "MyInstr";
            var pnames = GetFormatParams(ci.Attribute("format")?.Value ?? "");
            if (pnames.Count == 0)
                pnames = ci.Elements("Parameter").Select(p => San(p.Attribute("name")?.Value ?? "p")).ToList();
            string pStr = string.Join(", ", pnames.Select(p => $"\"{p}\""));
            string pCall = string.Join(", ", pnames);
            sb.AppendLine($"var {San(ciname)} = Vz.DeclareCustomInstruction(\"{Esc(ciname)}\"{(pStr.Length > 0 ? ", " + pStr : "")}).SetInstructions(({pCall}) =>");
            sb.AppendLine("{");
            EmitInstructions(bodyElements, sb, 1);
            sb.AppendLine("});");
            sb.AppendLine();
        }

        private void EmitTopLevelBlockMetadata(XElement firstEl, StringBuilder sb)
        {
            if (firstEl.Name.LocalName is not ("CustomInstruction" or "Event" or "CustomExpression"))
                return;

            if (firstEl.Attribute("pos") != null &&
                TryParsePositionAttribute(firstEl.Attribute("pos")!.Value, out int posX, out int posY))
            {
                sb.AppendLine($"// VZPOS x={posX} y={posY}");
            }

            string xml = firstEl.ToString(SaveOptions.DisableFormatting);
            string payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));
            sb.AppendLine($"// VZBLOCK {payload}");
        }

        private void EmitInstructionMetadata(XElement el, StringBuilder sb, string ind)
        {
            if (!ShouldPreserveInstructionMetadata(el))
                return;

            var shell = new XElement(el.Name, el.Attributes());
            if (el.Name.LocalName is "SetVariable" or "ChangeVariable")
            {
                var targetVar = el.Elements().FirstOrDefault(e => e.Name.LocalName == "Variable");
                if (targetVar != null)
                    shell.Add(new XElement(targetVar));
            }
            else if (el.Name.LocalName == "For")
            {
                foreach (var child in el.Elements().Where(e => e.Name.LocalName != "Instructions"))
                    shell.Add(new XElement(child));
            }
            string xml = shell.ToString(SaveOptions.DisableFormatting);
            string payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));
            sb.AppendLine($"{ind}// VZEL {payload}");
        }

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
                sb.AppendLine($"var {San(ciname)} = Vz.DeclareCustomInstruction(\"{Esc(ciname)}\"{(pStr.Length > 0 ? ", " + pStr : "")}).SetInstructions(({pCall}) =>");
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
            var list = elements.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var el = list[i];
                EmitInstruction(el, sb, depth);

                // If we just emitted an Event whose body comes from sibling elements
                // (rather than a nested <Instructions> child), skip those siblings so
                // the outer loop doesn't process them a second time.
                if (el.Name.LocalName == "Event" && el.Element("Instructions") == null)
                {
                    int j = i + 1;
                    while (j < list.Count && list[j].Name.LocalName != "Event")
                        j++;
                    i = j - 1; // the for-loop will do i++ → j
                }
            }
        }

        private void EmitInstruction(XElement el, StringBuilder sb, int depth)
        {
            if (el.Name.LocalName == "CustomInstruction" && el.Attribute("pos") != null) return;

            string ind = I(depth);
            EmitInstructionMetadata(el, sb, ind);
            switch (el.Name.LocalName)
            {
                case "Comment":         EmitComment(el, sb, ind); break;
                case "Event":           EmitEvent(el, sb, depth, ind); break;
                case "SetVariable":     EmitSetVariable(el, sb, ind); break;
                case "ChangeVariable":  EmitChangeVariable(el, sb, ind); break;
                case "While":           EmitBlock("While", el, sb, depth, ind); break;
                case "If":              EmitBlock("If", el, sb, depth, ind); break;
                case "ElseIf":
                    if (string.Equals(el.Attribute("style")?.Value, "else", StringComparison.OrdinalIgnoreCase))
                        EmitElse(el, sb, depth, ind);
                    else
                        EmitBlock("ElseIf", el, sb, depth, ind);
                    break;
                case "Else":            EmitElse(el, sb, depth, ind); break;
                case "Repeat":          EmitBlockN("Repeat", el, sb, depth, ind); break;
                case "For":
                case "ForLoop":         EmitForLoop(el, sb, depth, ind); break;
                case "WaitSeconds":     sb.AppendLine($"{ind}Vz.WaitSeconds({E1(el)});"); break;
                case "WaitUntil":       sb.AppendLine($"{ind}using (new WaitUntil({E1(el)})) {{ }}"); break;
                case "LogMessage":
                case "Log":             sb.AppendLine($"{ind}Vz.Log({E1(el)});"); break;
                case "Display":
                case "DisplayMessage":
                {
                    var _dch = el.Elements().ToList();
                    string _dmsg = _dch.Count > 0 ? ConvertExpression(_dch[0]) : "\"\"";
                    string _ddur = _dch.Count > 1 ? ConvertExpression(_dch[1]) : "7";
                    sb.AppendLine($"{ind}Vz.Display({_dmsg}, {_ddur});");
                    break;
                }
                case "LogFlight":
                case "FlightLog":
                {
                    var _flCh = el.Elements().ToList();
                    string _flMsg = _flCh.Count > 0 ? ConvertExpression(_flCh[0]) : "\"\"";
                    string _flShow = _flCh.Count > 1 ? ", " + ConvertExpression(_flCh[1]) : "";
                    sb.AppendLine($"{ind}Vz.FlightLog({_flMsg}{_flShow});");
                    break;
                }
                case "Break":           sb.AppendLine($"{ind}Vz.Break();"); break;
                case "Beep":            sb.AppendLine($"{ind}Vz.Beep();"); break;
                case "ActivateStage":   sb.AppendLine($"{ind}Vz.ActivateStage();"); break;
                case "SetThrottle":     sb.AppendLine($"{ind}Vz.SetThrottle({E1(el)});"); break;
                case "SetActivateGroup":
                case "SetActivationGroup": EmitSetAG(el, sb, ind); break;
                case "SetInput":        EmitSetInput(el, sb, ind); break;
                case "SetTargetHeading":
                case "LockHeading":     EmitLockHeading(el, sb, ind); break;
                case "SetTimeMode":
                    if (el.Elements().Any())
                        sb.AppendLine($"{ind}Vz.SetTimeMode({E1(el)});");
                    else
                        sb.AppendLine($"{ind}Vz.SetTimeModeAttr(\"{Esc(el.Attribute("mode")?.Value ?? "Normal")}\");");
                    break;
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
            string vname = el.Attribute("var")?.Value ?? el.Attribute("variable")?.Value ?? "i";
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
            var ch = el.Elements().ToList();
            if (ch.Count > 0)
                sb.AppendLine($"{ind}Vz.LockNavSphere(LockNavSphereIndicatorType.{Cap(type)}, {ConvertExpression(ch[0])});");
            else
                sb.AppendLine($"{ind}Vz.LockNavSphere(LockNavSphereIndicatorType.{Cap(type)});");
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
                case "Variable":
                {
                    string variableName = el.Attribute("variableName")?.Value ?? "";
                    if (variableName.Length == 0)
                        return "0";
                    // Preserve all attributes (list, local, variableName) via RawXmlVariable so the
                    // sidecar mechanism can restore them on export. CodeCleanView.SimplifyVisibleLine
                    // will strip this down to the plain identifier for the clean-view .vizzy.cs file.
                    return BuildReadablePreservedCall("RawXmlVariable", el);
                }
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
            if (NeedsRawConstantPreservation(el))
                return BuildReadablePreservedCall("RawXmlConstant", el);

            string? style = el.Attribute("style")?.Value;
            string? boolVal = el.Attribute("bool")?.Value;
            if (style == "true" || boolVal == "true")  return "true";
            if (style == "false" || boolVal == "false") return "false";
            var numAttr = el.Attribute("number");
            if (numAttr != null)
            {
                string nv = numAttr.Value;
                if (nv == "-0")
                    return BuildReadablePreservedCall("RawXmlConstant", el);
                return nv;
            }
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
            // Special cases that aren't infix — handle both capitalized and lowercase op names
            if (op.Equals("Rand",  StringComparison.OrdinalIgnoreCase)) return $"Vz.Random({l}, {r})";
            if (op.Equals("Min",   StringComparison.OrdinalIgnoreCase)) return $"Vz.Min({l}, {r})";
            if (op.Equals("Max",   StringComparison.OrdinalIgnoreCase)) return $"Vz.Max({l}, {r})";
            if (op.Equals("ATan2", StringComparison.OrdinalIgnoreCase)) return $"Vz.Atan2({l}, {r})";
            string cs = op switch { "+" => "+", "-" => "-", "*" => "*", "/" => "/",
                "^" => "^", "%" => "%", _ => op };
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
                "dist" or "distance"    => $"Vz.Distance({a}, {b})",
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
            string? style = el.Attribute("style")?.Value;
            if (style != null && !string.Equals(style, CraftPropertyStyle(prop), StringComparison.Ordinal))
                return BuildReadablePreservedCall("RawXmlCraftProperty", el);
            var ch = el.Elements().ToList();
            string arg = ch.Count > 0 ? ConvertExpression(ch[0]) : "";
            string arg2 = ch.Count > 1 ? ConvertExpression(ch[1]) : "";
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
                "Craft.Planet"                    => ch.Count > 0 ? $"Vz.OtherCraft({arg}).Planet()" : "Vz.Craft.Planet()",
                // Part queries
                "Part.NameToID"                   => $"Vz.PartNameToID({arg})",
                "Part.PciToLocal"                 => ch.Count > 1 ? $"Vz.PartPciToLocal({arg}, {arg2})" : $"Vz.PartPciToLocal({arg})",
                "Part.SetActivated"               => $"Vz.SetPartActivated({arg})",
                // Input
                "Input.Throttle"                  => "Vz.CraftInput(CraftInput.Throttle)",
                "Input.TranslationMode"           => "Vz.CraftInput(CraftInput.TranslationMode)",
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
                // Coordinate conversion ops: single child is the position/vector, not a planet name
                "toLatLongAgl"   => $"Vz.PosToLatLongAgl({planet})",
                "toLatLongAsl"   => $"Vz.PosToLatLongAsl({planet})",
                "toPosition"     => $"Vz.ToPosition({planet})",
                _ => $"Vz.Planet({planet}).Op(\"{Esc(op)}\")"
            };
        }

        private string ConvertEvalExpr(XElement el)
        {
            var child = el.Elements().FirstOrDefault();
            string expr = child?.Attribute("text")?.Value ?? child?.Attribute("number")?.Value ?? "";
            if (el.Attribute("style")?.Value == "evaluate-expression")
                return BuildReadablePreservedCall("RawXmlEval", el);
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
                "create" => ch.Count > 0
                    ? $"Vz.CreateListRaw({ConvertExpression(ch[0])})"
                    : $"Vz.CreateList()",
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
            // Build extra args for join/format which can have more than 2 children
            string extraArgs = ch.Count > 2
                ? ", " + string.Join(", ", ch.Skip(2).Select(ConvertExpression))
                : "";
            return op.ToLower() switch
            {
                "join"      => $"Vz.Join({a}, {b}{extraArgs})",
                "length"    => $"Vz.LengthOf({a})",
                "letter"    => $"Vz.LetterOf({a}, {b})",
                "substring" => $"Vz.SubString({a}, {b}, {c})",
                "contains"  => $"Vz.Contains({a}, {b})",
                "format"    => $"Vz.Format({a}, {b}{extraArgs})",
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
        private static string Unescape(string s) => s.Replace("\\\"", "\"").Replace("\\\\", "\\").Replace("\\n", "\n");
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
            _pendingTopLevelHeader = null;
            _pendingInstructionMetadata = null;
            _pendingTopLevelPosition = null;
            _preferTextConstantsForAuthoring =
                !code.Contains("VZEL", StringComparison.Ordinal) &&
                !code.Contains("Vz.RawConstant(", StringComparison.Ordinal) &&
                !code.Contains("Vz.RawVariable(", StringComparison.Ordinal) &&
                !code.Contains("Vz.RawCraftProperty(", StringComparison.Ordinal) &&
                !code.Contains("Vz.RawEval(", StringComparison.Ordinal);
            _listVariables.Clear();
            var lines = code.Split('\n').Select(l => l.TrimEnd()).ToList();
            string resolvedProgramName = programName;
            foreach (var rawLine in lines)
            {
                var initMatch = System.Text.RegularExpressions.Regex.Match(rawLine.Trim(),
                    @"^Vz\.Init\(""((?:\\.|[^""])*)""\)\s*;?\s*$");
                if (initMatch.Success)
                {
                    resolvedProgramName = Unescape(initMatch.Groups[1].Value);
                    break;
                }
            }
            var program = new XElement("Program", new XAttribute("name", resolvedProgramName));

            var variables = new XElement("Variables");
            var declaredVariables = new HashSet<string>(StringComparer.Ordinal); // Vizzy is case-sensitive: f ≠ F

            // Collect instruction blocks: one per event/CI, plus a preamble block
            var instructionBlocks = new List<XElement>();
            var preamble = new XElement("_preamble");
            var ceElements = new List<XElement>(); // CustomExpression declarations

            // Pre-scan 1: build CI name map (sanitized C# identifier → original CI name).
            // This lets us recognise calls like "Universal_Anomaly(a, b, c);" as
            // <CallCustomInstruction call="Universal Anomaly">.
            _ciNameMap = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var rawLine in lines)
            {
                var preCI = System.Text.RegularExpressions.Regex.Match(rawLine.Trim(),
                    @"^var\s+(\w+)\s*=\s*Vz\.DeclareCustomInstruction\(""([^""]+)""");
                if (preCI.Success)
                    _ciNameMap[preCI.Groups[1].Value] = preCI.Groups[2].Value;
                var preCE = System.Text.RegularExpressions.Regex.Match(rawLine.Trim(),
                    @"^var\s+(\w+)\s*=\s*Vz\.DeclareCustomExpression\(""([^""]+)""");
                if (preCE.Success)
                    _ciNameMap[preCE.Groups[1].Value] = preCE.Groups[2].Value; // CE calls look identical to CI calls
            }

            // Pre-scan 2: extract variable declarations from "// var X = N;" comment lines.
            // These are emitted by ConvertProgramToCode and are the authoritative source
            // for the <Variables> section.  Do this before the main loop so runtime
            // assignments inside lambdas are never misidentified as declarations.
            foreach (var rawLine in lines)
            {
                var varCmt = System.Text.RegularExpressions.Regex.Match(
                    rawLine.Trim(), @"^//\s*var\s+(\w+)\s*=\s*(.+);");
                if (!varCmt.Success) continue;
                string vname = varCmt.Groups[1].Value;
                string vval  = varCmt.Groups[2].Value.Trim();
                if (declaredVariables.Contains(vname)) continue;
                bool isList = vval.StartsWith("[]");
                if (isList)
                    _listVariables.Add(vname);
                variables.Add(isList
                    ? new XElement("Variable", new XAttribute("name", vname), new XElement("Items"))
                    : CreateVariableDeclaration(vname, vval));
                declaredVariables.Add(vname);
            }

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (line == "// VZTOPBLOCK")
                {
                    if (preamble.HasElements)
                    {
                        var pb = new XElement("Instructions");
                        foreach (var e in preamble.Elements().ToList()) pb.Add(new XElement(e));
                        instructionBlocks.Add(pb);
                        preamble = new XElement("_preamble");
                    }
                    continue;
                }

                if (line.StartsWith("// VZBLOCK ", StringComparison.Ordinal))
                {
                    if (preamble.HasElements)
                    {
                        var pb = new XElement("Instructions");
                        foreach (var e in preamble.Elements().ToList()) pb.Add(new XElement(e));
                        instructionBlocks.Add(pb);
                        preamble = new XElement("_preamble");
                    }

                    string payload = line.Substring("// VZBLOCK ".Length).Trim();
                    try
                    {
                        string xml = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
                        _pendingTopLevelHeader = XElement.Parse(xml);
                    }
                    catch
                    {
                        _pendingTopLevelHeader = null;
                    }
                    continue;
                }

                if (line.StartsWith("// VZEL ", StringComparison.Ordinal))
                {
                    string payload = line.Substring("// VZEL ".Length).Trim();
                    try
                    {
                        string xml = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
                        _pendingInstructionMetadata = XElement.Parse(xml);
                    }
                    catch
                    {
                        _pendingInstructionMetadata = null;
                    }
                    continue;
                }

                if (TryParseTopLevelPositionComment(line, out var topLevelPos))
                {
                    _pendingTopLevelPosition = topLevelPos;
                    continue;
                }

                // Preserve top-level comment blocks; only skip system/meta comments.
                if (line.StartsWith("//", StringComparison.Ordinal))
                {
                    bool isTopLevelSeparator =
                        line.StartsWith("// var ", StringComparison.Ordinal) ||
                        line.StartsWith("// [TODO]", StringComparison.Ordinal) ||
                        line.StartsWith("// Program ", StringComparison.Ordinal) ||
                        System.Text.RegularExpressions.Regex.IsMatch(line, @"^// [─═].*[─═]\s*$");
                    if (!isTopLevelSeparator)
                    {
                        string commentText = line.Length > 3 ? line.Substring(3) : line.Substring(2);
                        AddPreambleInstruction(preamble, ApplyPendingInstructionMetadata(new XElement("Comment",
                            new XAttribute("style", "comment"),
                            new XElement("Constant",
                                new XAttribute("style", "comment-text"),
                                new XAttribute("canReplace", "false"),
                                new XAttribute("text", commentText)))));
                    }
                    continue;
                }

                // ── DeclareCustomInstruction block ──────────────────────────────
                // Pattern emitted by ConvertProgramToCode:
                //   var <id> = Vz.DeclareCustomInstruction("<name>", "p1", ...).SetInstructions((<p1>, ...) =>
                //   {
                //       <body>
                //   });
                var ciMatch = System.Text.RegularExpressions.Regex.Match(line,
                    @"var\s+\w+\s*=\s*Vz\.DeclareCustomInstruction\(""([^""]+)""((?:\s*,\s*""[^""]*"")*)\s*,?\s*\)\s*\.SetInstructions\(\(([^)]*)\)\s*=>");
                if (ciMatch.Success)
                {
                    string ciName   = ciMatch.Groups[1].Value;
                    string paramStr = ciMatch.Groups[3].Value; // "p1, p2, ..."
                    var paramNames  = paramStr.Split(',')
                        .Select(p => p.Trim()).Where(p => p.Length > 0).ToList();

                    string callFormat = ciName + string.Concat(
                        paramNames.Select((p, idx) => $" ({idx})")) +
                        (paramNames.Count > 0 ? " " : "");
                    string format = ciName + string.Concat(
                        paramNames.Select(p => $" |{p}|")) +
                        (paramNames.Count > 0 ? " " : "");

                    var ciBlock = new XElement("Instructions");
                    XElement ciHeader;
                    if (_pendingTopLevelHeader?.Name.LocalName == "CustomInstruction" &&
                        string.Equals(_pendingTopLevelHeader.Attribute("name")?.Value, ciName, StringComparison.Ordinal))
                    {
                        ciHeader = new XElement(_pendingTopLevelHeader);
                    }
                    else
                    {
                        ciHeader = new XElement("CustomInstruction",
                            new XAttribute("callFormat", callFormat),
                            new XAttribute("format",     format),
                            new XAttribute("name",       ciName),
                            new XAttribute("style",      "custom-instruction"));
                    }
                    ciHeader = ApplyPendingTopLevelPosition(ciHeader);
                    _pendingTopLevelHeader = null;
                    ciBlock.Add(ciHeader);

                    // Extract the CI body using brace-depth counting so that we stop
                    // at the MATCHING `});` rather than the first bare `}` (which would
                    // be the closing brace of a nested While/If inside the body).
                    int ciBodyStart = i + 1;
                    // find the opening `{`
                    while (ciBodyStart < lines.Count && lines[ciBodyStart].Trim() != "{")
                        ciBodyStart++;
                    if (ciBodyStart < lines.Count)
                    {
                        // Extract lines between the opening `{` and the matching `}`
                        // (the `}` that precedes `);`).
                        var bodyLines = new List<string>();
                        int depth = 1;
                        int j = ciBodyStart + 1; // first line inside the block
                        for (; j < lines.Count && depth > 0; j++)
                        {
                            string bl = lines[j].Trim();
                            if (bl == "{")       depth++;
                            else if (bl == "}")  { depth--; if (depth == 0) break; }
                            else if (bl == "});" || bl == "})") { depth--; break; }
                            if (depth > 0) bodyLines.Add(lines[j]);
                        }
                        // Parse the extracted body lines as instructions.
                        // Set _localVariables so Variable references that are CI
                        // parameters get local="true" in the output XML.
                        // Use Ordinal (case-sensitive) — Vizzy names are case-sensitive
                        // and e.g. param "deltatime" must not match global "DeltaTime".
                        _localVariables = new HashSet<string>(
                            paramNames, StringComparer.Ordinal);
                        int bodyIndex = 0;
                        ParseInstructionLines(bodyLines, ref bodyIndex, ciBlock, stopAtClosingBrace: false);
                        _localVariables = new HashSet<string>();
                        i = j; // advance outer loop past the closing `}` / `});`
                    }

                    instructionBlocks.Add(ciBlock);
                    continue;
                }

                // ── DeclareCustomExpression block ───────────────────────────────
                // Pattern: var <id> = Vz.DeclareCustomExpression("<name>", ...).SetReturn((<p>) =>
                var ceMatch = System.Text.RegularExpressions.Regex.Match(line,
                    @"var\s+\w+\s*=\s*Vz\.DeclareCustomExpression\(""([^""]+)""((?:\s*,\s*""[^""]*"")*)\s*,?\s*\)\s*\.SetReturn\(\(([^)]*)\)\s*=>");
                if (ceMatch.Success)
                {
                    string ceName    = ceMatch.Groups[1].Value;
                    string ceParamStr = ceMatch.Groups[3].Value;
                    var ceParams = ceParamStr.Split(',').Select(p => p.Trim()).Where(p => p.Length > 0).ToList();

                    // Build callFormat and format attributes
                    string ceCallFmt = ceName + string.Concat(ceParams.Select((_, idx) => $" ({idx})")) +
                        (ceParams.Count > 0 ? " " : "");
                    string ceFmt     = ceName + string.Concat(ceParams.Select(p => $" |{p}|")) +
                        (ceParams.Count > 0 ? "  return (0)" : " return (0)");

                    // Extract body lines between { and });
                    int ceBodyStart = i + 1;
                    while (ceBodyStart < lines.Count && lines[ceBodyStart].Trim() != "{") ceBodyStart++;

                    string returnExprCode = "0";
                    int j2 = ceBodyStart + 1;
                    if (ceBodyStart < lines.Count)
                    {
                        int depth2 = 1;
                        for (; j2 < lines.Count && depth2 > 0; j2++)
                        {
                            string bl = lines[j2].Trim();
                            if (bl == "{") depth2++;
                            else if (bl == "}") { depth2--; if (depth2 == 0) break; }
                            else if (bl == "});" || bl == "})") { depth2--; break; }
                            else if (depth2 == 1 && bl.StartsWith("return ") && bl.EndsWith(";"))
                                returnExprCode = bl.Substring(7, bl.Length - 8).Trim();
                        }
                    }

                    _localVariables = new HashSet<string>(ceParams, StringComparer.Ordinal);
                    var returnXml = ConvertApiCallToXml(returnExprCode);
                    if (returnXml == null && returnExprCode != "0")
                        returnXml = CreateVariableReference(returnExprCode);
                    _localVariables = new HashSet<string>();

                    XElement ceEl;
                    if (_pendingTopLevelHeader?.Name.LocalName == "CustomExpression")
                    {
                        var preservedChildren = _pendingTopLevelHeader.Elements()
                            .Where(e => e.Name.LocalName == "Parameter")
                            .Select(e => new XElement(e))
                            .Cast<object>()
                            .ToList();
                        if (returnXml != null)
                            preservedChildren.Add(returnXml);
                        ceEl = new XElement("CustomExpression",
                            _pendingTopLevelHeader.Attributes(),
                            preservedChildren);
                    }
                    else
                    {
                        ceEl = new XElement("CustomExpression",
                            new XAttribute("callFormat", ceCallFmt),
                            new XAttribute("format",     ceFmt),
                            new XAttribute("name",       ceName),
                            new XAttribute("style",      "custom-expression"));
                        if (returnXml != null) ceEl.Add(returnXml);
                    }
                    ceEl = ApplyPendingTopLevelPosition(ceEl);
                    _pendingTopLevelHeader = null;
                    ceElements.Add(ceEl);
                    i = j2;
                    continue;
                }

                // ── Event handler block ─────────────────────────────────────────
                if (line.StartsWith("using (new "))
                {
                    var blockMatch = System.Text.RegularExpressions.Regex.Match(
                        line, @"using\s+\(new\s+((?:\w+\.)?(\w+))\((.*)\)\)");
                    if (blockMatch.Success)
                    {
                        // Group 2 is the unqualified type name (after optional "Vz." prefix)
                        string blockType = blockMatch.Groups[2].Value;
                        string blockArgs = blockMatch.Groups[3].Value;

                        if (IsEventType(blockType))
                        {
                            if (preamble.HasElements)
                            {
                                var pb = new XElement("Instructions");
                                foreach (var e in preamble.Elements().ToList()) pb.Add(new XElement(e));
                                instructionBlocks.Add(pb);
                                preamble = new XElement("_preamble");
                            }

                            var instrBlock  = new XElement("Instructions");
                            XElement? eventHeader = null;
                            if (_pendingTopLevelHeader?.Name.LocalName == "Event")
                                eventHeader = new XElement(_pendingTopLevelHeader);
                            else
                                eventHeader = BuildEventHeaderElement(blockType, blockArgs);
                            if (eventHeader != null)
                                eventHeader = ApplyPendingTopLevelPosition(eventHeader);
                            _pendingTopLevelHeader = null;
                            if (eventHeader != null) instrBlock.Add(eventHeader);

                            int start = FindBlockStart(lines, i + 1);
                            if (start >= 0)
                            {
                                int bodyIndex = start;
                                ParseInstructionLines(lines, ref bodyIndex, instrBlock, stopAtClosingBrace: true);
                                i = bodyIndex;
                            }
                            instructionBlocks.Add(instrBlock);
                        }
                        else
                        {
                            var block = ConvertBlockToXml(blockType, blockArgs, lines, ref i);
                            AddPreambleInstruction(preamble, block);
                        }
                    }
                    continue;
                }

                // ── Runtime assignments  ────────────────────────────────────────
                // Only treated as a variable DECLARATION when not already declared via
                // a "// var" comment and the value is a plain literal.
                if (System.Text.RegularExpressions.Regex.IsMatch(line, @"^(\w+)\s*=(?!=)"))
                {
                    var assignMatch = System.Text.RegularExpressions.Regex.Match(
                        line, @"^(\w+)\s*=\s*(.+);$");
                    if (assignMatch.Success)
                    {
                        string varName  = assignMatch.Groups[1].Value;
                        string varValue = assignMatch.Groups[2].Value;

                        bool alreadyDeclared = declaredVariables.Contains(varName);
                        bool isExpr          = IsVizzyExpression(varValue);

                        if (alreadyDeclared || isExpr)
                        {
                            var setVar = ConvertSetVariableToXml(varName, varValue);
                            AddPreambleInstruction(preamble, setVar);
                        }
                        else
                        {
                            // Plain literal not seen before — declare it
                            variables.Add(CreateVariableDeclaration(varName, varValue));
                            declaredVariables.Add(varName);
                        }
                    }
                    continue;
                }

                // ── Top-level Vz.* calls ────────────────────────────────────────
                if (line.StartsWith("Vz."))
                {
                    if (line.StartsWith("Vz.Init(")) continue;
                    if (TryAddInstructionAliases(line, preamble))
                        continue;

                    var instruction = ConvertInstructionToXml(line);
                    AddPreambleInstruction(preamble, instruction);
                }
            }

            // Flush remaining preamble (program with no events)
            if (preamble.HasElements)
            {
                var pb = new XElement("Instructions");
                foreach (var e in preamble.Elements().ToList()) pb.Add(new XElement(e));
                instructionBlocks.Add(pb);
            }
            if (instructionBlocks.Count == 0)
                instructionBlocks.Add(new XElement("Instructions"));

            program.Add(variables);
            foreach (var block in instructionBlocks)
                program.Add(block);
            var expressionsEl = new XElement("Expressions");
            foreach (var ce in ceElements) expressionsEl.Add(ce);
            program.Add(expressionsEl);

            // Post-process: assign sequential id and pos to all instruction-level elements
            AssignIdsAndPositions(program);

            return new XDocument(program);
        }

        /// <summary>
        /// Returns true if blockType represents a top-level event (OnStart, OnEnd, etc.)
        /// These become the first element of a new <Instructions> block, NOT nested inside Event.
        /// </summary>
        private static bool IsEventType(string blockType) =>
            blockType.ToLowerInvariant() is "onstart" or "onend" or "onreceivemessage"
                or "ondocked" or "onchangesoi" or "onpartcollision" or "onpartexplode"
                or "on";

        /// <summary>
        /// Builds the <Event .../> header element (self-closing, no body children).
        /// The event body becomes flat siblings in the containing <Instructions> block.
        /// </summary>
        private XElement? BuildEventHeaderElement(string blockType, string blockArgs)
        {
            string arg = string.IsNullOrWhiteSpace(blockArgs) ? "" : blockArgs.Trim();
            return blockType.ToLowerInvariant() switch
            {
                "onstart" => new XElement("Event",
                    new XAttribute("event", "FlightStart"),
                    new XAttribute("style", "flight-start")),
                "onend" => new XElement("Event",
                    new XAttribute("event", "FlightEnd"),
                    new XAttribute("style", "flight-end")),
                "onreceivemessage" => new XElement("Event",
                    new XAttribute("event", "ReceiveMessage"),
                    new XAttribute("style", "receive-msg"),
                    CreateConstant(trimQuotes(arg), forceText: true, canReplace: false)),
                "ondocked" => new XElement("Event",
                    new XAttribute("event", "Docked"),
                    new XAttribute("style", "craft-docked")),
                "onchangesoi" => new XElement("Event",
                    new XAttribute("event", "ChangeSoi"),
                    new XAttribute("style", "change-soi")),
                "onpartcollision" => new XElement("Event",
                    new XAttribute("event", "PartCollision"),
                    new XAttribute("style", "part-collision"),
                    new XAttribute("part", trimQuotes(arg))),
                "onpartexplode" => new XElement("Event",
                    new XAttribute("event", "PartExplode"),
                    new XAttribute("style", "part-explode"),
                    new XAttribute("part", trimQuotes(arg))),
                "on" => new XElement("Event",
                    new XAttribute("event", trimQuotes(arg)),
                    new XAttribute("style", trimQuotes(arg).ToLower().Replace(" ", "-"))),
                _ => null
            };
        }

        /// <summary>
        /// Assigns sequential id and pos attributes to all instruction-level elements
        /// (direct children of every <Instructions> block, recursively).
        /// Expression-level nodes (Constant, BinaryOp, CraftProperty, etc.) are not modified.
        /// </summary>
        private static void AssignIdsAndPositions(XElement program)
        {
            int nextId = 0;
            int blockPosX = -10;
            const int BlockXSpacing = 300;
            const int InstrYSpacing = 60;

            foreach (var instrBlock in program.Elements("Instructions"))
            {
                int posY = -20;
                bool isFirstInBlock = true;
                foreach (var instr in instrBlock.Elements())
                {
                    AssignNodeIdsRecursive(instr, ref nextId, blockPosX, posY, assignPos: isFirstInBlock);
                    isFirstInBlock = false;
                    posY += InstrYSpacing;
                }
                blockPosX += BlockXSpacing;
            }

            var expressions = program.Element("Expressions");
            if (expressions != null)
            {
                int exprPosX = blockPosX;
                int exprPosY = -20;
                foreach (var expr in expressions.Elements("CustomExpression"))
                {
                    if (expr.Attribute("pos") == null)
                    {
                        expr.Add(new XAttribute("pos", $"{exprPosX},{exprPosY}"));
                    }

                    exprPosY += InstrYSpacing;
                }
            }
        }

        private static void AssignNodeIdsRecursive(XElement el, ref int nextId, int posX, int posY, bool assignPos)
        {
            bool hadOriginalId = el.Attribute("id") != null;
            if (hadOriginalId)
            {
                if (int.TryParse(el.Attribute("id")?.Value, out int existingId) && existingId >= nextId)
                    nextId = existingId + 1;
            }
            else
            {
                el.Add(new XAttribute("id", nextId));
                nextId++;
            }

            if (assignPos && el.Attribute("pos") == null)
            {
                el.Add(new XAttribute("pos", $"{posX},{posY}"));
            }

            // Recurse into nested <Instructions> bodies (If/While/Repeat/For bodies)
            int nestedPosY = posY + InstrNestYOffset;
            foreach (var nested in el.Elements("Instructions"))
            {
                foreach (var nestedInstr in nested.Elements())
                {
                    AssignNodeIdsRecursive(nestedInstr, ref nextId, posX + InstrNestXOffset, nestedPosY, assignPos: false);
                    nestedPosY += NestedInstrYSpacing;
                }
            }
        }

        private const int InstrNestYOffset = 30;
        private const int InstrNestXOffset = 20;
        private const int NestedInstrYSpacing = 50;

        private static bool TryParseTopLevelPositionComment(string line, out (int X, int Y) position)
        {
            position = default;
            var match = System.Text.RegularExpressions.Regex.Match(
                line,
                @"^//\s*VZPOS\s+x\s*=\s*(-?\d+)\s+y\s*=\s*(-?\d+)\s*$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (!match.Success)
                return false;

            if (!int.TryParse(match.Groups[1].Value, out int x) ||
                !int.TryParse(match.Groups[2].Value, out int y))
                return false;

            position = (x, y);
            return true;
        }

        private static bool TryParsePositionAttribute(string value, out int x, out int y)
        {
            x = 0;
            y = 0;
            var parts = value.Split(',');
            if (parts.Length != 2)
                return false;

            return int.TryParse(parts[0], out x) && int.TryParse(parts[1], out y);
        }

        private XElement ApplyPendingTopLevelPosition(XElement element)
        {
            if (_pendingTopLevelPosition is { } pos && element.Attribute("pos") == null)
                element.Add(new XAttribute("pos", $"{pos.X},{pos.Y}"));

            _pendingTopLevelPosition = null;
            return element;
        }

        private void AddPreambleInstruction(XElement preamble, XElement? element)
        {
            if (element == null)
                return;

            if (!preamble.HasElements)
                element = ApplyPendingTopLevelPosition(element);

            preamble.Add(element);
        }

        private XElement? ConvertBlockToXml(string blockType, string blockArgs, List<string> lines, ref int index)
        {
            var body = new XElement("Instructions");
            var pendingBlockMetadata = _pendingInstructionMetadata;
            _pendingInstructionMetadata = null;

            // Check for inline empty body: "using (new Foo(...)) { }" on the same line.
            // In that case the body is empty and the next lines belong to the parent scope.
            string currentLine = index < lines.Count ? lines[index].Trim() : "";
            bool inlineEmpty = currentLine.EndsWith("{ }") || currentLine.EndsWith("{}");

            if (!inlineEmpty)
            {
                int start = FindBlockStart(lines, index + 1);
                if (start >= 0)
                {
                    int bodyIndex = start;
                    ParseInstructionLines(lines, ref bodyIndex, body, stopAtClosingBrace: true);
                    index = bodyIndex;
                }
            }

            _pendingInstructionMetadata = pendingBlockMetadata;
            return BuildBlockElement(blockType, blockArgs, body);
        }

        private XElement? BuildBlockElement(string blockType, string blockArgs, XElement body)
        {
            string condArg = string.IsNullOrWhiteSpace(blockArgs) ? "0" : blockArgs.Trim();
            var element = blockType.ToLower() switch
            {
                // Events: these are handled by BuildEventHeaderElement in the new ConvertCodeToXml path.
                // If they arrive here (nested context), produce the header with body attached for fallback.
                "onstart" => new XElement("Event", new XAttribute("event", "FlightStart"), new XAttribute("style", "flight-start"), body),
                "onend" => new XElement("Event", new XAttribute("event", "FlightEnd"), new XAttribute("style", "flight-end"), body),
                "onreceivemessage" => new XElement("Event", new XAttribute("event", "ReceiveMessage"),
                    new XAttribute("style", "receive-msg"), CreateConstant(trimQuotes(condArg), forceText: true, canReplace: false), body),
                "ondocked" => new XElement("Event", new XAttribute("event", "Docked"), new XAttribute("style", "craft-docked"), body),
                "onchangesoi" => new XElement("Event", new XAttribute("event", "ChangeSoi"), new XAttribute("style", "change-soi"), body),
                "onpartcollision" => new XElement("Event", new XAttribute("event", "PartCollision"),
                    new XAttribute("style", "part-collision"), new XAttribute("part", trimQuotes(condArg)), body),
                "onpartexplode" => new XElement("Event", new XAttribute("event", "PartExplode"),
                    new XAttribute("style", "part-explode"), new XAttribute("part", trimQuotes(condArg)), body),
                // Control-flow blocks — these have a condition and a nested <Instructions> body
                "if" => new XElement("If", new XAttribute("style", "if"), ConvertValueToXml(condArg), body),
                "elseif" => new XElement("ElseIf", new XAttribute("style", "else-if"), ConvertValueToXml(condArg), body),
                "else" => new XElement("ElseIf",
                    new XAttribute("style", "else"),
                    new XElement("Constant", new XAttribute("bool", "true")),
                    body),
                "while" => new XElement("While", new XAttribute("style", "while"), ConvertValueToXml(condArg), body),
                // WaitUntil has ONLY a condition child — NO nested <Instructions>
                "waituntil" => new XElement("WaitUntil", new XAttribute("style", "wait-until"), ConvertValueToXml(condArg)),
                "repeat" => new XElement("Repeat", new XAttribute("style", "repeat"), ConvertValueToXml(condArg), body),
                "for" => ParseForBlock(blockArgs, body),
                _ => null
            };
            return element != null ? ApplyPendingInstructionMetadata(element) : null;
        }

        private XElement ParseForBlock(string blockArgs, XElement body)
        {
            // blockArgs may be "\"i\".From(0).To(10).By(1)" or just the variable name
            string variable = "i";
            string from = "0", to = "10", step = "1";
            var varMatch = System.Text.RegularExpressions.Regex.Match(blockArgs, "\"(\\w+)\"");
            if (varMatch.Success) variable = varMatch.Groups[1].Value;
            from = TryExtractChainedCallArgument(blockArgs, "From") ?? from;
            to = TryExtractChainedCallArgument(blockArgs, "To") ?? to;
            step = TryExtractChainedCallArgument(blockArgs, "By") ?? step;
            return ApplyPendingInstructionMetadata(new XElement("For", new XAttribute("var", variable), new XAttribute("style", "for"),
                ConvertValueToXml(from),
                ConvertValueToXml(to),
                ConvertValueToXml(step),
                body));
        }

        private XElement? ConvertSetVariableToXml(string varName, string varValue)
        {
            string trimmedValue = varValue.Trim();
            if (trimmedValue.StartsWith("Vz.UserInput("))
            {
                return ApplyPendingInstructionMetadata(new XElement("UserInput",
                    new XAttribute("style", "user-input"),
                    CreateVariableReference(varName),
                    ConvertValueToXml(ExtractParenthesisContent(trimmedValue))));
            }

            var setVar = new XElement("SetVariable", new XAttribute("style", "set-variable"));

            var varRef = CreateVariableReference(varName);
            setVar.Add(varRef);

            setVar.Add(ConvertValueToXml(trimmedValue));

            return ApplyPendingInstructionMetadata(setVar);
        }

        private XElement? ConvertInstructionToXml(string line)
        {
            // Strip trailing semicolon for matching
            string l = line.TrimEnd(';', ' ');

            // ── CustomInstruction call ──────────────────────────────────────────
            // Pattern: SanitizedName(arg1, arg2, ...);
            // Only match if the sanitized name is in our CI registry.
            var ciCallM = System.Text.RegularExpressions.Regex.Match(l, @"^(\w+)\((.*)\)$");
            if (ciCallM.Success && _ciNameMap.TryGetValue(ciCallM.Groups[1].Value, out string? ciOrigName))
            {
                var ciEl = new XElement("CallCustomInstruction",
                    new XAttribute("call",  ciOrigName),
                    new XAttribute("style", "call-custom-instruction"));
                var callArgs = SplitArgs(ciCallM.Groups[2].Value);
                foreach (var a in callArgs)
                    ciEl.Add(ConvertValueToXml(a.Trim()));
                return ciEl;
            }

            // Zero-argument instructions
            if (l == "Vz.ActivateStage()") return new XElement("ActivateStage", new XAttribute("style", "activate-stage"));
            if (l == "Vz.Break()") return new XElement("Break", new XAttribute("style", "break"));
            if (l == "Vz.Beep()") return new XElement("Beep", new XAttribute("style", "play-beep"));
            if (l == "Vz.DestroyAllWidgets()") return new XElement("DestroyAllWidgets", new XAttribute("style", "destroy-all-mfd-widgets"));

            // One-argument instructions
            string c1 = ExtractParenthesisContent(l);
            if (l.StartsWith("Vz.Log("))
                return new XElement("Log", new XAttribute("style", "log"), ConvertValueToXml(c1));
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
            if (l.StartsWith("Vz.Display(") || l.StartsWith("Vz.DisplayMessage("))
            {
                var parts = SplitArgs(c1);
                var messageArg = ConvertValueToXml(parts.Count > 0 ? parts[0] : "\"\"");
                var durationArg = parts.Count > 1 ? parts[1] : "7";
                return new XElement("DisplayMessage", new XAttribute("style", "display"),
                    messageArg,
                    ConvertValueToXml(durationArg));
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
                return new XElement("SetInput",
                    new XAttribute("input", "throttle"),
                    new XAttribute("style", "set-input"),
                    MakeConstantArg(c1));
            if (l.StartsWith("Vz.SetTimeMode("))
                return new XElement("SetTimeMode",
                    new XAttribute("style", "set-time-mode"),
                    MakeConstantArg(c1));
            if (l.StartsWith("Vz.SetTimeModeAttr("))
                return new XElement("SetTimeMode",
                    new XAttribute("mode", trimQuotes(c1)),
                    new XAttribute("style", "set-time-mode"));
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
                bool isVector = indicator.Equals("Vector", StringComparison.OrdinalIgnoreCase);
                var el = new XElement("LockNavSphere",
                    new XAttribute("indicatorType", indicator),
                    new XAttribute("style", isVector ? "lock-nav-sphere-vector" : "lock-nav-sphere"));
                if (parts.Count > 1)
                    el.Add(ConvertValueToXml(parts[1]));
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
                string broadcastStyle = global ? "broadcast-msg-all-crafts" : (local ? "broadcast-msg" : "broadcast-msg-craft");
                var el = new XElement("BroadcastMessage", new XAttribute("style", broadcastStyle));
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
            if (l.StartsWith("Vz.ListRemoveValue("))
            {
                var parts = SplitArgs(c1);
                if (parts.Count >= 2)
                    return MakeListOp("Remove", new List<string> { parts[0], $"Vz.ListIndex({parts[0]}, {parts[1]})" });
            }
            if (l.StartsWith("Vz.ListSet("))    return MakeListOp("Set",    SplitArgs(c1));
            if (l.StartsWith("Vz.ListClear("))  return MakeListOp("Clear",  SplitArgs(c1));
            if (l.StartsWith("Vz.ListSort("))   return MakeListOp("Sort",   SplitArgs(c1));
            if (l.StartsWith("Vz.ListReverse("))return MakeListOp("Reverse",SplitArgs(c1));

            // UserInput: var = Vz.UserInput(prompt) — handled via assignment, but also as statement
            // Generic Vz. call fallback — treat as comment
            if (l.StartsWith("Vz."))
                return ApplyPendingInstructionMetadata(new XElement("Comment",
                    new XAttribute("style", "comment"),
                    CreateConstant($"[TODO] {l}", forceText: true)));

            return null;
        }

        private bool TryAddInstructionAliases(string line, XElement target)
        {
            string l = line.TrimEnd(';', ' ');

            // Authoring alias:
            // Vz.Wait(seconds)
            if (l.StartsWith("Vz.Wait(", StringComparison.Ordinal))
            {
                target.Add(new XElement("WaitSeconds",
                    new XAttribute("style", "wait-seconds"),
                    MakeConstantArg(ExtractParenthesisContent(l))));
                return true;
            }

            // Authoring alias:
            // Vz.WaitUntil(condition);
            if (l.StartsWith("Vz.WaitUntil(", StringComparison.Ordinal))
            {
                target.Add(new XElement("WaitUntil",
                    new XAttribute("style", "wait-until"),
                    MakeConstantArg(ExtractParenthesisContent(l))));
                return true;
            }

            // Authoring alias inspired by lizpy:
            // Vz.SetAutopilotMode(LockNavSphereIndicatorType.Prograde)
            // Vz.SetAutopilotMode("Prograde")
            if (l.StartsWith("Vz.SetAutopilotMode(", StringComparison.Ordinal))
            {
                string mode = ExtractParenthesisContent(l).Trim();
                if (mode.StartsWith("LockNavSphereIndicatorType.", StringComparison.Ordinal))
                    mode = mode.Substring("LockNavSphereIndicatorType.".Length);
                else
                    mode = trimQuotes(mode);

                var el = new XElement("LockNavSphere",
                    new XAttribute("indicatorType", mode),
                    new XAttribute("style", mode.Equals("Vector", StringComparison.OrdinalIgnoreCase)
                        ? "lock-nav-sphere-vector"
                        : "lock-nav-sphere"));

                if (mode.Equals("Vector", StringComparison.OrdinalIgnoreCase))
                    el.Add(CreateConstant("0"));

                target.Add(el);
                return true;
            }

            // Authoring alias:
            // Vz.LockHeading(heading, pitch)
            // Expand into the two instruction nodes Juno already understands.
            if (l.StartsWith("Vz.LockHeading(", StringComparison.Ordinal))
            {
                var parts = SplitArgs(ExtractParenthesisContent(l));
                if (parts.Count >= 2)
                {
                    target.Add(new XElement("SetTargetHeading",
                        new XAttribute("property", "heading"),
                        new XAttribute("style", "set-heading"),
                        MakeConstantArg(parts[0])));
                    target.Add(new XElement("SetTargetHeading",
                        new XAttribute("property", "pitch"),
                        new XAttribute("style", "set-heading"),
                        MakeConstantArg(parts[1])));
                    return true;
                }
            }

            return false;
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
            string opLower = op.ToLowerInvariant();
            // Instructions that mutate a list are <SetList>; expression reads are <ListOp>.
            var el = new XElement("SetList",
                new XAttribute("op", opLower),
                new XAttribute("style", $"list-{opLower}"));
            foreach (var p in parts) el.Add(MakeConstantArg(p));
            return el;
        }

        private List<string> SplitArgs(string argsStr)
        {
            var result = new List<string>();
            int depth = 0;
            bool inString = false;
            bool inVerbatim = false; // inside @"..." verbatim string
            bool escaped = false;
            var cur = new StringBuilder();
            int i = 0;
            while (i < argsStr.Length)
            {
                char c = argsStr[i];

                if (inVerbatim)
                {
                    cur.Append(c);
                    if (c == '"')
                    {
                        // doubled quote inside verbatim is an escaped quote
                        if (i + 1 < argsStr.Length && argsStr[i + 1] == '"')
                        {
                            i++;
                            cur.Append(argsStr[i]);
                        }
                        else
                        {
                            inVerbatim = false;
                        }
                    }
                    i++;
                    continue;
                }

                if (escaped)
                {
                    cur.Append(c);
                    escaped = false;
                    i++;
                    continue;
                }

                if (c == '\\' && inString)
                {
                    cur.Append(c);
                    escaped = true;
                    i++;
                    continue;
                }

                // Detect start of verbatim string: @"
                if (c == '@' && i + 1 < argsStr.Length && argsStr[i + 1] == '"' && !inString)
                {
                    cur.Append(c);
                    i++;
                    cur.Append(argsStr[i]); // the '"'
                    inVerbatim = true;
                    i++;
                    continue;
                }

                if (c == '"')
                {
                    inString = !inString;
                    cur.Append(c);
                    i++;
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
                    i++;
                    continue;
                }
                cur.Append(c);
                i++;
            }
            if (cur.Length > 0) result.Add(cur.ToString().Trim());
            return result;
        }

        private XElement? ConvertApiCallToXml(string call)
        {
            call = call.Trim().TrimEnd(';');

            while (HasOuterParentheses(call))
                call = call.Substring(1, call.Length - 2).Trim();

            if (TryDecodePreservedCall(call, "Vz.RawConstant", "Constant", out var rawConstantExpr) ||
                TryDecodePreservedCall(call, "Vz.RawXmlConstant", "Constant", out rawConstantExpr))
                return rawConstantExpr;
            if (TryDecodePreservedCall(call, "Vz.RawVariable", "Variable", out var rawVariableExpr) ||
                TryDecodePreservedCall(call, "Vz.RawXmlVariable", "Variable", out rawVariableExpr))
                return rawVariableExpr;
            if (TryDecodePreservedCall(call, "Vz.RawCraftProperty", "CraftProperty", out var rawCraftPropertyExpr) ||
                TryDecodePreservedCall(call, "Vz.RawXmlCraftProperty", "CraftProperty", out rawCraftPropertyExpr))
                return rawCraftPropertyExpr;

            // Numeric literal
            if (double.TryParse(call, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double numVal))
                return CreateConstant(numVal.ToString(System.Globalization.CultureInfo.InvariantCulture));

            // String literal
            if (call.StartsWith("\"") && call.EndsWith("\""))
                return CreateConstant(Unescape(trimQuotes(call)), forceText: true);

            // Boolean
            if (call == "true" || call == "false") return CreateConstant(call);

            if (call.StartsWith("!"))
            {
                var inner = ConvertApiCallToXml(call.Substring(1));
                if (inner != null)
                    return new XElement("Not", new XAttribute("style", "op-not"), inner);
            }

            // ── Ternary conditional: (condition ? trueVal : falseVal) ────────────
            // The outer-paren strip above may have already unwrapped one level;
            // scan for '?' and ':' at depth 0.
            {
                int qIdx = FindTopLevelOperator(call, "?");
                if (qIdx > 0)
                {
                    string condPart  = call.Substring(0, qIdx).Trim();
                    string remainder = call.Substring(qIdx + 1).Trim();
                    int    cIdx      = FindTopLevelOperator(remainder, ":");
                    if (cIdx > 0)
                    {
                        string truePart  = remainder.Substring(0, cIdx).Trim();
                        string falsePart = remainder.Substring(cIdx + 1).Trim();
                        var condEl = ConvertApiCallToXml(condPart)  ?? CreateVariableReference(condPart);
                        var trueEl = ConvertApiCallToXml(truePart)  ?? CreateVariableReference(truePart);
                        var falseEl= ConvertApiCallToXml(falsePart) ?? CreateVariableReference(falsePart);
                        return new XElement("Conditional",
                            new XAttribute("style", "conditional"),
                            condEl, trueEl, falseEl);
                    }
                }
            }

            // ── Vector component access: expr.x / expr.y / expr.z ───────────────
            // Matches when the expression ends with a literal ".x", ".y", ".z"
            // (not inside parentheses).
            {
                foreach (var component in new[] { ".x", ".y", ".z" })
                {
                    if (call.EndsWith(component, StringComparison.OrdinalIgnoreCase)
                        && call.Length > component.Length)
                    {
                        string vecPart = call.Substring(0, call.Length - component.Length).Trim();
                        // Only split here if the part before .x/y/z is a valid expression
                        // (not the end of a dotted identifier like "Vz.Craft.Nav.Position")
                        var vecEl = ConvertApiCallToXml(vecPart);
                        if (vecEl != null)
                        {
                            string axis = component.Substring(1); // "x", "y", or "z"
                            return new XElement("VectorOp",
                                new XAttribute("op",    axis),
                                new XAttribute("style", "vec-op-1"),
                                vecEl);
                        }
                    }
                }
            }

            // Vz.PosToLatLongAgl(vec) → <Planet op="toLatLongAgl" style="planet-to-lat-long-agl">
            if (call.StartsWith("Vz.PosToLatLongAgl("))
            {
                var pllArgs = SplitArgs(ExtractParenthesisContent(call));
                var pEl = pllArgs.Count > 0 ? (ConvertApiCallToXml(pllArgs[0]) ?? CreateVariableReference(pllArgs[0])) : CreateConstant("0");
                return new XElement("Planet", new XAttribute("op", "toLatLongAgl"), new XAttribute("style", "planet-to-lat-long-agl"), pEl);
            }
            if (call.StartsWith("Vz.PosToLatLongAsl("))
            {
                var pllArgs = SplitArgs(ExtractParenthesisContent(call));
                var pEl = pllArgs.Count > 0 ? (ConvertApiCallToXml(pllArgs[0]) ?? CreateVariableReference(pllArgs[0])) : CreateConstant("0");
                return new XElement("Planet", new XAttribute("op", "toLatLongAsl"), new XAttribute("style", "planet-to-lat-long-asl"), pEl);
            }
            if (call.StartsWith("Vz.ToPosition("))
            {
                var pllArgs = SplitArgs(ExtractParenthesisContent(call));
                var pEl = pllArgs.Count > 0 ? (ConvertApiCallToXml(pllArgs[0]) ?? CreateVariableReference(pllArgs[0])) : CreateConstant("0");
                return new XElement("Planet", new XAttribute("op", "toPosition"), new XAttribute("style", "planet-to-position"), pEl);
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
            // Word infix operators: "A max B", "A min B", "A rand B"
            if (TryConvertBinaryExpression(call, new[] { " max " }, CreateBinaryOpElement, out var maxWExpr))
                return maxWExpr;
            if (TryConvertBinaryExpression(call, new[] { " min " }, CreateBinaryOpElement, out var minWExpr))
                return minWExpr;
            if (TryConvertBinaryExpression(call, new[] { " rand " }, CreateBinaryOpElement, out var randWExpr))
                return randWExpr;

            // ── Vz.Planet(p).Property() ─────────────────────────────────────────
            if (TryConvertPlanetPropertyCall(call, out var planetExpr))
                return planetExpr;

            // Variable reference (plain identifier)
            if (System.Text.RegularExpressions.Regex.IsMatch(call, @"^[A-Za-z_]\w*$"))
                return CreateVariableReference(call);

            if (TryConvertFunctionCall(call, "Vz.RawVar",
                args => CreateVariableReference(Unescape(trimQuotes(args[0]))), 1, out var rawVarExpr))
                return rawVarExpr;

            // Named constants — must come BEFORE the generic dotted-name fallback below
            if (TryDecodePreservedCall(call, "Vz.RawEval", "EvaluateExpression", out var rawEvalExpr) ||
                TryDecodePreservedCall(call, "Vz.RawXmlEval", "EvaluateExpression", out rawEvalExpr))
                return rawEvalExpr;

            if (TryConvertFunctionCall(call, "Vz.ExactEval", args =>
            {
                string exactText = Unescape(trimQuotes(args[0]));
                return new XElement("EvaluateExpression",
                    new XAttribute("style", "evaluate-expression"),
                    new XElement("Constant", new XAttribute("text", exactText)));
            }, 1, out var exactEvalExpr))
                return exactEvalExpr;

            if (call == "Vz.Pi")       return new XElement("EvaluateExpression", new XAttribute("style", "evaluate-expression"), new XElement("Constant", new XAttribute("text", "pi")));
            if (call == "Vz.E")        return new XElement("EvaluateExpression", new XAttribute("style", "evaluate-expression"), new XElement("Constant", new XAttribute("text", "e")));
            if (call == "Vz.Infinity") return new XElement("EvaluateExpression", new XAttribute("style", "evaluate-expression"), new XElement("Constant", new XAttribute("text", "inf")));
            if (call == "Vz.NaN")      return new XElement("EvaluateExpression", new XAttribute("style", "evaluate-expression"), new XElement("Constant", new XAttribute("text", "nan")));

            if (System.Text.RegularExpressions.Regex.IsMatch(call, @"^[A-Za-z_]\w*(\.[A-Za-z_]\w*)+$"))
                return CreateConstant(call, forceText: true);

            if (TryConvertFunctionCall(call, "Vz.Random", args => CreateBinaryOpElement(("rand", args[0], args[1])), 2, out var randExpr)) return randExpr;
            if (TryConvertFunctionCall(call, "Vz.Min", args => CreateBinaryOpElement(("min", args[0], args[1])), 2, out var minExpr)) return minExpr;
            if (TryConvertFunctionCall(call, "Vz.Max", args => CreateBinaryOpElement(("max", args[0], args[1])), 2, out var maxExpr)) return maxExpr;
            if (TryConvertFunctionCall(call, "Vz.Atan2", args => CreateBinaryOpElement(("atan2", args[0], args[1])), 2, out var atan2Expr)) return atan2Expr;
            if (TryConvertFunctionCall(call, "Vz.Exp", args => CreateBinaryOpElement(("^", "Vz.E", args[0])), 1, out var expExpr)) return expExpr;
            if (TryConvertFunctionCall(call, "Vz.Sinh", args => CreateAdvancedMathAlias("sinh", args[0]), 1, out var sinhExpr)) return sinhExpr;
            if (TryConvertFunctionCall(call, "Vz.Cosh", args => CreateAdvancedMathAlias("cosh", args[0]), 1, out var coshExpr)) return coshExpr;
            if (TryConvertFunctionCall(call, "Vz.Tanh", args => CreateAdvancedMathAlias("tanh", args[0]), 1, out var tanhExpr)) return tanhExpr;
            if (TryConvertFunctionCall(call, "Vz.Asinh", args => CreateAdvancedMathAlias("asinh", args[0]), 1, out var asinhExpr)) return asinhExpr;
            if (TryConvertFunctionCall(call, "Vz.Acosh", args => CreateAdvancedMathAlias("acosh", args[0]), 1, out var acoshExpr)) return acoshExpr;
            if (TryConvertFunctionCall(call, "Vz.Atanh", args => CreateAdvancedMathAlias("atanh", args[0]), 1, out var atanhExpr)) return atanhExpr;

            // Vz.Vec(x, y, z)
            if (call.StartsWith("Vz.Vec("))
            {
                var parts = SplitArgs(ExtractParenthesisContent(call));
                var el = new XElement("Vector", new XAttribute("style", "vec"));
                foreach (var p in parts) { var c = ConvertApiCallToXml(p); if (c != null) el.Add(c); }
                return el;
            }

            if (TryConvertFunctionCall(call, "Vz.Join", args => CreateStringOp("join", args), 2, out var joinExpr)) return joinExpr;
            if (TryConvertFunctionCall(call, "Vz.StringOp", args =>
            {
                if (args.Count < 2) return CreateVariableReference(call);
                string op = trimQuotes(args[0]);
                return CreateStringOp(op, args.Skip(1).ToList());
            }, 2, out var stringOpExpr)) return stringOpExpr;
            if (TryConvertFunctionCall(call, "Vz.Concat", args => CreateStringOp("join", args), 2, out var concatExpr)) return concatExpr;
            if (TryConvertFunctionCall(call, "Vz.LengthOf", args => CreateStringOp("length", args), 1, out var lenExpr)) return lenExpr;
            if (TryConvertFunctionCall(call, "Vz.StringLength", args => CreateStringOp("length", args), 1, out var strLenExpr)) return strLenExpr;
            if (TryConvertFunctionCall(call, "Vz.LetterOf", args => CreateStringOp("letter", args), 2, out var letterExpr)) return letterExpr;
            if (TryConvertFunctionCall(call, "Vz.SubString", args => CreateStringOp("substring", args), 3, out var substringExpr)) return substringExpr;
            if (TryConvertFunctionCall(call, "Vz.SubString", args => CreateStringOp("substring", new[] { args[0], args[1], $"Vz.LengthOf({args[0]})" }), 2, out var substring2Expr)) return substring2Expr;
            if (TryConvertFunctionCall(call, "Vz.Contains", args => CreateStringOp("contains", args), 2, out var containsExpr)) return containsExpr;
            if (TryConvertFunctionCall(call, "Vz.Format", args => CreateStringOp("format", args), 2, out var formatExpr)) return formatExpr;

            if (TryConvertFunctionCall(call, "Vz.ListGet", args => CreateListExpression("get", args), 2, out var listGetExpr)) return listGetExpr;
            if (TryConvertFunctionCall(call, "Vz.ListLength", args => CreateListExpression("length", args), 1, out var listLengthExpr)) return listLengthExpr;
            if (TryConvertFunctionCall(call, "Vz.ListIndex", args => CreateListExpression("index", args), 2, out var listIndexExpr)) return listIndexExpr;
            if (TryConvertFunctionCall(call, "Vz.ListCreate", args => new XElement("ListOp",
                new XAttribute("op", "create"),
                new XAttribute("style", "list-create"),
                ConvertApiCallToXml(args[0]) ?? CreateConstant(Unescape(trimQuotes(args[0])), forceText: true)), 1, out var listCreateExpr)) return listCreateExpr;
            if (TryConvertFunctionCall(call, "Vz.CreateListRaw", args => new XElement("ListOp",
                new XAttribute("op", "create"),
                new XAttribute("style", "list-create"),
                ConvertApiCallToXml(args[0]) ?? CreateConstant(Unescape(trimQuotes(args[0])), forceText: true)), 1, out var createListRawExpr)) return createListRawExpr;
            if (call == "Vz.CreateList()") return new XElement("ListOp", new XAttribute("op", "create"), new XAttribute("style", "list-create"));

            if (TryConvertFunctionCall(call, "Vz.Length", args => CreateVectorOp("length", args), 1, out var vecLenExpr)) return vecLenExpr;
            if (TryConvertFunctionCall(call, "Vz.Norm", args => CreateVectorOp("norm", args), 1, out var vecNormExpr)) return vecNormExpr;
            if (TryConvertFunctionCall(call, "Vz.Normalize", args => CreateVectorOp("norm", args), 1, out var vecNormalizeExpr)) return vecNormalizeExpr;
            if (TryConvertFunctionCall(call, "Vz.X", args => CreateVectorOp("x", args), 1, out var vecXExpr)) return vecXExpr;
            if (TryConvertFunctionCall(call, "Vz.Y", args => CreateVectorOp("y", args), 1, out var vecYExpr)) return vecYExpr;
            if (TryConvertFunctionCall(call, "Vz.Z", args => CreateVectorOp("z", args), 1, out var vecZExpr)) return vecZExpr;
            if (TryConvertFunctionCall(call, "Vz.Dot", args => CreateVectorOp("dot", args), 2, out var vecDotExpr)) return vecDotExpr;
            if (TryConvertFunctionCall(call, "Vz.Cross", args => CreateVectorOp("cross", args), 2, out var vecCrossExpr)) return vecCrossExpr;
            if (TryConvertFunctionCall(call, "Vz.Angle", args => CreateVectorOp("angle", args), 2, out var vecAngleExpr)) return vecAngleExpr;
            if (TryConvertFunctionCall(call, "Vz.Distance", args => CreateVectorOp("dist", args), 2, out var vecDistExpr)) return vecDistExpr;
            if (TryConvertFunctionCall(call, "Vz.Project", args => CreateVectorOp("project", args), 2, out var vecProjectExpr)) return vecProjectExpr;
            if (TryConvertFunctionCall(call, "Vz.Scale", args => CreateVectorOp("scale", args), 2, out var vecScaleExpr)) return vecScaleExpr;
            if (TryConvertFunctionCall(call, "Vz.VecMin", args => CreateVectorOp("min", args), 2, out var vecMinExpr)) return vecMinExpr;
            if (TryConvertFunctionCall(call, "Vz.VecMax", args => CreateVectorOp("max", args), 2, out var vecMaxExpr)) return vecMaxExpr;
            if (TryConvertFunctionCall(call, "Vz.Clamp", args => CreateVectorOp("clamp", args), 2, out var vecClampExpr)) return vecClampExpr;

            if (TryConvertFunctionCall(call, "Vz.Craft.NameToID", args => new XElement("CraftProperty",
                new XAttribute("property", "Craft.NameToID"),
                new XAttribute("style", "craft-id"),
                ConvertApiCallToXml(args[0]) ?? CreateVariableReference(args[0])), 1, out var craftNameToIdExpr)) return craftNameToIdExpr;

            if (TryConvertFunctionCall(call, "Vz.PartNameToID", args => new XElement("CraftProperty",
                new XAttribute("property", "Part.NameToID"),
                new XAttribute("style", "part-id"),
                ConvertApiCallToXml(args[0]) ?? CreateVariableReference(args[0])), 1, out var partNameToIdExpr)) return partNameToIdExpr;
            if (TryConvertFunctionCall(call, "Vz.PartLocalToPci", args => {
                var cp = new XElement("PartLocalToPci", new XAttribute("style", "part-transform"));
                foreach (var a in args) cp.Add(ConvertApiCallToXml(a) ?? CreateVariableReference(a));
                return cp;
            }, 2, out var partLocalToPciExpr)) return partLocalToPciExpr;
            if (TryConvertFunctionCall(call, "Vz.PartPciToLocal", args => {
                var cp = new XElement("CraftProperty", new XAttribute("property", "Part.PciToLocal"), new XAttribute("style", "part-transform"));
                foreach (var a in args) cp.Add(ConvertApiCallToXml(a) ?? CreateVariableReference(a));
                return cp;
            }, 1, out var partPciToLocalExpr)) return partPciToLocalExpr;

            if (TryConvertCraftProperty(call, out var craftPropertyExpr))
                return craftPropertyExpr;

            if (TryConvertFunctionCall(call, "Vz.CraftInput", args =>
            {
                string inputType = trimQuotes(args[0]).Replace("CraftInput.", "");
                return new XElement("CraftProperty",
                    new XAttribute("property", "Input." + inputType),
                    new XAttribute("style", "prop-input"));
            }, 1, out var craftInputExpr))
                return craftInputExpr;

            // Vz.Craft.Property("propName") → CraftProperty property="propName"
            {
                var craftPropM = System.Text.RegularExpressions.Regex.Match(call, @"^Vz\.Craft\.Property\(""([^""]*)""\)$");
                if (craftPropM.Success)
                {
                    string prop = craftPropM.Groups[1].Value;
                    return new XElement("CraftProperty",
                        new XAttribute("property", prop),
                        new XAttribute("style", CraftPropertyStyle(prop)));
                }
            }

            // Vz.OtherCraft(craftId).Prop() → CraftOtherProperty
            if (call.StartsWith("Vz.OtherCraft("))
            {
                var otherM = System.Text.RegularExpressions.Regex.Match(call, @"^Vz\.OtherCraft\((.+)\)\.(\w+)\(\)$");
                if (otherM.Success)
                {
                    string craftIdStr = otherM.Groups[1].Value.Trim();
                    string propName   = otherM.Groups[2].Value;
                    var craftIdEl = ConvertApiCallToXml(craftIdStr) ?? CreateVariableReference(craftIdStr);
                    if (propName.Equals("Planet", StringComparison.OrdinalIgnoreCase))
                    {
                        return new XElement("CraftProperty",
                            new XAttribute("property", "Craft.Planet"),
                            new XAttribute("style", "craft"),
                            craftIdEl);
                    }
                    return new XElement("CraftOtherProperty",
                        new XAttribute("property", propName),
                        new XAttribute("style", CraftPropertyStyle(propName)),
                        craftIdEl);
                }
            }

            // Vz.Time.DeltaTime() etc → CraftProperty (Time.*)
            if (call.StartsWith("Vz.Time."))
            {
                // Already covered by CraftPropertyCallMap above, but handle any unknown ones
                string timeProp = call.Substring(8);
                if (timeProp.EndsWith("()")) timeProp = "Time." + timeProp.Substring(0, timeProp.Length - 2);
                else timeProp = "Time." + timeProp;
                return new XElement("CraftProperty",
                    new XAttribute("property", timeProp),
                    new XAttribute("style", "prop-time"));
            }

            // Vz.Craft.Nav.Position() → CraftProperty property="Nav.Position"
            // Strip trailing "()" so property names are clean.
            if (call.StartsWith("Vz.Craft."))
            {
                string prop = call.Substring(9);
                if (prop.EndsWith("()")) prop = prop.Substring(0, prop.Length - 2);
                return new XElement("CraftProperty",
                    new XAttribute("property", prop),
                    new XAttribute("style", CraftPropertyStyle(prop)));
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

            // Vz.Eval("expr") → <EvaluateExpression><Constant text="expr"/></EvaluateExpression>
            if (call.StartsWith("Vz.Eval("))
            {
                string evalContent = ExtractParenthesisContent(call).Trim();
                // Remove surrounding quotes if present
                if (evalContent.StartsWith("\"") && evalContent.EndsWith("\"") && evalContent.Length >= 2)
                    evalContent = evalContent.Substring(1, evalContent.Length - 2);
                return new XElement("EvaluateExpression",
                    new XAttribute("style", "evaluate-expression"),
                    new XElement("Constant", new XAttribute("text", Unescape(evalContent))));
            }
            // Named constants: Vz.Pi, Vz.E, Vz.Infinity, Vz.NaN
            if (call == "Vz.Pi")       return new XElement("EvaluateExpression", new XAttribute("style", "evaluate-expression"), new XElement("Constant", new XAttribute("text", "pi")));
            if (call == "Vz.E")        return new XElement("EvaluateExpression", new XAttribute("style", "evaluate-expression"), new XElement("Constant", new XAttribute("text", "e")));
            if (call == "Vz.Infinity") return new XElement("EvaluateExpression", new XAttribute("style", "evaluate-expression"), new XElement("Constant", new XAttribute("text", "inf")));
            if (call == "Vz.NaN")      return new XElement("EvaluateExpression", new XAttribute("style", "evaluate-expression"), new XElement("Constant", new XAttribute("text", "nan")));

            // Custom instruction CALL as an expression (when used as a value).
            // e.g. CallCustomExpression calls: Stump_C(z) where Stump_C is a CI expression.
            // Note: CI instruction calls are handled as statements in ConvertInstructionToXml.
            if (call.Contains("("))
            {
                string fnName = call.Substring(0, call.IndexOf('('));
                if (_ciNameMap.TryGetValue(fnName, out string? ciCallName))
                {
                    string argsStr = ExtractParenthesisContent(call);
                    var callArgs   = SplitArgs(argsStr);
                    var ciExpr = new XElement("CallCustomExpression",
                        new XAttribute("call", ciCallName),
                        new XAttribute("style", "call-custom-expression"));
                    foreach (var a in callArgs)
                    {
                        var argEl = ConvertApiCallToXml(a.Trim());
                        ciExpr.Add(argEl ?? CreateVariableReference(a.Trim()));
                    }
                    return ciExpr;
                }
            }

            return null;
        }

        private void ParseInstructionLines(List<string> lines, ref int index, XElement target, bool stopAtClosingBrace)
        {
            for (; index < lines.Count; index++)
            {
                string line = lines[index].Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line == "{")
                    continue;
                if (line == "}")
                {
                    if (stopAtClosingBrace)
                        return;
                    continue;
                }

                // ── Comment lines → <Comment> ──────────────────────────────────
                if (line.StartsWith("//"))
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(line, @"^// ──+\s*Program\s*:"))
                        continue;

                    if (line.StartsWith("// VZEL ", StringComparison.Ordinal))
                    {
                        string payload = line.Substring("// VZEL ".Length).Trim();
                        try
                        {
                            string xml = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
                            _pendingInstructionMetadata = XElement.Parse(xml);
                        }
                        catch
                        {
                            _pendingInstructionMetadata = null;
                        }
                        continue;
                    }

                    // Skip system/metadata comments (separators, var declarations, TODOs).
                    // Separator lines are generated with ─ or ═ characters and end in ─/═.
                    bool isSeparatorComment =
                        line.StartsWith("// var ", StringComparison.Ordinal) ||
                        line.StartsWith("// [TODO]", StringComparison.Ordinal) ||
                        line.StartsWith("// Program ", StringComparison.Ordinal) ||
                        System.Text.RegularExpressions.Regex.IsMatch(line, @"^// [─═].*[─═]\s*$");
                    if (!isSeparatorComment)
                    {
                        string commentText = line.Length > 3 ? line.Substring(3) : line.Substring(2);
                        target.Add(ApplyPendingInstructionMetadata(new XElement("Comment",
                            new XAttribute("style", "comment"),
                            new XElement("Constant",
                                new XAttribute("style", "comment-text"),
                                new XAttribute("canReplace", "false"),
                                new XAttribute("text", commentText)))));
                    }
                    continue;
                }

                // ── ChangeVariable: varName += expr; (and -=, *=, /=, %=, ^=) ──
                {
                    var chgM = System.Text.RegularExpressions.Regex.Match(line, @"^(\w+)\s*(\+|-|\*|/|%|\^)=\s*(.+);$");
                    if (chgM.Success)
                    {
                        string cvName = chgM.Groups[1].Value;
                        string cvOp   = chgM.Groups[2].Value;
                        string cvVal  = chgM.Groups[3].Value.Trim();
                        var cvVarEl = new XElement("Variable",
                            new XAttribute("list",  "false"),
                            new XAttribute("local", _localVariables.Contains(cvName) ? "true" : "false"),
                            new XAttribute("variableName", cvName));
                        // For +=, use ChangeVariable directly. For others, expand to set varName = varName op expr.
                        if (cvOp == "+")
                        {
                            var cvValEl = ConvertApiCallToXml(cvVal) ?? CreateVariableReference(cvVal);
                            target.Add(ApplyPendingInstructionMetadata(new XElement("ChangeVariable",
                                new XAttribute("style", "change-variable"),
                                cvVarEl, cvValEl)));
                        }
                        else
                        {
                            // Expand: varName op= expr → varName = varName op expr
                            string normalizedOp = NormalizeBinaryOp(cvOp);
                            var varRefEl = new XElement("Variable",
                                new XAttribute("list",  "false"),
                                new XAttribute("local", _localVariables.Contains(cvName) ? "true" : "false"),
                                new XAttribute("variableName", cvName));
                            var rhsEl = ConvertApiCallToXml(cvVal) ?? CreateVariableReference(cvVal);
                            var binEl = new XElement("BinaryOp",
                                new XAttribute("op", normalizedOp),
                                new XAttribute("style", BinaryStyle(normalizedOp)),
                                varRefEl, rhsEl);
                            var setEl = new XElement("SetVariable",
                                new XAttribute("style", "set-variable"),
                                cvVarEl, binEl);
                            target.Add(ApplyPendingInstructionMetadata(setEl));
                        }
                        continue;
                    }
                }

                if (line.StartsWith("using (new "))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(line, @"using\s+\(new\s+((?:\w+\.)?(\w+))\((.*)\)\)");
                    if (match.Success)
                    {
                        var block = ConvertBlockToXml(match.Groups[2].Value, match.Groups[3].Value, lines, ref index);
                        if (block != null)
                            target.Add(block);
                    }
                    continue;
                }

                if (System.Text.RegularExpressions.Regex.IsMatch(line, @"^(\w+)\s*=(?!=)"))
                {
                    var assignMatch = System.Text.RegularExpressions.Regex.Match(line, @"^(\w+)\s*=\s*(.+);$");
                    if (assignMatch.Success)
                    {
                        var setVar = ConvertSetVariableToXml(assignMatch.Groups[1].Value, assignMatch.Groups[2].Value);
                        if (setVar != null)
                            target.Add(setVar);
                        continue;
                    }
                }

                if (TryAddInstructionAliases(line, target))
                    continue;

                var instruction = ConvertInstructionToXml(line);
                if (instruction != null)
                    target.Add(ApplyPendingInstructionMetadata(instruction));
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

            // Preserve -0 literally (Vizzy uses -0 as the "uninitialized" sentinel)
            if (trimmed == "-0")
            {
                variable.Add(new XAttribute("number", "-0"));
                return variable;
            }
            if (double.TryParse(trimmed, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double num))
            {
                variable.Add(new XAttribute("number", num.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                return variable;
            }

            if (trimmed.StartsWith("\"") && trimmed.EndsWith("\""))
            {
                variable.Add(CreateConstant(Unescape(trimQuotes(trimmed)), forceText: true));
                return variable;
            }

            variable.Add(CreateConstant(trimmed, forceText: true));
            return variable;
        }

        private XElement CreateVariableReference(string variableName)
        {
            string name  = variableName.Trim();
            bool isLocal = _localVariables.Contains(name);
            bool isList = _listVariables.Contains(name);
            return new XElement("Variable",
                new XAttribute("list",  isList ? "true" : "false"),
                new XAttribute("local", isLocal ? "true" : "false"),
                new XAttribute("variableName", name));
        }

        private XElement CreateVariableOrExpression(string value)
        {
            return ConvertApiCallToXml(value) ?? CreateVariableReference(value);
        }

        private XElement CreateConstant(string value, bool forceText = false, bool canReplace = true)
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
                if (_preferTextConstantsForAuthoring)
                {
                    attrs.Add(new XAttribute("text", numVal.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                    return new XElement("Constant", attrs);
                }

                attrs.Add(new XAttribute("number", numVal.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                return new XElement("Constant", attrs);
            }

            string textValue = forceText ? StripOuterQuotesPreserveWhitespace(value) : trimQuotes(trimmed);
            attrs.Add(new XAttribute("text", textValue));
            return new XElement("Constant", attrs);
        }

        private XElement CreateNumericConstant(string value)
        {
            string trimmed = value.Trim();
            if (trimmed.StartsWith("\"") && trimmed.EndsWith("\""))
                trimmed = Unescape(trimQuotes(trimmed));

            return new XElement("Constant", new XAttribute("number", trimmed));
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

        private XElement CreateAdvancedMathAlias(string functionName, string arg)
        {
            var value = CreateVariableOrExpression(arg);

            XElement Num(string n) => CreateConstant(n);
            XElement E() => new XElement("EvaluateExpression", new XAttribute("style", "evaluate-expression"), new XElement("Constant", new XAttribute("text", "e")));
            XElement Bin(string op, XElement left, XElement right) => new XElement("BinaryOp",
                new XAttribute("op", NormalizeBinaryOp(op)),
                new XAttribute("style", BinaryStyle(NormalizeBinaryOp(op))),
                left,
                right);
            XElement Math(string fn, XElement inner) => new XElement("MathFunction",
                new XAttribute("function", fn),
                new XAttribute("style", "op-math"),
                inner);

            return functionName switch
            {
                "sinh" => Bin("/",
                    Bin("-",
                        Bin("^", E(), value),
                        Bin("^", E(), Bin("*", Num("-1"), value))),
                    Num("2")),
                "cosh" => Bin("/",
                    Bin("+",
                        Bin("^", E(), value),
                        Bin("^", E(), Bin("*", Num("-1"), value))),
                    Num("2")),
                "tanh" => Bin("/",
                    Bin("-",
                        Bin("^", E(), Bin("*", Num("2"), value)),
                        Num("1")),
                    Bin("+",
                        Bin("^", E(), Bin("*", Num("2"), value)),
                        Num("1"))),
                "asinh" => Math("ln",
                    Bin("+",
                        value,
                        Math("sqrt",
                            Bin("+",
                                Bin("^", value, Num("2")),
                                Num("1"))))),
                "acosh" => Math("ln",
                    Bin("+",
                        value,
                        Math("sqrt",
                            Bin("-",
                                Bin("^", value, Num("2")),
                                Num("1"))))),
                "atanh" => Bin("/",
                    Math("ln",
                        Bin("/",
                            Bin("+", Num("1"), value),
                            Bin("-", Num("1"), value))),
                    Num("2")),
                _ => value
            };
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
            if (op.Equals("friendly", StringComparison.OrdinalIgnoreCase))
            {
                string subOp = InferFriendlySubOp(args.Count > 0 ? args[0] : "");
                var friendlyEl = new XElement("StringOp",
                    new XAttribute("op", op),
                    new XAttribute("subOp", subOp),
                    new XAttribute("style", op));
                if (args.Count > 0)
                    friendlyEl.Add(CreateVariableOrExpression(args[0]));
                return friendlyEl;
            }

            var el = new XElement("StringOp", new XAttribute("op", op), new XAttribute("style", op));
            foreach (var arg in args)
                el.Add(CreateVariableOrExpression(arg));
            return el;
        }

        private string InferFriendlySubOp(string arg)
        {
            string value = arg.Trim();
            if (value.StartsWith("Vz.Length(", StringComparison.Ordinal) ||
                value.StartsWith("Vz.Distance(", StringComparison.Ordinal))
                return "distance";

            var expr = ConvertApiCallToXml(value);
            if (expr?.Name.LocalName == "VectorOp")
            {
                string op = expr.Attribute("op")?.Value ?? "";
                if (op.Equals("length", StringComparison.OrdinalIgnoreCase) ||
                    op.Equals("dist", StringComparison.OrdinalIgnoreCase) ||
                    op.Equals("distance", StringComparison.OrdinalIgnoreCase))
                    return "distance";
            }

            return "time";
        }

        private XElement CreateListExpression(string op, IReadOnlyList<string> args)
        {
            // Expression-context list ops use <ListOp> (matches Juno XML format)
            var el = new XElement("ListOp",
                new XAttribute("op", op),
                new XAttribute("style", $"list-{op}"));
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

        private bool TryConvertPlanetPropertyCall(string call, out XElement? element)
        {
            const string prefix = "Vz.Planet(";
            if (!call.StartsWith(prefix, StringComparison.Ordinal))
            {
                element = null;
                return false;
            }

            int depth = 1;
            int closeIndex = -1;
            for (int i = prefix.Length; i < call.Length; i++)
            {
                char ch = call[i];
                if (ch == '(') depth++;
                else if (ch == ')')
                {
                    depth--;
                    if (depth == 0)
                    {
                        closeIndex = i;
                        break;
                    }
                }
            }

            if (closeIndex < 0 || closeIndex + 4 > call.Length || call[closeIndex + 1] != '.' || !call.EndsWith("()", StringComparison.Ordinal))
            {
                element = null;
                return false;
            }

            string pArg = call.Substring(prefix.Length, closeIndex - prefix.Length).Trim();
            string prop = call.Substring(closeIndex + 2, call.Length - closeIndex - 4).Trim().ToLowerInvariant();
            if (!System.Text.RegularExpressions.Regex.IsMatch(prop, @"^[a-z_]\w*$"))
            {
                element = null;
                return false;
            }

            string opStr = prop switch
            {
                "mass"             => "mass",
                "radius"           => "radius",
                "atmospheredepth"  => "atmosphereHeight",
                "soi"              => "soiradius",
                "solarposition"    => "solarPosition",
                "childplanets"     => "childPlanets",
                "crafts"           => "crafts",
                "craftids"         => "craftids",
                "parent"           => "parent",
                "daylength"        => "day",
                "yearlength"       => "year",
                "velocity"         => "velocity",
                "apoapsis"         => "apoapsis",
                "periapsis"        => "periapsis",
                "period"           => "period",
                "inclination"      => "inclination",
                "eccentricity"     => "eccentricity",
                "meananomaly"      => "meananomaly",
                "semimajoraxis"    => "semimajoraxis",
                _                  => prop
            };

            var pEl = ConvertApiCallToXml(pArg) ?? CreateVariableReference(pArg);
            element = new XElement("Planet",
                new XAttribute("op", opStr),
                new XAttribute("style", "planet"),
                pEl);
            return true;
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
            // Start at expr.Length - 1 (not expr.Length - op.Length) so that trailing
            // bracket characters are processed into the depth counter before we attempt
            // to match any operator.  Skipping them caused depth to be wrong for
            // multi-char operators such as "&&" and "<=".
            for (int i = expr.Length - 1; i >= 0; i--)
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

                // Only attempt a match when enough characters remain for the operator.
                if (depth == 0 && i + op.Length <= expr.Length &&
                    expr.Substring(i).StartsWith(op, StringComparison.Ordinal))
                {
                    if (op == "+" || op == "-")
                    {
                        if (i == 0 || "+-*/%^<>=!&|(".Contains(expr[i - 1]))
                            continue;
                        if (i > 1
                            && (expr[i - 1] == 'E' || expr[i - 1] == 'e')
                            && (char.IsDigit(expr[i - 2]) || expr[i - 2] == '.'))
                            continue;
                    }
                    return i;
                }
            }
            return -1;
        }

        private static string? TryExtractChainedCallArgument(string source, string methodName)
        {
            string token = "." + methodName + "(";
            int start = source.IndexOf(token, StringComparison.Ordinal);
            if (start < 0) return null;

            int contentStart = start + token.Length;
            int depth = 1;
            bool inString = false;
            bool escaped = false;

            for (int i = contentStart; i < source.Length; i++)
            {
                char c = source[i];
                if (escaped)
                {
                    escaped = false;
                    continue;
                }

                if (c == '\\' && inString)
                {
                    escaped = true;
                    continue;
                }

                if (c == '"')
                {
                    inString = !inString;
                    continue;
                }

                if (inString)
                    continue;

                if (c == '(') depth++;
                else if (c == ')')
                {
                    depth--;
                    if (depth == 0)
                        return source.Substring(contentStart, i - contentStart).Trim();
                }
            }

            return null;
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

        private static string StripOuterQuotesPreserveWhitespace(string value)
        {
            return value.Length >= 2 && value[0] == '"' && value[^1] == '"'
                ? value.Substring(1, value.Length - 2)
                : value;
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
                || trimmed.Contains(" % ")
                || trimmed.Contains(" max ")
                || trimmed.Contains(" min ")
                || trimmed.Contains(" rand ");
        }

        private static string NormalizeBinaryOp(string op) => op.Trim().ToLowerInvariant() switch
        {
            "rand" => "rand",
            "min" => "min",
            "max" => "max",
            "atan2" => "atan2",
            _ => op.Trim()
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
            if (property.Equals("Craft.NameToID", StringComparison.OrdinalIgnoreCase)) return "craft-id";
            if (property.Equals("Part.NameToID", StringComparison.OrdinalIgnoreCase)) return "part-id";
            if (property.Equals("Part.PciToLocal", StringComparison.OrdinalIgnoreCase)) return "part-transform";
            if (property.Equals("Target.Position", StringComparison.OrdinalIgnoreCase)) return "prop-nav";
            if (property.Equals("Target.Velocity", StringComparison.OrdinalIgnoreCase)) return "prop-velocity";

            // Properties that return a name/string value use "prop-name"
            if (property.Equals("Orbit.Planet", StringComparison.OrdinalIgnoreCase)) return "prop-name";
            if (property.Equals("Target.Planet", StringComparison.OrdinalIgnoreCase)) return "prop-name";
            if (property.Equals("Target.Name", StringComparison.OrdinalIgnoreCase)) return "prop-name";
            if (property.Equals("Craft.Planet", StringComparison.OrdinalIgnoreCase)) return "prop-name";
            if (property.StartsWith("Name.", StringComparison.OrdinalIgnoreCase)) return "prop-name";

            if (property.StartsWith("Altitude.", StringComparison.OrdinalIgnoreCase)) return "prop-altitude";
            if (property.StartsWith("Orbit.", StringComparison.OrdinalIgnoreCase)) return "prop-orbit";
            if (property.StartsWith("Vel.", StringComparison.OrdinalIgnoreCase)) return "prop-velocity";
            if (property.StartsWith("Nav.", StringComparison.OrdinalIgnoreCase)) return "prop-nav";
            if (property.StartsWith("Target.", StringComparison.OrdinalIgnoreCase)) return "prop-name";
            if (property.StartsWith("Fuel.", StringComparison.OrdinalIgnoreCase)) return "prop-fuel";
            if (property.StartsWith("Performance.", StringComparison.OrdinalIgnoreCase)) return "prop-performance";
            if (property.StartsWith("Input.", StringComparison.OrdinalIgnoreCase)) return "prop-input";
            if (property.StartsWith("Time.", StringComparison.OrdinalIgnoreCase)) return "prop-time";
            if (property.StartsWith("Craft.", StringComparison.OrdinalIgnoreCase)) return "craft";
            if (property.StartsWith("Misc.", StringComparison.OrdinalIgnoreCase)) return "prop-misc";
            if (property.StartsWith("Atmosphere.", StringComparison.OrdinalIgnoreCase)) return "prop-atmosphere";
            return "craft";
        }

        private string ExtractParenthesisContent(string line)
        {
            // Find the opening paren that belongs to the outermost function call —
            // i.e. skip past the function-name prefix and find the first '(' that
            // opens a balanced region extending to a matching ')'.
            int openIdx = line.IndexOf('(');
            if (openIdx < 0) return "";
            int depth = 0;
            bool inStr = false;
            bool inVerbatim = false;
            int closeIdx = -1;
            for (int i = openIdx; i < line.Length; i++)
            {
                char c = line[i];
                if (inVerbatim)
                {
                    if (c == '"')
                    {
                        if (i + 1 < line.Length && line[i + 1] == '"') i++;
                        else inVerbatim = false;
                    }
                    continue;
                }
                if (!inStr && c == '@' && i + 1 < line.Length && line[i + 1] == '"') { inVerbatim = true; i++; continue; }
                if (c == '"') { inStr = !inStr; continue; }
                if (inStr) continue;
                if (c == '(') depth++;
                else if (c == ')') { depth--; if (depth == 0) { closeIdx = i; break; } }
            }
            if (closeIdx > openIdx)
                return line.Substring(openIdx + 1, closeIdx - openIdx - 1);
            return "";
        }

        private static bool NeedsRawConstantPreservation(XElement el)
        {
            var attrs = el.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value, StringComparer.Ordinal);
            if (attrs.ContainsKey("number"))
                return false;

            // bool constants are represented cleanly as true/false — no preservation needed
            if (attrs.ContainsKey("bool"))
                return false;

            if (attrs.TryGetValue("text", out string? textValue))
            {
                // text constants with style/canReplace are special (comment-text, etc.) — no preservation needed
                if (attrs.ContainsKey("style") || attrs.ContainsKey("canReplace"))
                    return false;

                // A text-attribute constant whose value parses as a number needs preservation
                // because it must round-trip as text=, not number=
                return double.TryParse(textValue, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out _);
            }

            return false;
        }

        private static string EncodePreservedElement(XElement el)
        {
            string xml = el.ToString(SaveOptions.DisableFormatting);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));
        }

        private static string BuildReadablePreservedCall(string functionName, XElement el)
        {
            string xml = el.ToString(SaveOptions.DisableFormatting).Replace("\"", "\"\"");
            return $"Vz.{functionName}(@\"{xml}\")";
        }

        private static bool ShouldPreserveInstructionMetadata(XElement el)
        {
            if (el.Name.LocalName == "CustomInstruction" && el.Attribute("pos") != null)
                return false;

            return el.Attribute("id") != null || el.Attribute("pos") != null || el.Attribute("mode") != null;
        }

        private XElement ApplyPendingInstructionMetadata(XElement element)
        {
            if (_pendingInstructionMetadata == null)
                return element;

            if (!InstructionMetadataMatches(_pendingInstructionMetadata.Name.LocalName, element.Name.LocalName))
                return element;

            IEnumerable<object> mergedNodes = element.Nodes();
            if (element.Name.LocalName is "SetVariable" or "ChangeVariable")
            {
                var preservedTarget = _pendingInstructionMetadata.Elements().FirstOrDefault(e => e.Name.LocalName == "Variable");
                if (preservedTarget != null)
                {
                    mergedNodes = new object[] { new XElement(preservedTarget) }
                        .Concat(element.Nodes().Skip(1))
                        .ToArray();
                }
            }
            else if (element.Name.LocalName == "For")
            {
                var preservedParts = _pendingInstructionMetadata.Elements().Where(e => e.Name.LocalName != "Instructions")
                    .Select(e => new XElement(e))
                    .Cast<object>()
                    .ToList();
                var body = element.Element("Instructions");
                if (preservedParts.Count > 0)
                {
                    if (body != null) preservedParts.Add(body);
                    mergedNodes = preservedParts;
                }
            }

            var preserved = new XElement(_pendingInstructionMetadata.Name,
                _pendingInstructionMetadata.Attributes(),
                mergedNodes);
            _pendingInstructionMetadata = null;
            return preserved;
        }

        private static bool InstructionMetadataMatches(string preservedName, string actualName)
        {
            if (string.Equals(preservedName, actualName, StringComparison.Ordinal))
                return true;

            return (preservedName, actualName) switch
            {
                ("LogMessage", "Log") => true,
                ("Log", "LogMessage") => true,
                ("DisplayMessage", "Display") => true,
                ("Display", "DisplayMessage") => true,
                _ => false
            };
        }

        private bool TryDecodePreservedCall(string call, string functionName, string expectedName, out XElement? element)
        {
            string prefix = functionName + "(";
            if (!call.StartsWith(prefix, StringComparison.Ordinal) || !call.EndsWith(")", StringComparison.Ordinal))
            {
                element = null;
                return false;
            }

            if (!HasOuterParentheses(call.Substring(functionName.Length)))
            {
                element = null;
                return false;
            }

            var args = SplitArgs(ExtractParenthesisContent(call));
            if (args.Count != 1)
            {
                element = null;
                return false;
            }

            element = DecodePreservedElement(args[0], expectedName);
            return true;
        }

        private static XElement DecodePreservedElement(string encodedArg, string expectedName)
        {
            string decodedArg = DecodeStringLiteral(encodedArg).Trim();
            string xml = decodedArg.StartsWith("<", StringComparison.Ordinal)
                ? decodedArg
                : Encoding.UTF8.GetString(Convert.FromBase64String(decodedArg));
            var element = XElement.Parse(xml);
            return element.Name.LocalName == expectedName ? element : new XElement(expectedName);
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

            return Unescape(trimQuotes(trimmed));
        }
    }
}
