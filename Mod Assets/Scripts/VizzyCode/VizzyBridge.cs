using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Assets.Scripts.Craft.Parts.Modifiers;
using ModApi.Common;
using ModApi.Craft;
using ModApi.Craft.Parts;
using UnityEngine;
// Note: System.Threading types are fully qualified below to avoid ambiguity
// with ModApi.Craft.Program.Thread and ModApi.Craft.Program.Process.

namespace VizzyCodeMod
{
    /// <summary>
    /// Minimal HTTP server running on http://127.0.0.1:7842/ inside the game process.
    ///
    /// REST API:
    ///   GET  /status              → StatusResponse JSON
    ///   GET  /craft               → CraftInfoResponse JSON (parts + which have Vizzy)
    ///   GET  /vizzy/{partId}      → VizzyResponse JSON (full &lt;Program&gt; XML)
    ///   PUT  /vizzy/{partId}      → body: {"xml":"..."} → VizzyResponse JSON
    ///   GET  /stages              → StagesResponse JSON
    ///   POST /stages/activate     → activates next stage → StagesResponse JSON
    /// </summary>
    public class VizzyBridge
    {
        private readonly int _port;
        private HttpListener _listener;
        private System.Threading.Thread _thread;
        private volatile bool _running;

        // Work items that must run on Unity's main thread
        private readonly Queue<Action> _mainQueue = new();
        private readonly object _queueLock = new();
        private VizzyCodeUpdater _updater;

        public VizzyBridge(int port) { _port = port; }

        // ── Lifecycle ──────────────────────────────────────────────────────────

        public void Start()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://127.0.0.1:{_port}/");

            try { _listener.Start(); }
            catch (Exception ex)
            {
                Debug.LogError($"[VizzyCode] Cannot start HTTP listener on port {_port}: {ex.Message}");
                return;
            }

            _running = true;
            _thread  = new System.Threading.Thread(ListenLoop) { IsBackground = true, Name = "VizzyCodeBridge" };
            _thread.Start();

            _updater = VizzyCodeUpdater.EnsureExists();
            _updater.OnUpdate += FlushMainQueue;
            Debug.Log($"[VizzyCode] Bridge listening on http://127.0.0.1:{_port}/");
        }

        public void Stop()
        {
            _running = false;
            if (_updater != null) _updater.OnUpdate -= FlushMainQueue;
            try { _listener?.Stop(); } catch { }
            _thread?.Join(500);
        }

        // ── Listen loop ────────────────────────────────────────────────────────

        private void ListenLoop()
        {
            while (_running)
            {
                HttpListenerContext ctx;
                try { ctx = _listener.GetContext(); }
                catch { break; }
                System.Threading.ThreadPool.QueueUserWorkItem(_ => HandleRequest(ctx));
            }
        }

        private void HandleRequest(HttpListenerContext ctx)
        {
            var req  = ctx.Request;
            var resp = ctx.Response;
            resp.Headers.Add("Access-Control-Allow-Origin", "*");
            resp.ContentType = "application/json; charset=utf-8";

            try
            {
                string path   = req.Url.AbsolutePath.TrimEnd('/').ToLowerInvariant();
                string method = req.HttpMethod.ToUpperInvariant();

                string body = null;
                if (req.HasEntityBody)
                    using (var sr = new StreamReader(req.InputStream, Encoding.UTF8))
                        body = sr.ReadToEnd();

                if (method == "OPTIONS") { resp.StatusCode = 204; resp.Close(); return; }

                if      (method == "GET"  && path == "/status")             WriteJson(resp, RunMain(GetStatus));
                else if (method == "GET"  && path == "/craft")              WriteJson(resp, RunMain(GetCraftInfo));
                else if (method == "GET"  && path.StartsWith("/vizzy/"))    WriteJson(resp, RunMain(() => GetVizzy(ParseId(path, "/vizzy/"))));
                else if (method == "PUT"  && path.StartsWith("/vizzy/"))    WriteJson(resp, RunMain(() => SetVizzy(ParseId(path, "/vizzy/"), body)));
                else if (method == "GET"  && path == "/stages")             WriteJson(resp, RunMain(GetStages));
                else if (method == "POST" && path == "/stages/activate")    WriteJson(resp, RunMain(ActivateStage));
                else WriteError(resp, 404, "Not found");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[VizzyCode] Request error: {ex}");
                WriteError(resp, 500, ex.Message);
            }
            finally { try { resp.Close(); } catch { } }
        }

        // ── Endpoint implementations (called on main thread via RunMain) ────────

        private object GetStatus()
        {
            string scene = "menu";
            string craft = null;

            try
            {
                if (Game.InDesignerScene)
                {
                    scene = "designer";
                    craft = Game.Instance.Designer?.CraftScript?.Data?.Name;
                }
                else if (Game.InFlightScene)
                {
                    scene = "flight";
                    craft = Game.Instance.FlightScene?.CraftNode?.CraftScript?.Data?.Name;
                }
            }
            catch { /* scene transitions */ }

            return new StatusResponse { scene = scene, craftName = craft };
        }

        private object GetCraftInfo()
        {
            var cs = CurrentCraftScript();
            if (cs == null) return Err("No craft loaded");

            var info = new CraftInfoResponse { name = cs.Data?.Name ?? "Unknown" };
            foreach (var part in cs.Data.Assembly.Parts)
                info.parts.Add(new PartInfo
                {
                    id       = part.Id,
                    name     = part.Name,
                    hasVizzy = part.GetModifier<FlightProgramData>() != null
                });

            return info;
        }

        private object GetVizzy(int partId)
        {
            if (partId < 0) return Err("Invalid part id");
            var cs = CurrentCraftScript();
            if (cs == null) return Err("No craft loaded");

            var part = FindPart(cs, partId);
            if (part == null) return Err($"Part {partId} not found");

            var fpData = part.GetModifier<FlightProgramData>();
            if (fpData == null) return Err("Part has no Vizzy program");

            try
            {
                var fpXml = fpData.FlightProgramXml;
                if (fpXml == null) return Err("FlightProgramXml is null");

                // FlightProgramXml is the <Program> element — serialize it back to a string.
                return new VizzyResponse
                {
                    ok       = true,
                    partId   = partId,
                    partName = part.Name,
                    xml      = fpXml.ToString(SaveOptions.None)
                };
            }
            catch (Exception ex) { return Err(ex.Message); }
        }

        private object SetVizzy(int partId, string body)
        {
            if (partId < 0) return Err("Invalid part id");
            if (string.IsNullOrWhiteSpace(body)) return Err("Empty body");

            var cs = CurrentCraftScript();
            if (cs == null) return Err("No craft loaded");

            var part = FindPart(cs, partId);
            if (part == null) return Err($"Part {partId} not found");

            try
            {
                string xmlStr = JsonExtractString(body, "xml");
                if (xmlStr == null) return Err("Missing 'xml' field");

                var programXml = XElement.Parse(xmlStr);

                var fpData = part.GetModifier<FlightProgramData>();
                if (fpData == null) return Err("Part has no FlightProgramData modifier");

                fpData.FlightProgramXml = programXml;

                // If in designer, fire the structure-changed event so the UI refreshes.
                if (Game.InDesignerScene)
                    cs.RaiseDesignerCraftStructureChangedEvent();

                return new VizzyResponse { ok = true, partId = partId, partName = part.Name };
            }
            catch (Exception ex) { return Err(ex.Message); }
        }

        private object GetStages()
        {
            var cs = CurrentCraftScript();
            if (cs == null) return Err("No craft loaded");

            var pod = cs.ActiveCommandPod;
            if (pod == null) return Err("No active command pod");

            var r = new StagesResponse { currentStage = pod.CurrentStage, numStages = pod.NumStages };
            if (pod.ActivationGroupNames != null)
                foreach (var n in pod.ActivationGroupNames)
                    r.activationGroupNames.Add(n ?? "");

            for (int i = 0; i < r.activationGroupNames.Count; i++)
                r.activationGroupStates.Add(pod.GetActivationGroupState(i));

            return r;
        }

        private object ActivateStage()
        {
            var cs = CurrentCraftScript();
            if (cs == null) return Err("No craft loaded");
            cs.ActiveCommandPod?.ActivateStage();
            return GetStages();
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        private ICraftScript CurrentCraftScript()
        {
            try
            {
                if (Game.InDesignerScene) return Game.Instance.Designer?.CraftScript;
                if (Game.InFlightScene)   return Game.Instance.FlightScene?.CraftNode?.CraftScript;
            }
            catch { }
            return null;
        }

        private static PartData FindPart(ICraftScript cs, int id)
        {
            foreach (var p in cs.Data.Assembly.Parts)
                if (p.Id == id) return p;
            return null;
        }

        private static int ParseId(string path, string prefix)
        {
            return int.TryParse(path.Substring(prefix.Length), out int id) ? id : -1;
        }

        private static object Err(string msg) => new ErrorResponse { error = msg };

        // ── Main-thread dispatch ───────────────────────────────────────────────

        private T RunMain<T>(Func<T> fn)
        {
            T val   = default;
            var evt = new System.Threading.ManualResetEventSlim(false);

            lock (_queueLock)
                _mainQueue.Enqueue(() =>
                {
                    try   { val = fn(); }
                    catch (Exception ex) { Debug.LogError($"[VizzyCode] {ex}"); }
                    finally { evt.Set(); }
                });

            evt.Wait(5000);
            return val;
        }

        private void FlushMainQueue()
        {
            lock (_queueLock)
                while (_mainQueue.Count > 0)
                    _mainQueue.Dequeue()?.Invoke();
        }

        // ── JSON helpers ───────────────────────────────────────────────────────

        // Minimal JSON string field extractor to avoid needing Newtonsoft
        private static string JsonExtractString(string json, string key)
        {
            int ki = json.IndexOf($"\"{key}\"", StringComparison.Ordinal);
            if (ki < 0) return null;
            int colon = json.IndexOf(':', ki);
            if (colon < 0) return null;
            int q1 = json.IndexOf('"', colon + 1);
            if (q1 < 0) return null;
            var sb  = new StringBuilder();
            int pos = q1 + 1;
            while (pos < json.Length)
            {
                char c = json[pos];
                if (c == '\\' && pos + 1 < json.Length)
                {
                    char n = json[pos + 1];
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
                sb.Append(c);
                pos++;
            }
            return sb.ToString();
        }

        private static void WriteJson(HttpListenerResponse r, object obj)
        {
            string json  = ToJson(obj);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            r.StatusCode = 200; r.ContentLength64 = bytes.Length;
            r.OutputStream.Write(bytes, 0, bytes.Length);
        }

        private static void WriteError(HttpListenerResponse r, int code, string msg)
        {
            string json  = $"{{\"ok\":false,\"error\":{Jstr(msg)}}}";
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            r.StatusCode = code; r.ContentLength64 = bytes.Length;
            r.OutputStream.Write(bytes, 0, bytes.Length);
        }

        // Reflection-free JSON serializer for our simple DTOs (public fields only)
        private static string ToJson(object o)
        {
            if (o == null)        return "null";
            if (o is bool b)      return b ? "true" : "false";
            if (o is int i)       return i.ToString();
            if (o is string s)    return Jstr(s);
            if (o is System.Collections.IList list)
            {
                var items = new List<string>();
                foreach (var item in list) items.Add(ToJson(item));
                return "[" + string.Join(",", items) + "]";
            }
            var parts = new List<string>();
            foreach (var f in o.GetType().GetFields())
                parts.Add($"{Jstr(f.Name)}:{ToJson(f.GetValue(o))}");
            return "{" + string.Join(",", parts) + "}";
        }

        private static string Jstr(string s) =>
            s == null ? "null" :
            "\"" + s.Replace("\\","\\\\").Replace("\"","\\\"")
                    .Replace("\n","\\n").Replace("\r","\\r").Replace("\t","\\t") + "\"";
    }
}
