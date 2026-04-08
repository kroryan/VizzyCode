# Vizzy Authoring Guide

This guide defines how to write new Vizzy programs in code form so that `ConvertCodeToXml()` produces XML that Juno recognizes reliably.

## Goal

For new scripts written by hand, the target is:

`code -> XML recognized by Juno`

For existing Vizzy XML files, the stronger target remains:

`XML -> code -> XML` with byte-for-byte fidelity

Those are related, but they are not the same problem.

## Safe Starting Point

Use a known-good starter instead of writing a program from scratch blind.

Current recommended starters:

- `Vizzy examples/orbiting maybe.xml`
- `Vizzy examples/orbiting maybe starter.cs`
- `Vizzy examples/Auto Orbit authoring-safe.cs`

Mission-scale validated references:

- `Vizzy examples/T.T.cs`
- `Vizzy examples/T.T. Mission Program.xml`

`T.T` is important because it exercises real-world patterns that simple starter scripts do not:

- nested orbital math
- nested `Vz.Planet(...).Property()` expressions
- formatted string output
- `friendly` time and distance formatting
- mission-scale control flow

## Authoring Rules

Follow these rules when writing new Vizzy code:

1. Keep documentation and explanatory comments in English.
2. Prefer patterns already emitted by `ConvertProgramToCode()`.
3. When possible, start from a known-good exported example and modify it incrementally.
4. After writing a new script, always export and verify that Juno can open it.
5. If a script is intended as a reusable template, verify it with the round-trip test harness before publishing it as an example.
6. When strings intentionally include leading or trailing spaces, preserve them exactly.
7. Prefer converter-emitted string formatting patterns over inventing new ones.
8. If a mission uses deep orbital math or nested planet expressions, verify it through `TestRT`.

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
- `Vz.Log(message)`

## Manual Aliases

The converter supports these manual aliases:

- `Vz.Wait(seconds)`
- `Vz.WaitUntil(condition)`
- `Vz.SetAutopilotMode(mode)`
- `Vz.LockHeading(heading, pitch)`

They are expanded during `code -> XML` into canonical Vizzy instructions:

- `Vz.Wait(seconds)` -> `WaitSeconds(...)`
- `Vz.WaitUntil(condition)` -> `WaitUntil(...)`
- `Vz.SetAutopilotMode(mode)` -> `LockNavSphere(...)`
- `Vz.LockHeading(heading, pitch)` -> heading target plus pitch target

The converter also supports these lizpy-inspired advanced math aliases in expressions:

- `Vz.Exp(x)`
- `Vz.Sinh(x)`
- `Vz.Cosh(x)`
- `Vz.Tanh(x)`
- `Vz.Asinh(x)`
- `Vz.Acosh(x)`
- `Vz.Atanh(x)`

Additional lizpy-aligned aliases supported in authoring code:

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

## Fidelity-Sensitive Patterns Now Supported

These patterns matter because they already appeared in real mission files:

- `Vz.StringOp("friendly", value, "")`
- nested `Vz.Planet(...).Property()` calls inside arithmetic
- `Vz.Log(...)` emitted from real `LogMessage` blocks
- `Vz.Display(...)` emitted from `DisplayMessage` blocks
- string constants with significant trailing spaces

If you are writing a script that uses these patterns, use the canonical forms that the converter already understands.

## Patterns To Avoid In New Handwritten Scripts

Avoid these unless they have been explicitly verified in the converter:

- ad-hoc helper syntax that does not appear in converter output
- alternative aliases for supported instructions when the canonical emitted form already exists
- custom string helpers replacing `Vz.StringOp("friendly", ...)`
- stripping spaces from display or log strings that are intended to be part of the final output
- replacing preserved raw expressions with "cleaner" code unless the resulting XML has been verified in Juno

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

Recommended repository command:

```powershell
dotnet run --project TestRT -c Release -- "<input.xml>" "<output.xml>" "<code.txt>"
```

Example:

```powershell
dotnet run --project TestRT -c Release -- "Vizzy examples\T.T. Mission Program.xml" "_tt_rt_output.xml" "_tt_rt_code.txt"
```

Then compare the original and generated XML:

```powershell
git diff --no-index -- "<input.xml>" "<output.xml>"
```

## Troubleshooting

If Juno does not recognize the generated XML:

1. Search the XML for `[TODO]` comments.
2. Compare the generated code with converter-emitted code from a known-good Vizzy example.
3. Replace unsupported instruction aliases with the canonical emitted form.
4. Re-export and test again.

If the XML looks structurally correct but Juno still rejects it:

1. Compare against the original XML, not just the code.
2. Check for lost whitespace in `Constant text="..."`.
3. Check `StringOp friendly` nodes for the correct `subOp`.
4. Check message blocks such as `LogMessage` and `DisplayMessage`.
5. Check nested `Planet` or `CraftProperty` expressions inside arithmetic expressions.
6. Check whether a raw variable or raw craft property was replaced by a normal variable reference.

If the generated XML contains comments like:

```xml
<Constant text="[TODO] ..." />
```

the converter failed to map some instruction, and the script should be considered invalid until that mapping is fixed.

## Real-World Pitfalls Already Seen In This Repository

### Friendly String Formatting

`Vz.StringOp("friendly", value, "")` is valid and important.

The converter must reconstruct:

- `subOp="time"` for time-like values
- `subOp="distance"` for distance-like values

Examples:

- encounter countdowns
- reentry timers
- landing distance readouts
- burn start times

### Nested Planet Property Expressions

Patterns like this are valid and used in real mission scripts:

```csharp
Vz.Planet(...).Mass() / Vz.Planet(...).Mass()
```

They must become proper nested `Planet` expressions inside `BinaryOp` nodes, not fallback variable names.

### String Whitespace Fidelity

Strings such as:

- `"Launch Time: "`
- `"Enter 1 - "`
- `" to choose a target location"`

must preserve whitespace exactly. Trimming text literals changes the XML and breaks fidelity-sensitive round-trips.

### Log vs LogMessage

The code form often uses `Vz.Log(...)`, but the original XML may use `LogMessage`.

For fidelity-sensitive work, these must round-trip without losing the original XML instruction identity.

## Maintenance Rule For Future Agents

When adding new authoring examples:

- prefer examples derived from real working Vizzy XML
- verify they load in Juno
- keep the documentation in English
- document unsupported syntax explicitly in this guide
- when fixing a real mission file, add the discovered pattern here if it changes what users should safely write
