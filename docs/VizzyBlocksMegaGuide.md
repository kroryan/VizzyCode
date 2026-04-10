# Vizzy Blocks Mega Guide

This document is the long-form reference for writing, reading, and maintaining Vizzy programs in this repository.

It is written from the perspective of this project. The goal is practical:

- explain what each supported block family does
- show how it appears in XML and in C#-style authoring code
- document the safe authoring subset for `code -> XML`
- document pitfalls that can make Juno reject a generated program
- document fidelity-sensitive behavior that matters in real mission files

If you are new to this project, read these files in this order:

1. `README.md`
2. `docs/VizzyAuthoringGuide.md`
3. `docs/RawPreservationGuide.md`
4. `docs/ExportValidationAndCoverageGuide.md`
5. this file

## Scope

This guide covers the block families currently supported by `VizzyXmlConverter.cs`, especially:

- instruction blocks
- expression blocks
- custom instructions
- custom expressions
- MFD widgets and widget interaction
- authoring patterns that round-trip reliably

It does not claim to be an exhaustive reverse-engineered reference for every block Juno has ever shipped. It is the supported reference for this repository.

That repository support now also includes:

- export-time structural validation
- repository-wide example verification
- clean-view imports plus metadata sidecars

## Four Layers You Can See In Code

When reading generated `.vizzy.cs`, you may see four different layers mixed together:

1. high-level authoring syntax
2. preserved metadata comments
3. readable raw XML preservation
4. manual layout hints

They are not the same thing.

### High-level authoring syntax

This is the preferred layer for normal handwritten scripts.

### Preserved metadata comments

- `VZTOPBLOCK`
- `VZBLOCK`
- `VZEL`

These preserve imported graph identity.

### Readable raw XML preservation

Examples:

- `Vz.RawXmlConstant(...)`
- `Vz.RawXmlVariable(...)`
- `Vz.RawXmlCraftProperty(...)`
- `Vz.RawXmlEval(...)`

These preserve exact XML fragments while remaining readable.

### Manual layout hints

Example:

```csharp
// VZPOS x=1200 y=-300
```

These affect top-level layout in exported XML for authored blocks.

## Mental Model

Vizzy programs in this project move between three forms:

1. Vizzy XML
2. C#-style Vizzy authoring code
3. round-tripped Vizzy XML

There are two different success criteria.

### Existing Program Fidelity

For an existing Vizzy program exported from Juno:

`XML -> code -> XML`

must preserve the exact structure that Juno expects, ideally byte-for-byte.

### New Script Authoring

For a new script written by hand:

`code -> XML`

must generate XML that Juno recognizes as a valid Vizzy program.

These goals overlap, but they are not the same.

## Fidelity Boundary

This repository has a hard boundary that future maintainers and AI agents must understand.

### Preserved Imported Blocks

When code came from real Vizzy XML and still retains metadata comments like:

- `VZTOPBLOCK`
- `VZBLOCK`
- `VZEL`

the converter can often preserve the original XML block identity and structure exactly.

### Reauthored Blocks

When a region is rewritten manually and those metadata comments no longer anchor the original nodes, the converter must build XML from authoring code patterns instead.

That is not the same task.

At that point, the block is no longer a strict round-trip block. It becomes a newly authored Vizzy graph.

Readable `RawXml*` does not automatically mean a block is fully reauthored. It may still mark an exact imported fidelity fragment inside an otherwise readable code region.

### Why This Matters

A large mission may contain both kinds of regions at the same time:

- preserved imported regions
- manually rewritten authoring regions

That mixed state is especially common in mission-scale files such as `T.T`.

When such a mission fails to load in Juno, the correct question is not just:

- "what differs from the original XML"

It is also:

- "which part of this mission is still preserved imported structure, and which part is now handwritten authoring code"

and sometimes:

- "which exact expressions must remain raw-preserved even inside an edited region"

## Top-Level Custom Blocks

Top-level custom blocks are especially sensitive in Juno.

### Custom Instruction Headers

A top-level custom instruction is represented as:

- one top-level `<Instructions>` block
- first child is a self-closing `<CustomInstruction ... />`
- body instructions follow in the same top-level `<Instructions>` block

The header must match Juno's conventions exactly, including fields such as:

- `callFormat`
- `format`
- `name`
- `style`
- `id`
- `pos`

For authoring-created custom instructions, spacing inside `callFormat` and `format` matters. Use the same shape emitted by working Juno XML.

### Custom Expression Headers

Top-level custom expressions live under `<Expressions>`.

For authoring-created custom expressions, ensure:

- they are direct children of `<Expressions>`
- they carry a valid `pos`
- their `callFormat`, `format`, `name`, and `style` match Juno's established pattern

Missing `pos` on a new top-level custom expression is a real structural failure, not just a cosmetic difference.

Top-level layout can now be guided manually with the compact form actually supported by VizzyCode:

```csharp
// VZPOS x=1200 y=-300
```

## Program Structure

A Vizzy program normally has:

- one `<Program>` root
- one `<Variables>` section
- one or more `<Instructions>` blocks
- one `<Expressions>` section

Top-level `Instructions` blocks usually start with one of:

- an `Event`
- a `CustomInstruction`
- or a preamble block for free-standing instructions

For authoring new scripts, the simplest safe entry point is:

```csharp
Vz.Init("Program Name");

using (new OnStart())
{
    // instructions...
}

Console.WriteLine(Vz.context.currentProgram.Serialize().ToString());
```

For fidelity work on existing XML, also use the round-trip harness:

```powershell
dotnet run --project TestRT -c Release -- "<input.xml>" "<output.xml>" "<code.txt>"
```

For broader repository export coverage, also use:

```powershell
dotnet run --project VizzyCode.csproj -c Release -- --verify-vizzy
```

## Diagnostic Method For Failing Mission Exports

When a mission export does not appear in the Vizzy editor:

1. compare the exported XML against working non-exported examples in `Vizzy examples`
2. identify whether the failing region corresponds to preserved imported blocks or handwritten code
3. compare repo CLI export and VS Code plugin export to rule out tool drift
4. inspect top-level custom blocks first
5. inspect control-flow structural encoding second
6. inspect raw-preserved fragments third
7. inspect the export validator result before inventing a new theory

Why this order:

- top-level block shape can make Juno reject the whole program immediately
- control-flow shape errors can make individual regions invalid
- formatting differences in nested expressions usually matter after the top-level graph is already valid

The validator currently catches some of these problems automatically, but not every future bad XML shape will already be encoded there. That is why comparison against working examples still matters.

When raw-preserved fragments are present, prefer understanding them first instead of rewriting them from appearance alone.

## T.T-Class Mission Warning

`T.T` is a reference case for why mission-scale files need stricter handling.

The current `T.T` work showed a real mixed-state scenario:

- some regions still preserved imported Vizzy metadata
- other regions had been manually rewritten into new authoring code
- new top-level custom instruction and custom expression blocks were introduced

That means a failure in `T.T` is not automatically "the exporter broke a round-trip".

It may be:

- a broken exporter for a handwritten authoring pattern
- a structurally invalid handwritten block
- or a mix of both

## Block Families

The easiest way to understand Vizzy is to group blocks by role:

- flow blocks control execution order
- action blocks change the craft, UI, or program state
- expression blocks compute values
- custom blocks package reusable logic

The sections below follow that structure.

## Event Blocks

Event blocks start execution threads. They are the roots of most top-level `Instructions` columns in Vizzy.

### Flight Start

Purpose:

- run code when the program starts in flight

Typical XML:

```xml
<Event event="FlightStart" style="flight-start" />
```

Typical code:

```csharp
using (new OnStart())
{
    // body
}
```

### Other Events

This converter also supports several other event forms in existing XML:

- flight end
- receive message
- docked
- change SOI
- part collision
- part explode

## Comment Block

Purpose:

- store human-readable notes in the Vizzy graph

Typical XML:

```xml
<Comment style="comment">
  <Constant style="comment-text" canReplace="false" text="My note" />
</Comment>
```

Typical authoring code:

```csharp
// This is a note
```

## Variable Blocks

### Set Variable

Purpose:

- assign a value to a variable

Typical XML:

```xml
<SetVariable style="set-variable">
  <Variable list="false" local="false" variableName="Pitch" />
  <Constant number="45" />
</SetVariable>
```

Typical authoring code:

```csharp
Pitch = 45;
```

### Change Variable

Purpose:

- increment or add to a variable

Typical authoring code:

```csharp
counter += 1;
```

### Variable References

Variable references appear inside expressions.

There are several important distinctions:

- global variables
- local variables
- list variables

For fidelity-sensitive cases, this converter can preserve raw variable metadata exactly.

## Flow Control Blocks

### While

Purpose:

- repeat while a condition is true

Typical code:

```csharp
using (new While(condition))
{
    // repeated instructions
}
```

### If / ElseIf / Else

Purpose:

- conditional branching

Typical code:

```csharp
using (new If(conditionA))
{
}
using (new ElseIf(conditionB))
{
}
using (new Else())
{
}
```

### For

Purpose:

- run a loop with an iterator variable and explicit start, end, and step

Important note:

`For` blocks can carry local-variable metadata that matters for fidelity. This project preserves that carefully for XML round-trips.

### Wait Seconds

Purpose:

- yield execution for a time interval

Typical code:

```csharp
Vz.WaitSeconds(0.1);
```

### Wait Until

Purpose:

- pause until a condition becomes true

Typical code:

```csharp
using (new WaitUntil(condition)) { }
```

This is one of the safest blocks for handwritten scripts.

## Output and Logging Blocks

### Display / DisplayMessage

Purpose:

- show a user-facing message on screen for a duration

Typical code:

```csharp
Vz.Display("Launching", 7);
```

Fidelity note:

- real XML often uses `DisplayMessage`
- code form may still appear as `Vz.Display(...)`
- round-trip work must preserve the original XML instruction identity

### Log / LogMessage

Purpose:

- write a simple log line

Typical code:

```csharp
Vz.Log("Entering coast phase");
```

Fidelity note:

- real XML may use `LogMessage`
- code form may still appear as `Vz.Log(...)`
- the converter now preserves `LogMessage` correctly during round-trip

## Craft Control Blocks

### Activate Stage

Purpose:

- trigger staging

Typical code:

```csharp
Vz.ActivateStage();
```

### Set Input

Purpose:

- write a raw craft input axis or toggle

Typical code:

```csharp
Vz.SetInput(CraftInput.Throttle, 1);
```

### Set Target Heading

Purpose:

- command a heading-related autopilot property

Typical code:

```csharp
Vz.SetTargetHeading(TargetHeadingProperty.Pitch, 45);
Vz.SetTargetHeading(TargetHeadingProperty.Heading, 90);
```

### Lock Heading Alias

This repository supports the manual authoring alias:

```csharp
Vz.LockHeading(90, 45);
```

During `code -> XML`, it is expanded to heading and pitch target instructions.

### Lock Nav Sphere

Purpose:

- lock orientation to a nav indicator such as `Current` or `Prograde`

Typical code:

```csharp
Vz.LockNavSphere(LockNavSphereIndicatorType.Current, 0);
Vz.LockNavSphere(LockNavSphereIndicatorType.Prograde, 0);
```

### Set Activation Group

Purpose:

- toggle action groups

### Set Part Property

Purpose:

- change a part-level property

This family is important for craft automation and can be structurally sensitive.

## User Input Block

Purpose:

- ask the player for a value

This block has fidelity-sensitive attribute ordering in some real examples, so the converter preserves it carefully.

## Broadcasting and Messaging Blocks

### BroadcastMessage

Purpose:

- send data or messages locally or across craft scopes depending on flags

### ReceiveMessage Event

Purpose:

- start a handler thread when a message arrives

## List Blocks

Lists are useful for sequences, lookup tables, and lightweight storage.

### Mutating List Blocks

Supported list instruction families include:

- add
- insert
- remove
- set
- clear
- sort
- reverse

### List Expressions

Supported read-oriented list expressions include:

- get item
- list length
- index lookup
- inline list creation

## Custom Instruction Blocks

Purpose:

- package reusable instruction sequences

Examples:

- gear deployment
- solar panel deployment
- repeated mission actions

## Custom Expression Blocks

Purpose:

- package reusable value computations

Use them for:

- orbital calculations
- vector helpers
- utility math
- string formatting helpers

Important fidelity note:

Custom expression headers may include formatting metadata and canvas position metadata that matter for exact reproduction.

## Expression Blocks

### Constant

Represents:

- number
- text
- bool

Important fidelity note:

- some string constants intentionally contain leading or trailing spaces
- those spaces must be preserved exactly

### BinaryOp

Typical operations:

- `+`
- `-`
- `*`
- `/`
- `%`
- `^`
- min
- max
- random
- atan2

### BoolOp

Typical operations:

- `and`
- `or`

### Comparison

Typical operations:

- equals
- not equals
- greater than
- less than
- greater-or-equal
- less-or-equal

### Conditional

Purpose:

- ternary-style conditional value

### MathFunction

Typical functions include:

- abs
- sqrt
- floor
- ceiling
- round
- sin
- cos
- tan
- asin
- acos
- atan
- ln
- log
- deg2rad
- rad2deg

### Vector and VectorOp

Typical vector operations include:

- x, y, z component
- length
- normalization
- dot
- cross
- angle
- projection

### CraftProperty

Purpose:

- read game state from the craft or related systems

Typical groups:

- altitude
- orbit
- velocity
- performance
- input
- time
- target
- navigation

### Planet

Purpose:

- read planet-scoped values or transform between frames

Use it for:

- radius
- mass
- parent body
- atmosphere height
- solar position
- orbital data

Important fidelity notes:

- nested `Planet` expressions inside arithmetic are common in real mission code
- examples such as `Vz.Planet(...).Mass() / Vz.Planet(...).Mass()` must stay as nested `Planet` children inside `BinaryOp`
- if such expressions degrade into `Variable.variableName="Vz.Planet(...)"`, the XML is broken

### EvaluateExpression

Purpose:

- evaluate a raw expression string that does not fit a higher-level Vizzy block cleanly

### CallCustomExpression

Purpose:

- call a named custom expression

### StringOp

Typical operations:

- format
- join
- contains
- length
- substring
- friendly time or distance formatting

Use them for:

- display text
- user input prompts
- structured logs
- formatted countdowns
- formatted distances

Important fidelity notes:

- `friendly` is not just a generic string helper
- real XML often requires `subOp="time"` or `subOp="distance"`
- these nodes are used in encounter countdowns, landing readouts, and burn timing output
- trailing or leading spaces in adjacent `Constant text="..."` nodes must be preserved exactly

Common real examples:

```csharp
Vz.StringOp("friendly", Time, "")
Vz.StringOp("friendly", Vz.Length(vector), "")
Vz.Join("Launch Time: ", Vz.StringOp("friendly", Time, ""), "")
```

### TerrainProperty

Purpose:

- read terrain-derived information such as terrain height or color

### PartProperty / Part Transforms

Purpose:

- inspect part-specific data
- convert between part-local and PCI-like frames

### Raycast

Purpose:

- probe geometry or environment intersections

### Coordinate Conversion Expressions

Supported families include:

- lat/long/AGL to position
- position to lat/long/AGL

### Widget Expressions

Supported MFD expression families include:

- widget property reads
- widget event message reads
- pixel reads
- hex color conversion

## MFD Blocks

This project supports a substantial MFD and widget block family.

Supported concepts include:

- widget creation
- widget mutation
- widget layering
- texture initialization
- pixel and line updates
- map, navball, gauge, label, and sprite related flows

## Patterns For Reliable New Scripts

If you are writing a fresh script by hand, prefer this style:

- one `OnStart` event
- `LockNavSphere` for orientation changes
- `SetInput` for throttle changes
- `WaitUntil` for milestones
- `SetTargetHeading(TargetHeadingProperty.Pitch, value)` for pitch programs
- `Display` for visible phase changes

## Patterns That Need Extra Care

Use extra caution with:

- handwritten aliases not already emitted by the converter
- raw expression helper shortcuts
- unusual widget code patterns
- part or craft property blocks with style-sensitive metadata
- custom expressions when exact header fidelity matters
- strings where spaces are semantically part of the original XML
- `friendly` string formatting
- message or log blocks whose XML names differ from their code aliases
- large real mission programs such as `T.T. Mission Program`

## Validation Checklist

Before calling a new authoring template safe, check all of the following:

1. The generated XML opens in Juno.
2. The generated XML does not contain `[TODO]` comments.
3. The program has a valid `Program -> Variables -> Instructions -> Expressions` structure.
4. All key control blocks mapped to actual Vizzy XML, not fallback comments.
5. If the example came from an existing Vizzy XML file, it round-trips cleanly.
6. Strings with surrounding spaces are still identical.
7. `friendly` string nodes still have the correct `subOp`.
8. `LogMessage` and `DisplayMessage` nodes did not collapse into a different XML instruction family.
9. Nested `Planet` expressions still appear as real expression nodes, not variable-name fallbacks.
10. Export validation reports zero errors.

## Export Validator Rules That Matter To Block Authors

The current validator already enforces some block-family rules directly.

### EvaluateExpression

Every `<EvaluateExpression>` must include:

- `style="evaluate-expression"`

This matters for:

- `Vz.E`
- `Vz.Pi`
- `Vz.Infinity`
- `Vz.NaN`
- `Vz.Eval(...)`
- helper-generated evaluate-expression fragments inside higher-level syntax

### Else encoding

VizzyCode must export the Juno-safe `else` shape:

- `ElseIf style="else"`
- leading `<Constant bool="true" />`
- then `<Instructions>`

Raw `<Else>` nodes are treated as invalid.

### Top-level block metadata

These top-level roots must include `pos`:

- `Event`
- `CustomInstruction`
- `CustomExpression`
- `Comment`

Top-level `CustomInstruction` and `CustomExpression` also need:

- `callFormat`
- `format`
- `name`

Top-level `Event` also needs:

- `event`

These are not style preferences. Missing metadata here has already produced real editor-loading failures.

## Final Rule For Future Agents

When in doubt:

- derive new examples from real working Vizzy XML
- preserve documentation in English
- prefer canonical emitted syntax
- verify support in `VizzyXmlConverter.cs` before documenting a pattern as safe

If a real mission file exposes a fidelity bug, document the exact pattern here after fixing it.

When a new Juno rejection is diagnosed, also decide whether it should become:

- a new parser/export fix
- a new export validator rule
- a new example in this guide

That is how this document should keep growing.
