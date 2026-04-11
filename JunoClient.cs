using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VizzyCode
{
    /// <summary>
    /// HTTP client that talks to the VizzyCode mod running inside Juno: New Origins.
    /// The mod listens on http://127.0.0.1:7842/.
    ///
    /// All methods return null / false on network error so callers can display
    /// a friendly "not connected" message.
    /// </summary>
    internal static class JunoClient
    {
        public const string BaseUrl  = "http://127.0.0.1:7842";
        public const int    TimeoutMs = 3000;

        private static readonly HttpClient _http = new()
        {
            Timeout = TimeSpan.FromMilliseconds(TimeoutMs)
        };

        // ── DTOs ──────────────────────────────────────────────────────────────

        public class StatusInfo
        {
            public bool   Ok          { get; set; }
            public string ModVersion  { get; set; }
            public string Scene       { get; set; }   // "designer" | "flight" | "menu"
            public string CraftName   { get; set; }
        }

        public class PartInfo
        {
            public int    Id        { get; set; }
            public string Name      { get; set; }
            public bool   HasVizzy  { get; set; }
        }

        public class CraftInfo
        {
            public string     Name  { get; set; }
            public PartInfo[] Parts { get; set; }
        }

        public class VizzyInfo
        {
            public bool   Ok       { get; set; }
            public int    PartId   { get; set; }
            public string PartName { get; set; }
            public string Xml      { get; set; }
            public string Error    { get; set; }
        }

        public class StagesInfo
        {
            public int      CurrentStage          { get; set; }
            public int      NumStages             { get; set; }
            public string[] ActivationGroupNames  { get; set; }
            public bool[]   ActivationGroupStates { get; set; }
        }

        // ── API calls ─────────────────────────────────────────────────────────

        /// <summary>Ping the mod. Returns null if not running.</summary>
        public static async Task<StatusInfo> GetStatusAsync()
        {
            try
            {
                string json = await GetAsync("/status");
                return ParseStatus(json);
            }
            catch { return null; }
        }

        /// <summary>Get the current craft's part list.</summary>
        public static async Task<CraftInfo> GetCraftAsync()
        {
            try
            {
                string json = await GetAsync("/craft");
                return ParseCraft(json);
            }
            catch { return null; }
        }

        /// <summary>Get the Vizzy XML for a specific part.</summary>
        public static async Task<VizzyInfo> GetVizzyAsync(int partId)
        {
            try
            {
                string json = await GetAsync($"/vizzy/{partId}");
                return ParseVizzy(json);
            }
            catch { return null; }
        }

        /// <summary>Push a Vizzy XML string to the game for a specific part.</summary>
        public static async Task<VizzyInfo> PutVizzyAsync(int partId, string xml)
        {
            try
            {
                // Escape XML for embedding in JSON
                string escaped = xml.Replace("\\", "\\\\").Replace("\"", "\\\"")
                                    .Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
                string body = $"{{\"xml\":\"{escaped}\"}}";
                string json = await PutAsync($"/vizzy/{partId}", body);
                return ParseVizzy(json);
            }
            catch (Exception ex) { return new VizzyInfo { Ok = false, Error = ex.Message }; }
        }

        /// <summary>Get staging information for the active craft.</summary>
        public static async Task<StagesInfo> GetStagesAsync()
        {
            try
            {
                string json = await GetAsync("/stages");
                return ParseStages(json);
            }
            catch { return null; }
        }

        /// <summary>Trigger the next stage activation (flight scene only).</summary>
        public static async Task<StagesInfo> ActivateStageAsync()
        {
            try
            {
                string json = await PostAsync("/stages/activate", "{}");
                return ParseStages(json);
            }
            catch { return null; }
        }

        // ── HTTP helpers ──────────────────────────────────────────────────────

        private static async Task<string> GetAsync(string path)
        {
            var r = await _http.GetAsync(BaseUrl + path);
            return await r.Content.ReadAsStringAsync();
        }

        private static async Task<string> PutAsync(string path, string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var r = await _http.PutAsync(BaseUrl + path, content);
            return await r.Content.ReadAsStringAsync();
        }

        private static async Task<string> PostAsync(string path, string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var r = await _http.PostAsync(BaseUrl + path, content);
            return await r.Content.ReadAsStringAsync();
        }

        // ── Minimal JSON parsers ──────────────────────────────────────────────
        // We use simple field extraction to avoid taking a JSON library dependency.

        private static StatusInfo ParseStatus(string j) => j == null ? null : new StatusInfo
        {
            Ok         = JsonBool(j, "ok") ?? false,
            ModVersion = JsonStr(j, "modVersion"),
            Scene      = JsonStr(j, "scene"),
            CraftName  = JsonStr(j, "craftName")
        };

        private static CraftInfo ParseCraft(string j)
        {
            if (j == null || j.Contains("\"error\"")) return null;
            var parts = ParsePartArray(j);
            return new CraftInfo { Name = JsonStr(j, "name"), Parts = parts };
        }

        private static PartInfo[] ParsePartArray(string j)
        {
            // Very simple: split on "}," within the "parts" array
            int arrStart = j.IndexOf("\"parts\"", StringComparison.Ordinal);
            if (arrStart < 0) return Array.Empty<PartInfo>();
            int bracket = j.IndexOf('[', arrStart);
            int bracketEnd = j.LastIndexOf(']');
            if (bracket < 0 || bracketEnd < 0) return Array.Empty<PartInfo>();

            string arr = j.Substring(bracket + 1, bracketEnd - bracket - 1);
            var list = new System.Collections.Generic.List<PartInfo>();

            // Split on object boundaries
            int depth = 0, start = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == '{') { if (depth++ == 0) start = i; }
                else if (arr[i] == '}')
                {
                    if (--depth == 0)
                    {
                        string obj = arr.Substring(start, i - start + 1);
                        list.Add(new PartInfo
                        {
                            Id       = JsonInt(obj, "id") ?? 0,
                            Name     = JsonStr(obj, "name") ?? "",
                            HasVizzy = JsonBool(obj, "hasVizzy") ?? false
                        });
                    }
                }
            }
            return list.ToArray();
        }

        private static VizzyInfo ParseVizzy(string j) => j == null ? null : new VizzyInfo
        {
            Ok       = JsonBool(j, "ok") ?? false,
            PartId   = JsonInt(j, "partId") ?? 0,
            PartName = JsonStr(j, "partName"),
            Xml      = JsonStr(j, "xml"),
            Error    = JsonStr(j, "error")
        };

        private static StagesInfo ParseStages(string j) => j == null ? null : new StagesInfo
        {
            CurrentStage = JsonInt(j, "currentStage") ?? 0,
            NumStages    = JsonInt(j, "numStages")    ?? 0
        };

        // ── Field extractors ──────────────────────────────────────────────────

        private static string JsonStr(string j, string key)
        {
            if (j == null) return null;
            int ki = j.IndexOf($"\"{key}\"", StringComparison.Ordinal);
            if (ki < 0) return null;
            int colon = j.IndexOf(':', ki);
            if (colon < 0) return null;
            // Skip whitespace
            int pos = colon + 1;
            while (pos < j.Length && j[pos] == ' ') pos++;
            if (pos >= j.Length) return null;
            if (j[pos] == 'n') return null; // null
            if (j[pos] != '"') return null;
            pos++;
            var sb = new StringBuilder();
            while (pos < j.Length)
            {
                char c = j[pos];
                if (c == '\\' && pos + 1 < j.Length)
                {
                    char n = j[pos + 1];
                    switch (n)
                    {
                        case '"':  sb.Append('"');  pos += 2; continue;
                        case '\\': sb.Append('\\'); pos += 2; continue;
                        case 'n':  sb.Append('\n'); pos += 2; continue;
                        case 'r':  sb.Append('\r'); pos += 2; continue;
                        case 't':  sb.Append('\t'); pos += 2; continue;
                        default:   sb.Append(n);   pos += 2; continue;
                    }
                }
                if (c == '"') break;
                sb.Append(c); pos++;
            }
            return sb.ToString();
        }

        private static bool? JsonBool(string j, string key)
        {
            if (j == null) return null;
            int ki = j.IndexOf($"\"{key}\"", StringComparison.Ordinal);
            if (ki < 0) return null;
            int colon = j.IndexOf(':', ki);
            if (colon < 0) return null;
            string rest = j.Substring(colon + 1).TrimStart();
            if (rest.StartsWith("true",  StringComparison.OrdinalIgnoreCase)) return true;
            if (rest.StartsWith("false", StringComparison.OrdinalIgnoreCase)) return false;
            return null;
        }

        private static int? JsonInt(string j, string key)
        {
            if (j == null) return null;
            int ki = j.IndexOf($"\"{key}\"", StringComparison.Ordinal);
            if (ki < 0) return null;
            int colon = j.IndexOf(':', ki);
            if (colon < 0) return null;
            int pos = colon + 1;
            while (pos < j.Length && j[pos] == ' ') pos++;
            var sb = new StringBuilder();
            while (pos < j.Length && (char.IsDigit(j[pos]) || j[pos] == '-')) { sb.Append(j[pos]); pos++; }
            return int.TryParse(sb.ToString(), out int v) ? v : (int?)null;
        }
    }
}
