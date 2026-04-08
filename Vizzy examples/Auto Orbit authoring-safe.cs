using System;
using System.Linq;
using System.Collections.Generic;
using REWJUNO;
using REWVIZZY;

// ── Program: Auto Orbit Authoring Safe ──────────────────────────────────
Vz.Init("Auto Orbit Authoring Safe");

// VZTOPBLOCK
using (new OnStart())
{
    Vz.LockNavSphere(LockNavSphereIndicatorType.Current, 0);
    Vz.Display("Launching", 7);
    Vz.ActivateStage();
    Vz.SetInput(CraftInput.Throttle, 1);

    using (new WaitUntil((Vz.Craft.AltitudeASL() > 8000))) { }

    Vz.Display("Gravity turn start", 7);
    Vz.SetTargetHeading(TargetHeadingProperty.Pitch, 45);

    using (new WaitUntil((Vz.Craft.Orbit.Apoapsis() > 120000))) { }

    Vz.Display("Target apoapsis reached", 7);
    Vz.SetInput(CraftInput.Throttle, 0);
    Vz.LockNavSphere(LockNavSphereIndicatorType.Prograde, 0);

    using (new WaitUntil((Vz.Craft.Orbit.TimeToAp() < 30))) { }

    Vz.Display("Circularization burn", 7);
    Vz.SetInput(CraftInput.Throttle, 1);

    using (new WaitUntil((Vz.Craft.Orbit.Periapsis() > 100000))) { }

    Vz.SetInput(CraftInput.Throttle, 0);
    Vz.Display("Orbit achieved", 7);
}

// ── Serialize output ──
Console.WriteLine(Vz.context.currentProgram.Serialize().ToString());
