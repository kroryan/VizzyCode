using System;
using System.Linq;
using System.Collections.Generic;
using REWJUNO;
using REWVIZZY;

// ── Program: Altair Basic Function ──────────────────────────────────
Vz.Init("Altair Basic Function");

// ── Variables ────────────────────────────────────────
// var Door = -0;
// var Elevator = -0;
// var Seat = -0;
// var DoorState = -0;
// var ElevatorState = -0;
// var SeatState = -0;
// var LegState = -0;
// var Reentry = -0;
// var LegCap = -0;
// var Ladder = -0;

// VZPOS x=-10 y=-20
using (new OnStart())
{
    // Altair Alphard Basic Function
    // Automated Vizzy located on "Vizzy Here" Part
    // Initial Value
    Door = -0;
    Elevator = -0;
    Seat = -0;
    LegCap = -0;
    Ladder = -0;
    DoorState = false;
    ElevatorState = false;
    LegState = false;
    SeatState = false;
    Reentry = false;
    Vz.SetInput(CraftInput.Slider1, -0);
    Vz.SetInput(CraftInput.Slider2, 0);
    Solar_Panel(false);
    Parachute(false, false);
    Gyro(true);
    Gear(false);
    All_False();
    using (new While(true))
    {
        // Seat Configuration
        using (new If(((Vz.ActivationGroup(5) == true) && (SeatState == false))))
        {
            Seat = 1;
            Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("RotatorChair"), true);
            Elevator(true);
            Vz.WaitSeconds(2);
            Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Rotator1"), true);
            Vz.WaitSeconds(3);
            Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Elevator5"), true);
            Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Elevator6"), true);
            Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("ElevatorTable"), true);
            Vz.WaitSeconds(2);
            All_False();
            SeatState = true;
        }
        using (new ElseIf(((Vz.ActivationGroup(5) == false) && (SeatState == true))))
        {
            Seat = 0;
            Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Elevator5"), true);
            Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Elevator6"), true);
            Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("ElevatorTable"), true);
            Vz.WaitSeconds(2);
            Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("RotatorChair"), true);
            Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Rotator1"), true);
            Vz.WaitSeconds(3);
            Elevator(true);
            Vz.WaitSeconds(3);
            All_False();
            SeatState = false;
        }
        // Hatch Door
        using (new If(((Vz.ActivationGroup(6) == true) && (DoorState == false))))
        {
            Door = 1;
            Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("RotatorDoor"), true);
            Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Hatch Rotator"), true);
            Vz.WaitSeconds(2);
            All_False();
            DoorState = true;
        }
        using (new ElseIf(((Vz.ActivationGroup(6) == false) && (DoorState == true))))
        {
            Door = 0;
            Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("RotatorDoor"), true);
            Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Hatch Rotator"), true);
            Vz.WaitSeconds(2);
            All_False();
            DoorState = false;
        }
        // Ladder, only active when door & landing leg are open
        using (new If(((Vz.ActivationGroup(6) == true) && (Vz.ActivationGroup(9) == true))))
        {
            Ladder = 1;
        }
        using (new ElseIf(((Vz.ActivationGroup(6) == false) || (Vz.ActivationGroup(9) == false))))
        {
            Ladder = -0;
        }
        // Landing Leg
        using (new If(((Vz.ActivationGroup(9) == true) && (LegState == false))))
        {
            LegCap = 1;
            Landing_Leg(false, true);
            Vz.WaitSeconds(0.8);
            Vz.SetInput(CraftInput.Slider1, 1);
            Gear(true);
            Landing_Leg(true, true);
            Vz.WaitSeconds(1.5);
            All_False();
            Gear(false);
            LegState = true;
        }
        using (new ElseIf(((Vz.ActivationGroup(9) == false) && (LegState == true))))
        {
            Vz.SetInput(CraftInput.Slider1, -0);
            LegCap = -0;
            Gear(true);
            Vz.WaitSeconds(1);
            Landing_Leg(true, false);
            Vz.WaitSeconds(0.4);
            Landing_Leg(true, true);
            Vz.WaitSeconds(0.4);
            All_False();
            Gear(false);
            LegState = false;
            Gyro(true);
        }
        Lift_Elevator();
        // Gyro
        using (new If(((Vz.ActivationGroup(9) == true) && Vz.Craft.Grounded())))
        {
            Gyro(false);
        }
        // Parachute Auto Deploy
        using (new If(((Vz.Craft.AltitudeAGL() < 2000) && (Reentry == true))))
        {
            using (new If((Vz.Craft.AltitudeAGL() >= 1500)))
            {
                Parachute(true, false);
            }
            using (new ElseIf((Vz.Craft.AltitudeAGL() < 500)))
            {
                Parachute(true, true);
            }
            // Restart Parachute
            using (new If(Vz.Craft.Grounded()))
            {
                Parachute(false, false);
                Reentry = false;
            }
        }
        // Reentry Trigger
        using (new If(((Vz.RawXmlCraftProperty(@"<CraftProperty property=""Part.Temperature"" style=""part""><CraftProperty property=""Part.NameToID"" style=""part-id""><Constant text=""HeatShield"" /></CraftProperty></CraftProperty>") > 473) && (Vz.Craft.Velocity.VerticalSurface() < -0))))
        {
            Reentry = true;
        }
        // Door Button
        using (new If((((Vz.RawXmlCraftProperty(@"<CraftProperty property=""Part.Activated"" style=""part""><CraftProperty property=""Part.NameToID"" style=""part-id""><Constant text=""Door Button 2"" /></CraftProperty></CraftProperty>") == true) || (Vz.RawXmlCraftProperty(@"<CraftProperty property=""Part.Activated"" style=""part""><CraftProperty property=""Part.NameToID"" style=""part-id""><Constant text=""Door Button 1"" /></CraftProperty></CraftProperty>") == true)) && (DoorState == false))))
        {
            Vz.SetActivationGroup(6, true);
        }
        using (new If((((Vz.RawXmlCraftProperty(@"<CraftProperty property=""Part.Activated"" style=""part""><CraftProperty property=""Part.NameToID"" style=""part-id""><Constant text=""Door Button 2"" /></CraftProperty></CraftProperty>") == true) || (Vz.RawXmlCraftProperty(@"<CraftProperty property=""Part.Activated"" style=""part""><CraftProperty property=""Part.NameToID"" style=""part-id""><Constant text=""Door Button 1"" /></CraftProperty></CraftProperty>") == true)) && (DoorState == true))))
        {
            Vz.SetActivationGroup(6, false);
        }
        Vz.WaitSeconds(0.1);
    }
}

// ── Custom Instructions ──────────────────────────────
var Gear = Vz.DeclareCustomInstruction("Gear", "value").SetInstructions((value) =>
{
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear1"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear2"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear3"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear4"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear5"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear6"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear7"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear8"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear9"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear10"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear11"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear12"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear13"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear14"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear15"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear16"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear17"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear18"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear19"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gear20"), value);
});

var All_False = Vz.DeclareCustomInstruction("All False").SetInstructions(() =>
{
    Elevator(false);
    Landing_Leg(false, false);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Rotator1"), false);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("RotatorChair"), false);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Hatch Rotator"), false);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("ElevatorTable"), false);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Elevator5"), false);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Elevator6"), false);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("RotatorDoor"), false);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Lift1"), false);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Lift2"), false);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("RotatorToilet"), false);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Door Button 1"), false);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Door Button 2"), false);
});

var Elevator = Vz.DeclareCustomInstruction("Elevator", "value").SetInstructions((value) =>
{
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Elevator1"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Elevator2"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Elevator3"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Elevator4"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("RotatorBed1"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("RotatorBed2"), value);
});

var Lift_Elevator = Vz.DeclareCustomInstruction("Lift Elevator").SetInstructions(() =>
{
    // Lift Elevator
    using (new If(((Vz.ActivationGroup(4) == true) && (ElevatorState == false))))
    {
        Elevator = 1;
        Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("RotatorToilet"), true);
        Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Lift1"), true);
        Vz.WaitSeconds(0.5);
        Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Lift2"), true);
        Vz.WaitSeconds(2);
        All_False();
        ElevatorState = true;
    }
    using (new ElseIf(((Vz.ActivationGroup(4) == false) && (ElevatorState == true))))
    {
        Elevator = 0;
        Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("RotatorToilet"), true);
        Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Lift1"), true);
        Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Lift2"), true);
        Vz.WaitSeconds(2);
        All_False();
        ElevatorState = false;
    }
    // Solar Panel Auto Close or Open
    using (new If((Vz.Craft.AltitudeASL() >= 100000)))
    {
        Solar_Panel(true);
    }
    using (new ElseIf((Vz.Craft.AltitudeASL() < 100000)))
    {
        Solar_Panel(false);
    }
});

var Landing_Leg = Vz.DeclareCustomInstruction("Landing Leg", "value1", "value2").SetInstructions((value1, value2) =>
{
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Leg1"), value1);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Leg2"), value1);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Leg3"), value1);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Leg4"), value1);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Leg5"), value1);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Leg6"), value1);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Leg7"), value1);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Leg8"), value1);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("LegCap1"), value2);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("LegCap2"), value2);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("LegCap3"), value2);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("LegCap4"), value2);
});

var Solar_Panel = Vz.DeclareCustomInstruction("Solar Panel", "value").SetInstructions((value) =>
{
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("SolarPanel1"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("SolarPanel2"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("SolarPanel3"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("SolarPanel4"), value);
});

var Parachute = Vz.DeclareCustomInstruction("Parachute", "value1", "value2").SetInstructions((value1, value2) =>
{
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Parachute1"), value1);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Parachute2"), value2);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Parachute3"), value1);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Parachute4"), value2);
});

var Gyro = Vz.DeclareCustomInstruction("Gyro", "value").SetInstructions((value) =>
{
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gyro1"), value);
    Vz.SetPartProperty(PartPropertySetType.Activated, Vz.PartNameToID("Gyro2"), value);
});


// ── Serialize output ──
Console.WriteLine(Vz.context.currentProgram.Serialize().ToString());
