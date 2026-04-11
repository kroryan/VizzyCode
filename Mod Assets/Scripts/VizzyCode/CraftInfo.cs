using System.Collections.Generic;

namespace VizzyCodeMod
{
    // Data transfer objects serialized by the bridge's tiny JSON writer.

    public class StatusResponse
    {
        public bool ok = true;
        public string modVersion = VizzyCodeMod.ModVersion;
        public string scene;
        public string craftName;
    }

    public class CraftInfoResponse
    {
        public bool ok = true;
        public string name;
        public int xmlVersion;
        public int partCount;
        public int bodyCount;
        public int connectionCount;
        public int rootPartId;
        public int activeCommandPodId;
        public double price;
        public Vec3 localCenterOfMass;
        public List<BodyInfo> bodies = new();
        public List<ConnectionInfo> connections = new();
        public List<PartInfo> parts = new();
    }

    public class PartInfo
    {
        public int id;
        public string name;
        public string tag;
        public string partType;
        public bool hasVizzy;
        public bool enabled;
        public bool activated;
        public bool isRootPart;
        public int activationStage;
        public int activationGroup;
        public bool activationStageOverride;
        public int commandPodId;
        public string bodyId;
        public string symmetryId;
        public string groupId;
        public double mass;
        public double dryMass;
        public double wetMass;
        public double price;
        public Vec3 position;
        public Vec3 rotation;
        public List<int> connectedPartIds = new();
        public List<ModifierInfo> modifiers = new();
    }

    public class BodyInfo
    {
        public string id;
        public double mass;
        public Vec3 centerOfMass;
        public Vec3 position;
        public Vec3 rotation;
        public List<int> partIds = new();
    }

    public class ConnectionInfo
    {
        public int partA;
        public int partB;
        public string symmetryId;
        public string type;
    }

    public class ModifierInfo
    {
        public string typeName;
        public string name;
        public string typeId;
        public string id;
        public string inputId;
        public bool inspectorEnabled;
        public bool partPropertiesEnabled;
        public double mass;
        public double dryMass;
        public double wetMass;
        public double price;
        public double scale;
        public List<int> activationGroups = new();
    }

    public class VizzyResponse
    {
        public int partId;
        public string partName;
        public string xml;
        public bool ok;
        public string error;
    }

    public class SetVizzyRequest
    {
        public string xml;
    }

    public class StagesResponse
    {
        public bool ok = true;
        public string error;
        public int currentStage;
        public int numStages;
        public List<string> activationGroupNames = new();
        public List<bool> activationGroupStates = new();
        public List<StageInfo> stages = new();
    }

    public class StageInfo
    {
        public int stage;
        public bool isCurrent;
        public int partCount;
        public List<int> partIds = new();
        public List<string> partNames = new();
    }

    public class BridgeSnapshotResponse
    {
        public bool ok = true;
        public string modVersion = VizzyCodeMod.ModVersion;
        public string scene;
        public string craftName;
        public long capturedUnixMs;
        public CraftInfoResponse craft;
        public StagesResponse stages;
        public TelemetryResponse telemetry;
        public List<string> notes = new();
    }

    public class TelemetryResponse
    {
        public bool ok = true;
        public string scene;
        public string quality;
        public bool activeFullFlightData;
        public bool trackedFallback;
        public bool hasCraftScript;
        public bool hasCraftNode;
        public bool hasCommandPod;
        public bool physicsEnabled;
        public string craftName;
        public int craftNodeId;
        public string parentName;
        public double timeReal;
        public double timeDelta;
        public double timeMultiplier;
        public double altitudeASL;
        public double altitudeAGL;
        public double altitudeTerrain;
        public double velocityMagnitude;
        public double surfaceVelocityMagnitude;
        public double verticalSurfaceVelocity;
        public double lateralSurfaceVelocity;
        public double accelerationMagnitude;
        public double machNumber;
        public double angleOfAttack;
        public double pitch;
        public double heading;
        public double bankAngle;
        public double sideSlip;
        public double gravityMagnitude;
        public double currentMassKg;
        public double fuelMass;
        public double remainingFuelInStage;
        public double remainingBattery;
        public double remainingMonopropellant;
        public double currentEngineThrustN;
        public double maxActiveEngineThrustN;
        public double currentReactionControlNozzleThrust;
        public bool grounded;
        public bool inWater;
        public bool supportsWarpBurn;
        public double solarRadiationIntensity;
        public double parentPlanetOcclusion;
        public Vec3 position;
        public Vec3 velocity;
        public Vec3 surfaceVelocity;
        public Vec3 acceleration;
        public Vec3 gravity;
        public Vec3 craftForward;
        public Vec3 craftUp;
        public Vec3 craftRight;
        public OrbitInfo orbit;
        public PerformanceInfo performance;
        public TargetInfo navTarget;
    }

    public class OrbitInfo
    {
        public string parent;
        public double apoapsisAltitude;
        public double periapsisAltitude;
        public double apoapsisTime;
        public double periapsisTime;
        public double eccentricity;
        public double inclination;
        public double period;
        public double semiMajorAxis;
        public double semiMinorAxis;
        public double trueAnomaly;
        public double meanAnomaly;
        public double meanMotion;
        public double rightAscension;
        public double periapsisArgument;
        public double burnNodeDeltaV;
        public bool hasBurnNodePoint;
    }

    public class PerformanceInfo
    {
        public double currentIsp;
        public double deltaVStage;
        public double fuelAllStagesPercentage;
        public double remainingBurnTime;
        public double thrustToWeightRatio;
    }

    public class TargetInfo
    {
        public bool exists;
        public string type;
        public string name;
        public string bodyName;
        public string rawType;
        public Vec3 position;
    }

    public class Vec3
    {
        public double x;
        public double y;
        public double z;
    }

    public class ErrorResponse
    {
        public bool ok = false;
        public string error;
    }

    // ── Planet endpoints ──────────────────────────────────────────────────────

    public class PlanetsResponse
    {
        public bool ok = true;
        public List<PlanetSummary> planets = new();
    }

    public class PlanetSummary
    {
        public string name;
        public string parent;
        public double radius;
        public double mass;
        public double surfaceGravity;
        public double escapeVelocity;
        public bool hasAtmosphere;
        public bool hasWater;
        public int childCount;
    }

    public class PlanetDetailResponse
    {
        public bool ok = true;
        public PlanetDetailInfo planet;
    }

    public class PlanetDetailInfo
    {
        public string name;
        public string parent;
        public double radius;
        public double mass;
        public double surfaceGravity;
        public double escapeVelocity;
        public double angularVelocity;
        public double impactRadius;
        public bool hasWater;
        public bool hasTerrainPhysics;
        public double maxTerrainElevation;
        public List<string> children = new();
        public PlanetAtmosphereInfo atmosphere;
        public PlanetCurrentOrbitInfo currentOrbit;
    }

    public class PlanetAtmosphereInfo
    {
        public bool hasAtmosphere;
        public double height;
        public double crushAltitude;
        public double scaleHeight;
        public double surfaceAirDensity;
        public double meanSurfaceTemperature;
        public double meanSurfaceTemperatureDay;
        public double meanSurfaceTemperatureNight;
        public double meanMassPerMolecule;
        public double meanGamma;
    }

    public class PlanetCurrentOrbitInfo
    {
        public double apoapsis;
        public double periapsis;
        public double eccentricity;
        public double inclination;
        public double period;
        public double semiMajorAxis;
        public double trueAnomaly;
        public double rightAscension;
        public double periapsisArgument;
        public double sphereOfInfluence;
        public Vec3 solarPosition;
        public Vec3 solarVelocity;
    }

    // ── Universe time ─────────────────────────────────────────────────────────

    public class UniverseTimeResponse
    {
        public bool ok = true;
        public double universalTime;
        public double timeReal;
        public double timeDelta;
        public double timeMultiplier;
        public double timeSinceLaunch;
    }

    // ── All craft nodes ───────────────────────────────────────────────────────

    public class AllCraftResponse
    {
        public bool ok = true;
        public List<CraftNodeSummary> crafts = new();
    }

    public class CraftNodeSummary
    {
        public int nodeId;
        public string name;
        public bool isPlayer;
        public double altitude;
        public double altitudeAgl;
        public string parentPlanet;
        public int partCount;
        public bool hasCommandPod;
        public double latDeg;
        public double lonDeg;
        public Vec3 surfaceVelocity;
    }

    // ── Biome ─────────────────────────────────────────────────────────────────

    public class BiomeResponse
    {
        public bool ok = true;
        public string biome;
        public string planet;
        public double latDeg;
        public double lonDeg;
    }

    // ── Full telemetry ────────────────────────────────────────────────────────

    public class TelemetryFullResponse
    {
        // ── copied core fields from TelemetryResponse ──
        public bool ok = true;
        public string scene;
        public string quality;
        public bool activeFullFlightData;
        public bool hasCraftScript;
        public bool hasCraftNode;
        public bool hasCommandPod;
        public bool physicsEnabled;
        public string craftName;
        public int craftNodeId;
        public string parentName;
        public double timeReal;
        public double timeDelta;
        public double timeMultiplier;
        public double altitudeASL;
        public double altitudeAGL;
        public double altitudeTerrain;
        public double velocityMagnitude;
        public double surfaceVelocityMagnitude;
        public double verticalSurfaceVelocity;
        public double lateralSurfaceVelocity;
        public double accelerationMagnitude;
        public double machNumber;
        public double angleOfAttack;
        public double pitch;
        public double heading;
        public double bankAngle;
        public double sideSlip;
        public double gravityMagnitude;
        public double currentMassKg;
        public double fuelMass;
        public double remainingFuelInStage;
        public double remainingBattery;
        public double remainingMonopropellant;
        public double currentEngineThrustN;
        public double maxActiveEngineThrustN;
        public double currentReactionControlNozzleThrust;
        public bool grounded;
        public bool inWater;
        public bool supportsWarpBurn;
        public double solarRadiationIntensity;
        public double parentPlanetOcclusion;
        public Vec3 position;
        public Vec3 velocity;
        public Vec3 surfaceVelocity;
        public Vec3 acceleration;
        public Vec3 gravity;
        public Vec3 craftForward;
        public Vec3 craftUp;
        public Vec3 craftRight;
        public OrbitInfo orbit;
        public PerformanceInfo performance;
        public TargetInfo navTarget;

        // ── extended fields ──
        public double angularVelocityMagnitude;
        public Vec3 angularVelocity;
        public double dragAccelerationMagnitude;
        public Vec3 north;
        public Vec3 east;
        public Vec3 solarRadiationDirection;
        public Vec3 positionNormalized;
        public double latDeg;
        public double lonDeg;
        public double universalTime;
        public AtmosphereSampleInfo atmosphereSample;
    }

    public class AtmosphereSampleInfo
    {
        public double density;
        public double pressure;
        public double temperature;
        public double speedOfSound;
    }
}
