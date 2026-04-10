# Export Validation And Coverage Guide

This guide explains how VizzyCode now protects the `code -> XML` path against known Juno-breaking output, how to verify broad exporter coverage, and how to use those tools safely during manual work and AI-assisted work.

It is the reference document for these topics:

- export-time validation
- coverage verification
- what current validation proves
- what current validation does **not** prove
- how to use these checks when debugging a failing mission

## Why This Guide Exists

Historically, some handwritten or AI-authored `.vizzy.cs` files produced XML that looked reasonable but still failed to appear in the Juno Vizzy editor.

The biggest lesson from those failures is simple:

- logical equivalence is not enough
- pretty-looking XML is not enough
- successful conversion is not enough

The exported XML must also avoid known structural patterns that Juno rejects.

VizzyCode now contains an explicit validator so the app and CLI can stop before saving clearly broken XML.

## Current Safety Layers

The `code -> XML` path now has four separate safety layers:

1. authoring parser support
2. clean-view plus metadata sidecar restoration
3. export validation
4. coverage verification across real example files

These layers do different jobs.

### 1. Authoring parser support

This is the normal `ConvertCodeToXml()` logic.

It is responsible for:

- reading handwritten VizzyCode syntax
- restoring untouched exact imported code from the sidecar when available
- generating a new XML graph

### 2. Clean view plus metadata sidecar restoration

If a file was imported from XML, the visible `.vizzy.cs` may be cleaner than the exact imported source.

Before export, VizzyCode combines:

- the visible clean code
- the sidecar metadata in `*.vizzy.meta.json`

This means unchanged fidelity-sensitive imported lines can still export exactly, while edited lines export through the normal authoring path.

### 3. Export validation

After XML is generated, VizzyCode validates it before saving.

This protects against known structural mistakes that previously produced XML Juno would silently ignore.

### 4. Coverage verification

The repository also contains a broader verification pass that scans current example files and reports whether they export cleanly under the validator.

This is not a unit test suite for all Vizzy in existence, but it is a strong regression detector for the real files in this repository.

## What The Export Validator Checks

The validator lives in:

- `VizzyExportValidator.cs`

It currently checks these rules.

### Program structure

- the root must be `<Program>`
- there must be at least one top-level `<Instructions>` block

### EvaluateExpression shape

- every `<EvaluateExpression>` must include:
  - `style="evaluate-expression"`

This rule matters because missing `style` caused real exports to disappear from the Juno editor.

### Else encoding

- raw `<Else>` nodes are not allowed
- VizzyCode expects the Juno-safe form:
  - `<ElseIf style="else">`
  - followed by `<Constant bool="true" />`
  - followed by `<Instructions>`

This rule matters because a real incorrect `<Else>` export was previously accepted by the converter but rejected by Juno.

### Top-level position metadata

For top-level instruction roots, the validator requires `pos` on these first-child node types:

- `Event`
- `CustomInstruction`
- `CustomExpression`
- `Comment`

This is important because Juno may reject or mishandle new authored top-level blocks that do not carry the expected layout metadata.

### Top-level metadata completeness

For top-level `CustomInstruction` and `CustomExpression` nodes, the validator requires:

- `callFormat`
- `format`
- `name`

For top-level `Event` nodes, it requires:

- `event`

## Where Validation Runs

### CLI

`VizzyCode.Cli export` validates XML before writing the output file.

If validation fails:

- export returns a non-zero exit code
- the CLI prints a structured validation error list
- the XML is not silently accepted as "good enough"

### Desktop app

The WinForms app validates XML before saving.

If validation fails:

- the app shows a blocking error dialog
- the XML save is stopped

This is intentional. It is better to stop than to produce another file that Juno silently refuses to open.

### Round-trip command

`VizzyCode.Cli roundtrip` also validates the regenerated XML before writing it.

That means a file can fail round-trip export even if it imported successfully, and that failure should be treated as useful diagnostic information.

## How To Run Coverage Verification

The desktop app entry point supports a repository-wide verification mode:

```powershell
dotnet run --project VizzyCode.csproj -c Release -- --verify-vizzy
```

This writes:

- `vizzy_coverage_report.txt`

The report currently includes:

- builder node coverage information
- round-trip sample checks for top-level XML files
- code-export validation checks for `.cs` and `.vizzy.cs` examples

## What The Coverage Report Checks

For XML round-trip samples, the report records:

- import warning count
- whether generated code contains `[TODO]`
- whether regenerated XML has a `<Program>` root
- whether regenerated XML contains `<Instructions>`
- how many export validation errors were found

For code export samples, the report records:

- whether export succeeded
- how many export validation errors were found
- the first validation error, if any

## What A Passing Validation Actually Means

If export validation passes, it means:

- the XML avoided the structural error patterns currently known and checked by VizzyCode
- the file is much less likely to be silently ignored by Juno for those exact reasons

This is a strong quality gate, but it is not the same as a mathematical proof that every possible Vizzy construct is supported.

## Known Converter Fixes (v0.0.58+)

The following bugs were fixed in the converter and are now covered by export:

| Area | What changed |
|------|-------------|
| `Vz.Sinh(x)` | Formula corrected to `(e^x - e^(-x)) / 2` |
| `Vz.Cosh(x)` | Formula corrected to `(e^x + e^(-x)) / 2` |
| `Vz.Acosh(x)` | Formula corrected to `ln(x + sqrt(x^2 - 1))` |
| `Vz.Display(msg, dur)` | Duration argument now accepts variables and expressions, not just literals |
| `Vz.FlightLog(msg, show)` | Second bool argument now correctly emitted on import |
| `Vz.LockNavSphere(type, vec)` | Child value now exported for all indicator types, not just Vector |
| Compound assignments | `x -= n`, `x *= n`, `x /= n`, `x %= n`, `x ^= n` now expand to `SetVariable` |
| Event handler prefix | `using (new Vz.OnStart())` now accepted in addition to `using (new OnStart())` |
| `SplitArgs` | Verbatim `@"..."` string literals no longer break argument splitting |
| `ExtractParenthesisContent` | Now correctly tracks balanced parentheses instead of using `LastIndexOf(')')` |
| Comment filter | Separator comments identified by em-dash/double-bar line shape, not by `Contains("Program:")` |
| `NeedsRawConstantPreservation` | Bool constants (`bool="true"/"false"`) no longer preserved unnecessarily |
| `_ceNameMap` variable | Unused variable removed |
| CE format strings | No-parameter CE blocks no longer produce trailing whitespace |

## What A Passing Validation Does Not Guarantee

A passing validator does **not** automatically guarantee:

- byte-for-byte equality with the original XML
- semantic equivalence with the original mission
- support for every undocumented Vizzy block in existence
- correct in-game layout for every authored graph
- correctness of the mission logic itself

That is why large or critical files should still be checked with:

- the original XML when one exists
- the round-trip harness
- direct loading in Juno

## Recommended Verification Workflow

### For existing imported missions

1. Import the original XML.
2. Keep the sidecar file.
3. Export with the CLI or app.
4. If needed, run `roundtrip`.
5. Compare against the original XML.
6. Open the result in Juno.

### For brand-new handwritten `.vizzy.cs`

1. Export with the CLI or app.
2. Confirm validation passes.
3. Open the XML in Juno.
4. If the script becomes a starter template, keep the working XML as a future reference.

### For AI-assisted mission repair

1. Compare repo CLI export and VS Code plugin export byte-for-byte.
2. Check whether the failing region is preserved-imported or handwritten.
3. Run export validation.
4. If validation passes but Juno still rejects the file, compare the XML structure against working examples from `Vizzy examples`.
5. Fix one exact structural issue at a time.

## Relationship To AI Context

An AI can easily misread a "validation passed" result if you do not give it the surrounding context.

For AI-assisted exporter debugging, the minimum context set should include:

- `README.md`
- `docs/VizzyAuthoringGuide.md`
- `docs/AiRepairContextGuide.md`
- this guide

For mission-scale files, also include:

- the original XML
- the current `.vizzy.cs`
- the current exported XML
- the matching `*.vizzy.meta.json` sidecar when the file was imported from XML

## Common Failure Interpretation

### Validation fails before save

Meaning:

- VizzyCode detected a known structural XML problem

Action:

- fix the exporter or the authoring code before trying Juno again

### Validation passes but Juno still rejects the XML

Meaning:

- the remaining problem is more specific than the current validator rules

Action:

- compare the XML directly against working examples
- inspect the failing region, not the whole file
- check whether the source region is preserved-imported or handwritten

### Repo CLI and plugin CLI differ

Meaning:

- tool drift

Action:

- fix the packaging or installed extension first

## Maintenance Rule

When a new real Juno rejection is diagnosed:

1. identify the exact structural XML pattern
2. decide whether it belongs in the authoring parser or the validator
3. add the rule to the validator if it is a clearly invalid exported shape
4. update this document

That is how this guide should grow over time.
