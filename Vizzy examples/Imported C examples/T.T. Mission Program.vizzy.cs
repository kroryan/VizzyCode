using System;
using System.Linq;
using System.Collections.Generic;
using REWJUNO;
using REWVIZZY;

// ── Program: T.T. Mission Program ──────────────────────────────────
Vz.Init("T.T. Mission Program");

// ── Variables ────────────────────────────────────────
// var BurnVector = 0;
// var Inclination = 0;
// var Apoapsis = 0;
// var ArgumentofPeriapsis = 0;
// var Periapsis = 0;
// var RightAscension = 0;
// var SemiMajorAxis = 0;
// var Position = 0;
// var Velocity = 0;
// var Mu = 0;
// var AngularMomentum = 0;
// var Eccentricity = 0;
// var Node = 0;
// var Yaxis = 0;
// var Period = 0;
// var Xaxis = 0;
// var TrueAnomaly = 0;
// var EccentricAnomaly = 0;
// var MeanAnomaly = 0;
// var MeanAngularMotion = 0;
// var BurnTimer = 0;
// var TimetoTA = 0;
// var Kepler = 0;
// var Radius = 0;
// var Time = 0;
// var SemiLatusRectum = 0;
// var HyperbolicAnomaly = 0;
// var FlightPathAngle = 0;
// var TargetSOI = 0;
// var CraftPosition = 0;
// var CraftVelocity = 0;
// var PlanetNames = [];   // list;
// var Positions = [];   // list;
// var Velocities = [];   // list;
// var Parents = [];   // list;
// var PlanetTimes = [];   // list;
// var PlanetPosition = 0;
// var PlanetVelocity = 0;
// var PlanetSOI = [];   // list;
// var SOI = 0;
// var SOIExitTrueAnomaly = 0;
// var TargetParameters = [];   // list;
// var Δv = 0;
// var CraftState = [];   // list;
// var I = 0;
// var HyperbolicAngle = 0;
// var BurnNode = 0;
// var TransferTime = 0;
// var BurnPosition = 0;
// var BurnVelocity = 0;
// var Prograde = 0;
// var Radial = 0;
// var Normal = 0;
// var LandingCoordinates = 0;
// var TA = 0;
// var Vector = 0;
// var Latitude = 0;
// var LaunchPlaneNormal = 0;
// var Launch = [];   // list;
// var Orbit = 0;
// var EccentricityVector = 0;
// var Targets = [];   // list;

using (new OnReceiveMessage("Timewarp"))
{
    Vz.WaitSeconds(1);
    Vz.SetTimeModeAttr("TimeWarp1");
    using (new If(((data - Vz.Time.TotalTime()) > 25)))
    {
        Vz.WaitSeconds(5);
        Vz.SetTimeModeAttr("TimeWarp2");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 100)))
    {
        Vz.WaitSeconds(12.5);
        Vz.SetTimeModeAttr("TimeWarp3");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 500)))
    {
        Vz.WaitSeconds(50);
        Vz.SetTimeModeAttr("TimeWarp4");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 2500)))
    {
        Vz.WaitSeconds(250);
        Vz.SetTimeModeAttr("TimeWarp5");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 10000)))
    {
        Vz.WaitSeconds(1250);
        Vz.SetTimeModeAttr("TimeWarp6");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 50000)))
    {
        Vz.WaitSeconds(5000);
        Vz.SetTimeModeAttr("TimeWarp7");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 250000)))
    {
        Vz.WaitSeconds(25000);
        Vz.SetTimeModeAttr("TimeWarp8");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 1000000)))
    {
        Vz.WaitSeconds(125000);
        Vz.SetTimeModeAttr("TimeWarp9");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 5000000)))
    {
        Vz.WaitSeconds(500000);
        Vz.SetTimeModeAttr("TimeWarp10");
        using (new WaitUntil(((data - Vz.Time.TotalTime()) < 500000))) { }
        Vz.SetTimeModeAttr("TimeWarp9");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 125000)))
    {
        using (new WaitUntil(((data - Vz.Time.TotalTime()) < 125000))) { }
        Vz.SetTimeModeAttr("TimeWarp8");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 25000)))
    {
        using (new WaitUntil(((data - Vz.Time.TotalTime()) < 25000))) { }
        Vz.SetTimeModeAttr("TimeWarp7");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 10000)))
    {
        using (new WaitUntil(((data - Vz.Time.TotalTime()) < 10000))) { }
        Vz.SetTimeModeAttr("TimeWarp6");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 2500)))
    {
        using (new WaitUntil(((data - Vz.Time.TotalTime()) < 2500))) { }
        Vz.SetTimeModeAttr("TimeWarp5");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 500)))
    {
        using (new WaitUntil(((data - Vz.Time.TotalTime()) < 500))) { }
        Vz.SetTimeModeAttr("TimeWarp4");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 100)))
    {
        using (new WaitUntil(((data - Vz.Time.TotalTime()) < 100))) { }
        Vz.SetTimeModeAttr("TimeWarp3");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 25)))
    {
        using (new WaitUntil(((data - Vz.Time.TotalTime()) < 25))) { }
        Vz.SetTimeModeAttr("TimeWarp2");
    }
    using (new If(((data - Vz.Time.TotalTime()) > 10)))
    {
        using (new WaitUntil(((data - Vz.Time.TotalTime()) < 10))) { }
        Vz.SetTimeModeAttr("TimeWarp1");
    }
    using (new WaitUntil(((data - Vz.Time.TotalTime()) < 1))) { }
    Vz.SetTimeModeAttr("Normal");
}

// ── Custom Instructions ──────────────────────────────
var Orbital_State_Vectors = Vz.DeclareCustomInstruction("Orbital State Vectors", "TrueAnomaly").SetInstructions((TrueAnomaly) =>
{
    TrueAnomaly = TrueAnomaly;
    Radius = Vz.ExactEval("(pow(magnitude(v:AngularMomentum),2)/Mu)/(1+(Eccentricity*cos(TrueAnomaly)))");
    FlightPathAngle = Vz.ExactEval("atan((Eccentricity*sin(TrueAnomaly))/(1+(Eccentricity*cos(TrueAnomaly))))");
    Position = Rotate_Y_Axis_Vector(Rotate_X_Axis_Vector(Rotate_Y_Axis_Vector((Radius * Vz.Vec(Vz.ExactEval("cos(TrueAnomaly)"), 0, Vz.ExactEval("sin(TrueAnomaly)"))), ArgumentofPeriapsis), Inclination), RightAscension);
    Velocity = Rotate_Y_Axis_Vector(Rotate_X_Axis_Vector(Rotate_Y_Axis_Vector((Orbital_Speed(Radius, SemiMajorAxis) * Vz.Vec(Vz.ExactEval("cos(TrueAnomaly+((pi)/2)-FlightPathAngle)"), 0, Vz.ExactEval("sin(TrueAnomaly+((pi)/2)-FlightPathAngle)"))), ArgumentofPeriapsis), Inclination), RightAscension);
});

var Planet = Vz.DeclareCustomInstruction("Planet", "Name", "Time").SetInstructions((Name, Time) =>
{
    Orbital_Elements_Parent_Body(Vz.ListGet(Parents, Vz.ListIndex(PlanetNames, Name)), Vz.ListGet(Positions, Vz.ListIndex(PlanetNames, Name)), Vz.ListGet(Velocities, Vz.ListIndex(PlanetNames, Name)));
    True_Anomaly_After_Time(((Vz.Time.TotalTime() + Time) - Vz.ListGet(PlanetTimes, Vz.ListIndex(PlanetNames, Name))));
    Orbital_State_Vectors(TrueAnomaly);
    PlanetPosition = Position;
    PlanetVelocity = Velocity;
});

var Orbital_Elements_Parent_Body = Vz.DeclareCustomInstruction("Orbital Elements Parent Body", "Planet", "Position", "Velocity").SetInstructions((Planet, Position, Velocity) =>
{
    Position = Position;
    Velocity = Velocity;
    Mu = (6.67384E-11 * Vz.Planet(Planet).Mass());
    SemiLatusRectum = Vz.ExactEval("pow(magnitude(cross(v:Position,v:Velocity)),2)/Mu");
    SemiMajorAxis = Vz.ExactEval("1/((2/(magnitude(v:Position)))-(pow(magnitude(v:Velocity),2)/Mu))");
    AngularMomentum = Vz.ExactEval("cross(v:Position,v:Velocity)");
    EccentricityVector = Vz.ExactEval("(cross(v:Velocity,v:AngularMomentum)/Mu)-normalize(v:Position)");
    Eccentricity = Vz.ExactEval("magnitude(v:EccentricityVector)");
    Node = Vz.ExactEval("cross(v:Yaxis,v:AngularMomentum)");
    using (new If((Eccentricity < 0.0001)))
    {
        EccentricityVector = Node;
    }
    MeanAngularMotion = Vz.ExactEval("sqrt(pow(abs(SemiMajorAxis),3)/Mu)");
    Period = Vz.ExactEval("(pi)*2*MeanAngularMotion");
    Periapsis = Vz.ExactEval("(1-Eccentricity)*SemiMajorAxis");
    Apoapsis = Vz.ExactEval("(1+Eccentricity)*SemiMajorAxis");
    ArgumentofPeriapsis = Vz.ExactEval("signedAngle(v:Node,v:EccentricityVector,v:AngularMomentum)*((pi)/180)");
    Inclination = Vz.ExactEval("acos(0-y(v:AngularMomentum)/magnitude(v:AngularMomentum))");
    RightAscension = Vz.ExactEval("signedAngle(v:Node,v:Xaxis,v:Yaxis)*((pi)/180)");
    TrueAnomaly = Signed_Angle(Position, EccentricityVector, AngularMomentum);
    SOI = Vz.ListGet(PlanetSOI, Vz.ListIndex(PlanetNames, Planet));
    SOIExitTrueAnomaly = ((((Apoapsis > SOI) || (Eccentricity > 1)) && (Vz.LengthOf(Vz.Planet(Planet).Parent()) > 0)) ? Vz.Acos(((SemiLatusRectum - SOI) / (Eccentricity * SOI))) : "");
    using (new If((Eccentricity < 1)))
    {
        EccentricAnomaly = Vz.ExactEval("2*atan(sqrt((1-Eccentricity)/(1+Eccentricity))*tan(TrueAnomaly/2))");
        MeanAnomaly = Vz.ExactEval("EccentricAnomaly-(Eccentricity*sin(EccentricAnomaly))<0?((EccentricAnomaly-(Eccentricity*sin(EccentricAnomaly)))+(2*(pi))):(EccentricAnomaly-(Eccentricity*sin(EccentricAnomaly)))");
    }
    using (new Else())
    {
        HyperbolicAnomaly = (2 * atanh(Vz.ExactEval("sqrt((Eccentricity-1)/(Eccentricity+1))*tan(TrueAnomaly/2)")));
        MeanAnomaly = (((Eccentricity * sinh(HyperbolicAnomaly)) - HyperbolicAnomaly) * MeanAngularMotion);
    }
});

var Time_to_TrueAnomaly = Vz.DeclareCustomInstruction("Time to TrueAnomaly", "TrueAnomaly").SetInstructions((TrueAnomaly) =>
{
    TrueAnomaly = TrueAnomaly;
    using (new If((Eccentricity < 1)))
    {
        TimetoTA = Vz.ExactEval("MeanAnomaly*MeanAngularMotion");
        EccentricAnomaly = Vz.ExactEval("2*atan(sqrt((1-Eccentricity)/(1+Eccentricity))*tan(TrueAnomaly/2))");
        MeanAnomaly = Vz.ExactEval("EccentricAnomaly-(Eccentricity*sin(EccentricAnomaly))<0?((EccentricAnomaly-(Eccentricity*sin(EccentricAnomaly)))+(2*(pi)))*MeanAngularMotion:(EccentricAnomaly-(Eccentricity*sin(EccentricAnomaly)))*MeanAngularMotion");
        TimetoTA = Vz.ExactEval("MeanAnomaly-TimetoTA<0?MeanAnomaly-TimetoTA+Period:MeanAnomaly-TimetoTA");
    }
    using (new Else())
    {
        TimetoTA = MeanAnomaly;
        HyperbolicAnomaly = (2 * atanh(Vz.ExactEval("sqrt((Eccentricity-1)/(Eccentricity+1))*tan(TrueAnomaly/2)")));
        MeanAnomaly = (((Eccentricity * sinh(HyperbolicAnomaly)) - HyperbolicAnomaly) * MeanAngularMotion);
        TimetoTA = Vz.ExactEval("MeanAnomaly-TimetoTA");
    }
});

var PlanetLists = Vz.DeclareCustomInstruction("PlanetLists").SetInstructions(() =>
{
    PlanetNames = Vz.Planet("Juno").ChildPlanets();
    using (new For("i").From(1).To(Vz.ListLength(PlanetNames)).By(1))
    {
        using (new For("k").From(1).To(Vz.ListLength(Vz.Planet(Vz.ListGet(PlanetNames, i)).ChildPlanets())).By(1))
        {
            Vz.ListAdd(PlanetNames, Vz.ListGet(Vz.Planet(Vz.ListGet(PlanetNames, i)).ChildPlanets(), k));
        }
    }
    PlanetSOI = Vz.CreateListRaw("30000000,80000000,0,500000,0,0,0,5500000,0,1500000,60000,500000,9000000,15000000,3500000,0,250000,9000000,0");
    using (new For("i").From(1).To(Vz.ListLength(PlanetNames)).By(1))
    {
        Vz.TargetNode(Vz.ListGet(PlanetNames, i));
        Vz.WaitSeconds(0);
        Vz.ListAdd(Positions, Vz.Craft.Target.Position());
        Vz.ListAdd(Velocities, Vz.Craft.Target.Velocity());
        Vz.ListAdd(PlanetTimes, Vz.Time.TotalTime());
        Vz.ListAdd(Parents, Vz.Planet(Vz.ListGet(PlanetNames, i)).Parent());
    }
    Vz.WaitSeconds(0);
    Vz.TargetNode("");
    using (new For("i").From(1).To(Vz.ListLength(PlanetNames)).By(1))
    {
        using (new If((Vz.ListGet(PlanetSOI, i) == 0)))
        {
            Position = Vz.ListGet(Positions, i);
            Velocity = Vz.ListGet(Velocities, i);
            Mu = (6.67384E-11 * Vz.Planet(Vz.ListGet(Parents, i)).Mass());
            SemiMajorAxis = Vz.ExactEval("1/((2/(magnitude(v:Position)))-(pow(magnitude(v:Velocity),2)/Mu))");
            Vz.ListSet(PlanetSOI, (SemiMajorAxis * ((Vz.Planet(Vz.ListGet(PlanetNames, i)).Mass() / Vz.Planet(Vz.ListGet(Parents, i)).Mass()) ^ 0.4)), i);
        }
    }
    Vz.WaitSeconds(0);
});

var Craft = Vz.DeclareCustomInstruction("Craft", "Position", "Velocity", "ParentBody", "Time").SetInstructions((Position, Velocity, ParentBody, Time) =>
{
    Orbital_Elements_Parent_Body(ParentBody, Position, Velocity);
    True_Anomaly_After_Time(Time);
    Orbital_State_Vectors(TrueAnomaly);
    CraftPosition = Position;
    CraftVelocity = Velocity;
});

var True_Anomaly_After_Time = Vz.DeclareCustomInstruction("True Anomaly After Time", "Time").SetInstructions((Time) =>
{
    using (new If((Eccentricity < 1)))
    {
        MeanAnomaly = (((Vz.ExactEval("MeanAnomaly*MeanAngularMotion") + Time) % Period) / MeanAngularMotion);
        EccentricAnomaly = Vz.ExactEval("pi");
        using (new Repeat(20))
        {
            Kepler = Vz.ExactEval("(EccentricAnomaly-(Eccentricity*sin(EccentricAnomaly)))-MeanAnomaly");
            EccentricAnomaly = Vz.ExactEval("EccentricAnomaly-(Kepler/(1-(Eccentricity*cos(EccentricAnomaly))))");
            using (new If((Vz.Abs(Kepler) <= 0.00000000000001)))
            {
                Vz.Break();
            }
        }
        TrueAnomaly = Vz.ExactEval("2*atan(sqrt((1+Eccentricity)/(1-Eccentricity))*(tan(EccentricAnomaly/2)))");
    }
    using (new Else())
    {
        MeanAnomaly = ((MeanAnomaly + Time) / MeanAngularMotion);
        HyperbolicAnomaly = Vz.ExactEval("pi");
        using (new Repeat(20))
        {
            Kepler = ((HyperbolicAnomaly - (Eccentricity * sinh(HyperbolicAnomaly))) + MeanAnomaly);
            HyperbolicAnomaly = (HyperbolicAnomaly - (Kepler / (1 - (Eccentricity * cosh(HyperbolicAnomaly)))));
            using (new If((Vz.Abs(Kepler) <= 0.00000000000001)))
            {
                Vz.Break();
            }
        }
        TrueAnomaly = (2 * Vz.Atan((Vz.ExactEval("sqrt((Eccentricity+1)/(Eccentricity-1))") * tanh(Vz.ExactEval("HyperbolicAnomaly/2")))));
    }
});

using (new OnStart())
{
    Xaxis = Vz.Vec(1, 0, 0);
    Yaxis = Vz.Vec(0, 1, 0);
    Vz.ActivateStage();
    Vz.WaitSeconds(1);
    Targets = Vz.CreateListRaw("DSC Launch Pad,Droo Space Center,DSC Landing Pad A,DSC Landing Pad B,DSC Launch Pad Large,Drone Ship,Droo Desert Base");
    PlanetLists();
    Planet("T.T.", 0);
    Orbital_Elements_Parent_Body(Vz.ListGet(Parents, Vz.ListIndex(PlanetNames, "T.T.")), PlanetPosition, PlanetVelocity);
    Vz.ListAdd(TargetParameters, Inclination);
    Vz.ListAdd(TargetParameters, RightAscension);
    Time_to_TrueAnomaly(Signed_Angle(Node, EccentricityVector, AngularMomentum));
    Time = TimetoTA;
    Orbital_Elements_Parent_Body(Vz.ListGet(Parents, Vz.ListIndex(PlanetNames, "T.T.")), PlanetPosition, PlanetVelocity);
    Time_to_TrueAnomaly(Signed_Angle((0 - Node), EccentricityVector, AngularMomentum));
    Launch_Inclination(Vz.ListGet(TargetParameters, 1), Vz.ListGet(TargetParameters, 2), 100000, 50400, Vz.Min(Time, TimetoTA));
    Vz.ActivateStage();
    Vz.WaitSeconds(2);
    Vz.ActivateStage();
    Vz.WaitSeconds(2);
    Inclination(Vz.ListGet(TargetParameters, 1), Vz.ListGet(TargetParameters, 2));
    Planet("T.T.", 0);
    TA = 0;
    using (new Repeat(10))
    {
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
        Orbital_State_Vectors(TA);
        Time_to_TrueAnomaly(TA);
        Prograde = Orbital_Speed(Vz.Length(Position), ((Vz.Length(Position) + Vz.Length(PlanetPosition)) / 2));
        Time = TimetoTA;
        Radial = (0 - FlightPathAngle);
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Position, Rotate_Vector((Vz.Norm(Velocity) * Prograde), Vz.Cross(Position, Velocity), Radial));
        Time_to_TrueAnomaly(Vz.ExactEval("pi"));
        Orbital_State_Vectors(Vz.ExactEval("pi"));
        CraftPosition = Position;
        Time += TimetoTA;
        Planet("T.T.", Time);
        TA += Signed_Angle(PlanetPosition, CraftPosition, AngularMomentum);
    }
    Maneuver_Node("Transfer", 0, TA, Prograde, Radial, 0);
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    Time_to_TrueAnomaly(Vz.ExactEval("pi"));
    Time = TimetoTA;
    I = (Time / 2);
    using (new While((I > 0.01)))
    {
        Craft(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), Vz.Craft.Orbit.Planet(), Time);
        Planet("T.T.", Time);
        Time += ((Vz.Distance(CraftPosition, PlanetPosition) < Vz.ListGet(PlanetSOI, Vz.ListIndex(PlanetNames, "T.T."))) ? (0 - I) : I);
        I = (I / 2);
    }
    Vz.Log(Vz.Join("T.T. encounter: ", Vz.StringOp("friendly", Time, ""), ""));
    Time += Vz.Time.TotalTime();
    Vz.Broadcast(BroadCastType.Local, "Timewarp", Time);
    using (new While(((Time - Vz.Time.TotalTime()) > 0)))
    {
        Vz.Display(Vz.Join("T.T. encounter:<br>", Clock((Time - Vz.Time.TotalTime())), ""), 7);
        Vz.WaitSeconds(0);
    }
    using (new WaitUntil(Match_String(Vz.Craft.Orbit.Planet(), "T.T."))) { }
    Vz.Display("", 7);
    Vz.WaitSeconds(2);
    Vz.ActivateStage();
    Vz.WaitSeconds(2);
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    TA = (TrueAnomaly + 0.001);
    Orbital_State_Vectors(TA);
    CraftPosition = Position;
    CraftVelocity = Velocity;
    Radial = 0;
    I = ((Vz.ExactEval("(pi)/2") + FlightPathAngle) / 2);
    using (new While((I > 0.00000000000001)))
    {
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), CraftPosition, Rotate_Vector(CraftVelocity, Vz.Cross(CraftPosition, CraftVelocity), Radial));
        Radial += ((Periapsis > (Vz.Planet("T.T.").Radius() + 20000)) ? (0 - I) : I);
        I = (I / 2);
    }
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), CraftPosition, CraftVelocity);
    Normal = Signed_Angle(Flatten(EccentricityVector, CraftPosition), Vz.Cross(Yaxis, CraftPosition), CraftPosition);
    Maneuver_Node("Trajectory Correction", 0, TA, Vz.Length(CraftVelocity), Radial, Normal);
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    Orbital_State_Vectors(0);
    Maneuver_Node("Capture", 0, 0, Orbital_Speed(Vz.Length(Position), Vz.Length(Position)), 0, ((Velocity.y > 0) ? Vz.ExactEval("(pi)-Inclination") : (0 - Vz.ExactEval("(pi)-Inclination"))));
    Circularization_at_Altitude(20000);
    LandingCoordinates = Vz.Vec(5.062555314, 111.830926304, 3);
    using (new If((Vz.Dot(Vz.ToPosition(LandingCoordinates), Vz.Craft.Nav.Position()) > 0)))
    {
        Inclination(Vz.ExactEval("pi"), 0);
    }
    Deorbit_Coordinates(LandingCoordinates, Vz.Deg2Rad(-45), -10800);
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    TA = (0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(LandingCoordinates))));
    Orbital_State_Vectors(TA);
    Time_to_TrueAnomaly(TA);
    Time = ((TimetoTA - Burn_Time(Vz.Length(Velocity))) + Vz.Time.TotalTime());
    Vz.Broadcast(BroadCastType.Local, "Timewarp", Time);
    using (new While(((Time - Vz.Time.TotalTime()) > 0)))
    {
        using (new If(((Time - Vz.Time.TotalTime()) < 1)))
        {
            Vz.SetTimeModeAttr("Normal");
        }
        using (new ElseIf(((Time - Vz.Time.TotalTime()) < 60)))
        {
            Vz.SetActivationGroup(7, true);
            Vz.SetTimeModeAttr("FastForward");
        }
        Vz.LockNavSphere(LockNavSphereIndicatorType.Vector, (0 - Vz.Craft.Velocity.Surface()));
        Vz.Display(Vz.Join("Landing Burn:<br>", Clock((Time - Vz.Time.TotalTime())), ""), 7);
        Vz.WaitSeconds(0);
    }
    using (new While((!Vz.Craft.Grounded())))
    {
        PlanetPosition = Vz.ToPosition(LandingCoordinates);
        Vector = (((Vz.Norm((PlanetPosition - Vz.Craft.Nav.Position())) * ((10 * (1.008 ^ Vz.Length((PlanetPosition - Vz.Craft.Nav.Position())))) - 10)) - Vz.Craft.Velocity.Gravity()) - Vz.Craft.Velocity.Surface());
        Vz.LockNavSphere(LockNavSphereIndicatorType.Vector, Vector);
        Vz.SetInput(CraftInput.Throttle, (Vz.Length(Vector) / (Vz.Craft.Performance.MaxThrust() / Vz.Craft.Performance.Mass())));
        Vz.Display(Vz.Join("Distance to Landing Coordinates: ", Vz.StringOp("friendly", Vz.Length((PlanetPosition - Vz.Craft.Nav.Position())), ""), ""), 7);
        using (new If((Vz.Craft.AltitudeAGL() < 10)))
        {
            Vz.SetActivationGroup(8, true);
        }
        Vz.WaitSeconds(0);
    }
    Vz.SetInput(CraftInput.Throttle, 0);
    Vz.Display("Touchdown!", 7);
    Vz.Log("Touchdown!");
    Vz.WaitSeconds(4);
    Vz.SetActivationGroup(7, false);
    Planet("T.T.", 0);
    Orbital_Elements_Parent_Body(Vz.ListGet(Parents, Vz.ListIndex(PlanetNames, "T.T.")), PlanetPosition, PlanetVelocity);
    Time_to_TrueAnomaly(Signed_Angle(Node, EccentricityVector, AngularMomentum));
    Time = TimetoTA;
    Orbital_Elements_Parent_Body(Vz.ListGet(Parents, Vz.ListIndex(PlanetNames, "T.T.")), PlanetPosition, PlanetVelocity);
    Time_to_TrueAnomaly(Signed_Angle((0 - Node), EccentricityVector, AngularMomentum));
    Launch_Inclination(Vz.ListGet(TargetParameters, 1), Vz.ListGet(TargetParameters, 2), 20000, -10800, Vz.Min(Time, TimetoTA));
    Inclination(Vz.ListGet(TargetParameters, 1), Vz.ListGet(TargetParameters, 2));
    Time = 0;
    Prograde = Vz.Length(Vz.Craft.Velocity.Orbital());
    I = 200;
    using (new While((I > 0.01)))
    {
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), (Vz.Norm(Vz.Craft.Velocity.Orbital()) * Prograde));
        HyperbolicAngle = ((Eccentricity > 1) ? Vz.ExactEval("(pi)-acos(-1/Eccentricity)") : 0);
        using (new Repeat(5))
        {
            Planet("T.T.", Time);
            Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
            TA = (Signed_Angle(PlanetVelocity, EccentricityVector, AngularMomentum) + HyperbolicAngle);
            Time_to_TrueAnomaly(TA);
            Time = TimetoTA;
        }
        Orbital_State_Vectors(TA);
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Position, (Vz.Norm(Velocity) * Prograde));
        using (new If(((Apoapsis < SOI) && (Apoapsis > 0))))
        {
            Prograde += I;
        }
        using (new Else())
        {
            Time_to_TrueAnomaly(SOIExitTrueAnomaly);
            Time += TimetoTA;
            Orbital_State_Vectors(SOIExitTrueAnomaly);
            CraftPosition = Position;
            CraftVelocity = Velocity;
            Planet("T.T.", Time);
            Orbital_Elements_Parent_Body("Droo", (PlanetPosition + CraftPosition), (PlanetVelocity + CraftVelocity));
            Prograde += ((Periapsis < (Vz.Planet("Droo").Radius() + 30000)) ? (0 - I) : I);
        }
        I = (I / 2);
    }
    Maneuver_Node("Escape", 0, TA, Prograde, 0, 0);
    Vz.WaitSeconds(1);
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    Time_to_TrueAnomaly(SOIExitTrueAnomaly);
    Time = TimetoTA;
    Vz.Log(Vz.Join("Droo encounter: ", Vz.StringOp("friendly", Time, ""), ""));
    Time += Vz.Time.TotalTime();
    Vz.Broadcast(BroadCastType.Local, "Timewarp", Time);
    using (new While(((Time - Vz.Time.TotalTime()) > 0)))
    {
        Vz.Display(Vz.Join("Droo encounter:<br>", Clock((Time - Vz.Time.TotalTime())), ""), 7);
        using (new If(Match_String(Vz.Craft.Orbit.Planet(), "Droo")))
        {
            Vz.Display("", 7);
            Vz.Break();
        }
        Vz.WaitSeconds(0);
    }
    using (new WaitUntil(Match_String(Vz.Craft.Orbit.Planet(), "Droo"))) { }
    using (new While(((Time - Vz.Time.TotalTime()) > 0)))
    {
        Vz.SetTimeModeAttr("TimeWarp2");
    }
    Vz.SetTimeModeAttr("Normal");
    Vz.WaitSeconds(1);
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    TA = (TrueAnomaly + 0.0001);
    Orbital_State_Vectors(TA);
    Maneuver_Node("Correction", 0, TA, Orbital_Speed(Vz.Length(Position), ((Vz.Length(Position) + (Vz.Planet("Droo").Radius() + 70000)) / 2)), (0 - FlightPathAngle), ((Velocity.y > 0) ? Signed_Angle(Velocity, Yaxis, Position) : Signed_Angle(Velocity, (0 - Yaxis), Position)));
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    Orbital_State_Vectors(0);
    Maneuver_Node("Correction", 0, 0, Orbital_Speed(Vz.Length(Position), ((Vz.Length(Position) + (Apoapsis - (Vz.ListGet(PlanetSOI, Vz.ListIndex(PlanetNames, "T.T.")) * 1.5))) / 2)), 0, 0);
    Vz.WaitSeconds(0.5);
    I = "";
    using (new For("i").From(Vz.ListLength(Targets)).To(1).By(-1))
    {
        Vz.Log(Vz.Join(i, " ", Vz.ListGet(Targets, i), " ", ""));
    }
    using (new For("i").From(1).To(Vz.ListLength(Targets)).By(1))
    {
        I = Vz.Join(I, "<br>", i, " ", Vz.ListGet(Targets, i), "");
    }
    Vz.Display(I, 7);
    I = Vz.UserInput(Vz.Join("Enter 1 - ", Vz.ListLength(Targets), " to choose a target location", ""));
    I = Vz.Min(Vz.ListLength(Targets), Vz.Max(1, Vz.Round(I)));
    Vz.TargetNode(Vz.ListGet(Targets, I));
    Vz.WaitSeconds(0.25);
    Vz.Log(Vz.Join("Commencing Orbital Drop onto ", Vz.ListGet(Targets, I), "", " ", ""));
    LandingCoordinates = Vz.PosToLatLongAgl(Vz.Craft.Target.Position());
    Vz.SetTimeModeAttr("TimeWarp1");
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    Time_to_TrueAnomaly(Vz.ExactEval("pi"));
    Time = TimetoTA;
    Vz.ListClear(CraftState);
    Vz.ListAdd(CraftState, Period);
    Vz.ListAdd(CraftState, Time);
    Orbital_State_Vectors(Vz.ExactEval("pi"));
    CraftPosition = Position;
    CraftVelocity = Velocity;
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), CraftPosition, (Vz.Norm(CraftVelocity) * Orbital_Speed(Vz.Length(CraftPosition), ((Vz.Length(CraftPosition) + (Vz.Planet("Droo").Radius() / 2)) / 2))));
    Time_to_TrueAnomaly((0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(LandingCoordinates)))));
    Time += TimetoTA;
    Orbit = 0;
    using (new While((Vz.Dot(Vz.Norm(CraftPosition), Vz.Norm(Rotate_Y_Axis_Vector(Vz.ToPosition(LandingCoordinates), (Vz.ExactEval("2*(pi)/50400") * Time)))) < 0.8)))
    {
        Orbit += 1;
        Time += Vz.ListGet(CraftState, 1);
    }
    Prograde = (Vz.Length(CraftVelocity) / 2);
    I = (Vz.Length(CraftVelocity) / 4);
    using (new While((I > 0.001)))
    {
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), CraftPosition, (Vz.Norm(CraftVelocity) * Prograde));
        TA = (0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(LandingCoordinates))));
        Time_to_TrueAnomaly(TA);
        Orbital_State_Vectors(TA);
        using (new If((Vz.Dot(CraftPosition, Position) < 0)))
        {
            Prograde += (0 - I);
        }
        using (new Else())
        {
            Prograde += ((Vz.Length(Flatten(Position, CraftPosition)) > Vz.Length(Flatten(Rotate_Y_Axis_Vector(Vz.ToPosition(LandingCoordinates), (Vz.ExactEval("2*(pi)/50400") * (((Vz.ListGet(CraftState, 1) * Orbit) + Vz.ListGet(CraftState, 2)) + TimetoTA))), CraftPosition))) ? (0 - I) : I);
        }
        I = (I / 2);
    }
    Normal = Signed_Angle(Flatten(Position, CraftPosition), Flatten(Rotate_Y_Axis_Vector(Vz.ToPosition(LandingCoordinates), (Vz.ExactEval("2*(pi)/50400") * (((Vz.ListGet(CraftState, 1) * Orbit) + Vz.ListGet(CraftState, 2)) + TimetoTA))), CraftPosition), CraftPosition);
    Maneuver_Node("Orbital Drop", (Orbit * Vz.ListGet(CraftState, 1)), Vz.ExactEval("pi"), Prograde, 0, Normal);
    Vz.SetTimeModeAttr("TimeWarp1");
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    Time_to_TrueAnomaly((0 - True_Anomaly_at_Radius((Vz.Planet("Droo").Radius() + 58000))));
    Time = TimetoTA;
    Vz.Log(Vz.Join("Droo Reentry: ", Vz.StringOp("friendly", Time, ""), ""));
    Time += Vz.Time.TotalTime();
    Vz.Broadcast(BroadCastType.Local, "Timewarp", Time);
    using (new While(((Time - Vz.Time.TotalTime()) > 0)))
    {
        Vz.Display(Vz.Join("Reentry Time:<br>", Clock((Time - Vz.Time.TotalTime())), ""), 7);
        using (new If(((Time - Vz.Time.TotalTime()) < 60)))
        {
            Vz.SetTimeModeAttr("FastForward");
        }
        Vz.WaitSeconds(0);
    }
    Vz.ActivateStage();
    using (new WaitUntil(Vz.Craft.Atmosphere.AirDensity())) { }
    using (new While((Vz.Length(Vz.Craft.Nav.Position()) > (Vz.Length(Vz.ToPosition(LandingCoordinates)) + 100))))
    {
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
        TA = (0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(LandingCoordinates))));
        Time_to_TrueAnomaly(TA);
        Orbital_State_Vectors(TA);
        Vector = Rotate_Y_Axis_Vector(Vz.ToPosition(LandingCoordinates), (Vz.ExactEval("2*(pi)/50400") * TimetoTA));
        Vz.LockNavSphere(LockNavSphereIndicatorType.Vector, ((Vector - Position) - Vz.Project(Vz.Craft.Velocity.Surface(), Vz.Craft.Nav.Position())));
        Vz.WaitSeconds(0);
    }
    Vz.SetTargetHeading(TargetHeadingProperty.Pitch, 90);
    Vz.SetTimeModeAttr("Normal");
    Vz.ActivateStage();
    using (new WaitUntil(Vz.Craft.Grounded())) { }
    Vz.Display("Touchdown!", 7);
    Vz.Log("Touchdown!");
}

var Inclination = Vz.DeclareCustomInstruction("Inclination", "Inclination", "RightAscension").SetInstructions((Inclination, RightAscension) =>
{
    Inclination = Rotate_Y_Axis_Vector(Rotate_X_Axis_Vector(Vz.Vec(0, -1, 0), Inclination), RightAscension);
    Vz.ListClear(CraftState);
    using (new For("i").From(1).To(-1).By(-2))
    {
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
        Vz.ListAdd(CraftState, Signed_Angle((i * Vz.Cross(AngularMomentum, Inclination)), EccentricityVector, AngularMomentum));
        Time_to_TrueAnomaly(Vz.ListGet(CraftState, ((i > 0) ? 1 : 3)));
        Vz.ListAdd(CraftState, TimetoTA);
    }
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    TA = ((Vz.ListGet(CraftState, 2) < Vz.ListGet(CraftState, 4)) ? Vz.ListGet(CraftState, 1) : Vz.ListGet(CraftState, 3));
    Orbital_State_Vectors(TA);
    Normal = Signed_Angle(AngularMomentum, Inclination, (((Vz.ListGet(CraftState, 2) < Vz.ListGet(CraftState, 4)) ? 1 : -1) * Vz.Cross(AngularMomentum, Inclination)));
    Maneuver_Node("Inclination", 0, TA, Vz.Length(Velocity), 0, Normal);
});

var Circularization_at_Altitude = Vz.DeclareCustomInstruction("Circularization at Altitude", "Altitude").SetInstructions((Altitude) =>
{
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    TA = True_Anomaly_at_Radius((Vz.Planet(Vz.Craft.Orbit.Planet()).Radius() + Altitude));
    Orbital_State_Vectors(TA);
    Maneuver_Node("Circularization", 0, TA, Orbital_Speed(Vz.Length(Position), Vz.Length(Position)), (0 - FlightPathAngle), 0);
});

var Deorbit_Coordinates = Vz.DeclareCustomInstruction("Deorbit Coordinates", "Coordinates", "VelocityAngle", "PlanetRotationRate").SetInstructions((Coordinates, VelocityAngle, PlanetRotationRate) =>
{
    Prograde = (Vz.Length(Vz.Craft.Velocity.Orbital()) / 2);
    I = (Vz.Length(Vz.Craft.Velocity.Orbital()) / 4);
    using (new While((I > 0.001)))
    {
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), (Vz.Norm(Vz.Craft.Velocity.Orbital()) * Prograde));
        Orbital_State_Vectors((0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(Coordinates)))));
        Prograde += ((FlightPathAngle > VelocityAngle) ? (0 - I) : I);
        I = (I / 2);
    }
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), (Vz.Norm(Vz.Craft.Velocity.Orbital()) * Prograde));
    Time_to_TrueAnomaly((0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(Coordinates)))));
    Orbital_State_Vectors((0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(Coordinates)))));
    Time = ((Signed_Angle(Rotate_Y_Axis_Vector(Vz.ToPosition(Coordinates), ((Vz.ExactEval("2*(pi)") / PlanetRotationRate) * TimetoTA)), Position, AngularMomentum) + Vz.ExactEval("2*(pi)")) % Vz.ExactEval("2*(pi)"));
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    Time = (Time / Vz.Abs(((Vz.ExactEval("2*(pi)") / ((Inclination > Vz.ExactEval("(pi)/2")) ? (0 - Period) : Period)) - (Vz.ExactEval("2*(pi)") / PlanetRotationRate))));
    Orbit = (Period * Vz.Floor((Time / Period)));
    True_Anomaly_After_Time(Time);
    TA = TrueAnomaly;
    Orbital_State_Vectors(TA);
    CraftPosition = Position;
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), CraftPosition, (Vz.Norm(Velocity) * Prograde));
    Time_to_TrueAnomaly((0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(Coordinates)))));
    Time += TimetoTA;
    Orbital_State_Vectors((0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(Coordinates)))));
    Normal = Signed_Angle(Flatten(Position, CraftPosition), Flatten(Rotate_Y_Axis_Vector(Vz.ToPosition(Coordinates), ((Vz.ExactEval("2*(pi)") / PlanetRotationRate) * Time)), CraftPosition), CraftPosition);
    Maneuver_Node("Deorbit", Orbit, TA, Prograde, 0, Normal);
});

var Launch_Inclination = Vz.DeclareCustomInstruction("Launch Inclination", "Inclination", "RightAscension", "Apoapsis", "PlanetRotationRate", "Wait").SetInstructions((Inclination, RightAscension, Apoapsis, PlanetRotationRate, Wait) =>
{
    Mu = (6.67384E-11 * Vz.Planet(Vz.Craft.Orbit.Planet()).Mass());
    Apoapsis = Apoapsis;
    Latitude = Vz.Abs(Vz.Deg2Rad(Vz.PosToLatLongAgl(Vz.Craft.Nav.Position()).x));
    Velocity = Orbital_Speed((Vz.Planet(Vz.Craft.Orbit.Planet()).Radius() + Apoapsis), (Vz.Planet(Vz.Craft.Orbit.Planet()).Radius() + Apoapsis));
    PlanetVelocity = (Vz.ExactEval("magnitude(FlightData.Position)*cos(abs(Latitude))*2*(pi)") / PlanetRotationRate);
    using (new If((Inclination <= Vz.ExactEval("(pi)/2"))))
    {
        Inclination = ((Inclination <= Latitude) ? Latitude : Inclination);
    }
    using (new Else())
    {
        Inclination = (((Vz.ExactEval("pi") - Inclination) <= Latitude) ? (Vz.ExactEval("pi") - Latitude) : Inclination);
    }
    LaunchPlaneNormal = Rotate_Y_Axis_Vector(Rotate_X_Axis_Vector(Vz.Vec(0, -1, 0), Inclination), RightAscension);
    Vz.ListClear(Launch);
    Vz.ListAdd(Launch, Vz.Asin((Vz.Tan(Vz.Deg2Rad(Vz.PosToLatLongAgl(Vz.Craft.Nav.Position()).x)) / Vz.Tan(Inclination))));
    Vz.ListAdd(Launch, ((Vz.ExactEval("2*(pi)") + (Vz.ExactEval("pi") - Vz.Deg2Rad(Vz.PosToLatLongAgl(Vz.Cross(LaunchPlaneNormal, Yaxis)).y))) % Vz.ExactEval("2*(pi)")));
    Vz.ListAdd(Launch, ((((((Vz.ListGet(Launch, 1) - Vz.ListGet(Launch, 2)) - Vz.Deg2Rad(Vz.PosToLatLongAgl(Vz.Craft.Nav.Position()).y)) * (PlanetRotationRate / Vz.ExactEval("2*(pi)"))) - Burn_Time(Velocity)) + Vz.Abs(PlanetRotationRate)) % Vz.Abs(PlanetRotationRate)));
    Vz.ListAdd(Launch, (((((((Vz.ExactEval("pi") - Vz.ListGet(Launch, 1)) - Vz.ListGet(Launch, 2)) - Vz.Deg2Rad(Vz.PosToLatLongAgl(Vz.Craft.Nav.Position()).y)) * (PlanetRotationRate / Vz.ExactEval("2*(pi)"))) - Burn_Time(Velocity)) + Vz.Abs(PlanetRotationRate)) % Vz.Abs(PlanetRotationRate)));
    Time = Vz.Min(Vz.ListGet(Launch, 3), Vz.ListGet(Launch, 4));
    using (new While((Time < Wait)))
    {
        Time = Vz.Max(Vz.ListGet(Launch, 3), Vz.ListGet(Launch, 4));
        using (new If((Time > Wait)))
        {
            Vz.Break();
        }
        Vz.ListSet(Launch, (Vz.ListGet(Launch, 3) + Vz.Abs(PlanetRotationRate)), 3);
        Vz.ListSet(Launch, (Vz.ListGet(Launch, 4) + Vz.Abs(PlanetRotationRate)), 4);
        Time = Vz.Min(Vz.ListGet(Launch, 3), Vz.ListGet(Launch, 4));
    }
    Vz.Log(Vz.Join("Launch Time: ", Vz.StringOp("friendly", Time, ""), ""));
    Time += Vz.Time.TotalTime();
    Vz.Broadcast(BroadCastType.Local, "Timewarp", Time);
    using (new While(((Time - Vz.Time.TotalTime()) > 0)))
    {
        Vz.Display(Vz.Join("Launch Time:<br>", Clock((Time - Vz.Time.TotalTime())), ""), 7);
        Vz.WaitSeconds(0);
    }
    Normal = Inclination;
    Vz.Display("Lift off!", 7);
    Vz.SetInput(CraftInput.Throttle, ((Vz.Length(Vz.Craft.Velocity.Gravity()) * 4) / (Vz.Craft.Performance.MaxThrust() / Vz.Craft.Performance.Mass())));
    Vz.SetTimeModeAttr("FastForward");
    using (new While((Vz.Craft.Orbit.Apoapsis() < Apoapsis)))
    {
        Vz.SetTargetHeading(TargetHeadingProperty.Pitch, Vz.Max(0, (90 - (90 * ((Vz.Craft.Orbit.Apoapsis() / Apoapsis) ^ 0.55)))));
        Vz.SetTargetHeading(TargetHeadingProperty.Heading, Vz.ExactEval("signedAngle(FlightData.North,(normalize(cross(v:LaunchPlaneNormal,FlightData.Position))*Velocity)-(normalize(FlightData.East)*PlanetVelocity),FlightData.Position)"));
        Vz.WaitSeconds(0);
    }
    Vz.SetInput(CraftInput.Throttle, 0);
    using (new WaitUntil(((!Vz.Craft.Performance.CurrentThrust()) && (!Vz.Craft.Atmosphere.AirDensity())))) { }
    Vz.WaitSeconds(1);
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    TA = True_Anomaly_at_Radius((Vz.Planet(Vz.Craft.Orbit.Planet()).Radius() + Apoapsis));
    Orbital_State_Vectors(TA);
    Maneuver_Node("Circularization", 0, TA, Orbital_Speed(Vz.Length(Position), Vz.Length(Position)), (0 - FlightPathAngle), ((Velocity.y > 0) ? (Normal - Inclination) : (Inclination - Normal)));
});

var Maneuver_Node = Vz.DeclareCustomInstruction("Maneuver Node", "Maneuver", "Time", "TrueAnomaly", "Prograde", "Radial", "Normal").SetInstructions((Maneuver, Time, TrueAnomaly, Prograde, Radial, Normal) =>
{
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    Time_to_TrueAnomaly(TrueAnomaly);
    Orbital_State_Vectors(TrueAnomaly);
    BurnPosition = Position;
    BurnVelocity = Velocity;
    BurnVector = (Rotate_Vector(Rotate_Vector((Vz.Norm(BurnVelocity) * Prograde), AngularMomentum, Radial), BurnPosition, Normal) - BurnVelocity);
    BurnVelocity = (BurnVector + BurnVelocity);
    BurnTimer = (((TimetoTA + Time) - ((Vz.Length(BurnVector) < (Vz.Craft.Performance.CurrentThrust() / Vz.Craft.Performance.Mass())) ? Burn_Time(Vz.Length(BurnVector)) : ((Burn_Time(Vz.Length(BurnVector)) + Vz.ExactEval("FlightData.WeightedThrottleResponseTime")) / 2))) + Vz.Time.TotalTime());
    Vz.Log(Vz.Format(((Burn_Time(Vz.Length(BurnVector)) > 1) ? "{0} Δv: {1:n1}m/s | Burn Length: {2:n1}s | Start time: {3}" : "{0} Δv: {1:n1}m/s | Burn Length: < 1s | Start time: {3}"), Maneuver, Vz.Length(BurnVector), Burn_Time(Vz.Length(BurnVector)), Vz.StringOp("friendly", (BurnTimer - Vz.Time.TotalTime()), ""), ""));
    Vz.Broadcast(BroadCastType.Local, "Timewarp", BurnTimer);
    using (new While(((BurnTimer - Vz.Time.TotalTime()) > 0)))
    {
        Vz.Display(Vz.Join(Maneuver, ": ", Clock((BurnTimer - Vz.Time.TotalTime())), ""), 7);
        Vz.LockNavSphere(LockNavSphereIndicatorType.Current);
        Vz.LockNavSphere(LockNavSphereIndicatorType.Vector, BurnVector);
        Vz.WaitSeconds(0);
    }
    using (new While(true))
    {
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
        Orbital_State_Vectors(Signed_Angle(BurnPosition, EccentricityVector, AngularMomentum));
        Vz.LockNavSphere(LockNavSphereIndicatorType.Current);
        Vz.LockNavSphere(LockNavSphereIndicatorType.Vector, BurnVector);
        Δv = Vz.Dot(Vz.Norm(BurnVector), (BurnVelocity - Velocity));
        using (new If((Vz.RawXmlVariable(@"<Variable list=""false"" local=""false"" variableName=""Δv"" />") > (((Vz.Craft.Performance.CurrentThrust() / Vz.Craft.Performance.Mass()) * (Vz.ExactEval("FlightData.WeightedThrottleResponseTime/2") * (Vz.Craft.Performance.CurrentThrust() / Vz.Craft.Performance.MaxThrust()))) + ((Vz.Craft.Performance.CurrentThrust() / Vz.Craft.Performance.Mass()) * 0.0334)))))
        {
            Vz.SetInput(CraftInput.Throttle, 1);
            using (new If((Vz.RawXmlVariable(@"<Variable list=""false"" local=""false"" variableName=""Δv"" />") < ((Vz.Craft.Performance.MaxThrust() / Vz.Craft.Performance.Mass()) * 2))))
            {
                Vz.SetTimeModeAttr("Normal");
            }
            using (new Else())
            {
                Vz.SetTimeModeAttr("FastForward");
            }
        }
        using (new Else())
        {
            Vz.SetInput(CraftInput.Throttle, 0);
            using (new If((!Vz.Craft.Performance.CurrentThrust())))
            {
                Vz.Break();
            }
        }
        Vz.Display(Vz.Format("{0} Δv: {1:n1}m/s", Maneuver, Vz.RawXmlVariable(@"<Variable list=""false"" local=""false"" variableName=""Δv"" />"), ""), 7);
        Vz.WaitSeconds(0);
    }
    Vz.WaitSeconds(1);
});

// ── Custom Expressions ───────────────────────────────
var Rotate_Vector = Vz.DeclareCustomExpression("Rotate Vector", "Vector", "Axis", "Angle").SetReturn((Vector, Axis, Angle) =>
{
    return ((Vector * Vz.Cos(Angle)) + ((Vz.Cross(Vector, Vz.Norm(Axis)) * Vz.Sin(Angle)) + (Vz.Norm(Axis) * (Vz.Dot(Vz.Norm(Axis), Vector) * (1 - Vz.Cos(Angle))))));
});

var Orbital_Speed = Vz.DeclareCustomExpression("Orbital Speed", "Radius", "SMA").SetReturn((Radius, SMA) =>
{
    return Vz.Sqrt((Mu * ((2 / Radius) - (1 / SMA))));
});

var Rotate_Y_Axis_Vector = Vz.DeclareCustomExpression("Rotate Y Axis Vector", "Vector", "Angle").SetReturn((Vector, Angle) =>
{
    return Vz.Vec(((Vector.x * Vz.Cos(Angle)) - (Vector.z * Vz.Sin(Angle))), Vector.y, ((Vector.z * Vz.Cos(Angle)) + (Vector.x * Vz.Sin(Angle))));
});

var Rotate_X_Axis_Vector = Vz.DeclareCustomExpression("Rotate X Axis Vector", "Vector", "Angle").SetReturn((Vector, Angle) =>
{
    return Vz.Vec(Vector.x, ((Vector.y * Vz.Cos(Angle)) + (Vector.z * Vz.Sin(Angle))), ((Vector.z * Vz.Cos(Angle)) - (Vector.y * Vz.Sin(Angle))));
});

var Burn_Time = Vz.DeclareCustomExpression("Burn Time", "DeltaV").SetReturn((DeltaV) =>
{
    return (((Vz.Craft.Performance.Mass() * (1 - (1 / (Vz.ExactEval("E") ^ (DeltaV / (9.80662 * Vz.Craft.Performance.ISP())))))) * (9.80662 * Vz.Craft.Performance.ISP())) / Vz.Craft.Performance.MaxThrust());
});

var Orbital_Speed_Period = Vz.DeclareCustomExpression("Orbital Speed Period", "Radius", "Period").SetReturn((Radius, Period) =>
{
    return Vz.Sqrt((Mu * ((2 / Radius) - (((4 ^ (1 / 3)) * (Vz.ExactEval("pi") ^ (2 / 3))) / ((Mu * (Period ^ 2)) ^ (1 / 3))))));
});

var Flatten = Vz.DeclareCustomExpression("Flatten", "Vector", "Normal").SetReturn((Vector, Normal) =>
{
    return Vz.Project(Vector, Vz.Cross(Vz.Cross(Normal, Vector), Normal));
});

var Match_String = Vz.DeclareCustomExpression("Match String", "String1", "String2").SetReturn((String1, String2) =>
{
    return (Vz.Contains(String1, String2) && Vz.Contains(String2, String1));
});

var atanh = Vz.DeclareCustomExpression("atanh", "value").SetReturn((value) =>
{
    return (Vz.Ln(((1 + value) / (1 - value))) / 2);
});

var asinh = Vz.DeclareCustomExpression("asinh", "value").SetReturn((value) =>
{
    return Vz.Ln((value + Vz.Sqrt(((value ^ 2) + 1))));
});

var acosh = Vz.DeclareCustomExpression("acosh", "value").SetReturn((value) =>
{
    return Vz.Ln((value + Vz.Sqrt(((value ^ 2) - 1))));
});

var tanh = Vz.DeclareCustomExpression("tanh", "value").SetReturn((value) =>
{
    return (sinh(value) / cosh(value));
});

var sinh = Vz.DeclareCustomExpression("sinh", "value").SetReturn((value) =>
{
    return (((Vz.ExactEval("E") ^ value) - (Vz.ExactEval("E") ^ (-1 * value))) / 2);
});

var cosh = Vz.DeclareCustomExpression("cosh", "value").SetReturn((value) =>
{
    return (((Vz.ExactEval("E") ^ value) + (Vz.ExactEval("E") ^ (-1 * value))) / 2);
});

var True_Anomaly_at_Radius = Vz.DeclareCustomExpression("True Anomaly at Radius", "Radius").SetReturn((Radius) =>
{
    return (((Radius > Apoapsis) && (Apoapsis > 0)) ? Vz.ExactEval("pi") : ((Radius < Periapsis) ? 0 : Vz.Acos(((SemiLatusRectum - Radius) / (Eccentricity * Radius)))));
});

var Signed_Angle = Vz.DeclareCustomExpression("Signed Angle", "Position", "X", "Z").SetReturn((Position, X, Z) =>
{
    return Vz.Atan2(Vz.Dot(Position, Vz.Norm(Vz.Cross(Z, X))), Vz.Dot(Position, Vz.Norm(X)));
});

var Clock = Vz.DeclareCustomExpression("Clock", "Time").SetReturn((Time) =>
{
    return Vz.Format("{0}{1}{2:00}:{3:00}:{4:00}", ((Time < 0) ? "- " : ""), Day(Time), (Vz.Floor((Vz.Abs(Time) / 3600)) % 24), (Vz.Floor((Vz.Abs(Time) / 60)) % 60), (Vz.Floor(Vz.Abs(Time)) % 60), "");
});

var Day = Vz.DeclareCustomExpression("Day", "Time").SetReturn((Time) =>
{
    return ((Vz.Floor((Vz.Abs(Time) / 86400)) > 0) ? Vz.Join(Vz.Floor((Vz.Abs(Time) / 86400)), " ", ((Vz.Floor((Vz.Abs(Time) / 86400)) == 1) ? "day " : "days "), "") : "");
});


// ── Serialize output ──
Console.WriteLine(Vz.context.currentProgram.Serialize().ToString());
