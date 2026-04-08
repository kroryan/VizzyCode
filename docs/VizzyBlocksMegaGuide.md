# Vizzy Blocks Mega Guide

This document is the long-form reference for writing, reading, and maintaining Vizzy programs in this repository.

It is intentionally written from the perspective of this project, not from an abstract game-design perspective. The goal is practical:

- explain what each supported block does
- show how it appears in XML and in C#-style authoring code
- document the safe authoring subset for `code -> XML`
- document pitfalls that can make Juno reject a generated program

If you are new to this project, read these files in this order:

1. `README.md`
2. `docs/VizzyAuthoringGuide.md`
3. this file

## Scope

This guide covers the block families that are currently supported by `VizzyXmlConverter.cs`, especially:

- instruction blocks
- expression blocks
- custom instructions
- custom expressions
- MFD widgets and widget interaction
- block authoring patterns that round-trip reliably

It does not claim to be an exhaustive reverse-engineered reference for every block Juno has ever shipped. It is the supported reference for this repository.

## Mental Model

Vizzy programs in this project move between three forms:

1. Vizzy XML
2. C#-style Vizzy authoring code
3. round-tripped Vizzy XML

There are two different success criteria:

### Existing program fidelity

For an existing Vizzy program exported from Juno:

`XML -> code -> XML`

must preserve the exact structure that Juno expects, ideally byte-for-byte.

### New script authoring

For a new script written by hand:

`code -> XML`

must generate XML that Juno recognizes as a valid Vizzy program.

These goals overlap, but they are not the same. A nice-looking handwritten script is not automatically valid unless it uses supported authoring patterns.

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

Use it when:

- you want the main control loop
- you are writing startup logic
- you are building a simple single-thread flight program

### Other Events

This converter also supports several other event forms in existing XML:

- flight end
- receive message
- docked
- change SOI
- part collision
- part explode

Use cases:

- message-driven coordination
- craft lifecycle hooks
- automation triggered by simulation events

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

Notes:

- comments are preserved as real Vizzy comment blocks
- comment text is documentation, so keep it in English in this repository

## Variable Blocks

Variable-related blocks control program state.

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

Use it for:

- state initialization
- sensor sampling into variables
- intermediate calculations

### Change Variable

Purpose:

- increment or add to a variable

Typical authoring code:

```csharp
counter += 1;
```

Use it for:

- counters
- timers
- accumulators

### Variable References

Variable references appear inside expressions.

There are several important distinctions:

- global variables
- local variables
- list variables

For fidelity-sensitive cases, this converter can preserve raw variable metadata exactly.

## Flow Control Blocks

These blocks shape execution.

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

Use it for:

- main loops
- polling loops
- burn loops
- state machines

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

Use it for:

- phase transitions
- safety checks
- different throttle or pitch regimes

### Repeat

Purpose:

- repeat a fixed number of times

Use it for:

- known iteration counts
- finite retries

### For

Purpose:

- run a loop with an iterator variable and explicit start, end, and step

Use it for:

- list scans
- countdowns
- sampled sweeps

Important note:

`For` blocks can carry local-variable metadata that matters for fidelity. This project preserves that carefully for XML round-trips.

### Wait Seconds

Purpose:

- yield execution for a time interval

Typical code:

```csharp
Vz.WaitSeconds(0.1);
```

Use it for:

- throttling loops
- spacing out state changes
- avoiding per-frame spam

### Wait Until

Purpose:

- pause until a condition becomes true

Typical code:

```csharp
using (new WaitUntil(condition)) { }
```

This is one of the safest blocks for handwritten scripts because it maps cleanly and keeps logic easy to read.

Use it for:

- altitude milestones
- apoapsis and periapsis milestones
- time-to-apoapsis triggers
- event-like conditions without writing a loop

### Break

Purpose:

- exit the current loop

Use it when:

- a loop has an emergency termination condition
- you want a simpler loop body than a giant negated condition

## Output and Logging Blocks

### Display / DisplayMessage

Purpose:

- show a user-facing message on screen for a duration

Typical code:

```csharp
Vz.Display("Launching", 7);
```

Use it for:

- mission status
- debugging milestones
- player feedback

### Log

Purpose:

- write a simple log line

Typical code:

```csharp
Vz.Log("Entering coast phase");
```

Use it for:

- debug traces
- internal state breadcrumbs

### FlightLog

Purpose:

- write to the flight log

Use it when:

- you want persistent mission logging instead of transient display text

## Craft Control Blocks

These are the core flight-automation actions.

### Activate Stage

Purpose:

- trigger staging

Typical code:

```csharp
Vz.ActivateStage();
```

Use it at:

- launch
- booster separation
- upper-stage ignition

### Set Input

Purpose:

- write a raw craft input axis or toggle

Common inputs:

- `Throttle`
- `Pitch`
- `Yaw`
- `Roll`
- sliders
- brake-like inputs depending on the model

Typical code:

```csharp
Vz.SetInput(CraftInput.Throttle, 1);
```

Use it for:

- throttle control
- manual axis-driven control logic
- cockpit-like automation

### Set Target Heading

Purpose:

- command a heading-related autopilot property

Typical code:

```csharp
Vz.SetTargetHeading(TargetHeadingProperty.Pitch, 45);
Vz.SetTargetHeading(TargetHeadingProperty.Heading, 90);
```

Use it for:

- gravity turns
- simple ascent pitch profiles
- fixed heading control

### Lock Heading Alias

This repository now supports the manual authoring alias:

```csharp
Vz.LockHeading(90, 45);
```

During `code -> XML`, it is expanded to:

- heading target = `90`
- pitch target = `45`

This makes handwritten code more convenient, but the explicit `SetTargetHeading(...)` form is still the canonical emitted style.

### Lock Nav Sphere

Purpose:

- lock orientation to a nav indicator such as `Current` or `Prograde`

Typical code:

```csharp
Vz.LockNavSphere(LockNavSphereIndicatorType.Current, 0);
Vz.LockNavSphere(LockNavSphereIndicatorType.Prograde, 0);
```

Use it for:

- holding current attitude at launch
- switching to prograde for circularization
- vector-following guidance when the vector form is used

This block is strongly recommended for new handwritten orbital examples because it is simple and reliable.

### Set Activation Group

Purpose:

- toggle action groups

Typical code:

```csharp
Vz.SetActivationGroup(1, true);
```

Use it for:

- toggling systems
- deploying mechanisms
- coordinating script states with craft setup

### Set Time Mode

Purpose:

- change time warp or time mode

Use it with care:

- this can be fidelity-sensitive in XML
- preserve the original form if round-tripping an existing program

### Switch Craft

Purpose:

- switch active craft

Use it for:

- multi-craft automation
- docking workflows

### Set Target / Target Node

Purpose:

- assign a target or node

Use it for:

- rendezvous logic
- navigation workflows

### Set Camera

Purpose:

- change a camera property such as zoom

Use it for:

- MFD or cinematic helpers
- debug visualization

### Set Part Property

Purpose:

- change a part-level property

Typical use cases:

- activate a named part
- toggle mechanisms
- drive animated assemblies

This family is very important for craft automation and can be structurally sensitive. Existing XML often preserves exact raw craft-property nodes for safety.

## User Input Block

Purpose:

- ask the player for a value

Typical use:

- entering mission parameters
- selecting a mode
- configuring altitudes or burn values

Because this block has fidelity-sensitive attribute ordering in some real examples, the converter preserves it carefully.

## Broadcasting and Messaging Blocks

### BroadcastMessage

Purpose:

- send data/messages locally or across craft scopes depending on flags

Use it for:

- decoupled event communication
- widget click handling
- multi-thread coordination

### ReceiveMessage Event

Purpose:

- start a handler thread when a message arrives

Use it for:

- UI events
- scripted protocol handling
- asynchronous sub-behaviors

## List Blocks

Lists are useful for sequences, lookup tables, and lightweight storage.

### Mutating List Blocks

Supported list instruction family includes operations such as:

- add
- insert
- remove
- set
- clear
- sort
- reverse

Use them for:

- waypoint lists
- state history
- lookup values

### List Expressions

Supported read-oriented list expressions include things like:

- get item
- list length
- index lookup
- inline list creation

Use them for:

- fetching table values
- reading a sampled sequence
- processing mission data

## Custom Instruction Blocks

Purpose:

- package reusable instruction sequences

Typical structure:

- top-level custom instruction declaration
- one or more parameter names
- body instructions
- calls from other threads or blocks

Use custom instructions for:

- repeated craft actions
- subsystem control
- reusable mission actions

Examples:

- gear deployment
- solar panel deployment
- repeated docking routines

## Custom Expression Blocks

Purpose:

- package reusable value computations

Use them for:

- vector math helpers
- orbital calculations
- utility math
- string formatting helpers

Important fidelity note:

Custom expression headers may include formatting metadata and canvas position metadata that matter for exact reproduction. This project preserves those headers when round-tripping real XML.

## Expression Blocks

Expressions are the value language of Vizzy. These are used inside conditions, assignments, action inputs, custom expressions, and custom instruction arguments.

### Constant

Represents:

- number
- text
- bool

Notes:

- some values must be preserved exactly, including `-0`
- some numeric-looking values are intentionally stored as text in real XML

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

Use them for:

- arithmetic
- orbital formulas
- control laws

### BoolOp

Typical operations:

- `and`
- `or`

Use them for:

- multi-clause conditions
- guard logic

### Not

Purpose:

- negate a boolean condition

### Comparison

Typical operations:

- equals
- not equals
- greater than
- less than
- greater-or-equal
- less-or-equal

Use them for:

- milestone tests
- range checks
- boolean state transitions

### Conditional

Purpose:

- ternary-style conditional value

Use it for:

- expression-level branching
- choosing values without a full block `If`

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

Use them for:

- orbital math
- geometry
- UI formatting

### Vector and VectorOp

Vectors are essential for navigation and orbital mechanics.

Typical vector operations include:

- x, y, z component
- length
- normalization
- dot
- cross
- angle
- projection

Use them for:

- velocity and position math
- basis changes
- orbital plane calculations

### CraftProperty

Purpose:

- read game state from the craft or related systems

Typical groups:

- altitude
- orbit
- velocity
- performance
- input
- misc
- time
- target
- navigation
- fuel

Use them for:

- altitude checks
- apoapsis/periapsis milestones
- current mass and thrust calculations
- control state inspection

Important note:

Some craft properties have style metadata that differs between real XML sources. When style mismatches are important, this converter preserves the original raw node.

### Planet

Purpose:

- read planet-scoped values or transform between position/planet frames

Use it for:

- atmosphere height
- radius
- parent body
- solar position
- orbital data

### EvaluateExpression

Purpose:

- evaluate a raw expression string that does not fit a higher-level Vizzy block cleanly

Use it carefully:

- it is useful
- but it is less structured than native block expressions
- and exact style/metadata can matter during round-trip work

### ActivationGroup Expression

Purpose:

- read whether an activation group is currently active

### CraftInput Expression

Purpose:

- read a craft input value

### CallCustomExpression

Purpose:

- call a named custom expression

### StringOp

Typical operations:

- format
- join
- contains
- length
- substring-like operations depending on source XML

Use them for:

- display text
- user input prompts
- structured logs

### TerrainProperty

Purpose:

- read terrain-derived information such as terrain height or color

### PartProperty / Part Transforms

Purpose:

- inspect part-specific data
- convert between part-local and PCI/world-like frames

Use them for:

- part sensors
- transform math
- advanced craft systems

### Raycast

Purpose:

- probe geometry/environment intersections

Use it for:

- landing logic
- obstacle sensing

### Coordinate Conversion Expressions

Supported families include:

- lat/long/AGL to position
- position to lat/long/AGL

Use them for:

- map conversion
- terrain-relative navigation

### Widget Expressions

Supported MFD expression families include:

- widget property reads
- widget event message reads
- pixel reads
- hex color conversion

Use them for:

- UI logic
- interactive displays

## MFD Blocks

This project supports a substantial MFD/widget block family.

### Widget Creation

Supported widget creation concepts include:

- label
- sprite
- gauge
- line
- navball
- map
- texture-like widgets

Use them for:

- cockpit displays
- mission status screens
- debugging UIs

### Widget Mutation

Supported block families include:

- set widget property
- set label
- set sprite
- set gauge
- set line
- set navball
- set map
- set widget anchor
- set widget event
- send widget to front
- send widget to back
- initialize texture
- set pixel
- set line points

These are useful for:

- dynamic UI layouts
- interactive MFD tools
- custom map/texture displays

## Patterns For Reliable New Scripts

If you are writing a fresh script by hand, prefer this style:

- one `OnStart` event
- `LockNavSphere` for orientation changes
- `SetInput` for throttle changes
- `WaitUntil` for milestones
- `SetTargetHeading(TargetHeadingProperty.Pitch, value)` for pitch programs
- `Display` for user-visible phase changes

Why:

- this style is easy to read
- this style has already been observed in safe authoring examples
- this style avoids unsupported aliases and half-parsed constructs

## Patterns That Need Extra Care

Use extra caution with:

- handwritten aliases not already emitted by the converter
- raw expression-like helper shortcuts
- unusual widget code patterns
- part/craft property blocks with style-sensitive metadata
- custom expressions when exact header fidelity matters

If a generated XML contains `[TODO]` comments, treat it as broken until the unsupported syntax is fixed.

## Example: Safe Orbital Script Shape

The safest small orbital authoring pattern is:

1. lock current orientation
2. display launch status
3. activate stage
4. set throttle to 1
5. wait until altitude threshold
6. set pitch target
7. wait until apoapsis target
8. throttle off
9. lock prograde
10. wait until near apoapsis
11. throttle on
12. wait until periapsis target
13. throttle off
14. display success

That is exactly why `orbiting maybe` and `Auto Orbit authoring-safe` are important reference files in this repository.

## Validation Checklist

Before calling a new authoring template “safe”, check all of the following:

1. The generated XML opens in Juno.
2. The generated XML does not contain `[TODO]` comments.
3. The program has a valid `Program -> Variables -> Instructions -> Expressions` structure.
4. All key control blocks mapped to actual Vizzy XML, not fallback comments.
5. If the example came from an existing Vizzy XML file, it round-trips cleanly.

## Final Rule For Future Agents

When in doubt:

- derive new examples from real working Vizzy XML
- preserve documentation in English
- prefer canonical emitted syntax
- verify support in `VizzyXmlConverter.cs` before documenting a pattern as safe

If a block family is missing from this guide but is supported in the converter, extend this file in English and document:

- purpose
- XML shape
- C#-style authoring shape
- safe usage
- pitfalls
