using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Assets.Scripts.Craft.Parts.Modifiers;
using ModApi.Common;
using ModApi.Craft;
using ModApi.Craft.Parts;
using ModApi.Flight;
using ModApi.Flight.Sim;
using UnityEngine;

namespace VizzyCodeMod
{
    /// <summary>
    /// Local HTTP server running on http://127.0.0.1:7842/ inside the game process.
    ///
    /// Vizzy read/write (designer + flight):
    ///   GET  /status
    ///   GET  /craft
    ///   GET  /craft/all          (flight only — all tracked craft nodes)
    ///   GET  /vizzy/{partId}
    ///   PUT  /vizzy/{partId}
    ///
    /// Craft intelligence (designer + flight):
    ///   GET  /snapshot
    ///   GET  /telemetry
    ///   GET  /telemetry/full     (extended: lat/lon, atmosphere, angular, solar)
    ///   GET  /parts/{partId}
    ///   GET  /stages
    ///
    /// Live action (flight only):
    ///   POST /stages/activate
    ///
    /// Planet / solar system (flight only):
    ///   GET  /planets
    ///   GET  /planets/{name}
    ///   GET  /universe/time
    ///   GET  /biome
    /// </summary>
    public class VizzyBridge
    {
        private readonly int _port;
        private HttpListener _listener;
        private System.Threading.Thread _thread;
        private volatile bool _running;

        private readonly Queue<Action> _mainQueue = new();
        private readonly object _queueLock = new();
        private VizzyCodeUpdater _updater;

        public VizzyBridge(int port) { _port = port; }

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
            _thread = new System.Threading.Thread(ListenLoop) { IsBackground = true, Name = "VizzyCodeBridge" };
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
            var req = ctx.Request;
            var resp = ctx.Response;
            resp.Headers.Add("Access-Control-Allow-Origin", "*");
            resp.ContentType = "application/json; charset=utf-8";

            try
            {
                string path = req.Url.AbsolutePath.TrimEnd('/').ToLowerInvariant();
                string method = req.HttpMethod.ToUpperInvariant();

                string body = null;
                if (req.HasEntityBody)
                    using (var sr = new StreamReader(req.InputStream, Encoding.UTF8))
                        body = sr.ReadToEnd();

                if (method == "OPTIONS") { resp.StatusCode = 204; resp.Close(); return; }

                if      (method == "GET"  && path == "/status")               WriteJson(resp, RunMain(GetStatus));
                else if (method == "GET"  && path == "/craft")                WriteJson(resp, RunMain(GetCraftInfo));
                else if (method == "GET"  && path == "/craft/all")            WriteJson(resp, RunMain(GetAllCraftNodes));
                else if (method == "GET"  && path == "/snapshot")             WriteJson(resp, RunMain(GetSnapshot));
                else if (method == "GET"  && path == "/telemetry")            WriteJson(resp, RunMain(GetTelemetry));
                else if (method == "GET"  && path == "/telemetry/full")       WriteJson(resp, RunMain(GetTelemetryFull));
                else if (method == "GET"  && path.StartsWith("/parts/"))      WriteJson(resp, RunMain(() => GetPartDetails(ParseId(path, "/parts/"))));
                else if (method == "GET"  && path.StartsWith("/vizzy/"))      WriteJson(resp, RunMain(() => GetVizzy(ParseId(path, "/vizzy/"))));
                else if (method == "PUT"  && path.StartsWith("/vizzy/"))      WriteJson(resp, RunMain(() => SetVizzy(ParseId(path, "/vizzy/"), body)));
                else if (method == "GET"  && path == "/stages")               WriteJson(resp, RunMain(GetStages));
                else if (method == "POST" && path == "/stages/activate")      WriteJson(resp, RunMain(ActivateStage));
                else if (method == "GET"  && path == "/planets")              WriteJson(resp, RunMain(GetPlanets));
                else if (method == "GET"  && path.StartsWith("/planets/"))    WriteJson(resp, RunMain(() => GetPlanetDetails(path.Substring("/planets/".Length))));
                else if (method == "GET"  && path == "/universe/time")        WriteJson(resp, RunMain(GetUniverseTime));
                else if (method == "GET"  && path == "/biome")                WriteJson(resp, RunMain(GetBiome));
                else WriteError(resp, 404, "Not found");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[VizzyCode] Request error: {ex}");
                WriteError(resp, 500, ex.Message);
            }
            finally { try { resp.Close(); } catch { } }
        }

        private object GetStatus()
        {
            string scene = CurrentScene();
            string craft = null;

            try
            {
                if (Game.InDesignerScene)
                    craft = Game.Instance.Designer?.CraftScript?.Data?.Name;
                else if (Game.InFlightScene)
                    craft = Game.Instance.FlightScene?.CraftNode?.CraftScript?.Data?.Name;
            }
            catch { }

            return new StatusResponse { scene = scene, craftName = craft };
        }

        private object GetCraftInfo()
        {
            var cs = CurrentCraftScript();
            if (cs == null) return Err("No craft loaded");
            return BuildCraftInfo(cs);
        }

        private object GetSnapshot()
        {
            try { return GetSnapshotInternal(); }
            catch (Exception ex) { return Err($"snapshot exception: {ex.GetType().Name}: {ex.Message}"); }
        }

        private object GetSnapshotInternal()
        {
            var status = (StatusResponse)GetStatus();
            var snapshot = new BridgeSnapshotResponse
            {
                scene = status.scene,
                craftName = status.craftName,
                capturedUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            var cs = CurrentCraftScript();
            if (cs == null)
            {
                snapshot.ok = false;
                snapshot.notes.Add("No active craft script is available in the current scene.");
                snapshot.telemetry = BuildTelemetry(null);
                return snapshot;
            }

            try { snapshot.craft = BuildCraftInfo(cs); } catch (Exception ex) { snapshot.notes.Add($"craft error: {ex.Message}"); }
            try { snapshot.stages = BuildStages(cs); } catch (Exception ex) { snapshot.notes.Add($"stages error: {ex.Message}"); }
            try { snapshot.telemetry = BuildTelemetry(cs); } catch (Exception ex) { snapshot.notes.Add($"telemetry error: {ex.Message}"); }
            snapshot.notes.Add("Designer scenes provide static craft structure. Flight scenes provide full active-craft flight data when ICraftFlightData is available.");
            snapshot.notes.Add("Tracked or unloaded craft data should be treated as lower quality than active craft FlightData.");
            return snapshot;
        }

        private object GetTelemetry()
        {
            try { return BuildTelemetry(CurrentCraftScript()); }
            catch (Exception ex) { return Err($"telemetry exception: {ex.GetType().Name}: {ex.Message}"); }
        }

        private object GetPartDetails(int partId)
        {
            if (partId < 0) return Err("Invalid part id");
            var cs = CurrentCraftScript();
            if (cs == null) return Err("No craft loaded");
            var part = FindPart(cs, partId);
            if (part == null) return Err($"Part {partId} not found");
            return BuildPartInfo(cs, part);
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

                return new VizzyResponse
                {
                    ok = true,
                    partId = partId,
                    partName = part.Name,
                    xml = fpXml.ToString(SaveOptions.None)
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
            return BuildStages(cs);
        }

        private object ActivateStage()
        {
            var cs = CurrentCraftScript();
            if (cs == null) return Err("No craft loaded");
            cs.ActiveCommandPod?.ActivateStage();
            return BuildStages(cs);
        }

        private CraftInfoResponse BuildCraftInfo(ICraftScript cs)
        {
            var data = cs.Data;
            var asm = data?.Assembly;
            var info = new CraftInfoResponse
            {
                name = data?.Name ?? "Unknown",
                xmlVersion = SafeInt(data, "XmlVersion"),
                partCount = asm?.Parts?.Count ?? 0,
                bodyCount = asm?.Bodies?.Count ?? 0,
                connectionCount = asm?.PartConnections?.Count ?? 0,
                rootPartId = SafeInt(SafeObj(cs.RootPart, "Data"), "Id"),
                activeCommandPodId = data?.ActiveCommandPodId ?? 0,
                price = SafeDouble(data, "Price"),
                localCenterOfMass = ToVec3(SafeObj(data, "LocalCenterOfMass"))
            };

            if (asm?.Bodies != null)
            {
                foreach (var body in asm.Bodies)
                    info.bodies.Add(BuildBodyInfo(body));
            }

            if (asm?.PartConnections != null)
            {
                foreach (var c in asm.PartConnections)
                    info.connections.Add(BuildConnectionInfo(c));
            }

            if (asm?.Parts != null)
            {
                foreach (var part in asm.Parts)
                    info.parts.Add(BuildPartInfo(cs, part));
            }

            return info;
        }

        private PartInfo BuildPartInfo(ICraftScript cs, PartData part)
        {
            var pm = part.PartMass;
            var info = new PartInfo
            {
                id = part.Id,
                name = part.Name,
                tag = part.Tag,
                partType = part.PartType?.Name ?? SafeString(part.PartType, "Id") ?? "",
                hasVizzy = part.GetModifier<FlightProgramData>() != null,
                enabled = part.Enabled,
                activated = part.Activated,
                isRootPart = part.IsRootPart,
                activationStage = part.ActivationStage,
                activationGroup = part.ActivationGroup,
                activationStageOverride = part.ActivationStageOverride,
                commandPodId = part.CommandPodId ?? 0,
                symmetryId = part.SymmetryId?.ToString() ?? "",
                groupId = part.GroupId?.ToString() ?? "",
                mass = part.Mass,
                dryMass = pm.Dry,
                wetMass = pm.Wet,
                price = part.Price,
                position = ToVec3(part.Position),
                rotation = ToVec3(part.Rotation)
            };

            try
            {
                var body = cs.Data?.Assembly?.GetBodyByPartId(part.Id);
                info.bodyId = body?.Id.ToString() ?? "";
            }
            catch { }

            if (part.PartConnections != null)
            {
                foreach (var c in part.PartConnections)
                {
                    var other = c.GetOtherPart(part);
                    if (other != null) info.connectedPartIds.Add(other.Id);
                }
            }

            if (part.Modifiers != null)
            {
                foreach (var mod in part.Modifiers)
                    info.modifiers.Add(BuildModifierInfo(mod));
            }

            return info;
        }

        private BodyInfo BuildBodyInfo(BodyData body)
        {
            var info = new BodyInfo
            {
                id = body.Id.ToString(),
                mass = body.Mass,
                centerOfMass = ToVec3(body.CenterOfMass),
                position = ToVec3(body.Position),
                rotation = ToVec3(body.Rotation)
            };

            if (body.Parts != null)
                foreach (var p in body.Parts)
                    info.partIds.Add(p.Id);

            return info;
        }

        private ConnectionInfo BuildConnectionInfo(PartConnection c)
        {
            return new ConnectionInfo
            {
                partA = c.PartA?.Id ?? 0,
                partB = c.PartB?.Id ?? 0,
                symmetryId = c.SymmetryId?.ToString() ?? "",
                type = c.IsPhysicsJoint ? "physics-joint" : "part-connection"
            };
        }

        private ModifierInfo BuildModifierInfo(PartModifierData mod)
        {
            var info = new ModifierInfo
            {
                typeName = mod.GetType().FullName,
                name = mod.Name,
                typeId = mod.TypeId,
                id = mod.Id,
                inputId = mod.InputId,
                inspectorEnabled = mod.InspectorEnabled,
                partPropertiesEnabled = mod.PartPropertiesEnabled,
                mass = mod.Mass,
                dryMass = mod.MassDry,
                wetMass = mod.MassWet,
                price = mod.Price,
                scale = mod.Scale
            };

            try
            {
                var groups = mod.GetAssociatedActivationGroups();
                if (groups != null)
                    foreach (var group in groups)
                        info.activationGroups.Add(group);
            }
            catch { }

            return info;
        }

        private StagesResponse BuildStages(ICraftScript cs)
        {
            var pod = cs.ActiveCommandPod;
            if (pod == null) return new StagesResponse { ok = false, error = "No active command pod" };

            var r = new StagesResponse { currentStage = pod.CurrentStage, numStages = pod.NumStages };
            if (pod.ActivationGroupNames != null)
                foreach (var n in pod.ActivationGroupNames)
                    r.activationGroupNames.Add(n ?? "");

            for (int i = 0; i < r.activationGroupNames.Count; i++)
                r.activationGroupStates.Add(pod.GetActivationGroupState(i));

            var byStage = new SortedDictionary<int, StageInfo>();
            var parts = cs.Data?.Assembly?.Parts;
            if (parts != null)
            {
                foreach (var part in parts)
                {
                    int stage = part.ActivationStage;
                    if (!byStage.TryGetValue(stage, out var si))
                    {
                        si = new StageInfo { stage = stage, isCurrent = stage == pod.CurrentStage };
                        byStage.Add(stage, si);
                    }
                    si.partIds.Add(part.Id);
                    si.partNames.Add(part.Name ?? "");
                    si.partCount++;
                }
            }

            foreach (var stage in byStage.Values)
                r.stages.Add(stage);

            return r;
        }

        private TelemetryResponse BuildTelemetry(ICraftScript cs)
        {
            var scene = CurrentScene();
            var t = new TelemetryResponse
            {
                scene = scene,
                quality = "no_craft",
                activeFullFlightData = false,
                trackedFallback = false,
                hasCraftScript = cs != null,
                hasCraftNode = false,
                hasCommandPod = false,
                physicsEnabled = false
            };

            if (cs == null) return t;

            try { t.craftName = cs.Data?.Name; } catch { }
            try { t.hasCommandPod = cs.ActiveCommandPod != null; } catch { }
            try { t.physicsEnabled = cs.IsPhysicsEnabled; } catch { }
            try { t.position = ToVec3(cs.FramePosition); } catch { }
            try { t.surfaceVelocity = ToVec3(cs.SurfaceVelocity); } catch { }
            try { t.gravity = ToVec3(cs.GravityForce); } catch { }
            try { t.gravityMagnitude = cs.GravityMagnitude; } catch { }
            try { t.currentMassKg = cs.Mass; } catch { }

            var node = SafeObj(cs, "CraftNode");
            t.hasCraftNode = node != null;
            if (node != null)
            {
                t.craftNodeId = SafeInt(node, "NodeId");
                t.parentName = SafeString(SafeObj(node, "Parent"), "Name") ?? SafeString(node, "ParentName") ?? SafeString(node, "ParentBodyName") ?? "";
                t.altitudeASL = SafeDouble(node, "Altitude");
                t.altitudeAGL = SafeDouble(node, "AltitudeAgl");
                t.altitudeTerrain = SafeDouble(node, "AltitudeAboveTerrain");
                t.grounded = SafeBool(node, "InContactWithPlanet");
                t.inWater = SafeBool(node, "InContactWithWater");
            }

            try
            {
                var fd = cs.FlightData;
                if (fd == null)
                {
                    t.quality = scene == "designer" ? "designer_static" : "flight_no_flightdata";
                    t.trackedFallback = node != null;
                    return t;
                }
                t.quality = "flight_active_full";
                t.activeFullFlightData = true;
                t.timeReal = SafeDouble(fd, "TimeReal");
                t.timeDelta = SafeDouble(fd, "TimeDelta");
                t.timeMultiplier = SafeDouble(fd, "TimeMultiplier");
                t.altitudeASL = fd.AltitudeAboveSeaLevel;
                t.altitudeAGL = fd.AltitudeAboveGroundLevel;
                t.altitudeTerrain = fd.AltitudeAboveTerrain;
                t.velocityMagnitude = fd.VelocityMagnitude;
                t.surfaceVelocityMagnitude = fd.SurfaceVelocityMagnitude;
                t.verticalSurfaceVelocity = fd.VerticalSurfaceVelocity;
                t.lateralSurfaceVelocity = fd.LateralSurfaceVelocity;
                t.accelerationMagnitude = fd.AccelerationMagnitude;
                t.machNumber = fd.MachNumber;
                t.angleOfAttack = fd.AngleOfAttack;
                t.pitch = fd.Pitch;
                t.heading = fd.Heading;
                t.bankAngle = fd.BankAngle;
                t.sideSlip = fd.SideSlip;
                t.gravityMagnitude = fd.GravityMagnitude;
                t.currentMassKg = fd.CurrentMassUnscaled;
                t.fuelMass = fd.FuelMass;
                t.remainingFuelInStage = fd.RemainingFuelInStage;
                t.remainingBattery = fd.RemainingBattery;
                t.remainingMonopropellant = fd.RemainingMonopropellant;
                t.currentEngineThrustN = fd.CurrentEngineThrustUnscaled;
                t.maxActiveEngineThrustN = fd.MaxActiveEngineThrustUnscaled;
                t.currentReactionControlNozzleThrust = fd.CurrentReactionControlNozzleThrust;
                t.grounded = fd.Grounded;
                t.inWater = fd.InWater;
                t.supportsWarpBurn = fd.SupportsWarpBurn;
                t.solarRadiationIntensity = fd.SolarRadiationIntensity;
                t.parentPlanetOcclusion = fd.ParentPlanetOcclusion;
                t.position = ToVec3(fd.Position);
                t.velocity = ToVec3(fd.Velocity);
                t.surfaceVelocity = ToVec3(fd.SurfaceVelocity);
                t.acceleration = ToVec3(fd.Acceleration);
                t.gravity = ToVec3(fd.Gravity);
                t.craftForward = ToVec3(fd.CraftForward);
                t.craftUp = ToVec3(fd.CraftUp);
                t.craftRight = ToVec3(fd.CraftRight);
                t.orbit = BuildOrbitInfo(fd.Orbit);
                t.performance = BuildPerformanceInfo(fd.Performance);
                t.navTarget = BuildTargetInfo(fd.NavSphereTarget);
            }
            catch
            {
                t.quality = scene == "designer" ? "designer_static" : "flight_no_flightdata";
                t.trackedFallback = node != null;
            }
            return t;
        }

        private OrbitInfo BuildOrbitInfo(object orbit)
        {
            if (orbit == null) return null;
            return new OrbitInfo
            {
                parent = SafeString(SafeObj(orbit, "Parent"), "Name") ?? SafeString(orbit, "ParentName") ?? "",
                apoapsisAltitude = SafeDouble(orbit, "ApoapsisAltitude"),
                periapsisAltitude = SafeDouble(orbit, "PeriapsisAltitude"),
                apoapsisTime = SafeDouble(orbit, "ApoapsisTime"),
                periapsisTime = SafeDouble(orbit, "PeriapsisTime"),
                eccentricity = SafeDouble(orbit, "Eccentricity"),
                inclination = SafeDouble(orbit, "Inclination"),
                period = SafeDouble(orbit, "Period"),
                semiMajorAxis = SafeDouble(orbit, "SemiMajorAxis"),
                semiMinorAxis = SafeDouble(orbit, "SemiMinorAxis"),
                trueAnomaly = SafeDouble(orbit, "TrueAnomaly"),
                meanAnomaly = SafeDouble(orbit, "MeanAnomaly"),
                meanMotion = SafeDouble(orbit, "MeanMotion"),
                rightAscension = SafeDouble(orbit, "RightAscension"),
                periapsisArgument = SafeDouble(orbit, "PeriapsisArgument"),
                burnNodeDeltaV = SafeDouble(orbit, "BurnNodeDeltaV"),
                hasBurnNodePoint = SafeObj(orbit, "BurnNodePoint") != null
            };
        }

        private PerformanceInfo BuildPerformanceInfo(object perf)
        {
            if (perf == null) return null;
            return new PerformanceInfo
            {
                currentIsp = SafeDouble(perf, "CurrentIsp"),
                deltaVStage = SafeDouble(perf, "DeltaVStage"),
                fuelAllStagesPercentage = SafeDouble(perf, "FuelAllStagesPercentage"),
                remainingBurnTime = SafeDouble(perf, "RemainingBurnTime"),
                thrustToWeightRatio = SafeDouble(perf, "ThrustToWeightRatio")
            };
        }

        private TargetInfo BuildTargetInfo(object target)
        {
            var info = new TargetInfo { exists = target != null };
            if (target == null) return info;

            info.rawType = target.GetType().FullName;
            info.type = SafeString(target, "TargetType") ?? SafeString(target, "Type") ?? target.GetType().Name;
            info.name = SafeString(target, "Name") ?? SafeString(target, "DisplayName");
            info.bodyName = SafeString(target, "PlanetName") ?? SafeString(target, "BodyName");
            info.position = ToVec3(SafeObj(target, "Position"));
            return info;
        }

        // ── Planet endpoints ──────────────────────────────────────────────────

        private object GetPlanets()
        {
            try
            {
                var root = GetRootPlanetNode();
                if (root == null) return Err("Planet tree unavailable — requires flight scene");

                var response = new PlanetsResponse();
                CollectPlanets(root, response.planets, "");
                return response;
            }
            catch (Exception ex) { return Err(ex.Message); }
        }

        private object GetPlanetDetails(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Err("Planet name required");

            try
            {
                var root = GetRootPlanetNode();
                if (root == null) return Err("Planet tree unavailable — requires flight scene");

                var node = FindPlanetNode(root, name.ToLowerInvariant());
                if (node == null) return Err($"Planet '{name}' not found");

                return new PlanetDetailResponse { planet = BuildPlanetDetail(node) };
            }
            catch (Exception ex) { return Err(ex.Message); }
        }

        // ── Extended telemetry ────────────────────────────────────────────────

        private object GetTelemetryFull()
        {
            try { return GetTelemetryFullInternal(); }
            catch (Exception ex) { return Err($"telemetry/full exception: {ex.GetType().Name}: {ex.Message}"); }
        }

        private object GetTelemetryFullInternal()
        {
            var cs = CurrentCraftScript();
            var scene = CurrentScene();
            var t = new TelemetryFullResponse { scene = scene };

            if (cs == null)
            {
                t.ok = false;
                t.quality = "no_craft";
                return t;
            }

            // Copy base fields from BuildTelemetry
            try { t.craftName = cs.Data?.Name; } catch { }
            t.hasCraftScript = true;
            try { t.hasCommandPod = cs.ActiveCommandPod != null; } catch { }
            try { t.physicsEnabled = cs.IsPhysicsEnabled; } catch { }
            t.quality = scene == "designer" ? "designer_static" : "flight_no_flightdata";

            var node = SafeObj(cs, "CraftNode");
            t.hasCraftNode = node != null;
            if (node != null)
            {
                t.craftNodeId = SafeInt(node, "NodeId");
                t.parentName = SafeString(SafeObj(node, "Parent"), "Name") ?? SafeString(node, "ParentName") ?? SafeString(node, "ParentBodyName") ?? "";
                t.altitudeASL = SafeDouble(node, "Altitude");
                t.altitudeAGL = SafeDouble(node, "AltitudeAgl");
                t.altitudeTerrain = SafeDouble(node, "AltitudeAboveTerrain");
                t.grounded = SafeBool(node, "InContactWithPlanet");
                t.inWater = SafeBool(node, "InContactWithWater");

                // Lat/lon from ICraftNode (radians → degrees)
                var latLon = SafeObj(node, "LatLon");
                if (latLon != null)
                {
                    double latRad = SafeCoord(latLon, "x");
                    double lonRad = SafeCoord(latLon, "y");
                    t.latDeg = latRad * (180.0 / Math.PI);
                    t.lonDeg = lonRad * (180.0 / Math.PI);
                }
            }

            try
            {
            var fd = cs.FlightData;
            if (fd != null)
            {
                t.quality = "flight_active_full";
                t.activeFullFlightData = true;
                t.timeReal = SafeDouble(fd, "TimeReal");
                t.timeDelta = SafeDouble(fd, "TimeDelta");
                t.timeMultiplier = SafeDouble(fd, "TimeMultiplier");
                t.altitudeASL = fd.AltitudeAboveSeaLevel;
                t.altitudeAGL = fd.AltitudeAboveGroundLevel;
                t.altitudeTerrain = fd.AltitudeAboveTerrain;
                t.velocityMagnitude = fd.VelocityMagnitude;
                t.surfaceVelocityMagnitude = fd.SurfaceVelocityMagnitude;
                t.verticalSurfaceVelocity = fd.VerticalSurfaceVelocity;
                t.lateralSurfaceVelocity = fd.LateralSurfaceVelocity;
                t.accelerationMagnitude = fd.AccelerationMagnitude;
                t.machNumber = fd.MachNumber;
                t.angleOfAttack = fd.AngleOfAttack;
                t.pitch = fd.Pitch;
                t.heading = fd.Heading;
                t.bankAngle = fd.BankAngle;
                t.sideSlip = fd.SideSlip;
                t.gravityMagnitude = fd.GravityMagnitude;
                t.currentMassKg = fd.CurrentMassUnscaled;
                t.fuelMass = fd.FuelMass;
                t.remainingFuelInStage = fd.RemainingFuelInStage;
                t.remainingBattery = fd.RemainingBattery;
                t.remainingMonopropellant = fd.RemainingMonopropellant;
                t.currentEngineThrustN = fd.CurrentEngineThrustUnscaled;
                t.maxActiveEngineThrustN = fd.MaxActiveEngineThrustUnscaled;
                t.currentReactionControlNozzleThrust = fd.CurrentReactionControlNozzleThrust;
                t.grounded = fd.Grounded;
                t.inWater = fd.InWater;
                t.supportsWarpBurn = fd.SupportsWarpBurn;
                t.solarRadiationIntensity = fd.SolarRadiationIntensity;
                t.parentPlanetOcclusion = fd.ParentPlanetOcclusion;
                t.position = ToVec3(fd.Position);
                t.velocity = ToVec3(fd.Velocity);
                t.surfaceVelocity = ToVec3(fd.SurfaceVelocity);
                t.acceleration = ToVec3(fd.Acceleration);
                t.gravity = ToVec3(fd.Gravity);
                t.craftForward = ToVec3(fd.CraftForward);
                t.craftUp = ToVec3(fd.CraftUp);
                t.craftRight = ToVec3(fd.CraftRight);
                t.orbit = BuildOrbitInfo(fd.Orbit);
                t.performance = BuildPerformanceInfo(fd.Performance);
                t.navTarget = BuildTargetInfo(fd.NavSphereTarget);

                // Extended fields not in base telemetry
                t.angularVelocityMagnitude = SafeDouble(fd, "AngularVelocityMagnitude");
                t.angularVelocity = ToVec3(SafeObj(fd, "AngularVelocity"));
                t.dragAccelerationMagnitude = SafeDouble(fd, "DragAccelerationMagnitude");
                t.north = ToVec3(SafeObj(fd, "North"));
                t.east = ToVec3(SafeObj(fd, "East"));
                t.solarRadiationDirection = ToVec3(SafeObj(fd, "SolarRadiationDirection"));
                t.positionNormalized = ToVec3(SafeObj(fd, "PositionNormalized"));

                // Atmosphere sample at current position
                var atm = SafeObj(fd, "AtmosphereSample");
                if (atm != null)
                {
                    t.atmosphereSample = new AtmosphereSampleInfo
                    {
                        density = SafeDouble(atm, "Density"),
                        pressure = SafeDouble(atm, "Pressure"),
                        temperature = SafeDouble(atm, "Temperature"),
                        speedOfSound = SafeDouble(atm, "SpeedOfSound")
                    };
                }
            }
            } catch { /* FlightData not available in this scene */ }

            // Universal time from flight state
            try
            {
                var flightState = Game.Instance.FlightScene?.FlightState;
                if (flightState != null)
                    t.universalTime = SafeDouble(flightState, "Time");
            }
            catch { }

            return t;
        }

        // ── Universe time ─────────────────────────────────────────────────────

        private object GetUniverseTime()
        {
            var r = new UniverseTimeResponse();
            try
            {
                if (Game.InFlightScene)
                {
                    var flightState = Game.Instance.FlightScene?.FlightState;
                    r.universalTime = SafeDouble(flightState, "Time");

                    var cs = CurrentCraftScript();
                    var fd = cs?.FlightData;
                    if (fd != null)
                    {
                        r.timeReal = SafeDouble(fd, "TimeReal");
                        r.timeDelta = SafeDouble(fd, "TimeDelta");
                        r.timeMultiplier = SafeDouble(fd, "TimeMultiplier");
                    }

                    // TimeSinceLaunch from CraftNode or FlightProgramData fallback
                    r.timeSinceLaunch = SafeDouble(cs, "TimeSinceLaunch");
                }
            }
            catch (Exception ex) { return Err(ex.Message); }
            return r;
        }

        // ── All craft nodes ───────────────────────────────────────────────────

        private object GetAllCraftNodes()
        {
            if (!Game.InFlightScene) return Err("Craft list requires flight scene");
            try
            {
                var flightState = Game.Instance.FlightScene?.FlightState;
                var craftNodes = SafeObj(flightState, "CraftNodes");
                var r = new AllCraftResponse();

                if (craftNodes is IEnumerable list)
                {
                    foreach (var node in list)
                    {
                        if (node == null) continue;
                        var summary = new CraftNodeSummary
                        {
                            nodeId = SafeInt(node, "NodeId"),
                            name = SafeString(node, "Name") ?? "",
                            isPlayer = SafeBool(node, "IsPlayer"),
                            altitude = SafeDouble(node, "Altitude"),
                            altitudeAgl = SafeDouble(node, "AltitudeAgl"),
                            parentPlanet = SafeString(SafeObj(node, "Parent"), "Name") ?? SafeString(node, "ParentName") ?? SafeString(node, "ParentBodyName") ?? "",
                            hasCommandPod = SafeBool(node, "HasCommandPod"),
                        };

                        // Part count from CraftScript
                        var csObj = SafeObj(node, "CraftScript");
                        var dataObj = SafeObj(csObj, "Data");
                        var asmObj = SafeObj(dataObj, "Assembly");
                        var partsObj = SafeObj(asmObj, "Parts");
                        summary.partCount = partsObj is ICollection col ? col.Count : SafeInt(partsObj, "Count");

                        // Lat/lon
                        var latLon = SafeObj(node, "LatLon");
                        if (latLon != null)
                        {
                            summary.latDeg = SafeCoord(latLon, "x") * (180.0 / Math.PI);
                            summary.lonDeg = SafeCoord(latLon, "y") * (180.0 / Math.PI);
                        }

                        // Surface velocity
                        summary.surfaceVelocity = ToVec3(SafeObj(node, "SurfaceVelocity"));

                        r.crafts.Add(summary);
                    }
                }

                return r;
            }
            catch (Exception ex) { return Err(ex.Message); }
        }

        // ── Biome ─────────────────────────────────────────────────────────────

        private object GetBiome()
        {
            if (!Game.InFlightScene) return Err("Biome data requires flight scene");
            try
            {
                var flightScene = Game.Instance.FlightScene;
                var biomeData = SafeObj(flightScene, "CraftBiomeData");

                var r = new BiomeResponse
                {
                    biome = SafeString(biomeData, "BiomeName") ?? SafeString(biomeData, "Name") ?? "unknown",
                    planet = SafeString(biomeData, "PlanetName") ?? SafeString(biomeData, "BodyName")
                          ?? SafeString(SafeObj(biomeData, "Planet"), "Name")
                          ?? SafeString(SafeObj(biomeData, "Body"), "Name") ?? ""
                };

                // Try to get lat/lon from craft node
                var craftNode = Game.Instance.FlightScene?.CraftNode;
                if (craftNode != null)
                {
                    var latLon = SafeObj(craftNode, "LatLon");
                    if (latLon != null)
                    {
                        r.latDeg = SafeCoord(latLon, "x") * (180.0 / Math.PI);
                        r.lonDeg = SafeCoord(latLon, "y") * (180.0 / Math.PI);
                    }
                }

                return r;
            }
            catch (Exception ex) { return Err(ex.Message); }
        }

        // ── Planet helpers ────────────────────────────────────────────────────

        private object GetRootPlanetNode()
        {
            try
            {
                if (!Game.InFlightScene) return null;
                var flightState = Game.Instance.FlightScene?.FlightState;
                return SafeObj(flightState, "RootNode");
            }
            catch { return null; }
        }

        private void CollectPlanets(object node, List<PlanetSummary> list, string parentName)
        {
            if (node == null) return;
            var pd = SafeObj(node, "PlanetData");
            if (pd == null) return; // not a planet node

            var atm = SafeObj(pd, "AtmosphereData");
            var name = SafeString(pd, "Name") ?? "";
            var children = SafeObj(node, "ChildPlanets");
            int childCount = children is ICollection cc ? cc.Count : 0;

            list.Add(new PlanetSummary
            {
                name = name,
                parent = parentName,
                radius = SafeDouble(pd, "Radius"),
                mass = SafeDouble(pd, "Mass"),
                surfaceGravity = SafeDouble(pd, "SurfaceGravity"),
                escapeVelocity = SafeDouble(pd, "EscapeVelocity"),
                hasAtmosphere = SafeBool(atm, "HasPhysicsAtmosphere"),
                hasWater = SafeBool(pd, "HasWater"),
                childCount = childCount
            });

            if (children is IEnumerable childList)
                foreach (var child in childList)
                    CollectPlanets(child, list, name);
        }

        private object FindPlanetNode(object node, string nameLower)
        {
            if (node == null) return null;
            var pd = SafeObj(node, "PlanetData");
            if (pd != null)
            {
                var pname = (SafeString(pd, "Name") ?? "").ToLowerInvariant();
                if (pname == nameLower) return node;
            }

            var children = SafeObj(node, "ChildPlanets");
            if (children is IEnumerable childList)
                foreach (var child in childList)
                {
                    var found = FindPlanetNode(child, nameLower);
                    if (found != null) return found;
                }
            return null;
        }

        private PlanetDetailInfo BuildPlanetDetail(object node)
        {
            var pd = SafeObj(node, "PlanetData");
            var parentPd = SafeObj(pd, "Parent");
            var info = new PlanetDetailInfo
            {
                name = SafeString(pd, "Name") ?? "",
                parent = SafeString(parentPd, "Name") ?? "",
                radius = SafeDouble(pd, "Radius"),
                mass = SafeDouble(pd, "Mass"),
                surfaceGravity = SafeDouble(pd, "SurfaceGravity"),
                escapeVelocity = SafeDouble(pd, "EscapeVelocity"),
                angularVelocity = SafeDouble(pd, "AngularVelocity"),
                impactRadius = SafeDouble(pd, "ImpactRadius"),
                hasWater = SafeBool(pd, "HasWater"),
                hasTerrainPhysics = SafeBool(pd, "HasTerrainPhysics"),
                maxTerrainElevation = SafeDouble(pd, "MaxEstimatedTerrainElevation"),
                atmosphere = BuildPlanetAtmosphere(SafeObj(pd, "AtmosphereData")),
                currentOrbit = BuildPlanetCurrentOrbit(node)
            };

            var children = SafeObj(node, "ChildPlanets");
            if (children is IEnumerable childList)
                foreach (var child in childList)
                {
                    var cpd = SafeObj(SafeObj(child, "PlanetData"), "Name");
                    if (cpd != null) info.children.Add(cpd.ToString());
                }

            return info;
        }

        private PlanetAtmosphereInfo BuildPlanetAtmosphere(object atm)
        {
            if (atm == null) return new PlanetAtmosphereInfo { hasAtmosphere = false };
            return new PlanetAtmosphereInfo
            {
                hasAtmosphere = SafeBool(atm, "HasPhysicsAtmosphere"),
                height = SafeDouble(atm, "Height"),
                crushAltitude = SafeDouble(atm, "CrushAltitude"),
                scaleHeight = SafeDouble(atm, "ScaleHeight"),
                surfaceAirDensity = SafeDouble(atm, "SurfaceAirDensity"),
                meanSurfaceTemperature = SafeDouble(atm, "MeanSurfaceTemperature"),
                meanSurfaceTemperatureDay = SafeDouble(atm, "MeanSurfaceTemperatureDay"),
                meanSurfaceTemperatureNight = SafeDouble(atm, "MeanSurfaceTemperatureNight"),
                meanMassPerMolecule = SafeDouble(atm, "MeanMassPerMolecule"),
                meanGamma = SafeDouble(atm, "MeanGamma")
            };
        }

        private PlanetCurrentOrbitInfo BuildPlanetCurrentOrbit(object node)
        {
            if (node == null) return null;
            var info = new PlanetCurrentOrbitInfo
            {
                sphereOfInfluence = SafeDouble(node, "SphereOfInfluence"),
            };

            // Solar position/velocity from INode base
            info.solarPosition = ToVec3(SafeObj(node, "SolarPosition"));
            info.solarVelocity = ToVec3(SafeObj(node, "SolarVelocity"));

            // Orbit mechanics from IOrbitNode.Orbit
            var orbit = SafeObj(node, "Orbit");
            if (orbit != null)
            {
                info.apoapsis = SafeDouble(orbit, "Apoapsis");
                info.periapsis = SafeDouble(orbit, "Periapsis");
                info.eccentricity = SafeDouble(orbit, "Eccentricity");
                info.inclination = SafeDouble(orbit, "Inclination");
                info.period = SafeDouble(orbit, "Period");
                info.semiMajorAxis = SafeDouble(orbit, "SemiMajorAxis");
                info.trueAnomaly = SafeDouble(orbit, "TrueAnomaly");
                info.rightAscension = SafeDouble(orbit, "RightAscensionOfAscendingNode");
                info.periapsisArgument = SafeDouble(orbit, "PeriapsisAngle");
            }

            return info;
        }

        private ICraftScript CurrentCraftScript()
        {
            try
            {
                if (Game.InDesignerScene) return Game.Instance.Designer?.CraftScript;
                if (Game.InFlightScene) return Game.Instance.FlightScene?.CraftNode?.CraftScript;
            }
            catch { }
            return null;
        }

        private string CurrentScene()
        {
            try
            {
                if (Game.InDesignerScene) return "designer";
                if (Game.InFlightScene) return "flight";
            }
            catch { }
            return "menu";
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

        private T RunMain<T>(Func<T> fn)
        {
            T val = default;
            var evt = new System.Threading.ManualResetEventSlim(false);

            lock (_queueLock)
                _mainQueue.Enqueue(() =>
                {
                    try { val = fn(); }
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

        private static object SafeObj(object source, string propertyName)
        {
            if (source == null) return null;
            try
            {
                var p = source.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
                return p?.GetValue(source, null);
            }
            catch { return null; }
        }

        private static string SafeString(object source, string propertyName)
        {
            var v = SafeObj(source, propertyName);
            return v?.ToString();
        }

        private static int SafeInt(object source, string propertyName)
        {
            var v = SafeObj(source, propertyName);
            if (v == null) return 0;
            try { return Convert.ToInt32(v, CultureInfo.InvariantCulture); }
            catch { return 0; }
        }

        private static bool SafeBool(object source, string propertyName)
        {
            var v = SafeObj(source, propertyName);
            if (v == null) return false;
            try { return Convert.ToBoolean(v, CultureInfo.InvariantCulture); }
            catch { return false; }
        }

        private static double SafeDouble(object source, string propertyName)
        {
            var v = SafeObj(source, propertyName);
            if (v == null) return 0d;
            try { return Convert.ToDouble(v, CultureInfo.InvariantCulture); }
            catch { return 0d; }
        }

        private static Vec3 ToVec3(object value)
        {
            if (value == null) return null;
            return new Vec3
            {
                x = SafeCoord(value, "x"),
                y = SafeCoord(value, "y"),
                z = SafeCoord(value, "z")
            };
        }

        private static double SafeCoord(object value, string fieldName)
        {
            if (value == null) return 0d;
            try
            {
                var t = value.GetType();
                var p = t.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Public);
                if (p != null) return Convert.ToDouble(p.GetValue(value, null), CultureInfo.InvariantCulture);
                var f = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
                if (f != null) return Convert.ToDouble(f.GetValue(value), CultureInfo.InvariantCulture);
            }
            catch { }
            return 0d;
        }

        private static string JsonExtractString(string json, string key)
        {
            int ki = json.IndexOf($"\"{key}\"", StringComparison.Ordinal);
            if (ki < 0) return null;
            int colon = json.IndexOf(':', ki);
            if (colon < 0) return null;
            int q1 = json.IndexOf('"', colon + 1);
            if (q1 < 0) return null;
            var sb = new StringBuilder();
            int pos = q1 + 1;
            while (pos < json.Length)
            {
                char c = json[pos];
                if (c == '\\' && pos + 1 < json.Length)
                {
                    char n = json[pos + 1];
                    switch (n)
                    {
                        case '"': sb.Append('"'); pos += 2; continue;
                        case '\\': sb.Append('\\'); pos += 2; continue;
                        case 'n': sb.Append('\n'); pos += 2; continue;
                        case 'r': sb.Append('\r'); pos += 2; continue;
                        case 't': sb.Append('\t'); pos += 2; continue;
                        default: sb.Append(n); pos += 2; continue;
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
            string json = ToJson(obj);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            r.StatusCode = 200;
            r.ContentLength64 = bytes.Length;
            r.OutputStream.Write(bytes, 0, bytes.Length);
        }

        private static void WriteError(HttpListenerResponse r, int code, string msg)
        {
            string json = $"{{\"ok\":false,\"error\":{Jstr(msg)}}}";
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            r.StatusCode = code;
            r.ContentLength64 = bytes.Length;
            r.OutputStream.Write(bytes, 0, bytes.Length);
        }

        private static string ToJson(object o)
        {
            if (o == null) return "null";
            if (o is string s) return Jstr(s);
            if (o is bool b) return b ? "true" : "false";
            if (o is byte || o is sbyte || o is short || o is ushort ||
                o is int || o is uint || o is long || o is ulong)
                return Convert.ToString(o, CultureInfo.InvariantCulture);
            if (o is float || o is double || o is decimal)
            {
                double d = Convert.ToDouble(o, CultureInfo.InvariantCulture);
                if (double.IsNaN(d) || double.IsInfinity(d)) return "null";
                return d.ToString("R", CultureInfo.InvariantCulture);
            }
            if (o is Enum) return Jstr(o.ToString());
            if (o is IDictionary dict)
            {
                var items = new List<string>();
                foreach (DictionaryEntry entry in dict)
                    items.Add($"{Jstr(Convert.ToString(entry.Key, CultureInfo.InvariantCulture))}:{ToJson(entry.Value)}");
                return "{" + string.Join(",", items) + "}";
            }
            if (o is IEnumerable list)
            {
                var items = new List<string>();
                foreach (var item in list) items.Add(ToJson(item));
                return "[" + string.Join(",", items) + "]";
            }

            var parts = new List<string>();
            foreach (var f in o.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
                parts.Add($"{Jstr(f.Name)}:{ToJson(f.GetValue(o))}");
            return "{" + string.Join(",", parts) + "}";
        }

        private static string Jstr(string s) =>
            s == null ? "null" :
            "\"" + s.Replace("\\", "\\\\").Replace("\"", "\\\"")
                    .Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t") + "\"";
    }
}
