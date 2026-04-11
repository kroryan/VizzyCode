using System;
using System.Linq;
using System.Collections.Generic;
using REWJUNO;
using REWVIZZY;

// ── Program: Universal Vizzy Mission 2 ──────────────────────────────────
Vz.Init("Universal Vizzy Mission 2");

// ── Variables ────────────────────────────────────────
// var Craft_List = [];   // list;
// var CountDown = -0;
// var Distance = -0;
// var Seconds = -0;
// var TimeWrap = 0;
// var Days = -0;
// var DeltaTime = -0;
// var TimeSincePerigee = -0;
// var TrueAnomaly = -0;
// var Mu = -0;
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
// var Universal_Anomaly_X = -0;
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
// var Input = 0;
// var Output = 0;
// var Display = 0;
// var E = 0;
// var Temp1 = 0;
// var N = 0;
// var D = 0;
// var Temp2 = 0;
// var Temp3 = 0;
// var OrbitVelocity = -0;
// var ExhaustVelocity = -0;
// var OrbitDeltaV = -0;
// var OrbitBurnTime = -0;
// var Minutes = -0;
// var velocity = -0;
// var Inclination = -0;
// var RightAscension = -0;
// var AscendingNode = -0;
// var TransferTime = -0;
// var ParentSMA = -0;
// var TargetSMA = -0;
// var TransferSMA = -0;
// var PotentialEnergy = -0;
// var TargetOrbitEnergy = -0;
// var ParentEnergy = -0;
// var TransferEnergy = -0;
// var TransferPotentialPeriapsis = -0;
// var TransferDeltaV = -0;
// var ExitSOI = -0;
// var SMA = -0;
// var EscapeEnergy = -0;
// var CurrentPotentialEnergy = -0;
// var EscapeDeltaV = -0;
// var EscapeEccentricity = -0;
// var TransferAngle = -0;
// var Eccentricity = -0;
// var MeanAnomaly = -0;
// var OrbitLongitude = -0;
// var PlanetRotationSpeed = -0;
// var Heading = -0;
// var TargetHeading = -0;
// var TimeUntilSurface = -0;
// var TargetDelta = -0;
// var TransferVelocity = -0;
// var Node = -0;
// var Azimuth = -0;
// var OrbitalPlane = -0;
// var HeadingError = -0;
// var Velocity = [];   // list;
// var Vector = -0;
// var DescendingNode = -0;
// var time = -0;
// var Height = [];   // list;
// var Craft_Name = [];   // list;
// var Letter_Dump = [];   // list;
// var Throttle = -0;
// var g0 = 0;
// var rho = 0;
// var CD = 0;
// var h_Scale = 0;
// var R = 0;
// var Isp = 0;
// var TWR = 0;
// var m0 = 0;
// var mfinal = 0;
// var mdot = 0;
// var mprop = 0;
// var Thrust = 0;
// var Q = 0;
// var h_Turn = 0;
// var height = 0;
// var gamma = 0;
// var BurnTime = 0;
// var m = 0;
// var vdot = 0;
// var T = 0;
// var DownRange = [];   // list;
// var vg = 0;
// var Gamma = [];   // list;
// var K4 = [];   // list;
// var dydt = 0;
// var dvdt = 0;
// var dxdt = 0;
// var dhdt = 0;
// var K3 = [];   // list;
// var K2 = [];   // list;
// var K1 = [];   // list;
// var rho0 = 0;
// var SaveData = 0;
// var ThrustVacuum = 0;
// var P0 = 0;
// var ExitArea = 0;
// var EngineNumber = 0;
// var BurnOut = 0;
// var P = 0;
// var Diameter = 0;
// var Count = 0;
// var Temp4 = 0;
// var Hmax = 0;
// var Period = 0;
// var Proportional = 0;
// var Integral = 0;
// var Derivat = 0;
// var Gain = 0;
// var Previous = 0;
// var Delta = 0;
// var TimeWarp = [];   // list;
// var Temp = [];   // list;
// var h = 0;
// var v = 0;
// var position = 0;
// var Periapsis = 0;
// var y = 0;
// var vectorR = 0;
// var vectorV = 0;
// var Temp5 = 0;
// var Location = 0;
// var Temp6 = 0;
// var State_Vector = [];   // list;
// var Target = 0;
// var Destination = 0;
// var Target_Name = [];   // list;
// var Planet = [];   // list;

// ── Custom Instructions ──────────────────────────────
var Universal_Anomaly = Vz.DeclareCustomInstruction("Universal Anomaly", "position", "velocity", "deltatime").SetInstructions((position, velocity, deltatime) =>
{
    // Kepler Universal Variable, for all conic patch orbit
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
    Universal_Anomaly_X = x;
});

var F_and_G_function_from_Universal = Vz.DeclareCustomInstruction("F and G function from Universal", "anomaly", "position", "deltatime").SetInstructions((anomaly, position, deltatime) =>
{
    // input: Universal Anomaly since delta t, r0. Required Mu and Reciprocal SMA
    DeltaTime = deltatime;
    ro = position;
    x = anomaly;
    z = (alpha * (x ^ 2));
    f = (1 - (((x ^ 2) / ro) * Stump_C(z)));
    g = (DeltaTime - (((x ^ 3) / Vz.Sqrt(Mu)) * Stump_S(z)));
    // The output is f & g
});

var Orbit_Prediction_from = Vz.DeclareCustomInstruction("Orbit Prediction from", "position", "velocity", "time", "Mu").SetInstructions((position, velocity, time, Mu) =>
{
    // Predictor for guessing future orbit state vector, based on Lagrange formula
    DeltaTime = time;
    Mu = Mu;
    ro = Vz.Length(position);
    vo = Vz.Length(velocity);
    vro = (Vz.Dot(position, velocity) / Vz.Length(position));
    alpha = ((2 / ro) - ((vo ^ 2) / Mu));
    Universal_Anomaly(position, velocity, DeltaTime);
    F_and_G_function_from_Universal(Universal_Anomaly_X, ro, DeltaTime);
    vectorR2 = ((f * position) + (g * velocity));
    r = Vz.Length(vectorR2);
    F__and_G__from_Universal(Universal_Anomaly_X, ro, r, DeltaTime);
    vectorV2 = ((fdot * position) + (gdot * velocity));
    // Output Variable = VectorR2 & VectorV2
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

// for elliptic & hyperbolic orbit
var Eccentric_Anomaly_from_e__ = Vz.DeclareCustomInstruction("Eccentric Anomaly from e =", "eccentricity", "mean_anomaly").SetInstructions((eccentricity, mean_anomaly) =>
{
    // Newton's Method to solve Kepler's Equation, given Eccentricity & Mean Anomaly
    Error = 0.00000001;
    using (new If((eccentricity <= 1)))
    {
        using (new If((mean_anomaly < pi())))
        {
            E = (mean_anomaly + (eccentricity / 2));
        }
        using (new Else())
        {
            E = (mean_anomaly - (eccentricity / 2));
        }
        ratio = 1;
        using (new While((Vz.Abs(ratio) > Error)))
        {
            ratio = (((E - (eccentricity * Vz.Sin(E))) - mean_anomaly) / (1 - (eccentricity * Vz.Cos(E))));
            E = (E - ratio);
        }
    }
    using (new Else())
    {
        // For Hyperbola, e > 1
        E = mean_anomaly;
        ratio = 1;
        using (new While((Vz.Abs(ratio) > Error)))
        {
            ratio = ((((eccentricity * Sinh(E)) - E) - mean_anomaly) / ((eccentricity * Cosh(E)) - 1));
            E = (E - ratio);
        }
    }
    // Output Value is E, Eccentric Anomaly
});

// Orbital Parameter
// Looks confusing? Me too!
var Count_Down = Vz.DeclareCustomInstruction("Count Down", "time").SetInstructions((time) =>
{
    using (new For("i").From(time).To(0).By(-1))
    {
        Vz.Display(Vz.Format("Count Down <color=#ffdd00><b>{0:n0}</b></color> sec", i, ""), 7);
        Vz.WaitSeconds(1);
    }
});

var Current_Planet_Rotation = Vz.DeclareCustomInstruction("Current Planet Rotation").SetInstructions(() =>
{
    // Find planet rototation rate in rad/s
    Period = Vz.Abs(Vz.Planet(Vz.Craft.Orbit.Planet()).DayLength());
    PlanetRotationSpeed = ((2 * Vz.ExactEval("pi")) / Period);
});

var RK4__h__ = Vz.DeclareCustomInstruction("RK4, h =", "h", "tspan").SetInstructions((h, tspan) =>
{
    // Runge-Kutta 4th Order Numeric Solver
    Temp3 = h;
    Count = true;
    // Initial Value
    m = m0;
    time = 0;
    TotalTime = 0;
    Throttle = 0;
    velocity = 0;
    height = Vz.Craft.AltitudeASL();
    x = 0;
    Hmax = 0;
    // Calculating each step
    using (new For("i").From(1).To((tspan / h)).By(1))
    {
        using (new If(Count))
        {
            Clear_K1_K4();
            // RK1: f (t,y)
            function_ODE(time, velocity, gamma, x, height);
            Vz.ListAdd(K1, (h * dvdt));
            Vz.ListAdd(K1, (h * dydt));
            Vz.ListAdd(K1, (h * dxdt));
            Vz.ListAdd(K1, (h * dhdt));
            // RK2, f (t+(h/2),y+(k1/2))
            function_ODE((time + (h / 2)), (velocity + (Vz.ListGet(K1, 1) / 2)), (gamma + (Vz.ListGet(K1, 2) / 2)), (x + (Vz.ListGet(K1, 3) / 2)), (height + (Vz.ListGet(K1, 4) / 2)));
            Vz.ListAdd(K2, (h * dvdt));
            Vz.ListAdd(K2, (h * dydt));
            Vz.ListAdd(K2, (h * dxdt));
            Vz.ListAdd(K2, (h * dhdt));
            // RK3, f (t+(h/2),y+(k2/2))
            function_ODE((time + (h / 2)), (velocity + (Vz.ListGet(K2, 1) / 2)), (gamma + (Vz.ListGet(K2, 2) / 2)), (x + (Vz.ListGet(K2, 3) / 2)), (height + (Vz.ListGet(K2, 4) / 2)));
            Vz.ListAdd(K3, (h * dvdt));
            Vz.ListAdd(K3, (h * dydt));
            Vz.ListAdd(K3, (h * dxdt));
            Vz.ListAdd(K3, (h * dhdt));
            // RK4, f (t+h,y+k3)
            function_ODE((time + h), (velocity + Vz.ListGet(K3, 1)), (gamma + Vz.ListGet(K3, 2)), (x + Vz.ListGet(K3, 3)), (height + Vz.ListGet(K3, 4)));
            Vz.ListAdd(K4, (h * dvdt));
            Vz.ListAdd(K4, (h * dydt));
            Vz.ListAdd(K4, (h * dxdt));
            Vz.ListAdd(K4, (h * dhdt));
            // solution value
            time = (time + h);
            velocity = (velocity + (((Vz.ListGet(K1, 1) + (2 * Vz.ListGet(K2, 1))) + ((2 * Vz.ListGet(K3, 1)) + Vz.ListGet(K4, 1))) / 6));
            gamma = (gamma + (((Vz.ListGet(K1, 2) + (2 * Vz.ListGet(K2, 2))) + ((2 * Vz.ListGet(K3, 2)) + Vz.ListGet(K4, 2))) / 6));
            x = (x + (((Vz.ListGet(K1, 3) + (2 * Vz.ListGet(K2, 3))) + ((2 * Vz.ListGet(K3, 3)) + Vz.ListGet(K4, 3))) / 6));
            height = (height + (((Vz.ListGet(K1, 4) + (2 * Vz.ListGet(K2, 4))) + ((2 * Vz.ListGet(K3, 4)) + Vz.ListGet(K4, 4))) / 6));
            Hmax = Vz.Max(Hmax, height);
            TotalTime = Vz.Max(time, TotalTime);
            using (new If((SaveData && ((i % (1 / h)) == 0))))
            {
                Vz.ListAdd(Velocity, (Vz.Round((velocity * 10)) / 10));
                Vz.ListAdd(Gamma, (Vz.Round((Vz.Rad2Deg(gamma) * 1000)) / 1000));
                Vz.ListAdd(DownRange, (Vz.Round((x * 10)) / 10));
                Vz.ListAdd(Height, (Vz.Round((height * 10)) / 10));
                using (new If(((height > 1000) && (T == 0))))
                {
                    Count = false;
                }
            }
            using (new If(((gamma < 0) && (height < 800))))
            {
                Count = false;
            }
            using (new If(((height > 225000) && (x > 550000))))
            {
                Count = false;
            }
        }
    }
});

var Staging = Vz.DeclareCustomInstruction("Staging").SetInstructions(() =>
{
    Vz.SetTimeModeAttr("Normal");
    Vz.SetInput(CraftInput.Throttle, 0);
    using (new WaitUntil((Vz.Craft.Performance.CurrentThrust() == 0))) { }
    Vz.WaitSeconds(0.5);
    Vz.ActivateStage();
    Vz.WaitSeconds(0.5);
});

var Time_Wrap = Vz.DeclareCustomInstruction("Time Wrap", "time").SetInstructions((time) =>
{
    TimeWrap = 0;
    using (new For("i").From(1).To(Vz.ListLength(TimeWarp)).By(1))
    {
        using (new If(((time * 4) > Vz.ListGet(TimeWarp, i))))
        {
            TimeWrap = i;
        }
    }
    using (new If((time < 2)))
    {
        Vz.SetTimeModeAttr("Normal");
    }
    using (new ElseIf((TimeWrap == 0)))
    {
        Vz.SetTimeModeAttr("FastForward");
    }
    using (new ElseIf((TimeWrap == 1)))
    {
        Vz.SetTimeModeAttr("TimeWarp1");
    }
    using (new ElseIf((TimeWrap == 2)))
    {
        Vz.SetTimeModeAttr("TimeWarp2");
    }
    using (new ElseIf((TimeWrap == 3)))
    {
        Vz.SetTimeModeAttr("TimeWarp3");
    }
    using (new ElseIf((TimeWrap == 4)))
    {
        Vz.SetTimeModeAttr("TimeWarp4");
    }
    using (new ElseIf((TimeWrap == 5)))
    {
        Vz.SetTimeModeAttr("TimeWarp5");
    }
    using (new ElseIf((TimeWrap == 6)))
    {
        Vz.SetTimeModeAttr("TimeWarp6");
    }
    using (new ElseIf((TimeWrap == 7)))
    {
        Vz.SetTimeModeAttr("TimeWarp7");
    }
    using (new ElseIf((TimeWrap == 8)))
    {
        Vz.SetTimeModeAttr("TimeWarp8");
    }
    using (new ElseIf((TimeWrap == 9)))
    {
        Vz.SetTimeModeAttr("TimeWarp9");
    }
});

var Gibbs = Vz.DeclareCustomInstruction("Gibbs", "R1", "R2", "R3", "Mu").SetInstructions((R1, R2, R3, Mu) =>
{
    // Gibbs Method of Orbit Determination, return orbital state vector, all vector must be coplanar
    Temp1 = Vz.Cross(R1, R2);
    Temp2 = Vz.Cross(R2, R3);
    Temp3 = Vz.Cross(R3, R1);
    N = (((Vz.Length(R1) * Temp2) + (Vz.Length(R2) * Temp3)) + (Vz.Length(R3) * Temp1));
    D = ((Temp1 + Temp2) + Temp3);
    S = (((R1 * (Vz.Length(R2) - Vz.Length(R3))) + (R2 * (Vz.Length(R3) - Vz.Length(R1)))) + (R3 * (Vz.Length(R1) - Vz.Length(R2))));
    vectorR2 = R2;
    vectorV2 = (Vz.Sqrt(((Mu / Vz.Length(N)) / Vz.Length(D))) * ((Vz.Cross(D, R2) / Vz.Length(R2)) + S));
    // Output value is VectorV2
});

var Orbit_Launch_Window__RA = Vz.DeclareCustomInstruction("Orbit Launch Window, RA", "RA", "inclination").SetInstructions((RA, inclination) =>
{
    // Formula for orbital launch with AN opportunity, given variable total time to orbit
    Vz.ListClear(Temp);
    Node = Ascending_Node(RA);
    OrbitalPlane = Orbit_Plane_Normal(Node, inclination);
    inclination = Vz.Max(inclination, Vz.Abs(Vz.PosToLatLongAgl(Vz.Craft.Nav.Position()).x));
    theta = Vz.Rad2Deg(Vz.Asin((Vz.Tan(Vz.Deg2Rad((90 - inclination))) * Vz.Tan(Vz.Deg2Rad(Vz.PosToLatLongAgl(Vz.Craft.Nav.Position()).x)))));
    Temp3 = Vz.Rad2Deg((PlanetRotationSpeed * (0.33 * TotalTime)));
    Vz.ListAdd(Temp, Rotate(Node, Vz.Vec(0, 1, 0), ((-1 * theta) + Temp3)));
    Vz.ListAdd(Temp, Rotate((-1 * Node), Vz.Vec(0, 1, 0), (theta + Temp3)));
    Temp1 = Vz.Angle(Rejection_of(Vz.Craft.Nav.Position(), Vz.Vec(0, 1, 0)), Vz.ListGet(Temp, 1));
    Vz.ListAdd(Temp, ((Vz.Cross(Vz.ListGet(Temp, 1), Rejection_of(Vz.Craft.Nav.Position(), Vz.Vec(0, 1, 0))).y > 0) ? Temp1 : (360 - Temp1)));
    Temp2 = Vz.Angle(Rejection_of(Vz.Craft.Nav.Position(), Vz.Vec(0, 1, 0)), Vz.ListGet(Temp, 2));
    Vz.ListAdd(Temp, ((Vz.Cross(Vz.ListGet(Temp, 2), Rejection_of(Vz.Craft.Nav.Position(), Vz.Vec(0, 1, 0))).y > 0) ? Temp2 : (360 - Temp2)));
    time = (Vz.Deg2Rad(Vz.Min(Vz.ListGet(Temp, 3), Vz.ListGet(Temp, 4))) / PlanetRotationSpeed);
    Warp_Time("Waiting Launch Window", time);
});

var Set_Heading_to_Azimuth = Vz.DeclareCustomInstruction("Set Heading to Azimuth", "OrbitalPlane").SetInstructions((OrbitalPlane) =>
{
    // Azimuth heading to orbital plane
    Vector = (Vz.Norm(Vz.Cross(Vz.Craft.Nav.Position(), OrbitalPlane)) * velocity);
    HeadingError = (Vz.Dot((-1 * OrbitalPlane), Vz.Craft.Nav.Position()) * 1.1);
    Heading = (Vz.Dot(Vz.Project(Vz.Craft.Velocity.Orbital(), (-1 * OrbitalPlane)), (-1 * OrbitalPlane)) * 30);
    TargetHeading = (TargetHeading + ((HeadingError + Heading) * 0.0015));
    using (new If((((HeadingError + Heading) * Temp4) < 0)))
    {
        TargetHeading = 0;
    }
    Temp4 = (HeadingError + Heading);
    Azimuth = (Vector + (OrbitalPlane * ((HeadingError + Heading) + TargetHeading)));
    Vz.SetTargetHeading(TargetHeadingProperty.Heading, Angle(Azimuth, Vz.Craft.Nav.North(), Vz.Craft.Nav.East()));
});

var Calculating_Launch_Trajectory__Craft_s_Diameter = Vz.DeclareCustomInstruction("Calculating Launch Trajectory, Craft's Diameter", "diameter").SetInstructions((diameter) =>
{
    // Initial Craft value, set by yourself
    R = Vz.Planet(Vz.Craft.Orbit.Planet()).Radius();
    h_Turn = (50 + Vz.Craft.AltitudeASL());
    Diameter = diameter;
    BurnTime = 999;
    height = 0;
    Hmax = 0;
    Temp4 = 0;
    SaveData = false;
    Temp1 = Vz.Planet(Vz.Craft.Orbit.Planet()).AtmosphereDepth();
    using (new For("i").From(1).To(49).By(1))
    {
        Temp4 = (89.5 + (i / 100));
        Gravity_Turn__h(h_Turn, Temp4, BurnTime, 500, 1);
        using (new If(((Hmax > Temp1) || (height > Temp1))))
        {
            Vz.Break();
        }
    }
    // Smooth Iteration
    Temp4 = (Temp4 - 0.05);
    using (new For("i").From(1).To(99).By(1))
    {
        Temp4 = (Temp4 + (i / 100));
        Gravity_Turn__h(h_Turn, Temp4, BurnTime, 500, 0.1);
        using (new If(((Hmax > Temp1) || (height > Temp1))))
        {
            Vz.Break();
        }
        using (new If((Temp4 >= 89.98)))
        {
            Vz.Break();
        }
    }
    Vz.WaitSeconds(0);
    SaveData = true;
    Temp1 = Hmax;
    Temp2 = TotalTime;
    Hmax = 0;
    Gravity_Turn__h(h_Turn, Temp4, BurnTime, 500, 0.01);
    TotalTime = time;
    Vz.Display(Vz.Format("<b><color=#ffdd11>Calculation Completed!</color></b><br>Time to Orbit= {0:n1} sec<br>Altitude= {1:n1} m", Temp2, Temp1, ""), 7);
    Vz.WaitSeconds(1);
});

var Impulsive_Burn = Vz.DeclareCustomInstruction("Impulsive Burn", "text", "true_anomaly", "timer", "vector").SetInstructions((text, true_anomaly, timer, vector) =>
{
    // For burn time less than a minute
    Perifocal_Frame_True_Anomaly(true_anomaly, Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()), Angular_Momentum(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()));
    CountDown = 0;
    vectorR = position;
    vectorV = velocity;
    OrbitVelocity = (velocity + vector);
    BurnOut = Vz.Sqrt(((Vz.Length(vector) / 2) / (Max_Acceleration() * Vz.ExactEval("FlightData.WeightedThrottleResponseTime"))));
    BurnTime = (((BurnOut < Vz.ExactEval("FlightData.WeightedThrottleResponseTime")) ? (4 * BurnOut) : (Burn_Time((Vz.Length(vector) / 2)) + Vz.ExactEval("FlightData.WeightedThrottleResponseTime/2"))) + Vz.Time.DeltaTime());
    BurnTime = ((Vz.Craft.Performance.StageDeltaV() < Vz.Length(vector)) ? (1.1 * BurnTime) : BurnTime);
    using (new Repeat((((Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) > 1) && (true_anomaly == 0)) ? 2 : 1)))
    {
        Time_from_True_Anomaly(True_Anomaly(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()), true_anomaly, Mu(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
        time = ((TotalTime + timer) - BurnTime);
        Wait_Time_for(((CountDown > 0) ? "Retro Burn" : text), time, vector);
        CountDown += 1;
    }
    OrbitDeltaV = Vz.Max(1, (2 * Vz.Length(vector)));
    Gain = 0;
    Vz.SetInput(CraftInput.TranslationMode, 0);
    Vz.Broadcast(BroadCastType.Local, "Auto_Staging", 0);
    using (new While((Vz.Craft.Fuel.AllStages() > 0)))
    {
        Eccentricity_Vector___Angular_Momentum(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
        Perifocal_Frame_True_Anomaly(Angle(vectorR, E, Vz.Cross(E, h)), Vz.Length(E), h);
        Temp4 = Vz.Dot(Vz.Norm(vector), (OrbitVelocity - velocity));
        Previous = OrbitDeltaV;
        OrbitDeltaV = Vz.Min(Temp4, OrbitDeltaV);
        Vz.LockNavSphere(LockNavSphereIndicatorType.Current);
        Vz.LockNavSphere(LockNavSphereIndicatorType.Vector, vector);
        using (new If((OrbitDeltaV > 0.00025)))
        {
            using (new If((OrbitDeltaV > Vz.Max(10, (Max_Acceleration() + ((Vz.ExactEval("FlightData.WeightedThrottleResponseTime/2") + Vz.Time.DeltaTime()) * Max_Acceleration()))))))
            {
                Vz.SetTimeModeAttr("FastForward");
                Vz.SetInput(CraftInput.Throttle, 1);
            }
            using (new ElseIf((OrbitDeltaV > 2)))
            {
                Vz.SetTimeModeAttr("Normal");
                Vz.SetInput(CraftInput.Throttle, Vz.Max(0.0001, Vz.Min(1, Gain)));
            }
            using (new Else())
            {
                Temp5 = (OrbitVelocity - velocity);
                CD = (((Vz.Abs((OrbitDeltaV - Previous)) * Vz.Time.DeltaTime()) < 0.0005) ? true : false);
                Vector = Vz.PartPciToLocal(-0, (Vz.Norm(Temp5) * Vz.Max(((CD && (OrbitDeltaV > 0.025)) ? 0.25 : 0.05), (20 * Vz.Log10(((2 * OrbitDeltaV) + 1))))));
                Vz.SetActivationGroup(10, true);
                Vz.SetInput(CraftInput.Throttle, 0);
                Vz.SetInput(CraftInput.TranslationMode, 1);
                Vz.SetInput(CraftInput.TranslateForward, Vector.z);
                Vz.SetInput(CraftInput.TranslateRight, Vector.x);
                Vz.SetInput(CraftInput.TranslateUp, Vector.y);
            }
        }
        using (new Else())
        {
            Vz.LockNavSphere(LockNavSphereIndicatorType.None);
            Temp5 = 0;
            OrbitDeltaV = 0;
            Vector = 0;
            Vz.SetInput(CraftInput.TranslationMode, 0);
            Vz.SetInput(CraftInput.Throttle, 0);
            Vz.Break();
        }
        Vz.Display(Display(text, "Delta V", OrbitDeltaV, 3, "m/s", "Timer", (((OrbitDeltaV / Vz.Craft.Performance.StageDeltaV()) * Vz.Craft.Performance.BurnTime()) / Current_Throttle()), 1, "sec"), 7);
        PID_Control(Vz.Max(0.0005, OrbitDeltaV), (0.9 / Max_Acceleration()), (0.01 / Max_Acceleration()), (1.6 / Max_Acceleration()));
        Vz.LockNavSphere(LockNavSphereIndicatorType.None);
    }
    using (new If((Vz.Craft.Fuel.AllStages() == 0)))
    {
        Vz.Display("Fuel Depleted !!!", 7);
        Vz.WaitSeconds(1);
    }
});

var function_ODE = Vz.DeclareCustomInstruction("function ODE", "t", "velocity", "gamma", "range", "height").SetInstructions((t, velocity, gamma, range, height) =>
{
    // Derivation formula for gravity turn
    Display = "<b><color=#ffdd11>Calculating Gravity Turn</color></b><br>";
    Vz.Display(Vz.Join(Display, Vz.Format("Precision= {0:n2} sec<br>Gamma= {1:n2}°<br>Time= {2:n1} sec<br>Altitude= {3:n0} m", Temp3, Temp4, time, height, ""), ""), 7);
    // y1 = velocity (v), y2 = gamma (ɣ), y3 = downrange (x), y4 = height (h)
    using (new If(((t < BurnTime) && ((m - mfinal) > 0))))
    {
        T = Thrust;
        Throttle = Vz.Min(1, (T / (ThrustVacuum - ((ExitArea * P) * EngineNumber))));
        m = (m0 - ((mdot * Throttle) * t));
    }
    using (new Else())
    {
        m = (m0 - (mdot * BurnOut));
        T = 0;
        Throttle = 0;
    }
    g = (g0 / ((1 + (height / R)) ^ 2));
    rho = (rho0 * (e() ^ ((-1 * height) / h_Scale)));
    P = (P0 * (e() ^ ((-1 * height) / h_Scale)));
    D = ((0.5 * rho) * ((velocity ^ 2) * (A * CD)));
    Q = ((0.5 * rho) * velocity);
    // Differential equation
    dvdt = ((height < h_Turn) ? (((T / m) - (D / m)) - g) : (((T / m) - (D / m)) - (g * Vz.Sin(gamma))));
    dydt = ((height < h_Turn) ? 0 : ((-1 / velocity) * ((g - ((velocity ^ 2) / (R + height))) * Vz.Cos(gamma))));
    dxdt = ((height < h_Turn) ? 0 : ((R / (R + height)) * (velocity * Vz.Cos(gamma))));
    dhdt = ((height < h_Turn) ? velocity : (velocity * Vz.Sin(gamma)));
});

var Craft = Vz.DeclareCustomInstruction("Craft", "vector").SetInstructions((vector) =>
{
    Vz.LockNavSphere(LockNavSphereIndicatorType.None);
    Vz.SetTargetHeading(TargetHeadingProperty.Pitch, (90 - Vz.Angle(Vz.Cross(Vz.Craft.Nav.North(), Vz.Craft.Nav.East()), vector)));
    Vz.SetTargetHeading(TargetHeadingProperty.Heading, Angle(vector, Vz.Craft.Nav.North(), Vz.Craft.Nav.East()));
});

var Launch__Apoapsis = Vz.DeclareCustomInstruction("Launch, Apoapsis", "altitude", "RA", "inclination").SetInstructions((altitude, RA, inclination) =>
{
    // minimal altitude 100 km
    altitude = Vz.Max(100000, altitude);
    using (new If((!((inclination == 0) && (RA == 0)))))
    {
        Orbit_Launch_Window__RA(RA, inclination);
    }
    using (new Else())
    {
        OrbitalPlane = Vz.Vec(0, 1, 0);
        Vz.WaitSeconds(1);
    }
    Vz.SetActivationGroup(10, true);
    Temp1 = Vz.Time.MissionTime();
    Temp4 = 0;
    TargetHeading = 0;
    velocity = Orbital_Speed((Vz.Planet(Vz.Craft.Orbit.Planet()).Radius() + altitude), (Vz.Planet(Vz.Craft.Orbit.Planet()).Radius() + altitude));
    using (new While(true))
    {
        Vz.Display(Display("Gravity Turn", "Apoapsis", (Vz.Craft.Orbit.Apoapsis() / 1000), 2, "km", "Orbital Plane", Vz.Angle(Vz.Craft.Nav.Position(), Rejection_of(Vz.Craft.Nav.Position(), OrbitalPlane)), 3, "°"), 7);
        time = (Vz.Time.MissionTime() - Temp1);
        theta = (90 - Vz.Angle(Vz.Cross(Vz.Craft.Nav.North(), Vz.Craft.Nav.East()), Vz.Craft.Velocity.Surface()));
        gamma = Vz.ListGet(Gamma, Vz.Ceiling(time));
        Error = ((time > 2) ? (gamma - theta) : 0);
        Set_Heading_to_Azimuth(OrbitalPlane);
        Vz.SetTargetHeading(TargetHeadingProperty.Pitch, ((time < 2) ? Vz.ListGet(Gamma, 2) : ((time >= Vz.ListLength(Gamma)) ? Vz.ListGet(Gamma, Vz.ListLength(Gamma)) : (gamma + (0.75 * Error)))));
        Vz.SetInput(CraftInput.Throttle, (Thrust / Vz.Craft.Performance.MaxThrust()));
        using (new If((time > 6)))
        {
            Vz.SetTimeModeAttr("FastForward");
        }
        using (new If(((time - 0.5) > Vz.ListLength(Gamma))))
        {
            Vz.Break();
        }
        Vz.WaitSeconds(0);
    }
    Vz.WaitSeconds(0.1);
    Staging();
    Vz.WaitSeconds(1);
    Temp4 = 0;
    TargetHeading = 0;
    Input = (altitude - Vz.Craft.Orbit.Apoapsis());
    using (new While((Vz.Craft.Orbit.Apoapsis() < (altitude - 0.005))))
    {
        Vz.Display(Display("Gravity Turn", "Apoapsis", (Vz.Craft.Orbit.Apoapsis() / 1000), 3, "km", "Target", (altitude / 1000), 0, "km"), 7);
        Set_Heading_to_Azimuth(OrbitalPlane);
        Vz.SetTargetHeading(TargetHeadingProperty.Pitch, (90 - Vz.Angle(Vz.Cross(Vz.Craft.Nav.North(), Vz.Craft.Nav.East()), Vz.Craft.Velocity.Orbital())));
        PID_Control(((altitude - Vz.Craft.Orbit.Apoapsis()) / Input), 1.5, 0.1, 2.5);
        using (new If(((altitude - Vz.Craft.Orbit.Apoapsis()) < (0.4 * Input))))
        {
            Vz.SetTimeModeAttr("Normal");
            Vz.SetInput(CraftInput.Throttle, Vz.Max(0.0002, Vz.Min(1, (Gain * (Thrust / Vz.Craft.Performance.MaxThrust())))));
        }
        using (new Else())
        {
            Vz.SetTimeModeAttr("FastForward");
            Vz.SetInput(CraftInput.Throttle, (Thrust / Vz.Craft.Performance.MaxThrust()));
        }
    }
    Vz.SetInput(CraftInput.Throttle, 0);
    Vz.WaitSeconds(1);
    // to Circularization
});

var Warp_Time = Vz.DeclareCustomInstruction("Warp Time", "title", "time").SetInstructions((title, time) =>
{
    Vz.WaitSeconds(0);
    time = (time + Vz.Time.TotalTime());
    Vz.SetTimeModeAttr("FastForward");
    Vz.WaitSeconds(1);
    using (new While((time > Vz.Time.TotalTime())))
    {
        Vz.Display(Vz.Format("<b>{0} | {1}</b>", title, Format_Time((time - Vz.Time.TotalTime())), ""), 7);
        Time_Wrap((time - Vz.Time.TotalTime()));
        Vz.WaitSeconds(0);
    }
});

var Atmospheric_Data = Vz.DeclareCustomInstruction("Atmospheric Data").SetInstructions(() =>
{
    h_Scale = ((-1 * Vz.Planet(Vz.Craft.Orbit.Planet()).AtmosphereDepth()) / Vz.Ln(0.001));
    rho0 = (Vz.Craft.Atmosphere.AirDensity() / (e() ^ ((-1 * Vz.Craft.AltitudeASL()) / h_Scale)));
    P0 = ((Vz.Craft.Atmosphere.AirPressure() / Vz.Craft.Atmosphere.AirDensity()) * rho0);
});

var Eccentricity_Vector___Angular_Momentum = Vz.DeclareCustomInstruction("Eccentricity Vector & Angular Momentum", "position", "velocity").SetInstructions((position, velocity) =>
{
    E = Position_Z_up(Eccentricity_Vector(position, velocity));
    h = Position_Z_up(Angular_Momentum(position, velocity));
});

var Lagrange_F_and_G_function = Vz.DeclareCustomInstruction("Lagrange F and G function", "position", "velocity", "true_anomaly", "Mu").SetInstructions((position, velocity, true_anomaly, Mu) =>
{
    // Calculate Lagrange F and G coefficient from the change in True Anomaly
    h = Vz.Length(Vz.Cross(position, velocity));
    vro = (Vz.Dot(velocity, position) / Vz.Length(position));
    ro = Vz.Length(position);
    S = Vz.Sin(Vz.Deg2Rad(true_anomaly));
    C = Vz.Cos(Vz.Deg2Rad(true_anomaly));
    r = (((h ^ 2) / Mu) / (1 + (((((h ^ 2) / (Mu * ro)) - 1) * C) - (((h * vro) * S) / Mu))));
    f = (1 - (((Mu * r) * (1 - C)) / (h ^ 2)));
    g = (((r * ro) * S) / h);
    // time derivative of F and G Lagrange coefficient
    fdot = ((Mu / h) * (((vro / h) * (1 - C)) - (S / ro)));
    gdot = (1 - (((Mu * ro) / (h ^ 2)) * (1 - C)));
});

var Orbital_State_Vector = Vz.DeclareCustomInstruction("Orbital State Vector", "position", "velocity", "true_anomaly", "Mu").SetInstructions((position, velocity, true_anomaly, Mu) =>
{
    // Compute State Vector from the initial State Vector and the change in True Anomaly
    Lagrange_F_and_G_function(position, velocity, true_anomaly, Mu);
    vectorR2 = ((f * position) + (g * velocity));
    vectorV2 = ((fdot * position) + (gdot * velocity));
});

var Gravity_Turn__h = Vz.DeclareCustomInstruction("Gravity Turn, h", "height", "angle", "burnout", "timeout", "second").SetInstructions((height, angle, burnout, timeout, second) =>
{
    // RK4 ODE solver
    Clear_Saved_List();
    h_Turn = height;
    gamma = Vz.Deg2Rad(angle);
    BurnTime = burnout;
    P = Vz.Craft.Atmosphere.AirPressure();
    A = (pi() * ((Diameter / 2) ^ 2));
    CD = 0.5;
    RK4__h__(second, timeout);
    // Numerical Solution
});

var Clear_K1_K4 = Vz.DeclareCustomInstruction("Clear K1-K4").SetInstructions(() =>
{
    Vz.ListClear(K1);
    Vz.ListClear(K2);
    Vz.ListClear(K3);
    Vz.ListClear(K4);
});

var PID_Control = Vz.DeclareCustomInstruction("PID Control", "delta", "kP", "kI", "kD").SetInstructions((delta, kP, kI, kD) =>
{
    Delta = delta;
    Proportional = (kP * Delta);
    Integral = (kI * (Delta * Vz.Time.DeltaTime()));
    Derivat = (kD * (Delta - Previous));
    Gain = (Proportional + (Integral + Derivat));
    Vz.WaitSeconds(0);
    Previous = Delta;
});

var Wait_Time_for = Vz.DeclareCustomInstruction("Wait Time for", "text", "time", "vector").SetInstructions((text, time, vector) =>
{
    Heading = true;
    Vz.Broadcast(BroadCastType.Local, "Craft Heading", vector);
    Warp_Time(Vz.Format("<color=#ffdd11>Waiting {0}</color>", text, ""), time);
    Heading = false;
    Vz.WaitSeconds(0);
});

var Time_from_True_Anomaly = Vz.DeclareCustomInstruction("Time from True Anomaly", "anomaly1", "anomaly2", "Mu", "position", "velocity").SetInstructions((anomaly1, anomaly2, Mu, position, velocity) =>
{
    // Calculate delta time between true anomaly
    Time_Since_Periapsis(position, velocity, anomaly1, Mu);
    Temp1 = TimeSincePerigee;
    T = (((2 * pi()) * ((Vz.Length(h) / Vz.Sqrt((1 - (Eccentricity ^ 2)))) ^ 3)) / (Mu ^ 2));
    Time_Since_Periapsis(position, velocity, anomaly2, Mu);
    Temp2 = TimeSincePerigee;
    using (new If((Eccentricity < 1)))
    {
        TotalTime = ((Temp1 > Temp2) ? ((T - Temp1) + Temp2) : (Temp2 - Temp1));
    }
    using (new Else())
    {
        TotalTime = (Temp2 - Temp1);
    }
});

var Time_Since_Periapsis = Vz.DeclareCustomInstruction("Time Since Periapsis", "position", "velocity", "true_anomaly", "Mu").SetInstructions((position, velocity, true_anomaly, Mu) =>
{
    // Calculate time from Perigee to True Anomaly, not accurate for elliptic orbit with e > 0.65 due to Laplace's Limit
    true_anomaly = Vz.Deg2Rad(true_anomaly);
    h = Angular_Momentum(position, velocity);
    Eccentricity = Vz.Length(((Vz.Cross(Velocity_Z_up(velocity), h) / Mu) - Vz.Norm(Position_Z_up(position))));
    using (new If((Eccentricity < 1)))
    {
        E = (2 * Vz.Atan((Vz.Sqrt(((1 - Eccentricity) / (1 + Eccentricity))) * Vz.Tan((true_anomaly / 2)))));
        MeanAnomaly = (E - (Eccentricity * Vz.Sin(E)));
        TimeSincePerigee = ((((Vz.Length(h) ^ 3) / (Mu ^ 2)) / ((1 - (Eccentricity ^ 2)) ^ 1.5)) * MeanAnomaly);
    }
    using (new Else())
    {
        E = (2 * atanh((Vz.Sqrt(((Eccentricity - 1) / (Eccentricity + 1))) * Vz.Tan((true_anomaly / 2)))));
        MeanAnomaly = ((Eccentricity * Sinh(E)) - E);
        TimeSincePerigee = ((((Vz.Length(h) ^ 3) / (Mu ^ 2)) / (((Eccentricity ^ 2) - 1) ^ 1.5)) * MeanAnomaly);
    }
});

var Rendezvous = Vz.DeclareCustomInstruction("Rendezvous", "title", "position1", "velocity1", "position2", "velocity2", "transfer_time", "delay", "precision", "Mu").SetInstructions((title, position1, velocity1, position2, velocity2, transfer_time, delay, precision, Mu) =>
{
    // Output Variable: VectorV1 & VectorV2
    delay = Vz.Max(11, delay);
    precision = ((precision > 0) ? ((Vz.Planet(Target).Radius() + precision) * (Vz.Abs(precision) / precision)) : 0);
    Orbit_Prediction_from(position1, velocity1, delay, Mu);
    Temp1 = vectorR2;
    Temp2 = vectorV2;
    Orbit_Prediction_from(position2, velocity2, delay, Mu);
    Temp3 = vectorR2;
    Temp4 = vectorV2;
    Orbit_Prediction_from(Temp3, Temp4, transfer_time, Mu);
    Lambert_s_Problem_Solver(Temp1, (Vz.Norm(vectorR2) * (Vz.Length(vectorR2) + precision)), transfer_time, Mu);
    Eccentricity_Vector___Angular_Momentum(Temp1, Temp2);
    Impulsive_Burn(title, Angle(Temp1, E, Vz.Cross(E, h)), 0, (VectorV1 - Temp2));
    Vz.Display("Done !", 7);
});

using (new OnReceiveMessage("Craft Heading"))
{
    using (new While(Heading))
    {
        Craft(data);
        Vz.WaitSeconds(0.1);
    }
}

var Target_Node = Vz.DeclareCustomInstruction("Target Node", "target").SetInstructions((target) =>
{
    Vz.SetTimeModeAttr("Normal");
    Vz.WaitSeconds(0);
    Vz.TargetNode(target);
    Vz.WaitSeconds(0);
    using (new WaitUntil(String_Input(Vz.Craft.Target.Name(), target))) { }
    Vz.WaitSeconds(0.2);
});

var Matching_Orbit_Inclination = Vz.DeclareCustomInstruction("Matching Orbit Inclination", "inclination", "RA", "error").SetInstructions((inclination, RA, error) =>
{
    // Comment Text
    Error = Vz.Max(0.001, error);
    Plane("Plane Node", inclination, RA);
    Vz.Display("Please Wait !", 7);
    Vz.WaitSeconds(0.5);
    using (new If((Vz.Angle(OrbitalPlane, Position_Z_up(Angular_Momentum(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()))) > Error)))
    {
        Plane("Correction", inclination, RA);
    }
    Vz.Display("Orbit Plane Match !", 7);
});

var Transfer_Window_to = Vz.DeclareCustomInstruction("Transfer Window to", "target").SetInstructions((target) =>
{
    // Total Time = item no 6 on State_Vector List
    Target_Node(target);
    Vz.ListClear(State_Vector);
    using (new If(String_Input(Vz.Craft.Orbit.Planet(), Vz.Planet(Vz.Craft.Target.Name()).Parent())))
    {
        Vz.ListAdd(State_Vector, Vz.Craft.Nav.Position());
        Vz.ListAdd(State_Vector, Vz.Craft.Velocity.Orbital());
        Vz.ListAdd(State_Vector, Vz.Craft.Target.Position());
        Vz.ListAdd(State_Vector, Vz.Craft.Target.Velocity());
        Vz.ListAdd(State_Vector, Mu());
    }
    using (new Else())
    {
        Vz.ListAdd(State_Vector, Parent_Solar_Position());
        Vz.ListAdd(State_Vector, Parent_Solar_Velocity());
        Vz.ListAdd(State_Vector, Target_Solar_Position());
        Vz.ListAdd(State_Vector, Target_Solar_Velocity());
        Vz.ListAdd(State_Vector, (G() * Vz.Planet(Parent_Star()).Mass()));
    }
    Vz.ListAdd(State_Vector, Vz.Time.TotalTime());
    Transfer_Window_from(Vz.ListGet(State_Vector, 1), Vz.ListGet(State_Vector, 2), Vz.ListGet(State_Vector, 3), Vz.ListGet(State_Vector, 4), Vz.ListGet(State_Vector, 5));
});

var Circularize_at = Vz.DeclareCustomInstruction("Circularize at", "altitude", "error").SetInstructions((altitude, error) =>
{
    // More precision means more correction
    error = Vz.Max(0.00002, error);
    Circular_Orbit("Circularization", altitude);
    Vz.Display("Please Wait !", 7);
    Vz.WaitSeconds(0.5);
    using (new If((Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) > error)))
    {
        Circular_Orbit("Correction", altitude);
    }
    Vz.Display("Circular !", 7);
});

var Warp_from = Vz.DeclareCustomInstruction("Warp from", "position", "velocity", "position2", "velocity2", "precision", "ETA").SetInstructions((position, velocity, position2, velocity2, precision, ETA) =>
{
    // Estimate time to target, output variable: transfer_time
    TransferTime = ETA;
    Mu = Mu();
    CountDown = (TransferTime / 2);
    Distance = Vz.Distance(position, position2);
    Location = Vz.Distance(position, (position2 + (Vz.Norm((position - position2)) * precision)));
    using (new While((CountDown > 0.001)))
    {
        Orbit_Prediction_from(position, velocity, TransferTime, Mu);
        Temp1 = vectorR2;
        Temp2 = vectorV2;
        Orbit_Prediction_from(position2, velocity2, TransferTime, Mu);
        Temp3 = vectorR2;
        Temp4 = vectorV2;
        Distance = Vz.Distance(Temp1, vectorR2);
        Location = Vz.Distance(Temp1, (vectorR2 + (Vz.Norm((Temp1 - vectorR2)) * precision)));
        TransferTime += ((Distance < precision) ? (-1 * CountDown) : CountDown);
        CountDown = (CountDown / 2);
    }
});

var Perifocal_Frame_of = Vz.DeclareCustomInstruction("Perifocal Frame of", "position", "velocity", "anomaly", "Mu").SetInstructions((position, velocity, anomaly, Mu) =>
{
    // Find orbital state vector given true anomaly
    Angular_Momentum___Eccentricity(position, velocity, Mu);
    x = Vz.Vec(1, 0, 0);
    y = Vz.Vec(0, 1, 0);
    S = Vz.Sin(Vz.Deg2Rad(anomaly));
    C = Vz.Cos(Vz.Deg2Rad(anomaly));
    r = (((Vz.Length(h) ^ 2) / (Mu * (1 + (Vz.Length(E) * C)))) * Vz.Vec(C, 0, S));
    v = ((Mu / Vz.Length(h)) * Vz.Vec((-1 * S), 0, (Vz.Length(E) + C)));
    Periapsis = Argument_of_Periapsis(position, velocity);
    RightAscension = Right_Ascension(position, velocity);
    Inclination = Inclination(position, velocity);
    position = Rotate(Rotate(Rotate(r, y, (-1 * Periapsis)), x, (-1 * Inclination)), y, (-1 * RightAscension));
    velocity = Rotate(Rotate(Rotate(v, y, (-1 * Periapsis)), x, (-1 * Inclination)), y, (-1 * RightAscension));
});

var Angular_Momentum___Eccentricity = Vz.DeclareCustomInstruction("Angular Momentum & Eccentricity", "position", "velocity", "Mu").SetInstructions((position, velocity, Mu) =>
{
    h = Vz.Cross(Position_Z_up(position), Velocity_Z_up(velocity));
    E = Position_Z_up(((Vz.Cross(Velocity_Z_up(velocity), h) / Mu) - Vz.Norm(Position_Z_up(position))));
});

var Perifocal_Frame_of_Vector = Vz.DeclareCustomInstruction("Perifocal Frame of Vector", "vector", "position", "velocity", "Mu").SetInstructions((vector, position, velocity, Mu) =>
{
    // Find orbital state vector given true anomaly and state vector
    Angular_Momentum___Eccentricity(position, velocity, Mu);
    TrueAnomaly = Angle(vector, E, Vz.Cross(E, Position_Z_up(h)));
    Perifocal_Frame_of(position, velocity, TrueAnomaly, Mu);
});

var Orbit_Transfer_to_Altitude = Vz.DeclareCustomInstruction("Orbit Transfer to Altitude", "altitude", "true_anomaly").SetInstructions((altitude, true_anomaly) =>
{
    Eccentricity_Vector___Angular_Momentum(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    Perifocal_Frame_True_Anomaly(true_anomaly, Vz.Length(E), h);
    SMA = ((Vz.Length(position) + (Vz.Planet(Vz.Craft.Orbit.Planet()).Radius() + altitude)) / 2);
    OrbitDeltaV = ((Orbital_Speed(Vz.Length(position), SMA) * Vz.Norm(velocity)) - velocity);
    Impulsive_Burn("Orbit Transfer", true_anomaly, 0, OrbitDeltaV);
    Circularize_at(altitude, 0);
});

var Transfer_Window_from = Vz.DeclareCustomInstruction("Transfer Window from", "position1", "velocity1", "position2", "velocity2", "Mu").SetInstructions((position1, velocity1, position2, velocity2, Mu) =>
{
    // Output: TransferTime & Countdown
    CountDown = 0;
    Count = 0;
    TransferAngle = 0;
    Integral = 0;
    Time_from_True_Anomaly(0, 180, Mu, position1, velocity1);
    Vz.ListAdd(State_Vector, TimeSincePerigee);
    Time_from_True_Anomaly(0, 180, Mu, position2, velocity2);
    Vz.ListAdd(State_Vector, TimeSincePerigee);
    Period = (Vz.Min(Vz.ListGet(State_Vector, 7), Vz.ListGet(State_Vector, 8)) / 180);
    using (new While(true))
    {
        Orbit_Prediction_from(Position_Z_up(position1), Velocity_Z_up(velocity1), CountDown, Mu);
        Temp5 = Position_Z_up(vectorR2);
        TransferVelocity = Velocity_Z_up(vectorV2);
        Perifocal_Frame_of_Vector((-1 * Temp5), position2, velocity2, Mu);
        R = Vz.Length(position);
        OrbitVelocity = Vz.Sqrt((Mu * ((2 / Vz.Length(Temp5)) - (1 / ((Vz.Length(Temp5) + R) / 2)))));
        OrbitDeltaV = (OrbitVelocity - Vz.Length(TransferVelocity));
        Time_from_True_Anomaly(0, 180, Mu, Temp5, (OrbitVelocity * Vz.Norm(TransferVelocity)));
        TransferTime = TotalTime;
        Previous = TransferAngle;
        Orbit_Prediction_from(Position_Z_up(position2), Velocity_Z_up(velocity2), (TransferTime + CountDown), Mu);
        h = Position_Z_up(Angular_Momentum(Position_Z_up(vectorR2), Velocity_Z_up(vectorV2)));
        TransferAngle = Angle(Rejection_of((-1 * Temp5), h), Position_Z_up(vectorR2), Vz.Cross(Position_Z_up(vectorR2), h));
        TransferAngle = ((TransferAngle > 180) ? (360 - TransferAngle) : TransferAngle);
        TimeWrap = Clamp(((0.25 * Period) * TransferAngle), (0.02 * Period), (30 * Period));
        using (new If((Different_Sign(TransferAngle, Previous) && (Vz.Abs(TransferAngle) < 10))))
        {
            Vz.Break();
        }
        CountDown += ((CountDown < 1) ? (0.1 * Period) : TimeWrap);
        Vz.Display(Vz.Format("Calculating | Angle {0:n1}° | Timelapse {1:n0} sec", TransferAngle, CountDown, ""), 7);
    }
});

var Orbital_Plane = Vz.DeclareCustomInstruction("Orbital Plane", "orbital_plane", "text").SetInstructions((orbital_plane, text) =>
{
    // Comment Text
    Eccentricity_Vector___Angular_Momentum(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    Node = Vz.Cross(orbital_plane, h);
    TrueAnomaly = Angle(Node, E, Vz.Cross(E, h));
    Perifocal_Frame_True_Anomaly(TrueAnomaly, Vz.Length(E), h);
    OrbitVelocity = (Vz.Length(velocity) * Vz.Norm(Vz.Cross(position, orbital_plane)));
    OrbitDeltaV = (OrbitVelocity - velocity);
    Impulsive_Burn(text, TrueAnomaly, 0, OrbitDeltaV);
});

var Circular_Orbit = Vz.DeclareCustomInstruction("Circular Orbit", "text", "altitude").SetInstructions((text, altitude) =>
{
    // Comment Text
    vectorR = (Vz.Planet(Vz.Craft.Orbit.Planet()).Radius() + altitude);
    using (new If((Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) < 1)))
    {
        TrueAnomaly = (((altitude > Vz.Craft.Orbit.Apoapsis()) && (altitude > 0)) ? 180 : ((altitude <= Vz.Craft.Orbit.Periapsis()) ? 0 : True_Anomaly_at_Radius(vectorR)));
        using (new If(((Vz.Abs((TrueAnomaly - True_Anomaly(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()))) < 2) && (Vz.Craft.Orbit.Periapsis() > 1000))))
        {
            TrueAnomaly = (0 - TrueAnomaly);
        }
    }
    using (new Else())
    {
        TrueAnomaly = 0;
    }
    Perifocal_Frame_True_Anomaly(TrueAnomaly, Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()), Angular_Momentum(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()));
    OrbitVelocity = (Orbital_Speed(Vz.Length(position), Vz.Length(position)) * Vz.Norm(Vz.Cross(Vz.Cross(position, velocity), position)));
    OrbitDeltaV = (OrbitVelocity - velocity);
    Impulsive_Burn(text, TrueAnomaly, 0, OrbitDeltaV);
    Vz.Display("Please Wait!", 7);
});

var Perifocal_Frame_True_Anomaly = Vz.DeclareCustomInstruction("Perifocal Frame True Anomaly", "true_anomaly", "eccentricity", "angular_momentum").SetInstructions((true_anomaly, eccentricity, angular_momentum) =>
{
    // Find orbital state vector given true anomaly
    x = Vz.Vec(1, 0, 0);
    y = Vz.Vec(0, 1, 0);
    S = Vz.Sin(Vz.Deg2Rad(true_anomaly));
    C = Vz.Cos(Vz.Deg2Rad(true_anomaly));
    r = (((Vz.Length(angular_momentum) ^ 2) / (Mu() * (1 + (eccentricity * C)))) * Vz.Vec(C, 0, S));
    v = ((Mu() / Vz.Length(angular_momentum)) * Vz.Vec((-1 * S), 0, (eccentricity + C)));
    Periapsis = Argument_of_Periapsis(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    RightAscension = Right_Ascension(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    Inclination = Inclination(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    position = Rotate(Rotate(Rotate(r, y, (-1 * Periapsis)), x, (-1 * Inclination)), y, (-1 * RightAscension));
    velocity = Rotate(Rotate(Rotate(v, y, (-1 * Periapsis)), x, (-1 * Inclination)), y, (-1 * RightAscension));
});

var Encounter = Vz.DeclareCustomInstruction("Encounter", "target", "position1", "velocity1", "position2", "velocity2", "periapsis", "ETA").SetInstructions((target, position1, velocity1, position2, velocity2, periapsis, ETA) =>
{
    // Comment Text
    Target_Node(target);
    Count = 1;
    Heading = 0;
    Output = Vz.Vec(0, 0, 0);
    HeadingError = Vz.Vec(Vz.Planet(target).SOI(), 0, 0);
    using (new While((Vz.Length(HeadingError) > 2500)))
    {
        Warp_from(position1, (velocity1 + Output), position2, velocity2, Vz.Planet(target).SOI(), ETA);
        Temp5 = (Temp1 - Temp3);
        Temp6 = (Temp2 - Temp4);
        Perifocal_Frame_of(Temp5, Temp6, 0, (G() * Vz.Planet(target).Mass()));
        TargetDelta = ((Vz.Cross(Temp5, Temp6).y > 0) ? (-1 * position) : position);
        TargetSMA = (Vz.Norm(TargetDelta) * ((Vz.Planet(target).Radius() + periapsis) + 2500));
        Previous = HeadingError;
        HeadingError = (position - TargetSMA);
        Heading = (0.025 * ((Vz.Length(HeadingError) / Vz.Distance(position1, Temp3)) * Vz.Length((velocity2 - velocity1))));
        Heading = Clamp(Heading, (0.00001 * Vz.Length(Temp6)), (0.025 * Vz.Length(Temp6)));
        Count = (Count + (Different_Sign(Vz.Dot(HeadingError, TargetSMA), Vz.Dot(Previous, TargetSMA)) ? 1 : 0));
        Heading = (Heading / Count);
        Input += ((Vz.Dot((TargetSMA - position), Vz.Norm(TargetSMA)) > 0) ? Heading : (-1 * Heading));
        Vz.Display(Vz.Format("<b>Calculating</b> | DeltaV {0:n3} m/s | Periapsis {1:n2} km | Target {2:n0} km", Vz.Length(Output), ((Vz.Length(position) - Vz.Planet(target).Radius()) / 1000), (periapsis / 1000), ""), 7);
        Output = (Vz.Norm(Vz.Cross(Vz.Vec(0, 1, 0), Temp6)) * Input);
    }
});

var Target_Name = Vz.DeclareCustomInstruction("Target Name", "name").SetInstructions((name) =>
{
    Vz.TargetNode(name);
    Vz.WaitSeconds(0);
    using (new WaitUntil((Vz.LengthOf(Vz.Craft.Target.Name()) > 0))) { }
    Vz.ListClear(Target_Name);
    using (new If(String_Input(Target_Status(name), "Moon")))
    {
        Vz.ListAdd(Target_Name, Vz.Planet(name).Parent());
        Vz.ListAdd(Target_Name, Vz.Craft.Target.Name());
        Vz.ListAdd(Target_Name, 0);
    }
    using (new ElseIf(String_Input(Target_Status(name), "Craft")))
    {
        using (new If(String_Input(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Craft.Planet"" style=""craft""><CraftProperty property=""Craft.NameToID"" style=""craft-id""><Variable list=""false"" local=""true"" variableName=""name"" /></CraftProperty></CraftProperty>"), "Moon")))
        {
            Vz.ListAdd(Target_Name, Vz.Planet(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Craft.Planet"" style=""craft""><CraftProperty property=""Craft.NameToID"" style=""craft-id""><Variable list=""false"" local=""true"" variableName=""name"" /></CraftProperty></CraftProperty>")).Parent());
            Vz.ListAdd(Target_Name, Vz.RawXmlCraftProperty(@"<CraftProperty property=""Craft.Planet"" style=""craft""><CraftProperty property=""Craft.NameToID"" style=""craft-id""><Variable list=""false"" local=""true"" variableName=""name"" /></CraftProperty></CraftProperty>"));
        }
        using (new Else())
        {
            Vz.ListAdd(Target_Name, Vz.RawXmlCraftProperty(@"<CraftProperty property=""Craft.Planet"" style=""craft""><CraftProperty property=""Craft.NameToID"" style=""craft-id""><Variable list=""false"" local=""true"" variableName=""name"" /></CraftProperty></CraftProperty>"));
            Vz.ListAdd(Target_Name, 0);
        }
        Vz.ListAdd(Target_Name, Vz.Craft.Target.Name());
    }
    using (new Else())
    {
        Vz.ListAdd(Target_Name, Vz.Craft.Target.Name());
        Vz.ListAdd(Target_Name, 0);
        Vz.ListAdd(Target_Name, 0);
    }
    Destination = (String_Input(Vz.Craft.Orbit.Planet(), Vz.ListGet(Target_Name, 1)) ? Vz.ListGet(Target_Name, 2) : Vz.ListGet(Target_Name, 1));
    Vz.TargetNode(Destination);
    Vz.WaitSeconds(0);
});

// Lagrange coefficient & its derivat
using (new OnReceiveMessage("Auto_Staging"))
{
    using (new While(((OrbitDeltaV > 2) && (Vz.CraftInput(CraftInput.TranslationMode) == 0))))
    {
        Vz.WaitSeconds((5 * Vz.Time.DeltaTime()));
        using (new If(((Vz.CraftInput(CraftInput.Throttle) > 0) && (Vz.Craft.Performance.CurrentThrust() == 0))))
        {
            Vz.WaitSeconds(0.1);
            Vz.ActivateStage();
        }
    }
}

using (new OnReceiveMessage("Vizzy"))
{
    Vz.Broadcast(BroadCastType.Local, "Planet_List", 0);
    using (new WaitUntil((Count == 1))) { }
    Temp1 = "<br>1. Default Orbit<br>2. Custom Orbit";
    Input = Vz.UserInput(Vz.Format("<br><b><color=#ffdd11>{0}</color><br><br><color=#ddffdd>{1}:</color></b><br><color=#ddff11>{2}</color>", Vz.Format("{0}'s Orbit", Target, ""), "Please select your parking orbit<br>input by number", Temp1, ""));
    Input = ((Input == 2) ? 2 : 1);
    using (new If((Input == 1)))
    {
        Periapsis = (0.03 * Vz.Planet(Target).SOI());
        Inclination = "random";
        RightAscension = "random";
    }
    using (new Else())
    {
        Periapsis = Vz.UserInput(Vz.Format("<br><b><color=#ffdd11>{0}</color><br><br><color=#ddffdd>{1}:</color></b><br><color=#ddff11>{2}</color>", Vz.Format("{0}'s Periapsis Altitude", Target, ""), "Please type your desire orbit altitude", "(input in meters)", ""));
        Periapsis = Clamp(Periapsis, (50000 + (0.15 * Vz.Planet(Target).AtmosphereDepth())), (0.8 * Vz.Planet(Target).SOI()));
        Inclination = Vz.UserInput(Vz.Format("<br><b><color=#ffdd11>{0}</color><br><br><color=#ddffdd>{1}:</color></b><br><color=#ddff11>{2}</color>", Vz.Format("{0}'s Orbit Inclination", Target, ""), "Please type your desire orbit inclination", "(input in degrees)", ""));
        Inclination = Clamp(Inclination, 0, 80);
        RightAscension = Vz.UserInput(Vz.Format("<br><b><color=#ffdd11>{0}</color><br><br><color=#ddffdd>{1}:</color></b><br><color=#ddff11>{2}</color>", Vz.Format("{0}'s Orbit RA", Target, ""), "Please type your desire orbit Right Ascension", "(input in degrees)", ""));
        RightAscension = Clamp(RightAscension, 0, 360);
    }
    Output = Vz.UserInput(Vz.Format("<br><b><color=#ffdd11>{0}</color></b><br><br><color=#ddffdd><align=\"left\"><margin=1em>{1}</margin></align></color><br><br><b><color=#ddff11>{2}</color>", "Vizzy Mission Plan", Vz.Format("Target: {0}'s Orbit<br>Altitude: {1:n0} km<br>Inclination: {2:n1}°<br>Right Ascension: {3:n1}°", Target, (Periapsis / 1000), Inclination, RightAscension, ""), "Select action:<br>1. Proceed", ""));
    using (new If((Output == 1)))
    {
        Vz.ListClear(Planet);
        Vz.ListAdd(Planet, Target);
        Vz.ListAdd(Planet, Periapsis);
        Vz.ListAdd(Planet, RightAscension);
        Vz.ListAdd(Planet, Inclination);
        Target_Name(Target);
        using (new If(String_Input(Vz.Craft.Orbit.Planet(), Vz.ListGet(Target_Name, 1))))
        {
            Vz.ListAdd(Planet, "interorbit");
        }
        using (new Else())
        {
            Vz.ListAdd(Planet, "interplanet");
        }
        Vz.Broadcast(BroadCastType.Craft, "Vizzy_Run", 0);
    }
    using (new Else())
    {
        Vz.ListClear(Planet);
        Vz.Broadcast(BroadCastType.Craft, "Vizzy_Fin", 0);
    }
}

var Transfer_to = Vz.DeclareCustomInstruction("Transfer to", "target", "periapsis", "value").SetInstructions((target, periapsis, value) =>
{
    // Comment Text
    Target_Name(target);
    Target = target;
    using (new If((!String_Input(Vz.Craft.Orbit.Planet(), target))))
    {
        using (new If(String_Input(Vz.Craft.Orbit.Planet(), Vz.ListGet(Target_Name, 1))))
        {
            Transfer_to_Moon(Destination, Vz.ListGet(Planet, 2));
        }
        using (new Else())
        {
            Transfer_to_Planet(Destination, Vz.ListGet(Planet, 2), value);
        }
    }
    using (new If((Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) > 0.05)))
    {
        Circularize_at(Vz.Craft.Orbit.Periapsis(), 0.01);
    }
    using (new If((!String_Input(Vz.ListGet(Planet, 3), "random"))))
    {
        Matching_Orbit_Inclination(Vz.ListGet(Planet, 4), Vz.ListGet(Planet, 3), 0.1);
        Orbit_Transfer_to_Altitude(Vz.ListGet(Planet, 2), 0);
    }
    Vz.TargetNode("");
    Vz.Display("Done !", 7);
    Vz.Broadcast(BroadCastType.Craft, "Detach", 0);
    Vz.SetActivationGroup(6, true);
    Vz.SetActivationGroup(9, true);
    Vz.WaitSeconds(1);
    Vz.SetActivationGroup(3, true);
});

using (new OnStart())
{
    // Orbital Mechanic Vizzy 2.0 @2022 by Rizkman
    // Kepler's Equation, Newton's Method, Lambert's Problem Solver, Runge-Kutta 4th ODE Solver, Gibbs Orbit Determination.
    // Keep reminder the game engine using left handed 3D axis, vector cross product using left handed rule too.
    // Don't complaint about my vizzy, i'm not an engineer or programmer
    Initialize();
    Rocket_Engine_Data();
    Atmospheric_Data();
}

var Transfer_to_Moon = Vz.DeclareCustomInstruction("Transfer to Moon", "target", "periapsis").SetInstructions((target, periapsis) =>
{
    // Comment Text
    Target_Node(target);
    using (new If((Vz.Craft.Orbit.Eccentricity() >= 0.7)))
    {
        Circular_Orbit("Orbit Circularization", Vz.Craft.Orbit.Periapsis());
    }
    using (new If((Vz.Angle(Angular_Momentum(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()), Angular_Momentum(Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity())) > 5)))
    {
        Input = (Different_Sign(Angular_Momentum(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()).y, Angular_Momentum(Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity()).y) ? -1 : 1);
        Matching_Orbit_Inclination(Inclination(Vz.Craft.Target.Position(), (Input * Vz.Craft.Target.Velocity())), Right_Ascension(Vz.Craft.Target.Position(), (Input * Vz.Craft.Target.Velocity())), 1);
    }
    Target_Node(target);
    Transfer_Window_to(target);
    Rendezvous(Vz.Format("Transfer to {0}", target, ""), Vz.ListGet(State_Vector, 1), Vz.ListGet(State_Vector, 2), Vz.ListGet(State_Vector, 3), Vz.ListGet(State_Vector, 4), TransferTime, CountDown, 0, Mu());
    Target_Node(target);
    Rendezvous("Correction Burn", Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity(), (0.35 * TransferTime), (0.65 * TransferTime), 0, Mu());
    Target_Node(target);
    Encounter(target, Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity(), periapsis, (0.35 * TransferTime));
    Orbit_Prediction_from(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), 30, Mu());
    Eccentricity_Vector___Angular_Momentum(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    Perifocal_Frame_True_Anomaly(Angle(vectorR2, E, Vz.Cross(E, h)), Vz.Length(E), h);
    Impulsive_Burn("Raising Periapsis", True_Anomaly(position, velocity), 0, Output);
    Target_Node(target);
    Warp_from(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity(), Vz.Planet(target).SOI(), (TransferTime - 30));
    Warp_Time("Encounter", TransferTime);
    using (new WaitUntil(String_Input(target, Vz.Craft.Orbit.Planet()))) { }
    Circular_Orbit("Orbit Insertion", periapsis);
    Vz.Display("Done !", 7);
    Vz.TargetNode("");
});

var Transfer_to_GEO_with_Longitude = Vz.DeclareCustomInstruction("Transfer to GEO with Longitude", "longitude").SetInstructions((longitude) =>
{
    // Comment Text
    Period = Vz.Abs(Vz.Planet(Vz.Craft.Orbit.Planet()).DayLength());
    PlanetRotationSpeed = ((2 * Vz.ExactEval("pi")) / Period);
    R = (((Mu() * (Period ^ 2)) / (4 * (pi() ^ 2))) ^ (1 / 3));
    OrbitVelocity = Orbital_Speed(Vz.Length(Vz.Craft.Nav.Position()), ((Vz.Length(Vz.Craft.Nav.Position()) + R) / 2));
    Time_from_True_Anomaly(0, 180, Mu(), Vz.Craft.Nav.Position(), (OrbitVelocity * Vz.Norm(Vz.Craft.Velocity.Orbital())));
    time = (((TotalTime % Period) / Period) * 360);
    position = (-1 * Vz.ToPosition(Vz.Vec(0, longitude, 0)));
    Eccentricity_Vector___Angular_Momentum(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    TrueAnomaly = Angle(position, E, Vz.Cross(E, h));
    TrueAnomaly = ((TrueAnomaly + time) % 360);
    gamma = Angle(Vz.Craft.Nav.Position(), E, Vz.Cross(E, h));
    Delta = (((TrueAnomaly - gamma) + 360) % 360);
    BurnTime = Burn_Time(((OrbitVelocity - Vz.Length(Vz.Craft.Velocity.Orbital())) / 2));
    Delta = (((TrueAnomaly - gamma) + 360) % 360);
    DeltaTime = ((Delta / Vz.Rad2Deg((((2 * Vz.ExactEval("pi")) / Vz.Craft.Orbit.Period()) - PlanetRotationSpeed))) - BurnTime);
    TrueAnomaly = ((gamma + Vz.Rad2Deg((((2 * Vz.ExactEval("pi")) / Vz.Craft.Orbit.Period()) * DeltaTime))) % 360);
    Perifocal_Frame_True_Anomaly(TrueAnomaly, Vz.Length(E), h);
    Impulsive_Burn("GEO Transfer", TrueAnomaly, 0, ((OrbitVelocity * Vz.Norm(velocity)) - velocity));
    Circularize_at((R - Vz.Planet(Vz.Craft.Orbit.Planet()).Radius()), 0);
});

var Transfer_to_Planet = Vz.DeclareCustomInstruction("Transfer to Planet", "target", "periapsis", "value").SetInstructions((target, periapsis, value) =>
{
    // Comment Text
    Target_Node(target);
    using (new If((!((value == 1) ? true : false))))
    {
        Transfer_Window_to(Destination);
        Warp_Time("Launch Window", CountDown);
    }
    TransferTime = (TransferTime + 1800);
    Orbit_Prediction_from(Target_Solar_Position(), Target_Solar_Velocity(), TransferTime, (G() * Vz.Planet(Parent_Star()).Mass()));
    Lambert_s_Problem_Solver(Parent_Solar_Position(), vectorR2, TransferTime, (G() * Vz.Planet(Parent_Star()).Mass()));
    TransferDeltaV = (VectorV1 - Parent_Solar_Velocity());
    h = Vz.Cross(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    OrbitalPlane = Vz.Cross(TransferDeltaV, Rejection_of(Parent_Solar_Position(), Vz.Vec(0, 1, 0)));
    OrbitalPlane = ((OrbitalPlane.y < 0) ? (-1 * OrbitalPlane) : OrbitalPlane);
    using (new If((Vz.Angle(OrbitalPlane, (-1 * h)) > 0.5)))
    {
        Orbital_Plane(OrbitalPlane, "Aligning Escape Vector");
        Vz.SetTimeModeAttr("Normal");
        Vz.WaitSeconds(0.5);
    }
    using (new If((Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) > 0.0005)))
    {
        Circular_Orbit("Orbit Correction", Vz.Craft.Orbit.Apoapsis());
    }
    EscapeEnergy = (Mu() / (-2 * (1 / ((2 / Vz.Planet(Vz.Craft.Orbit.Planet()).SOI()) - ((Vz.Length(TransferDeltaV) ^ 2) / Mu())))));
    CurrentPotentialEnergy = (-1 * (Mu() / Vz.Length(Vz.Craft.Nav.Position())));
    EscapeDeltaV = (Vz.Sqrt((2 * (EscapeEnergy - CurrentPotentialEnergy))) - Vz.Sqrt((2 * (Specific_Orbital_Energy(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) - CurrentPotentialEnergy))));
    EscapeEccentricity = Vz.Max((1 + Vz.Abs(((Vz.Length(Vz.Craft.Nav.Position()) * (2 * EscapeEnergy)) / Mu()))), 1.1);
    TransferAngle = (Vz.Rad2Deg(Vz.Acos((1 / EscapeEccentricity))) - 0.01);
    h = Vz.Cross(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    Node = Vz.Norm(Vz.Cross((-1 * h), Parent_Solar_Position()));
    Node = Rotate(Node, (-1 * h), (-1 * (TransferAngle + Angle(TransferDeltaV, (-1 * Node), (-1 * Parent_Solar_Position())))));
    Perifocal_Frame_of_Vector(Node, Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), Mu());
    OrbitVelocity = (EscapeDeltaV * Vz.Norm(Vz.Cross(Vz.Cross(position, velocity), position)));
    Impulsive_Burn(Vz.Format("{0} Transfer Burn", Destination, ""), TrueAnomaly, 0, OrbitVelocity);
    TotalTime = Vz.Time.TotalTime();
    Vz.LockNavSphere(LockNavSphereIndicatorType.Prograde);
    Warp_Time("Waiting Mid Course Corection", (0.8 * TransferTime));
    Target_Node(Destination);
    Rendezvous("Trajectory Correction Burn", Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity(), (0.2 * TransferTime), (30 + (30 * Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()))), 0, Mu());
    TransferTime = (0.2 * TransferTime);
    using (new If((!String_Input(Target_Status(Target), "Moon"))))
    {
        Clear_Temp();
        Target_Node(Destination);
        Encounter(Destination, Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity(), periapsis, TransferTime);
        Orbit_Prediction_from(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), (30 + (10 * Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()))), Mu());
        Eccentricity_Vector___Angular_Momentum(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
        Impulsive_Burn("Raising Periapsis", Angle(vectorR2, E, Vz.Cross(E, h)), 0, Output);
    }
    Clear_Temp();
    Target_Node(Destination);
    Warp_from(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity(), Vz.Planet(Destination).SOI(), (TransferTime - 30));
    Warp_Time("Encounter", TransferTime);
    using (new WaitUntil(String_Input(Destination, Vz.Craft.Orbit.Planet()))) { }
    Vz.SetTimeModeAttr("Normal");
    using (new If(String_Input(Target_Status(Target), "Moon")))
    {
        Target = Vz.ListGet(Planet, 1);
        Time_from_True_Anomaly(True_Anomaly(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()), 0, Mu(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
        Target_Node(Target);
        Node = Vz.Cross(Vz.Craft.Nav.Position(), (-1 * Vz.Cross(Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity())));
        Hyperbolic_Transfer_Time_to(Target, Node, TotalTime);
        Rendezvous("Trajectory Correction", Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity(), TransferTime, (30 + (2 * Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()))), 0, Mu());
        Vz.LockNavSphere(LockNavSphereIndicatorType.Prograde);
        Warp_Time("Waiting Corection Burn", (0.8 * TransferTime));
        Target_Node(Target);
        Rendezvous("Trajectory Correction Burn", Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity(), (0.2 * TransferTime), (30 + (2 * Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()))), 0, Mu());
        periapsis = Vz.Max((0.25 * Vz.Planet(Target).Radius()), (0.2 * Vz.Planet(Target).SOI()));
        Clear_Temp();
        Target_Node(Target);
        Encounter(Target, Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity(), periapsis, ((0.2 * TransferTime) - (30 + (2 * Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital())))));
        Orbit_Prediction_from(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), (30 + (5 * Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()))), Mu());
        Eccentricity_Vector___Angular_Momentum(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
        Impulsive_Burn("Raising Periapsis", Angle(vectorR2, E, Vz.Cross(E, h)), 0, Output);
        Target_Node(Target);
        Warp_from(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity(), Vz.Planet(Target).SOI(), (TransferTime - 30));
        Warp_Time("Encounter", TransferTime);
        using (new WaitUntil(String_Input(Target, Vz.Craft.Orbit.Planet()))) { }
    }
});

var Clear_Temp = Vz.DeclareCustomInstruction("Clear Temp").SetInstructions(() =>
{
    Vz.ListClear(Gamma);
    Vz.ListClear(Height);
    Vz.ListClear(Craft_List);
    Vz.ListClear(Craft_Name);
    Vz.ListClear(Velocity);
    Vz.ListClear(Temp);
    Temp1 = 0;
    Temp2 = 0;
    Temp3 = 0;
    Temp4 = 0;
    Temp5 = 0;
    Temp6 = 0;
    A = 0;
    alpha = 0;
    AscendingNode = 0;
    Azimuth = 0;
    BurnOut = 0;
    BurnTime = 0;
    C = 0;
    CD = 0;
    Chord = 0;
    Count = 0;
    CurrentPotentialEnergy = 0;
    D = 0;
    Days = 0;
    Delta = 0;
    DeltaTime = 0;
    Derivat = 0;
    DescendingNode = 0;
    dFdx = 0;
    dhdt = 0;
    Diameter = 0;
    Display = 0;
    Distance = 0;
    dvdt = 0;
    dxdt = 0;
    dydt = 0;
    E = 0;
    Eccentricity = 0;
    EngineNumber = 0;
    Error = 0;
    EscapeDeltaV = 0;
    EscapeEccentricity = 0;
    EscapeEnergy = 0;
    ExhaustVelocity = 0;
    ExitArea = 0;
    ExitSOI = 0;
    F = 0;
    f = 0;
    fdot = 0;
    g = 0;
    g0 = 0;
    Gain = 0;
    gamma = 0;
    gdot = 0;
    h = 0;
    Heading = 0;
    HeadingError = 0;
    height = 0;
    Hmax = 0;
    h_Scale = 0;
    h_Turn = 0;
    Inclination = 0;
    Input = 0;
    Isp = 0;
    Location = 0;
    m = 0;
    m0 = 0;
    mdot = 0;
    MeanAnomaly = 0;
    mfinal = 0;
    Minutes = 0;
    mprop = 0;
    Mu = 0;
    n = 0;
    N = 0;
    Nmax = 0;
    Node = 0;
    OrbitalPlane = 0;
    OrbitBurnTime = 0;
    OrbitDeltaV = 0;
    OrbitLongitude = 0;
    OrbitVelocity = 0;
    Output = 0;
    P = 0;
    P0 = 0;
    ParentEnergy = 0;
    ParentSMA = 0;
    Periapsis = 0;
    Period = 0;
    PlanetRotationSpeed = 0;
    position = 0;
    PotentialEnergy = 0;
    Previous = 0;
    Proportional = 0;
    Q = 0;
    r = 0;
    R = 0;
    ratio = 0;
    rho = 0;
    rho0 = 0;
    RightAscension = 0;
    ro = 0;
    S = 0;
    SaveData = 0;
    Seconds = 0;
    SMA = 0;
    T = 0;
    TargetDelta = 0;
    TargetHeading = 0;
    TargetOrbitEnergy = 0;
    TargetSMA = 0;
    theta = 0;
    Throttle = 0;
    Thrust = 0;
    ThrustVacuum = 0;
    time = 0;
    TimeSincePerigee = 0;
    TimeUntilSurface = 0;
    TimeWrap = 0;
    TotalTime = 0;
    TransferAngle = 0;
    TransferDeltaV = 0;
    TransferEnergy = 0;
    TransferPotentialPeriapsis = 0;
    TransferSMA = 0;
    TransferVelocity = 0;
    TrueAnomaly = 0;
    TWR = 0;
    Universal_Anomaly_X = 0;
    v = 0;
    vdot = 0;
    Vector = 0;
    VectorR1 = 0;
    vectorR2 = 0;
    vectorV = 0;
    VectorV1 = 0;
    vectorV2 = 0;
    velocity = 0;
    vg = 0;
    vo = 0;
    vro = 0;
    x = 0;
    y = 0;
    z = 0;
});

TransferTime = 0;
Target = 0;
Destination = 0;
var Planet_List = Vz.DeclareCustomInstruction("Planet List", "name").SetInstructions((name) =>
{
    using (new If(Vz.Craft.Grounded()))
    {
        Temp1 = "<br>";
        using (new For("i").From(0).To(Vz.ListLength(Vz.Planet(name).ChildPlanets())).By(1))
        {
            Temp1 = ((i == 0) ? (String_Input(Parent_Star(), name) ? "<br>0. Exit" : "<br>0. Back") : ((i == 1) ? Vz.Join(Temp1, "<br>", Vz.Format("{0}. {1}", i, Vz.ListGet(Vz.Planet(name).ChildPlanets(), i), ""), "") : Vz.Join(Temp1, "<br>", Vz.Format("{0}. {1}", i, Vz.ListGet(Vz.Planet(name).ChildPlanets(), i), ""), "")));
        }
        Temp2 = (String_Input(Parent_Star(), name) ? "<br>" : Vz.Format("{0}. {1}'s orbit", (Vz.ListLength(Vz.Planet(name).ChildPlanets()) + 1), name, ""));
        Temp1 = Vz.Join(Temp1, "<br>", Temp2, "");
        Input = Vz.UserInput(Vz.Format("<br><b><color=#ffdd11>{0}</color><br><br><color=#ddffdd>{1}:</color></b><br><align=\"left\"><margin=6em><color=#ddff11>{2}</color>", "Vizzy Auto Mission", "Please select your destination<br>input by number", Temp1, ""));
        using (new If((Input == 0)))
        {
            Vz.ListRemove(Planet, Vz.ListLength(Planet));
            using (new If((Vz.ListLength(Planet) > 0)))
            {
                Planet_List(Vz.ListGet(Planet, Vz.ListLength(Planet)));
            }
            using (new Else())
            {
                Vz.ListClear(Planet);
                Vz.Broadcast(BroadCastType.Craft, "Vizzy_Fin", 0);
                Vz.Break();
            }
        }
        using (new ElseIf(((Input > 0) && (Input <= Vz.ListLength(Vz.Planet(name).ChildPlanets())))))
        {
            Target = Vz.ListGet(Vz.Planet(name).ChildPlanets(), Input);
            using (new If(String_Input(Parent_Star(), name)))
            {
                Vz.ListAdd(Planet, Parent_Star());
            }
            Vz.ListAdd(Planet, Target);
            Planet_List(Target);
        }
    }
    using (new Else())
    {
        Target = Vz.Craft.Orbit.Planet();
    }
});

using (new OnReceiveMessage("Planet_List"))
{
    Planet_List(Parent_Star());
    Count = 1;
}

var Initialize = Vz.DeclareCustomInstruction("Initialize").SetInstructions(() =>
{
    Vz.WaitSeconds(1);
    g0 = g0();
    SaveData = true;
    TotalTime = 0;
    Current_Planet_Rotation();
    TimeWarp = Vz.CreateListRaw("36,80,400,2000,10000,50000,250000,1000000,5000000");
});

var Rocket_Engine_Data = Vz.DeclareCustomInstruction("Rocket Engine Data").SetInstructions(() =>
{
    using (new While((Vz.PartNameToID("MainEngine") < 0)))
    {
        Vz.Display("Please Change your first stage engine name into \"MainEngine\" !", 7);
    }
    // When engine is not active, it will return engine data on vacuum condition
    ThrustVacuum = (Vz.ExactEval("MainEngine.RocketEngine.MaximumThrust") * 100);
    using (new If((Vz.Craft.Performance.MaxThrust() < 1)))
    {
        Vz.ActivateStage();
    }
    using (new WaitUntil((Vz.Craft.Performance.MaxThrust() > 1))) { }
    Thrust = Vz.Craft.Performance.MaxThrust();
    EngineNumber = Vz.Round((Vz.Craft.Performance.MaxThrust() / (Vz.ExactEval("MainEngine.RocketEngine.MaximumThrust") * 100)));
    ExitArea = Vz.ExactEval("MainEngine.RocketEngine.Data.NozzleAreaExit");
    m0 = Vz.Craft.Performance.Mass();
    mdot = Vz.ExactEval("MainEngine.RocketEngine.MaximumMassFlowRate");
    mdot = (mdot * EngineNumber);
    using (new WaitUntil((Vz.Craft.Performance.BurnTime() > 1))) { }
    BurnOut = Vz.Craft.Performance.BurnTime();
    mprop = (mdot * BurnOut);
    mfinal = (m0 - mprop);
    TWR = (Vz.Craft.Performance.MaxThrust() / (m0 * g0));
});

var Lambert_s_Problem_Solver = Vz.DeclareCustomInstruction("Lambert's Problem Solver", "position1", "position2", "time", "Mu").SetInstructions((position1, position2, time, Mu) =>
{
    // Solution for Lambert's Problem, input: vectorR1, vectorR2, & elapsed time
    ro = Vz.Length(position1);
    r = Vz.Length(position2);
    DeltaTime = time;
    Chord = (position2 - position1);
    theta = Vz.Acos((Vz.Dot(position1, position2) / (ro * r)));
    Temp5 = ((Vz.Cross(position1, position2).y > 0) ? ((2 * pi()) - theta) : theta);
    Temp6 = ((Vz.Cross(position1, position2).y <= 0) ? ((2 * pi()) - theta) : theta);
    theta = ((Vz.Craft.Orbit.Eccentricity() > 1) ? theta : ((Vz.Cross(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()).y > 0) ? Temp6 : Temp5));
    A = (Vz.Sin(theta) * Vz.Sqrt(((ro * r) / (1 - Vz.Cos(theta)))));
    Mu = Mu;
    z = 0;
    using (new While((F(z, DeltaTime) < -0)))
    {
        z = (z + 0.1);
    }
    Error = 0.00000001;
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

var Clear_Saved_List = Vz.DeclareCustomInstruction("Clear Saved List").SetInstructions(() =>
{
    Vz.ListClear(Gamma);
    Vz.ListClear(Height);
    Vz.ListClear(Velocity);
    Vz.ListClear(DownRange);
});

using (new OnReceiveMessage("Vizzy_Run"))
{
    using (new If(Vz.Craft.Grounded()))
    {
        using (new If(String_Input(Vz.ListGet(Planet, 5), "interplanet")))
        {
            Transfer_Window_to(Destination);
            Warp_Time("Launch Window", CountDown);
            TransferTime = (TransferTime + 1800);
            Target_Node(Destination);
            Orbit_Prediction_from(Target_Solar_Position(), Target_Solar_Velocity(), TransferTime, (G() * Vz.Planet(Parent_Star()).Mass()));
            Lambert_s_Problem_Solver(Parent_Solar_Position(), vectorR2, TransferTime, (G() * Vz.Planet(Parent_Star()).Mass()));
            TransferDeltaV = (VectorV1 - Parent_Solar_Velocity());
            OrbitalPlane = Vz.Cross(TransferDeltaV, Rejection_of(Parent_Solar_Position(), Vz.Vec(0, 1, 0)));
            OrbitalPlane = ((OrbitalPlane.y < 0) ? (-1 * OrbitalPlane) : OrbitalPlane);
            Node = Vz.Cross(OrbitalPlane, Vz.Vec(0, 1, 0));
            RightAscension = Angle(Node, Vz.Vec(1, 0, 0), Vz.Vec(0, 0, 1));
            Inclination = Vz.Angle(OrbitalPlane, Vz.Vec(0, 1, 0));
        }
        using (new ElseIf(String_Input(Vz.Craft.Orbit.Planet(), Vz.ListGet(Planet, 1))))
        {
            RightAscension = Vz.ListGet(Planet, 3);
            Inclination = Vz.ListGet(Planet, 4);
        }
        using (new Else())
        {
            Target_Node(Vz.ListGet(Planet, 1));
            RightAscension = Right_Ascension(Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity());
            Inclination = Inclination(Vz.Craft.Target.Position(), Vz.Craft.Target.Velocity());
        }
        Calculating_Launch_Trajectory__Craft_s_Diameter(6);
        Launch__Apoapsis((String_Input(Vz.Craft.Orbit.Planet(), Vz.ListGet(Planet, 1)) ? Vz.ListGet(Planet, 2) : 160000), RightAscension, Inclination);
        Circularize_at((String_Input(Vz.Craft.Orbit.Planet(), Vz.ListGet(Planet, 1)) ? Vz.ListGet(Planet, 2) : Vz.Craft.Orbit.Apoapsis()), 0.01);
        using (new If(String_Input(Target_Status(Vz.Craft.Target.Name()), "Moon")))
        {
            Clear_Temp();
        }
        Target_Node(Vz.ListGet(Planet, 1));
        Transfer_to(Vz.Craft.Target.Name(), Vz.ListGet(Planet, 2), 1);
        Vz.Broadcast(BroadCastType.Craft, "Vizzy_Fin", 0);
    }
    using (new Else())
    {
        Target_Node(Vz.ListGet(Planet, 1));
        Transfer_to(Vz.Craft.Target.Name(), Vz.ListGet(Planet, 2), 0);
        Vz.Broadcast(BroadCastType.Craft, "Vizzy_Fin", 0);
    }
}

var Hyperbolic_Transfer_Time_to = Vz.DeclareCustomInstruction("Hyperbolic Transfer Time to", "target", "node", "ETA").SetInstructions((target, node, ETA) =>
{
    // Comment Text
    CountDown = 0;
    TransferAngle = 0;
    Target_Node(target);
    Temp3 = Vz.Craft.Target.Position();
    Temp4 = Vz.Craft.Target.Velocity();
    Time_from_True_Anomaly(0, 180, Mu(), Temp3, Temp4);
    Period = (TotalTime / 180);
    h = Vz.Cross(Temp3, Temp4);
    using (new While(true))
    {
        Orbit_Prediction_from(Temp3, Temp4, (ETA + CountDown), Mu());
        Temp5 = vectorR2;
        Previous = TransferAngle;
        TransferAngle = Angle(Rejection_of(Temp5, (-1 * h)), node, Vz.Cross(node, (-1 * h)));
        TransferAngle = ((TransferAngle > 180) ? (360 - TransferAngle) : TransferAngle);
        TimeWrap = Clamp(((0.25 * Period) * TransferAngle), (0.02 * Period), (30 * Period));
        using (new If((Different_Sign(TransferAngle, Previous) && (Vz.Abs(TransferAngle) < 5))))
        {
            Vz.Break();
        }
        CountDown += ((CountDown < 1) ? (0.1 * Period) : TimeWrap);
        Vz.Display(Vz.Format("Calculating | Angle {0:n1}° | Timelapse {1:n0} sec", TransferAngle, CountDown, ""), 7);
    }
    TransferTime = (ETA + CountDown);
});

var Plane = Vz.DeclareCustomInstruction("Plane", "text", "inclination", "RA").SetInstructions((text, inclination, RA) =>
{
    // Comment Text
    OrbitalPlane = Orbit_Plane_Normal(Ascending_Node(RA), inclination);
    Orbital_Plane(OrbitalPlane, text);
});

// ── Custom Expressions ───────────────────────────────
var Sinh = Vz.DeclareCustomExpression("Sinh", "value").SetReturn((value) =>
{
    return (((e() ^ value) - (e() ^ (-1 * value))) / 2);
});

var Cosh = Vz.DeclareCustomExpression("Cosh", "value").SetReturn((value) =>
{
    return (((e() ^ value) + (e() ^ (-1 * value))) / 2);
});

var Position_Z_up = Vz.DeclareCustomExpression("Position Z-up", "position").SetReturn((position) =>
{
    return Vz.Vec(position.x, position.z, position.y);
});

var Velocity_Z_up = Vz.DeclareCustomExpression("Velocity Z-up", "velocity").SetReturn((velocity) =>
{
    return Vz.Vec(velocity.x, velocity.z, velocity.y);
});

var Eccentricity = Vz.DeclareCustomExpression("Eccentricity", "position", "velocity").SetReturn((position, velocity) =>
{
    return Vz.Length(Eccentricity_Vector(position, velocity));
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

var Absolute_True_Anomaly = Vz.DeclareCustomExpression("Absolute True Anomaly", "position", "velocity").SetReturn((position, velocity) =>
{
    return Vz.Rad2Deg(Vz.Acos((Vz.Dot(Eccentricity_Vector(position, velocity), Position_Z_up(position)) / (Eccentricity(position, velocity) * Vz.Length(Position_Z_up(position))))));
});

var Absolute_Argument_of_Periapsis = Vz.DeclareCustomExpression("Absolute Argument of Periapsis", "position", "velocity").SetReturn((position, velocity) =>
{
    return Vz.Rad2Deg(Vz.Acos((Vz.Dot(Node_Vector(position, velocity), Eccentricity_Vector(position, velocity)) / (Vz.Length(Node_Vector(position, velocity)) * Eccentricity(position, velocity)))));
});

var True_Anomaly = Vz.DeclareCustomExpression("True Anomaly", "position", "velocity").SetReturn((position, velocity) =>
{
    return ((Vz.Dot(Position_Z_up(position), Velocity_Z_up(velocity)) < -0) ? (360 - Absolute_True_Anomaly(position, velocity)) : Absolute_True_Anomaly(position, velocity));
});

var Specific_Orbital_Energy = Vz.DeclareCustomExpression("Specific Orbital Energy", "position", "velocity").SetReturn((position, velocity) =>
{
    return (((Vz.Length(velocity) ^ 2) / 2) - (Mu() / Vz.Length(position)));
});

var Eccentric_Anomaly = Vz.DeclareCustomExpression("Eccentric Anomaly", "position", "velocity").SetReturn((position, velocity) =>
{
    return (IsNan((1 / Eccentricity(position, velocity))) ? True_Anomaly(position, velocity) : ((True_Anomaly(position, velocity) <= 180) ? Partial_Eccentric_Anomaly(position, velocity) : (360 - Partial_Eccentric_Anomaly(velocity, position))));
});

var Partial_Eccentric_Anomaly = Vz.DeclareCustomExpression("Partial Eccentric Anomaly", "position", "velocity").SetReturn((position, velocity) =>
{
    return Vz.Rad2Deg(Vz.Acos((1 - ((Vz.Length(position) / Semi_Major_Axis(position, velocity)) / Eccentricity(position, velocity)))));
});

var Rejection_of = Vz.DeclareCustomExpression("Rejection of", "vector1", "vector2").SetReturn((vector1, vector2) =>
{
    return (vector1 - ((Vz.Dot(vector1, vector2) / Vz.Dot(vector2, vector2)) * vector2));
});

var Sign = Vz.DeclareCustomExpression("Sign", "value").SetReturn((value) =>
{
    return ((value < -0) ? -1 : ((value > -0) ? 1 : value));
});

var Target_DeltaV__to = Vz.DeclareCustomExpression("Target DeltaV, to", "TargetEnergy", "PotentialEnergy", "position", "velocity").SetReturn((TargetEnergy, PotentialEnergy, position, velocity) =>
{
    return (Vz.Sqrt((2 * (TargetEnergy - PotentialEnergy))) - Vz.Sqrt((2 * (Specific_Orbital_Energy(position, velocity) - PotentialEnergy))));
});

var Altitude = Vz.DeclareCustomExpression("Altitude", "position").SetReturn((position) =>
{
    return (Vz.Length(position) - Vz.Planet(Vz.Craft.Orbit.Planet()).Radius());
});

var Max_Acceleration = Vz.DeclareCustomExpression("Max Acceleration").SetReturn(() =>
{
    return (Vz.Craft.Performance.MaxThrust() / Vz.Craft.Performance.Mass());
});

var Craft_Status = Vz.DeclareCustomExpression("Craft Status").SetReturn(() =>
{
    return ((Vz.Craft.Grounded() || (Vz.Craft.AltitudeAGL() < 50)) ? "On Ground" : ((Vz.Craft.Orbit.Apoapsis() < Vz.Planet(Vz.Craft.Orbit.Planet()).AtmosphereDepth()) ? "On Atmosphere" : ((Vz.Craft.Orbit.Periapsis() < 500) ? "Suborbital Trajectorh" : ((Vz.Craft.Orbit.Eccentricity() > 1) ? "Hyperbolic Orbit" : ((Vz.Craft.Orbit.Eccentricity() > 0.1) ? "Elliptical Orbit" : "Circular Orbit")))));
});

var Target_Orbit_Status = Vz.DeclareCustomExpression("Target Orbit Status").SetReturn(() =>
{
    return ((Vz.LengthOf(Vz.Craft.Target.Name()) > 0) ? (Vz.Contains(Vz.Craft.Target.Planet(), Parent_Star()) ? "Orbiting Sun" : (Vz.Contains(Vz.Planet(Vz.Craft.Target.Planet()).Parent(), Parent_Star()) ? "Orbiting Planet" : "Orbiting Moon")) : "no target");
});

var Orbit_Status = Vz.DeclareCustomExpression("Orbit Status").SetReturn(() =>
{
    return (((Vz.LengthOf(Grand_Parent()) == -0) && (Vz.LengthOf(Parent()) == -0)) ? "Star" : (((Vz.LengthOf(Parent()) > -0) && (Vz.LengthOf(Grand_Parent()) == -0)) ? "Planet" : "Moon"));
});

var Parent_Star = Vz.DeclareCustomExpression("Parent Star").SetReturn(() =>
{
    return ((Vz.LengthOf(Grand_Parent()) > -0) ? Grand_Parent() : ((Vz.LengthOf(Parent()) > -0) ? Parent() : Vz.Craft.Orbit.Planet()));
});

var Grand_Parent = Vz.DeclareCustomExpression("Grand Parent").SetReturn(() =>
{
    return Vz.Planet(Parent()).Parent();
});

var Parent = Vz.DeclareCustomExpression("Parent").SetReturn(() =>
{
    return Vz.Planet(Vz.Craft.Orbit.Planet()).Parent();
});

var G = Vz.DeclareCustomExpression("G").SetReturn(() =>
{
    return 0.0000000000667384;
});

var e = Vz.DeclareCustomExpression("e").SetReturn(() =>
{
    return Vz.ExactEval("E");
});

var pi = Vz.DeclareCustomExpression("pi").SetReturn(() =>
{
    return Vz.ExactEval("pi");
});

var g0 = Vz.DeclareCustomExpression("g0").SetReturn(() =>
{
    return 9.80622;
});

var Mu = Vz.DeclareCustomExpression("Mu").SetReturn(() =>
{
    return (G() * Vz.Planet(Vz.Craft.Orbit.Planet()).Mass());
});

var dFdz_z_0 = Vz.DeclareCustomExpression("dFdz z=0", "z").SetReturn((z) =>
{
    return (((Vz.Sqrt(2) / 40) * (y(0.00) ^ 1.5)) + ((A / 8) * (Vz.Sqrt(y(0.00)) + (A * Vz.Sqrt((1 / (2 * y(0.00))))))));
});

var dFdz_z_not_0 = Vz.DeclareCustomExpression("dFdz z not 0", "z").SetReturn((z) =>
{
    return ((((y(z) / Stump_C(z)) ^ 1.5) * (((1 / (2 * z)) * (Stump_C(z) - ((3 * Stump_S(z)) / (2 * Stump_C(z))))) + ((3 * (Stump_S(z) ^ 2)) / (4 * Stump_C(z))))) + ((A / 8) * ((((3 * Stump_S(z)) / Stump_C(z)) * Vz.Sqrt(y(z))) + (A * Vz.Sqrt((Stump_C(z) / y(z)))))));
});

var F = Vz.DeclareCustomExpression("F", "z", "t").SetReturn((z, t) =>
{
    return (((((y(z) / Stump_C(z)) ^ 1.5) * Stump_S(z)) + (A * Vz.Sqrt(y(z)))) - (Vz.Sqrt(Mu) * t));
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

var Display = Vz.DeclareCustomExpression("Display", "title", "label1", "value1", "precision1", "unit1", "label2", "value2", "precision2", "unit2").SetReturn((title, label1, value1, precision1, unit1, label2, value2, precision2, unit2) =>
{
    return Vz.Format("<b><color=#ffdd11>{0}</color></b> | {1}: <color=#aaff11><b>{2}</b></color> {3} | {4}: <color=#aaff11><b>{5}</b></color> {6}", title, label1, (Vz.Round((value1 * (10 ^ precision1))) / (10 ^ precision1)), unit1, label2, (Vz.Round((value2 * (10 ^ precision2))) / (10 ^ precision2)), unit2, "");
});

var TWR__ = Vz.DeclareCustomExpression("TWR *", "value").SetReturn((value) =>
{
    return Vz.Max((((Vz.Craft.Performance.Mass() * Vz.Length(Vz.Craft.Velocity.Gravity())) * value) / Vz.Craft.Performance.MaxThrust()), 0.05);
});

var IsNan = Vz.DeclareCustomExpression("IsNan", "Value").SetReturn((Value) =>
{
    return (!((Value < -0) || (Value >= -0)));
});

var Rotate = Vz.DeclareCustomExpression("Rotate", "vector", "axis", "angle").SetReturn((vector, axis, angle) =>
{
    return ((vector * Vz.Cos(Vz.Deg2Rad(angle))) + ((Vz.Cross(Vz.Norm(axis), vector) * Vz.Sin(Vz.Deg2Rad(angle))) + (Vz.Norm(axis) * (Vz.Dot(Vz.Norm(axis), vector) * (1 - Vz.Cos(Vz.Deg2Rad(angle)))))));
});

var Format_Time = Vz.DeclareCustomExpression("Format Time", "time").SetReturn((time) =>
{
    return Vz.Format("<color=#aaff11><b>{0} {1}</b></color> | {2:00}:{3:00}:{4:00}", Vz.Floor((time / 86400)), ((Vz.Floor((time / 86400)) <= 1) ? "day" : "days"), (Vz.Floor((time / 3600)) % 24), (Vz.Floor((time / 60)) % 60), (Vz.Floor(time) % 60), "");
});

var Eccentric_Anomaly_with = Vz.DeclareCustomExpression("Eccentric Anomaly with", "TrueAnomaly").SetReturn((TrueAnomaly) =>
{
    return ((360 + Vz.Rad2Deg((2 * Vz.Atan((Vz.Sqrt(((1 - Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital())) / (1 + Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital())))) * Vz.Tan((Vz.Deg2Rad(TrueAnomaly) / 2))))))) % 360);
});

var String_Input = Vz.DeclareCustomExpression("String Input", "string1", "string2").SetReturn((string1, string2) =>
{
    return (Vz.Contains(string1, string2) && Vz.Contains(string2, string1));
});

var Orbit_Plane_Normal = Vz.DeclareCustomExpression("Orbit Plane Normal", "AscendingNode", "inclination").SetReturn((AscendingNode, inclination) =>
{
    return Rotate(Vz.Vec(-0, 1, -0), AscendingNode, (-1 * inclination));
});

var Ascending_Node = Vz.DeclareCustomExpression("Ascending Node", "RightAscention").SetReturn((RightAscention) =>
{
    return Rotate(Vz.Vec(1, -0, -0), Vz.Vec(-0, 1, -0), (-1 * RightAscention));
});

var Argument_of_Periapsis = Vz.DeclareCustomExpression("Argument of Periapsis", "position", "velocity").SetReturn((position, velocity) =>
{
    return ((Eccentricity_Vector(position, velocity).z < -0) ? (360 - Absolute_Argument_of_Periapsis(position, velocity)) : Absolute_Argument_of_Periapsis(position, velocity));
});

var Angle = Vz.DeclareCustomExpression("Angle", "vector", "x_axis", "y_axis").SetReturn((vector, x_axis, y_axis) =>
{
    return ((360 + Vz.Rad2Deg(Vz.Atan2(Vz.Dot(vector, Vz.Norm(y_axis)), Vz.Dot(vector, Vz.Norm(x_axis))))) % 360);
});

var Burn_Vector = Vz.DeclareCustomExpression("Burn Vector", "VelocityVector").SetReturn((VelocityVector) =>
{
    return ((Vz.Norm((VelocityVector + Vz.Craft.Velocity.Orbital())) * Vz.Length(VelocityVector)) - (Vz.Norm((VelocityVector + Vz.Craft.Velocity.Orbital())) * Vz.Length(Vz.Craft.Velocity.Orbital())));
});

var Eccentricity_Vector = Vz.DeclareCustomExpression("Eccentricity Vector", "position", "velocity").SetReturn((position, velocity) =>
{
    return ((Vz.Cross(Velocity_Z_up(velocity), Angular_Momentum(position, velocity)) / Mu()) - Vz.Norm(Position_Z_up(position)));
});

var Angular_Momentum = Vz.DeclareCustomExpression("Angular Momentum", "position", "velocity").SetReturn((position, velocity) =>
{
    return Vz.Cross(Position_Z_up(position), Velocity_Z_up(velocity));
});

var Current_Throttle = Vz.DeclareCustomExpression("Current Throttle").SetReturn(() =>
{
    return (Vz.Craft.Performance.CurrentThrust() / Vz.Craft.Performance.MaxThrust());
});

var Current_Acceleration = Vz.DeclareCustomExpression("Current Acceleration").SetReturn(() =>
{
    return (Vz.Craft.Performance.CurrentThrust() / Vz.Craft.Performance.Mass());
});

var Burn_Time = Vz.DeclareCustomExpression("Burn Time", "deltaV").SetReturn((deltaV) =>
{
    return (((Vz.Craft.Performance.Mass() * (1 - (1 / (e() ^ (deltaV / (g0() * Vz.Craft.Performance.ISP())))))) * (g0() * Vz.Craft.Performance.ISP())) / Vz.Craft.Performance.MaxThrust());
});

var True_Anomaly_at_Radius = Vz.DeclareCustomExpression("True Anomaly at Radius", "radius").SetReturn((radius) =>
{
    return Vz.Rad2Deg(Vz.Acos((((Semi_Major_Axis(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) * (1 - (Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()) ^ 2))) / (radius * Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital()))) - (1 / Eccentricity(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital())))));
});

var Target_Solar_Velocity = Vz.DeclareCustomExpression("Target Solar Velocity").SetReturn(() =>
{
    return Vz.Planet(Vz.Craft.Target.Name()).Velocity();
});

var Target_Solar_Position = Vz.DeclareCustomExpression("Target Solar Position").SetReturn(() =>
{
    return Vz.Planet(Vz.Craft.Target.Name()).SolarPosition();
});

var Parent_Solar_Velocity = Vz.DeclareCustomExpression("Parent Solar Velocity").SetReturn(() =>
{
    return Vz.Planet(Vz.Craft.Orbit.Planet()).Velocity();
});

var Parent_Solar_Position = Vz.DeclareCustomExpression("Parent Solar Position").SetReturn(() =>
{
    return Vz.Planet(Vz.Craft.Orbit.Planet()).SolarPosition();
});

var Target_Status = Vz.DeclareCustomExpression("Target Status", "name").SetReturn((name) =>
{
    return ((Vz.LengthOf(Vz.Craft.Target.Name()) > 0) ? ((Vz.Length(Vz.Planet(name).SolarPosition()) == -0) ? "Craft" : ((Vz.Craft.Target.Position() == Vz.Planet(name).SolarPosition()) ? "Planet" : "Moon")) : "no target");
});

var Orbital_Speed = Vz.DeclareCustomExpression("Orbital Speed", "radius", "SMA").SetReturn((radius, SMA) =>
{
    return Vz.Sqrt((Mu() * ((2 / radius) - (1 / SMA))));
});

var atanh = Vz.DeclareCustomExpression("atanh", "value").SetReturn((value) =>
{
    return (Vz.Ln(((1 + value) / (1 - value))) / 2);
});

var Semi_Major_Axis = Vz.DeclareCustomExpression("Semi-Major Axis", "position", "velocity").SetReturn((position, velocity) =>
{
    return ((Eccentricity(position, velocity) < 1) ? (-1 * (Mu() / (2 * Specific_Orbital_Energy(position, velocity)))) : (1 / ((2 * Vz.Length(position)) - ((Vz.Length(velocity) ^ 2) / Mu()))));
});

var Clamp = Vz.DeclareCustomExpression("Clamp", "value", "min", "max").SetReturn((value, min, max) =>
{
    return Vz.Max(min, Vz.Min(max, value));
});

var Different_Sign = Vz.DeclareCustomExpression("Different Sign", "value1", "value2").SetReturn((value1, value2) =>
{
    return (((value1 >= 0) && (value2 < 0)) || ((value2 >= 0) && (0 < value1)));
});

var Stump_C = Vz.DeclareCustomExpression("Stump C", "Z").SetReturn((Z) =>
{
    return ((Z < 0) ? ((Cosh(Vz.Sqrt((-1 * Z))) - 1) / (-1 * Z)) : ((Z > -0) ? ((1 - Vz.Cos(Vz.Sqrt(Z))) / Z) : 0.5));
});

var Stump_S = Vz.DeclareCustomExpression("Stump S", "Z").SetReturn((Z) =>
{
    return ((Z < 0) ? ((Sinh(Vz.Sqrt((-1 * Z))) - Vz.Sqrt((-1 * Z))) / (Vz.Sqrt((-1 * Z)) ^ 3)) : ((Z > -0) ? ((Vz.Sqrt(Z) - Vz.Sin(Vz.Sqrt(Z))) / (Vz.Sqrt(Z) ^ 3)) : (1 / 6)));
});


// ── Serialize output ──
Console.WriteLine(Vz.context.currentProgram.Serialize().ToString());
