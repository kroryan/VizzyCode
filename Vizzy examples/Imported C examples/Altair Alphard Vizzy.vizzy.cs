using System;
using System.Linq;
using System.Collections.Generic;
using REWJUNO;
using REWVIZZY;

// ── Program: Altair Alphard Vizzy ──────────────────────────────────
Vz.Init("Altair Alphard Vizzy");

// ── Variables ────────────────────────────────────────
// var Craft_List = [];   // list;
// var CountDown = -0;
// var OrbitVelocity = -0;
// var ExhaustVelocity = -0;
// var OrbitDeltaV = -0;
// var OrbitBurnTime = -0;
// var OrbitApoapsis = -0;
// var PhaseAngle = -0;
// var Craft = -0;
// var Distance = -0;
// var Hours = -0;
// var Minutes = -0;
// var Seconds = -0;
// var Warp = [];   // list;
// var WarpTime = -0;
// var OrbitPeriapsis = -0;
// var VelocityHeading = -0;
// var Inclination = -0;
// var RightAscension = -0;
// var AscendingNode = -0;
// var TransferPeriod = -0;
// var VectorAngle = -0;
// var ParentSMA = -0;
// var TargetSMA = -0;
// var TransferSMA = -0;
// var TargetAngularVelocity = -0;
// var ParentAngularVelocity = -0;
// var Days = -0;
// var PotentialEnergy = -0;
// var TargetOrbitEnergy = -0;
// var ParentEnergy = -0;
// var TransferEnergy = -0;
// var TransferPotentialPeriapsis = -0;
// var TransferDeltaV = -0;
// var ExitSOI = -0;
// var EscapeSMA = -0;
// var EscapeEnergy = -0;
// var CurrentPotentialEnergy = -0;
// var EscapeDeltaV = -0;
// var EscapeEccentricity = -0;
// var EscapeAngle = -0;
// var AngularVelocity = -0;
// var AngleNode = -0;
// var DeltaTime = -0;
// var TimeSincePerigee = -0;
// var Eccentricity = -0;
// var TrueAnomaly = -0;
// var MeanAnomaly = -0;
// var Mu = -0;
// var AngularMomentum = -0;
// var TotalTime = -0;
// var Error = -0;
// var Nmax = -0;
// var alpha = -0;
// var x = -0;
// var n = -0;
// var ratio = -0;
// var C = -0;
// var S = -0;
// var F = -0;
// var dFdx = -0;
// var ro = -0;
// var r = -0;
// var vro = -0;
// var UniX = -0;
// var z = -0;
// var f = -0;
// var g = -0;
// var fdot = -0;
// var gdot = -0;
// var vo = -0;
// var vectorR2 = -0;
// var vectorV2 = -0;
// var Chord = -0;
// var A = -0;
// var theta = -0;
// var VectorV1 = -0;
// var VectorR1 = -0;
// var OrbitLongitude = -0;
// var TargetName = -0;
// var Text = -0;
// var UpHill = -0;
// var Interplanetary = -0;
// var Landing = -0;
// var PlanetRotationSpeed = -0;
// var TargetNorthDistance = -0;
// var TargetEastDistance = -0;
// var CraftNorthVelocity = -0;
// var CraftEastVelocity = -0;
// var CraftVelocityHeading = -0;
// var PitchProportional = -0;
// var PitchDerivat = -0;
// var Pitch = -0;
// var Heading = -0;
// var TargetHeading = -0;
// var LateralRelativeVelocity = -0;
// var TimeUntilSurface = -0;
// var NorthTargetVelDelta = -0;
// var EastTargetVelDelta = -0;
// var TargetDelta = -0;
// var PrevTargetDelta = -0;
// var EastRangeRatio = -0;
// var NorthRangeRatio = -0;
// var PrevNorthDistance = -0;
// var PrevEastDistance = -0;
// var KP = -0;
// var KI = -0;
// var KD = -0;
// var EscapeVelocity = -0;
// var Time1 = -0;
// var Time2 = -0;
// var Flyby = -0;
// var Vizzy = -0;
// var Node = -0;
// var Azimuth = -0;
// var OrbitalPlane = -0;
// var HeadingError = -0;
// var Mission = [];   // list;
// var Display = -0;
// var Docked = -0;
// var Vector = -0;
// var DescendingNode = -0;
// var time = -0;
// var LEM_Landing = [];   // list;
// var Dump = -0;
// var Craft_Name = [];   // list;
// var Letter_Dump = [];   // list;
// var Throttle = -0;
// var Abort = -0;

using (new OnStart())
{
    // Universal MFD Vizzy Mission 1.05 by Rizkman (c)2020
    // Auto Orbit, Rendezvous, Docking, Orbit Transfer, Landing
    // You can adjust parameter above
    // Don't complain about my Vizzy. I'm not a programmer or enginer
    Abort = false;
    CountDown = -0;
    PrevTargetDelta = -0;
    Interplanetary = false;
    Landing = false;
    Flyby = false;
    Warp = Vz.CreateListRaw("50,100,500,2500,10000,50000,250000,1000000,5000000");
    Vz.Broadcast(BroadCastType.Local, "Staging", -0);
}

// Vizzy Stumpff Function, for z > 0 because vizzy can't calculate imaginary number
// Looks confusing? Me too!
// ── Custom Instructions ──────────────────────────────
var Time_format = Vz.DeclareCustomInstruction("Time format", "time_in_seconds").SetInstructions((time_in_seconds) =>
{
    Days = Vz.Floor((time_in_seconds / (24 * 3600)));
    Hours = Vz.Floor((time_in_seconds / 3600));
    Minutes = Vz.Floor(((time_in_seconds - (Hours * 3600)) / 60));
    Seconds = (time_in_seconds - ((Hours * 3600) + (Minutes * 60)));
});

var Manuver_Burn = Vz.DeclareCustomInstruction("Manuver Burn", "delta_v").SetInstructions((delta_v) =>
{
    OrbitDeltaV = delta_v;
    OrbitBurnTime = ((delta_v / Vz.Craft.Performance.StageDeltaV()) * Vz.Craft.Performance.BurnTime());
});

var Universal_Anomaly = Vz.DeclareCustomInstruction("Universal Anomaly", "position", "velocity", "deltatime").SetInstructions((position, velocity, deltatime) =>
{
    // Kepler Universal Variable, for eliptic orbit only
    Error = 0.00000001;
    Nmax = 1000;
    ro = Vz.Length(position);
    vro = (Vz.Dot(position, velocity) / Vz.Length(position));
    DeltaTime = deltatime;
    x = ((Vz.Sqrt(Mu) * Vz.Abs(alpha)) * DeltaTime);
    n = -0;
    ratio = 1;
    using (new While(((Vz.Abs(ratio) > Error) && (n <= Nmax))))
    {
        n = (n + 1);
        C = Stump_C((alpha * (x ^ 2)));
        S = Stump_S((alpha * (x ^ 2)));
        F = (((((ro * vro) / Vz.Sqrt(Mu)) * ((x ^ 2) * C)) + (((1 - (alpha * ro)) * ((x ^ 3) * S)) + (ro * x))) - (Vz.Sqrt(Mu) * DeltaTime));
        dFdx = (((((ro * vro) / Vz.Sqrt(Mu)) * (x * (1 - ((alpha * (x ^ 2)) * S)))) + ((1 - (alpha * ro)) * ((x ^ 2) * C))) + ro);
        ratio = (F / dFdx);
        x = (x - ratio);
    }
    // Return Universal Anomaly X since Delta Time
    UniX = x;
});

var Orbit_Prediction_from = Vz.DeclareCustomInstruction("Orbit Prediction from", "position", "velocity", "deltatime").SetInstructions((position, velocity, deltatime) =>
{
    // Predictor for guessing intercept point, based on Lagrange formula
    DeltaTime = deltatime;
    // Change Mu variable input here
    Mu = Mu;
    ro = Vz.Length(position);
    vo = Vz.Length(velocity);
    vro = (Vz.Dot(position, velocity) / Vz.Length(position));
    alpha = ((2 / ro) - ((vo ^ 2) / Mu));
    Universal_Anomaly(position, velocity, deltatime);
    F_and_G_function_from_Universal(UniX, ro, DeltaTime);
    vectorR2 = ((f * position) + (g * velocity));
    r = Vz.Length(vectorR2);
    F__and_G__from_Universal(UniX, ro, r, DeltaTime);
    vectorV2 = ((fdot * position) + (gdot * velocity));
});

var F_and_G_function_from_Universal = Vz.DeclareCustomInstruction("F and G function from Universal", "anomaly", "position", "deltatime").SetInstructions((anomaly, position, deltatime) =>
{
    // Lagrange coefficient & its derivat
    // input: Universal Anomaly since delta t, r0. Required Mu and Reciprocal SMA
    DeltaTime = deltatime;
    ro = position;
    x = anomaly;
    z = (alpha * (x ^ 2));
    f = (1 - (((x ^ 2) / ro) * Stump_C(z)));
    g = (DeltaTime - (((x ^ 3) / Vz.Sqrt(Mu)) * Stump_S(z)));
});

var F__and_G__from_Universal = Vz.DeclareCustomInstruction("F. and G. from Universal", "anomaly", "position1", "position2", "deltatime").SetInstructions((anomaly, position1, position2, deltatime) =>
{
    // input: Universal Anomaly since delta t, r0, r since delta t. Required Mu and Reciprocal SMA
    DeltaTime = deltatime;
    ro = position1;
    r = position2;
    x = anomaly;
    z = (alpha * (x ^ 2));
    fdot = ((Vz.Sqrt(Mu) / (r * ro)) * (((alpha * (x ^ 3)) * Stump_S(z)) - x));
    gdot = (1 - (((x ^ 2) / r) * Stump_C(z)));
});

var Lambert_s_Solver_from = Vz.DeclareCustomInstruction("Lambert's Solver from", "position1", "position2", "deltatime").SetInstructions((position1, position2, deltatime) =>
{
    // Solution for Lambert's Problem in my version, input: vectorR1, vectorR2, & elapsed time
    ro = Vz.Length(position1);
    r = Vz.Length(position2);
    DeltaTime = deltatime;
    Chord = (position2 - position1);
    theta = Vz.Acos((Vz.Dot(position1, position2) / (ro * r)));
    A = (Vz.Sin(theta) * Vz.Sqrt(((ro * r) / (1 - Vz.Cos(theta)))));
    z = 0;
    using (new While((F(z, DeltaTime) < -0)))
    {
        z = (z + 0.1);
    }
    Error = 0.00000001;
    Nmax = 5000;
    Nmax = 5000;
    n = -0;
    ratio = 1;
    using (new While(((Vz.Abs(ratio) > Error) && (n <= Nmax))))
    {
        n = (n + 1);
        ratio = (F(z, DeltaTime) / dFdz(z));
        z = (z - ratio);
    }
    f = (1 - (y(z) / ro));
    g = (A * Vz.Sqrt((y(z) / Mu)));
    gdot = (1 - (y(z) / r));
    VectorV1 = ((1 / g) * (position2 - (f * position1)));
    vectorV2 = ((1 / g) * ((gdot * position2) - position1));
});

var Warp = Vz.DeclareCustomInstruction("Warp", "time").SetInstructions((time) =>
{
    // Gozinya's timewarp
    using (new For("i").From(1).To(Vz.ListLength(Warp)).By(1))
    {
        using (new If(((time * 2) > Vz.ListGet(Warp, i))))
        {
            WarpTime = i;
        }
    }
    using (new If((time < 4)))
    {
        Vz.SetTimeModeAttr("Normal");
    }
    using (new ElseIf((WarpTime == -0)))
    {
        Vz.SetTimeModeAttr("FastForward");
    }
    using (new ElseIf((WarpTime == 1)))
    {
        Vz.SetTimeModeAttr("TimeWarp1");
    }
    using (new ElseIf((WarpTime == 2)))
    {
        Vz.SetTimeModeAttr("TimeWarp2");
    }
    using (new ElseIf((WarpTime == 3)))
    {
        Vz.SetTimeModeAttr("TimeWarp3");
    }
    using (new ElseIf((WarpTime == 4)))
    {
        Vz.SetTimeModeAttr("TimeWarp4");
    }
    using (new ElseIf((WarpTime == 5)))
    {
        Vz.SetTimeModeAttr("TimeWarp5");
    }
    using (new ElseIf((WarpTime == 6)))
    {
        Vz.SetTimeModeAttr("TimeWarp6");
    }
    using (new ElseIf((WarpTime == 7)))
    {
        Vz.SetTimeModeAttr("TimeWarp7");
    }
    using (new ElseIf((WarpTime == 8)))
    {
        Vz.SetTimeModeAttr("TimeWarp8");
    }
    using (new ElseIf((WarpTime == 9)))
    {
        Vz.SetTimeModeAttr("TimeWarp9");
    }
    Vz.WaitSeconds(0.1);
});

var Trajectory_Calculation_from = Vz.DeclareCustomInstruction("Trajectory Calculation from", "position1", "position2", "CenterOfMass", "DeltaTime").SetInstructions((position1, position2, CenterOfMass, DeltaTime) =>
{
    // Calculating Transfer Trajectory
    using (new If((!Abort)))
    {
        Vz.SetTimeModeAttr("Normal");
        Vz.Display("Please Wait, Checking Trajectory", 7);
        DeltaTime = DeltaTime;
        Mu = (G() * Vz.Planet(CenterOfMass).Mass());
        Orbit_Prediction_from(position2, Vz.Craft.Target.Velocity(), DeltaTime);
        VectorR1 = position1;
        Lambert_s_Solver_from(VectorR1, vectorR2, DeltaTime);
    }
});

var Phase_Angle_from = Vz.DeclareCustomInstruction("Phase Angle from", "position1", "position2", "CenterOfMass").SetInstructions((position1, position2, CenterOfMass) =>
{
    // Phase Angle with same Orbit Plane
    TransferSMA = ((Vz.Length(position1) + Vz.Length(position2)) / 2);
    TransferPeriod = (Pi() * Vz.Sqrt(((TransferSMA ^ 3) / (G() * Vz.Planet(CenterOfMass).Mass()))));
    ParentAngularVelocity = ((360 / (2 * Pi())) * Vz.Sqrt(((G() * Vz.Planet(CenterOfMass).Mass()) / (Vz.Length(position1) ^ 3))));
    TargetAngularVelocity = ((360 / (2 * Pi())) * Vz.Sqrt(((G() * Vz.Planet(CenterOfMass).Mass()) / (Vz.Length(position2) ^ 3))));
    PhaseAngle = ((180 - (TransferPeriod * TargetAngularVelocity)) % 360);
    OrbitalPlane = Orbit_Plane_Normal(Ascending_Node(Right_Ascension(Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity())), Inclination(Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity()));
    using (new If((PhaseAngle < -180)))
    {
        PhaseAngle = (360 - PhaseAngle);
    }
    using (new While((true && (!Abort))))
    {
        Vz.LockNavSphere(LockNavSphereIndicatorType.Prograde);
        AngleNode = Rotate(((Interplanetary == true) ? Target_Solar_Position() : Vz.Craft.Target.Position()), OrbitalPlane, (PhaseAngle + ((PhaseAngle > -0) ? 1 : -1)));
        VectorAngle = Vz.Angle(((Interplanetary == true) ? Parent_Solar_Position() : Rejection_of(Vz.Craft.Nav.Position(), OrbitalPlane)), AngleNode);
        using (new If(((Vz.Cross(((Interplanetary == true) ? Parent_Solar_Position() : Rejection_of(Vz.Craft.Nav.Position(), OrbitalPlane)), AngleNode).y >= -0) && (PhaseAngle < -0))))
        {
            VectorAngle = (360 - VectorAngle);
        }
        using (new ElseIf(((Vz.Cross(((Interplanetary == true) ? Parent_Solar_Position() : Rejection_of(Vz.Craft.Nav.Position(), OrbitalPlane)), AngleNode).y < -0) && (PhaseAngle >= -0))))
        {
            VectorAngle = (360 - VectorAngle);
        }
        CountDown = ((360 - VectorAngle) / Vz.Abs((ParentAngularVelocity - TargetAngularVelocity)));
        Time_format(CountDown);
        using (new If(((CountDown / (3600 * 24)) >= 1)))
        {
            Display(Vz.Format("Target Angle Alignment: {0:n0} deg | Current Angle: {1:n0} deg | Departure Time: {2} day(s)", PhaseAngle, (VectorAngle + PhaseAngle), Days, ""));
        }
        using (new ElseIf(((CountDown / (3600 * 24)) < 1)))
        {
            Display(Vz.Format("Target Angle Alignment: {0:n0} deg | Current Angle: {1:n0} deg | Departure Time: {2}:{3:00}:{4:00}", PhaseAngle, (VectorAngle + PhaseAngle), Hours, Minutes, Seconds, ""));
        }
        using (new If(((Vz.Round(VectorAngle) <= 2) || (Vz.Round(VectorAngle) >= 358))))
        {
            Vz.Display("Phase Angle Match", 7);
            Vz.Break();
        }
        Warp((CountDown - 5));
    }
    Vz.SetTimeModeAttr("Normal");
});

var Reentry_Time_Wrap = Vz.DeclareCustomInstruction("Reentry Time Wrap").SetInstructions(() =>
{
    using (new If((Vz.Craft.AltitudeASL() > 200000)))
    {
        Vz.SetTimeModeAttr("TimeWarp3");
    }
    using (new ElseIf((Vz.Craft.AltitudeASL() > 100000)))
    {
        Vz.SetTimeModeAttr("TimeWarp2");
    }
    using (new ElseIf((Vz.Craft.AltitudeASL() > 62000)))
    {
        Vz.SetTimeModeAttr("TimeWarp1");
    }
    using (new ElseIf((Vz.Craft.AltitudeASL() < 62000)))
    {
        Vz.SetTimeModeAttr("FastForward");
    }
});

var Period_Phase_Angle = Vz.DeclareCustomInstruction("Period Phase Angle", "degree").SetInstructions((degree) =>
{
    AngleNode = Rotate(Vz.Craft.Target.Position(), Vz.Craft.Nav.North(), degree);
    VectorAngle = Vz.Angle(Vz.Craft.Nav.Position(), AngleNode);
    using (new If((Vz.Cross(Vz.Craft.Nav.Position(), AngleNode).y < -0)))
    {
        VectorAngle = (360 - VectorAngle);
    }
    CountDown = (((360 - VectorAngle) / 360) * Vz.Craft.Orbit.Period());
});

var Countdown = Vz.DeclareCustomInstruction("Countdown", "time").SetInstructions((time) =>
{
    CountDown = time;
    using (new While((CountDown > -0)))
    {
        Display(Vz.Format("T- {0} second(s)", CountDown, ""));
        CountDown += -1;
        Vz.WaitSeconds(1);
    }
});

var Vizzy_Status = Vz.DeclareCustomInstruction("Vizzy Status", "value").SetInstructions((value) =>
{
    Vizzy = value;
    using (new If(Vizzy))
    {
        Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Vizzy Indicator"), true);
        Vz.WaitSeconds(1);
        Vz.SetActivationGroup(1, false);
    }
    using (new Else())
    {
        Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Vizzy Indicator"), false);
    }
});

using (new OnReceiveMessage("Staging"))
{
    // Auto staging
    Boot_Start();
    using (new While(true))
    {
        using (new While((Vizzy && (Vz.CraftInput(CraftInput.Throttle) > 0))))
        {
            using (new If((Vz.Craft.Fuel.FuelInStage() == -0)))
            {
                using (new WaitUntil((Vz.Craft.Performance.CurrentThrust() == -0))) { }
                Vz.WaitSeconds(0.5);
                Vz.ActivateStage();
                Vz.WaitSeconds(1);
            }
            Vz.WaitSeconds(0.1);
        }
        Vz.WaitSeconds(0);
    }
}

var Display = Vz.DeclareCustomInstruction("Display", "text").SetInstructions((text) =>
{
    Vz.Display(text, 7);
    Vz.Log(text);
});

using (new OnReceiveMessage("CMLaunch to Orbit"))
{
    Vizzy_Status(true);
    Launch_to_Orbit(data, -0, -0);
    Orbit_Circularization();
    Vizzy_Status(false);
    Vz.Broadcast(BroadCastType.Local, "Deorbit Menu", -0);
}

// Navigation
var Launch_to_Orbit = Vz.DeclareCustomInstruction("Launch to Orbit", "apoapsis", "inclination", "longitude").SetInstructions((apoapsis, inclination, longitude) =>
{
    // Atmospheric & non Atmospheric Launch
    using (new If((!((inclination == 0) && (longitude == 0)))))
    {
        Node = Rejection_of(Vz.Craft.Nav.Position(), Orbit_Plane_Normal(Ascending_Node(longitude), inclination));
        using (new If(((Vz.Angle(Vz.Craft.Nav.Position(), Node) > 5) || (Vz.Length(Vz.Craft.Velocity.Orbital()) > 50))))
        {
            Launch_Window(Orbit_Plane_Normal(Ascending_Node(longitude), inclination));
        }
        OrbitalPlane = Orbit_Plane_Normal(Ascending_Node(longitude), inclination);
    }
    using (new Else())
    {
        OrbitalPlane = Vz.Vec(-0, 1, -0);
    }
    using (new If((Vz.PartNameToID("Side Booster") > -0)))
    {
        Vz.Broadcast(BroadCastType.Local, "Side Booster", -0);
    }
    Vz.SetTimeModeAttr("Normal");
    Countdown(6);
    Display("Ignition!");
    Vz.SetInput(CraftInput.Throttle, 1);
    Vz.SetActivationGroup(9, false);
    Vz.WaitSeconds(3);
    using (new While((Vz.Craft.Velocity.Surface() < 50)))
    {
        Craft((Vz.Craft.Velocity.Gravity() * -1));
        Vz.SetTimeModeAttr("FastForward");
    }
    using (new While(((Vz.Craft.Orbit.Apoapsis() < (apoapsis * 1000)) && (Vz.Craft.Orbit.TimeToAp() > 1))))
    {
        Display(Vz.Format("Gravity Turn | Orbit Apoapsis: {0:n1} km", (Vz.Craft.Orbit.Apoapsis() / 1000), ""));
        Set_Heading_to_Azimuth(OrbitalPlane);
        using (new If((Vz.Craft.AltitudeASL() < Vz.Planet(Vz.Craft.Orbit.Planet()).AtmosphereDepth())))
        {
            Vz.SetTargetHeading(TargetHeadingProperty.Pitch, (90 - (90 * ((Vz.Craft.AltitudeASL() / Vz.Planet(Vz.Craft.Orbit.Planet()).AtmosphereDepth()) ^ 0.5))));
            Vz.SetInput(CraftInput.Throttle, TWR__(2.3));
        }
        using (new ElseIf((Vz.Craft.AltitudeASL() > Vz.Planet(Vz.Craft.Orbit.Planet()).AtmosphereDepth())))
        {
            Vz.SetTargetHeading(TargetHeadingProperty.Pitch, 0);
            Vz.SetActivationGroup(10, true);
            using (new If(((Vz.Sqrt((Mu() / (apoapsis + Vz.Planet(Vz.Craft.Orbit.Planet()).Radius()))) - Vz.Length(Vz.Craft.Velocity.Orbital())) < 325)))
            {
                Vz.SetInput(CraftInput.Throttle, Vz.Max(((((apoapsis * 1000) - Vz.Craft.Orbit.Apoapsis()) / (apoapsis * 1000)) * 1), 0.02));
            }
            using (new Else())
            {
                Vz.SetInput(CraftInput.Throttle, 1);
            }
        }
        using (new If(Abort))
        {
            Vz.Broadcast(BroadCastType.Local, "Escape Tower", -0);
            Vz.Break();
        }
        Vz.WaitSeconds(0);
    }
    Vz.SetInput(CraftInput.Throttle, 0);
    Vz.SetTimeModeAttr("FastForward");
    Vz.WaitSeconds(1);
    using (new If((Vz.PartNameToID("First Stage") > -0)))
    {
        using (new WaitUntil((Vz.Craft.Performance.CurrentThrust() == -0))) { }
        Vz.ActivateStage();
    }
});

var Launch_Window = Vz.DeclareCustomInstruction("Launch Window", "OrbitalPlane").SetInstructions((OrbitalPlane) =>
{
    CraftEastVelocity = Vz.Length(Vz.Craft.Velocity.Orbital());
    PlanetRotationSpeed = (Vz.Length(Vz.Craft.Velocity.Orbital()) / Vz.Length(Vz.Craft.Nav.Position()));
    AngleNode = Node;
    using (new While((Vz.Angle(Vz.Craft.Nav.Position(), AngleNode) > 0.25)))
    {
        AngleNode = Rejection_of(Vz.Craft.Nav.Position(), OrbitalPlane);
        VectorAngle = Vz.Angle(Vz.Craft.Nav.Position(), AngleNode);
        Display(Vz.Format("Aligning to Target Orbital Plane : {0:n2} deg", VectorAngle, ""));
        using (new If((Vz.Cross(Vz.Craft.Nav.Position(), AngleNode).y > -0)))
        {
            VectorAngle = (360 - VectorAngle);
        }
        CountDown = (Vz.Deg2Rad(VectorAngle) / PlanetRotationSpeed);
        Warp((CountDown * 2));
        using (new If((CountDown < 4)))
        {
            Vz.Break();
        }
    }
});

// MFD Listener
var Hohmann_Parameter_from = Vz.DeclareCustomInstruction("Hohmann Parameter from", "position1", "position2", "CenterOfMass").SetInstructions((position1, position2, CenterOfMass) =>
{
    ParentSMA = Vz.Length(position1);
    TargetSMA = Vz.Length(position2);
    TransferSMA = ((ParentSMA + TargetSMA) / 2);
    TransferEnergy = (-1 * ((G() * Vz.Planet(CenterOfMass).Mass()) / (2 * TransferSMA)));
    ParentEnergy = (-1 * ((G() * Vz.Planet(CenterOfMass).Mass()) / (2 * ParentSMA)));
    TransferPotentialPeriapsis = (-1 * ((G() * Vz.Planet(CenterOfMass).Mass()) / Vz.Length(Vz.Planet(Vz.Craft.Orbit.Planet()).SolarPosition())));
    TransferDeltaV = (Vz.Sqrt((2 * Vz.Abs((TransferEnergy - TransferPotentialPeriapsis)))) - Vz.Sqrt((2 * Vz.Abs((ParentEnergy - TransferPotentialPeriapsis)))));
    TransferPeriod = (Pi() * Vz.Sqrt(((TransferSMA ^ 3) / (G() * Vz.Planet(CenterOfMass).Mass()))));
});

var Hohmann_Transfer = Vz.DeclareCustomInstruction("Hohmann Transfer").SetInstructions(() =>
{
    Vz.Display("Please Wait", 7);
    Vz.SetTimeModeAttr("Normal");
    Vz.WaitSeconds(1);
    Hohmann_Parameter_from(Parent_Solar_Position(), (((Vz.Length(Target_Solar_Position()) > Vz.Length(Parent_Solar_Position())) ? 0.88 : 1.05) * Target_Solar_Position()), Parent_Star());
    // Sflanker's style Energy Equation
    ExitSOI = (ParentSMA * ((Vz.Planet(Vz.Craft.Orbit.Planet()).Mass() / Vz.Planet(Parent_Star()).Mass()) ^ 0.4));
    EscapeSMA = (1 / ((2 / ExitSOI) - ((TransferDeltaV ^ 2) / Mu())));
    EscapeEnergy = (Mu() / (-2 * EscapeSMA));
    CurrentPotentialEnergy = (-1 * (Mu() / Vz.Length(Vz.Craft.Nav.Position())));
    EscapeDeltaV = (Vz.Sqrt((2 * (EscapeEnergy - CurrentPotentialEnergy))) - Vz.Sqrt((2 * (Specific_Orbital_Energy(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) - CurrentPotentialEnergy))));
    OrbitBurnTime = ((EscapeDeltaV / Vz.Craft.Performance.StageDeltaV()) * Vz.Craft.Performance.BurnTime());
    EscapeEccentricity = Vz.Max((1 + Vz.Abs(((Vz.Length(Vz.Craft.Nav.Position()) * (2 * EscapeEnergy)) / Mu()))), 1.1);
    EscapeAngle = (Vz.Rad2Deg(Vz.Acos((1 / EscapeEccentricity))) - ((360 / Vz.Craft.Orbit.Period()) * ((OrbitBurnTime / 2) + 10)));
    AngularVelocity = ((360 / (2 * Pi())) * Vz.Sqrt((Mu() / (Vz.Length(Vz.Craft.Nav.Position()) ^ 3))));
    TransferPeriod = (Pi() * Vz.Sqrt(((((Vz.Length(Parent_Solar_Position()) + Vz.Length(Target_Solar_Position())) / 2) ^ 3) / (G() * Vz.Planet(Parent_Star()).Mass()))));
    // Transfer Burn
    using (new While(true))
    {
        Craft(Vz.Craft.Nav.East());
        AngleNode = Rotate(Vz.Cross(Vz.Norm(Parent_Solar_Position()), Vz.Craft.Nav.North()), Vz.Craft.Nav.North(), ((PhaseAngle > -0) ? (180 - EscapeAngle) : (-1 * EscapeAngle)));
        VectorAngle = Vz.Angle(AngleNode, Vz.Craft.Nav.Position());
        using (new If((Vz.Cross(Vz.Craft.Nav.Position(), AngleNode).y < -0)))
        {
            VectorAngle = (360 - VectorAngle);
        }
        CountDown = ((360 - VectorAngle) / AngularVelocity);
        Time_format(CountDown);
        Display(Vz.Format("Escape Angle: {0:n0} deg | Current: {1:n0} deg | Time Remaining: {2}:{3:00}:{4:00} | T: {5:n0} sec", EscapeAngle, ((360 - VectorAngle) + EscapeAngle), Hours, Minutes, Seconds, OrbitBurnTime, ""));
        using (new If(((Vz.Round(VectorAngle) == -0) || Abort)))
        {
            Display("Angle Match, Preparing Burn!");
            Vz.Break();
        }
        Warp((CountDown - 3));
    }
    Vz.SetTimeModeAttr("Normal");
    Vz.WaitSeconds(1);
    using (new While((Specific_Orbital_Energy(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) < EscapeEnergy)))
    {
        Craft(Vz.Craft.Velocity.Orbital());
        CurrentPotentialEnergy = (-1 * (Mu() / Vz.Length(Vz.Craft.Nav.Position())));
        EscapeDeltaV = (Vz.Sqrt((2 * (EscapeEnergy - CurrentPotentialEnergy))) - Vz.Sqrt((2 * (Specific_Orbital_Energy(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) - CurrentPotentialEnergy))));
        Manuver_Burn(EscapeDeltaV);
        Display(Vz.Format("Transfer Burn | DeltaV Remaining: {0:n1} m/s | Burn Time: {1:n1} sec", EscapeDeltaV, (OrbitBurnTime * (Vz.Craft.Performance.MaxThrust() / Vz.Craft.Performance.CurrentThrust())), ""));
        using (new If((EscapeDeltaV < 5)))
        {
            Vz.SetInput(CraftInput.Throttle, Vz.Max((EscapeDeltaV / 100), 0.05));
        }
        using (new ElseIf((OrbitBurnTime <= 2)))
        {
            Vz.SetInput(CraftInput.Throttle, Vz.Max(Vz.Log10(((OrbitBurnTime * 4.5) + 1)), 0.1));
        }
        using (new Else())
        {
            Vz.SetInput(CraftInput.Throttle, 1);
        }
        using (new If(((Vz.Craft.Orbit.Eccentricity() >= EscapeEccentricity) || Abort)))
        {
            Vz.Break();
        }
    }
    Vz.SetInput(CraftInput.Throttle, -0);
    Vz.LockNavSphere(LockNavSphereIndicatorType.Prograde);
    Display(Vz.Format("Burn Completed | Heading to: {0}", Vz.Craft.Target.Name(), ""));
});

var Orbit_Circularization = Vz.DeclareCustomInstruction("Orbit Circularization").SetInstructions(() =>
{
    // Exhaust Velocity between game data & vizzy not in same result
    PotentialEnergy = (-1 * (Mu() / (Vz.Craft.Orbit.Apoapsis() + Vz.Planet(Vz.Craft.Orbit.Planet()).Radius())));
    TargetOrbitEnergy = (Mu() / (-2 * (Vz.Craft.Orbit.Apoapsis() + Vz.Planet(Vz.Craft.Orbit.Planet()).Radius())));
    Manuver_Burn((Vz.Sqrt((2 * (TargetOrbitEnergy - PotentialEnergy))) - Vz.Sqrt((2 * (Specific_Orbital_Energy(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) - PotentialEnergy)))));
    using (new While((Vz.Craft.Orbit.TimeToAp() > ((OrbitBurnTime * 0.5) + 2))))
    {
        Time_format((Vz.Craft.Orbit.TimeToAp() - ((OrbitBurnTime * 0.5) + 1)));
        Display(Vz.Format("Waiting Apoapsis Burn: {0}:{1:00}:{2:00} | DeltaV: {3:n0} m/s | Burn time: {4:n0} sec", Hours, Minutes, Seconds, OrbitDeltaV, OrbitBurnTime, ""));
        Craft((Vz.Cross(OrbitalPlane, Vz.Craft.Nav.Position()) * -1));
        Warp((Vz.Craft.Orbit.TimeToAp() - ((OrbitBurnTime * 0.5) + 1)));
    }
    Vz.SetTimeModeAttr("Normal");
    using (new While((Specific_Orbital_Energy(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) < TargetOrbitEnergy)))
    {
        Vz.SetTimeModeAttr("Normal");
        Craft((Vz.Cross(OrbitalPlane, Vz.Craft.Nav.Position()) * -1));
        OrbitDeltaV = Target_DeltaV__to(TargetOrbitEnergy, PotentialEnergy, Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
        Display(Vz.Format("Circularization | DeltaV Remaining: {0:n1} m/s | Burn Time: {1:n1} sec", OrbitDeltaV, (OrbitBurnTime * (Vz.Craft.Performance.MaxThrust() / Vz.Craft.Performance.CurrentThrust())), ""));
        Manuver_Burn(OrbitDeltaV);
        using (new If((OrbitDeltaV < 5)))
        {
            Vz.SetInput(CraftInput.Throttle, Vz.Max((OrbitDeltaV / 100), 0.05));
        }
        using (new ElseIf((OrbitBurnTime <= 2)))
        {
            Vz.SetInput(CraftInput.Throttle, Vz.Max(Vz.Log10(((OrbitBurnTime * 4.5) + 1)), 0.05));
        }
        using (new Else())
        {
            Vz.SetInput(CraftInput.Throttle, 1);
        }
        using (new If((Vz.Craft.Orbit.Eccentricity() < 0.00025)))
        {
            Vz.Break();
        }
        Vz.WaitSeconds(0);
    }
    Vz.SetInput(CraftInput.Throttle, -0);
    Vz.LockNavSphere(LockNavSphereIndicatorType.Prograde);
    Vz.Display("Circular!", 7);
});

var Set_Heading_to_Azimuth = Vz.DeclareCustomInstruction("Set Heading to Azimuth", "OrbitalPlane").SetInstructions((OrbitalPlane) =>
{
    HeadingError = ((Vz.Norm(Vz.Cross(Vz.Craft.Nav.Position(), OrbitalPlane)) * Vz.Craft.Velocity.Orbital()) - Vz.Craft.Velocity.Orbital());
    Azimuth = ((Vz.Norm(Vz.Cross(Vz.Craft.Nav.Position(), OrbitalPlane)) * Vz.Craft.Velocity.Orbital()) + ((Vz.Length(HeadingError) > 1) ? (5 * HeadingError) : -0));
    Vz.SetTargetHeading(TargetHeadingProperty.Heading, Vz.Rad2Deg(Vz.Atan2(Vz.Dot(Azimuth, Vz.Craft.Nav.East()), Vz.Dot(Azimuth, Vz.Craft.Nav.North()))));
});

// Custom Expression
// HomePlanetLandingPad
// Target Landing Offset at (east,north,0) in meter
// Default Home Planet Parking Orbit Altitude
// Default Target Planet Parking Orbit Altitude
using (new OnReceiveMessage("Launch to Target Planet"))
{
    using (new WaitUntil((Vz.LengthOf(Vz.Craft.Target.Name()) > -0))) { }
    Vz.Display(Vz.Format("Finding {0} Launch Window", Vz.Craft.Target.Name(), ""), 7);
    Interplanetary = true;
    Phase_Angle_from(Parent_Solar_Position(), Target_Solar_Position(), Parent_Star());
    Vz.ListClear(Mission);
    Vz.ListAdd(Mission, Vz.Craft.Orbit.Planet());
    Vz.ListAdd(Mission, data);
    Vizzy_Status(true);
    Launch_to_Orbit(150, -0, -0);
    Orbit_Circularization();
    Hohmann_Transfer();
    Vz.SetTimeModeAttr("FastForward");
    Vz.WaitSeconds(1);
    Warp((0.25 * TransferPeriod));
}

var Matching_Orbit_Plane = Vz.DeclareCustomInstruction("Matching Orbit Plane", "TargetPlaneNormal").SetInstructions((TargetPlaneNormal) =>
{
    // Simple clockwise inclination sync
    Vz.SetTimeModeAttr("Normal");
    RightAscension = Right_Ascension(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    OrbitalPlane = Orbit_Plane_Normal(Ascending_Node(RightAscension), Inclination(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()));
    AscendingNode = Vz.Norm(Vz.Cross(TargetPlaneNormal, OrbitalPlane));
    DescendingNode = (-1 * AscendingNode);
    AngleNode = Vz.Min(Vz.Angle(Vz.Craft.Nav.Position(), DescendingNode), Vz.Angle(Vz.Craft.Nav.Position(), AscendingNode));
    using (new While(((AngleNode > 0.05) && (!Abort))))
    {
        AngleNode = Vz.Min(Vz.Angle(Vz.Craft.Nav.Position(), DescendingNode), Vz.Angle(Vz.Craft.Nav.Position(), AscendingNode));
        PhaseAngle = ((AngleNode / 360) * Vz.Craft.Orbit.Period());
        Warp(PhaseAngle);
        Time_format(PhaseAngle);
        using (new If((Vz.Cross(Vz.Craft.Nav.Position(), AscendingNode).y < -0)))
        {
            Vz.Display(((CountDown == true) ? Vz.Format("Trying 2nd Burn | Next Burn = {0}:{1:00}:{2:00}", Hours, Minutes, Seconds, "") : Vz.Format("Matching Inclination | Time to Ascending Node= {0}:{1:00}:{2:00}", Hours, Minutes, Seconds, "")), 7);
            Craft(Normal_Vector());
        }
        using (new Else())
        {
            Vz.Display(((CountDown == true) ? Vz.Format("Trying 2nd Burn | Next Burn = {0}:{1:00}:{2:00}", Hours, Minutes, Seconds, "") : Vz.Format("Matching Inclination | Time to Descending Node= {0}:{1:00}:{2:00}", Hours, Minutes, Seconds, "")), 7);
            Craft(Anti_Normal_Vector());
        }
    }
    Vz.SetTimeModeAttr("FastForward");
    OrbitDeltaV = ((2 * Vz.Length(Vz.Craft.Velocity.Orbital())) * Vz.Sin((Vz.Deg2Rad(Vz.Angle(OrbitalPlane, TargetPlaneNormal)) / 2)));
    PrevTargetDelta = 10000;
    using (new While(((OrbitDeltaV > 0.1) && (!Abort))))
    {
        OrbitalPlane = Orbit_Plane_Normal(Ascending_Node(Right_Ascension(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital())), Inclination(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()));
        OrbitDeltaV = ((2 * Vz.Length(Vz.Craft.Velocity.Orbital())) * Vz.Sin((Vz.Deg2Rad(Vz.Angle(OrbitalPlane, TargetPlaneNormal)) / 2)));
        Vz.Display(Vz.Format("Matching Inclination | Remaining: {0:n3} Degree | Delta V: {1:n2} m/s | Burn time: {2:n2} sec", "", OrbitDeltaV, OrbitBurnTime, ""), 7);
        AscendingNode = Vz.Norm(Vz.Cross(OrbitalPlane, TargetPlaneNormal));
        DescendingNode = (-1 * AscendingNode);
        Manuver_Burn(OrbitDeltaV);
        using (new If((Vz.Cross(Vz.Craft.Nav.Position(), AscendingNode).y < -0)))
        {
            Craft(Normal_Vector());
        }
        using (new Else())
        {
            Craft(Anti_Normal_Vector());
        }
        using (new If((OrbitDeltaV < 3)))
        {
            Vz.SetTimeModeAttr("Normal");
            Vz.SetInput(CraftInput.Throttle, Vz.Max((OrbitDeltaV / 200), 0.02));
        }
        using (new ElseIf((OrbitDeltaV < 100)))
        {
            Vz.SetInput(CraftInput.Throttle, Vz.Max((OrbitDeltaV / 100), 0.1));
        }
        using (new Else())
        {
            Vz.SetTimeModeAttr("FastForward");
            Vz.SetInput(CraftInput.Throttle, 1);
        }
        using (new If((OrbitDeltaV > PrevTargetDelta)))
        {
            CountDown = true;
            Vz.Break();
        }
        Vz.WaitSeconds(0);
        PrevTargetDelta = OrbitDeltaV;
    }
    Vz.SetInput(CraftInput.Throttle, -0);
});

// Orbital Parameter, sflanker's vizzy ++ style
var Inclination_Checker = Vz.DeclareCustomInstruction("Inclination Checker").SetInstructions(() =>
{
    using (new WaitUntil((Vz.LengthOf(Vz.Craft.Target.Name()) > -0))) { }
    Inclination = Inclination(Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity());
    RightAscension = Right_Ascension(Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity());
    OrbitalPlane = Orbit_Plane_Normal(Ascending_Node(RightAscension), Inclination);
    using (new If((Vz.Angle(OrbitalPlane, Orbit_Plane_Normal(Ascending_Node(Right_Ascension(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital())), Inclination(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()))) > 0.5)))
    {
        Matching_Orbit_Plane(OrbitalPlane);
    }
});

// Craft & Target Status
var Landing_Trajectory = Vz.DeclareCustomInstruction("Landing Trajectory", "position1", "position2", "CenterMass", "time").SetInstructions((position1, position2, CenterMass, time) =>
{
    // Calculating Transfer Trajectory
    Vz.SetTimeModeAttr("Normal");
    Vz.Display("Please Wait, Checking Trajectory", 7);
    DeltaTime = time;
    Mu = (G() * Vz.Planet(CenterMass).Mass());
    VectorR1 = position1;
    vectorR2 = position2;
    Lambert_s_Solver_from(VectorR1, vectorR2, DeltaTime);
});

var Heading_Align = Vz.DeclareCustomInstruction("Heading Align", "vector", "precision", "text").SetInstructions((vector, precision, text) =>
{
    using (new While(((Vz.Angle(Vz.Craft.Nav.Direction(), Vz.Norm(vector)) > precision) || (Vz.Length(Vz.Craft.Velocity.Angular()) > (Landing ? 0.1 : 0.04)))))
    {
        Vz.SetTimeModeAttr("FastForward");
        Craft(vector);
        Display = text;
        Display(Vz.Join(Display, "<br>", Vz.Format("Align: {0:n0} deg | Vel: {1:n3} rad/s", Vz.Angle(Vz.Craft.Nav.Direction(), Vz.Norm(vector)), Vz.Length(Vz.Craft.Velocity.Angular()), ""), ""));
        using (new If(Abort))
        {
            Vz.Break();
        }
        Vz.WaitSeconds(0);
    }
});

var Craft = Vz.DeclareCustomInstruction("Craft", "vector").SetInstructions((vector) =>
{
    // Orbital Orientation
    Vz.LockNavSphere(LockNavSphereIndicatorType.None);
    Vz.SetTargetHeading(TargetHeadingProperty.Pitch, (90 - Vz.Angle(Vz.Cross(Vz.Craft.Nav.North(), Vz.Craft.Nav.East()), vector)));
    Vz.SetTargetHeading(TargetHeadingProperty.Heading, Vz.Rad2Deg(Vz.Atan2(Vz.Dot(vector, Vz.Craft.Nav.East()), Vz.Dot(vector, Vz.Craft.Nav.North()))));
});

var Docking_to = Vz.DeclareCustomInstruction("Docking to", "target").SetInstructions((target) =>
{
    Vz.SetActivationGroup(10, true);
    Vz.SetActivationGroup(5, true);
    using (new While(((!Docked) && (!Abort))))
    {
        using (new If((Vz.LengthOf(Vz.Craft.Target.Name()) > 0)))
        {
            using (new If(((Vz.Angle(Target_Vector(), Docking_Port(target)) < 0.5) && Vz.Contains(Vz.Craft.Target.Name(), "Docking Port"))))
            {
                using (new If((Target_Distance() > 5)))
                {
                    Distance = Vz.Max(0, (Distance - (1 * Vz.Time.DeltaTime())));
                }
                using (new Else())
                {
                    Distance = Vz.Max(0, (Distance - (0.5 * Vz.Time.DeltaTime())));
                }
            }
            using (new Else())
            {
                using (new If((!Vz.Contains(Vz.Craft.Target.Name(), "Docking Port"))))
                {
                    Distance = 12;
                }
            }
            Craft(Target_Vector());
            CraftVelocityHeading = (2 * (Docking_Entry_Point(target, Distance) - Docking_Entry_Vector(target, Distance)));
            LateralRelativeVelocity = (Relative_Velocity(target) * 5);
            Vector = Vz.PartPciToLocal(0, (Vz.Clamp(CraftVelocityHeading, ((Target_Distance() >= 750) ? 150 : 20)) + LateralRelativeVelocity));
            Vz.SetInput(CraftInput.TranslationMode, 1);
            Vz.SetInput(CraftInput.TranslateForward, Vector.z);
            Vz.SetInput(CraftInput.TranslateRight, Vector.x);
            Vz.SetInput(CraftInput.TranslateUp, Vector.y);
        }
        using (new If((Target_Distance() >= 12.2)))
        {
            Display(Vz.Format("Rendezvous Rate X:{0:n1} | Y:{1:n1} | Z {2:n1} m/s", Relative_Velocity(target).x, Relative_Velocity(target).y, Relative_Velocity(target).z, ""));
        }
        using (new ElseIf((!Vz.Contains(Vz.Craft.Target.Name(), "Docking Port"))))
        {
            Vz.Display(Vz.Format("Please Select Docking Port!<br>Selected: {0} (<color=#dd0505>is not docking port</color>)", Vz.Craft.Target.Name(), ""), 7);
            Vz.SetActivationGroup(7, true);
        }
        using (new ElseIf((Target_Distance() > 5)))
        {
            Display(Vz.Format("Target Dist: {0:n1} m | Rel Vel: {1:n1} m/s", Target_Distance(), Vz.Length(Relative_Velocity(target)), ""));
        }
        using (new Else())
        {
            Vz.Display("Docking in progress!", 7);
        }
        using (new If((Target_Distance() >= 20)))
        {
            Vz.SetTimeModeAttr("FastForward");
        }
        using (new Else())
        {
            Vz.SetTimeModeAttr("Normal");
        }
        Vz.WaitSeconds(0);
    }
    Vz.SetInput(CraftInput.TranslationMode, 0);
    Vz.SetActivationGroup(5, true);
    Vz.SetActivationGroup(7, false);
});

using (new OnReceiveMessage("CMTargeted Landing"))
{
    Vizzy_Status(true);
    Vz.Display(Vz.Format("Target Landing: {0}", Vz.Craft.Target.Name(), ""), 7);
    using (new WaitUntil((Vz.LengthOf(Vz.Craft.Target.Name()) > -0))) { }
    Inclination = Vz.PosToLatLongAgl(Vz.Craft.Target.Position()).x;
    Node = Rotate(Vz.Norm(Y_Removal(Vz.Craft.Target.Position())), Vz.Vec(-0, 1, -0), ((Vz.Craft.Target.Position().y < -0) ? -90 : 90));
    OrbitalPlane = Orbit_Plane_Normal(Node, Inclination);
    using (new While((Vz.Angle(OrbitalPlane, Orbit_Plane_Normal(Ascending_Node(Right_Ascension(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital())), Inclination(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()))) > 0.2)))
    {
        Matching_Orbit_Plane(OrbitalPlane);
    }
    AngleNode = Rotate(Vz.Norm(Y_Removal(Vz.Craft.Target.Position())), Vz.Vec(-0, 1, -0), 90);
    VectorAngle = Vz.Angle(Vz.Craft.Nav.Position(), AngleNode);
    using (new While((VectorAngle > 0.5)))
    {
        Display(Vz.Format("Waiting Orbital Longitude {0:n0}°", VectorAngle, ""));
        VectorAngle = Vz.Angle(Vz.Norm(Rejection_of(Vz.Craft.Nav.Position(), OrbitalPlane)), AngleNode);
        using (new If((Vz.Cross(Vz.Craft.Nav.Position(), AngleNode).y > -0)))
        {
            VectorAngle = (360 - VectorAngle);
        }
        CountDown = ((VectorAngle / 360) * Vz.Craft.Orbit.Period());
        Craft((Vz.Craft.Velocity.Orbital() * -1));
        using (new If(((Vz.Round(VectorAngle) <= 1) || (Vz.Round(VectorAngle) >= 359))))
        {
            Vz.Break();
        }
        Warp(((CountDown * 2) + 3));
    }
    Vz.SetTimeModeAttr("Normal");
    Vz.Broadcast(BroadCastType.Local, "Targeted Landing", Vz.Craft.Target.Name());
}

using (new OnReceiveMessage("Moon Home Return"))
{
    // Return from the Moon
    Vizzy_Status(true);
    Launch_to_Orbit(150, -0, -0);
    Orbit_Circularization();
    Interplanetary = false;
    using (new While(true))
    {
        Vz.SetInput(CraftInput.Throttle, -0);
        Vz.LockNavSphere(LockNavSphereIndicatorType.Prograde);
        AngleNode = Rotate(Vz.Norm((Vz.Planet(Vz.Craft.Orbit.Planet()).SolarPosition() - Vz.Planet(Vz.Planet(Vz.Craft.Orbit.Planet()).Parent()).SolarPosition())), Vz.Craft.Nav.North(), -90);
        VectorAngle = Vz.Angle(Y_Removal(Vz.Norm(Vz.Craft.Nav.Position())), Y_Removal(Vz.Norm(AngleNode)));
        using (new If((Vz.Cross(Vz.Craft.Nav.Position(), AngleNode).y > -0)))
        {
            VectorAngle = (360 - VectorAngle);
        }
        CountDown = ((VectorAngle / 360) * Vz.Craft.Orbit.Period());
        Time_format(CountDown);
        Vz.Display(Vz.Format("Aligning {0} Prograde Angle | Current: {1:n0}° | Remaining: {2}:{3:00}:{4:00}", Vz.Craft.Orbit.Planet(), VectorAngle, Hours, Minutes, Seconds, ""), 7);
        using (new If(((Vz.Round(VectorAngle) <= 1) || (Vz.Round(VectorAngle) >= 359))))
        {
            Vz.Break();
        }
        Warp((CountDown - 2));
    }
    Vz.SetTimeModeAttr("Normal");
    EscapeVelocity = Vz.Sqrt(((2 * (G() * Vz.Planet(Vz.Craft.Orbit.Planet()).Mass())) / Vz.Length(Vz.Craft.Nav.Position())));
    using (new While((Vz.Length(Vz.Craft.Velocity.Orbital()) < EscapeVelocity)))
    {
        Vz.LockNavSphere(LockNavSphereIndicatorType.Prograde);
        Vz.Display(Vz.Format("Escape Burn | Delta V Remaining: {0:n1} m/s", (EscapeVelocity - Vz.Length(Vz.Craft.Velocity.Orbital())), ""), 7);
        Vz.SetInput(CraftInput.Throttle, Vz.Max(((EscapeVelocity - Vz.Length(Vz.Craft.Velocity.Orbital())) / EscapeVelocity), 0.1));
    }
    Vz.SetInput(CraftInput.Throttle, -0);
    Vz.Display(Vz.Format("Leaving {0} SOI", Vz.Craft.Orbit.Planet(), ""), 7);
    Vz.SetTimeModeAttr("FastForward");
    Vz.WaitSeconds(1);
    Vz.SetTimeModeAttr("TimeWarp5");
}

using (new OnReceiveMessage("Home Arrival"))
{
    Vz.SetTimeModeAttr("Normal");
    Vizzy_Status(true);
    Vz.Display(Vz.Format("Welcome home to {0} SOI", Vz.Craft.Orbit.Planet(), ""), 7);
    Vz.SetActivationGroup(10, true);
    Vz.WaitSeconds(1);
    OrbitApoapsis = Vz.Craft.AltitudeASL();
    using (new While((Vz.Craft.AltitudeASL() > (0.9 * OrbitApoapsis))))
    {
        Vz.SetTimeModeAttr("TimeWarp5");
        using (new If((Vz.Craft.Orbit.Periapsis() < 10)))
        {
            Craft(Vz.Craft.Nav.East());
        }
        using (new Else())
        {
            Craft(Radial_Vector());
        }
        Vz.Display("Checking Reentry Trajectory!", 7);
    }
    Vz.SetTimeModeAttr("Normal");
    // Avoiding direct impact trajectory
    using (new While((Vz.Craft.Orbit.Periapsis() < 1000)))
    {
        Craft(Vz.Craft.Nav.East());
        Vz.SetTimeModeAttr("FastForward");
        Vz.Display(Vz.Format("Aligning Reentry Trajectory | Periapsis = {0} km", (Vz.Round(Vz.Craft.Orbit.Periapsis()) / 1000), ""), 7);
        Vz.SetInput(CraftInput.Throttle, 0.15);
    }
    Vz.SetInput(CraftInput.Throttle, -0);
    // Lowering Periapsis
    Craft(Radial_Vector());
    Warp(1000);
    using (new While((Vz.Craft.Orbit.Periapsis() > 60000)))
    {
        Vz.SetTimeModeAttr("FastForward");
        Vz.Display(Vz.Format("Lowering Periapsis | Periapsis = {0} km", (Vz.Round(Vz.Craft.Orbit.Periapsis()) / 1000), ""), 7);
        Craft((Vz.Norm(Radial_Vector()) + (-1 * Vz.Norm(Vz.Craft.Velocity.Orbital()))));
        Vz.SetInput(CraftInput.Throttle, (0.01 * (Vz.Craft.Orbit.Periapsis() / 60000)));
    }
    Vz.SetInput(CraftInput.Throttle, -0);
    Vz.LockNavSphere(LockNavSphereIndicatorType.Prograde);
    using (new While((Vz.Craft.AltitudeASL() > 300000)))
    {
        Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
        Time_format(((Vz.Craft.Orbit.Periapsis() <= -0) ? Time_until_Surface__with_Alt(50000) : Vz.Craft.Orbit.TimeToPe()));
        Vz.Display(Vz.Format("Arrival = {0}:{1:00}:{2:00} remaining", Hours, Minutes, Seconds, ""), 7);
        Warp(((Vz.Craft.Orbit.Periapsis() <= -0) ? Time_until_Surface__with_Alt(50000) : Vz.Craft.Orbit.TimeToPe()));
    }
    using (new While((Vz.Length(Vz.Craft.Velocity.Surface()) > 2000)))
    {
        Vz.SetTimeModeAttr("FastForward");
        Vz.Display(Vz.Format("Retro Burn | Delta V = {0} m/s", Vz.Round((Vz.Length(Vz.Craft.Velocity.Surface()) - 1500)), ""), 7);
        Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
        Vz.SetInput(CraftInput.Throttle, 1);
        using (new If((Vz.Craft.Fuel.FuelInStage() == -0)))
        {
            Vz.Break();
        }
    }
    Vz.SetInput(CraftInput.Throttle, 0);
    Vz.SetTimeModeAttr("Normal");
    Vz.Display("CM & SM separation !", 7);
    using (new WaitUntil((Vz.Craft.Performance.CurrentThrust() < 5))) { }
    Vz.WaitSeconds(0.5);
    Vz.ActivateStage();
    Vz.WaitSeconds(1);
    using (new While((Vz.Craft.AltitudeAGL() > 50)))
    {
        using (new While((Vz.Craft.AltitudeAGL() > 1000)))
        {
            Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
            Vz.SetTimeModeAttr("FastForward");
            using (new While((Vz.Craft.AltitudeASL() > 10000)))
            {
                Time_format(Impact_Time());
                Vz.SetInput(CraftInput.Throttle, 0);
                Vz.Display(Vz.Format("Atmospher Entry | time to impact = {0}:{1:00}:{2:00}", Hours, Minutes, Seconds, ""), 7);
                Reentry_Time_Wrap();
            }
            Time_format(Impact_Time());
            Vz.Display(Vz.Format("Aerobraking | time to impact = {0}:{1:00}:{2:00}", Hours, Minutes, Seconds, ""), 7);
        }
        Vz.SetTimeModeAttr("FastForward");
        Vz.Display(Vz.Format("Parachute | time to impact = {0}:{1:00}:{2:00}", Hours, Minutes, Seconds, ""), 7);
        Vz.LockNavSphere(LockNavSphereIndicatorType.None);
        Vz.SetActivationGroup(10, false);
        Time_format(Impact_Time());
        using (new If(Vz.Craft.Grounded()))
        {
            Vz.Break();
        }
    }
    using (new While((Vz.Craft.AltitudeAGL() > 0)))
    {
        // Simple Aggresive Suicide Burn
        Time_format(Time_until_Surface__with_Alt(3));
        Vz.Display(Vz.Format("Impact = {0}:{1:00}:{2:00} sec", Hours, Minutes, Seconds, ""), 7);
        using (new If((Vz.Craft.AltitudeAGL() < 100)))
        {
            Vz.SetTimeModeAttr("Normal");
            Vz.SetTargetHeading(TargetHeadingProperty.Pitch, 90);
            Vz.SetActivationGroup(9, true);
        }
        using (new Else())
        {
            Vz.SetTimeModeAttr("FastForward");
            Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
        }
        using (new If(Vz.Craft.Grounded()))
        {
            Vz.Break();
        }
        using (new If((Vz.Craft.SolarRadiation() == -0)))
        {
            Vz.SetActivationGroup(7, true);
        }
    }
    Vz.SetInput(CraftInput.Throttle, -0);
    Vz.SetActivationGroup(10, false);
    Vz.Display("Touch Down !", 7);
    Vz.WaitSeconds(2);
    Vz.Display("Thanks for your support !", 7);
    Vz.WaitSeconds(2);
    Vizzy_Status(false);
    Vz.Break();
}

using (new On("PartCollision")) // [event: PartCollision]
{
    using (new If(((Vz.Distance(Vz.Craft.Nav.Position(), Vz.Craft.Target.Position()) < 20) && Vz.Contains(Vz.Craft.Target.Name(), "Docking Port"))))
    {
        Vz.SetActivationGroup(5, false);
        Vz.SetActivationGroup(10, false);
    }
}

using (new OnReceiveMessage("Abort"))
{
    // I will add abort system for the next update
    Abort = true;
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Abort Indicator"), true);
    Vz.WaitSeconds(1);
    Vizzy_Status(false);
    Vz.WaitSeconds(2);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Abort Indicator"), false);
    Abort = false;
}

using (new OnReceiveMessage("Docking"))
{
    using (new WaitUntil((Vz.LengthOf(Vz.Craft.Target.Name()) > -0))) { }
    Vizzy_Status(true);
    using (new If((Vz.Craft.NameToID(Vz.Craft.Target.Name()) == -1)))
    {
        Original_Target_Name(Vz.Craft.Target.Name());
        Vz.TargetNode(Craft);
        Docking_to(Craft);
    }
    using (new Else())
    {
        Docking_to(Vz.Craft.Target.Name());
    }
    Vizzy_Status(false);
}

using (new OnReceiveMessage("Rendezvous"))
{
    Vizzy_Status(true);
    Interplanetary = false;
    Inclination_Checker();
    Orbit_Transfer_to(data);
    Rendezvous_to(data);
    Docking_to(data);
    Vizzy_Status(false);
}

using (new OnReceiveMessage("CMDocking"))
{
    using (new If((Vz.Distance(Vz.Craft.Nav.Position(), Vz.Craft.Target.Position()) < 5000)))
    {
        Vz.Broadcast(BroadCastType.Local, "Docking", -0);
    }
    using (new Else())
    {
        Vz.Broadcast(BroadCastType.Local, "Rendezvous", Vz.Craft.Target.Name());
    }
}

using (new OnReceiveMessage("CMLaunch"))
{
    using (new If(Vz.Craft.Grounded()))
    {
        Vz.Broadcast(BroadCastType.Local, "Launch", Vz.Craft.Target.Name());
    }
    using (new Else())
    {
        Vz.Broadcast(BroadCastType.Local, "Rendezvous", Vz.Craft.Target.Name());
    }
}

using (new OnReceiveMessage("CMRendezvous"))
{
    using (new If(Vz.Craft.Grounded()))
    {
        Vz.Broadcast(BroadCastType.Local, "Launch", Vz.Craft.Target.Name());
    }
    using (new Else())
    {
        Vz.Broadcast(BroadCastType.Local, "Rendezvous", Vz.Craft.Target.Name());
    }
}

var Original_Craft_Name = Vz.DeclareCustomInstruction("Original Craft Name").SetInstructions(() =>
{
    Vz.ListClear(Letter_Dump);
    Vz.ListClear(Craft_Name);
    TargetName = target;
    using (new If((Vz.ListLength(Craft_Name) == -0)))
    {
        using (new For("i").From(1).To(Vz.LengthOf(TargetName)).By(1))
        {
            Vz.ListAdd(Craft_Name, Vz.LetterOf(i, TargetName));
        }
    }
    using (new If((Vz.ListLength(Letter_Dump) == -0)))
    {
        using (new For("i").From(1).To(Vz.ListLength(Craft_Name)).By(1))
        {
            using (new If(Vz.Contains(Vz.ListGet(Craft_Name, i), "-")))
            {
                Vz.ListAdd(Letter_Dump, i);
            }
        }
    }
    Craft = Vz.SubString(1, (Vz.ListGet(Letter_Dump, 1) - 1), TargetName);
});

var Orbital_Manuever_Burn = Vz.DeclareCustomInstruction("Orbital Manuever Burn", "text").SetInstructions((text) =>
{
    // Calculating Manuever Burn
    PotentialEnergy = (-1 * (Mu() / Vz.Length(VectorR1)));
    TargetOrbitEnergy = Specific_Orbital_Energy(vectorR2, vectorV2);
    OrbitDeltaV = Target_DeltaV__to(TargetOrbitEnergy, PotentialEnergy, Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    Vz.SetTimeModeAttr("FastForward");
    Manuver_Burn(OrbitDeltaV);
    // Vector Heading
    Heading_Align((VectorV1 - Vz.Craft.Velocity.Orbital()), (Landing ? 12 : 2), Vz.Format("Preparing {0} | DeltaV: {1:n2} m/s | : {2:n2} sec", text, OrbitDeltaV, TransferPeriod, ""));
    PrevTargetDelta = 100000;
    TargetDelta = (VectorV1 - Vz.Craft.Velocity.Orbital());
    using (new If((TargetOrbitEnergy > Specific_Orbital_Energy(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()))))
    {
        UpHill = true;
    }
    using (new If((TargetOrbitEnergy < Specific_Orbital_Energy(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()))))
    {
        UpHill = false;
    }
    using (new While(((UpHill == true) ? Uphill_Orbit() : Downhill_Orbit())))
    {
        OrbitDeltaV = ((UpHill == true) ? Uphill_Delta_V() : Downhill_Delta_V());
        Display = Vz.Format("Wait until Delta V is <color=#05DD05>ZERO</color> || {0} || DeltaV Remaining: {1:n2} m/s |", text, OrbitDeltaV, "");
        Display(Vz.Join(Display, Vz.Format("| Elapsed: {0:n2} sec", (OrbitBurnTime / Vz.CraftInput(CraftInput.Throttle)), ""), ""));
        Manuver_Burn(OrbitDeltaV);
        using (new If(((OrbitDeltaV < 5) && (UpHill == true))))
        {
            Vz.SetTimeModeAttr("Normal");
            Vz.SetInput(CraftInput.Throttle, Vz.Max((OrbitDeltaV / 50), 0.025));
            Craft(Vz.Norm(VectorV1));
        }
        using (new ElseIf(((OrbitDeltaV < 5) && (UpHill == false))))
        {
            using (new If((OrbitDeltaV < 2)))
            {
                Vz.SetTimeModeAttr("Normal");
                Craft(Vz.Norm(VectorV1));
            }
            using (new Else())
            {
                Vz.SetTimeModeAttr("FastForward");
                Craft((VectorV1 - Vz.Craft.Velocity.Orbital()));
            }
            Vz.SetInput(CraftInput.Throttle, Vz.Max((OrbitDeltaV / 50), 0.025));
        }
        using (new ElseIf((OrbitBurnTime < 0.7)))
        {
            Vz.SetTimeModeAttr("FastForward");
            Vz.SetInput(CraftInput.Throttle, Vz.Max((Vz.Log10(((OrbitBurnTime * 4.5) + 1)) * 2), 0.05));
            using (new If(Landing))
            {
                Craft((VectorV1 - Vz.Craft.Velocity.Orbital()));
            }
            using (new Else())
            {
                Craft(((Vz.Angle(TargetDelta, (VectorV1 - Vz.Craft.Velocity.Orbital())) > 30) ? VectorV1 : (VectorV1 - Vz.Craft.Velocity.Orbital())));
            }
        }
        using (new Else())
        {
            Vz.SetTimeModeAttr("FastForward");
            Vz.SetInput(CraftInput.Throttle, Vz.Min(1, (OrbitDeltaV * 0.1)));
            Craft(((Vz.Angle(TargetDelta, (VectorV1 - Vz.Craft.Velocity.Orbital())) > 30) ? VectorV1 : (VectorV1 - Vz.Craft.Velocity.Orbital())));
        }
        using (new If((((OrbitDeltaV < 5) && (Landing == true)) || Abort)))
        {
            Vz.Break();
        }
        using (new If(((OrbitDeltaV < 3) && Vz.Contains(Target_Status(Vz.Craft.Target.Name()), "Craft"))))
        {
            Vz.Break();
        }
        using (new If((OrbitDeltaV > PrevTargetDelta)))
        {
            Vz.Break();
        }
        Vz.WaitSeconds(0);
        PrevTargetDelta = OrbitDeltaV;
    }
    Vz.SetInput(CraftInput.Throttle, -0);
});

var Orbit_Transfer_to = Vz.DeclareCustomInstruction("Orbit Transfer to", "target").SetInstructions((target) =>
{
    // Simple Same Parent Orbit Transfer
    using (new If((!Abort)))
    {
        Vz.TargetNode(target);
        using (new WaitUntil((Vz.LengthOf(Vz.Craft.Target.Name()) > 0))) { }
        Phase_Angle_from(Vz.Craft.Nav.Position(), Vz.Craft.Target.Position(), Vz.Craft.Orbit.Planet());
        Hohmann_Parameter_from(Vz.Craft.Nav.Position(), Vz.Craft.Target.Position(), Vz.Craft.Orbit.Planet());
        Trajectory_Calculation_from(Vz.Craft.Nav.Position(), Vz.Craft.Target.Position(), Vz.Craft.Orbit.Planet(), (0.9 * TransferPeriod));
        Heading_Align((VectorV1 - Vz.Craft.Velocity.Orbital()), 2, "Preparing Heading");
        Trajectory_Calculation_from(Vz.Craft.Nav.Position(), Vz.Craft.Target.Position(), Vz.Craft.Orbit.Planet(), (0.9 * TransferPeriod));
        Text = Vz.Format("{0} Injection Burn", Vz.Craft.Target.Name(), "");
        Orbital_Manuever_Burn(Text);
    }
});

using (new OnReceiveMessage("Launch"))
{
    Vizzy_Status(true);
    Inclination = Inclination(Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity());
    RightAscension = Right_Ascension(Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity());
    Launch_to_Orbit(((((0.001 * Altitude(Vz.Craft.Target.Position())) < 160) ? 0.00065 : 0.0007) * Altitude(Vz.Craft.Target.Position())), Inclination, RightAscension);
    using (new If((!Abort)))
    {
        Orbit_Circularization();
        using (new WaitUntil((Vz.Craft.Performance.CurrentThrust() == -0))) { }
        Vz.SetTimeModeAttr("Normal");
        Vz.SetActivationGroup(2, true);
        Vz.WaitSeconds(1);
        Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("SM Engine"), true);
        Interplanetary = false;
        Vz.TargetNode(data);
        Inclination_Checker();
        Orbit_Transfer_to(data);
        Rendezvous_to(data);
        Docking_to(data);
        Vizzy_Status(false);
    }
}

Vz.TargetNode("name");
var Time_Wrap_in = Vz.DeclareCustomInstruction("Time Wrap in", "DeltaT", "time", "distance", "vector").SetInstructions((DeltaT, time, distance, vector) =>
{
    time = DeltaT;
    Vz.Display("Please Wait", 7);
    Vz.SetTimeModeAttr("FastForward");
    Vz.WaitSeconds(3);
    OrbitalPlane = Orbit_Plane_Normal(Ascending_Node(Right_Ascension(Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity())), Inclination(Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity()));
    using (new While((((time - Vz.Time.MissionTime()) > time) && (!Abort))))
    {
        Time_format(((time - Vz.Time.MissionTime()) - time));
        Display(Vz.Format("Approaching | Longitude: {0:n0}° | Dist: {1:n0} km | {2:00}:{3:00}:{4:00} sec", Vz.Angle(Vz.Craft.Nav.Position(), Vz.Craft.Target.Position()), (Target_Distance() / 1000), Hours, Minutes, Seconds, ""));
        Craft(((vector == 1) ? (Vz.Craft.Velocity.Orbital() * -1) : ((vector == 2) ? Relative_Velocity(Vz.Craft.Target.Name()) : ((vector == 3) ? Target_Vector() : Vz.Craft.Velocity.Orbital()))));
        using (new If((((time - Vz.Time.MissionTime()) < time) || (Target_Distance() < distance))))
        {
            Vz.Break();
        }
        using (new If((Vz.Angle(Rejection_of(Vz.Craft.Nav.Position(), OrbitalPlane), Rejection_of(Vz.Craft.Target.Position(), OrbitalPlane)) < 0.05)))
        {
            Vz.Break();
        }
        Warp((((Parent_Star() == -0) ? 2 : 1) * (time - Vz.Time.MissionTime())));
        Vz.SetActivationGroup(10, false);
    }
    Vz.SetActivationGroup(10, true);
});

var Roll_Control = Vz.DeclareCustomInstruction("Roll Control").SetInstructions(() =>
{
    // Auto Roll Control, Roll Bug on 0.9.404
    Vz.SetInput(CraftInput.Roll, ((Vz.Craft.Nav.BankAngle() / -90) * 0.3));
});

using (new OnReceiveMessage("Target Orbit Insertion"))
{
    Vz.Display("Preparing Orbit Insertion", 7);
    using (new While((Vz.Craft.Orbit.TimeToPe() > 120)))
    {
        Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
        Time_format(Vz.Craft.Orbit.TimeToPe());
        Vz.Display(Vz.Format("Time to Periapsis = {0}:{1:00}:{2:00} remaining", Hours, Minutes, Seconds, ""), 7);
        Warp(Vz.Craft.Orbit.TimeToPe());
    }
    OrbitPeriapsis = Vz.Craft.Orbit.Periapsis();
    using (new While((Vz.Craft.Orbit.Periapsis() > Target_Planet_Orbit())))
    {
        Vz.SetTimeModeAttr("FastForward");
        Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
        using (new If(((Vz.Craft.Orbit.Apoapsis() - 100000) > OrbitPeriapsis)))
        {
            Vz.Display(Vz.Format("Retrogade Burn, Apoapsis = {0:n1} km", Vz.Round((Vz.Craft.Orbit.Apoapsis() / 1000)), ""), 7);
        }
        using (new ElseIf((OrbitPeriapsis > Vz.Craft.Orbit.Periapsis())))
        {
            Vz.Display(Vz.Format("Retrogade Burn, Periapsis = {0:n1} km", Vz.Round((Vz.Craft.Orbit.Periapsis() / 1000)), ""), 7);
        }
        Vz.SetInput(CraftInput.Throttle, ((Vz.Craft.Orbit.Eccentricity() > 0.9) ? 1 : Vz.Min((0.03 * (Vz.Craft.Orbit.Apoapsis() / Target_Planet_Orbit())), 1)));
    }
    Vz.Display("Waiting Circularization", 7);
    Vz.SetInput(CraftInput.Throttle, 0);
    using (new While((Vz.Rad2Deg(Vz.Craft.Orbit.Inclination()) > 0.05)))
    {
        Matching_Orbit_Plane(Vz.Vec(-0, 1, -0));
    }
    Vz.SetActivationGroup(10, false);
    using (new While((Vz.Craft.Orbit.TimeToAp() > 120)))
    {
        Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
        Time_format(Vz.Craft.Orbit.TimeToAp());
        Vz.Display(Vz.Format("Time to Apoapsis = {0}:{1:00}:{2:00} remaining", Hours, Minutes, Seconds, ""), 7);
        Warp(Vz.Craft.Orbit.TimeToAp());
    }
    OrbitPeriapsis = Vz.Craft.Orbit.Periapsis();
    using (new While((Vz.Craft.Orbit.Periapsis() > Target_Planet_Orbit())))
    {
        Vz.SetTimeModeAttr("FastForward");
        Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
        Vz.Display(Vz.Format("Retrogade Burn, Periapsis = {0:n1} km", Vz.Round((Vz.Craft.Orbit.Periapsis() / 1000)), ""), 7);
        Vz.SetInput(CraftInput.Throttle, ((Vz.Craft.Orbit.Eccentricity() > 0.9) ? 0.5 : Vz.Max(0.01, ((Vz.Craft.Orbit.Periapsis() - Target_Planet_Orbit()) / Target_Planet_Orbit()))));
    }
    Vz.Display("Waiting Circularization", 7);
    Vz.SetInput(CraftInput.Throttle, 0);
    Warp(50);
    using (new While((Vz.Craft.Orbit.TimeToPe() > 3)))
    {
        Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
        Time_format(Vz.Craft.Orbit.TimeToPe());
        Warp((Vz.Craft.Orbit.TimeToPe() + 5));
        Vz.Display(Vz.Format("Time to Periapsis = {0}:{1:00}:{2:00} remaining", Hours, Minutes, Seconds, ""), 7);
    }
    OrbitApoapsis = Vz.Craft.Orbit.Apoapsis();
    Vz.SetActivationGroup(10, true);
    using (new While(((Vz.Craft.Orbit.Apoapsis() - Vz.Craft.Orbit.Periapsis()) > 2000)))
    {
        Vz.SetTimeModeAttr("Normal");
        Vz.Display(Vz.Format("Circularization, Eccentricity = {0}", (Vz.Round((Vz.Craft.Orbit.Eccentricity() * 1000)) / 1000), ""), 7);
        Vz.SetInput(CraftInput.Throttle, (0.25 + ((Vz.Craft.Orbit.Apoapsis() - Vz.Craft.Orbit.Periapsis()) / OrbitApoapsis)));
        Vz.SetTargetHeading(TargetHeadingProperty.Pitch, (((Vz.Craft.Orbit.TimeToPe() / Vz.Craft.Orbit.Period()) * 360) - 1));
        using (new If((Vz.Craft.Orbit.Eccentricity() < 0.005)))
        {
            Vz.Break();
        }
    }
    Vz.SetInput(CraftInput.Throttle, 0);
    Vizzy_Status(false);
    Vz.Broadcast(BroadCastType.Local, "Deorbit Menu", -0);
}

using (new OnDocked())
{
    Docked = true;
    Vz.Display("Docked !", 7);
    Vz.WaitSeconds(3);
    Docked = false;
}

using (new OnReceiveMessage("Juno SOI"))
{
    Vz.SetTimeModeAttr("Normal");
    Vz.Display(Vz.Format("Welcome to {0} SOI !", Vz.Craft.Orbit.Planet(), ""), 7);
    Vz.WaitSeconds(1);
    Vz.LockNavSphere(LockNavSphereIndicatorType.Prograde);
    Vz.Display("Please Wait", 7);
    Trajectory_Correction_Burn(Vz.ListGet(Mission, 2), TransferPeriod);
}

using (new OnReceiveMessage("Target Planet SOI"))
{
    Vz.SetTimeModeAttr("Normal");
    Vz.Display(Vz.Format("Entering {0} SOI", Vz.Craft.Orbit.Planet(), ""), 7);
    Vz.SetActivationGroup(10, true);
    Vz.WaitSeconds(1);
    OrbitApoapsis = Vz.Craft.AltitudeASL();
    using (new While(((Vz.Craft.Orbit.TimeToPe() > 1200) && (Vz.Craft.AltitudeASL() > (0.7 * OrbitApoapsis)))))
    {
        Vz.SetTimeModeAttr("TimeWarp5");
        using (new If((IsNan(Vz.Craft.Orbit.Periapsis()) || (Vz.Craft.Orbit.Periapsis() == -0))))
        {
            Craft(Vz.Craft.Nav.East());
        }
        using (new Else())
        {
            Craft(Radial_Vector());
        }
        Vz.Display("Checking Desired Trajectory!", 7);
    }
    Vz.SetTimeModeAttr("Normal");
    // Lowering Periapsis
    Craft(Radial_Vector());
    using (new While((Vz.Craft.Orbit.Periapsis() > 0)))
    {
        Vz.SetTimeModeAttr("FastForward");
        Vz.Display(Vz.Format("Reorienting Orbit Direction | Periapsis = {0} km", (Vz.Round(Vz.Craft.Orbit.Periapsis()) / 1000), ""), 7);
        Craft(Radial_Vector());
        Vz.SetInput(CraftInput.Throttle, Vz.Max(0.1, (0.05 * (Vz.Craft.Orbit.Periapsis() / (3 * Target_Planet_Orbit())))));
        using (new If(IsNan(Vz.Craft.Orbit.Periapsis())))
        {
            Vz.Break();
        }
    }
    Vz.SetInput(CraftInput.Throttle, -0);
    // Avoiding direct impact trajectory
    Craft(Vz.Craft.Nav.East());
    Warp(1000);
    using (new While((IsNan(Vz.Craft.Orbit.Periapsis()) || (Vz.Craft.Orbit.Periapsis() < (Target_Planet_Orbit() * 2)))))
    {
        Craft(Vz.Craft.Nav.East());
        Vz.SetTimeModeAttr("FastForward");
        Vz.Display(Vz.Format("Raising Periapsis | Periapsis = {0} km", (Vz.Round(Vz.Craft.Orbit.Periapsis()) / 1000), ""), 7);
        Vz.SetInput(CraftInput.Throttle, Vz.Max((((2 * Target_Planet_Orbit()) - Vz.Craft.Orbit.Periapsis()) / (4 * Target_Planet_Orbit())), 0.01));
    }
    Vz.SetInput(CraftInput.Throttle, -0);
    Vz.Broadcast(BroadCastType.Local, "Target Orbit Insertion", -0);
}

var Landing_PID__P = Vz.DeclareCustomInstruction("Landing PID, P", "KP", "KI", "KD", "x", "y").SetInstructions((KP, KI, KD, x, y) =>
{
    TargetNorthDistance = (Vz.Dot((Vz.Craft.Target.Position() - Vz.Craft.Nav.Position()), Vz.Craft.Nav.North()) + y);
    TargetEastDistance = (Vz.Dot((Vz.Craft.Target.Position() - Vz.Craft.Nav.Position()), Vz.Craft.Nav.East()) + x);
    CraftNorthVelocity = Vz.Dot(Vz.Craft.Velocity.Surface(), Vz.Craft.Nav.North());
    CraftEastVelocity = Vz.Dot(Vz.Craft.Velocity.Surface(), Vz.Craft.Nav.East());
    Distance = Vz.Vec(TargetEastDistance, TargetNorthDistance, 0);
    LateralRelativeVelocity = Vz.Vec(CraftEastVelocity, CraftNorthVelocity, -0);
    NorthTargetVelDelta = (((KP * TargetNorthDistance) + (((TargetNorthDistance * PrevNorthDistance) < -0) ? -0 : (KI * TargetNorthDistance))) - (KD * CraftNorthVelocity));
    EastTargetVelDelta = (((KP * TargetEastDistance) + (((TargetEastDistance * PrevEastDistance) < -0) ? -0 : (KI * TargetEastDistance))) - (KD * CraftEastVelocity));
    TargetHeading = Vz.Vec(EastTargetVelDelta, NorthTargetVelDelta, 0);
    PitchProportional = Vz.Min(5, (((Vz.Length(LateralRelativeVelocity) < 20) ? 0.01 : 0.05) * Vz.Length(TargetHeading)));
    Pitch = Vz.Max(((Vz.Craft.AltitudeAGL() < 3000) ? 75 : 55), Vz.Min(90, Vz.Rad2Deg(Vz.Acos(Vz.Min(1, Vz.Max(-0, ((PitchProportional * Vz.Craft.Performance.Mass()) / (Vz.Craft.Performance.MaxThrust() * Throttle))))))));
    Heading = ((360 + Vz.Rad2Deg(Vz.Atan2(EastTargetVelDelta, NorthTargetVelDelta))) % 360);
    Vz.SetTargetHeading(TargetHeadingProperty.Pitch, Pitch);
    Vz.SetTargetHeading(TargetHeadingProperty.Heading, Heading);
    Vz.WaitSeconds(0);
    PrevNorthDistance = TargetNorthDistance;
    PrevEastDistance = TargetEastDistance;
});

using (new OnReceiveMessage("Simple Deorbit"))
{
    // Blind Landing
    Vz.SetActivationGroup(10, true);
    OrbitVelocity = Vz.Length(Vz.Craft.Velocity.Surface());
    using (new While((Vz.Craft.Orbit.Periapsis() > -0)))
    {
        using (new While((Vz.Length(Vz.Craft.Velocity.Surface()) > (0.6 * OrbitVelocity))))
        {
            Vz.Display(Vz.Format("No Target Selected | Retro Burn, Delta V = {0} m/s", Vz.Round((Vz.Length(Vz.Craft.Velocity.Surface()) - (0.6 * OrbitVelocity))), ""), 7);
            Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
            Vz.SetInput(CraftInput.Throttle, 1);
        }
        Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
        Vz.SetInput(CraftInput.Throttle, 1);
    }
    Vz.SetInput(CraftInput.Throttle, 0);
    using (new If((Vz.PartNameToID("Second Stage") > -0)))
    {
        using (new WaitUntil((Vz.Craft.Performance.CurrentThrust() == -0))) { }
        Vz.ActivateStage();
    }
    using (new While(((Vz.Craft.AltitudeAGL() - 3000) > (0.6 * Suicide_Burn_Trigger()))))
    {
        using (new While((Vz.Craft.AltitudeASL() > 40000)))
        {
            Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
            Time_format(Impact_Time());
            Vz.Display(Vz.Format("Re Entry | time to impact = {0}:{1:00}:{2:00}", Hours, Minutes, Seconds, ""), 7);
            Vz.SetInput(CraftInput.Throttle, 0);
            Reentry_Time_Wrap();
            Roll_Control();
        }
        Time_format(Impact_Time());
        Vz.Display(((Vz.Craft.Atmosphere.AirDensity() == 0) ? Vz.Format("Waiting Suicide Burn | time to impact = {0}:{1:00}:{2:00}", Hours, Minutes, Seconds, "") : Vz.Format("Aerobraking | time to impact = {0}:{1:00}:{2:00}", Hours, Minutes, Seconds, "")), 7);
        Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
    }
    using (new While((Vz.Craft.AltitudeAGL() > 0)))
    {
        // Simple Aggresive Suicide Burn
        Time_format(Impact_Time());
        Vz.Display(Vz.Format("Suicide Burn | time to impact = {0}:{1:00}:{2:00}", Hours, Minutes, Seconds, ""), 7);
        Suicide_Burn();
        using (new If(((Vz.Abs(Vz.Craft.Velocity.LateralSurface()) < 10) || (Vz.Craft.AltitudeAGL() < 100))))
        {
            Vz.SetTargetHeading(TargetHeadingProperty.Pitch, 90);
        }
        using (new If(((Vz.Abs(Vz.Craft.Velocity.LateralSurface()) > 10) && (Vz.Craft.AltitudeAGL() >= 100))))
        {
            Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
        }
        using (new If((Vz.Craft.AltitudeAGL() < 2000)))
        {
            Vz.SetTimeModeAttr("Normal");
            Vz.SetActivationGroup(9, true);
        }
        using (new If(Vz.Craft.Grounded()))
        {
            Vz.Break();
        }
    }
    Vz.SetInput(CraftInput.Throttle, -0);
    Vz.SetActivationGroup(10, false);
    Vz.Display("Touch Down !", 7);
    Vz.Broadcast(BroadCastType.Craft, "Target Planet Landed", -0);
}

var Boot_Start = Vz.DeclareCustomInstruction("Boot Start").SetInstructions(() =>
{
    using (new WaitUntil((Vz.LengthOf(Vz.Craft.Orbit.Planet()) > -0))) { }
    using (new If((Vz.PartNameToID("Side Booster") > 0)))
    {
        using (new If(Vz.Contains(Orbit_Status(), "Planet")))
        {
            using (new If((Vz.Craft.Grounded() || (Vz.Craft.AltitudeAGL() < 100))))
            {
                Vz.Broadcast(BroadCastType.Local, "Launch Menu", -0);
            }
            using (new ElseIf(((Vz.Craft.Orbit.Eccentricity() <= 0.1) && (Vz.Craft.AltitudeASL() > 60000))))
            {
                Vz.Broadcast(BroadCastType.Local, "Home Deorbit", -0);
            }
        }
    }
    using (new ElseIf(Vz.Contains(Orbit_Status(), "Star")))
    {
        Vz.Broadcast(BroadCastType.Local, "Juno SOI", -0);
    }
    using (new ElseIf((Vz.Craft.Grounded() || (Vz.Craft.AltitudeAGL() < 100))))
    {
        Vz.Broadcast(BroadCastType.Local, "Target Planet Landed", -0);
    }
    using (new ElseIf(((Vz.Craft.Orbit.Eccentricity() <= 0.1) && (Vz.Craft.AltitudeASL() > 60000))))
    {
        Vz.Broadcast(BroadCastType.Local, "Deorbit Menu", -0);
    }
    using (new ElseIf(((Vz.Craft.Orbit.Eccentricity() > 0.1) && (Vz.Craft.Orbit.Eccentricity() <= 0.85))))
    {
        Vz.Broadcast(BroadCastType.Local, "Orbit Insertion", -0);
    }
    using (new ElseIf((Vz.Craft.Orbit.Eccentricity() > 0.85)))
    {
        Vz.Broadcast(BroadCastType.Local, "Planet SOI", -0);
    }
});

using (new OnReceiveMessage("Launch Menu"))
{
    using (new While((Vz.ActivationGroup(1) == false)))
    {
        using (new If((Vz.LengthOf(Vz.Craft.Target.Name()) > -0)))
        {
            Vz.Display(Vz.Format("Selected: <color=#AAFF00>{0}</color><br>Press AG.1 to Start!", Vz.Craft.Target.Name(), ""), 7);
        }
        using (new Else())
        {
            Vz.Display("Select your target<br>Then Press AG.1 to Start!", 7);
        }
        Vz.SetActivationGroup(2, false);
        Vz.SetActivationGroup(3, false);
        Vz.WaitSeconds(0);
    }
    Vz.WaitSeconds(0.1);
    using (new If((Vz.LengthOf(Vz.Craft.Target.Name()) > -0)))
    {
        using (new If(Vz.Contains(Target_Status(Vz.Craft.Target.Name()), "Moon")))
        {
            Vz.Broadcast(BroadCastType.Local, "Launch to the Moon", Vz.Craft.Target.Name());
        }
        using (new ElseIf(Vz.Contains(Target_Status(Vz.Craft.Target.Name()), "Planet")))
        {
            Vz.Broadcast(BroadCastType.Local, "Launch to Target Planet", Vz.Craft.Target.Name());
        }
        using (new Else())
        {
            Vz.Broadcast(BroadCastType.Local, "Launch", -0);
        }
    }
    using (new Else())
    {
        Vz.Broadcast(BroadCastType.Local, "CMLaunch to Orbit", -0);
    }
}

using (new OnReceiveMessage("Side Booster"))
{
    using (new While((Vz.PartNameToID("Side Booster") > -0)))
    {
        using (new If((Vz.RawXmlCraftProperty(@"<CraftProperty property=""Part.Mass"" style=""part""><CraftProperty property=""Part.NameToID"" style=""part-id""><Constant text=""Side Booster"" /></CraftProperty></CraftProperty>") < 760)))
        {
            Vz.WaitSeconds(0);
            Vz.ActivateStage();
            Vz.Break();
        }
        Vz.WaitSeconds(0);
    }
}

using (new OnReceiveMessage("Targeted Landing"))
{
    Vz.SetActivationGroup(10, true);
    TransferPeriod = (0.225 * Vz.Craft.Orbit.Period());
    Distance = Vz.ToPosition(Vz.Vec(Vz.PosToLatLongAgl(Vz.Craft.Target.Position()).x, Vz.PosToLatLongAgl(Vz.Craft.Target.Position()).y, 25000));
    Landing = true;
    Landing_Trajectory(Vz.Craft.Nav.Position(), Distance, Vz.Craft.Orbit.Planet(), TransferPeriod);
    Text = Vz.Format("Lowering Periapsis", "");
    Orbital_Manuever_Burn(Text);
    using (new If((Vz.PartNameToID("Second Stage") > -0)))
    {
        using (new WaitUntil((Vz.Craft.Performance.CurrentThrust() == -0))) { }
        Vz.ActivateStage();
    }
    Time_Wrap_in(((TransferPeriod * 0.85) + Vz.Time.MissionTime()), 20, 20000, 1);
    TransferPeriod = (TransferPeriod * 0.3);
    Distance = Vz.ToPosition(Vz.Vec(Vz.PosToLatLongAgl(Vz.Craft.Target.Position()).x, Vz.PosToLatLongAgl(Vz.Craft.Target.Position()).y, 15000));
    Landing_Trajectory(Vz.Craft.Nav.Position(), Distance, Vz.Craft.Orbit.Planet(), TransferPeriod);
    Text = Vz.Format("Retrogade Burn", "");
    Orbital_Manuever_Burn(Text);
    Time_Wrap_in(((TransferPeriod * 0.8) + Vz.Time.MissionTime()), 20, 15000, 1);
    TransferPeriod = Time_until_Surface__with_Alt(1000);
    Distance = Vz.ToPosition(Vz.Vec(Vz.PosToLatLongAgl(Vz.Craft.Target.Position()).x, Vz.PosToLatLongAgl(Vz.Craft.Target.Position()).y, 2000));
    Landing_Trajectory(Vz.Craft.Nav.Position(), Distance, Vz.Craft.Orbit.Planet(), TransferPeriod);
    Text = Vz.Format("Retrogade Burn", "");
    Orbital_Manuever_Burn(Text);
    TransferPeriod = Time_until_Surface__with_Alt(100);
    Distance = Vz.ToPosition(Vz.Vec(Vz.PosToLatLongAgl(Vz.Craft.Target.Position()).x, Vz.PosToLatLongAgl(Vz.Craft.Target.Position()).y, 1000));
    Landing_Trajectory(Vz.Craft.Nav.Position(), Distance, Vz.Craft.Orbit.Planet(), TransferPeriod);
    Text = Vz.Format("Retrogade Burn", "");
    Orbital_Manuever_Burn(Text);
    using (new While(((Vz.Craft.AltitudeAGL() > 12000) && ((Vz.Craft.AltitudeAGL() - 3000) > Suicide_Burn_Trigger()))))
    {
        using (new If((Vz.Craft.AltitudeASL() > 60000)))
        {
            Reentry_Time_Wrap();
        }
        using (new Else())
        {
            Vz.SetTimeModeAttr("FastForward");
        }
        Vz.LockNavSphere(LockNavSphereIndicatorType.Retrograde);
        Time_format(Time_until_Surface__with_Alt(100));
        Vz.Display(((Vz.Craft.Atmosphere.AirDensity() == 0) ? Vz.Format("Waiting Suicide Burn | Current Lateral Dist: {0:n2} km | time to impact = {1}:{2:00}:{3:00}", (Lateral_Distance() / 1000), Hours, Minutes, Seconds, "") : Vz.Format("Aero Diving | Lateral Dist: {0:n2} km | time to impact = {1}:{2:00}:{3:00}", (Lateral_Distance() / 1000), Hours, Minutes, Seconds, "")), 7);
        Vz.SetInput(CraftInput.Throttle, 0);
        Vz.WaitSeconds(0);
    }
    Vz.LockNavSphere(LockNavSphereIndicatorType.None);
    using (new While((Vz.Craft.AltitudeAGL() > 0)))
    {
        // Simple Aggresive Suicide Burn
        Time_format(Impact_Time());
        Vz.Display(Vz.Format("Lat Dist: {0:n2} km | Lat Vel: {1:n1} m/s | Impact Time: {2}:{3:00}:{4:00} sec | Pitch: {5:n2}", (Vz.Length(Distance) / 1000), Vz.Craft.Velocity.LateralSurface(), Hours, Minutes, Seconds, Pitch, ""), 7);
        Suicide_Burn();
        Landing_PID__P(((Vz.Craft.AltitudeAGL() < 100) ? 0.2 : 0.15), 0.001, (((Vz.Length(LateralRelativeVelocity) < 20) && (Vz.Length(Distance) < 250)) ? ((Vz.Craft.AltitudeAGL() < 200) ? 3 : 5) : 10), 5, -0);
        using (new If((Vz.Craft.AltitudeAGL() < 100)))
        {
            Vz.SetTimeModeAttr("Normal");
            Vz.SetActivationGroup(9, true);
        }
        using (new If(Vz.Craft.Grounded()))
        {
            Vz.Break();
        }
        using (new If((Vz.Craft.SolarRadiation() == -0)))
        {
            Vz.SetActivationGroup(7, true);
        }
    }
    Vz.SetActivationGroup(10, false);
    Vz.SetInput(CraftInput.Throttle, -0);
    Vz.Display("Touch Down !", 7);
    Vz.Broadcast(BroadCastType.Craft, "Target Planet Landed", -0);
}

using (new OnReceiveMessage("Deorbit Menu"))
{
    Vz.SetActivationGroup(1, false);
    CountDown = false;
    Vz.TargetNode("");
    using (new While((Vz.ActivationGroup(2) == false)))
    {
        using (new If((Vz.LengthOf(Vz.Craft.Target.Name()) > -0)))
        {
            Vz.Display(Vz.Format("Selected: <color=#AAFF00>{0}</color><br>Press AG.2 to Land!", Vz.Craft.Target.Name(), ""), 7);
        }
        using (new Else())
        {
            Vz.Display("Select your target<br>Then Press AG.2 to Land!", 7);
        }
        Vz.SetActivationGroup(1, false);
        Vz.SetActivationGroup(3, false);
        Vz.WaitSeconds(0);
    }
    using (new If((Vz.Planet(Vz.Craft.Orbit.Planet()).AtmosphereDepth() < 1000)))
    {
        using (new If((Vz.PartNameToID("Second Stage") > -0)))
        {
            Vz.ActivateStage();
        }
    }
    Vz.WaitSeconds(0.1);
    using (new If((Vz.LengthOf(Vz.Craft.Target.Name()) > -0)))
    {
        using (new If(Vz.Contains(Vz.Craft.Target.Name(), Vz.Craft.Orbit.Planet())))
        {
            Vz.Broadcast(BroadCastType.Local, "Simple Deorbit", -0);
        }
        using (new Else())
        {
            Vz.Broadcast(BroadCastType.Local, "CMTargeted Landing", -0);
        }
    }
    using (new Else())
    {
        Vz.Broadcast(BroadCastType.Local, "Simple Deorbit", -0);
    }
}

using (new OnReceiveMessage("Target Planet Landed"))
{
    Vz.Display("Landed !", 7);
    Vz.SetActivationGroup(10, false);
    Landing = false;
    Interplanetary = false;
    Flyby = false;
    Vz.LockNavSphere(LockNavSphereIndicatorType.None);
    Vz.WaitSeconds(3);
    Vz.SetActivationGroup(3, false);
    using (new While((Vz.ActivationGroup(3) == false)))
    {
        Vz.Display("Return to Home?<br>Then press AG.3 to start!", 7);
        Vz.SetActivationGroup(1, false);
        Vz.SetActivationGroup(2, false);
        Vz.WaitSeconds(0);
    }
    Vz.ListAdd(Mission, "Home");
    Vz.ListSet(Mission, Vz.ListGet(Mission, 1), 2);
    Vz.WaitSeconds(0.1);
    using (new If(Vz.Contains(Orbit_Status(), "Planet")))
    {
        Vz.TargetNode(Vz.ListGet(Mission, 1));
        Vz.Broadcast(BroadCastType.Local, "Launch to Target Planet", Vz.ListGet(Mission, 1));
    }
    using (new Else())
    {
        Vz.Broadcast(BroadCastType.Local, "Moon Home Return", -0);
    }
}

using (new OnReceiveMessage("Launch to the Moon"))
{
    Vz.ListClear(Mission);
    Vz.ListAdd(Mission, Vz.Craft.Orbit.Planet());
    Vz.ListAdd(Mission, data);
    Vizzy_Status(true);
    Launch_to_Orbit(150, -0, -0);
    Orbit_Circularization();
    Orbit_Transfer_to(data);
    Vz.SetTimeModeAttr("FastForward");
    Vz.WaitSeconds(1);
    Warp((0.25 * TransferPeriod));
}

var Trajectory_Correction_Burn = Vz.DeclareCustomInstruction("Trajectory Correction Burn", "target", "time").SetInstructions((target, time) =>
{
    Vz.TargetNode(target);
    using (new If((Vz.Length(Target_Solar_Position()) > Vz.Length(Parent_Solar_Position()))))
    {
        DeltaTime = (Vz.Craft.Orbit.TimeToAp() - (0.85 * time));
    }
    using (new Else())
    {
        DeltaTime = (Vz.Craft.Orbit.TimeToPe() - (0.95 * time));
    }
    Time_Wrap_in((DeltaTime + Vz.Time.MissionTime()), 20, 100000000, 3);
    Time1 = Vz.Time.MissionTime();
    using (new If((Vz.Length(Target_Solar_Position()) > Vz.Length(Parent_Solar_Position()))))
    {
        DeltaTime = (0.85 * time);
    }
    using (new Else())
    {
        DeltaTime = (0.95 * time);
    }
    Trajectory_Calculation_from(Vz.Craft.Nav.Position(), Vz.Craft.Target.Position(), Vz.Craft.Orbit.Planet(), DeltaTime);
    Heading_Align((VectorV1 - Vz.Craft.Velocity.Orbital()), 2, "Preparing Heading");
    Trajectory_Calculation_from(Vz.Craft.Nav.Position(), Vz.Craft.Target.Position(), Vz.Craft.Orbit.Planet(), DeltaTime);
    Text = Vz.Format("{0} Correction Manuever", Vz.Craft.Target.Name(), "");
    Orbital_Manuever_Burn(Text);
    Time_Wrap_in(((1.05 * DeltaTime) + Vz.Time.MissionTime()), 20, 10000000, 3);
    Vz.SetTimeModeAttr("TimeWarp6");
});

var Rendezvous_to = Vz.DeclareCustomInstruction("Rendezvous to", "target").SetInstructions((target) =>
{
    Vz.TargetNode(target);
    using (new WaitUntil((Vz.LengthOf(Vz.Craft.Target.Name()) > -0))) { }
    Time_Wrap_in(((0.9 * TransferPeriod) + Vz.Time.MissionTime()), 20, 5000, 3);
    TransferPeriod = (Target_Distance() / Vz.Max(10, Vz.Length(Relative_Velocity(Vz.Craft.Target.Name()))));
    time = (TransferPeriod + Vz.Time.MissionTime());
    Trajectory_Calculation_from(Vz.Craft.Nav.Position(), Vz.Craft.Target.Position(), Vz.Craft.Orbit.Planet(), (20 + TransferPeriod));
    using (new If((Vz.Length(Burn_Vector(VectorV1)) > 15)))
    {
        Heading_Align(VectorV1, 5, Vz.Format("Rendezvous time: {0:n0} sec | Delta V: {1:n0} m/s", TransferPeriod, Vz.Length(Burn_Vector(VectorV1)), ""));
    }
    Precision_Burn(VectorV1);
    Time_Wrap_in(time, 10, 1500, 2);
    using (new While(((Vz.Length(Forward_Speed(Vz.Craft.Target.Name())) > 8) && (Vz.Length(Relative_Velocity(Vz.Craft.Target.Name())) > 5))))
    {
        Vz.SetTimeModeAttr("Normal");
        LateralRelativeVelocity = (Vz.Norm(Vz.Cross(Vz.Cross(Target_Vector(), Relative_Velocity(Vz.Craft.Target.Name())), Target_Vector())) * (Vz.Length(Relative_Velocity(Vz.Craft.Target.Name())) * Vz.Sin(Vz.Deg2Rad(Vz.Angle(Relative_Velocity(Vz.Craft.Target.Name()), Target_Vector())))));
        CraftVelocityHeading = (Forward_Speed(Vz.Craft.Target.Name()) + (LateralRelativeVelocity * Vz.Min(1.5, (Target_Distance() / 50))));
        Craft(CraftVelocityHeading);
        Display(Vz.Format("Relative Vel: {0:n0} m/s | Lateral Vel: {1:n0} m/s | Distance: {2:n1} km", Vz.Length(Forward_Speed(Vz.Craft.Target.Name())), Vz.Length(LateralRelativeVelocity), (Target_Distance() / 1000), ""));
        Vz.SetInput(CraftInput.Throttle, (Vz.Length(CraftVelocityHeading) / (Max_Acceleration() * 4)));
        using (new If(Abort))
        {
            Vz.Break();
        }
        Vz.WaitSeconds(0);
    }
    Vz.SetInput(CraftInput.Throttle, 0);
});

var Original_Target_Name = Vz.DeclareCustomInstruction("Original Target Name", "target").SetInstructions((target) =>
{
    Vz.ListClear(Letter_Dump);
    Vz.ListClear(Craft_Name);
    TargetName = target;
    using (new If((Vz.ListLength(Craft_Name) == -0)))
    {
        using (new For("i").From(1).To(Vz.LengthOf(TargetName)).By(1))
        {
            Vz.ListAdd(Craft_Name, Vz.LetterOf(i, TargetName));
        }
    }
    using (new If((Vz.ListLength(Letter_Dump) == -0)))
    {
        using (new For("i").From(1).To(Vz.ListLength(Craft_Name)).By(1))
        {
            using (new If(Vz.Contains(Vz.ListGet(Craft_Name, i), "-")))
            {
                Vz.ListAdd(Letter_Dump, i);
            }
        }
    }
    using (new If((Vz.ListLength(Letter_Dump) > 1)))
    {
        Dump = "- -";
        using (new For("i").From(1).To(Vz.ListLength(Letter_Dump)).By(1))
        {
            using (new If(String_Input(Vz.Join(Vz.LetterOf((Vz.ListGet(Letter_Dump, i) - 1), TargetName), Vz.LetterOf(Vz.ListGet(Letter_Dump, i), TargetName), Vz.LetterOf((Vz.ListGet(Letter_Dump, i) + 1), TargetName), ""), Vz.Join(Vz.LetterOf(2, Dump), Vz.LetterOf(1, Dump), Vz.LetterOf(2, Dump), ""))))
            {
                Craft = Vz.SubString(1, (Vz.ListGet(Letter_Dump, i) - 2), TargetName);
            }
        }
    }
    using (new Else())
    {
        Craft = Vz.SubString(1, (Vz.ListGet(Letter_Dump, 1) - 2), TargetName);
    }
});

var Suicide_Burn = Vz.DeclareCustomInstruction("Suicide Burn").SetInstructions(() =>
{
    using (new If((Vz.Craft.Velocity.VerticalSurface() <= ((Vz.Length(Vz.Craft.Velocity.Gravity()) > 8.5) ? -150 : -70))))
    {
        Throttle = ((((Vz.Craft.Velocity.LateralSurface() < 15) && (Vz.Craft.AltitudeAGL() > 2000)) ? 0.992 : 0.999) * Vz.Abs(((((Vz.Craft.Velocity.VerticalSurface() ^ 2) / (2 * (Vz.Craft.AltitudeAGL() - 100))) + Vz.Length(Vz.Craft.Velocity.Gravity())) / ((0.5 * Vz.Sin(Vz.Deg2Rad(Vz.Craft.Nav.Pitch()))) * Max_Acceleration()))));
    }
    using (new If(((Vz.Craft.Velocity.VerticalSurface() > ((Vz.Length(Vz.Craft.Velocity.Gravity()) > 8.5) ? -150 : -70)) && (Vz.Craft.Velocity.VerticalSurface() <= -1))))
    {
        Throttle = ((((Vz.Craft.Velocity.LateralSurface() < 15) && (Vz.Craft.AltitudeAGL() > 2000)) ? 0.988 : 0.996) * Vz.Abs(((((Vz.Craft.Velocity.VerticalSurface() ^ 2) / (2 * (Vz.Craft.AltitudeAGL() - 3.2))) + Vz.Length(Vz.Craft.Velocity.Gravity())) / (Vz.Sin(Vz.Deg2Rad(Vz.Craft.Nav.Pitch())) * Max_Acceleration()))));
    }
    using (new If((Vz.Craft.Velocity.VerticalSurface() >= -0)))
    {
        Throttle = (((Vz.Craft.Performance.Mass() * Vz.Length(Vz.Craft.Velocity.Gravity())) * 0.2) / Vz.Craft.Performance.MaxThrust());
    }
    using (new If(((-1 < Vz.Craft.Velocity.VerticalSurface()) && (Vz.Craft.AltitudeAGL() < 10))))
    {
        Throttle = (0.97 * Vz.Abs(((Vz.Length(Vz.Craft.Velocity.Gravity()) - (0.5 + Vz.Craft.Velocity.VerticalSurface())) / Max_Acceleration())));
    }
    Vz.SetInput(CraftInput.Throttle, Throttle);
});

var Precision_Burn = Vz.DeclareCustomInstruction("Precision Burn", "vector").SetInstructions((vector) =>
{
    Vector = vector;
    using (new While((Vz.Length(Vector) > 0.05)))
    {
        Vz.SetTimeModeAttr("Normal");
        Vector = Burn_Vector(vector);
        Display(Vz.Format("Just little correction<br>DeltaV: {0:n3} m/s", Vz.Length(Vector), ""));
        using (new If((Vz.Length(Vector) > 15)))
        {
            Vz.SetInput(CraftInput.TranslationMode, 0);
            Craft(VectorV1);
            Vz.SetInput(CraftInput.Throttle, (Vz.Length(Vector) / (Max_Acceleration() * 2)));
        }
        using (new Else())
        {
            Vz.LockNavSphere(LockNavSphereIndicatorType.Current);
            VelocityHeading = Vz.PartPciToLocal(-0, (Vector * 10));
            Vz.SetActivationGroup(10, true);
            Vz.SetInput(CraftInput.Throttle, -0);
            Vz.SetInput(CraftInput.TranslationMode, 1);
            Vz.SetInput(CraftInput.TranslateForward, VelocityHeading.z);
            Vz.SetInput(CraftInput.TranslateRight, VelocityHeading.x);
            Vz.SetInput(CraftInput.TranslateUp, VelocityHeading.y);
        }
        Vz.WaitSeconds(0);
    }
    Vz.SetInput(CraftInput.TranslationMode, 0);
    Vz.SetInput(CraftInput.Throttle, 0);
});

using (new OnReceiveMessage("Moon Fly By"))
{
    Vz.SetTimeModeAttr("Normal");
    Flyby = true;
    Vz.Display(Vz.Format("{0} SOI Fly By", Vz.Craft.Orbit.Planet(), ""), 7);
    Vz.SetActivationGroup(10, true);
    OrbitApoapsis = Vz.Craft.AltitudeASL();
    using (new While((Vz.Craft.AltitudeASL() > (0.9 * OrbitApoapsis))))
    {
        Vz.SetTimeModeAttr("TimeWarp5");
        Craft((Radial_Vector() * -1));
    }
    Vz.SetTimeModeAttr("Normal");
    // Avoiding direct impact trajectory
    using (new While((Vz.Craft.Orbit.Periapsis() < 300000)))
    {
        Craft((Radial_Vector() * -1));
        Vz.SetTimeModeAttr("FastForward");
        Vz.Display(Vz.Format("Flyby | Periapsis = {0} km", (Vz.Round(Vz.Craft.Orbit.Periapsis()) / 1000), ""), 7);
        Vz.SetInput(CraftInput.Throttle, 0.15);
    }
    Vz.SetInput(CraftInput.Throttle, -0);
    Vz.Display(Vz.Format("Escaping {0} SOI", Vz.Craft.Orbit.Planet(), ""), 7);
    Vz.SetTimeModeAttr("TimeWarp6");
}

using (new OnChangeSoi())
{
    // Determining SOI
    Vz.Display("Please Wait !", 7);
    Vz.SetTimeModeAttr("Normal");
    Abort = true;
    Vz.WaitSeconds(1);
    using (new If((Vz.RawXmlCraftProperty(@"<CraftProperty property=""Part.Activated"" style=""part""><CraftProperty property=""Part.NameToID"" style=""part-id""><Constant text=""Vizzy Indicator"" /></CraftProperty></CraftProperty>") == true)))
    {
        Vizzy_Status(true);
    }
    Vz.Display("Detecting SOI", 7);
    Vz.WaitSeconds(1);
    Vz.Broadcast(BroadCastType.Local, "Staging", -0);
    using (new WaitUntil((Vz.LengthOf(Vz.Craft.Orbit.Planet()) > 0))) { }
    Abort = false;
    using (new If((Vz.ListLength(Mission) > 0)))
    {
        using (new If(String_Input(Vz.Craft.Orbit.Planet(), Vz.ListGet(Mission, 2))))
        {
            using (new If((Vz.ListLength(Mission) == 2)))
            {
                Vz.Broadcast(BroadCastType.Local, "Target Planet SOI", -0);
            }
            using (new ElseIf((Vz.ListLength(Mission) == 3)))
            {
                Vz.Broadcast(BroadCastType.Local, "Home Arrival", -0);
            }
        }
        using (new ElseIf((!String_Input(Vz.Craft.Orbit.Planet(), Vz.ListGet(Mission, 2)))))
        {
            using (new If(String_Input(Parent_Star(), 0)))
            {
                using (new If(Interplanetary))
                {
                    Vz.Broadcast(BroadCastType.Local, "Juno SOI", -0);
                }
            }
            using (new ElseIf(String_Input(Orbit_Status(), "Planet")))
            {
                using (new If(Flyby))
                {
                    Vz.SetTimeModeAttr("TimeWarp6");
                }
            }
            using (new Else())
            {
                Vz.Broadcast(BroadCastType.Local, "Moon Fly By", -0);
            }
        }
    }
    using (new Else())
    {
        Vz.Display(Vz.Format("Welcome to {0} SOI, Auto Pilot Off", Vz.Craft.Orbit.Planet(), ""), 7);
    }
}

using (new OnReceiveMessage("CMCustom Orbit"))
{
    Vizzy_Status(true);
    Launch_to_Orbit(Vz.ListGet(data, 2), Vz.ListGet(data, 3), Vz.ListGet(data, 4));
    Orbit_Circularization();
    Vizzy_Status(false);
}

// ── Custom Expressions ───────────────────────────────
var Position_Z_up = Vz.DeclareCustomExpression("Position Z-up", "position").SetReturn((position) =>
{
    return Vz.Vec(position.x, position.z, position.y);
});

var Velocity_Z_up = Vz.DeclareCustomExpression("Velocity Z-up", "velocity").SetReturn((velocity) =>
{
    return Vz.Vec(velocity.x, velocity.z, velocity.y);
});

var Angular_Momentum = Vz.DeclareCustomExpression("Angular Momentum", "position", "velocity").SetReturn((position, velocity) =>
{
    return Vz.Cross(Position_Z_up(position), Velocity_Z_up(velocity));
});

var Eccentricity = Vz.DeclareCustomExpression("Eccentricity", "position", "velocity").SetReturn((position, velocity) =>
{
    return Vz.Length(Eccentricity_Vector(position, velocity));
});

var Eccentricity_Vector = Vz.DeclareCustomExpression("Eccentricity Vector", "position", "velocity").SetReturn((position, velocity) =>
{
    return (((((Vz.Length(velocity) ^ 2) - (Mu() / Vz.Length(position))) * Position_Z_up(position)) - (Vz.Dot(Position_Z_up(position), Velocity_Z_up(velocity)) * Velocity_Z_up(velocity))) / Mu());
});

var Node_Vector = Vz.DeclareCustomExpression("Node Vector", "position", "velocity").SetReturn((position, velocity) =>
{
    return Vz.Cross(Vz.Vec(-0, -0, 1), Angular_Momentum(position, velocity));
});

var Inclination = Vz.DeclareCustomExpression("Inclination", "position", "velocity").SetReturn((position, velocity) =>
{
    return Vz.Rad2Deg(Vz.Acos((Angular_Momentum(position, velocity).z / Vz.Length(Angular_Momentum(position, velocity)))));
});

var Absolute_Right_Ascension = Vz.DeclareCustomExpression("Absolute Right Ascension", "position", "velocity").SetReturn((position, velocity) =>
{
    return Vz.Rad2Deg(Vz.Acos((Node_Vector(position, velocity).x / Vz.Length(Node_Vector(position, velocity)))));
});

var Right_Ascension = Vz.DeclareCustomExpression("Right Ascension", "position", "velocity").SetReturn((position, velocity) =>
{
    return ((Node_Vector(position, velocity).y < -0) ? (360 - Absolute_Right_Ascension(position, velocity)) : Absolute_Right_Ascension(position, velocity));
});

var Argument_of_Periapsis = Vz.DeclareCustomExpression("Argument of Periapsis", "position", "velocity").SetReturn((position, velocity) =>
{
    return ((Eccentricity_Vector(position, velocity).z < -0) ? (360 - Absolute_Argument_of_Periapsis(position, velocity)) : Absolute_Argument_of_Periapsis(position, velocity));
});

var Absolute_True_Anomaly = Vz.DeclareCustomExpression("Absolute True Anomaly", "position", "velocity").SetReturn((position, velocity) =>
{
    return Vz.Rad2Deg(Vz.Acos((Vz.Dot(Eccentricity_Vector(position, velocity), Position_Z_up(position)) / (Eccentricity(position, velocity) * Vz.Length(Position_Z_up(position))))));
});

var String_Input = Vz.DeclareCustomExpression("String Input", "string1", "string2").SetReturn((string1, string2) =>
{
    return (Vz.Contains(string1, string2) && Vz.Contains(string2, string1));
});

var IsNan = Vz.DeclareCustomExpression("IsNan", "Value").SetReturn((Value) =>
{
    return (!((Value < -0) || (Value >= -0)));
});

var Impact_Time = Vz.DeclareCustomExpression("Impact Time").SetReturn(() =>
{
    return ((2 * Vz.Craft.AltitudeAGL()) / Vz.Abs(Vz.Craft.Velocity.VerticalSurface()));
});

var Absolute_Argument_of_Periapsis = Vz.DeclareCustomExpression("Absolute Argument of Periapsis", "position", "velocity").SetReturn((position, velocity) =>
{
    return Vz.Rad2Deg(Vz.Acos((Vz.Dot(Node_Vector(position, velocity), Eccentricity_Vector(position, velocity)) / (Vz.Length(Node_Vector(position, velocity)) * Eccentricity("", velocity)))));
});

var True_Anomaly = Vz.DeclareCustomExpression("True Anomaly", "position", "velocity").SetReturn((position, velocity) =>
{
    return ((Vz.Dot(Position_Z_up(position), Velocity_Z_up(velocity)) < -0) ? (360 - Absolute_True_Anomaly(position, velocity)) : Absolute_True_Anomaly(position, velocity));
});

var Specific_Orbital_Energy = Vz.DeclareCustomExpression("Specific Orbital Energy", "position", "velocity").SetReturn((position, velocity) =>
{
    return (((Vz.Length(velocity) ^ 2) / 2) - (Mu() / Vz.Length(position)));
});

var Rotate = Vz.DeclareCustomExpression("Rotate", "vector", "axis", "angle").SetReturn((vector, axis, angle) =>
{
    return ((vector * Vz.Cos(Vz.Deg2Rad(angle))) + ((Vz.Cross(Vz.Norm(axis), vector) * Vz.Sin(Vz.Deg2Rad(angle))) + (Vz.Norm(axis) * (Vz.Dot(Vz.Norm(axis), vector) * (1 - Vz.Cos(Vz.Deg2Rad(angle)))))));
});

var Eccentric_Anomaly = Vz.DeclareCustomExpression("Eccentric Anomaly", "position", "velocity").SetReturn((position, velocity) =>
{
    return (IsNan((1 / Eccentricity(position, velocity))) ? True_Anomaly(position, velocity) : ((True_Anomaly(position, velocity) <= 180) ? Partial_Eccentric_Anomaly(position, velocity) : (360 - Partial_Eccentric_Anomaly(velocity, position))));
});

var Partial_Eccentric_Anomaly = Vz.DeclareCustomExpression("Partial Eccentric Anomaly", "position", "velocity").SetReturn((position, velocity) =>
{
    return Vz.Rad2Deg(Vz.Acos((1 - ((Vz.Length(position) / Semi_Major_Axis(position, velocity)) / Eccentricity(position, velocity)))));
});

var Semi_Major_Axis = Vz.DeclareCustomExpression("Semi-Major Axis", "position", "velocity").SetReturn((position, velocity) =>
{
    return ((Eccentricity(position, velocity) < 1) ? (-1 * (Mu() / (2 * Specific_Orbital_Energy(position, velocity)))) : (1 / ((2 * Vz.Length(position)) - ((Vz.Length(velocity) ^ 2) / Mu()))));
});

var Rejection_of = Vz.DeclareCustomExpression("Rejection of", "vector1", "vector2").SetReturn((vector1, vector2) =>
{
    return (vector1 - ((Vz.Dot(vector1, vector2) / Vz.Dot(vector2, vector2)) * vector2));
});

var Sign = Vz.DeclareCustomExpression("Sign", "value").SetReturn((value) =>
{
    return ((value < -0) ? -1 : ((value > -0) ? 1 : value));
});

var Pi = Vz.DeclareCustomExpression("Pi").SetReturn(() =>
{
    return (22 / 7);
});

var Uphill_Orbit = Vz.DeclareCustomExpression("Uphill Orbit").SetReturn(() =>
{
    return (TargetOrbitEnergy > Specific_Orbital_Energy(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()));
});

var Downhill_Orbit = Vz.DeclareCustomExpression("Downhill Orbit").SetReturn(() =>
{
    return (Specific_Orbital_Energy(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) > TargetOrbitEnergy);
});

var Uphill_Delta_V = Vz.DeclareCustomExpression("Uphill Delta V").SetReturn(() =>
{
    return (Vz.Sqrt((2 * (TargetOrbitEnergy - PotentialEnergy))) - Vz.Sqrt((2 * (Specific_Orbital_Energy(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) - PotentialEnergy))));
});

var Downhill_Delta_V = Vz.DeclareCustomExpression("Downhill Delta V").SetReturn(() =>
{
    return (Vz.Sqrt((2 * (Specific_Orbital_Energy(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) - PotentialEnergy))) - Vz.Sqrt((2 * (TargetOrbitEnergy - PotentialEnergy))));
});

var Exp = Vz.DeclareCustomExpression("Exp").SetReturn(() =>
{
    return 2.71828182845904523;
});

var g0 = Vz.DeclareCustomExpression("g0").SetReturn(() =>
{
    return 9.798;
});

var G = Vz.DeclareCustomExpression("G").SetReturn(() =>
{
    return 0.0000000000667384;
});

var Mu = Vz.DeclareCustomExpression("Mu").SetReturn(() =>
{
    return (G() * Vz.Planet(Vz.Craft.Orbit.Planet()).Mass());
});

var Absolute_Normal_Vector = Vz.DeclareCustomExpression("Absolute Normal Vector").SetReturn(() =>
{
    return Vz.Cross(Vz.Norm(Vz.Craft.Velocity.Orbital()), Vz.Norm(Vz.Craft.Nav.Position()));
});

var Anti_Normal_Vector = Vz.DeclareCustomExpression("Anti Normal Vector").SetReturn(() =>
{
    return ((Vz.Dot(Vz.Craft.Velocity.Orbital(), Vz.Craft.Nav.East()) < -0) ? Absolute_Normal_Vector() : (-1 * Absolute_Normal_Vector()));
});

var Normal_Vector = Vz.DeclareCustomExpression("Normal Vector").SetReturn(() =>
{
    return ((Vz.Dot(Vz.Craft.Velocity.Orbital(), Vz.Craft.Nav.East()) >= -0) ? Absolute_Normal_Vector() : (-1 * Absolute_Normal_Vector()));
});

var East_Target_Distance = Vz.DeclareCustomExpression("East Target Distance").SetReturn(() =>
{
    return (Vz.Deg2Rad((Vz.PosToLatLongAgl(Vz.Craft.Target.Position()).y - Vz.PosToLatLongAgl(Vz.Craft.Nav.Position()).y)) * Vz.Planet(Vz.Craft.Orbit.Planet()).Radius());
});

var North_Target_Distance = Vz.DeclareCustomExpression("North Target Distance").SetReturn(() =>
{
    return (Vz.Deg2Rad((Vz.PosToLatLongAgl(Vz.Craft.Target.Position()).x - Vz.PosToLatLongAgl(Vz.Craft.Nav.Position()).x)) * Vz.Planet(Vz.Craft.Orbit.Planet()).Radius());
});

var Vector_Display = Vz.DeclareCustomExpression("Vector Display", "vector").SetReturn((vector) =>
{
    return Vz.Format("({0:n0}, {1:n0}, {2:n0})", (vector.x / 1000), (vector.y / 1000), (vector.z / 1000), "");
});

var Parent_Solar_Position = Vz.DeclareCustomExpression("Parent Solar Position").SetReturn(() =>
{
    return Vz.Planet(Vz.Craft.Orbit.Planet()).SolarPosition();
});

var Target_Solar_Position = Vz.DeclareCustomExpression("Target Solar Position").SetReturn(() =>
{
    return Vz.Planet(Vz.Craft.Target.Name()).SolarPosition();
});

var Lateral_Distance = Vz.DeclareCustomExpression("Lateral Distance").SetReturn(() =>
{
    return Vz.Distance(Surface(Vz.Craft.Target.Position()), Surface(Vz.Craft.Nav.Position()));
});

var Surface = Vz.DeclareCustomExpression("Surface", "vector").SetReturn((vector) =>
{
    return Vz.ToPosition(Vz.Vec(Vz.PosToLatLongAgl(vector).x, Vz.PosToLatLongAgl(vector).y, -0));
});

var Stump_S = Vz.DeclareCustomExpression("Stump S", "Z").SetReturn((Z) =>
{
    return ((Z > -0) ? ((Vz.Sqrt(Z) - Vz.Sin(Vz.Sqrt(Z))) / (Vz.Sqrt(Z) ^ 3)) : (1 / 6));
});

var Stump_C = Vz.DeclareCustomExpression("Stump C", "Z").SetReturn((Z) =>
{
    return ((Z > -0) ? ((1 - Vz.Cos(Vz.Sqrt(Z))) / Z) : 0.5);
});

var F = Vz.DeclareCustomExpression("F", "z", "t").SetReturn((z, t) =>
{
    return (((((y(z) / Stump_C(z)) ^ 1.5) * Stump_S(z)) + (A * Vz.Sqrt(y(z)))) - (Vz.Sqrt(Mu) * t));
});

var dFdz_z_0 = Vz.DeclareCustomExpression("dFdz z=0", "z").SetReturn((z) =>
{
    return (((Vz.Sqrt(2) / 40) * (y(0.00) ^ 1.5)) + ((A / 8) * (Vz.Sqrt(y(0.00)) + (A * Vz.Sqrt((1 / (2 * y(0.00))))))));
});

var dFdz_z_not_0 = Vz.DeclareCustomExpression("dFdz z not 0", "z").SetReturn((z) =>
{
    return ((((y(z) / Stump_C(z)) ^ 1.5) * (((1 / (2 * z)) * (Stump_C(z) - ((3 * Stump_S(z)) / (2 * Stump_C(z))))) + ((3 * (Stump_S(z) ^ 2)) / (4 * Stump_C(z))))) + ((A / 8) * ((((3 * Stump_S(z)) / Stump_C(z)) * Vz.Sqrt(y(z))) + (A * Vz.Sqrt((Stump_C(z) / y(z)))))));
});

var y = Vz.DeclareCustomExpression("y", "z").SetReturn((z) =>
{
    return ((ro + r) + (A * (((z * Stump_S(z)) - 1) / Vz.Sqrt(Stump_C(z)))));
});

var dFdz = Vz.DeclareCustomExpression("dFdz", "z").SetReturn((z) =>
{
    return ((z == 0) ? dFdz_z_0(z) : dFdz_z_not_0(z));
});

var Reflection = Vz.DeclareCustomExpression("Reflection", "vector", "normal").SetReturn((vector, normal) =>
{
    return (((2 * Vz.Dot(vector, normal)) * normal) + (-1 * vector));
});

var Y_Removal = Vz.DeclareCustomExpression("Y Removal", "vector").SetReturn((vector) =>
{
    return Vz.Vec(vector.x, -0, vector.z);
});

var Suicide_Burn_Trigger = Vz.DeclareCustomExpression("Suicide Burn Trigger").SetReturn(() =>
{
    return (((Vz.Craft.Velocity.VerticalSurface() ^ 2) / (2 * ((0.5 * Max_Acceleration()) - Vz.Length(Vz.Craft.Velocity.Gravity())))) + (2.5 - Vz.Craft.Velocity.VerticalSurface()));
});

var Target_DeltaV__to = Vz.DeclareCustomExpression("Target DeltaV, to", "TargetEnergy", "PotentialEnergy", "position", "velocity").SetReturn((TargetEnergy, PotentialEnergy, position, velocity) =>
{
    return (Vz.Sqrt((2 * (TargetEnergy - PotentialEnergy))) - Vz.Sqrt((2 * (Specific_Orbital_Energy(position, velocity) - PotentialEnergy))));
});

var TWR__ = Vz.DeclareCustomExpression("TWR *", "value").SetReturn((value) =>
{
    return Vz.Max((((Vz.Craft.Performance.Mass() * Vz.Length(Vz.Craft.Velocity.Gravity())) * value) / Vz.Craft.Performance.MaxThrust()), 0.05);
});

var Orbit_Plane_Normal = Vz.DeclareCustomExpression("Orbit Plane Normal", "AscendingNode", "inclination").SetReturn((AscendingNode, inclination) =>
{
    return Rotate(Vz.Vec(-0, 1, -0), AscendingNode, (-1 * inclination));
});

var Docking_Port = Vz.DeclareCustomExpression("Docking Port", "target").SetReturn((target) =>
{
    return Vz.Norm((Target_Position(target) - Vz.Craft.Target.Position()));
});

var Altitude = Vz.DeclareCustomExpression("Altitude", "position").SetReturn((position) =>
{
    return (Vz.Length(position) - Vz.Planet(Vz.Craft.Orbit.Planet()).Radius());
});

var Target_Distance = Vz.DeclareCustomExpression("Target Distance").SetReturn(() =>
{
    return Vz.Distance(Vz.Craft.Nav.Position(), Vz.Craft.Target.Position());
});

var Target_Vector = Vz.DeclareCustomExpression("Target Vector").SetReturn(() =>
{
    return (Vz.Craft.Target.Position() - Vz.Craft.Nav.Position());
});

var Target_Velocity = Vz.DeclareCustomExpression("Target Velocity", "name").SetReturn((name) =>
{
    return Vz.Craft.Property("Craft.Velocity");
});

var Relative_Velocity = Vz.DeclareCustomExpression("Relative Velocity", "target").SetReturn((target) =>
{
    return (Target_Velocity(target) - Vz.Craft.Velocity.Orbital());
});

var Target_Position = Vz.DeclareCustomExpression("Target Position", "name").SetReturn((name) =>
{
    return Vz.Craft.Property("Craft.Position");
});

var Target_Planet_Orbit = Vz.DeclareCustomExpression("Target Planet Orbit").SetReturn(() =>
{
    return ((Vz.Planet(Vz.Craft.Orbit.Planet()).AtmosphereDepth() > 70000) ? (3 * Vz.Planet(Vz.Craft.Orbit.Planet()).AtmosphereDepth()) : 150000);
});

var Home_Parking_Orbit = Vz.DeclareCustomExpression("Home Parking Orbit").SetReturn(() =>
{
    return 150000;
});

var Guided_Landing_Offset = Vz.DeclareCustomExpression("Guided Landing Offset").SetReturn(() =>
{
    return Vz.Vec(10, -0, -0);
});

var LandingPadLL = Vz.DeclareCustomExpression("LandingPadLL").SetReturn(() =>
{
    return Vz.Vec(0.00285, -0.00929, -0);
});

var Target_Orbit_Status = Vz.DeclareCustomExpression("Target Orbit Status").SetReturn(() =>
{
    return (Vz.Contains(Vz.Craft.Target.Planet(), Parent_Star()) ? "Orbiting Sun" : (Vz.Contains(Vz.Planet(Vz.Craft.Target.Planet()).Parent(), Parent_Star()) ? "Orbiting Planet" : "Orbiting Moon"));
});

var Target_Status = Vz.DeclareCustomExpression("Target Status", "name").SetReturn((name) =>
{
    return ((Vz.Length(Vz.Planet(name).SolarPosition()) == -0) ? "Craft" : ((Vz.Craft.Target.Position() == Vz.Planet(name).SolarPosition()) ? "Planet" : "Moon"));
});

var Craft_Status = Vz.DeclareCustomExpression("Craft Status").SetReturn(() =>
{
    return ((Vz.Craft.Grounded() || (Vz.Craft.AltitudeAGL() < 100)) ? "Ground" : ((Vz.Craft.Orbit.Apoapsis() < Vz.Planet(Vz.Craft.Orbit.Planet()).AtmosphereDepth()) ? "Atmospher" : ((Vz.Craft.Orbit.Periapsis() < 500) ? "Suborbital" : ((Vz.Craft.Orbit.Eccentricity() > 1) ? "Hyperbola" : ((Vz.Craft.Orbit.Eccentricity() > 0.1) ? "Elliptical" : "Circular")))));
});

var Orbit_Status = Vz.DeclareCustomExpression("Orbit Status").SetReturn(() =>
{
    return (((Vz.LengthOf(Grand_Parent()) == -0) && (Vz.LengthOf(Parent()) == -0)) ? "Star" : (((Vz.LengthOf(Parent()) > -0) && (Vz.LengthOf(Grand_Parent()) == -0)) ? "Planet" : "Moon"));
});

var Parent_Star = Vz.DeclareCustomExpression("Parent Star").SetReturn(() =>
{
    return ((Vz.LengthOf(Grand_Parent()) > -0) ? Grand_Parent() : ((Vz.LengthOf(Parent()) > -0) ? Parent() : Vz.Craft.Orbit.Planet()));
});

var Parent = Vz.DeclareCustomExpression("Parent").SetReturn(() =>
{
    return Vz.Planet(Vz.Craft.Orbit.Planet()).Parent();
});

var Grand_Parent = Vz.DeclareCustomExpression("Grand Parent").SetReturn(() =>
{
    return Vz.Planet(Parent()).Parent();
});

var Ascending_Node = Vz.DeclareCustomExpression("Ascending Node", "RightAscention").SetReturn((RightAscention) =>
{
    return Rotate(Vz.Vec(1, -0, -0), Vz.Vec(-0, 1, -0), (-1 * RightAscention));
});

var Radial_Vector = Vz.DeclareCustomExpression("Radial Vector").SetReturn(() =>
{
    return Rotate(Vz.Cross(Vz.Craft.Velocity.Orbital(), Vz.Craft.Velocity.Gravity()), Vz.Craft.Velocity.Orbital(), -90);
});

var Max_Acceleration = Vz.DeclareCustomExpression("Max Acceleration").SetReturn(() =>
{
    return (Vz.Craft.Performance.MaxThrust() / Vz.Craft.Performance.Mass());
});

var Time_since_Perigee = Vz.DeclareCustomExpression("Time since Perigee", "TrueAnomaly").SetReturn((TrueAnomaly) =>
{
    return (Vz.Sqrt(((Semi_Major_Axis(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) ^ 3) / Mu())) * (Vz.Deg2Rad(Eccentric_Anomaly_with(TrueAnomaly)) - (Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) * Vz.Sin(Vz.Deg2Rad(Eccentric_Anomaly_with(TrueAnomaly))))));
});

var Time_until_Surface__with_Alt = Vz.DeclareCustomExpression("Time until Surface, with Alt", "AGL").SetReturn((AGL) =>
{
    return (Time_since_Perigee(Theta(((Vz.Craft.Target.Name() == -0) ? Vz.Planet(Vz.Craft.Orbit.Planet()).Radius() : Vz.Length(Vz.Craft.Target.Position())))) - Time_since_Perigee(True_Anomaly(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital())));
});

var Eccentric_Anomaly_with = Vz.DeclareCustomExpression("Eccentric Anomaly with", "TrueAnomaly").SetReturn((TrueAnomaly) =>
{
    return ((360 + Vz.Rad2Deg((2 * Vz.Atan((Vz.Sqrt(((1 - Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital())) / (1 + Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital())))) * Vz.Tan((Vz.Deg2Rad(TrueAnomaly) / 2))))))) % 360);
});

var Theta = Vz.DeclareCustomExpression("Theta", "radius").SetReturn((radius) =>
{
    return (360 - Vz.Rad2Deg(Vz.Acos(((((Vz.Length(Angular_Momentum(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital())) ^ 2) / Mu()) - radius) / (Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) * radius)))));
});

var Burn_Vector = Vz.DeclareCustomExpression("Burn Vector", "VelocityVector").SetReturn((VelocityVector) =>
{
    return ((Vz.Norm((VelocityVector + Vz.Craft.Velocity.Orbital())) * Vz.Length(VelocityVector)) - (Vz.Norm((VelocityVector + Vz.Craft.Velocity.Orbital())) * Vz.Length(Vz.Craft.Velocity.Orbital())));
});

var Docking_Entry_Point = Vz.DeclareCustomExpression("Docking Entry Point", "target", "offset").SetReturn((target, offset) =>
{
    return (Target_Position(target) + ((Vz.Norm((Vz.Contains(Vz.Craft.Target.Name(), "Docking Port") ? Docking_Port(target) : (Target_Position(target) - Vz.Craft.Nav.Position()))) * offset) * -1));
});

var Docking_Entry_Vector = Vz.DeclareCustomExpression("Docking Entry Vector", "target", "offset").SetReturn((target, offset) =>
{
    return (Vz.Craft.Nav.Position() + (Vz.Norm(Vz.Cross(Target_Vector(), Vz.Cross(Target_Vector(), (Docking_Entry_Point(target, offset) - Vz.Craft.Nav.Position())))) * Vz.Min(12, (Vz.Angle(Target_Vector(), Docking_Port(target)) * 0.2))));
});

var Forward_Speed = Vz.DeclareCustomExpression("Forward Speed", "target").SetReturn((target) =>
{
    return (Vz.Norm((-1 * Target_Vector())) * (Vz.Dot(Vz.Norm((-1 * Relative_Velocity(target))), Vz.Norm(Target_Vector())) * Vz.Length(Relative_Velocity(target))));
});

var Lateral_Vector = Vz.DeclareCustomExpression("Lateral Vector", "vector").SetReturn((vector) =>
{
    return Vz.Cross(Vz.Cross(Vz.Norm(Vz.Craft.Velocity.Gravity()), vector), Vz.Norm(Vz.Craft.Velocity.Gravity()));
});


// ── Serialize output ──
Console.WriteLine(Vz.context.currentProgram.Serialize().ToString());
