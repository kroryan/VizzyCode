# Vizzy Authoring Guide

This guide defines how to write new Vizzy programs in code form so that `ConvertCodeToXml()` produces XML that Juno recognizes reliably.

## Goal

For new scripts written by hand, the target is:

`code -> XML recognized by Juno`

For existing Vizzy XML files, the stronger target remains:

`XML -> code -> XML` with byte-for-byte fidelity

Those are related, but they are not the same problem. A script that looks reasonable in code can still generate XML that Juno refuses to load if the code uses patterns the converter does not map correctly.

## Safe Starting Point

Use a known-good starter instead of writing a program from scratch blind.

Current recommended starter:

- `Vizzy examples/orbiting maybe.xml`
- `Vizzy examples/orbiting maybe starter.cs`

That starter is based on a real Vizzy XML program that round-trips exactly.

## Authoring Rules

Follow these rules when writing new Vizzy code:

1. Keep documentation and explanatory comments in English.
2. Prefer patterns already emitted by `ConvertProgramToCode()`.
3. When possible, start from a known-good exported example and modify it incrementally.
4. After writing a new script, always export and verify that Juno can open it.
5. If a script is intended as a reusable template, verify it with the round-trip test harness before publishing it as an example.

## Safe Instruction Patterns

These patterns are known-good and are preferred for new code:

- `using (new OnStart())`
- `Vz.ActivateStage()`
- `Vz.SetInput(CraftInput.Throttle, value)`
- `Vz.Display(message, duration)`
- `Vz.WaitSeconds(value)`
- `Vz.Wait(value)`
- `Vz.WaitUntil(condition)`
- `using (new WaitUntil(condition)) { }`
- `Vz.LockNavSphere(LockNavSphereIndicatorType.Current, 0)`
- `Vz.LockNavSphere(LockNavSphereIndicatorType.Prograde, 0)`
- `Vz.SetAutopilotMode(LockNavSphereIndicatorType.Current)`
- `Vz.SetAutopilotMode(LockNavSphereIndicatorType.Prograde)`
- `Vz.SetTargetHeading(TargetHeadingProperty.Pitch, value)`
- `Vz.SetHeading(value)`
- `Vz.SetPitch(value)`

## Manual Aliases

The converter now supports these manual aliases:

- `Vz.Wait(seconds)`
- `Vz.WaitUntil(condition)`
- `Vz.SetAutopilotMode(mode)`
- `Vz.LockHeading(heading, pitch)`

They are expanded during `code -> XML` into canonical Vizzy instructions:

- `Vz.Wait(seconds)` -> `WaitSeconds(...)`
- `Vz.WaitUntil(condition)` -> `WaitUntil(...)`
- `Vz.SetAutopilotMode(mode)` -> `LockNavSphere(...)`
- `SetTargetHeading(property="heading", ...)`
- `SetTargetHeading(property="pitch", ...)`

The converter also supports these lizpy-inspired advanced math aliases in expressions:

- `Vz.Exp(x)`
- `Vz.Sinh(x)`
- `Vz.Cosh(x)`
- `Vz.Tanh(x)`
- `Vz.Asinh(x)`
- `Vz.Acosh(x)`
- `Vz.Atanh(x)`

These are expanded into standard Vizzy expression trees. The canonical emitted form is still the explicit low-level one.

Additional lizpy-aligned authoring aliases now supported:

- `Vz.Concat(a, b, ...)`
- `Vz.StringLength(text)`
- `Vz.SubString(text, start)`
- `Vz.ListCreate(csvText)`
- `Vz.ListRemoveValue(list, item)`
- `Vz.Normalize(vector)`
- `Vz.X(vector)`
- `Vz.Y(vector)`
- `Vz.Z(vector)`
- `Vz.PartLocalToPci(partId, localVector)`

## Patterns To Avoid In New Hand-Written Scripts

Avoid these unless they have been explicitly verified in the converter:

- ad-hoc helper syntax that does not appear in converter output
- alternative aliases for supported instructions when the canonical emitted form already exists

Reason:

Unsupported aliases can degrade into `[TODO]` comments during `code -> XML`, which makes the generated XML invalid as an actual flight script.

## Preferred Control Strategy

For new orbital scripts, prefer:

- `LockNavSphere(Prograde)` for prograde hold
- `SetTargetHeading(TargetHeadingProperty.Pitch, value)` for simple pitch programs
- `WaitUntil(...)` for milestone-driven flow

This is safer than inventing custom control patterns that the converter may not parse.

## Minimal Safe Orbit Structure

A small orbit-capable script should usually follow this order:

1. `OnStart`
2. initial orientation or nav lock
3. launch message
4. stage activation
5. throttle on
6. wait until ascent milestone
7. pitch program
8. wait until target apoapsis
9. throttle off
10. orient prograde
11. wait until near apoapsis
12. throttle on
13. wait until periapsis target
14. throttle off
15. final confirmation message

## Validation Workflow

When creating a new script template:

1. Write the code using the safe subset.
2. Export to XML.
3. Open the XML in Juno and confirm it is recognized as a Vizzy program.
4. If the script came from an XML template, run the round-trip harness too.

Recommended local harness:

```powershell
TestRT\bin\Release\net9.0-windows\TestRT.exe "<input.xml>" "<output.xml>" "<code.txt>"
```

## Troubleshooting

If Juno does not recognize the generated XML:

1. Search the XML for `[TODO]` comments.
2. Compare the generated code with converter-emitted code from a known-good Vizzy example.
3. Replace unsupported instruction aliases with the canonical emitted form.
4. Re-export and test again.

If the generated XML contains comments like:

```xml
<Constant text="[TODO] ..." />
```

the converter failed to map some instruction, and the script should be considered invalid until that mapping is fixed.

## Maintenance Rule For Future Agents

When adding new authoring examples:

- prefer examples derived from real working Vizzy XML
- verify they load in Juno
- keep the documentation in English
- document any unsupported syntax explicitly in this guide
