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

// VZTOPBLOCK
// VZBLOCK PEV2ZW50IGV2ZW50PSJSZWNlaXZlTWVzc2FnZSIgaWQ9IjAiIHN0eWxlPSJyZWNlaXZlLW1zZyIgcG9zPSIyMTk4Ljk2NywtNTkuNzM3MjMiPjxDb25zdGFudCBjYW5SZXBsYWNlPSJmYWxzZSIgdGV4dD0iVGltZXdhcnAiIC8+PC9FdmVudD4=
// VZEL PEV2ZW50IGV2ZW50PSJSZWNlaXZlTWVzc2FnZSIgaWQ9IjAiIHN0eWxlPSJyZWNlaXZlLW1zZyIgcG9zPSIyMTk4Ljk2NywtNTkuNzM3MjMiIC8+
using (new OnReceiveMessage("Timewarp"))
{
    // VZEL PFdhaXRTZWNvbmRzIGlkPSIxIiBzdHlsZT0id2FpdC1zZWNvbmRzIiAvPg==
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"));
    // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwMSIgaWQ9IjIiIHN0eWxlPSJzZXQtdGltZS1tb2RlIiAvPg==
    Vz.SetTimeModeAttr("TimeWarp1");
    // VZEL PElmIGlkPSIzIiBzdHlsZT0iaWYiIC8+
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjI1IiAvPg=="))))
    {
        // VZEL PFdhaXRTZWNvbmRzIGlkPSI0IiBzdHlsZT0id2FpdC1zZWNvbmRzIiAvPg==
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjUiIC8+"));
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwMiIgaWQ9IjUiIHN0eWxlPSJzZXQtdGltZS1tb2RlIiAvPg==
        Vz.SetTimeModeAttr("TimeWarp2");
    }
    // VZEL PElmIGlkPSI2IiBzdHlsZT0iaWYiIC8+
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwMCIgLz4="))))
    {
        // VZEL PFdhaXRTZWNvbmRzIGlkPSI3IiBzdHlsZT0id2FpdC1zZWNvbmRzIiAvPg==
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEyLjUiIC8+"));
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwMyIgaWQ9IjgiIHN0eWxlPSJzZXQtdGltZS1tb2RlIiAvPg==
        Vz.SetTimeModeAttr("TimeWarp3");
    }
    // VZEL PElmIGlkPSI5IiBzdHlsZT0iaWYiIC8+
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjUwMCIgLz4="))))
    {
        // VZEL PFdhaXRTZWNvbmRzIGlkPSIxMCIgc3R5bGU9IndhaXQtc2Vjb25kcyIgLz4=
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjUwIiAvPg=="));
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwNCIgaWQ9IjExIiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp4");
    }
    // VZEL PElmIGlkPSIxMiIgc3R5bGU9ImlmIiAvPg==
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjI1MDAiIC8+"))))
    {
        // VZEL PFdhaXRTZWNvbmRzIGlkPSIxMyIgc3R5bGU9IndhaXQtc2Vjb25kcyIgLz4=
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjI1MCIgLz4="));
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwNSIgaWQ9IjE0IiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp5");
    }
    // VZEL PElmIGlkPSIxNSIgc3R5bGU9ImlmIiAvPg==
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwMDAwIiAvPg=="))))
    {
        // VZEL PFdhaXRTZWNvbmRzIGlkPSIxNiIgc3R5bGU9IndhaXQtc2Vjb25kcyIgLz4=
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEyNTAiIC8+"));
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwNiIgaWQ9IjE3IiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp6");
    }
    // VZEL PElmIGlkPSIxOCIgc3R5bGU9ImlmIiAvPg==
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjUwMDAwIiAvPg=="))))
    {
        // VZEL PFdhaXRTZWNvbmRzIGlkPSIxOSIgc3R5bGU9IndhaXQtc2Vjb25kcyIgLz4=
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjUwMDAiIC8+"));
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwNyIgaWQ9IjIwIiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp7");
    }
    // VZEL PElmIGlkPSIyMSIgc3R5bGU9ImlmIiAvPg==
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjI1MDAwMCIgLz4="))))
    {
        // VZEL PFdhaXRTZWNvbmRzIGlkPSIyMiIgc3R5bGU9IndhaXQtc2Vjb25kcyIgLz4=
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjI1MDAwIiAvPg=="));
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwOCIgaWQ9IjIzIiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp8");
    }
    // VZEL PElmIGlkPSIyNCIgc3R5bGU9ImlmIiAvPg==
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwMDAwMDAiIC8+"))))
    {
        // VZEL PFdhaXRTZWNvbmRzIGlkPSIyNSIgc3R5bGU9IndhaXQtc2Vjb25kcyIgLz4=
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEyNTAwMCIgLz4="));
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwOSIgaWQ9IjI2IiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp9");
    }
    // VZEL PElmIGlkPSIyNyIgc3R5bGU9ImlmIiAvPg==
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjUwMDAwMDAiIC8+"))))
    {
        // VZEL PFdhaXRTZWNvbmRzIGlkPSIyOCIgc3R5bGU9IndhaXQtc2Vjb25kcyIgLz4=
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjUwMDAwMCIgLz4="));
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwMTAiIGlkPSIyOSIgc3R5bGU9InNldC10aW1lLW1vZGUiIC8+
        Vz.SetTimeModeAttr("TimeWarp10");
        // VZEL PFdhaXRVbnRpbCBpZD0iMzAiIHN0eWxlPSJ3YWl0LXVudGlsIiAvPg==
        using (new WaitUntil(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjUwMDAwMCIgLz4=")))) { }
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwOSIgaWQ9IjMxIiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp9");
    }
    // VZEL PElmIGlkPSIzMiIgc3R5bGU9ImlmIiAvPg==
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEyNTAwMCIgLz4="))))
    {
        // VZEL PFdhaXRVbnRpbCBpZD0iMzMiIHN0eWxlPSJ3YWl0LXVudGlsIiAvPg==
        using (new WaitUntil(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEyNTAwMCIgLz4=")))) { }
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwOCIgaWQ9IjM0IiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp8");
    }
    // VZEL PElmIGlkPSIzNSIgc3R5bGU9ImlmIiAvPg==
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjI1MDAwIiAvPg=="))))
    {
        // VZEL PFdhaXRVbnRpbCBpZD0iMzYiIHN0eWxlPSJ3YWl0LXVudGlsIiAvPg==
        using (new WaitUntil(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjI1MDAwIiAvPg==")))) { }
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwNyIgaWQ9IjM3IiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp7");
    }
    // VZEL PElmIGlkPSIzOCIgc3R5bGU9ImlmIiAvPg==
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwMDAwIiAvPg=="))))
    {
        // VZEL PFdhaXRVbnRpbCBpZD0iMzkiIHN0eWxlPSJ3YWl0LXVudGlsIiAvPg==
        using (new WaitUntil(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwMDAwIiAvPg==")))) { }
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwNiIgaWQ9IjQwIiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp6");
    }
    // VZEL PElmIGlkPSI0MSIgc3R5bGU9ImlmIiAvPg==
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjI1MDAiIC8+"))))
    {
        // VZEL PFdhaXRVbnRpbCBpZD0iNDIiIHN0eWxlPSJ3YWl0LXVudGlsIiAvPg==
        using (new WaitUntil(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjI1MDAiIC8+")))) { }
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwNSIgaWQ9IjQzIiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp5");
    }
    // VZEL PElmIGlkPSI0NCIgc3R5bGU9ImlmIiAvPg==
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjUwMCIgLz4="))))
    {
        // VZEL PFdhaXRVbnRpbCBpZD0iNDUiIHN0eWxlPSJ3YWl0LXVudGlsIiAvPg==
        using (new WaitUntil(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjUwMCIgLz4=")))) { }
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwNCIgaWQ9IjQ2IiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp4");
    }
    // VZEL PElmIGlkPSI0NyIgc3R5bGU9ImlmIiAvPg==
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwMCIgLz4="))))
    {
        // VZEL PFdhaXRVbnRpbCBpZD0iNDgiIHN0eWxlPSJ3YWl0LXVudGlsIiAvPg==
        using (new WaitUntil(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwMCIgLz4=")))) { }
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwMyIgaWQ9IjQ5IiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp3");
    }
    // VZEL PElmIGlkPSI1MCIgc3R5bGU9ImlmIiAvPg==
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjI1IiAvPg=="))))
    {
        // VZEL PFdhaXRVbnRpbCBpZD0iNTEiIHN0eWxlPSJ3YWl0LXVudGlsIiAvPg==
        using (new WaitUntil(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjI1IiAvPg==")))) { }
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwMiIgaWQ9IjUyIiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp2");
    }
    // VZEL PElmIGlkPSI1MyIgc3R5bGU9ImlmIiAvPg==
    using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwIiAvPg=="))))
    {
        // VZEL PFdhaXRVbnRpbCBpZD0iNTQiIHN0eWxlPSJ3YWl0LXVudGlsIiAvPg==
        using (new WaitUntil(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwIiAvPg==")))) { }
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwMSIgaWQ9IjU1IiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
        Vz.SetTimeModeAttr("TimeWarp1");
    }
    // VZEL PFdhaXRVbnRpbCBpZD0iNTYiIHN0eWxlPSJ3YWl0LXVudGlsIiAvPg==
    using (new WaitUntil(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==") - Vz.Time.TotalTime()) < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")))) { }
    // VZEL PFNldFRpbWVNb2RlIG1vZGU9Ik5vcm1hbCIgaWQ9IjU3IiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
    Vz.SetTimeModeAttr("Normal");
}

// VZTOPBLOCK
// VZBLOCK PEN1c3RvbUluc3RydWN0aW9uIGNhbGxGb3JtYXQ9Ik9yYml0YWwgU3RhdGUgVmVjdG9ycyAoMCkgIiBmb3JtYXQ9Ik9yYml0YWwgU3RhdGUgVmVjdG9ycyB8VHJ1ZUFub21hbHl8ICIgbmFtZT0iT3JiaXRhbCBTdGF0ZSBWZWN0b3JzIiBpZD0iNTgiIHN0eWxlPSJjdXN0b20taW5zdHJ1Y3Rpb24iIHBvcz0iMjg0Ny44MywxODUuNyIgLz4=
// ── Custom Instructions ──────────────────────────────
var Orbital_State_Vectors = Vz.DeclareCustomInstruction("Orbital State Vectors", "TrueAnomaly").SetInstructions((TrueAnomaly) =>
{
    // VZEL PFNldFZhcmlhYmxlIGlkPSI1OSIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVHJ1ZUFub21hbHkiIC8+PC9TZXRWYXJpYWJsZT4=
    TrueAnomaly = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUcnVlQW5vbWFseSIgLz4=");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI2MCIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUmFkaXVzIiAvPjwvU2V0VmFyaWFibGU+
    Radius = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9Iihwb3cobWFnbml0dWRlKHY6QW5ndWxhck1vbWVudHVtKSwyKS9NdSkvKDErKEVjY2VudHJpY2l0eSpjb3MoVHJ1ZUFub21hbHkpKSkiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI2MSIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRmxpZ2h0UGF0aEFuZ2xlIiAvPjwvU2V0VmFyaWFibGU+
    FlightPathAngle = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9ImF0YW4oKEVjY2VudHJpY2l0eSpzaW4oVHJ1ZUFub21hbHkpKS8oMSsoRWNjZW50cmljaXR5KmNvcyhUcnVlQW5vbWFseSkpKSkiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI2MiIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+PC9TZXRWYXJpYWJsZT4=
    Position = Rotate_Y_Axis_Vector(Rotate_X_Axis_Vector(Rotate_Y_Axis_Vector((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUmFkaXVzIiAvPg==") * Vz.Vec(Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9ImNvcyhUcnVlQW5vbWFseSkiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"), Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InNpbihUcnVlQW5vbWFseSkiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+"))), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQXJndW1lbnRvZlBlcmlhcHNpcyIgLz4=")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSW5jbGluYXRpb24iIC8+")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUmlnaHRBc2NlbnNpb24iIC8+"));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI2MyIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+PC9TZXRWYXJpYWJsZT4=
    Velocity = Rotate_Y_Axis_Vector(Rotate_X_Axis_Vector(Rotate_Y_Axis_Vector((Orbital_Speed(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUmFkaXVzIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU2VtaU1ham9yQXhpcyIgLz4=")) * Vz.Vec(Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9ImNvcyhUcnVlQW5vbWFseSsoKHBpKS8yKS1GbGlnaHRQYXRoQW5nbGUpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"), Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InNpbihUcnVlQW5vbWFseSsoKHBpKS8yKS1GbGlnaHRQYXRoQW5nbGUpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg=="))), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQXJndW1lbnRvZlBlcmlhcHNpcyIgLz4=")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSW5jbGluYXRpb24iIC8+")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUmlnaHRBc2NlbnNpb24iIC8+"));
});

// VZTOPBLOCK
// VZBLOCK PEN1c3RvbUluc3RydWN0aW9uIGNhbGxGb3JtYXQ9IlBsYW5ldCAoMCkgVmVjdG9yIEFmdGVyIFRpbWUgKDEpICIgZm9ybWF0PSJQbGFuZXQgfE5hbWV8IFZlY3RvciBBZnRlciBUaW1lIHxUaW1lfCAiIG5hbWU9IlBsYW5ldCIgaWQ9IjY0IiBzdHlsZT0iY3VzdG9tLWluc3RydWN0aW9uIiBwb3M9IjEzNDkuOTMsMzMzLjcxMzciIC8+
var Planet = Vz.DeclareCustomInstruction("Planet", "Name", "Time").SetInstructions((Name, Time) =>
{
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iNjUiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_Elements_Parent_Body(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQYXJlbnRzIiAvPg=="), Vz.ListIndex(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJOYW1lIiAvPg=="))), Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQb3NpdGlvbnMiIC8+"), Vz.ListIndex(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJOYW1lIiAvPg=="))), Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJWZWxvY2l0aWVzIiAvPg=="), Vz.ListIndex(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJOYW1lIiAvPg=="))));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUcnVlIEFub21hbHkgQWZ0ZXIgVGltZSIgaWQ9IjY2IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    True_Anomaly_After_Time(((Vz.Time.TotalTime() + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUaW1lIiAvPg==")) - Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRUaW1lcyIgLz4="), Vz.ListIndex(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJOYW1lIiAvPg==")))));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSI2NyIgc3R5bGU9ImNhbGwtY3VzdG9tLWluc3RydWN0aW9uIiAvPg==
    Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVHJ1ZUFub21hbHkiIC8+"));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI2OCIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0UG9zaXRpb24iIC8+PC9TZXRWYXJpYWJsZT4=
    PlanetPosition = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI2OSIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0VmVsb2NpdHkiIC8+PC9TZXRWYXJpYWJsZT4=
    PlanetVelocity = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+");
});

// VZTOPBLOCK
// VZBLOCK PEN1c3RvbUluc3RydWN0aW9uIGNhbGxGb3JtYXQ9Ik9yYml0YWwgRWxlbWVudHMgUGFyZW50IEJvZHkgKDApIFBvc2l0aW9uICgxKSBWZWxvY2l0eSAoMikgIiBmb3JtYXQ9Ik9yYml0YWwgRWxlbWVudHMgUGFyZW50IEJvZHkgfFBsYW5ldHwgUG9zaXRpb24gfFBvc2l0aW9ufCBWZWxvY2l0eSB8VmVsb2NpdHl8ICIgbmFtZT0iT3JiaXRhbCBFbGVtZW50cyBQYXJlbnQgQm9keSIgaWQ9IjcwIiBzdHlsZT0iY3VzdG9tLWluc3RydWN0aW9uIiBwb3M9IjI4NTEuMDUzLDE2NzQuNjAyIiAvPg==
var Orbital_Elements_Parent_Body = Vz.DeclareCustomInstruction("Orbital Elements Parent Body", "Planet", "Position", "Velocity").SetInstructions((Planet, Position, Velocity) =>
{
    // VZEL PFNldFZhcmlhYmxlIGlkPSI3MSIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+PC9TZXRWYXJpYWJsZT4=
    Position = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQb3NpdGlvbiIgLz4=");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI3MiIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+PC9TZXRWYXJpYWJsZT4=
    Velocity = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWxvY2l0eSIgLz4=");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI3MyIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTXUiIC8+PC9TZXRWYXJpYWJsZT4=
    Mu = (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjYuNjczODRFLTExIiAvPg==") * Vz.Planet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXQiIC8+")).Mass());
    // VZEL PFNldFZhcmlhYmxlIGlkPSI3NCIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU2VtaUxhdHVzUmVjdHVtIiAvPjwvU2V0VmFyaWFibGU+
    SemiLatusRectum = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBvdyhtYWduaXR1ZGUoY3Jvc3ModjpQb3NpdGlvbix2OlZlbG9jaXR5KSksMikvTXUiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI3NSIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU2VtaU1ham9yQXhpcyIgLz48L1NldFZhcmlhYmxlPg==
    SemiMajorAxis = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjEvKCgyLyhtYWduaXR1ZGUodjpQb3NpdGlvbikpKS0ocG93KG1hZ25pdHVkZSh2OlZlbG9jaXR5KSwyKS9NdSkpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI3NiIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPjwvU2V0VmFyaWFibGU+
    AngularMomentum = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9ImNyb3NzKHY6UG9zaXRpb24sdjpWZWxvY2l0eSkiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI3NyIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5VmVjdG9yIiAvPjwvU2V0VmFyaWFibGU+
    EccentricityVector = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9Iihjcm9zcyh2OlZlbG9jaXR5LHY6QW5ndWxhck1vbWVudHVtKS9NdSktbm9ybWFsaXplKHY6UG9zaXRpb24pIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI3OCIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5IiAvPjwvU2V0VmFyaWFibGU+
    Eccentricity = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9Im1hZ25pdHVkZSh2OkVjY2VudHJpY2l0eVZlY3RvcikiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI3OSIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTm9kZSIgLz48L1NldFZhcmlhYmxlPg==
    Node = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9ImNyb3NzKHY6WWF4aXMsdjpBbmd1bGFyTW9tZW50dW0pIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==");
    // VZEL PElmIGlkPSI4MCIgc3R5bGU9ImlmIiAvPg==
    using (new If((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5IiAvPg==") < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuMDAwMSIgLz4="))))
    {
        // VZEL PFNldFZhcmlhYmxlIGlkPSI4MSIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5VmVjdG9yIiAvPjwvU2V0VmFyaWFibGU+
        EccentricityVector = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTm9kZSIgLz4=");
    }
    // VZEL PFNldFZhcmlhYmxlIGlkPSI4MiIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTWVhbkFuZ3VsYXJNb3Rpb24iIC8+PC9TZXRWYXJpYWJsZT4=
    MeanAngularMotion = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InNxcnQocG93KGFicyhTZW1pTWFqb3JBeGlzKSwzKS9NdSkiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI4MyIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGVyaW9kIiAvPjwvU2V0VmFyaWFibGU+
    Period = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IihwaSkqMipNZWFuQW5ndWxhck1vdGlvbiIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI4NCIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGVyaWFwc2lzIiAvPjwvU2V0VmFyaWFibGU+
    Periapsis = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IigxLUVjY2VudHJpY2l0eSkqU2VtaU1ham9yQXhpcyIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI4NSIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQXBvYXBzaXMiIC8+PC9TZXRWYXJpYWJsZT4=
    Apoapsis = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IigxK0VjY2VudHJpY2l0eSkqU2VtaU1ham9yQXhpcyIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI4NiIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQXJndW1lbnRvZlBlcmlhcHNpcyIgLz48L1NldFZhcmlhYmxlPg==
    ArgumentofPeriapsis = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InNpZ25lZEFuZ2xlKHY6Tm9kZSx2OkVjY2VudHJpY2l0eVZlY3Rvcix2OkFuZ3VsYXJNb21lbnR1bSkqKChwaSkvMTgwKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI4NyIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSW5jbGluYXRpb24iIC8+PC9TZXRWYXJpYWJsZT4=
    Inclination = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9ImFjb3MoMC15KHY6QW5ndWxhck1vbWVudHVtKS9tYWduaXR1ZGUodjpBbmd1bGFyTW9tZW50dW0pKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI4OCIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUmlnaHRBc2NlbnNpb24iIC8+PC9TZXRWYXJpYWJsZT4=
    RightAscension = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InNpZ25lZEFuZ2xlKHY6Tm9kZSx2OlhheGlzLHY6WWF4aXMpKigocGkpLzE4MCkiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI4OSIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVHJ1ZUFub21hbHkiIC8+PC9TZXRWYXJpYWJsZT4=
    TrueAnomaly = Signed_Angle(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5VmVjdG9yIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPg=="));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI5MCIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU09JIiAvPjwvU2V0VmFyaWFibGU+
    SOI = Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRTT0kiIC8+"), Vz.ListIndex(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXQiIC8+")));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI5MSIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU09JRXhpdFRydWVBbm9tYWx5IiAvPjwvU2V0VmFyaWFibGU+
    SOIExitTrueAnomaly = ((((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQXBvYXBzaXMiIC8+") > Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU09JIiAvPg==")) || (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5IiAvPg==") > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"))) && (Vz.LengthOf(Vz.Planet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXQiIC8+")).Parent()) > 0)) ? Vz.Acos(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU2VtaUxhdHVzUmVjdHVtIiAvPg==") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU09JIiAvPg==")) / (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5IiAvPg==") * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU09JIiAvPg==")))) : "");
    // VZEL PElmIGlkPSI5MiIgc3R5bGU9ImlmIiAvPg==
    using (new If((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5IiAvPg==") < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"))))
    {
        // VZEL PFNldFZhcmlhYmxlIGlkPSI5MyIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljQW5vbWFseSIgLz48L1NldFZhcmlhYmxlPg==
        EccentricAnomaly = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqYXRhbihzcXJ0KCgxLUVjY2VudHJpY2l0eSkvKDErRWNjZW50cmljaXR5KSkqdGFuKFRydWVBbm9tYWx5LzIpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=");
        // VZEL PFNldFZhcmlhYmxlIGlkPSI5NCIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTWVhbkFub21hbHkiIC8+PC9TZXRWYXJpYWJsZT4=
        MeanAnomaly = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkVjY2VudHJpY0Fub21hbHktKEVjY2VudHJpY2l0eSpzaW4oRWNjZW50cmljQW5vbWFseSkpJmx0OzA/KChFY2NlbnRyaWNBbm9tYWx5LShFY2NlbnRyaWNpdHkqc2luKEVjY2VudHJpY0Fub21hbHkpKSkrKDIqKHBpKSkpOihFY2NlbnRyaWNBbm9tYWx5LShFY2NlbnRyaWNpdHkqc2luKEVjY2VudHJpY0Fub21hbHkpKSkiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+");
    }
    // VZEL PEVsc2VJZiBpZD0iOTUiIHN0eWxlPSJlbHNlIiAvPg==
    using (new ElseIf(Vz.RawConstant("PENvbnN0YW50IGJvb2w9InRydWUiIC8+")))
    {
        // VZEL PFNldFZhcmlhYmxlIGlkPSI5NiIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSHlwZXJib2xpY0Fub21hbHkiIC8+PC9TZXRWYXJpYWJsZT4=
        HyperbolicAnomaly = (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+") * atanh(Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InNxcnQoKEVjY2VudHJpY2l0eS0xKS8oRWNjZW50cmljaXR5KzEpKSp0YW4oVHJ1ZUFub21hbHkvMikiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+")));
        // VZEL PFNldFZhcmlhYmxlIGlkPSI5NyIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTWVhbkFub21hbHkiIC8+PC9TZXRWYXJpYWJsZT4=
        MeanAnomaly = (((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5IiAvPg==") * sinh(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSHlwZXJib2xpY0Fub21hbHkiIC8+"))) - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSHlwZXJib2xpY0Fub21hbHkiIC8+")) * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTWVhbkFuZ3VsYXJNb3Rpb24iIC8+"));
    }
});

// VZTOPBLOCK
// VZBLOCK PEN1c3RvbUluc3RydWN0aW9uIGNhbGxGb3JtYXQ9IlRpbWUgdG8gVHJ1ZUFub21hbHkgKDApICIgZm9ybWF0PSJUaW1lIHRvIFRydWVBbm9tYWx5IHxUcnVlQW5vbWFseXwgIiBuYW1lPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iOTgiIHN0eWxlPSJjdXN0b20taW5zdHJ1Y3Rpb24iIHBvcz0iMjg1Ni42MDYsMjMxMy41OTUiIC8+
var Time_to_TrueAnomaly = Vz.DeclareCustomInstruction("Time to TrueAnomaly", "TrueAnomaly").SetInstructions((TrueAnomaly) =>
{
    // VZEL PFNldFZhcmlhYmxlIGlkPSI5OSIgc3R5bGU9InNldC12YXJpYWJsZSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVHJ1ZUFub21hbHkiIC8+PC9TZXRWYXJpYWJsZT4=
    TrueAnomaly = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUcnVlQW5vbWFseSIgLz4=");
    // VZEL PElmIGlkPSIxMDAiIHN0eWxlPSJpZiIgLz4=
    using (new If((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5IiAvPg==") < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"))))
    {
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxMDEiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWV0b1RBIiAvPjwvU2V0VmFyaWFibGU+
        TimetoTA = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9Ik1lYW5Bbm9tYWx5Kk1lYW5Bbmd1bGFyTW90aW9uIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==");
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxMDIiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkVjY2VudHJpY0Fub21hbHkiIC8+PC9TZXRWYXJpYWJsZT4=
        EccentricAnomaly = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqYXRhbihzcXJ0KCgxLUVjY2VudHJpY2l0eSkvKDErRWNjZW50cmljaXR5KSkqdGFuKFRydWVBbm9tYWx5LzIpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=");
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxMDMiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik1lYW5Bbm9tYWx5IiAvPjwvU2V0VmFyaWFibGU+
        MeanAnomaly = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkVjY2VudHJpY0Fub21hbHktKEVjY2VudHJpY2l0eSpzaW4oRWNjZW50cmljQW5vbWFseSkpJmx0OzA/KChFY2NlbnRyaWNBbm9tYWx5LShFY2NlbnRyaWNpdHkqc2luKEVjY2VudHJpY0Fub21hbHkpKSkrKDIqKHBpKSkpKk1lYW5Bbmd1bGFyTW90aW9uOihFY2NlbnRyaWNBbm9tYWx5LShFY2NlbnRyaWNpdHkqc2luKEVjY2VudHJpY0Fub21hbHkpKSkqTWVhbkFuZ3VsYXJNb3Rpb24iIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+");
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxMDQiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWV0b1RBIiAvPjwvU2V0VmFyaWFibGU+
        TimetoTA = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9Ik1lYW5Bbm9tYWx5LVRpbWV0b1RBJmx0OzA/TWVhbkFub21hbHktVGltZXRvVEErUGVyaW9kOk1lYW5Bbm9tYWx5LVRpbWV0b1RBIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==");
    }
    // VZEL PEVsc2VJZiBpZD0iMTA1IiBzdHlsZT0iZWxzZSIgLz4=
    using (new ElseIf(Vz.RawConstant("PENvbnN0YW50IGJvb2w9InRydWUiIC8+")))
    {
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxMDYiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWV0b1RBIiAvPjwvU2V0VmFyaWFibGU+
        TimetoTA = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTWVhbkFub21hbHkiIC8+");
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxMDciIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ikh5cGVyYm9saWNBbm9tYWx5IiAvPjwvU2V0VmFyaWFibGU+
        HyperbolicAnomaly = (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+") * atanh(Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InNxcnQoKEVjY2VudHJpY2l0eS0xKS8oRWNjZW50cmljaXR5KzEpKSp0YW4oVHJ1ZUFub21hbHkvMikiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+")));
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxMDgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik1lYW5Bbm9tYWx5IiAvPjwvU2V0VmFyaWFibGU+
        MeanAnomaly = (((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5IiAvPg==") * sinh(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSHlwZXJib2xpY0Fub21hbHkiIC8+"))) - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSHlwZXJib2xpY0Fub21hbHkiIC8+")) * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTWVhbkFuZ3VsYXJNb3Rpb24iIC8+"));
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxMDkiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWV0b1RBIiAvPjwvU2V0VmFyaWFibGU+
        TimetoTA = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9Ik1lYW5Bbm9tYWx5LVRpbWV0b1RBIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==");
    }
});

// VZTOPBLOCK
// VZBLOCK PEN1c3RvbUluc3RydWN0aW9uIGNhbGxGb3JtYXQ9IlBsYW5ldExpc3RzIiBmb3JtYXQ9IlBsYW5ldExpc3RzIiBuYW1lPSJQbGFuZXRMaXN0cyIgaWQ9IjExMCIgc3R5bGU9ImN1c3RvbS1pbnN0cnVjdGlvbiIgcG9zPSItMzAxLjgxMDEsMTkyOC43OCIgLz4=
var PlanetLists = Vz.DeclareCustomInstruction("PlanetLists").SetInstructions(() =>
{
    // VZEL PFNldFZhcmlhYmxlIGlkPSIxMTEiIHN0eWxlPSJsaXN0LWluaXQiPjxWYXJpYWJsZSBsaXN0PSJ0cnVlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0TmFtZXMiIC8+PC9TZXRWYXJpYWJsZT4=
    PlanetNames = Vz.Planet("Juno").ChildPlanets();
    // VZEL PEZvciB2YXI9ImkiIGlkPSIxMTIiIHN0eWxlPSJmb3IiPjxDb25zdGFudCBudW1iZXI9IjEiIC8+PExpc3RPcCBvcD0ibGVuZ3RoIiBzdHlsZT0ibGlzdC1sZW5ndGgiPjxWYXJpYWJsZSBsaXN0PSJ0cnVlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0TmFtZXMiIC8+PC9MaXN0T3A+PENvbnN0YW50IG51bWJlcj0iMSIgLz48L0Zvcj4=
    using (new For("i").From(1).To(Vz.ListLength(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="))).By(1))
    {
        // VZEL PEZvciB2YXI9ImsiIGlkPSIxMTMiIHN0eWxlPSJmb3IiPjxDb25zdGFudCBudW1iZXI9IjEiIC8+PExpc3RPcCBvcD0ibGVuZ3RoIiBzdHlsZT0ibGlzdC1sZW5ndGgiPjxQbGFuZXQgb3A9ImNoaWxkUGxhbmV0cyIgc3R5bGU9InBsYW5ldCI+PExpc3RPcCBvcD0iZ2V0IiBzdHlsZT0ibGlzdC1nZXQiPjxWYXJpYWJsZSBsaXN0PSJ0cnVlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0TmFtZXMiIC8+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPjwvTGlzdE9wPjwvUGxhbmV0PjwvTGlzdE9wPjxDb25zdGFudCBudW1iZXI9IjEiIC8+PC9Gb3I+
        using (new For("k").From(1).To(Vz.ListLength(Vz.Planet(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="))).ChildPlanets())).By(1))
        {
            // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjExNCIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
            Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), Vz.ListGet(Vz.Planet(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="))).ChildPlanets(), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJrIiAvPg==")));
        }
    }
    // VZEL PFNldFZhcmlhYmxlIGlkPSIxMTUiIHN0eWxlPSJsaXN0LWluaXQiPjxWYXJpYWJsZSBsaXN0PSJ0cnVlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0U09JIiAvPjwvU2V0VmFyaWFibGU+
    PlanetSOI = Vz.CreateListRaw("30000000,80000000,0,500000,0,0,0,5500000,0,1500000,60000,500000,9000000,15000000,3500000,0,250000,9000000,0");
    // VZEL PEZvciB2YXI9ImkiIGlkPSIxMTYiIHN0eWxlPSJmb3IiPjxDb25zdGFudCBudW1iZXI9IjEiIC8+PExpc3RPcCBvcD0ibGVuZ3RoIiBzdHlsZT0ibGlzdC1sZW5ndGgiPjxWYXJpYWJsZSBsaXN0PSJ0cnVlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0TmFtZXMiIC8+PC9MaXN0T3A+PENvbnN0YW50IG51bWJlcj0iMSIgLz48L0Zvcj4=
    using (new For("i").From(1).To(Vz.ListLength(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="))).By(1))
    {
        // VZEL PFNldFRhcmdldCBpZD0iMTE3IiBzdHlsZT0ic2V0LXRhcmdldCIgLz4=
        Vz.TargetNode(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg==")));
        // VZEL PFdhaXRTZWNvbmRzIGlkPSIxMTgiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
        // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjExOSIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
        Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQb3NpdGlvbnMiIC8+"), Vz.Craft.Target.Position());
        // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjEyMCIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
        Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJWZWxvY2l0aWVzIiAvPg=="), Vz.Craft.Target.Velocity());
        // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjEyMSIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
        Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRUaW1lcyIgLz4="), Vz.Time.TotalTime());
        // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjEyMiIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
        Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQYXJlbnRzIiAvPg=="), Vz.Planet(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="))).Parent());
    }
    // VZEL PFdhaXRTZWNvbmRzIGlkPSIxMjMiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    // VZEL PFNldFRhcmdldCBpZD0iMTI0IiBzdHlsZT0ic2V0LXRhcmdldCIgLz4=
    Vz.TargetNode("");
    // VZEL PEZvciB2YXI9ImkiIGlkPSIxMjUiIHN0eWxlPSJmb3IiPjxDb25zdGFudCBudW1iZXI9IjEiIC8+PExpc3RPcCBvcD0ibGVuZ3RoIiBzdHlsZT0ibGlzdC1sZW5ndGgiPjxWYXJpYWJsZSBsaXN0PSJ0cnVlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0TmFtZXMiIC8+PC9MaXN0T3A+PENvbnN0YW50IG51bWJlcj0iMSIgLz48L0Zvcj4=
    using (new For("i").From(1).To(Vz.ListLength(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="))).By(1))
    {
        // VZEL PElmIGlkPSIxMjYiIHN0eWxlPSJpZiIgLz4=
        using (new If((Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRTT0kiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg==")) == 0)))
        {
            // VZEL PFNldFZhcmlhYmxlIGlkPSIxMjciIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlBvc2l0aW9uIiAvPjwvU2V0VmFyaWFibGU+
            Position = Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQb3NpdGlvbnMiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="));
            // VZEL PFNldFZhcmlhYmxlIGlkPSIxMjgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlZlbG9jaXR5IiAvPjwvU2V0VmFyaWFibGU+
            Velocity = Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJWZWxvY2l0aWVzIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="));
            // VZEL PFNldFZhcmlhYmxlIGlkPSIxMjkiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik11IiAvPjwvU2V0VmFyaWFibGU+
            Mu = (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjYuNjczODRFLTExIiAvPg==") * Vz.Planet(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQYXJlbnRzIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="))).Mass());
            // VZEL PFNldFZhcmlhYmxlIGlkPSIxMzAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlNlbWlNYWpvckF4aXMiIC8+PC9TZXRWYXJpYWJsZT4=
            SemiMajorAxis = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjEvKCgyLyhtYWduaXR1ZGUodjpQb3NpdGlvbikpKS0ocG93KG1hZ25pdHVkZSh2OlZlbG9jaXR5KSwyKS9NdSkpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==");
            // VZEL PFNldExpc3Qgb3A9InNldCIgaWQ9IjEzMSIgc3R5bGU9Imxpc3Qtc2V0IiAvPg==
            Vz.ListSet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRTT0kiIC8+"), (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU2VtaU1ham9yQXhpcyIgLz4=") * ((Vz.Planet(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="))).Mass() / Vz.Planet(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQYXJlbnRzIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="))).Mass()) ^ Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuNCIgLz4="))), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="));
        }
    }
    // VZEL PFdhaXRTZWNvbmRzIGlkPSIxMzIiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
});

// VZTOPBLOCK
// VZBLOCK PEN1c3RvbUluc3RydWN0aW9uIGNhbGxGb3JtYXQ9IkNyYWZ0ICgwKSAoMSkgKDIpIFZlY3RvciBBZnRlciBUaW1lICgzKSAiIGZvcm1hdD0iQ3JhZnQgfFBvc2l0aW9ufCB8VmVsb2NpdHl8IHxQYXJlbnRCb2R5fCBWZWN0b3IgQWZ0ZXIgVGltZSB8VGltZXwgIiBuYW1lPSJDcmFmdCIgaWQ9IjEzMyIgc3R5bGU9ImN1c3RvbS1pbnN0cnVjdGlvbiIgcG9zPSIxMzQ0LjY1MSw2NjUuMzA0NCIgLz4=
var Craft = Vz.DeclareCustomInstruction("Craft", "Position", "Velocity", "ParentBody", "Time").SetInstructions((Position, Velocity, ParentBody, Time) =>
{
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMTM0IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQYXJlbnRCb2R5IiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQb3NpdGlvbiIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWxvY2l0eSIgLz4="));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUcnVlIEFub21hbHkgQWZ0ZXIgVGltZSIgaWQ9IjEzNSIgc3R5bGU9ImNhbGwtY3VzdG9tLWluc3RydWN0aW9uIiAvPg==
    True_Anomaly_After_Time(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUaW1lIiAvPg=="));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSIxMzYiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVHJ1ZUFub21hbHkiIC8+"));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIxMzciIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkNyYWZ0UG9zaXRpb24iIC8+PC9TZXRWYXJpYWJsZT4=
    CraftPosition = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSIxMzgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkNyYWZ0VmVsb2NpdHkiIC8+PC9TZXRWYXJpYWJsZT4=
    CraftVelocity = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+");
});

// VZTOPBLOCK
// VZBLOCK PEN1c3RvbUluc3RydWN0aW9uIGNhbGxGb3JtYXQ9IlRydWUgQW5vbWFseSBBZnRlciBUaW1lICgwKSAiIGZvcm1hdD0iVHJ1ZSBBbm9tYWx5IEFmdGVyIFRpbWUgfFRpbWV8ICIgbmFtZT0iVHJ1ZSBBbm9tYWx5IEFmdGVyIFRpbWUiIGlkPSIxMzkiIHN0eWxlPSJjdXN0b20taW5zdHJ1Y3Rpb24iIHBvcz0iMTM1NS4wODgsMTgwNS40NDQiIC8+
var True_Anomaly_After_Time = Vz.DeclareCustomInstruction("True Anomaly After Time", "Time").SetInstructions((Time) =>
{
    // VZEL PElmIGlkPSIxNDAiIHN0eWxlPSJpZiIgLz4=
    using (new If((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5IiAvPg==") < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"))))
    {
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxNDEiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik1lYW5Bbm9tYWx5IiAvPjwvU2V0VmFyaWFibGU+
        MeanAnomaly = (((Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9Ik1lYW5Bbm9tYWx5Kk1lYW5Bbmd1bGFyTW90aW9uIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==") + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUaW1lIiAvPg==")) % Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGVyaW9kIiAvPg==")) / Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTWVhbkFuZ3VsYXJNb3Rpb24iIC8+"));
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxNDIiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkVjY2VudHJpY0Fub21hbHkiIC8+PC9TZXRWYXJpYWJsZT4=
        EccentricAnomaly = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==");
        // VZEL PFJlcGVhdCBpZD0iMTQzIiBzdHlsZT0icmVwZWF0IiAvPg==
        using (new Repeat(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIwIiAvPg==")))
        {
            // VZEL PFNldFZhcmlhYmxlIGlkPSIxNDQiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IktlcGxlciIgLz48L1NldFZhcmlhYmxlPg==
            Kepler = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IihFY2NlbnRyaWNBbm9tYWx5LShFY2NlbnRyaWNpdHkqc2luKEVjY2VudHJpY0Fub21hbHkpKSktTWVhbkFub21hbHkiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+");
            // VZEL PFNldFZhcmlhYmxlIGlkPSIxNDUiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkVjY2VudHJpY0Fub21hbHkiIC8+PC9TZXRWYXJpYWJsZT4=
            EccentricAnomaly = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkVjY2VudHJpY0Fub21hbHktKEtlcGxlci8oMS0oRWNjZW50cmljaXR5KmNvcyhFY2NlbnRyaWNBbm9tYWx5KSkpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=");
            // VZEL PElmIGlkPSIxNDYiIHN0eWxlPSJpZiIgLz4=
            using (new If((Vz.Abs(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iS2VwbGVyIiAvPg==")) <= Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuMDAwMDAwMDAwMDAwMDEiIC8+"))))
            {
                // VZEL PEJyZWFrIGlkPSIxNDciIHN0eWxlPSJicmVhayIgLz4=
                Vz.Break();
            }
        }
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxNDgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRydWVBbm9tYWx5IiAvPjwvU2V0VmFyaWFibGU+
        TrueAnomaly = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqYXRhbihzcXJ0KCgxK0VjY2VudHJpY2l0eSkvKDEtRWNjZW50cmljaXR5KSkqKHRhbihFY2NlbnRyaWNBbm9tYWx5LzIpKSkiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+");
    }
    // VZEL PEVsc2VJZiBpZD0iMTQ5IiBzdHlsZT0iZWxzZSIgLz4=
    using (new ElseIf(Vz.RawConstant("PENvbnN0YW50IGJvb2w9InRydWUiIC8+")))
    {
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxNTAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik1lYW5Bbm9tYWx5IiAvPjwvU2V0VmFyaWFibGU+
        MeanAnomaly = ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTWVhbkFub21hbHkiIC8+") + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUaW1lIiAvPg==")) / Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTWVhbkFuZ3VsYXJNb3Rpb24iIC8+"));
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxNTEiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ikh5cGVyYm9saWNBbm9tYWx5IiAvPjwvU2V0VmFyaWFibGU+
        HyperbolicAnomaly = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==");
        // VZEL PFJlcGVhdCBpZD0iMTUyIiBzdHlsZT0icmVwZWF0IiAvPg==
        using (new Repeat(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIwIiAvPg==")))
        {
            // VZEL PFNldFZhcmlhYmxlIGlkPSIxNTMiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IktlcGxlciIgLz48L1NldFZhcmlhYmxlPg==
            Kepler = ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSHlwZXJib2xpY0Fub21hbHkiIC8+") - (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5IiAvPg==") * sinh(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSHlwZXJib2xpY0Fub21hbHkiIC8+")))) + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTWVhbkFub21hbHkiIC8+"));
            // VZEL PFNldFZhcmlhYmxlIGlkPSIxNTQiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ikh5cGVyYm9saWNBbm9tYWx5IiAvPjwvU2V0VmFyaWFibGU+
            HyperbolicAnomaly = (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSHlwZXJib2xpY0Fub21hbHkiIC8+") - (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iS2VwbGVyIiAvPg==") / (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+") - (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5IiAvPg==") * cosh(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSHlwZXJib2xpY0Fub21hbHkiIC8+"))))));
            // VZEL PElmIGlkPSIxNTUiIHN0eWxlPSJpZiIgLz4=
            using (new If((Vz.Abs(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iS2VwbGVyIiAvPg==")) <= Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuMDAwMDAwMDAwMDAwMDEiIC8+"))))
            {
                // VZEL PEJyZWFrIGlkPSIxNTYiIHN0eWxlPSJicmVhayIgLz4=
                Vz.Break();
            }
        }
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxNTciIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRydWVBbm9tYWx5IiAvPjwvU2V0VmFyaWFibGU+
        TrueAnomaly = (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+") * Vz.Atan((Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InNxcnQoKEVjY2VudHJpY2l0eSsxKS8oRWNjZW50cmljaXR5LTEpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=") * tanh(Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9Ikh5cGVyYm9saWNBbm9tYWx5LzIiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+")))));
    }
});

// VZTOPBLOCK
// VZBLOCK PEV2ZW50IGV2ZW50PSJGbGlnaHRTdGFydCIgaWQ9IjE1OCIgc3R5bGU9ImZsaWdodC1zdGFydCIgcG9zPSItMTI5Ni42OSwtMjEzLjk4MTgiIC8+
// VZEL PEV2ZW50IGV2ZW50PSJGbGlnaHRTdGFydCIgaWQ9IjE1OCIgc3R5bGU9ImZsaWdodC1zdGFydCIgcG9zPSItMTI5Ni42OSwtMjEzLjk4MTgiIC8+
using (new OnStart())
{
    // VZEL PFNldFZhcmlhYmxlIGlkPSIxNTkiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlhheGlzIiAvPjwvU2V0VmFyaWFibGU+
    Xaxis = Vz.Vec(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIxNjAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IllheGlzIiAvPjwvU2V0VmFyaWFibGU+
    Yaxis = Vz.Vec(0, Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    // VZEL PEFjdGl2YXRlU3RhZ2UgaWQ9IjE2MSIgc3R5bGU9ImFjdGl2YXRlLXN0YWdlIiAvPg==
    Vz.ActivateStage();
    // VZEL PFdhaXRTZWNvbmRzIGlkPSIxNjIiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIxNjMiIHN0eWxlPSJsaXN0LWluaXQiPjxWYXJpYWJsZSBsaXN0PSJ0cnVlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGFyZ2V0cyIgLz48L1NldFZhcmlhYmxlPg==
    Targets = Vz.CreateListRaw("DSC Launch Pad,Droo Space Center,DSC Landing Pad A,DSC Landing Pad B,DSC Launch Pad Large,Drone Ship,Droo Desert Base");
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJQbGFuZXRMaXN0cyIgaWQ9IjE2NCIgc3R5bGU9ImNhbGwtY3VzdG9tLWluc3RydWN0aW9uIiAvPg==
    PlanetLists();
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJQbGFuZXQiIGlkPSIxNjUiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Planet("T.T.", 0);
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMTY2IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQYXJlbnRzIiAvPg=="), Vz.ListIndex(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), "T.T.")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0UG9zaXRpb24iIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0VmVsb2NpdHkiIC8+"));
    // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjE2NyIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
    Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRQYXJhbWV0ZXJzIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSW5jbGluYXRpb24iIC8+"));
    // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjE2OCIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
    Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRQYXJhbWV0ZXJzIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUmlnaHRBc2NlbnNpb24iIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMTY5IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Time_to_TrueAnomaly(Signed_Angle(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTm9kZSIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5VmVjdG9yIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPg==")));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIxNzAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
    Time = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+");
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMTcxIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQYXJlbnRzIiAvPg=="), Vz.ListIndex(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), "T.T.")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0UG9zaXRpb24iIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0VmVsb2NpdHkiIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMTcyIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Time_to_TrueAnomaly(Signed_Angle((0 - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTm9kZSIgLz4=")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5VmVjdG9yIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPg==")));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJMYXVuY2ggSW5jbGluYXRpb24iIGlkPSIxNzMiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Launch_Inclination(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRQYXJhbWV0ZXJzIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")), Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRQYXJhbWV0ZXJzIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+")), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwMDAwMCIgLz4="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjUwNDAwIiAvPg=="), Vz.Min(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+")));
    // VZEL PEFjdGl2YXRlU3RhZ2UgaWQ9IjE3NCIgc3R5bGU9ImFjdGl2YXRlLXN0YWdlIiAvPg==
    Vz.ActivateStage();
    // VZEL PFdhaXRTZWNvbmRzIGlkPSIxNzUiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
    // VZEL PEFjdGl2YXRlU3RhZ2UgaWQ9IjE3NiIgc3R5bGU9ImFjdGl2YXRlLXN0YWdlIiAvPg==
    Vz.ActivateStage();
    // VZEL PFdhaXRTZWNvbmRzIGlkPSIxNzciIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJJbmNsaW5hdGlvbiIgaWQ9IjE3OCIgc3R5bGU9ImNhbGwtY3VzdG9tLWluc3RydWN0aW9uIiAvPg==
    Inclination(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRQYXJhbWV0ZXJzIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")), Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRQYXJhbWV0ZXJzIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+")));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJQbGFuZXQiIGlkPSIxNzkiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Planet("T.T.", Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIxODAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRBIiAvPjwvU2V0VmFyaWFibGU+
    TA = Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+");
    // VZEL PFJlcGVhdCBpZD0iMTgxIiBzdHlsZT0icmVwZWF0IiAvPg==
    using (new Repeat(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwIiAvPg==")))
    {
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMTgyIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSIxODMiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
        Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMTg0IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Time_to_TrueAnomaly(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxODUiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlByb2dyYWRlIiAvPjwvU2V0VmFyaWFibGU+
        Prograde = Orbital_Speed(Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+")), ((Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+")) + Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0UG9zaXRpb24iIC8+"))) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+")));
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxODYiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
        Time = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+");
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxODciIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlJhZGlhbCIgLz48L1NldFZhcmlhYmxlPg==
        Radial = (0 - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRmxpZ2h0UGF0aEFuZ2xlIiAvPg=="));
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMTg4IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+"), Rotate_Vector((Vz.Norm(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+")) * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUHJvZ3JhZGUiIC8+")), Vz.Cross(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUmFkaWFsIiAvPg==")));
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMTg5IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Time_to_TrueAnomaly(Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg=="));
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSIxOTAiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
        Orbital_State_Vectors(Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg=="));
        // VZEL PFNldFZhcmlhYmxlIGlkPSIxOTEiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkNyYWZ0UG9zaXRpb24iIC8+PC9TZXRWYXJpYWJsZT4=
        CraftPosition = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+");
        // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIxOTIiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9DaGFuZ2VWYXJpYWJsZT4=
        Time += Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+");
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJQbGFuZXQiIGlkPSIxOTMiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
        Planet("T.T.", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="));
        // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIxOTQiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRBIiAvPjwvQ2hhbmdlVmFyaWFibGU+
        TA += Signed_Angle(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0UG9zaXRpb24iIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPg=="));
        TA = ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+") + Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=")) % Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4="));
    }
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJNYW5ldXZlciBOb2RlIiBpZD0iMTk1IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Maneuver_Node("Transfer", 0, Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUHJvZ3JhZGUiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUmFkaWFsIiAvPg=="), 0);
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMTk2IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMTk3IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Time_to_TrueAnomaly(Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg=="));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIxOTgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
    Time = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSIxOTkiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkkiIC8+PC9TZXRWYXJpYWJsZT4=
    I = (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
    // VZEL PFdoaWxlIGlkPSIyMDAiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=") > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuMDEiIC8+"))))
    {
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJDcmFmdCIgaWQ9IjIwMSIgc3R5bGU9ImNhbGwtY3VzdG9tLWluc3RydWN0aW9uIiAvPg==
        Craft(Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital(), Vz.Craft.Orbit.Planet(), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="));
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJQbGFuZXQiIGlkPSIyMDIiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
        Planet("T.T.", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="));
        // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIyMDMiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9DaGFuZ2VWYXJpYWJsZT4=
        Time += ((Vz.Distance(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0UG9zaXRpb24iIC8+")) < Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRTT0kiIC8+"), Vz.ListIndex(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), "T.T."))) ? (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=")) : Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4="));
        // VZEL PFNldFZhcmlhYmxlIGlkPSIyMDQiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkkiIC8+PC9TZXRWYXJpYWJsZT4=
        I = (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=") / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
    }
    // VZEL PExvZ01lc3NhZ2UgaWQ9IjIwNSIgc3R5bGU9ImxvZyIgLz4=
    Vz.Log(Vz.Join("T.T. encounter: ", Vz.StringOp("friendly", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="), ""), ""));
    // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIyMDYiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9DaGFuZ2VWYXJpYWJsZT4=
    Time += Vz.Time.TotalTime();
    // VZEL PEJyb2FkY2FzdE1lc3NhZ2UgZ2xvYmFsPSJmYWxzZSIgbG9jYWw9InRydWUiIGlkPSIyMDciIHN0eWxlPSJicm9hZGNhc3QtbXNnIiAvPg==
    Vz.Broadcast(BroadCastType.Local, "Timewarp", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="));
    // VZEL PFdoaWxlIGlkPSIyMDgiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") - Vz.Time.TotalTime()) > 0)))
    {
        // VZEL PERpc3BsYXlNZXNzYWdlIGlkPSIyMDkiIHN0eWxlPSJkaXNwbGF5IiAvPg==
        Vz.Display(Vz.Join("T.T. encounter:<br>", Clock((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") - Vz.Time.TotalTime())), ""), 7);
        // VZEL PFdhaXRTZWNvbmRzIGlkPSIyMTAiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    }
    // VZEL PFdhaXRVbnRpbCBpZD0iMjExIiBzdHlsZT0id2FpdC11bnRpbCIgLz4=
    using (new WaitUntil(Match_String(Vz.Craft.Orbit.Planet(), "T.T."))) { }
    // VZEL PERpc3BsYXlNZXNzYWdlIGlkPSIyMTIiIHN0eWxlPSJkaXNwbGF5IiAvPg==
    Vz.Display("", 7);
    // VZEL PFdhaXRTZWNvbmRzIGlkPSIyMTMiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
    // VZEL PEFjdGl2YXRlU3RhZ2UgaWQ9IjIxNCIgc3R5bGU9ImFjdGl2YXRlLXN0YWdlIiAvPg==
    Vz.ActivateStage();
    // VZEL PFdhaXRTZWNvbmRzIGlkPSIyMTUiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMjE2IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    // VZEL PFNldFZhcmlhYmxlIGlkPSIyMTciIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRBIiAvPjwvU2V0VmFyaWFibGU+
    TA = (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVHJ1ZUFub21hbHkiIC8+") + Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuMDAxIiAvPg=="));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSIyMTgiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIyMTkiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkNyYWZ0UG9zaXRpb24iIC8+PC9TZXRWYXJpYWJsZT4=
    CraftPosition = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSIyMjAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkNyYWZ0VmVsb2NpdHkiIC8+PC9TZXRWYXJpYWJsZT4=
    CraftVelocity = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSIyMjEiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlJhZGlhbCIgLz48L1NldFZhcmlhYmxlPg==
    Radial = Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSIyMjIiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkkiIC8+PC9TZXRWYXJpYWJsZT4=
    I = ((Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IihwaSkvMiIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=") + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRmxpZ2h0UGF0aEFuZ2xlIiAvPg==")) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
    // VZEL PFdoaWxlIGlkPSIyMjMiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=") > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuMDAwMDAwMDAwMDAwMDEiIC8+"))))
    {
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMjI0IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4="), Rotate_Vector(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRWZWxvY2l0eSIgLz4="), Vz.Cross(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRWZWxvY2l0eSIgLz4=")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUmFkaWFsIiAvPg==")));
        // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIyMjUiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlJhZGlhbCIgLz48L0NoYW5nZVZhcmlhYmxlPg==
        Radial += ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGVyaWFwc2lzIiAvPg==") > (Vz.Planet("T.T.").Radius() + Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIwMDAwIiAvPg=="))) ? (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=")) : Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4="));
        // VZEL PFNldFZhcmlhYmxlIGlkPSIyMjYiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkkiIC8+PC9TZXRWYXJpYWJsZT4=
        I = (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=") / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
    }
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMjI3IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRWZWxvY2l0eSIgLz4="));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIyMjgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik5vcm1hbCIgLz48L1NldFZhcmlhYmxlPg==
    Normal = Signed_Angle(Flatten(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5VmVjdG9yIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4=")), Vz.Cross(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iWWF4aXMiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4=")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4="));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJNYW5ldXZlciBOb2RlIiBpZD0iMjI5IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Maneuver_Node("Trajectory Correction", 0, Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"), Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRWZWxvY2l0eSIgLz4=")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUmFkaWFsIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTm9ybWFsIiAvPg=="));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMjMwIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSIyMzEiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_State_Vectors(0);
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJNYW5ldXZlciBOb2RlIiBpZD0iMjMyIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Maneuver_Node("Capture", 0, Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"), Orbital_Speed(Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+")), Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+"))), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"), ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+").y > 0) ? Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IihwaSktSW5jbGluYXRpb24iIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+") : (0 - Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IihwaSktSW5jbGluYXRpb24iIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+"))));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJDaXJjdWxhcml6YXRpb24gYXQgQWx0aXR1ZGUiIGlkPSIyMzMiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Circularization_at_Altitude(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIwMDAwIiAvPg=="));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIyMzQiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkxhbmRpbmdDb29yZGluYXRlcyIgLz48L1NldFZhcmlhYmxlPg==
    LandingCoordinates = Vz.Vec(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjUuMDYyNTU1MzE0IiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjExMS44MzA5MjYzMDQiIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjMiIC8+"));
    // VZEL PElmIGlkPSIyMzUiIHN0eWxlPSJpZiIgLz4=
    using (new If((Vz.Dot(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGFuZGluZ0Nvb3JkaW5hdGVzIiAvPg==")), Vz.Craft.Nav.Position()) > 0)))
    {
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJJbmNsaW5hdGlvbiIgaWQ9IjIzNiIgc3R5bGU9ImNhbGwtY3VzdG9tLWluc3RydWN0aW9uIiAvPg==
        Inclination(Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    }
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJEZW9yYml0IENvb3JkaW5hdGVzIiBpZD0iMjM3IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Deorbit_Coordinates(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGFuZGluZ0Nvb3JkaW5hdGVzIiAvPg=="), Vz.Deg2Rad(Vz.RawConstant("PENvbnN0YW50IHRleHQ9Ii00NSIgLz4=")), Vz.RawConstant("PENvbnN0YW50IHRleHQ9Ii0xMDgwMCIgLz4="));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMjM4IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    // VZEL PFNldFZhcmlhYmxlIGlkPSIyMzkiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRBIiAvPjwvU2V0VmFyaWFibGU+
    TA = (0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGFuZGluZ0Nvb3JkaW5hdGVzIiAvPg==")))));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSIyNDAiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMjQxIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Time_to_TrueAnomaly(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIyNDIiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
    Time = ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+") - Burn_Time(Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+")))) + Vz.Time.TotalTime());
    // VZEL PEJyb2FkY2FzdE1lc3NhZ2UgZ2xvYmFsPSJmYWxzZSIgbG9jYWw9InRydWUiIGlkPSIyNDMiIHN0eWxlPSJicm9hZGNhc3QtbXNnIiAvPg==
    Vz.Broadcast(BroadCastType.Local, "Timewarp", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="));
    // VZEL PFdoaWxlIGlkPSIyNDQiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") - Vz.Time.TotalTime()) > 0)))
    {
        // VZEL PElmIGlkPSIyNDUiIHN0eWxlPSJpZiIgLz4=
        using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") - Vz.Time.TotalTime()) < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"))))
        {
            // VZEL PFNldFRpbWVNb2RlIG1vZGU9Ik5vcm1hbCIgaWQ9IjI0NiIgc3R5bGU9InNldC10aW1lLW1vZGUiIC8+
            Vz.SetTimeModeAttr("Normal");
        }
        // VZEL PEVsc2VJZiBpZD0iMjQ3IiBzdHlsZT0iZWxzZS1pZiIgLz4=
        using (new ElseIf(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") - Vz.Time.TotalTime()) < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjYwIiAvPg=="))))
        {
            // VZEL PFNldEFjdGl2YXRpb25Hcm91cCBpZD0iMjQ4IiBzdHlsZT0ic2V0LWFnIiAvPg==
            Vz.SetActivationGroup(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjciIC8+"), Vz.RawConstant("PENvbnN0YW50IHN0eWxlPSJ0cnVlIiBib29sPSJ0cnVlIiAvPg=="));
            // VZEL PFNldFRpbWVNb2RlIG1vZGU9IkZhc3RGb3J3YXJkIiBpZD0iMjQ5IiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
            Vz.SetTimeModeAttr("FastForward");
        }
        // VZEL PExvY2tOYXZTcGhlcmUgaW5kaWNhdG9yVHlwZT0iVmVjdG9yIiBpZD0iMjUwIiBzdHlsZT0ibG9jay1uYXYtc3BoZXJlLXZlY3RvciIgLz4=
        Vz.LockNavSphere(LockNavSphereIndicatorType.Vector, (0 - Vz.Craft.Velocity.Surface()));
        // VZEL PERpc3BsYXlNZXNzYWdlIGlkPSIyNTEiIHN0eWxlPSJkaXNwbGF5IiAvPg==
        Vz.Display(Vz.Join("Landing Burn:<br>", Clock((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") - Vz.Time.TotalTime())), ""), 7);
        // VZEL PFdhaXRTZWNvbmRzIGlkPSIyNTIiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    }
    // VZEL PFdoaWxlIGlkPSIyNTMiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While((!Vz.Craft.Grounded())))
    {
        // VZEL PFNldFZhcmlhYmxlIGlkPSIyNTQiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlBsYW5ldFBvc2l0aW9uIiAvPjwvU2V0VmFyaWFibGU+
        PlanetPosition = Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGFuZGluZ0Nvb3JkaW5hdGVzIiAvPg=="));
        // VZEL PFNldFZhcmlhYmxlIGlkPSIyNTUiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlZlY3RvciIgLz48L1NldFZhcmlhYmxlPg==
        Vector = (((Vz.Norm((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0UG9zaXRpb24iIC8+") - Vz.Craft.Nav.Position())) * ((Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwIiAvPg==") * (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEuMDA4IiAvPg==") ^ Vz.Length((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0UG9zaXRpb24iIC8+") - Vz.Craft.Nav.Position())))) - Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwIiAvPg=="))) - Vz.Craft.Velocity.Gravity()) - Vz.Craft.Velocity.Surface());
        // VZEL PExvY2tOYXZTcGhlcmUgaW5kaWNhdG9yVHlwZT0iVmVjdG9yIiBpZD0iMjU2IiBzdHlsZT0ibG9jay1uYXYtc3BoZXJlLXZlY3RvciIgLz4=
        Vz.LockNavSphere(LockNavSphereIndicatorType.Vector, Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVjdG9yIiAvPg=="));
        // VZEL PFNldElucHV0IGlucHV0PSJ0aHJvdHRsZSIgaWQ9IjI1NyIgc3R5bGU9InNldC1pbnB1dCIgLz4=
        Vz.SetInput(CraftInput.Throttle, (Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVjdG9yIiAvPg==")) / (Vz.Craft.Performance.MaxThrust() / Vz.Craft.Performance.Mass())));
        // VZEL PERpc3BsYXlNZXNzYWdlIGlkPSIyNTgiIHN0eWxlPSJkaXNwbGF5IiAvPg==
        Vz.Display(Vz.Join("Distance to Landing Coordinates: ", Vz.StringOp("friendly", Vz.Length((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0UG9zaXRpb24iIC8+") - Vz.Craft.Nav.Position())), ""), ""), 7);
        // VZEL PElmIGlkPSIyNTkiIHN0eWxlPSJpZiIgLz4=
        using (new If((Vz.Craft.AltitudeAGL() < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwIiAvPg=="))))
        {
            // VZEL PFNldEFjdGl2YXRpb25Hcm91cCBpZD0iMjYwIiBzdHlsZT0ic2V0LWFnIiAvPg==
            Vz.SetActivationGroup(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjgiIC8+"), Vz.RawConstant("PENvbnN0YW50IHN0eWxlPSJ0cnVlIiBib29sPSJ0cnVlIiAvPg=="));
        }
        // VZEL PFdhaXRTZWNvbmRzIGlkPSIyNjEiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    }
    // VZEL PFNldElucHV0IGlucHV0PSJ0aHJvdHRsZSIgaWQ9IjI2MiIgc3R5bGU9InNldC1pbnB1dCIgLz4=
    Vz.SetInput(CraftInput.Throttle, Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    // VZEL PERpc3BsYXlNZXNzYWdlIGlkPSIyNjMiIHN0eWxlPSJkaXNwbGF5IiAvPg==
    Vz.Display("Touchdown!", 7);
    // VZEL PExvZ01lc3NhZ2UgaWQ9IjI2NCIgc3R5bGU9ImxvZyIgLz4=
    Vz.Log("Touchdown!");
    // VZEL PFdhaXRTZWNvbmRzIGlkPSIyNjUiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjQiIC8+"));
    // VZEL PFNldEFjdGl2YXRpb25Hcm91cCBpZD0iMjY2IiBzdHlsZT0ic2V0LWFnIiAvPg==
    Vz.SetActivationGroup(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjciIC8+"), Vz.RawConstant("PENvbnN0YW50IHN0eWxlPSJmYWxzZSIgYm9vbD0iZmFsc2UiIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJQbGFuZXQiIGlkPSIyNjciIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Planet("T.T.", 0);
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMjY4IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQYXJlbnRzIiAvPg=="), Vz.ListIndex(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), "T.T.")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0UG9zaXRpb24iIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0VmVsb2NpdHkiIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMjY5IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Time_to_TrueAnomaly(Signed_Angle(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTm9kZSIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5VmVjdG9yIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPg==")));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIyNzAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
    Time = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+");
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMjcxIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQYXJlbnRzIiAvPg=="), Vz.ListIndex(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), "T.T.")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0UG9zaXRpb24iIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0VmVsb2NpdHkiIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMjcyIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Time_to_TrueAnomaly(Signed_Angle((Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTm9kZSIgLz4=")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5VmVjdG9yIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPg==")));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJMYXVuY2ggSW5jbGluYXRpb24iIGlkPSIyNzMiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Launch_Inclination(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRQYXJhbWV0ZXJzIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")), Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRQYXJhbWV0ZXJzIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+")), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIwMDAwIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9Ii0xMDgwMCIgLz4="), Vz.Min(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+")));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJJbmNsaW5hdGlvbiIgaWQ9IjI3NCIgc3R5bGU9ImNhbGwtY3VzdG9tLWluc3RydWN0aW9uIiAvPg==
    Inclination(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRQYXJhbWV0ZXJzIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")), Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRQYXJhbWV0ZXJzIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+")));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIyNzUiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
    Time = 0;
    // VZEL PFNldFZhcmlhYmxlIGlkPSIyNzYiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlByb2dyYWRlIiAvPjwvU2V0VmFyaWFibGU+
    Prograde = Vz.Length(Vz.Craft.Velocity.Orbital());
    // VZEL PFNldFZhcmlhYmxlIGlkPSIyNzciIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkkiIC8+PC9TZXRWYXJpYWJsZT4=
    I = Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIwMCIgLz4=");
    // VZEL PFdoaWxlIGlkPSIyNzgiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=") > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuMDEiIC8+"))))
    {
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMjc5IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), (Vz.Norm(Vz.Craft.Velocity.Orbital()) * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUHJvZ3JhZGUiIC8+")));
        // VZEL PFNldFZhcmlhYmxlIGlkPSIyODAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ikh5cGVyYm9saWNBbmdsZSIgLz48L1NldFZhcmlhYmxlPg==
        HyperbolicAngle = ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5IiAvPg==") > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")) ? Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IihwaSktYWNvcygtMS9FY2NlbnRyaWNpdHkpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==") : 0);
        // VZEL PFJlcGVhdCBpZD0iMjgxIiBzdHlsZT0icmVwZWF0IiAvPg==
        using (new Repeat(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjUiIC8+")))
        {
            // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJQbGFuZXQiIGlkPSIyODIiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
            Planet("T.T.", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="));
            // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMjgzIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
            Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
            // VZEL PFNldFZhcmlhYmxlIGlkPSIyODQiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRBIiAvPjwvU2V0VmFyaWFibGU+
            TA = ((Signed_Angle(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0VmVsb2NpdHkiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5VmVjdG9yIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPg==")) + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSHlwZXJib2xpY0FuZ2xlIiAvPg==") + Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=")) % Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4="));
            // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMjg1IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
            Time_to_TrueAnomaly(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
            // VZEL PFNldFZhcmlhYmxlIGlkPSIyODYiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
            Time = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+");
        }
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSIyODciIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
        Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMjg4IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+"), (Vz.Norm(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+")) * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUHJvZ3JhZGUiIC8+")));
        // VZEL PElmIGlkPSIyODkiIHN0eWxlPSJpZiIgLz4=
        using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQXBvYXBzaXMiIC8+") < Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU09JIiAvPg==")) && (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQXBvYXBzaXMiIC8+") > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+")))))
        {
            // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIyOTAiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlByb2dyYWRlIiAvPjwvQ2hhbmdlVmFyaWFibGU+
            Prograde += Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=");
        }
        // VZEL PEVsc2VJZiBpZD0iMjkxIiBzdHlsZT0iZWxzZSIgLz4=
        using (new ElseIf(Vz.RawConstant("PENvbnN0YW50IGJvb2w9InRydWUiIC8+")))
        {
            // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMjkyIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
            Time_to_TrueAnomaly(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU09JRXhpdFRydWVBbm9tYWx5IiAvPg=="));
            // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIyOTMiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9DaGFuZ2VWYXJpYWJsZT4=
            Time += Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+");
            // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSIyOTQiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
            Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU09JRXhpdFRydWVBbm9tYWx5IiAvPg=="));
            // VZEL PFNldFZhcmlhYmxlIGlkPSIyOTUiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkNyYWZ0UG9zaXRpb24iIC8+PC9TZXRWYXJpYWJsZT4=
            CraftPosition = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+");
            // VZEL PFNldFZhcmlhYmxlIGlkPSIyOTYiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkNyYWZ0VmVsb2NpdHkiIC8+PC9TZXRWYXJpYWJsZT4=
            CraftVelocity = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+");
            // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJQbGFuZXQiIGlkPSIyOTciIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
            Planet("T.T.", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="));
            // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMjk4IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
            Orbital_Elements_Parent_Body("Droo", (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0UG9zaXRpb24iIC8+") + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4=")), (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGxhbmV0VmVsb2NpdHkiIC8+") + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRWZWxvY2l0eSIgLz4=")));
            // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIyOTkiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlByb2dyYWRlIiAvPjwvQ2hhbmdlVmFyaWFibGU+
            Prograde += ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGVyaWFwc2lzIiAvPg==") < (Vz.Planet("Droo").Radius() + Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjMwMDAwIiAvPg=="))) ? (0 - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=")) : Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4="));
        }
        // VZEL PFNldFZhcmlhYmxlIGlkPSIzMDAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkkiIC8+PC9TZXRWYXJpYWJsZT4=
        I = (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=") / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
    }
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJNYW5ldXZlciBOb2RlIiBpZD0iMzAxIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Maneuver_Node("Escape", 0, Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUHJvZ3JhZGUiIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"), 0);
    // VZEL PFdhaXRTZWNvbmRzIGlkPSIzMDIiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMzAzIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMzA0IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Time_to_TrueAnomaly(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU09JRXhpdFRydWVBbm9tYWx5IiAvPg=="));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIzMDUiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
    Time = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+");
    // VZEL PExvZ01lc3NhZ2UgaWQ9IjMwNiIgc3R5bGU9ImxvZyIgLz4=
    Vz.Log(Vz.Join("Droo encounter: ", Vz.StringOp("friendly", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="), ""), ""));
    // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIzMDciIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9DaGFuZ2VWYXJpYWJsZT4=
    Time += Vz.Time.TotalTime();
    // VZEL PEJyb2FkY2FzdE1lc3NhZ2UgZ2xvYmFsPSJmYWxzZSIgbG9jYWw9InRydWUiIGlkPSIzMDgiIHN0eWxlPSJicm9hZGNhc3QtbXNnIiAvPg==
    Vz.Broadcast(BroadCastType.Local, "Timewarp", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="));
    // VZEL PFdoaWxlIGlkPSIzMDkiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") - Vz.Time.TotalTime()) > 0)))
    {
        // VZEL PERpc3BsYXlNZXNzYWdlIGlkPSIzMTAiIHN0eWxlPSJkaXNwbGF5IiAvPg==
        Vz.Display(Vz.Join("Droo encounter:<br>", Clock((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") - Vz.Time.TotalTime())), ""), 7);
        // VZEL PElmIGlkPSIzMTEiIHN0eWxlPSJpZiIgLz4=
        using (new If(Match_String(Vz.Craft.Orbit.Planet(), "Droo")))
        {
            // VZEL PERpc3BsYXlNZXNzYWdlIGlkPSIzMTIiIHN0eWxlPSJkaXNwbGF5IiAvPg==
            Vz.Display("", 7);
            // VZEL PEJyZWFrIGlkPSIzMTMiIHN0eWxlPSJicmVhayIgLz4=
            Vz.Break();
        }
        // VZEL PFdhaXRTZWNvbmRzIGlkPSIzMTQiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    }
    // VZEL PFdhaXRVbnRpbCBpZD0iMzE1IiBzdHlsZT0id2FpdC11bnRpbCIgLz4=
    using (new WaitUntil(Match_String(Vz.Craft.Orbit.Planet(), "Droo"))) { }
    // VZEL PFdoaWxlIGlkPSIzMTYiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") - Vz.Time.TotalTime()) > 0)))
    {
        // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwMiIgaWQ9IjMxNyIgc3R5bGU9InNldC10aW1lLW1vZGUiIC8+
        Vz.SetTimeModeAttr("TimeWarp2");
    }
    // VZEL PFNldFRpbWVNb2RlIG1vZGU9Ik5vcm1hbCIgaWQ9IjMxOCIgc3R5bGU9InNldC10aW1lLW1vZGUiIC8+
    Vz.SetTimeModeAttr("Normal");
    // VZEL PFdhaXRTZWNvbmRzIGlkPSIzMTkiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMzIwIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    // VZEL PFNldFZhcmlhYmxlIGlkPSIzMjEiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRBIiAvPjwvU2V0VmFyaWFibGU+
    TA = (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVHJ1ZUFub21hbHkiIC8+") + Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuMDAwMSIgLz4="));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSIzMjIiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJNYW5ldXZlciBOb2RlIiBpZD0iMzIzIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Maneuver_Node("Correction", 0, Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"), Orbital_Speed(Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+")), ((Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+")) + (Vz.Planet("Droo").Radius() + Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjcwMDAwIiAvPg=="))) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"))), (0 - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRmxpZ2h0UGF0aEFuZ2xlIiAvPg==")), ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+").y > 0) ? Signed_Angle(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iWWF4aXMiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+")) : Signed_Angle(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+"), (0 - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iWWF4aXMiIC8+")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+"))));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMzI0IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSIzMjUiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_State_Vectors(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJNYW5ldXZlciBOb2RlIiBpZD0iMzI2IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Maneuver_Node("Correction", 0, Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"), Orbital_Speed(Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+")), ((Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+")) + (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQXBvYXBzaXMiIC8+") - (Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRTT0kiIC8+"), Vz.ListIndex(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJQbGFuZXROYW1lcyIgLz4="), "T.T.")) * Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEuNSIgLz4=")))) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"))), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"), 0);
    // VZEL PFdhaXRTZWNvbmRzIGlkPSIzMjciIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuNSIgLz4="));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIzMjgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkkiIC8+PC9TZXRWYXJpYWJsZT4=
    I = "";
    // VZEL PEZvciB2YXI9ImkiIGlkPSIzMjkiIHN0eWxlPSJmb3IiPjxMaXN0T3Agb3A9Imxlbmd0aCIgc3R5bGU9Imxpc3QtbGVuZ3RoIj48VmFyaWFibGUgbGlzdD0idHJ1ZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRhcmdldHMiIC8+PC9MaXN0T3A+PENvbnN0YW50IHRleHQ9IjEiIC8+PENvbnN0YW50IHRleHQ9Ii0xIiAvPjwvRm9yPg==
    using (new For("i").From(Vz.ListLength(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRzIiAvPg=="))).To(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")).By(Vz.RawConstant("PENvbnN0YW50IHRleHQ9Ii0xIiAvPg==")))
    {
        // VZEL PExvZ01lc3NhZ2UgaWQ9IjMzMCIgc3R5bGU9ImxvZyIgLz4=
        Vz.Log(Vz.Join(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="), " ", Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRzIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg==")), " ", ""));
    }
    // VZEL PEZvciB2YXI9ImkiIGlkPSIzMzEiIHN0eWxlPSJmb3IiPjxDb25zdGFudCBudW1iZXI9IjEiIC8+PExpc3RPcCBvcD0ibGVuZ3RoIiBzdHlsZT0ibGlzdC1sZW5ndGgiPjxWYXJpYWJsZSBsaXN0PSJ0cnVlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGFyZ2V0cyIgLz48L0xpc3RPcD48Q29uc3RhbnQgbnVtYmVyPSIxIiAvPjwvRm9yPg==
    using (new For("i").From(1).To(Vz.ListLength(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRzIiAvPg=="))).By(1))
    {
        // VZEL PFNldFZhcmlhYmxlIGlkPSIzMzIiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkkiIC8+PC9TZXRWYXJpYWJsZT4=
        I = Vz.Join(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4="), "<br>", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="), " ", Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRzIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg==")), "");
    }
    // VZEL PERpc3BsYXlNZXNzYWdlIGlkPSIzMzMiIHN0eWxlPSJkaXNwbGF5IiAvPg==
    Vz.Display(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4="), 7);
    // VZEL PFVzZXJJbnB1dCBpZD0iMzM0IiBzdHlsZT0idXNlci1pbnB1dCIgLz4=
    I = Vz.UserInput(Vz.Join("Enter 1 - ", Vz.ListLength(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRzIiAvPg==")), " to choose a target location", ""));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIzMzUiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkkiIC8+PC9TZXRWYXJpYWJsZT4=
    I = Vz.Min(Vz.ListLength(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRzIiAvPg==")), Vz.Max(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"), Vz.Round(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4="))));
    // VZEL PFNldFRhcmdldCBpZD0iMzM2IiBzdHlsZT0ic2V0LXRhcmdldCIgLz4=
    Vz.TargetNode(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRzIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=")));
    // VZEL PFdhaXRTZWNvbmRzIGlkPSIzMzciIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuMjUiIC8+"));
    // VZEL PExvZ01lc3NhZ2UgaWQ9IjMzOCIgc3R5bGU9ImxvZyIgLz4=
    Vz.Log(Vz.Join("Commencing Orbital Drop onto ", Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJUYXJnZXRzIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=")), "", " ", ""));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIzMzkiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkxhbmRpbmdDb29yZGluYXRlcyIgLz48L1NldFZhcmlhYmxlPg==
    LandingCoordinates = Vz.PosToLatLongAgl(Vz.Craft.Target.Position());
    // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwMSIgaWQ9IjM0MCIgc3R5bGU9InNldC10aW1lLW1vZGUiIC8+
    Vz.SetTimeModeAttr("TimeWarp1");
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMzQxIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMzQyIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Time_to_TrueAnomaly(Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg=="));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIzNDMiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
    Time = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+");
    // VZEL PFNldExpc3Qgb3A9ImNsZWFyIiBpZD0iMzQ0IiBzdHlsZT0ibGlzdC1jbGVhciIgLz4=
    Vz.ListClear(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="));
    // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjM0NSIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
    Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGVyaW9kIiAvPg=="));
    // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjM0NiIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
    Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSIzNDciIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_State_Vectors(Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg=="));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIzNDgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkNyYWZ0UG9zaXRpb24iIC8+PC9TZXRWYXJpYWJsZT4=
    CraftPosition = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSIzNDkiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkNyYWZ0VmVsb2NpdHkiIC8+PC9TZXRWYXJpYWJsZT4=
    CraftVelocity = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+");
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMzUwIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4="), (Vz.Norm(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRWZWxvY2l0eSIgLz4=")) * Orbital_Speed(Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4=")), ((Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4=")) + (Vz.Planet("Droo").Radius() / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"))) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+")))));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMzUxIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Time_to_TrueAnomaly((0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGFuZGluZ0Nvb3JkaW5hdGVzIiAvPg=="))))));
    // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIzNTIiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9DaGFuZ2VWYXJpYWJsZT4=
    Time += Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSIzNTMiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik9yYml0IiAvPjwvU2V0VmFyaWFibGU+
    Orbit = 0;
    // VZEL PFdoaWxlIGlkPSIzNTQiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While((Vz.Dot(Vz.Norm(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4=")), Vz.Norm(Rotate_Y_Axis_Vector(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGFuZGluZ0Nvb3JkaW5hdGVzIiAvPg==")), (Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKS81MDQwMCIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=") * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="))))) < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuOCIgLz4="))))
    {
        // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIzNTUiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik9yYml0IiAvPjwvQ2hhbmdlVmFyaWFibGU+
        Orbit += Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+");
        // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIzNTYiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9DaGFuZ2VWYXJpYWJsZT4=
        Time += Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), 1);
    }
    // VZEL PFNldFZhcmlhYmxlIGlkPSIzNTciIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlByb2dyYWRlIiAvPjwvU2V0VmFyaWFibGU+
    Prograde = (Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRWZWxvY2l0eSIgLz4=")) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIzNTgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkkiIC8+PC9TZXRWYXJpYWJsZT4=
    I = (Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRWZWxvY2l0eSIgLz4=")) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjQiIC8+"));
    // VZEL PFdoaWxlIGlkPSIzNTkiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=") > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuMDAxIiAvPg=="))))
    {
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMzYwIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4="), (Vz.Norm(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRWZWxvY2l0eSIgLz4=")) * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUHJvZ3JhZGUiIC8+")));
        // VZEL PFNldFZhcmlhYmxlIGlkPSIzNjEiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRBIiAvPjwvU2V0VmFyaWFibGU+
        TA = (0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGFuZGluZ0Nvb3JkaW5hdGVzIiAvPg==")))));
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMzYyIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Time_to_TrueAnomaly(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSIzNjMiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
        Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
        // VZEL PElmIGlkPSIzNjQiIHN0eWxlPSJpZiIgLz4=
        using (new If((Vz.Dot(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+")) < 0)))
        {
            // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIzNjUiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlByb2dyYWRlIiAvPjwvQ2hhbmdlVmFyaWFibGU+
            Prograde += (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4="));
        }
        // VZEL PEVsc2VJZiBpZD0iMzY2IiBzdHlsZT0iZWxzZSIgLz4=
        using (new ElseIf(Vz.RawConstant("PENvbnN0YW50IGJvb2w9InRydWUiIC8+")))
        {
            // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIzNjciIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlByb2dyYWRlIiAvPjwvQ2hhbmdlVmFyaWFibGU+
            Prograde += ((Vz.Length(Flatten(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4="))) > Vz.Length(Flatten(Rotate_Y_Axis_Vector(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGFuZGluZ0Nvb3JkaW5hdGVzIiAvPg==")), (Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKS81MDQwMCIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=") * (((Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")) * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iT3JiaXQiIC8+")) + Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"))) + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+")))), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4=")))) ? (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=")) : Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4="));
        }
        // VZEL PFNldFZhcmlhYmxlIGlkPSIzNjgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkkiIC8+PC9TZXRWYXJpYWJsZT4=
        I = (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=") / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
    }
    // VZEL PFNldFZhcmlhYmxlIGlkPSIzNjkiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik5vcm1hbCIgLz48L1NldFZhcmlhYmxlPg==
    Normal = Signed_Angle(Flatten(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4=")), Flatten(Rotate_Y_Axis_Vector(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGFuZGluZ0Nvb3JkaW5hdGVzIiAvPg==")), (Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKS81MDQwMCIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=") * (((Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")) * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iT3JiaXQiIC8+")) + Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"))) + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+")))), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4=")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4="));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJNYW5ldXZlciBOb2RlIiBpZD0iMzcwIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Maneuver_Node("Orbital Drop", (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iT3JiaXQiIC8+") * Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), 1)), Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUHJvZ3JhZGUiIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTm9ybWFsIiAvPg=="));
    // VZEL PFNldFRpbWVNb2RlIG1vZGU9IlRpbWVXYXJwMSIgaWQ9IjM3MSIgc3R5bGU9InNldC10aW1lLW1vZGUiIC8+
    Vz.SetTimeModeAttr("TimeWarp1");
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMzcyIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMzczIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Time_to_TrueAnomaly((0 - True_Anomaly_at_Radius((Vz.Planet("Droo").Radius() + Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjU4MDAwIiAvPg==")))));
    // VZEL PFNldFZhcmlhYmxlIGlkPSIzNzQiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
    Time = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+");
    // VZEL PExvZ01lc3NhZ2UgaWQ9IjM3NSIgc3R5bGU9ImxvZyIgLz4=
    Vz.Log(Vz.Join("Droo Reentry: ", Vz.StringOp("friendly", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="), ""), ""));
    // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSIzNzYiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9DaGFuZ2VWYXJpYWJsZT4=
    Time += Vz.Time.TotalTime();
    // VZEL PEJyb2FkY2FzdE1lc3NhZ2UgZ2xvYmFsPSJmYWxzZSIgbG9jYWw9InRydWUiIGlkPSIzNzciIHN0eWxlPSJicm9hZGNhc3QtbXNnIiAvPg==
    Vz.Broadcast(BroadCastType.Local, "Timewarp", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="));
    // VZEL PFdoaWxlIGlkPSIzNzgiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") - Vz.Time.TotalTime()) > 0)))
    {
        // VZEL PERpc3BsYXlNZXNzYWdlIGlkPSIzNzkiIHN0eWxlPSJkaXNwbGF5IiAvPg==
        Vz.Display(Vz.Join("Reentry Time:<br>", Clock((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") - Vz.Time.TotalTime())), ""), 7);
        // VZEL PElmIGlkPSIzODAiIHN0eWxlPSJpZiIgLz4=
        using (new If(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") - Vz.Time.TotalTime()) < Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjYwIiAvPg=="))))
        {
            // VZEL PFNldFRpbWVNb2RlIG1vZGU9IkZhc3RGb3J3YXJkIiBpZD0iMzgxIiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
            Vz.SetTimeModeAttr("FastForward");
        }
        // VZEL PFdhaXRTZWNvbmRzIGlkPSIzODIiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    }
    // VZEL PEFjdGl2YXRlU3RhZ2UgaWQ9IjM4MyIgc3R5bGU9ImFjdGl2YXRlLXN0YWdlIiAvPg==
    Vz.ActivateStage();
    // VZEL PFdhaXRVbnRpbCBpZD0iMzg0IiBzdHlsZT0id2FpdC11bnRpbCIgLz4=
    using (new WaitUntil(Vz.Craft.Atmosphere.AirDensity())) { }
    // VZEL PFdoaWxlIGlkPSIzODUiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While((Vz.Length(Vz.Craft.Nav.Position()) > (Vz.Length(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGFuZGluZ0Nvb3JkaW5hdGVzIiAvPg=="))) + Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEwMCIgLz4=")))))
    {
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iMzg2IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
        // VZEL PFNldFZhcmlhYmxlIGlkPSIzODciIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRBIiAvPjwvU2V0VmFyaWFibGU+
        TA = (0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGFuZGluZ0Nvb3JkaW5hdGVzIiAvPg==")))));
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iMzg4IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Time_to_TrueAnomaly(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSIzODkiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
        Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
        // VZEL PFNldFZhcmlhYmxlIGlkPSIzOTAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlZlY3RvciIgLz48L1NldFZhcmlhYmxlPg==
        Vector = Rotate_Y_Axis_Vector(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGFuZGluZ0Nvb3JkaW5hdGVzIiAvPg==")), (Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKS81MDQwMCIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=") * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+")));
        // VZEL PExvY2tOYXZTcGhlcmUgaW5kaWNhdG9yVHlwZT0iVmVjdG9yIiBpZD0iMzkxIiBzdHlsZT0ibG9jay1uYXYtc3BoZXJlLXZlY3RvciIgLz4=
        Vz.LockNavSphere(LockNavSphereIndicatorType.Vector, ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVjdG9yIiAvPg==") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+")) - Vz.Project(Vz.Craft.Velocity.Surface(), Vz.Craft.Nav.Position())));
        // VZEL PFdhaXRTZWNvbmRzIGlkPSIzOTIiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    }
    // VZEL PFNldFRhcmdldEhlYWRpbmcgcHJvcGVydHk9InBpdGNoIiBpZD0iMzkzIiBzdHlsZT0ic2V0LWhlYWRpbmciIC8+
    Vz.SetTargetHeading(TargetHeadingProperty.Pitch, Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjkwIiAvPg=="));
    // VZEL PFNldFRpbWVNb2RlIG1vZGU9Ik5vcm1hbCIgaWQ9IjM5NCIgc3R5bGU9InNldC10aW1lLW1vZGUiIC8+
    Vz.SetTimeModeAttr("Normal");
    // VZEL PEFjdGl2YXRlU3RhZ2UgaWQ9IjM5NSIgc3R5bGU9ImFjdGl2YXRlLXN0YWdlIiAvPg==
    Vz.ActivateStage();
    // VZEL PFdhaXRVbnRpbCBpZD0iMzk2IiBzdHlsZT0id2FpdC11bnRpbCIgLz4=
    using (new WaitUntil(Vz.Craft.Grounded())) { }
    // VZEL PERpc3BsYXlNZXNzYWdlIGlkPSIzOTciIHN0eWxlPSJkaXNwbGF5IiAvPg==
    Vz.Display("Touchdown!", 7);
    // VZEL PExvZ01lc3NhZ2UgaWQ9IjM5OCIgc3R5bGU9ImxvZyIgLz4=
    Vz.Log("Touchdown!");
}

// VZTOPBLOCK
// VZBLOCK PEN1c3RvbUluc3RydWN0aW9uIGNhbGxGb3JtYXQ9IkluY2xpbmF0aW9uICgwKSBSaWdodCBBc2NlbnNpb24gKDEpICIgZm9ybWF0PSJJbmNsaW5hdGlvbiB8SW5jbGluYXRpb258IFJpZ2h0IEFzY2Vuc2lvbiB8UmlnaHRBc2NlbnNpb258ICIgbmFtZT0iSW5jbGluYXRpb24iIGlkPSIzOTkiIHN0eWxlPSJjdXN0b20taW5zdHJ1Y3Rpb24iIHBvcz0iMjg0OS40MTMsLTYxODAuMzMyIiAvPg==
var Inclination = Vz.DeclareCustomInstruction("Inclination", "Inclination", "RightAscension").SetInstructions((Inclination, RightAscension) =>
{
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0MDAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iSW5jbGluYXRpb24iIC8+PC9TZXRWYXJpYWJsZT4=
    Inclination = Rotate_Y_Axis_Vector(Rotate_X_Axis_Vector(Vz.Vec(0, Vz.RawConstant("PENvbnN0YW50IHRleHQ9Ii0xIiAvPg=="), 0), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJJbmNsaW5hdGlvbiIgLz4=")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJSaWdodEFzY2Vuc2lvbiIgLz4="));
    // VZEL PFNldExpc3Qgb3A9ImNsZWFyIiBpZD0iNDAxIiBzdHlsZT0ibGlzdC1jbGVhciIgLz4=
    Vz.ListClear(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="));
    // VZEL PEZvciB2YXI9ImkiIGlkPSI0MDIiIHN0eWxlPSJmb3IiPjxDb25zdGFudCBudW1iZXI9IjEiIC8+PENvbnN0YW50IHRleHQ9Ii0xIiAvPjxDb25zdGFudCB0ZXh0PSItMiIgLz48L0Zvcj4=
    using (new For("i").From(1).To(Vz.RawConstant("PENvbnN0YW50IHRleHQ9Ii0xIiAvPg==")).By(Vz.RawConstant("PENvbnN0YW50IHRleHQ9Ii0yIiAvPg==")))
    {
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iNDAzIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
        // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjQwNCIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
        Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), Signed_Angle((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg==") * Vz.Cross(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJJbmNsaW5hdGlvbiIgLz4="))), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5VmVjdG9yIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPg==")));
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iNDA1IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Time_to_TrueAnomaly(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg==") > 0) ? Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+") : Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjMiIC8+"))));
        // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjQwNiIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
        Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+"));
    }
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iNDA3IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0MDgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRBIiAvPjwvU2V0VmFyaWFibGU+
    TA = ((Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+")) < Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjQiIC8+"))) ? Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")) : Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjMiIC8+")));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSI0MDkiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0MTAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik5vcm1hbCIgLz48L1NldFZhcmlhYmxlPg==
    Normal = Signed_Angle(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJJbmNsaW5hdGlvbiIgLz4="), (((Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+")) < Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJDcmFmdFN0YXRlIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjQiIC8+"))) ? Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+") : Vz.RawConstant("PENvbnN0YW50IHRleHQ9Ii0xIiAvPg==")) * Vz.Cross(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJJbmNsaW5hdGlvbiIgLz4="))));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJNYW5ldXZlciBOb2RlIiBpZD0iNDExIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Maneuver_Node("Inclination", 0, Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"), Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+")), 0, Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTm9ybWFsIiAvPg=="));
});

// VZTOPBLOCK
// VZBLOCK PEN1c3RvbUluc3RydWN0aW9uIGNhbGxGb3JtYXQ9IkNpcmN1bGFyaXphdGlvbiBhdCBBbHRpdHVkZSAoMCkgIiBmb3JtYXQ9IkNpcmN1bGFyaXphdGlvbiBhdCBBbHRpdHVkZSB8QWx0aXR1ZGV8ICIgbmFtZT0iQ2lyY3VsYXJpemF0aW9uIGF0IEFsdGl0dWRlIiBpZD0iNDEyIiBzdHlsZT0iY3VzdG9tLWluc3RydWN0aW9uIiBwb3M9IjI4NDUuNzUzLC01ODg3LjgyNiIgLz4=
var Circularization_at_Altitude = Vz.DeclareCustomInstruction("Circularization at Altitude", "Altitude").SetInstructions((Altitude) =>
{
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iNDEzIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0MTQiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRBIiAvPjwvU2V0VmFyaWFibGU+
    TA = True_Anomaly_at_Radius((Vz.Planet(Vz.Craft.Orbit.Planet()).Radius() + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBbHRpdHVkZSIgLz4=")));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSI0MTUiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJNYW5ldXZlciBOb2RlIiBpZD0iNDE2IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Maneuver_Node("Circularization", 0, Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"), Orbital_Speed(Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+")), Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+"))), (0 - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRmxpZ2h0UGF0aEFuZ2xlIiAvPg==")), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
});

// VZTOPBLOCK
// VZBLOCK PEN1c3RvbUluc3RydWN0aW9uIGNhbGxGb3JtYXQ9IkRlb3JiaXQgQ29vcmRpbmF0ZXMgKDApIFZlbG9jaXR5IEFuZ2xlICgxKSBQbGFuZXQgUm90YXRpb24gUmF0ZSAoMikgIiBmb3JtYXQ9IkRlb3JiaXQgQ29vcmRpbmF0ZXMgfENvb3JkaW5hdGVzfCBWZWxvY2l0eSBBbmdsZSB8VmVsb2NpdHlBbmdsZXwgUGxhbmV0IFJvdGF0aW9uIFJhdGUgfFBsYW5ldFJvdGF0aW9uUmF0ZXwgIiBuYW1lPSJEZW9yYml0IENvb3JkaW5hdGVzIiBpZD0iNDE3IiBzdHlsZT0iY3VzdG9tLWluc3RydWN0aW9uIiBwb3M9IjI4NDAuNTUyLC00NTQxLjA3NiIgLz4=
var Deorbit_Coordinates = Vz.DeclareCustomInstruction("Deorbit Coordinates", "Coordinates", "VelocityAngle", "PlanetRotationRate").SetInstructions((Coordinates, VelocityAngle, PlanetRotationRate) =>
{
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0MTgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlByb2dyYWRlIiAvPjwvU2V0VmFyaWFibGU+
    Prograde = (Vz.Length(Vz.Craft.Velocity.Orbital()) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0MTkiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkkiIC8+PC9TZXRWYXJpYWJsZT4=
    I = (Vz.Length(Vz.Craft.Velocity.Orbital()) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjQiIC8+"));
    // VZEL PFdoaWxlIGlkPSI0MjAiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=") > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuMDAxIiAvPg=="))))
    {
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iNDIxIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), (Vz.Norm(Vz.Craft.Velocity.Orbital()) * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUHJvZ3JhZGUiIC8+")));
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSI0MjIiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
        Orbital_State_Vectors((0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJDb29yZGluYXRlcyIgLz4="))))));
        // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSI0MjMiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlByb2dyYWRlIiAvPjwvQ2hhbmdlVmFyaWFibGU+
        Prograde += ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRmxpZ2h0UGF0aEFuZ2xlIiAvPg==") > Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWxvY2l0eUFuZ2xlIiAvPg==")) ? (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=")) : Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4="));
        // VZEL PFNldFZhcmlhYmxlIGlkPSI0MjQiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkkiIC8+PC9TZXRWYXJpYWJsZT4=
        I = (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSSIgLz4=") / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
    }
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iNDI1IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), (Vz.Norm(Vz.Craft.Velocity.Orbital()) * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUHJvZ3JhZGUiIC8+")));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iNDI2IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Time_to_TrueAnomaly((0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJDb29yZGluYXRlcyIgLz4="))))));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSI0MjciIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_State_Vectors((0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJDb29yZGluYXRlcyIgLz4="))))));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0MjgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
    Time = ((Signed_Angle(Rotate_Y_Axis_Vector(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJDb29yZGluYXRlcyIgLz4=")), ((Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=") / Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRSb3RhdGlvblJhdGUiIC8+")) * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+"))), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPg==")) + Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=")) % Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4="));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iNDI5IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0MzAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
    Time = (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") / Vz.Abs(((Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=") / ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSW5jbGluYXRpb24iIC8+") > Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IihwaSkvMiIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=")) ? (0 - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGVyaW9kIiAvPg==")) : Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGVyaW9kIiAvPg=="))) - (Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=") / Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRSb3RhdGlvblJhdGUiIC8+")))));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0MzEiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik9yYml0IiAvPjwvU2V0VmFyaWFibGU+
    Orbit = (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGVyaW9kIiAvPg==") * Vz.Floor((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") / Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGVyaW9kIiAvPg=="))));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUcnVlIEFub21hbHkgQWZ0ZXIgVGltZSIgaWQ9IjQzMiIgc3R5bGU9ImNhbGwtY3VzdG9tLWluc3RydWN0aW9uIiAvPg==
    True_Anomaly_After_Time(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0MzMiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRBIiAvPjwvU2V0VmFyaWFibGU+
    TA = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVHJ1ZUFub21hbHkiIC8+");
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSI0MzQiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0MzUiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkNyYWZ0UG9zaXRpb24iIC8+PC9TZXRWYXJpYWJsZT4=
    CraftPosition = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+");
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iNDM2IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4="), (Vz.Norm(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+")) * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUHJvZ3JhZGUiIC8+")));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iNDM3IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Time_to_TrueAnomaly((0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJDb29yZGluYXRlcyIgLz4="))))));
    // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSI0MzgiIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9DaGFuZ2VWYXJpYWJsZT4=
    Time += Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+");
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSI0MzkiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_State_Vectors((0 - True_Anomaly_at_Radius(Vz.Length(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJDb29yZGluYXRlcyIgLz4="))))));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0NDAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik5vcm1hbCIgLz48L1NldFZhcmlhYmxlPg==
    Normal = Signed_Angle(Flatten(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4=")), Flatten(Rotate_Y_Axis_Vector(Vz.ToPosition(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJDb29yZGluYXRlcyIgLz4=")), ((Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=") / Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRSb3RhdGlvblJhdGUiIC8+")) * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="))), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4=")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQ3JhZnRQb3NpdGlvbiIgLz4="));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJNYW5ldXZlciBOb2RlIiBpZD0iNDQxIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Maneuver_Node("Deorbit", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iT3JiaXQiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUHJvZ3JhZGUiIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTm9ybWFsIiAvPg=="));
});

// VZTOPBLOCK
// VZBLOCK PEN1c3RvbUluc3RydWN0aW9uIGNhbGxGb3JtYXQ9IkxhdW5jaCBJbmNsaW5hdGlvbiAoMCkgUmlnaHQgQXNjZW5zaW9uICgxKSBBcG9hcHNpcyAoMikgUGxhbmV0IFJvdGF0aW9uIFJhdGUgKDMpIFdhaXQgV2luZG93ICg0KSAiIGZvcm1hdD0iTGF1bmNoIEluY2xpbmF0aW9uIHxJbmNsaW5hdGlvbnwgUmlnaHQgQXNjZW5zaW9uIHxSaWdodEFzY2Vuc2lvbnwgQXBvYXBzaXMgfEFwb2Fwc2lzfCBQbGFuZXQgUm90YXRpb24gUmF0ZSB8UGxhbmV0Um90YXRpb25SYXRlfCBXYWl0IFdpbmRvdyB8V2FpdHwgIiBuYW1lPSJMYXVuY2ggSW5jbGluYXRpb24iIGlkPSI0NDIiIHN0eWxlPSJjdXN0b20taW5zdHJ1Y3Rpb24iIHBvcz0iMjgzOS4yNDcsLTIxNzEuMjM5IiAvPg==
var Launch_Inclination = Vz.DeclareCustomInstruction("Launch Inclination", "Inclination", "RightAscension", "Apoapsis", "PlanetRotationRate", "Wait").SetInstructions((Inclination, RightAscension, Apoapsis, PlanetRotationRate, Wait) =>
{
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0NDMiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik11IiAvPjwvU2V0VmFyaWFibGU+
    Mu = (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjYuNjczODRFLTExIiAvPg==") * Vz.Planet(Vz.Craft.Orbit.Planet()).Mass());
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0NDQiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkFwb2Fwc2lzIiAvPjwvU2V0VmFyaWFibGU+
    Apoapsis = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBcG9hcHNpcyIgLz4=");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0NDUiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkxhdGl0dWRlIiAvPjwvU2V0VmFyaWFibGU+
    Latitude = Vz.Abs(Vz.Deg2Rad(Vz.PosToLatLongAgl(Vz.Craft.Nav.Position()).x));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0NDYiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlZlbG9jaXR5IiAvPjwvU2V0VmFyaWFibGU+
    Velocity = Orbital_Speed((Vz.Planet(Vz.Craft.Orbit.Planet()).Radius() + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBcG9hcHNpcyIgLz4=")), (Vz.Planet(Vz.Craft.Orbit.Planet()).Radius() + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBcG9hcHNpcyIgLz4=")));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0NDciIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlBsYW5ldFZlbG9jaXR5IiAvPjwvU2V0VmFyaWFibGU+
    PlanetVelocity = (Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9Im1hZ25pdHVkZShGbGlnaHREYXRhLlBvc2l0aW9uKSpjb3MoYWJzKExhdGl0dWRlKSkqMioocGkpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==") / Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRSb3RhdGlvblJhdGUiIC8+"));
    // VZEL PElmIGlkPSI0NDgiIHN0eWxlPSJpZiIgLz4=
    using (new If((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJJbmNsaW5hdGlvbiIgLz4=") <= Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IihwaSkvMiIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4="))))
    {
        // VZEL PFNldFZhcmlhYmxlIGlkPSI0NDkiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iSW5jbGluYXRpb24iIC8+PC9TZXRWYXJpYWJsZT4=
        Inclination = ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJJbmNsaW5hdGlvbiIgLz4=") <= Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGF0aXR1ZGUiIC8+")) ? Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGF0aXR1ZGUiIC8+") : Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJJbmNsaW5hdGlvbiIgLz4="));
    }
    // VZEL PEVsc2VJZiBpZD0iNDUwIiBzdHlsZT0iZWxzZSIgLz4=
    using (new ElseIf(Vz.RawConstant("PENvbnN0YW50IGJvb2w9InRydWUiIC8+")))
    {
        // VZEL PFNldFZhcmlhYmxlIGlkPSI0NTEiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iSW5jbGluYXRpb24iIC8+PC9TZXRWYXJpYWJsZT4=
        Inclination = (((Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJJbmNsaW5hdGlvbiIgLz4=")) <= Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGF0aXR1ZGUiIC8+")) ? (Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGF0aXR1ZGUiIC8+")) : Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJJbmNsaW5hdGlvbiIgLz4="));
    }
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0NTIiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkxhdW5jaFBsYW5lTm9ybWFsIiAvPjwvU2V0VmFyaWFibGU+
    LaunchPlaneNormal = Rotate_Y_Axis_Vector(Rotate_X_Axis_Vector(Vz.Vec(0, Vz.RawConstant("PENvbnN0YW50IHRleHQ9Ii0xIiAvPg=="), 0), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJJbmNsaW5hdGlvbiIgLz4=")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJSaWdodEFzY2Vuc2lvbiIgLz4="));
    // VZEL PFNldExpc3Qgb3A9ImNsZWFyIiBpZD0iNDUzIiBzdHlsZT0ibGlzdC1jbGVhciIgLz4=
    Vz.ListClear(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"));
    // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjQ1NCIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
    Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), Vz.Asin((Vz.Tan(Vz.Deg2Rad(Vz.PosToLatLongAgl(Vz.Craft.Nav.Position()).x)) / Vz.Tan(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJJbmNsaW5hdGlvbiIgLz4=")))));
    // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjQ1NSIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
    Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), ((Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=") + (Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==") - Vz.Deg2Rad(Vz.PosToLatLongAgl(Vz.Cross(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTGF1bmNoUGxhbmVOb3JtYWwiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iWWF4aXMiIC8+"))).y))) % Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=")));
    // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjQ1NiIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
    Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), ((((((Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")) - Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"))) - Vz.Deg2Rad(Vz.PosToLatLongAgl(Vz.Craft.Nav.Position()).y)) * (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRSb3RhdGlvblJhdGUiIC8+") / Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4="))) - Burn_Time(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+"))) + Vz.Abs(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRSb3RhdGlvblJhdGUiIC8+"))) % Vz.Abs(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRSb3RhdGlvblJhdGUiIC8+"))));
    // VZEL PFNldExpc3Qgb3A9ImFkZCIgaWQ9IjQ1NyIgc3R5bGU9Imxpc3QtYWRkIiAvPg==
    Vz.ListAdd(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), (((((((Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==") - Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"))) - Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"))) - Vz.Deg2Rad(Vz.PosToLatLongAgl(Vz.Craft.Nav.Position()).y)) * (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRSb3RhdGlvblJhdGUiIC8+") / Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjIqKHBpKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4="))) - Burn_Time(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+"))) + Vz.Abs(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRSb3RhdGlvblJhdGUiIC8+"))) % Vz.Abs(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRSb3RhdGlvblJhdGUiIC8+"))));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0NTgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
    Time = Vz.Min(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjMiIC8+")), Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjQiIC8+")));
    // VZEL PFdoaWxlIGlkPSI0NTkiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") < Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJXYWl0IiAvPg=="))))
    {
        // VZEL PFNldFZhcmlhYmxlIGlkPSI0NjAiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
        Time = Vz.Max(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjMiIC8+")), Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjQiIC8+")));
        // VZEL PElmIGlkPSI0NjEiIHN0eWxlPSJpZiIgLz4=
        using (new If((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") > Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJXYWl0IiAvPg=="))))
        {
            // VZEL PEJyZWFrIGlkPSI0NjIiIHN0eWxlPSJicmVhayIgLz4=
            Vz.Break();
        }
        // VZEL PFNldExpc3Qgb3A9InNldCIgaWQ9IjQ2MyIgc3R5bGU9Imxpc3Qtc2V0IiAvPg==
        Vz.ListSet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), (Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjMiIC8+")) + Vz.Abs(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRSb3RhdGlvblJhdGUiIC8+"))), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjMiIC8+"));
        // VZEL PFNldExpc3Qgb3A9InNldCIgaWQ9IjQ2NCIgc3R5bGU9Imxpc3Qtc2V0IiAvPg==
        Vz.ListSet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), (Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjQiIC8+")) + Vz.Abs(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQbGFuZXRSb3RhdGlvblJhdGUiIC8+"))), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjQiIC8+"));
        // VZEL PFNldFZhcmlhYmxlIGlkPSI0NjUiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9TZXRWYXJpYWJsZT4=
        Time = Vz.Min(Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjMiIC8+")), Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJMYXVuY2giIC8+"), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjQiIC8+")));
    }
    // VZEL PExvZ01lc3NhZ2UgaWQ9IjQ2NiIgc3R5bGU9ImxvZyIgLz4=
    Vz.Log(Vz.Join("Launch Time: ", Vz.StringOp("friendly", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="), ""), ""));
    // VZEL PENoYW5nZVZhcmlhYmxlIGlkPSI0NjciIHN0eWxlPSJjaGFuZ2UtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9DaGFuZ2VWYXJpYWJsZT4=
    Time += Vz.Time.TotalTime();
    // VZEL PEJyb2FkY2FzdE1lc3NhZ2UgZ2xvYmFsPSJmYWxzZSIgbG9jYWw9InRydWUiIGlkPSI0NjgiIHN0eWxlPSJicm9hZGNhc3QtbXNnIiAvPg==
    Vz.Broadcast(BroadCastType.Local, "Timewarp", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4="));
    // VZEL PFdoaWxlIGlkPSI0NjkiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") - Vz.Time.TotalTime()) > 0)))
    {
        // VZEL PERpc3BsYXlNZXNzYWdlIGlkPSI0NzAiIHN0eWxlPSJkaXNwbGF5IiAvPg==
        Vz.Display(Vz.Join("Launch Time:<br>", Clock((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz4=") - Vz.Time.TotalTime())), ""), 7);
        // VZEL PFdhaXRTZWNvbmRzIGlkPSI0NzEiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    }
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0NzIiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik5vcm1hbCIgLz48L1NldFZhcmlhYmxlPg==
    Normal = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJJbmNsaW5hdGlvbiIgLz4=");
    // VZEL PERpc3BsYXlNZXNzYWdlIGlkPSI0NzMiIHN0eWxlPSJkaXNwbGF5IiAvPg==
    Vz.Display("Lift off!", 7);
    // VZEL PFNldElucHV0IGlucHV0PSJ0aHJvdHRsZSIgaWQ9IjQ3NCIgc3R5bGU9InNldC1pbnB1dCIgLz4=
    Vz.SetInput(CraftInput.Throttle, ((Vz.Length(Vz.Craft.Velocity.Gravity()) * Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjQiIC8+")) / (Vz.Craft.Performance.MaxThrust() / Vz.Craft.Performance.Mass())));
    // VZEL PFNldFRpbWVNb2RlIG1vZGU9IkZhc3RGb3J3YXJkIiBpZD0iNDc1IiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
    Vz.SetTimeModeAttr("FastForward");
    // VZEL PFdoaWxlIGlkPSI0NzYiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While((Vz.Craft.Orbit.Apoapsis() < Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQXBvYXBzaXMiIC8+"))))
    {
        // VZEL PFNldFRhcmdldEhlYWRpbmcgcHJvcGVydHk9InBpdGNoIiBpZD0iNDc3IiBzdHlsZT0ic2V0LWhlYWRpbmciIC8+
        Vz.SetTargetHeading(TargetHeadingProperty.Pitch, Vz.Max(0, (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjkwIiAvPg==") - (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjkwIiAvPg==") * ((Vz.Craft.Orbit.Apoapsis() / Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQXBvYXBzaXMiIC8+")) ^ Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuNTUiIC8+"))))));
        // VZEL PFNldFRhcmdldEhlYWRpbmcgcHJvcGVydHk9ImhlYWRpbmciIGlkPSI0NzgiIHN0eWxlPSJzZXQtaGVhZGluZyIgLz4=
        Vz.SetTargetHeading(TargetHeadingProperty.Heading, Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InNpZ25lZEFuZ2xlKEZsaWdodERhdGEuTm9ydGgsKG5vcm1hbGl6ZShjcm9zcyh2OkxhdW5jaFBsYW5lTm9ybWFsLEZsaWdodERhdGEuUG9zaXRpb24pKSpWZWxvY2l0eSktKG5vcm1hbGl6ZShGbGlnaHREYXRhLkVhc3QpKlBsYW5ldFZlbG9jaXR5KSxGbGlnaHREYXRhLlBvc2l0aW9uKSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4="));
        // VZEL PFdhaXRTZWNvbmRzIGlkPSI0NzkiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    }
    // VZEL PFNldElucHV0IGlucHV0PSJ0aHJvdHRsZSIgaWQ9IjQ4MCIgc3R5bGU9InNldC1pbnB1dCIgLz4=
    Vz.SetInput(CraftInput.Throttle, Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    // VZEL PFdhaXRVbnRpbCBpZD0iNDgxIiBzdHlsZT0id2FpdC11bnRpbCIgLz4=
    using (new WaitUntil(((!Vz.Craft.Performance.CurrentThrust()) && (!Vz.Craft.Atmosphere.AirDensity())))) { }
    // VZEL PFdhaXRTZWNvbmRzIGlkPSI0ODIiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iNDgzIiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0ODQiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IlRBIiAvPjwvU2V0VmFyaWFibGU+
    TA = True_Anomaly_at_Radius((Vz.Planet(Vz.Craft.Orbit.Planet()).Radius() + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBcG9hcHNpcyIgLz4=")));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSI0ODUiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJNYW5ldXZlciBOb2RlIiBpZD0iNDg2IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Maneuver_Node("Circularization", 0, Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVEEiIC8+"), Orbital_Speed(Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+")), Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+"))), (0 - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRmxpZ2h0UGF0aEFuZ2xlIiAvPg==")), ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+").y > 0) ? (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTm9ybWFsIiAvPg==") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSW5jbGluYXRpb24iIC8+")) : (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iSW5jbGluYXRpb24iIC8+") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTm9ybWFsIiAvPg=="))));
});

// VZTOPBLOCK
// VZBLOCK PEN1c3RvbUluc3RydWN0aW9uIGNhbGxGb3JtYXQ9Ik1hbmV1dmVyIE5vZGUgKDApIFRpbWUgKDEpIFRydWUgQW5vbWFseSAoMikgUHJvZ3JhZGUgKDMpIFJhZGlhbCAoNCkgTm9ybWFsICg1KSAiIGZvcm1hdD0iTWFuZXV2ZXIgTm9kZSB8TWFuZXV2ZXJ8IFRpbWUgfFRpbWV8IFRydWUgQW5vbWFseSB8VHJ1ZUFub21hbHl8IFByb2dyYWRlIHxQcm9ncmFkZXwgUmFkaWFsIHxSYWRpYWx8IE5vcm1hbCB8Tm9ybWFsfCAiIG5hbWU9Ik1hbmV1dmVyIE5vZGUiIGlkPSI0ODciIHN0eWxlPSJjdXN0b20taW5zdHJ1Y3Rpb24iIHBvcz0iMjg1MC4zOTcsLTE5NC41NjY3IiAvPg==
var Maneuver_Node = Vz.DeclareCustomInstruction("Maneuver Node", "Maneuver", "Time", "TrueAnomaly", "Prograde", "Radial", "Normal").SetInstructions((Maneuver, Time, TrueAnomaly, Prograde, Radial, Normal) =>
{
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iNDg4IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJUaW1lIHRvIFRydWVBbm9tYWx5IiBpZD0iNDg5IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
    Time_to_TrueAnomaly(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUcnVlQW5vbWFseSIgLz4="));
    // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSI0OTAiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
    Orbital_State_Vectors(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUcnVlQW5vbWFseSIgLz4="));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0OTEiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkJ1cm5Qb3NpdGlvbiIgLz48L1NldFZhcmlhYmxlPg==
    BurnPosition = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUG9zaXRpb24iIC8+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0OTIiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkJ1cm5WZWxvY2l0eSIgLz48L1NldFZhcmlhYmxlPg==
    BurnVelocity = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+");
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0OTMiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkJ1cm5WZWN0b3IiIC8+PC9TZXRWYXJpYWJsZT4=
    BurnVector = (Rotate_Vector(Rotate_Vector((Vz.Norm(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblZlbG9jaXR5IiAvPg==")) * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQcm9ncmFkZSIgLz4=")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJSYWRpYWwiIC8+")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblBvc2l0aW9uIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJOb3JtYWwiIC8+")) - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblZlbG9jaXR5IiAvPg=="));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0OTQiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkJ1cm5WZWxvY2l0eSIgLz48L1NldFZhcmlhYmxlPg==
    BurnVelocity = (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblZlY3RvciIgLz4=") + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblZlbG9jaXR5IiAvPg=="));
    // VZEL PFNldFZhcmlhYmxlIGlkPSI0OTUiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkJ1cm5UaW1lciIgLz48L1NldFZhcmlhYmxlPg==
    BurnTimer = (((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVGltZXRvVEEiIC8+") + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUaW1lIiAvPg==")) - ((Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblZlY3RvciIgLz4=")) < (Vz.Craft.Performance.CurrentThrust() / Vz.Craft.Performance.Mass())) ? Burn_Time(Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblZlY3RvciIgLz4="))) : ((Burn_Time(Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblZlY3RvciIgLz4="))) + Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkZsaWdodERhdGEuV2VpZ2h0ZWRUaHJvdHRsZVJlc3BvbnNlVGltZSIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=")) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+")))) + Vz.Time.TotalTime());
    // VZEL PExvZ01lc3NhZ2UgaWQ9IjQ5NiIgc3R5bGU9ImxvZyIgLz4=
    Vz.Log(Vz.Format(((Burn_Time(Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblZlY3RvciIgLz4="))) > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")) ? "{0} Δv: {1:n1}m/s | Burn Length: {2:n1}s | Start time: {3}" : "{0} Δv: {1:n1}m/s | Burn Length: < 1s | Start time: {3}"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJNYW5ldXZlciIgLz4="), Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblZlY3RvciIgLz4=")), Burn_Time(Vz.Length(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblZlY3RvciIgLz4="))), Vz.StringOp("friendly", (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblRpbWVyIiAvPg==") - Vz.Time.TotalTime()), ""), ""));
    // VZEL PEJyb2FkY2FzdE1lc3NhZ2UgZ2xvYmFsPSJmYWxzZSIgbG9jYWw9InRydWUiIGlkPSI0OTciIHN0eWxlPSJicm9hZGNhc3QtbXNnIiAvPg==
    Vz.Broadcast(BroadCastType.Local, "Timewarp", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblRpbWVyIiAvPg=="));
    // VZEL PFdoaWxlIGlkPSI0OTgiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblRpbWVyIiAvPg==") - Vz.Time.TotalTime()) > 0)))
    {
        // VZEL PERpc3BsYXlNZXNzYWdlIGlkPSI0OTkiIHN0eWxlPSJkaXNwbGF5IiAvPg==
        Vz.Display(Vz.Join(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJNYW5ldXZlciIgLz4="), ": ", Clock((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblRpbWVyIiAvPg==") - Vz.Time.TotalTime())), ""), 7);
        // VZEL PExvY2tOYXZTcGhlcmUgaW5kaWNhdG9yVHlwZT0iQ3VycmVudCIgaWQ9IjUwMCIgc3R5bGU9ImxvY2stbmF2LXNwaGVyZSIgLz4=
        Vz.LockNavSphere(LockNavSphereIndicatorType.Current, 0);
        // VZEL PExvY2tOYXZTcGhlcmUgaW5kaWNhdG9yVHlwZT0iVmVjdG9yIiBpZD0iNTAxIiBzdHlsZT0ibG9jay1uYXYtc3BoZXJlLXZlY3RvciIgLz4=
        Vz.LockNavSphere(LockNavSphereIndicatorType.Vector, Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblZlY3RvciIgLz4="));
        // VZEL PFdhaXRTZWNvbmRzIGlkPSI1MDIiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    }
    // VZEL PFdoaWxlIGlkPSI1MDMiIHN0eWxlPSJ3aGlsZSIgLz4=
    using (new While(Vz.RawConstant("PENvbnN0YW50IHN0eWxlPSJ0cnVlIiBib29sPSJ0cnVlIiAvPg==")))
    {
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIEVsZW1lbnRzIFBhcmVudCBCb2R5IiBpZD0iNTA0IiBzdHlsZT0iY2FsbC1jdXN0b20taW5zdHJ1Y3Rpb24iIC8+
        Orbital_Elements_Parent_Body(Vz.Craft.Orbit.Planet(), Vz.Craft.Nav.Position(), Vz.Craft.Velocity.Orbital());
        // VZEL PENhbGxDdXN0b21JbnN0cnVjdGlvbiBjYWxsPSJPcmJpdGFsIFN0YXRlIFZlY3RvcnMiIGlkPSI1MDUiIHN0eWxlPSJjYWxsLWN1c3RvbS1pbnN0cnVjdGlvbiIgLz4=
        Orbital_State_Vectors(Signed_Angle(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblBvc2l0aW9uIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5VmVjdG9yIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQW5ndWxhck1vbWVudHVtIiAvPg==")));
        // VZEL PExvY2tOYXZTcGhlcmUgaW5kaWNhdG9yVHlwZT0iQ3VycmVudCIgaWQ9IjUwNiIgc3R5bGU9ImxvY2stbmF2LXNwaGVyZSIgLz4=
        Vz.LockNavSphere(LockNavSphereIndicatorType.Current, 0);
        // VZEL PExvY2tOYXZTcGhlcmUgaW5kaWNhdG9yVHlwZT0iVmVjdG9yIiBpZD0iNTA3IiBzdHlsZT0ibG9jay1uYXYtc3BoZXJlLXZlY3RvciIgLz4=
        Vz.LockNavSphere(LockNavSphereIndicatorType.Vector, Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblZlY3RvciIgLz4="));
        // VZEL PFNldFZhcmlhYmxlIGlkPSI1MDgiIHN0eWxlPSJzZXQtdmFyaWFibGUiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Is6UdiIgLz48L1NldFZhcmlhYmxlPg==
        Δv = Vz.Dot(Vz.Norm(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblZlY3RvciIgLz4=")), (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQnVyblZlbG9jaXR5IiAvPg==") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iVmVsb2NpdHkiIC8+")));
        // VZEL PElmIGlkPSI1MDkiIHN0eWxlPSJpZiIgLz4=
        using (new If((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0izpR2IiAvPg==") > (((Vz.Craft.Performance.CurrentThrust() / Vz.Craft.Performance.Mass()) * (Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkZsaWdodERhdGEuV2VpZ2h0ZWRUaHJvdHRsZVJlc3BvbnNlVGltZS8yIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==") * (Vz.Craft.Performance.CurrentThrust() / Vz.Craft.Performance.MaxThrust()))) + ((Vz.Craft.Performance.CurrentThrust() / Vz.Craft.Performance.Mass()) * Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuMDMzNCIgLz4="))))))
        {
            // VZEL PFNldElucHV0IGlucHV0PSJ0aHJvdHRsZSIgaWQ9IjUxMCIgc3R5bGU9InNldC1pbnB1dCIgLz4=
            Vz.SetInput(CraftInput.Throttle, Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"));
            // VZEL PElmIGlkPSI1MTEiIHN0eWxlPSJpZiIgLz4=
            using (new If((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0izpR2IiAvPg==") < ((Vz.Craft.Performance.MaxThrust() / Vz.Craft.Performance.Mass()) * Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+")))))
            {
                // VZEL PFNldFRpbWVNb2RlIG1vZGU9Ik5vcm1hbCIgaWQ9IjUxMiIgc3R5bGU9InNldC10aW1lLW1vZGUiIC8+
                Vz.SetTimeModeAttr("Normal");
            }
            // VZEL PEVsc2VJZiBpZD0iNTEzIiBzdHlsZT0iZWxzZSIgLz4=
            using (new ElseIf(Vz.RawConstant("PENvbnN0YW50IGJvb2w9InRydWUiIC8+")))
            {
                // VZEL PFNldFRpbWVNb2RlIG1vZGU9IkZhc3RGb3J3YXJkIiBpZD0iNTE0IiBzdHlsZT0ic2V0LXRpbWUtbW9kZSIgLz4=
                Vz.SetTimeModeAttr("FastForward");
            }
        }
        // VZEL PEVsc2VJZiBpZD0iNTE1IiBzdHlsZT0iZWxzZSIgLz4=
        using (new ElseIf(Vz.RawConstant("PENvbnN0YW50IGJvb2w9InRydWUiIC8+")))
        {
            // VZEL PFNldElucHV0IGlucHV0PSJ0aHJvdHRsZSIgaWQ9IjUxNiIgc3R5bGU9InNldC1pbnB1dCIgLz4=
            Vz.SetInput(CraftInput.Throttle, Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
            // VZEL PElmIGlkPSI1MTciIHN0eWxlPSJpZiIgLz4=
            using (new If((!Vz.Craft.Performance.CurrentThrust())))
            {
                // VZEL PEJyZWFrIGlkPSI1MTgiIHN0eWxlPSJicmVhayIgLz4=
                Vz.Break();
            }
        }
        // VZEL PERpc3BsYXlNZXNzYWdlIGlkPSI1MTkiIHN0eWxlPSJkaXNwbGF5IiAvPg==
        Vz.Display(Vz.Format("{0} Δv: {1:n1}m/s", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJNYW5ldXZlciIgLz4="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0izpR2IiAvPg=="), ""), 7);
        // VZEL PFdhaXRTZWNvbmRzIGlkPSI1MjAiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
        Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"));
    }
    // VZEL PFdhaXRTZWNvbmRzIGlkPSI1MjEiIHN0eWxlPSJ3YWl0LXNlY29uZHMiIC8+
    Vz.WaitSeconds(Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"));
});

// ── Custom Expressions ───────────────────────────────
// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iUm90YXRlIFZlY3RvciAoMCkgQXhpcyAoMSkgQW5nbGUgKDIpICIgZm9ybWF0PSJSb3RhdGUgVmVjdG9yIHxWZWN0b3J8IEF4aXMgfEF4aXN8IEFuZ2xlIHxBbmdsZXwgIHJldHVybiAoMCkiIG5hbWU9IlJvdGF0ZSBWZWN0b3IiIHN0eWxlPSJjdXN0b20tZXhwcmVzc2lvbiIgcG9zPSItMTM1OS40OTcsMjIzLjYyOTgiPjxCaW5hcnlPcCBvcD0iKyIgc3R5bGU9Im9wLWFkZCI+PEJpbmFyeU9wIG9wPSIqIiBzdHlsZT0ib3AtbXVsIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlZlY3RvciIgLz48TWF0aEZ1bmN0aW9uIGZ1bmN0aW9uPSJjb3MiIHN0eWxlPSJvcC1tYXRoIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IkFuZ2xlIiAvPjwvTWF0aEZ1bmN0aW9uPjwvQmluYXJ5T3A+PEJpbmFyeU9wIG9wPSIrIiBzdHlsZT0ib3AtYWRkIj48QmluYXJ5T3Agb3A9IioiIHN0eWxlPSJvcC1tdWwiPjxWZWN0b3JPcCBvcD0iY3Jvc3MiIHN0eWxlPSJ2ZWMtb3AtMiI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+PFZlY3Rvck9wIG9wPSJub3JtIiBzdHlsZT0idmVjLW9wLTEiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iQXhpcyIgLz48L1ZlY3Rvck9wPjwvVmVjdG9yT3A+PE1hdGhGdW5jdGlvbiBmdW5jdGlvbj0ic2luIiBzdHlsZT0ib3AtbWF0aCI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBbmdsZSIgLz48L01hdGhGdW5jdGlvbj48L0JpbmFyeU9wPjxCaW5hcnlPcCBvcD0iKiIgc3R5bGU9Im9wLW11bCI+PFZlY3Rvck9wIG9wPSJub3JtIiBzdHlsZT0idmVjLW9wLTEiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iQXhpcyIgLz48L1ZlY3Rvck9wPjxCaW5hcnlPcCBvcD0iKiIgc3R5bGU9Im9wLW11bCI+PFZlY3Rvck9wIG9wPSJkb3QiIHN0eWxlPSJ2ZWMtb3AtMiI+PFZlY3Rvck9wIG9wPSJub3JtIiBzdHlsZT0idmVjLW9wLTEiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iQXhpcyIgLz48L1ZlY3Rvck9wPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iVmVjdG9yIiAvPjwvVmVjdG9yT3A+PEJpbmFyeU9wIG9wPSItIiBzdHlsZT0ib3Atc3ViIj48Q29uc3RhbnQgdGV4dD0iMSIgLz48TWF0aEZ1bmN0aW9uIGZ1bmN0aW9uPSJjb3MiIHN0eWxlPSJvcC1tYXRoIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IkFuZ2xlIiAvPjwvTWF0aEZ1bmN0aW9uPjwvQmluYXJ5T3A+PC9CaW5hcnlPcD48L0JpbmFyeU9wPjwvQmluYXJ5T3A+PC9CaW5hcnlPcD48L0N1c3RvbUV4cHJlc3Npb24+
var Rotate_Vector = Vz.DeclareCustomExpression("Rotate Vector", "Vector", "Axis", "Angle").SetReturn((Vector, Axis, Angle) =>
{
    return ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+") * Vz.Cos(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBbmdsZSIgLz4="))) + ((Vz.Cross(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+"), Vz.Norm(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBeGlzIiAvPg=="))) * Vz.Sin(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBbmdsZSIgLz4="))) + (Vz.Norm(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBeGlzIiAvPg==")) * (Vz.Dot(Vz.Norm(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBeGlzIiAvPg==")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+")) * (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+") - Vz.Cos(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBbmdsZSIgLz4=")))))));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iT3JiaXRhbCBTcGVlZCAoMCkgKDEpICIgZm9ybWF0PSJPcmJpdGFsIFNwZWVkIHxSYWRpdXN8IHxTTUF8ICByZXR1cm4gKDApIiBuYW1lPSJPcmJpdGFsIFNwZWVkIiBzdHlsZT0iY3VzdG9tLWV4cHJlc3Npb24iIHBvcz0iLTEzNjEuNDk4LDU0Mi4xMzQ2Ij48TWF0aEZ1bmN0aW9uIGZ1bmN0aW9uPSJzcXJ0IiBzdHlsZT0ib3AtbWF0aCI+PEJpbmFyeU9wIG9wPSIqIiBzdHlsZT0ib3AtbXVsIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJNdSIgLz48QmluYXJ5T3Agb3A9Ii0iIHN0eWxlPSJvcC1zdWIiPjxCaW5hcnlPcCBvcD0iLyIgc3R5bGU9Im9wLWRpdiI+PENvbnN0YW50IHRleHQ9IjIiIC8+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJSYWRpdXMiIC8+PC9CaW5hcnlPcD48QmluYXJ5T3Agb3A9Ii8iIHN0eWxlPSJvcC1kaXYiPjxDb25zdGFudCB0ZXh0PSIxIiAvPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iU01BIiAvPjwvQmluYXJ5T3A+PC9CaW5hcnlPcD48L0JpbmFyeU9wPjwvTWF0aEZ1bmN0aW9uPjwvQ3VzdG9tRXhwcmVzc2lvbj4=
var Orbital_Speed = Vz.DeclareCustomExpression("Orbital Speed", "Radius", "SMA").SetReturn((Radius, SMA) =>
{
    return Vz.Sqrt((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTXUiIC8+") * ((Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+") / Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJSYWRpdXMiIC8+")) - (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+") / Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJTTUEiIC8+")))));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iUm90YXRlIFkgQXhpcyBWZWN0b3IgKDApIEFuZ2xlICgxKSAiIGZvcm1hdD0iUm90YXRlIFkgQXhpcyBWZWN0b3IgfFZlY3RvcnwgQW5nbGUgfEFuZ2xlfCAgcmV0dXJuICgwKSIgbmFtZT0iUm90YXRlIFkgQXhpcyBWZWN0b3IiIHN0eWxlPSJjdXN0b20tZXhwcmVzc2lvbiIgcG9zPSItMTM2MC4wMzQsMjgzLjAyODgiPjxWZWN0b3Igc3R5bGU9InZlYyI+PEJpbmFyeU9wIG9wPSItIiBzdHlsZT0ib3Atc3ViIj48QmluYXJ5T3Agb3A9IioiIHN0eWxlPSJvcC1tdWwiPjxWZWN0b3JPcCBvcD0ieCIgc3R5bGU9InZlYy1vcC0xIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlZlY3RvciIgLz48L1ZlY3Rvck9wPjxNYXRoRnVuY3Rpb24gZnVuY3Rpb249ImNvcyIgc3R5bGU9Im9wLW1hdGgiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iQW5nbGUiIC8+PC9NYXRoRnVuY3Rpb24+PC9CaW5hcnlPcD48QmluYXJ5T3Agb3A9IioiIHN0eWxlPSJvcC1tdWwiPjxWZWN0b3JPcCBvcD0ieiIgc3R5bGU9InZlYy1vcC0xIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlZlY3RvciIgLz48L1ZlY3Rvck9wPjxNYXRoRnVuY3Rpb24gZnVuY3Rpb249InNpbiIgc3R5bGU9Im9wLW1hdGgiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iQW5nbGUiIC8+PC9NYXRoRnVuY3Rpb24+PC9CaW5hcnlPcD48L0JpbmFyeU9wPjxWZWN0b3JPcCBvcD0ieSIgc3R5bGU9InZlYy1vcC0xIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlZlY3RvciIgLz48L1ZlY3Rvck9wPjxCaW5hcnlPcCBvcD0iKyIgc3R5bGU9Im9wLWFkZCI+PEJpbmFyeU9wIG9wPSIqIiBzdHlsZT0ib3AtbXVsIj48VmVjdG9yT3Agb3A9InoiIHN0eWxlPSJ2ZWMtb3AtMSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+PC9WZWN0b3JPcD48TWF0aEZ1bmN0aW9uIGZ1bmN0aW9uPSJjb3MiIHN0eWxlPSJvcC1tYXRoIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IkFuZ2xlIiAvPjwvTWF0aEZ1bmN0aW9uPjwvQmluYXJ5T3A+PEJpbmFyeU9wIG9wPSIqIiBzdHlsZT0ib3AtbXVsIj48VmVjdG9yT3Agb3A9IngiIHN0eWxlPSJ2ZWMtb3AtMSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+PC9WZWN0b3JPcD48TWF0aEZ1bmN0aW9uIGZ1bmN0aW9uPSJzaW4iIHN0eWxlPSJvcC1tYXRoIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IkFuZ2xlIiAvPjwvTWF0aEZ1bmN0aW9uPjwvQmluYXJ5T3A+PC9CaW5hcnlPcD48L1ZlY3Rvcj48L0N1c3RvbUV4cHJlc3Npb24+
var Rotate_Y_Axis_Vector = Vz.DeclareCustomExpression("Rotate Y Axis Vector", "Vector", "Angle").SetReturn((Vector, Angle) =>
{
    return Vz.Vec(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+").x * Vz.Cos(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBbmdsZSIgLz4="))) - (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+").z * Vz.Sin(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBbmdsZSIgLz4=")))), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+").y, ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+").z * Vz.Cos(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBbmdsZSIgLz4="))) + (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+").x * Vz.Sin(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBbmdsZSIgLz4=")))));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iUm90YXRlIFggQXhpcyBWZWN0b3IgKDApIEFuZ2xlICgxKSAiIGZvcm1hdD0iUm90YXRlIFggQXhpcyBWZWN0b3IgfFZlY3RvcnwgQW5nbGUgfEFuZ2xlfCAgcmV0dXJuICgwKSIgbmFtZT0iUm90YXRlIFggQXhpcyBWZWN0b3IiIHN0eWxlPSJjdXN0b20tZXhwcmVzc2lvbiIgcG9zPSItMTM2MS4zNDgsMzQyLjUyNTQiPjxWZWN0b3Igc3R5bGU9InZlYyI+PFZlY3Rvck9wIG9wPSJ4IiBzdHlsZT0idmVjLW9wLTEiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iVmVjdG9yIiAvPjwvVmVjdG9yT3A+PEJpbmFyeU9wIG9wPSIrIiBzdHlsZT0ib3AtYWRkIj48QmluYXJ5T3Agb3A9IioiIHN0eWxlPSJvcC1tdWwiPjxWZWN0b3JPcCBvcD0ieSIgc3R5bGU9InZlYy1vcC0xIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlZlY3RvciIgLz48L1ZlY3Rvck9wPjxNYXRoRnVuY3Rpb24gZnVuY3Rpb249ImNvcyIgc3R5bGU9Im9wLW1hdGgiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iQW5nbGUiIC8+PC9NYXRoRnVuY3Rpb24+PC9CaW5hcnlPcD48QmluYXJ5T3Agb3A9IioiIHN0eWxlPSJvcC1tdWwiPjxWZWN0b3JPcCBvcD0ieiIgc3R5bGU9InZlYy1vcC0xIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlZlY3RvciIgLz48L1ZlY3Rvck9wPjxNYXRoRnVuY3Rpb24gZnVuY3Rpb249InNpbiIgc3R5bGU9Im9wLW1hdGgiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iQW5nbGUiIC8+PC9NYXRoRnVuY3Rpb24+PC9CaW5hcnlPcD48L0JpbmFyeU9wPjxCaW5hcnlPcCBvcD0iLSIgc3R5bGU9Im9wLXN1YiI+PEJpbmFyeU9wIG9wPSIqIiBzdHlsZT0ib3AtbXVsIj48VmVjdG9yT3Agb3A9InoiIHN0eWxlPSJ2ZWMtb3AtMSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+PC9WZWN0b3JPcD48TWF0aEZ1bmN0aW9uIGZ1bmN0aW9uPSJjb3MiIHN0eWxlPSJvcC1tYXRoIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IkFuZ2xlIiAvPjwvTWF0aEZ1bmN0aW9uPjwvQmluYXJ5T3A+PEJpbmFyeU9wIG9wPSIqIiBzdHlsZT0ib3AtbXVsIj48VmVjdG9yT3Agb3A9InkiIHN0eWxlPSJ2ZWMtb3AtMSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+PC9WZWN0b3JPcD48TWF0aEZ1bmN0aW9uIGZ1bmN0aW9uPSJzaW4iIHN0eWxlPSJvcC1tYXRoIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IkFuZ2xlIiAvPjwvTWF0aEZ1bmN0aW9uPjwvQmluYXJ5T3A+PC9CaW5hcnlPcD48L1ZlY3Rvcj48L0N1c3RvbUV4cHJlc3Npb24+
var Rotate_X_Axis_Vector = Vz.DeclareCustomExpression("Rotate X Axis Vector", "Vector", "Angle").SetReturn((Vector, Angle) =>
{
    return Vz.Vec(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+").x, ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+").y * Vz.Cos(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBbmdsZSIgLz4="))) + (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+").z * Vz.Sin(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBbmdsZSIgLz4=")))), ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+").z * Vz.Cos(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBbmdsZSIgLz4="))) - (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+").y * Vz.Sin(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJBbmdsZSIgLz4=")))));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iQnVybiBUaW1lICgwKSAiIGZvcm1hdD0iQnVybiBUaW1lIHxEZWx0YVZ8ICByZXR1cm4gKDApIiBuYW1lPSJCdXJuIFRpbWUiIHN0eWxlPSJjdXN0b20tZXhwcmVzc2lvbiIgcG9zPSItMTM1OS4yOSw0MDguMTQwMyI+PEJpbmFyeU9wIG9wPSIvIiBzdHlsZT0ib3AtZGl2Ij48QmluYXJ5T3Agb3A9IioiIHN0eWxlPSJvcC1tdWwiPjxCaW5hcnlPcCBvcD0iKiIgc3R5bGU9Im9wLW11bCI+PENyYWZ0UHJvcGVydHkgcHJvcGVydHk9IlBlcmZvcm1hbmNlLk1hc3MiIHN0eWxlPSJwcm9wLXBlcmZvcm1hbmNlIiAvPjxCaW5hcnlPcCBvcD0iLSIgc3R5bGU9Im9wLXN1YiI+PENvbnN0YW50IHRleHQ9IjEiIC8+PEJpbmFyeU9wIG9wPSIvIiBzdHlsZT0ib3AtZGl2Ij48Q29uc3RhbnQgdGV4dD0iMSIgLz48QmluYXJ5T3Agb3A9Il4iIHN0eWxlPSJvcC1leHAiPjxFdmFsdWF0ZUV4cHJlc3Npb24gc3R5bGU9ImV2YWx1YXRlLWV4cHJlc3Npb24iPjxDb25zdGFudCB0ZXh0PSJFIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPjxCaW5hcnlPcCBvcD0iLyIgc3R5bGU9Im9wLWRpdiI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJEZWx0YVYiIC8+PEJpbmFyeU9wIG9wPSIqIiBzdHlsZT0ib3AtbXVsIj48Q29uc3RhbnQgdGV4dD0iOS44MDY2MiIgLz48Q3JhZnRQcm9wZXJ0eSBwcm9wZXJ0eT0iUGVyZm9ybWFuY2UuQ3VycmVudElzcCIgc3R5bGU9InByb3AtcGVyZm9ybWFuY2UiIC8+PC9CaW5hcnlPcD48L0JpbmFyeU9wPjwvQmluYXJ5T3A+PC9CaW5hcnlPcD48L0JpbmFyeU9wPjwvQmluYXJ5T3A+PEJpbmFyeU9wIG9wPSIqIiBzdHlsZT0ib3AtbXVsIj48Q29uc3RhbnQgdGV4dD0iOS44MDY2MiIgLz48Q3JhZnRQcm9wZXJ0eSBwcm9wZXJ0eT0iUGVyZm9ybWFuY2UuQ3VycmVudElzcCIgc3R5bGU9InByb3AtcGVyZm9ybWFuY2UiIC8+PC9CaW5hcnlPcD48L0JpbmFyeU9wPjxDcmFmdFByb3BlcnR5IHByb3BlcnR5PSJQZXJmb3JtYW5jZS5NYXhBY3RpdmVFbmdpbmVUaHJ1c3QiIHN0eWxlPSJwcm9wLXBlcmZvcm1hbmNlIiAvPjwvQmluYXJ5T3A+PC9DdXN0b21FeHByZXNzaW9uPg==
var Burn_Time = Vz.DeclareCustomExpression("Burn Time", "DeltaV").SetReturn((DeltaV) =>
{
    return (((Vz.Craft.Performance.Mass() * (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+") - (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+") / (Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkUiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+") ^ (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJEZWx0YVYiIC8+") / (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjkuODA2NjIiIC8+") * Vz.Craft.Performance.ISP())))))) * (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjkuODA2NjIiIC8+") * Vz.Craft.Performance.ISP())) / Vz.Craft.Performance.MaxThrust());
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iT3JiaXRhbCBTcGVlZCBQZXJpb2QgKDApICgxKSAiIGZvcm1hdD0iT3JiaXRhbCBTcGVlZCBQZXJpb2QgfFJhZGl1c3wgfFBlcmlvZHwgIHJldHVybiAoMCkiIG5hbWU9Ik9yYml0YWwgU3BlZWQgUGVyaW9kIiBzdHlsZT0iY3VzdG9tLWV4cHJlc3Npb24iIHBvcz0iLTEzNTguNzczLDQ3OC4zMTciPjxNYXRoRnVuY3Rpb24gZnVuY3Rpb249InNxcnQiIHN0eWxlPSJvcC1tYXRoIj48QmluYXJ5T3Agb3A9IioiIHN0eWxlPSJvcC1tdWwiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik11IiAvPjxCaW5hcnlPcCBvcD0iLSIgc3R5bGU9Im9wLXN1YiI+PEJpbmFyeU9wIG9wPSIvIiBzdHlsZT0ib3AtZGl2Ij48Q29uc3RhbnQgdGV4dD0iMiIgLz48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlJhZGl1cyIgLz48L0JpbmFyeU9wPjxCaW5hcnlPcCBvcD0iLyIgc3R5bGU9Im9wLWRpdiI+PEJpbmFyeU9wIG9wPSIqIiBzdHlsZT0ib3AtbXVsIj48QmluYXJ5T3Agb3A9Il4iIHN0eWxlPSJvcC1leHAiPjxDb25zdGFudCB0ZXh0PSI0IiAvPjxCaW5hcnlPcCBvcD0iLyIgc3R5bGU9Im9wLWRpdiI+PENvbnN0YW50IHRleHQ9IjEiIC8+PENvbnN0YW50IHRleHQ9IjMiIC8+PC9CaW5hcnlPcD48L0JpbmFyeU9wPjxCaW5hcnlPcCBvcD0iXiIgc3R5bGU9Im9wLWV4cCI+PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPjxCaW5hcnlPcCBvcD0iLyIgc3R5bGU9Im9wLWRpdiI+PENvbnN0YW50IHRleHQ9IjIiIC8+PENvbnN0YW50IHRleHQ9IjMiIC8+PC9CaW5hcnlPcD48L0JpbmFyeU9wPjwvQmluYXJ5T3A+PEJpbmFyeU9wIG9wPSJeIiBzdHlsZT0ib3AtZXhwIj48QmluYXJ5T3Agb3A9IioiIHN0eWxlPSJvcC1tdWwiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9Ik11IiAvPjxCaW5hcnlPcCBvcD0iXiIgc3R5bGU9Im9wLWV4cCI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQZXJpb2QiIC8+PENvbnN0YW50IHRleHQ9IjIiIC8+PC9CaW5hcnlPcD48L0JpbmFyeU9wPjxCaW5hcnlPcCBvcD0iLyIgc3R5bGU9Im9wLWRpdiI+PENvbnN0YW50IHRleHQ9IjEiIC8+PENvbnN0YW50IHRleHQ9IjMiIC8+PC9CaW5hcnlPcD48L0JpbmFyeU9wPjwvQmluYXJ5T3A+PC9CaW5hcnlPcD48L0JpbmFyeU9wPjwvTWF0aEZ1bmN0aW9uPjwvQ3VzdG9tRXhwcmVzc2lvbj4=
var Orbital_Speed_Period = Vz.DeclareCustomExpression("Orbital Speed Period", "Radius", "Period").SetReturn((Radius, Period) =>
{
    return Vz.Sqrt((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTXUiIC8+") * ((Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+") / Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJSYWRpdXMiIC8+")) - (((Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjQiIC8+") ^ (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+") / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjMiIC8+"))) * (Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==") ^ (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+") / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjMiIC8+")))) / ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iTXUiIC8+") * (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQZXJpb2QiIC8+") ^ Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"))) ^ (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+") / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjMiIC8+")))))));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iRmxhdHRlbiAoMCkgTm9ybWFsICgxKSAiIGZvcm1hdD0iRmxhdHRlbiB8VmVjdG9yfCBOb3JtYWwgfE5vcm1hbHwgIHJldHVybiAoMCkiIG5hbWU9IkZsYXR0ZW4iIHN0eWxlPSJjdXN0b20tZXhwcmVzc2lvbiIgcG9zPSItMTM1OS4yNjcsNzM4LjUyODUiPjxWZWN0b3JPcCBvcD0icHJvamVjdCIgc3R5bGU9InZlYy1vcC0yIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlZlY3RvciIgLz48VmVjdG9yT3Agb3A9ImNyb3NzIiBzdHlsZT0idmVjLW9wLTIiPjxWZWN0b3JPcCBvcD0iY3Jvc3MiIHN0eWxlPSJ2ZWMtb3AtMiI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJOb3JtYWwiIC8+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+PC9WZWN0b3JPcD48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9Ik5vcm1hbCIgLz48L1ZlY3Rvck9wPjwvVmVjdG9yT3A+PC9DdXN0b21FeHByZXNzaW9uPg==
var Flatten = Vz.DeclareCustomExpression("Flatten", "Vector", "Normal").SetReturn((Vector, Normal) =>
{
    return Vz.Project(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+"), Vz.Cross(Vz.Cross(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJOb3JtYWwiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJWZWN0b3IiIC8+")), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJOb3JtYWwiIC8+")));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iTWF0Y2ggU3RyaW5nICgwKSAoMSkgIiBmb3JtYXQ9Ik1hdGNoIFN0cmluZyB8U3RyaW5nMXwgfFN0cmluZzJ8ICByZXR1cm4gKDApIiBuYW1lPSJNYXRjaCBTdHJpbmciIHN0eWxlPSJjdXN0b20tZXhwcmVzc2lvbiIgcG9zPSItMTM2MC4xNTEsNzk2LjU0MDUiPjxCb29sT3Agb3A9ImFuZCIgc3R5bGU9Im9wLWFuZCI+PFN0cmluZ09wIG9wPSJjb250YWlucyIgc3R5bGU9ImNvbnRhaW5zIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlN0cmluZzEiIC8+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJTdHJpbmcyIiAvPjwvU3RyaW5nT3A+PFN0cmluZ09wIG9wPSJjb250YWlucyIgc3R5bGU9ImNvbnRhaW5zIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlN0cmluZzIiIC8+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJTdHJpbmcxIiAvPjwvU3RyaW5nT3A+PC9Cb29sT3A+PC9DdXN0b21FeHByZXNzaW9uPg==
var Match_String = Vz.DeclareCustomExpression("Match String", "String1", "String2").SetReturn((String1, String2) =>
{
    return (Vz.Contains(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJTdHJpbmcxIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJTdHJpbmcyIiAvPg==")) && Vz.Contains(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJTdHJpbmcyIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJTdHJpbmcxIiAvPg==")));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iYXRhbmggKDApICIgZm9ybWF0PSJhdGFuaCB8dmFsdWV8ICByZXR1cm4gKDApIiBuYW1lPSJhdGFuaCIgc3R5bGU9ImN1c3RvbS1leHByZXNzaW9uIiBwb3M9Ii0xMzU5LjI2OSw4NjIuMzE1MSI+PEJpbmFyeU9wIG9wPSIvIiBzdHlsZT0ib3AtZGl2Ij48TWF0aEZ1bmN0aW9uIGZ1bmN0aW9uPSJsbiIgc3R5bGU9Im9wLW1hdGgiPjxCaW5hcnlPcCBvcD0iLyIgc3R5bGU9Im9wLWRpdiI+PEJpbmFyeU9wIG9wPSIrIiBzdHlsZT0ib3AtYWRkIj48Q29uc3RhbnQgdGV4dD0iMSIgLz48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9InZhbHVlIiAvPjwvQmluYXJ5T3A+PEJpbmFyeU9wIG9wPSItIiBzdHlsZT0ib3Atc3ViIj48Q29uc3RhbnQgdGV4dD0iMSIgLz48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9InZhbHVlIiAvPjwvQmluYXJ5T3A+PC9CaW5hcnlPcD48L01hdGhGdW5jdGlvbj48Q29uc3RhbnQgdGV4dD0iMiIgLz48L0JpbmFyeU9wPjwvQ3VzdG9tRXhwcmVzc2lvbj4=
var atanh = Vz.DeclareCustomExpression("atanh", "value").SetReturn((value) =>
{
    return (Vz.Ln(((Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+") + Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz4=")) / (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz4=")))) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iYXNpbmggKDApICIgZm9ybWF0PSJhc2luaCB8dmFsdWV8ICByZXR1cm4gKDApIiBuYW1lPSJhc2luaCIgc3R5bGU9ImN1c3RvbS1leHByZXNzaW9uIiBwb3M9Ii0xMzYwLjI1NSw5MjUuNTkxMSI+PE1hdGhGdW5jdGlvbiBmdW5jdGlvbj0ibG4iIHN0eWxlPSJvcC1tYXRoIj48QmluYXJ5T3Agb3A9IisiIHN0eWxlPSJvcC1hZGQiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0idmFsdWUiIC8+PE1hdGhGdW5jdGlvbiBmdW5jdGlvbj0ic3FydCIgc3R5bGU9Im9wLW1hdGgiPjxCaW5hcnlPcCBvcD0iKyIgc3R5bGU9Im9wLWFkZCI+PEJpbmFyeU9wIG9wPSJeIiBzdHlsZT0ib3AtZXhwIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9InZhbHVlIiAvPjxDb25zdGFudCB0ZXh0PSIyIiAvPjwvQmluYXJ5T3A+PENvbnN0YW50IHRleHQ9IjEiIC8+PC9CaW5hcnlPcD48L01hdGhGdW5jdGlvbj48L0JpbmFyeU9wPjwvTWF0aEZ1bmN0aW9uPjwvQ3VzdG9tRXhwcmVzc2lvbj4=
var asinh = Vz.DeclareCustomExpression("asinh", "value").SetReturn((value) =>
{
    return Vz.Ln((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz4=") + Vz.Sqrt(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz4=") ^ Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+")) + Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")))));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iYWNvc2ggKDApICIgZm9ybWF0PSJhY29zaCB8dmFsdWV8ICByZXR1cm4gKDApIiBuYW1lPSJhY29zaCIgc3R5bGU9ImN1c3RvbS1leHByZXNzaW9uIiBwb3M9Ii0xMzU3Ljk2Myw5OTAuMDEwMyI+PE1hdGhGdW5jdGlvbiBmdW5jdGlvbj0ibG4iIHN0eWxlPSJvcC1tYXRoIj48QmluYXJ5T3Agb3A9IisiIHN0eWxlPSJvcC1hZGQiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0idmFsdWUiIC8+PE1hdGhGdW5jdGlvbiBmdW5jdGlvbj0ic3FydCIgc3R5bGU9Im9wLW1hdGgiPjxCaW5hcnlPcCBvcD0iLSIgc3R5bGU9Im9wLXN1YiI+PEJpbmFyeU9wIG9wPSJeIiBzdHlsZT0ib3AtZXhwIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9InZhbHVlIiAvPjxDb25zdGFudCB0ZXh0PSIyIiAvPjwvQmluYXJ5T3A+PENvbnN0YW50IHRleHQ9IjEiIC8+PC9CaW5hcnlPcD48L01hdGhGdW5jdGlvbj48L0JpbmFyeU9wPjwvTWF0aEZ1bmN0aW9uPjwvQ3VzdG9tRXhwcmVzc2lvbj4=
var acosh = Vz.DeclareCustomExpression("acosh", "value").SetReturn((value) =>
{
    return Vz.Ln((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz4=") + Vz.Sqrt(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz4=") ^ Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+")) - Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")))));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0idGFuaCAoMCkgIiBmb3JtYXQ9InRhbmggfHZhbHVlfCAgcmV0dXJuICgwKSIgbmFtZT0idGFuaCIgc3R5bGU9ImN1c3RvbS1leHByZXNzaW9uIiBwb3M9Ii0xMzU3Ljc5NSwxMDU0Ljc4NyI+PEJpbmFyeU9wIG9wPSIvIiBzdHlsZT0ib3AtZGl2Ij48Q2FsbEN1c3RvbUV4cHJlc3Npb24gY2FsbD0ic2luaCIgc3R5bGU9ImNhbGwtY3VzdG9tLWV4cHJlc3Npb24iPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0idmFsdWUiIC8+PC9DYWxsQ3VzdG9tRXhwcmVzc2lvbj48Q2FsbEN1c3RvbUV4cHJlc3Npb24gY2FsbD0iY29zaCIgc3R5bGU9ImNhbGwtY3VzdG9tLWV4cHJlc3Npb24iPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0idmFsdWUiIC8+PC9DYWxsQ3VzdG9tRXhwcmVzc2lvbj48L0JpbmFyeU9wPjwvQ3VzdG9tRXhwcmVzc2lvbj4=
var tanh = Vz.DeclareCustomExpression("tanh", "value").SetReturn((value) =>
{
    return (sinh(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz4=")) / cosh(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz4=")));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0ic2luaCAoMCkgIiBmb3JtYXQ9InNpbmggfHZhbHVlfCAgcmV0dXJuICgwKSIgbmFtZT0ic2luaCIgc3R5bGU9ImN1c3RvbS1leHByZXNzaW9uIiBwb3M9Ii0xMzU4LjIxMSwxMTE2Ljc3MyI+PEJpbmFyeU9wIG9wPSIvIiBzdHlsZT0ib3AtZGl2Ij48QmluYXJ5T3Agb3A9Ii0iIHN0eWxlPSJvcC1zdWIiPjxCaW5hcnlPcCBvcD0iXiIgc3R5bGU9Im9wLWV4cCI+PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkUiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz48L0JpbmFyeU9wPjxCaW5hcnlPcCBvcD0iXiIgc3R5bGU9Im9wLWV4cCI+PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkUiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+PEJpbmFyeU9wIG9wPSIqIiBzdHlsZT0ib3AtbXVsIj48Q29uc3RhbnQgdGV4dD0iLTEiIC8+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz48L0JpbmFyeU9wPjwvQmluYXJ5T3A+PC9CaW5hcnlPcD48Q29uc3RhbnQgdGV4dD0iMiIgLz48L0JpbmFyeU9wPjwvQ3VzdG9tRXhwcmVzc2lvbj4=
var sinh = Vz.DeclareCustomExpression("sinh", "value").SetReturn((value) =>
{
    return (((Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkUiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+") ^ Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz4=")) - (Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkUiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+") ^ (Vz.RawConstant("PENvbnN0YW50IHRleHQ9Ii0xIiAvPg==") * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz4=")))) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iY29zaCAoMCkgIiBmb3JtYXQ9ImNvc2ggfHZhbHVlfCAgcmV0dXJuICgwKSIgbmFtZT0iY29zaCIgc3R5bGU9ImN1c3RvbS1leHByZXNzaW9uIiBwb3M9Ii0xMzU4LjUyNiwxMTgxLjY2MSI+PEJpbmFyeU9wIG9wPSIvIiBzdHlsZT0ib3AtZGl2Ij48QmluYXJ5T3Agb3A9IisiIHN0eWxlPSJvcC1hZGQiPjxCaW5hcnlPcCBvcD0iXiIgc3R5bGU9Im9wLWV4cCI+PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkUiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz48L0JpbmFyeU9wPjxCaW5hcnlPcCBvcD0iXiIgc3R5bGU9Im9wLWV4cCI+PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkUiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+PEJpbmFyeU9wIG9wPSIqIiBzdHlsZT0ib3AtbXVsIj48Q29uc3RhbnQgdGV4dD0iLTEiIC8+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz48L0JpbmFyeU9wPjwvQmluYXJ5T3A+PC9CaW5hcnlPcD48Q29uc3RhbnQgdGV4dD0iMiIgLz48L0JpbmFyeU9wPjwvQ3VzdG9tRXhwcmVzc2lvbj4=
var cosh = Vz.DeclareCustomExpression("cosh", "value").SetReturn((value) =>
{
    return (((Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkUiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+") ^ Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz4=")) + (Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkUiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+") ^ (Vz.RawConstant("PENvbnN0YW50IHRleHQ9Ii0xIiAvPg==") * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ2YWx1ZSIgLz4=")))) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+"));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iVHJ1ZSBBbm9tYWx5IGF0IFJhZGl1cyAoMCkgIiBmb3JtYXQ9IlRydWUgQW5vbWFseSBhdCBSYWRpdXMgfFJhZGl1c3wgIHJldHVybiAoMCkiIG5hbWU9IlRydWUgQW5vbWFseSBhdCBSYWRpdXMiIHN0eWxlPSJjdXN0b20tZXhwcmVzc2lvbiIgcG9zPSItMTM2MC4zMzYsNjA4LjQxNTUiPjxDb25kaXRpb25hbCBzdHlsZT0iY29uZGl0aW9uYWwiPjxCb29sT3Agb3A9ImFuZCIgc3R5bGU9Im9wLWFuZCI+PENvbXBhcmlzb24gb3A9ImciIHN0eWxlPSJvcC1ndCI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJSYWRpdXMiIC8+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQXBvYXBzaXMiIC8+PC9Db21wYXJpc29uPjxDb21wYXJpc29uIG9wPSJnIiBzdHlsZT0ib3AtZ3QiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9ImZhbHNlIiB2YXJpYWJsZU5hbWU9IkFwb2Fwc2lzIiAvPjxDb25zdGFudCB0ZXh0PSIwIiAvPjwvQ29tcGFyaXNvbj48L0Jvb2xPcD48RXZhbHVhdGVFeHByZXNzaW9uIHN0eWxlPSJldmFsdWF0ZS1leHByZXNzaW9uIj48Q29uc3RhbnQgdGV4dD0icGkiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+PENvbmRpdGlvbmFsIHN0eWxlPSJjb25kaXRpb25hbCI+PENvbXBhcmlzb24gb3A9ImwiIHN0eWxlPSJvcC1sdCI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJSYWRpdXMiIC8+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGVyaWFwc2lzIiAvPjwvQ29tcGFyaXNvbj48Q29uc3RhbnQgbnVtYmVyPSIwIiAvPjxNYXRoRnVuY3Rpb24gZnVuY3Rpb249ImFjb3MiIHN0eWxlPSJvcC1tYXRoIj48QmluYXJ5T3Agb3A9Ii8iIHN0eWxlPSJvcC1kaXYiPjxCaW5hcnlPcCBvcD0iLSIgc3R5bGU9Im9wLXN1YiI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU2VtaUxhdHVzUmVjdHVtIiAvPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iUmFkaXVzIiAvPjwvQmluYXJ5T3A+PEJpbmFyeU9wIG9wPSIqIiBzdHlsZT0ib3AtbXVsIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJFY2NlbnRyaWNpdHkiIC8+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJSYWRpdXMiIC8+PC9CaW5hcnlPcD48L0JpbmFyeU9wPjwvTWF0aEZ1bmN0aW9uPjwvQ29uZGl0aW9uYWw+PC9Db25kaXRpb25hbD48L0N1c3RvbUV4cHJlc3Npb24+
var True_Anomaly_at_Radius = Vz.DeclareCustomExpression("True Anomaly at Radius", "Radius").SetReturn((Radius) =>
{
    return (((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJSYWRpdXMiIC8+") > Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQXBvYXBzaXMiIC8+")) && (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iQXBvYXBzaXMiIC8+") > Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+"))) ? Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InBpIiAvPjwvRXZhbHVhdGVFeHByZXNzaW9uPg==") : ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJSYWRpdXMiIC8+") < Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iUGVyaWFwc2lzIiAvPg==")) ? 0 : Vz.Acos(((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iU2VtaUxhdHVzUmVjdHVtIiAvPg==") - Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJSYWRpdXMiIC8+")) / (Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iRWNjZW50cmljaXR5IiAvPg==") * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJSYWRpdXMiIC8+"))))));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iU2lnbmVkIEFuZ2xlICgwKSBYLUF4aXMgKDEpIFotQXhpcyAoMikgIiBmb3JtYXQ9IlNpZ25lZCBBbmdsZSB8UG9zaXRpb258IFgtQXhpcyB8WHwgWi1BeGlzIHxafCAgcmV0dXJuICgwKSIgbmFtZT0iU2lnbmVkIEFuZ2xlIiBzdHlsZT0iY3VzdG9tLWV4cHJlc3Npb24iIHBvcz0iLTEzNTkuMDE5LDY3Ni40MDE2Ij48QmluYXJ5T3Agb3A9ImF0YW4yIiBzdHlsZT0ib3AtYXRhbi0yIj48VmVjdG9yT3Agb3A9ImRvdCIgc3R5bGU9InZlYy1vcC0yIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlBvc2l0aW9uIiAvPjxWZWN0b3JPcCBvcD0ibm9ybSIgc3R5bGU9InZlYy1vcC0xIj48VmVjdG9yT3Agb3A9ImNyb3NzIiBzdHlsZT0idmVjLW9wLTIiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iWiIgLz48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlgiIC8+PC9WZWN0b3JPcD48L1ZlY3Rvck9wPjwvVmVjdG9yT3A+PFZlY3Rvck9wIG9wPSJkb3QiIHN0eWxlPSJ2ZWMtb3AtMiI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQb3NpdGlvbiIgLz48VmVjdG9yT3Agb3A9Im5vcm0iIHN0eWxlPSJ2ZWMtb3AtMSI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJYIiAvPjwvVmVjdG9yT3A+PC9WZWN0b3JPcD48L0JpbmFyeU9wPjwvQ3VzdG9tRXhwcmVzc2lvbj4=
var Signed_Angle = Vz.DeclareCustomExpression("Signed Angle", "Position", "X", "Z").SetReturn((Position, X, Z) =>
{
    return Vz.Atan2(Vz.Dot(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQb3NpdGlvbiIgLz4="), Vz.Norm(Vz.Cross(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJaIiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJYIiAvPg==")))), Vz.Dot(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJQb3NpdGlvbiIgLz4="), Vz.Norm(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJYIiAvPg=="))));
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iQ2xvY2sgKDApICIgZm9ybWF0PSJDbG9jayB8VGltZXwgIHJldHVybiAoMCkiIG5hbWU9IkNsb2NrIiBzdHlsZT0iY3VzdG9tLWV4cHJlc3Npb24iIHBvcz0iLTEzNTcuNDM1LDczLjcxNjU1Ij48U3RyaW5nT3Agb3A9ImZvcm1hdCIgc3R5bGU9ImZvcm1hdCI+PENvbnN0YW50IHRleHQ9InswfXsxfXsyOjAwfTp7MzowMH06ezQ6MDB9IiAvPjxDb25kaXRpb25hbCBzdHlsZT0iY29uZGl0aW9uYWwiPjxDb21wYXJpc29uIG9wPSJsIiBzdHlsZT0ib3AtbHQiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz48Q29uc3RhbnQgbnVtYmVyPSIwIiAvPjwvQ29tcGFyaXNvbj48Q29uc3RhbnQgdGV4dD0iLSAiIC8+PENvbnN0YW50IHRleHQ9IiIgLz48L0NvbmRpdGlvbmFsPjxDYWxsQ3VzdG9tRXhwcmVzc2lvbiBjYWxsPSJEYXkiIHN0eWxlPSJjYWxsLWN1c3RvbS1leHByZXNzaW9uIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9DYWxsQ3VzdG9tRXhwcmVzc2lvbj48QmluYXJ5T3Agb3A9IiUiIHN0eWxlPSJvcC1tb2QiPjxNYXRoRnVuY3Rpb24gZnVuY3Rpb249ImZsb29yIiBzdHlsZT0ib3AtbWF0aCI+PEJpbmFyeU9wIG9wPSIvIiBzdHlsZT0ib3AtZGl2Ij48TWF0aEZ1bmN0aW9uIGZ1bmN0aW9uPSJhYnMiIHN0eWxlPSJvcC1tYXRoIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9NYXRoRnVuY3Rpb24+PENvbnN0YW50IHRleHQ9IjM2MDAiIC8+PC9CaW5hcnlPcD48L01hdGhGdW5jdGlvbj48Q29uc3RhbnQgdGV4dD0iMjQiIC8+PC9CaW5hcnlPcD48QmluYXJ5T3Agb3A9IiUiIHN0eWxlPSJvcC1tb2QiPjxNYXRoRnVuY3Rpb24gZnVuY3Rpb249ImZsb29yIiBzdHlsZT0ib3AtbWF0aCI+PEJpbmFyeU9wIG9wPSIvIiBzdHlsZT0ib3AtZGl2Ij48TWF0aEZ1bmN0aW9uIGZ1bmN0aW9uPSJhYnMiIHN0eWxlPSJvcC1tYXRoIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9NYXRoRnVuY3Rpb24+PENvbnN0YW50IHRleHQ9IjYwIiAvPjwvQmluYXJ5T3A+PC9NYXRoRnVuY3Rpb24+PENvbnN0YW50IHRleHQ9IjYwIiAvPjwvQmluYXJ5T3A+PEJpbmFyeU9wIG9wPSIlIiBzdHlsZT0ib3AtbW9kIj48TWF0aEZ1bmN0aW9uIGZ1bmN0aW9uPSJmbG9vciIgc3R5bGU9Im9wLW1hdGgiPjxNYXRoRnVuY3Rpb24gZnVuY3Rpb249ImFicyIgc3R5bGU9Im9wLW1hdGgiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz48L01hdGhGdW5jdGlvbj48L01hdGhGdW5jdGlvbj48Q29uc3RhbnQgdGV4dD0iNjAiIC8+PC9CaW5hcnlPcD48Q29uc3RhbnQgdGV4dD0iIiAvPjwvU3RyaW5nT3A+PC9DdXN0b21FeHByZXNzaW9uPg==
var Clock = Vz.DeclareCustomExpression("Clock", "Time").SetReturn((Time) =>
{
    return Vz.Format("{0}{1}{2:00}:{3:00}:{4:00}", ((Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUaW1lIiAvPg==") < 0) ? "- " : ""), Day(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUaW1lIiAvPg==")), (Vz.Floor((Vz.Abs(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUaW1lIiAvPg==")) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjM2MDAiIC8+"))) % Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjI0IiAvPg==")), (Vz.Floor((Vz.Abs(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUaW1lIiAvPg==")) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjYwIiAvPg=="))) % Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjYwIiAvPg==")), (Vz.Floor(Vz.Abs(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUaW1lIiAvPg=="))) % Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjYwIiAvPg==")), "");
});

// VZBLOCK PEN1c3RvbUV4cHJlc3Npb24gY2FsbEZvcm1hdD0iRGF5ICgwKSAiIGZvcm1hdD0iRGF5IHxUaW1lfCAgcmV0dXJuICgwKSIgbmFtZT0iRGF5IiBzdHlsZT0iY3VzdG9tLWV4cHJlc3Npb24iIHBvcz0iLTEzNjAuNjM4LDE0NS4yNjA3Ij48Q29uZGl0aW9uYWwgc3R5bGU9ImNvbmRpdGlvbmFsIj48Q29tcGFyaXNvbiBvcD0iZyIgc3R5bGU9Im9wLWd0Ij48TWF0aEZ1bmN0aW9uIGZ1bmN0aW9uPSJmbG9vciIgc3R5bGU9Im9wLW1hdGgiPjxCaW5hcnlPcCBvcD0iLyIgc3R5bGU9Im9wLWRpdiI+PE1hdGhGdW5jdGlvbiBmdW5jdGlvbj0iYWJzIiBzdHlsZT0ib3AtbWF0aCI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUaW1lIiAvPjwvTWF0aEZ1bmN0aW9uPjxDb25zdGFudCB0ZXh0PSI4NjQwMCIgLz48L0JpbmFyeU9wPjwvTWF0aEZ1bmN0aW9uPjxDb25zdGFudCBudW1iZXI9IjAiIC8+PC9Db21wYXJpc29uPjxTdHJpbmdPcCBvcD0iam9pbiIgc3R5bGU9ImpvaW4iPjxNYXRoRnVuY3Rpb24gZnVuY3Rpb249ImZsb29yIiBzdHlsZT0ib3AtbWF0aCI+PEJpbmFyeU9wIG9wPSIvIiBzdHlsZT0ib3AtZGl2Ij48TWF0aEZ1bmN0aW9uIGZ1bmN0aW9uPSJhYnMiIHN0eWxlPSJvcC1tYXRoIj48VmFyaWFibGUgbGlzdD0iZmFsc2UiIGxvY2FsPSJ0cnVlIiB2YXJpYWJsZU5hbWU9IlRpbWUiIC8+PC9NYXRoRnVuY3Rpb24+PENvbnN0YW50IHRleHQ9Ijg2NDAwIiAvPjwvQmluYXJ5T3A+PC9NYXRoRnVuY3Rpb24+PENvbnN0YW50IHRleHQ9IiAiIC8+PENvbmRpdGlvbmFsIHN0eWxlPSJjb25kaXRpb25hbCI+PENvbXBhcmlzb24gb3A9Ij0iIHN0eWxlPSJvcC1lcSI+PE1hdGhGdW5jdGlvbiBmdW5jdGlvbj0iZmxvb3IiIHN0eWxlPSJvcC1tYXRoIj48QmluYXJ5T3Agb3A9Ii8iIHN0eWxlPSJvcC1kaXYiPjxNYXRoRnVuY3Rpb24gZnVuY3Rpb249ImFicyIgc3R5bGU9Im9wLW1hdGgiPjxWYXJpYWJsZSBsaXN0PSJmYWxzZSIgbG9jYWw9InRydWUiIHZhcmlhYmxlTmFtZT0iVGltZSIgLz48L01hdGhGdW5jdGlvbj48Q29uc3RhbnQgdGV4dD0iODY0MDAiIC8+PC9CaW5hcnlPcD48L01hdGhGdW5jdGlvbj48Q29uc3RhbnQgdGV4dD0iMSIgLz48L0NvbXBhcmlzb24+PENvbnN0YW50IHRleHQ9ImRheSAiIC8+PENvbnN0YW50IHRleHQ9ImRheXMgIiAvPjwvQ29uZGl0aW9uYWw+PENvbnN0YW50IHRleHQ9IiIgLz48L1N0cmluZ09wPjxDb25zdGFudCB0ZXh0PSIiIC8+PC9Db25kaXRpb25hbD48L0N1c3RvbUV4cHJlc3Npb24+
var Day = Vz.DeclareCustomExpression("Day", "Time").SetReturn((Time) =>
{
    return ((Vz.Floor((Vz.Abs(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUaW1lIiAvPg==")) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9Ijg2NDAwIiAvPg=="))) > 0) ? Vz.Join(Vz.Floor((Vz.Abs(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUaW1lIiAvPg==")) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9Ijg2NDAwIiAvPg=="))), " ", ((Vz.Floor((Vz.Abs(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJUaW1lIiAvPg==")) / Vz.RawConstant("PENvbnN0YW50IHRleHQ9Ijg2NDAwIiAvPg=="))) == Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")) ? "day " : "days "), "") : "");
});


// ── Serialize output ──
Console.WriteLine(Vz.context.currentProgram.Serialize().ToString());
