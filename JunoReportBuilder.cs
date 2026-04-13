using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace VizzyCode
{
    internal static class JunoReportBuilder
    {
        public static string BuildMarkdownReport(string json, string kind)
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Locate craft and telemetry sub-objects (snapshot has them nested;
            // bare /telemetry or /telemetry/full keep everything at the root).
            var craftEl  = Obj(root, "craft");
            var teleEl   = Obj(root, "telemetry") ?? (HasTelemetryFields(root) ? root : (JsonElement?)null);
            var stagesEl = Obj(root, "stages");
            var parts    = craftEl.HasValue ? Arr(craftEl, "parts") : new List<JsonElement>();

            string craftName = Str(root, "craftName")
                            ?? Str(craftEl, "name")
                            ?? Str(teleEl, "craftName")
                            ?? "Unknown craft";
            string quality = Str(teleEl, "quality") ?? Str(root, "quality") ?? "";
            bool fullFlight = quality == "flight_active_full";
            bool designerOnly = quality == "designer_static";

            string titleKind = kind.Equals("telemetry", StringComparison.OrdinalIgnoreCase)
                ? "Telemetry"
                : "Craft Snapshot";

            var sb = new StringBuilder();
            sb.AppendLine($"# Juno {titleKind} Report");
            sb.AppendLine();
            sb.AppendLine("Human/AI-readable VizzyCode bridge data. Use the raw JSON when exact machine-readable values are needed.");
            sb.AppendLine();

            // ── Summary ────────────────────────────────────────────────────────
            Section(sb, "Summary");
            AddBullet(sb, "Craft", craftName);
            AddBullet(sb, "Scene", Str(root, "scene") ?? Str(teleEl, "scene"));
            AddBullet(sb, "Mod version", Str(root, "modVersion"));
            AddBullet(sb, "Telemetry quality", quality);
            AddBullet(sb, "Active full flight data", BoolText(teleEl, "activeFullFlightData"));
            AddBullet(sb, "Has command pod", BoolText(teleEl, "hasCommandPod"));
            AddBullet(sb, "Physics enabled", BoolText(teleEl, "physicsEnabled"));
            if (craftEl.HasValue)
            {
                AddBullet(sb, "Part count", NumText(craftEl, "partCount"));
                AddBullet(sb, "Body count", NumText(craftEl, "bodyCount"));
            }
            AddBullet(sb, "Current stage", StageSummary(stagesEl));

            // Data quality warning
            if (!fullFlight)
            {
                sb.AppendLine();
                if (designerOnly)
                    sb.AppendLine("> **Designer scene** — craft structure is available but flight telemetry is not. Altitude, velocity, fuel, and orbit data will be zero or unavailable.");
                else if (quality == "no_craft")
                    sb.AppendLine("> **No craft loaded** — no telemetry or craft data is available. Launch a craft in the flight scene.");
                else if (quality == "flight_no_flightdata")
                    sb.AppendLine("> **Flight scene — no active FlightData** — the craft may be tracked/unloaded. Some telemetry is available via CraftNode.");
                else
                    sb.AppendLine("> **Limited telemetry** — flight data quality: `" + quality + "`.");
            }
            sb.AppendLine();

            // ── AI Authoring Notes ─────────────────────────────────────────────
            Section(sb, "AI Authoring Notes");
            sb.AppendLine("- Use exact part names from this report when calling `Vz.PartNameToID(...)` or targeting part properties.");
            sb.AppendLine("- Prefer activation group and stage data from this report over guessing from craft appearance.");
            sb.AppendLine("- Treat `designer_static`, `flight_no_flightdata`, and `trackedFallback=true` as limited-quality data.");
            sb.AppendLine("- If the Vizzy script depends on orbit, velocity, fuel, or target state, capture this report again in-flight.");
            sb.AppendLine("- If a needed part is missing here, the script should not assume it exists.");
            if (!craftEl.HasValue)
                sb.AppendLine("- This is a telemetry-only report. Use **Save Full Craft Report** for the complete part listing.");
            sb.AppendLine();

            // ── Telemetry sections ─────────────────────────────────────────────
            AppendTelemetry(sb, teleEl, fullFlight);
            AppendExtendedTelemetry(sb, teleEl, fullFlight);
            AppendOrbit(sb, Obj(teleEl, "orbit"), fullFlight);
            AppendPerformance(sb, Obj(teleEl, "performance"), fullFlight);
            AppendTarget(sb, Obj(teleEl, "navTarget"));

            // ── Craft sections ─────────────────────────────────────────────────
            AppendStages(sb, stagesEl);
            AppendParts(sb, parts, craftEl);
            AppendNotes(sb, Arr(root, "notes"));

            Section(sb, "Raw Data");
            sb.AppendLine("Save the original JSON next to this report when exact vectors or unlisted fields are required.");
            return sb.ToString();
        }

        // ── Telemetry ──────────────────────────────────────────────────────────

        private static void AppendTelemetry(StringBuilder sb, JsonElement? telemetry, bool fullFlight)
        {
            if (telemetry == null || telemetry.Value.ValueKind != JsonValueKind.Object) return;

            Section(sb, "Live Telemetry");

            if (!fullFlight)
            {
                sb.AppendLine("*Flight telemetry is not available in the current scene. Values below reflect static or fallback data.*");
                sb.AppendLine();
                // Still show what we have — but only non-zero values
                TableNonZero(sb, telemetry,
                    ("Altitude ASL", "altitudeASL", "m"),
                    ("Altitude AGL", "altitudeAGL", "m"),
                    ("Terrain altitude", "altitudeTerrain", "m"),
                    ("Velocity", "velocityMagnitude", "m/s"),
                    ("Grounded", "grounded", null),
                    ("In water", "inWater", null)
                );
                return;
            }

            Table(sb,
                ("Altitude ASL", Unit(telemetry, "altitudeASL", "m")),
                ("Altitude AGL", Unit(telemetry, "altitudeAGL", "m")),
                ("Terrain altitude", Unit(telemetry, "altitudeTerrain", "m")),
                ("Velocity", Unit(telemetry, "velocityMagnitude", "m/s")),
                ("Surface velocity", Unit(telemetry, "surfaceVelocityMagnitude", "m/s")),
                ("Vertical speed", Unit(telemetry, "verticalSurfaceVelocity", "m/s")),
                ("Lateral speed", Unit(telemetry, "lateralSurfaceVelocity", "m/s")),
                ("Acceleration", Unit(telemetry, "accelerationMagnitude", "m/s²")),
                ("Mach", NumText(telemetry, "machNumber")),
                ("Angle of attack", Unit(telemetry, "angleOfAttack", "deg")),
                ("Pitch", Unit(telemetry, "pitch", "deg")),
                ("Heading", Unit(telemetry, "heading", "deg")),
                ("Bank", Unit(telemetry, "bankAngle", "deg")),
                ("Side-slip", Unit(telemetry, "sideSlip", "deg")),
                ("Grounded", BoolText(telemetry, "grounded")),
                ("In water", BoolText(telemetry, "inWater")),
                ("Mass", Unit(telemetry, "currentMassKg", "kg")),
                ("Fuel mass", Unit(telemetry, "fuelMass", "kg")),
                ("Stage fuel %", NumText(telemetry, "remainingFuelInStage")),
                ("Battery", NumText(telemetry, "remainingBattery")),
                ("Monopropellant", NumText(telemetry, "remainingMonopropellant")),
                ("Current thrust", Unit(telemetry, "currentEngineThrustN", "N")),
                ("Max active thrust", Unit(telemetry, "maxActiveEngineThrustN", "N")),
                ("Gravity", Unit(telemetry, "gravityMagnitude", "m/s²")),
                ("Solar radiation", Unit(telemetry, "solarRadiationIntensity", "W/m²")),
                ("Planet occlusion", NumText(telemetry, "parentPlanetOcclusion"))
            );
            sb.AppendLine();
        }

        private static void AppendExtendedTelemetry(StringBuilder sb, JsonElement? t, bool fullFlight)
        {
            if (t == null || t.Value.ValueKind != JsonValueKind.Object) return;

            // Only render if any extended field is present and non-zero
            bool hasLat = HasNonZero(t, "latDeg");
            bool hasAtm = Obj(t, "atmosphereSample").HasValue;
            bool hasAng = HasNonZero(t, "angularVelocityMagnitude");
            bool hasDrag = HasNonZero(t, "dragAccelerationMagnitude");
            bool hasUnivTime = HasNonZero(t, "universalTime");
            if (!hasLat && !hasAtm && !hasAng && !hasDrag && !hasUnivTime) return;

            Section(sb, "Extended Telemetry");
            var rows = new List<(string, string?)>();

            if (hasUnivTime) rows.Add(("Universal time", Unit(t, "universalTime", "s")));
            if (hasLat)
            {
                rows.Add(("Latitude", Unit(t, "latDeg", "°")));
                rows.Add(("Longitude", Unit(t, "lonDeg", "°")));
            }
            if (hasAng)
            {
                rows.Add(("Angular velocity", Unit(t, "angularVelocityMagnitude", "rad/s")));
                rows.Add(("Drag decel", Unit(t, "dragAccelerationMagnitude", "m/s²")));
            }

            if (hasAtm)
            {
                var atm = Obj(t, "atmosphereSample");
                rows.Add(("Atm density", Unit(atm, "density", "kg/m³")));
                rows.Add(("Atm pressure", Unit(atm, "pressure", "Pa")));
                rows.Add(("Atm temperature", Unit(atm, "temperature", "K")));
                rows.Add(("Speed of sound", Unit(atm, "speedOfSound", "m/s")));
            }

            if (rows.Count > 0) Table(sb, rows.ToArray());
            sb.AppendLine();
        }

        private static void AppendOrbit(StringBuilder sb, JsonElement? orbit, bool fullFlight)
        {
            if (orbit == null || orbit.Value.ValueKind != JsonValueKind.Object) return;
            if (!fullFlight) return; // orbit data is only valid in active flight

            Section(sb, "Orbit");
            Table(sb,
                ("Parent body", Str(orbit, "parent")),
                ("Apoapsis altitude", Unit(orbit, "apoapsisAltitude", "m")),
                ("Periapsis altitude", Unit(orbit, "periapsisAltitude", "m")),
                ("Time to apoapsis", Unit(orbit, "apoapsisTime", "s")),
                ("Time to periapsis", Unit(orbit, "periapsisTime", "s")),
                ("Eccentricity", NumText(orbit, "eccentricity")),
                ("Inclination", Unit(orbit, "inclination", "deg")),
                ("Period", Unit(orbit, "period", "s")),
                ("Semi-major axis", Unit(orbit, "semiMajorAxis", "m")),
                ("True anomaly", Unit(orbit, "trueAnomaly", "deg")),
                ("Burn node Δv", Vec3Magnitude(Obj(orbit, "burnNodeDeltaV"), "m/s")),
                ("Has burn node", BoolText(orbit, "hasBurnNodePoint"))
            );
            sb.AppendLine();
        }

        private static void AppendPerformance(StringBuilder sb, JsonElement? perf, bool fullFlight)
        {
            if (perf == null || perf.Value.ValueKind != JsonValueKind.Object) return;
            if (!fullFlight) return;

            Section(sb, "Performance");
            Table(sb,
                ("Current ISP", Unit(perf, "currentIsp", "s")),
                ("Stage Δv", Unit(perf, "deltaVStage", "m/s")),
                ("All-stage fuel %", NumText(perf, "fuelAllStagesPercentage")),
                ("Remaining burn time", Unit(perf, "remainingBurnTime", "s")),
                ("TWR", NumText(perf, "thrustToWeightRatio"))
            );
            sb.AppendLine();
        }

        private static void AppendTarget(StringBuilder sb, JsonElement? target)
        {
            if (target == null || target.Value.ValueKind != JsonValueKind.Object) return;
            if (Bool(target, "exists") != true) return;

            Section(sb, "Nav Target");
            Table(sb,
                ("Type", Str(target, "type")),
                ("Name", Str(target, "name")),
                ("Body", Str(target, "bodyName"))
            );
            sb.AppendLine();
        }

        // ── Stages ────────────────────────────────────────────────────────────

        private static void AppendStages(StringBuilder sb, JsonElement? stages)
        {
            if (stages == null || stages.Value.ValueKind != JsonValueKind.Object) return;

            Section(sb, "Stages and Activation Groups");
            AddBullet(sb, "Current stage", NumText(stages, "currentStage"));
            AddBullet(sb, "Stage count", NumText(stages, "numStages"));
            sb.AppendLine();

            var names  = Arr(stages, "activationGroupNames");
            var states = Arr(stages, "activationGroupStates");
            if (names.Count > 0)
            {
                sb.AppendLine("| Group | Name | State |");
                sb.AppendLine("| --- | --- | --- |");
                for (int i = 0; i < names.Count; i++)
                {
                    string name  = names[i].ValueKind == JsonValueKind.String ? names[i].GetString() ?? "" : "";
                    string state = i < states.Count && states[i].ValueKind is JsonValueKind.True or JsonValueKind.False
                        ? (states[i].GetBoolean() ? "**ON**" : "off")
                        : "—";
                    sb.AppendLine($"| {i} | {Md(name)} | {state} |");
                }
                sb.AppendLine();
            }

            var stageList = Arr(stages, "stages");
            if (stageList.Count > 0)
            {
                sb.AppendLine("| Stage | Active | Parts | Part names |");
                sb.AppendLine("| --- | --- | --- | --- |");
                foreach (var stage in stageList)
                {
                    var partNames = Arr(stage, "partNames")
                        .Select(e => e.ValueKind == JsonValueKind.String ? e.GetString() ?? "" : "")
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Take(12);
                    bool isCurrent = Bool(stage, "isCurrent") == true;
                    string stageNum = NumText(stage, "stage");
                    sb.AppendLine($"| {(isCurrent ? "**" + stageNum + "**" : stageNum)} | {(isCurrent ? "✓" : "")} | {NumText(stage, "partCount")} | {Md(string.Join(", ", partNames))} |");
                }
                sb.AppendLine();
            }
        }

        // ── Parts ─────────────────────────────────────────────────────────────

        private static void AppendParts(StringBuilder sb, List<JsonElement> parts, JsonElement? craftEl)
        {
            if (!craftEl.HasValue)
            {
                Section(sb, "Craft Parts");
                sb.AppendLine("*This is a telemetry-only report. Use **Save Full Craft Report** (or /snapshot) to include the part listing.*");
                sb.AppendLine();
                return;
            }

            if (parts.Count == 0)
            {
                Section(sb, "Craft Parts");
                sb.AppendLine("*No parts were reported. The craft may be empty or the data was captured outside a valid scene.*");
                sb.AppendLine();
                return;
            }

            Section(sb, "Vizzy-Capable Parts");
            var vizzy = parts.Where(p => Bool(p, "hasVizzy") == true).ToList();
            if (vizzy.Count == 0)
            {
                sb.AppendLine("No Vizzy-capable parts on this craft.");
            }
            else
            {
                sb.AppendLine("| ID | Name | Type | Stage | AG |");
                sb.AppendLine("| --- | --- | --- | --- | --- |");
                foreach (var p in vizzy)
                    sb.AppendLine($"| {NumText(p, "id")} | {Md(Str(p, "name"))} | {Md(Str(p, "partType"))} | {NumText(p, "activationStage")} | {NumText(p, "activationGroup")} |");
            }
            sb.AppendLine();

            Section(sb, "Parts by Role");
            var byRole = parts
                .Select(p => new { Part = p, Role = ClassifyPart(p) })
                .GroupBy(x => x.Role)
                .OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase);
            foreach (var group in byRole)
            {
                var names = group.Select(x =>
                {
                    string id    = NumText(x.Part, "id");
                    string pname = Str(x.Part, "name") ?? "(unnamed)";
                    string stage = NumText(x.Part, "activationStage");
                    return $"{pname} #{id} s{stage}";
                }).Take(20);
                sb.AppendLine($"- **{Md(group.Key)}**: {Md(string.Join("; ", names))}");
            }
            sb.AppendLine();

            Section(sb, "All Parts");
            sb.AppendLine("| ID | Name | Type | Role | Stage | AG | Mass (kg) | Mods |");
            sb.AppendLine("| --- | --- | --- | --- | --- | --- | --- | --- |");
            foreach (var p in parts.OrderBy(p => Int(p, "id") ?? 0))
            {
                sb.AppendLine($"| {NumText(p, "id")} | {Md(Str(p, "name"))} | {Md(Str(p, "partType"))} | {Md(ClassifyPart(p))} | {NumText(p, "activationStage")} | {NumText(p, "activationGroup")} | {NumText(p, "mass")} | {Arr(p, "modifiers").Count} |");
            }
            sb.AppendLine();
        }

        private static void AppendNotes(StringBuilder sb, List<JsonElement> notes)
        {
            if (notes.Count == 0) return;
            Section(sb, "Bridge Notes");
            foreach (var note in notes)
                if (note.ValueKind == JsonValueKind.String)
                    sb.AppendLine($"- {note.GetString()}");
            sb.AppendLine();
        }

        // ── Part role classifier ───────────────────────────────────────────────

        private static string ClassifyPart(JsonElement p)
        {
            string hay = ((Str(p, "name") ?? "") + " " + (Str(p, "partType") ?? "") + " " +
                          string.Join(" ", Arr(p, "modifiers")
                              .Select(m => $"{Str(m, "typeName")} {Str(m, "name")} {Str(m, "typeId")}")))
                         .ToLowerInvariant();

            if (hay.Contains("flightprogram") || Bool(p, "hasVizzy") == true) return "Vizzy / flight computer";
            if (hay.Contains("commandpod") || hay.Contains("command pod"))    return "Command pod";
            if (hay.Contains("rocketengine") || hay.Contains("engine"))       return "Engine";
            if (hay.Contains("fueltank") || hay.Contains("fuel tank") || hay.Contains("propellant")) return "Fuel";
            if (hay.Contains("parachute"))                                     return "Parachute";
            if (hay.Contains("solar"))                                         return "Solar / power";
            if (hay.Contains("battery"))                                       return "Battery / power";
            if (hay.Contains("gyro") || hay.Contains("reaction wheel"))        return "Attitude control";
            if (hay.Contains("rcs") || hay.Contains("nozzle"))                 return "RCS";
            if (hay.Contains("landingleg") || hay.Contains("landing leg") || hay.Contains("leg")) return "Landing gear";
            if (hay.Contains("wheel"))                                         return "Wheel";
            if (hay.Contains("docking"))                                       return "Docking";
            if (hay.Contains("separator") || hay.Contains("decoupler"))        return "Separation";
            if (hay.Contains("wing") || hay.Contains("fin") || hay.Contains("controlsurface")) return "Aero / control surface";
            return "Other";
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        private static bool HasTelemetryFields(JsonElement e) =>
            e.ValueKind == JsonValueKind.Object &&
            (e.TryGetProperty("altitudeASL", out _) ||
             e.TryGetProperty("velocityMagnitude", out _) ||
             e.TryGetProperty("quality", out _));

        private static bool HasNonZero(JsonElement? e, string key)
        {
            if (e == null || e.Value.ValueKind != JsonValueKind.Object) return false;
            if (!e.Value.TryGetProperty(key, out var v)) return false;
            if (v.ValueKind == JsonValueKind.Number && v.TryGetDouble(out double d)) return d != 0.0;
            return false;
        }

        private static void Section(StringBuilder sb, string title)
        {
            sb.AppendLine($"## {title}");
            sb.AppendLine();
        }

        private static void AddBullet(StringBuilder sb, string label, string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "null") return;
            sb.AppendLine($"- **{label}:** {value}");
        }

        private static void Table(StringBuilder sb, params (string Label, string? Value)[] rows)
        {
            var visible = rows.Where(r => !string.IsNullOrWhiteSpace(r.Value) && r.Value != "null").ToList();
            if (visible.Count == 0) return;
            sb.AppendLine("| Field | Value |");
            sb.AppendLine("| --- | --- |");
            foreach (var (label, value) in visible)
                sb.AppendLine($"| {Md(label)} | {Md(value)} |");
        }

        private static void Table(StringBuilder sb, List<(string Label, string? Value)> rows)
            => Table(sb, rows.ToArray());

        /// <summary>Only renders non-zero numeric values and non-false booleans.</summary>
        private static void TableNonZero(StringBuilder sb, JsonElement? e, params (string Label, string Key, string? Unit)[] fields)
        {
            var rows = new List<(string, string?)>();
            foreach (var (label, key, unit) in fields)
            {
                if (!HasNonZero(e, key)) continue;
                string v = unit != null ? (Unit(e, key, unit) ?? "") : (BoolText(e, key) ?? "");
                if (!string.IsNullOrWhiteSpace(v)) rows.Add((label, v));
            }
            if (rows.Count > 0) Table(sb, rows);
        }

        private static string StageSummary(JsonElement? stages)
        {
            if (stages == null || stages.Value.ValueKind != JsonValueKind.Object) return "";
            string current = NumText(stages, "currentStage");
            string total   = NumText(stages, "numStages");
            return string.IsNullOrEmpty(total) ? current : $"{current} / {total}";
        }

        private static string? Unit(JsonElement? e, string key, string unit)
        {
            string? n = NumText(e, key);
            return string.IsNullOrEmpty(n) ? null : $"{n} {unit}";
        }

        /// <summary>Computes the magnitude of a Vec3 object {x,y,z} and formats it with a unit.</summary>
        private static string? Vec3Magnitude(JsonElement? vec, string unit)
        {
            if (vec == null || vec.Value.ValueKind != JsonValueKind.Object) return null;
            if (!vec.Value.TryGetProperty("x", out var xv)) return null;
            if (!vec.Value.TryGetProperty("y", out var yv)) return null;
            if (!vec.Value.TryGetProperty("z", out var zv)) return null;
            if (!xv.TryGetDouble(out double x) || !yv.TryGetDouble(out double y) || !zv.TryGetDouble(out double z))
                return null;
            double mag = Math.Sqrt(x * x + y * y + z * z);
            if (mag == 0.0) return null;
            return $"{mag.ToString("0.####", CultureInfo.InvariantCulture)} {unit}";
        }

        private static string? NumText(JsonElement? e, string key)
        {
            if (e == null || e.Value.ValueKind != JsonValueKind.Object) return null;
            if (!e.Value.TryGetProperty(key, out var v)) return null;
            if (v.ValueKind == JsonValueKind.Number)
            {
                if (v.TryGetInt64(out long i)) return i.ToString(CultureInfo.InvariantCulture);
                if (v.TryGetDouble(out double d))
                {
                    if (double.IsNaN(d) || double.IsInfinity(d)) return null;
                    return d.ToString("0.####", CultureInfo.InvariantCulture);
                }
            }
            if (v.ValueKind == JsonValueKind.String) return v.GetString();
            return null;
        }

        private static string? NumText(JsonElement e, string key)
            => NumText((JsonElement?)e, key);

        private static string? BoolText(JsonElement? e, string key)
        {
            var v = Bool(e, key);
            return v == null ? null : (v.Value ? "true" : "false");
        }

        private static bool? Bool(JsonElement? e, string key)
        {
            if (e == null || e.Value.ValueKind != JsonValueKind.Object) return null;
            if (!e.Value.TryGetProperty(key, out var v)) return null;
            return v.ValueKind switch { JsonValueKind.True => true, JsonValueKind.False => false, _ => null };
        }

        private static bool? Bool(JsonElement e, string key)
            => Bool((JsonElement?)e, key);

        private static int? Int(JsonElement e, string key)
        {
            if (e.ValueKind != JsonValueKind.Object || !e.TryGetProperty(key, out var v)) return null;
            return v.ValueKind == JsonValueKind.Number && v.TryGetInt32(out int i) ? i : null;
        }

        private static string? Str(JsonElement? e, string key)
        {
            if (e == null || e.Value.ValueKind != JsonValueKind.Object) return null;
            if (!e.Value.TryGetProperty(key, out var v)) return null;
            return v.ValueKind == JsonValueKind.String ? v.GetString() : null;
        }

        private static JsonElement? Obj(JsonElement? e, string key)
        {
            if (e == null || e.Value.ValueKind != JsonValueKind.Object) return null;
            if (!e.Value.TryGetProperty(key, out var v)) return null;
            return v.ValueKind == JsonValueKind.Object ? v : null;
        }

        private static JsonElement? Obj(JsonElement e, string key)
            => Obj((JsonElement?)e, key);

        private static List<JsonElement> Arr(JsonElement? e, string key)
        {
            if (e == null || e.Value.ValueKind != JsonValueKind.Object) return new List<JsonElement>();
            if (!e.Value.TryGetProperty(key, out var v) || v.ValueKind != JsonValueKind.Array) return new List<JsonElement>();
            return v.EnumerateArray().ToList();
        }

        private static List<JsonElement> Arr(JsonElement e, string key)
            => Arr((JsonElement?)e, key);

        private static string Md(string? value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return value.Replace("|", "\\|").Replace("\r", " ").Replace("\n", " ");
        }
    }
}
