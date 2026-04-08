# Vizzy Authoring Guide

This guide defines how to write new Vizzy programs in code form so that `ConvertCodeToXml()` produces XML that Juno recognizes reliably.

## Goal

For new scripts written by hand, the target is:

`code -> XML recognized by Juno`

For existing Vizzy XML files, the stronger target remains:

`XML -> code -> XML` with byte-for-byte fidelity

Those are related, but they are not the same problem.

## Source Of Truth Rule

When debugging a failing mission, the first question is:

- is the source of truth the original XML
- or is the source of truth the current `.cs`

This distinction matters.

### XML Is Source Of Truth

If the mission began from a real Juno-exported Vizzy XML and the goal is exact fidelity, the safe target is:

`original XML -> code -> XML`

In that mode, the original XML structure is authoritative. The code is only an editable representation of that XML.

### Code Is Source Of Truth

If the `.cs` has been manually rewritten beyond the preserved imported structure, then the target changes to:

`current code -> new Juno-compatible XML`

In that mode, the exporter is no longer reproducing the original mission graph exactly. It is authoring a new graph from code.

This is where most hard failures happen.

## Preserved Metadata Rule

Imported programs preserve exact XML structure through metadata comments such as:

- `// VZTOPBLOCK`
- `// VZBLOCK ...`
- `// VZEL ...`

These comments are not noise. They are fidelity anchors.

If a block still has its preserved `VZEL` / `VZBLOCK` metadata, VizzyCode can often reconstruct the original XML exactly.

If a block is manually rewritten and that metadata disappears, VizzyCode must rebuild the block from authoring code instead of preserving the imported node.

That is the key line between:

- exact round-trip preservation
- best-effort code authoring export

## What Breaks Exact Fidelity

A mission is no longer a strict round-trip candidate when one or more of these happen:

1. a preserved imported block is replaced by handwritten code without its `VZEL` metadata
2. a new top-level `CustomInstruction` is added manually
3. a new top-level `CustomExpression` is added manually
4. large orbital math sections are rewritten from raw imported forms into cleaner handwritten helper code
5. control-flow structure changes enough that the original imported block graph no longer exists

At that point, the generated XML may still be valid, but it is no longer expected to match the original XML structure.

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

## VS Code Workflow

If you prefer writing Vizzy scripts in VS Code instead of the WinForms editor, use the repository integration:

- `VizzyCode.Cli`
- `vscode-extension`
- `scripts/install-vscode-integration.ps1`

That workflow lets you:

- import XML to `.vizzy.cs`
- edit the code in VS Code
- export the code back to XML
- run round-trip checks without leaving VS Code

Important implementation notes:

- the VS Code extension is distributed as a self-contained bundle
- the bundled CLI lives inside the extension folder
- the supported install path is a generated `.vsix`, not an ad-hoc copied folder
- the extension declares support for Workspace Trust / Restricted Mode so its commands remain available in normal restricted sessions

Activation indicators in VS Code:

- a `VizzyCode` status bar item when the extension is active
- Command Palette entries containing `VizzyCode`
- context menu actions on `.xml` and `.cs` files

Build and packaging instructions for the desktop app, CLI, and VS Code extension are documented in:

- `README.md`
- `vscode-extension/README.md`

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

## How To Decide Whether The Problem Is In The Code Or The Exporter

Use this exact test:

1. export the current `.cs` using the repo CLI
2. export the same `.cs` using the VS Code plugin CLI
3. compare the two XML outputs byte for byte
4. compare the current `.cs` against the imported reference `.cs` from the original XML

Interpretation:

- if repo CLI and plugin CLI produce different XML, the problem is tool drift
- if repo CLI and plugin CLI produce the same XML, the problem is not a plugin mismatch
- if the current `.cs` still preserves the original `VZEL` blocks in the failing region, the bug is likely exporter-side
- if the current `.cs` has handwritten replacements in the failing region, the bug may be in the code, the exporter, or both, but the mission is no longer pure round-trip work

For large mission files such as `T.T`, this distinction is mandatory.

## Special Warning For Large Missions

Mission-scale files such as `T.T` are not good candidates for blind AI cleanup.

Why:

- they mix preserved imported blocks with handwritten edits
- they often contain top-level custom instructions and custom expressions
- they rely on exact Juno block formatting in places that simple starter scripts do not

If an AI rewrites a large preserved region into cleaner-looking code, it may destroy the very metadata that allowed exact XML reconstruction.

## Safe Repair Strategy For AI

When asking an AI to repair a failing mission script, do not ask for a generic cleanup or rewrite.

Ask for this instead:

1. identify whether the failing region still has preserved `VZEL` metadata
2. if it does, preserve that region and repair the exporter
3. if it does not, treat that region as authoring-mode code and compare it against real working XML examples
4. isolate the first exact XML discrepancy that is unsupported by Juno
5. fix one discrepancy at a time and re-export

## Prompt To Give The AI

Use a prompt like this:

```text
Investigate this Vizzy mission without rewriting it blindly.

Goal:
- Determine whether the failure is caused by the current .cs source or by VizzyCode's exporter.
- If the failing region still preserves VZEL/VZBLOCK metadata, preserve it and fix the exporter.
- If the failing region was manually rewritten and no longer preserves metadata, treat it as authoring-mode code and compare the exported XML against real working XML examples from the repo.

Rules:
- Do not "clean up" or refactor the code unless that is proven necessary.
- Do not replace preserved imported blocks with handwritten equivalents.
- Compare the repo CLI export and the VS Code plugin export and confirm whether they are byte-identical.
- Find the first exact XML discrepancy that Juno likely rejects.
- Fix one exact structural problem at a time, then re-export and test again.

Important context:
- VZTOPBLOCK, VZBLOCK, and VZEL comments are fidelity metadata, not normal comments.
- If those metadata comments disappear from a region, that region is no longer guaranteed round-trip exact.
- Large mission files such as T.T must be handled region by region, not globally rewritten.
```

## What To Tell The AI About T.T-Like Cases

For files similar to `T.T`, add this context:

```text
This mission contains a mix of preserved imported Vizzy blocks and handwritten modifications.
Do not assume the original XML and the current .cs describe the same graph anymore.
First determine whether the failing section still has preserved VZEL metadata.
If it does not, do not describe the task as a pure round-trip failure.
Describe it as authoring-mode export of a partially rewritten mission.
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
