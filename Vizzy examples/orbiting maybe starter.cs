using System;
using System.Linq;
using System.Collections.Generic;
using REWJUNO;
using REWVIZZY;

// ── Program: orbiting maybe ──────────────────────────────────
Vz.Init("orbiting maybe");

// VZTOPBLOCK
using (new OnStart())
{
    Vz.LockNavSphere(LockNavSphereIndicatorType.Current, 0);
    Vz.Display("Launching", 7);
    Vz.ActivateStage();
    Vz.SetInput(CraftInput.Throttle, 1);

    using (new WaitUntil((Vz.Craft.AltitudeASL() > 8000))) { }

    Vz.Display("Gravity turn to 8km", 7);
    Vz.SetTargetHeading(TargetHeadingProperty.Pitch, 45);
    Vz.Display("Pitch set to 45", 7);

    using (new WaitUntil((Vz.Craft.Orbit.Apoapsis() > 120000))) { }

    Vz.Display("apoapsis reached, engines off", 7);
    Vz.SetInput(CraftInput.Throttle, 0);
    Vz.LockNavSphere(LockNavSphereIndicatorType.Prograde, 0);
    Vz.Display("Oriented to Prograde", 7);

    using (new WaitUntil((Vz.Craft.Orbit.TimeToAp() < 30))) { }

    Vz.Display("preparing orbital insertion", 7);
    Vz.SetInput(CraftInput.Throttle, 1);

    using (new WaitUntil((Vz.Craft.Orbit.Periapsis() > 100000))) { }

    Vz.SetInput(CraftInput.Throttle, 0);
    Vz.Display("orbit achieved", 7);
}

// ── Serialize output ──
Console.WriteLine(Vz.context.currentProgram.Serialize().ToString());
