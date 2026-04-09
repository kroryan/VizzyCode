# Raw Preservation Guide

This guide explains the `Raw*` and `RawXml*` escape hatches used by VizzyCode when exact Vizzy XML preservation matters more than ordinary high-level readability.

## What Raw Preservation Is

VizzyCode sometimes emits calls such as:

```csharp
Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkUiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+")
```

That means:

- keep this Vizzy XML fragment exactly as it was
- do not reinterpret it into a higher-level authoring form
- preserve round-trip fidelity

The long string is base64-encoded Vizzy XML.

VizzyCode can now also emit the same preserved fragment in readable form:

```csharp
Vz.RawXmlEval(@"<EvaluateExpression style=""evaluate-expression""><Constant text=""E"" /></EvaluateExpression>")
```

## When You Need Raw Preservation

Usually you do **not** need raw preservation for normal handwritten scripts.

It is mainly useful when:

- importing real Vizzy XML and keeping exact structure
- preserving a fidelity-sensitive expression or property
- forcing a very specific XML shape that the high-level authoring syntax does not cover yet

If you are writing a fresh script by hand and you immediately need many raw fragments, that usually means one of two things:

- the high-level authoring syntax for that case is still missing
- or you are trying to hand-author a region that is still better treated as preserved imported XML

## Current Raw Forms

VizzyCode supports these preserved forms:

- `Vz.RawConstant("...base64...")`
- `Vz.RawVariable("...base64...")`
- `Vz.RawCraftProperty("...base64...")`
- `Vz.RawEval("...base64...")`

It also now supports readable XML equivalents:

- `Vz.RawXmlConstant(@"<Constant ... />")`
- `Vz.RawXmlVariable(@"<Variable ... />")`
- `Vz.RawXmlCraftProperty(@"<CraftProperty ...>...</CraftProperty>")`
- `Vz.RawXmlEval(@"<EvaluateExpression ...>...</EvaluateExpression>")`

The `RawXml*` forms are intended to be easier to read, review, reproduce, and edit by hand.

## Current Recommended Behavior

VizzyCode now prefers readable `RawXml*` when importing fidelity-sensitive fragments from real XML.

That means imported code should now usually show:

- `Vz.RawXmlConstant(...)`
- `Vz.RawXmlVariable(...)`
- `Vz.RawXmlCraftProperty(...)`
- `Vz.RawXmlEval(...)`

instead of opaque base64 blobs.

On top of that, the current clean-view import workflow hides most fidelity metadata in a sidecar file:

- visible code file: `name.vizzy.cs`
- sidecar: `name.vizzy.meta.json`

When possible, the visible code is simplified further:

- `Vz.RawXmlVariable(...)` -> plain variable names
- `Vz.RawXmlConstant(...)` -> normal literals
- simple `Vz.RawXmlEval(...)` -> `Vz.ExactEval("...")`

The sidecar keeps the exact imported form so unchanged lines can still export with fidelity.

## Recommended Rule

Use this order of preference:

1. normal high-level VizzyCode authoring syntax
2. `RawXml*` when you need exact XML and want a readable escape hatch
3. base64 `Raw*` only when you specifically want a compact preserved payload

## How To Know Which One You Should Use

Use normal high-level syntax when:

- the converter already supports the feature cleanly
- the exported XML is verified in Juno

Use `RawXml*` when:

- the exact XML fragment matters
- you need to preserve a block or expression as-is
- you want something readable and reproducible

Use legacy base64 `Raw*` only when:

- you specifically need a compact payload
- or you are dealing with older imported code that still contains it

## How To Reproduce A Raw Payload

### Encode XML into a reproducible raw payload

```powershell
dotnet run --project VizzyCode.Cli\VizzyCode.Cli.csproj -- raw-encode "snippet.xml"
```

That prints:

- the element kind
- the base64 payload
- a `Vz.Raw*("...")` form
- a `Vz.RawXml*(@"...")` form

This is the recommended way to generate a reproducible preserved fragment for documentation, AI context, or code review.

You can also save the result:

```powershell
dotnet run --project VizzyCode.Cli\VizzyCode.Cli.csproj -- raw-encode "snippet.xml" -o "raw-snippet.txt"
```

### Decode a raw payload back into XML

You can decode:

- a bare base64 payload
- a `Vz.Raw*("...")` call
- a `Vz.RawXml*(@"...")` call
- a plain XML string

That means the CLI can act as the canonical tool for:

- documenting what a raw payload means
- regenerating readable preserved code
- converting legacy base64 payloads into readable XML

Examples:

```powershell
dotnet run --project VizzyCode.Cli\VizzyCode.Cli.csproj -- raw-decode "PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkUiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+" -o "decoded.xml"
dotnet run --project VizzyCode.Cli\VizzyCode.Cli.csproj -- raw-decode "Vz.RawEval(\"PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkUiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+\")" -o "decoded.xml"
```

## Example

These two forms are equivalent:

```csharp
Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IkUiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+")
```

```csharp
Vz.RawXmlEval(@"<EvaluateExpression style=""evaluate-expression""><Constant text=""E"" /></EvaluateExpression>")
```

If you are writing new code by hand, the second form is usually the better choice.

If the fragment is just an exact evaluate-expression constant, clean-view code may also show:

```csharp
Vz.ExactEval("E")
```

That is a cleaner authoring surface for the same concept.

## Practical Rule For AI And Maintainers

When you show raw-preserved code to an AI or another maintainer:

- prefer the `RawXml*` form
- avoid posting only opaque base64 payloads when a readable equivalent is available

If you receive legacy base64 `Raw*` code:

1. decode it with `raw-decode`
2. inspect the XML
3. if needed, regenerate the readable `RawXml*` form with `raw-encode`

## Safety Notes

- Do not replace a preserved raw fragment with cleaner high-level code unless you have re-verified the exported XML in Juno.
- Raw preservation exists because some XML shapes are fidelity-sensitive.
- Logical equivalence is not enough for those cases.
- If imported code still preserves fidelity-sensitive XML, readability improvements should prefer `RawXml*`, not structural rewriting.
