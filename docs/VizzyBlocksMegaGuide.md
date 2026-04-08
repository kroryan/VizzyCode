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
3. this file

## Scope

This guide covers the block families currently supported by `VizzyXmlConverter.cs`, especially:

- instruction blocks
- expression blocks
- custom instructions
- custom expressions
- MFD widgets and widget interaction
- authoring patterns that round-trip reliably

It does not claim to be an exhaustive reverse-engineered reference for every block Juno has ever shipped. It is the supported reference for this repository.

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

## Final Rule For Future Agents

When in doubt:

- derive new examples from real working Vizzy XML
- preserve documentation in English
- prefer canonical emitted syntax
- verify support in `VizzyXmlConverter.cs` before documenting a pattern as safe

If a real mission file exposes a fidelity bug, document the exact pattern here after fixing it.
