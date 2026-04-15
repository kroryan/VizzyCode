# VizzyCode

[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/kroryan/VizzyCode)

[![VizzyCodeWiki](https://kroryan.github.io/quartz-vizzycode)



VizzyCode is a Windows Forms editor for opening Vizzy XML, converting it to C#-style Vizzy authoring code, editing it, and exporting back to Vizzy XML.

## Documentation Language Policy

All repository documentation must be written in English.

This rule applies to:

- `README` files
- Markdown documentation
- text guides
- user-facing maintenance notes
- documentation comments written for future agents or maintainers

If documentation is added or updated, it must stay in English.

## What VizzyCode Does

VizzyCode supports two related workflows.

### Existing Vizzy Program Fidelity

`XML -> code -> XML`

This workflow is for real Vizzy programs exported from Juno. The goal is to preserve the exact XML structure that Juno expects.

### New Handwritten Script Authoring

`code -> XML`

This workflow is for writing new Vizzy scripts directly in code. The goal is to generate XML that Juno actually recognizes as a Vizzy program.

These workflows overlap, but they are not the same problem. A handwritten script can be logically correct and still fail to load if it uses unsupported authoring patterns.

## Documentation

Start here if you want to write, maintain, or debug Vizzy scripts with this project:

- [Vizzy Authoring Guide](docs/VizzyAuthoringGuide.md)
- [Vizzy Blocks Mega Guide](docs/VizzyBlocksMegaGuide.md)
- [AI Repair Context Guide](docs/AiRepairContextGuide.md)
- [Raw Preservation Guide](docs/RawPreservationGuide.md)
- [Export Validation And Coverage Guide](docs/ExportValidationAndCoverageGuide.md)
- [Juno Live Bridge Guide](docs/JunoLiveBridgeGuide.md)
- [Juno Telemetry And Craft Snapshot Guide](docs/JunoTelemetrySnapshotGuide.md)
- [Mastering Vizzy - A Complete Guide](docs/Mastering%20Vizzy%20_%20A%20Complete%20Guide%20-%20Early%20Access%2008.07.25.md)

Useful reference examples:

- [orbiting maybe.xml](Vizzy%20examples/orbiting%20maybe.xml)
- [orbiting maybe starter.cs](Vizzy%20examples/orbiting%20maybe%20starter.cs)
- [Auto Orbit authoring-safe.cs](Vizzy%20examples/Auto%20Orbit%20authoring-safe.cs)
- [T.T. Mission Program.xml](Vizzy%20examples/T.T.%20Mission%20Program.xml)
- [T.T.cs](Vizzy%20examples/T.T.cs)

Important maintenance guidance:

- [Vizzy Authoring Guide](docs/VizzyAuthoringGuide.md)
- [Vizzy Blocks Mega Guide](docs/VizzyBlocksMegaGuide.md)
- [AI Repair Context Guide](docs/AiRepairContextGuide.md)
- [Export Validation And Coverage Guide](docs/ExportValidationAndCoverageGuide.md)
- [Juno Live Bridge Guide](docs/JunoLiveBridgeGuide.md)
- [Juno Telemetry And Craft Snapshot Guide](docs/JunoTelemetrySnapshotGuide.md)

## Required Reading By Use Case

If you are using VizzyCode for a specific purpose, use this minimum documentation set.

### For normal handwritten Vizzy authoring

Read:

- `README.md`
- `docs/VizzyAuthoringGuide.md`
- `docs/VizzyBlocksMegaGuide.md`
- `docs/ExportValidationAndCoverageGuide.md`

### For round-trip fidelity work

Read:

- `README.md`
- `docs/VizzyAuthoringGuide.md`
- `docs/AiRepairContextGuide.md`
- `docs/RawPreservationGuide.md`
- `docs/ExportValidationAndCoverageGuide.md`

### For AI-assisted work

Do not give the AI only the current `.cs` file.

Minimum required context:

- `README.md`
- `docs/VizzyAuthoringGuide.md`
- `docs/VizzyBlocksMegaGuide.md`
- `docs/AiRepairContextGuide.md`
- `docs/RawPreservationGuide.md`
- `docs/ExportValidationAndCoverageGuide.md`

Strongly recommended additional language reference:

- `docs/Mastering Vizzy _ A Complete Guide - Early Access 08.07.25.md`

For fidelity-sensitive work, also include:

- the original working XML
- the current `.cs`
- the current exported XML if one exists
- a saved Juno craft snapshot JSON from the running bridge when the script depends on real part names, stages, activation groups, mass, fuel, or flight state

This is mandatory for large mixed missions such as `T.T`.

## Complete AI Context Bundle

If you want reliable AI help with VizzyCode, the documentation is part of the input, not optional background.

### Minimum bundle

- `README.md`
- `docs/VizzyAuthoringGuide.md`
- `docs/VizzyBlocksMegaGuide.md`
- `docs/AiRepairContextGuide.md`
- `docs/RawPreservationGuide.md`
- `docs/ExportValidationAndCoverageGuide.md`

### Strongly recommended additional Vizzy language reference

- `docs/Mastering Vizzy _ A Complete Guide - Early Access 08.07.25.md`

### Also include for fidelity-sensitive or exporter work

- the original working XML
- the current `.vizzy.cs`
- the current exported XML
- the matching `*.vizzy.meta.json` sidecar if the `.vizzy.cs` came from imported XML
- the Juno craft snapshot JSON when working against a specific loaded craft
- the Juno telemetry JSON when diagnosing a runtime failure
- whether repo CLI export and VS Code plugin export are byte-identical
- whether export validation currently passes or fails

If you skip this context, the AI is much more likely to rewrite the wrong region, remove a fidelity boundary, or diagnose the wrong failure class.

## VS Code Integration

This repository now includes a complete VS Code workflow built on top of the same converter:

- [VizzyCode.Cli](VizzyCode.Cli)
- [vscode-extension](vscode-extension)
- [install-vscode-integration.ps1](scripts/install-vscode-integration.ps1)

The VS Code integration gives you:

- `VizzyCode: Import XML to Code`
- `VizzyCode: Export Code to XML`
- `VizzyCode: Round-Trip XML`
- `VizzyCode: Connect to Juno`
- `VizzyCode: Import Vizzy from Running Game`
- `VizzyCode: Export Vizzy to Running Game`
- `VizzyCode: Browse Craft Parts in Juno`
- `VizzyCode: View Stages in Juno`
- `VizzyCode: Save Juno Telemetry JSON`
- `VizzyCode: Save Full Juno Craft Snapshot JSON`
- `VizzyCode: Save Juno Telemetry Report for Humans/AI`
- `VizzyCode: Save Full Juno Craft Report for Humans/AI`

## Juno Live Bridge

VizzyCode can also talk directly to a running Juno: New Origins instance when the optional VizzyCode Juno mod is installed and enabled.

The mod exposes a localhost-only HTTP bridge on:

```text
http://127.0.0.1:7842/
```

Supported bridge operations:

- check whether the mod is running and which scene/craft is active
- list the current craft parts and identify parts with Vizzy programs
- import a part's Vizzy XML directly from the running game
- export generated Vizzy XML directly back into the selected part
- inspect current stage and activation group metadata
- save active-craft telemetry JSON for mission debugging
- save a full craft snapshot JSON for AI/human authoring context
- save readable Markdown reports built from telemetry/snapshot data for humans and AI agents
- activate the next stage from the desktop app or VS Code extension

For AI workflows, the readable Markdown report is the best first context file. The JSON is still useful as the exact raw data source, but it is intentionally more verbose and easier for an AI or human to misread. If an AI agent cannot access VS Code commands directly, use the standalone app menu or `VizzyCode.Cli.exe` for conversion tasks, and give the agent the saved `.report.md` plus the raw `.json` only when exact fields are needed.

The bridge is intentionally local-only and should be treated as a live editing tool, not as a replacement for saving craft backups. Read the full bridge guide before changing complex programs through the game connection:

- [Juno Live Bridge Guide](docs/JunoLiveBridgeGuide.md)
- [Juno Telemetry And Craft Snapshot Guide](docs/JunoTelemetrySnapshotGuide.md)

### Export Validation In The App, CLI, And Extension

VizzyCode no longer treats "XML was generated" as the same thing as "XML is safe to save".

Before a `code -> XML` result is accepted, VizzyCode validates the output against known Juno-breaking structural patterns.

This validation runs in:

- the WinForms app before `Save as Vizzy XML`
- `VizzyCode.Cli export`
- `VizzyCode.Cli roundtrip`
- the VS Code extension, because it uses the same CLI

If validation fails:

- the desktop app blocks the save and shows the validation errors
- the CLI exits with an error instead of silently writing likely-broken XML

### Clean View And Metadata Sidecar

Imported `.vizzy.cs` files now use a clean-view workflow by default.

What the user sees:

- clean authoring code
- no visible `VZTOPBLOCK`, `VZBLOCK`, or `VZEL` comment noise
- simplified preserved fragments where safe, for example:
  - `Vz.RawXmlVariable(...)` -> plain variable names
  - `Vz.RawXmlConstant(...)` -> normal literals
  - simple `Vz.RawXmlEval(...)` -> `Vz.ExactEval("...")`

What VizzyCode stores next to the code file:

- a sidecar file named `*.vizzy.meta.json`

That sidecar stores the exact imported metadata needed to restore fidelity-sensitive lines during export.

This means:

- unchanged imported lines can still export back with their exact preserved structure
- the visible code can stay much cleaner for normal editing
- handwritten changes still export through the normal authoring parser

Example:

- code file: `mission.vizzy.cs`
- sidecar: `mission.vizzy.meta.json`

Do not delete the sidecar if you want maximum fidelity for an imported mission.

Recommended install:

```powershell
.\scripts\install-vscode-integration.ps1
```

That script:

- publishes a bundled `VizzyCode.Cli`
- builds a self-contained extension bundle in `vscode-extension-dist`
- creates a local `.vsix` package
- installs the extension into VS Code
- installs the same self-contained bundle users can distribute separately

Generated artifacts:

- `vscode-extension-dist\`
- `vizzycode-tools-0.0.60.vsix`

After installation, restart VS Code or run `Developer: Reload Window`.

The extension is configured to support VS Code Restricted Mode / Workspace Trust and uses the officially supported `contributes.commands` + VSIX install path.

Visible activation signals:

- `VizzyCode` status bar entry at the lower left when the extension is active
- commands in the Command Palette
- Explorer context menu entries for `.xml` and `.cs`
- editor title actions for `.xml` and `.cs`

If the commands do not appear:

1. Confirm the extension is enabled.
2. Reload the VS Code window.
3. Check whether the `VizzyCode` status bar item is visible.
4. Search the Command Palette for:
   - `VizzyCode Import XML`
   - `VizzyCode Export Code`
   - `VizzyCode Round-Trip`

The current app and the VS Code workflow live in the same repository. A separate repository is not required.

## Build Matrix

This repository now has three practical deliverables:

1. `VizzyCode.exe`
   The WinForms desktop editor.
2. `VizzyCode.Cli`
   The command-line converter used by automation and the VS Code extension.
3. `VizzyCode Tools for VS Code`
   The self-contained VS Code extension bundle and `.vsix`.

## How To Build The Desktop App

Build the Windows desktop editor:

```powershell
dotnet build VizzyCode.csproj -c Release
```

Publish a framework-dependent Windows executable:

```powershell
dotnet publish VizzyCode.csproj -c Release -r win-x64 -p:PublishSingleFile=true --self-contained false
```

Output:

- `bin\Release\net9.0-windows\win-x64\publish\VizzyCode.exe`

Publish a standalone Windows executable that includes .NET:

```powershell
dotnet publish VizzyCode.csproj -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true -o publish_standalone_win64
```

Output:

- `publish_standalone_win64\VizzyCode.exe`

## How To Build The CLI

Build the CLI:

```powershell
dotnet build VizzyCode.Cli\VizzyCode.Cli.csproj -c Release
```

Run the CLI directly from source:

```powershell
dotnet run --project VizzyCode.Cli\VizzyCode.Cli.csproj -- import "input.xml" -o "output.vizzy.cs"
dotnet run --project VizzyCode.Cli\VizzyCode.Cli.csproj -- export "input.vizzy.cs" -o "output.xml"
dotnet run --project VizzyCode.Cli\VizzyCode.Cli.csproj -- roundtrip "input.xml" -o "roundtrip.xml" -c "roundtrip.vizzy.cs"
```

Publish a standalone CLI for distribution:

```powershell
dotnet publish VizzyCode.Cli\VizzyCode.Cli.csproj -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true -o publish_cli_win64
```

Typical output:

- `publish_cli_win64\VizzyCode.Cli.exe`

## CLI Commands

`VizzyCode.Cli` currently supports:

- `import <input.xml> [-o output.vizzy.cs]`
- `export <input.vizzy.cs> [-o output.xml] [-n programName]`
- `roundtrip <input.xml> [-o output.xml] [-c output.vizzy.cs]`
- `raw-encode <input.xml> [-o output.txt]`
- `raw-decode <input.txt|payload|call> [-o output.xml]`

Export and round-trip commands now validate the generated XML before saving it.

That means a command can fail even if XML generation technically succeeded, because the validator found a shape that is already known to break Juno loading.

Examples:

```powershell
VizzyCode.Cli.exe import "Vizzy examples\orbiting maybe.xml" -o "orbiting maybe.vizzy.cs"
VizzyCode.Cli.exe export "Vizzy examples\Auto Orbit authoring-safe.cs" -o "autorbit.xml"
VizzyCode.Cli.exe roundtrip "Vizzy examples\T.T. Mission Program.xml" -o "_tt_rt.xml" -c "_tt_rt.vizzy.cs"
VizzyCode.Cli.exe raw-encode "snippet.xml" -o "raw-snippet.txt"
VizzyCode.Cli.exe raw-decode "Vz.RawEval(\"...\")" -o "decoded-snippet.xml"
```

## Export Validation

Export validation is now a first-class part of the `code -> XML` workflow.

Validator source:

- `VizzyExportValidator.cs`

Current validation rules include:

- root must be `<Program>`
- there must be at least one top-level `<Instructions>` block
- every `<EvaluateExpression>` must include `style="evaluate-expression"`
- raw `<Else>` nodes are not allowed
- `ElseIf style="else"` must carry a leading `<Constant bool="true" />`
- top-level `Event`, `CustomInstruction`, `CustomExpression`, and `Comment` roots must include `pos`
- top-level `CustomInstruction` and `CustomExpression` roots must include `callFormat`, `format`, and `name`
- top-level `Event` roots must include `event`

Why this matters:

- these are not cosmetic checks
- each of these patterns has already been involved in real Juno loading failures in this repository

Read the full explanation here:

- [Export Validation And Coverage Guide](docs/ExportValidationAndCoverageGuide.md)

## Coverage Verification

The repository now also includes a broader verification pass for current examples and mission files.

Run:

```powershell
dotnet run --project VizzyCode.csproj -c Release -- --verify-vizzy
```

Output:

- `vizzy_coverage_report.txt`

The report checks both:

- round-trip samples from XML
- direct `code -> XML` export samples from `.cs` / `.vizzy.cs` files

This does not prove support for every Vizzy construct ever shipped, but it does create a practical regression net over the real examples in this repository.

## Raw Preservation

VizzyCode uses `Raw*` escape hatches when it needs exact XML preservation for fidelity-sensitive fragments.

Examples:

- `Vz.RawConstant("...base64...")`
- `Vz.RawVariable("...base64...")`
- `Vz.RawCraftProperty("...base64...")`
- `Vz.RawEval("...base64...")`

Readable XML equivalents are also supported now:

- `Vz.RawXmlConstant(@"<Constant ... />")`
- `Vz.RawXmlVariable(@"<Variable ... />")`
- `Vz.RawXmlCraftProperty(@"<CraftProperty ...>...</CraftProperty>")`
- `Vz.RawXmlEval(@"<EvaluateExpression ...>...</EvaluateExpression>")`

Use these only when normal high-level authoring syntax is not enough or when exact round-trip preservation matters.

If you need to reproduce or inspect these payloads formally, use the CLI `raw-encode` and `raw-decode` commands described above.

For normal imported editing, you should now see clean-view output first, not the full raw preservation noise. The sidecar file carries the hidden metadata needed for exact export where possible.

## How To Build The VS Code Extension

The repository script is the recommended path because it does the full pipeline:

```powershell
.\scripts\install-vscode-integration.ps1
```

That script performs all of these steps:

1. publishes the bundled standalone CLI into `vscode-extension-dist\bin\win-x64`
2. copies the extension files into `vscode-extension-dist`
3. creates a `.vsix` package
4. installs the extension into the local VS Code profile

Generated outputs:

- `vscode-extension-dist\`
- `vizzycode-tools-0.0.60.vsix`

## How To Create A Distributable Extension Bundle

If you want to distribute only the extension without the whole repository:

1. run the repository installer once:

```powershell
.\scripts\install-vscode-integration.ps1
```

2. take one of these outputs:

- `vscode-extension-dist\`
- `vizzycode-tools-0.0.60.vsix`

The bundle in `vscode-extension-dist\` is self-contained and already includes:

- `extension.js`
- `package.json`
- `README.md`
- `install.ps1`
- `bin\win-x64\VizzyCode.Cli.exe`

An end user can take only that folder and run:

```powershell
.\install.ps1
```

inside the extension folder.

## Manual VSIX Install

If you already have the generated `.vsix`, you can install it manually with either:

```powershell
code --install-extension .\vizzycode-tools-0.0.60.vsix --force
```

or from inside VS Code:

1. open Extensions
2. open the `...` menu
3. choose `Install from VSIX...`
4. select `vizzycode-tools-0.0.60.vsix`

## Current Converter Capabilities

The converter now handles several fidelity-sensitive cases that previously broke Juno loading or changed the XML structure:

- exact round-trip preservation for many existing Vizzy programs
- preservation of instruction metadata such as `id`, `pos`, and original headers
- preservation of sensitive variable metadata when raw variable references are required
- preservation of exact string constants where whitespace matters
- reconstruction of `LogMessage` and `DisplayMessage` compatible shapes during round-trip
- correct reconstruction of `StringOp friendly` with the proper `subOp`
- correct parsing of nested `Vz.Planet(...).Property()` expressions inside larger arithmetic expressions
- support for handwritten aliases such as `Vz.LockHeading(heading, pitch)`
- readable preserved raw XML forms such as `Vz.RawXmlEval(...)`
- deterministic top-level layout metadata with `// VZPOS x=... y=...`
- CLI raw payload tooling through `raw-encode` and `raw-decode`

Important real-world patterns now covered:

- `Vz.StringOp("friendly", value, "")`
- `Vz.Planet(...).Mass() / Vz.Planet(...).Mass()`
- strings with trailing spaces such as `"Launch Time: "`
- large mission scripts such as `T.T. Mission Program`

## Recommended Workflow

### For Existing Juno-Exported Vizzy XML

1. Open the XML in VizzyCode.
2. Convert XML to code.
3. Edit conservatively.
4. Export back to XML.
5. Run the round-trip harness when fidelity matters.
6. Confirm the output opens in Juno.

### For New Handwritten Scripts

1. Start from a known-good example.
2. Use the safe authoring subset described in the docs.
3. Export to XML.
4. Open the XML in Juno.
5. Only then treat the script as a reusable template.

### For Imported XML You Intend To Edit Deeply

1. Import the original XML to `.vizzy.cs`.
2. Check whether the region you want to change still contains `VZTOPBLOCK`, `VZBLOCK`, and `VZEL`.
3. If it does, prefer preserving that region.
4. If you rewrite the region manually, treat it as authoring-mode code from that point on.
5. If fidelity-sensitive expressions remain, prefer `RawXml*` forms over opaque base64 `Raw*` blobs.

## How To Read Imported Code

Imported `.vizzy.cs` files can contain four different layers at once:

1. normal handwritten-style code
2. preserved metadata comments
3. readable raw XML preservation calls
4. deterministic layout hints

### Metadata comments

- `VZTOPBLOCK`
- `VZBLOCK ...`
- `VZEL ...`

These are preservation anchors. Do not remove them casually in imported regions.

### Readable raw XML preservation calls

Examples:

- `Vz.RawXmlConstant(...)`
- `Vz.RawXmlVariable(...)`
- `Vz.RawXmlCraftProperty(...)`
- `Vz.RawXmlEval(...)`

These now appear by default when VizzyCode imports fidelity-sensitive fragments from real XML.

### Clean view versus exact metadata

When you import XML through the current CLI, app, or VS Code extension:

- the visible `.vizzy.cs` is a clean view
- the exact imported fidelity metadata is stored in `*.vizzy.meta.json`

That is the current compromise between:

- readable code for humans and AI
- exact export for untouched imported regions

If a line remains unchanged and still matches the sidecar mapping, VizzyCode restores its exact imported form during export.

If you rewrite the line, VizzyCode exports it through the normal authoring parser instead.

They mean:

- keep this exact XML fragment
- but expose it in a readable and reproducible form

### Manual layout hints

Example:

```csharp
// VZPOS x=1200 y=-300
```

This carries top-level layout information into exported XML when the block does not already have preserved imported `pos`.

## Round-Trip Test Harness

This repository contains a dedicated test harness in `TestRT/`.

Run it like this:

```powershell
dotnet run --project TestRT -c Release -- "<input.xml>" "<output.xml>" "<code.txt>"
```

Example:

```powershell
dotnet run --project TestRT -c Release -- "Vizzy examples\T.T. Mission Program.xml" "_tt_rt_output.xml" "_tt_rt_code.txt"
```

After that, compare the original and generated XML directly:

```powershell
git diff --no-index -- "<input.xml>" "<output.xml>"
```

If the diff is empty, the current round-trip output is identical.

For broader repository verification instead of a single file, use:

```powershell
dotnet run --project VizzyCode.csproj -c Release -- --verify-vizzy
```

That command writes `vizzy_coverage_report.txt` and checks current round-trip and direct export samples under the repository.

## AI Integration

For AI-assisted work, giving the model only the current `.cs` file is not enough.

It is necessary to provide the relevant project documentation as context, especially when asking an AI to:

- repair a failing Vizzy export
- modify a mission-scale script
- preserve round-trip fidelity
- debug Juno editor loading failures
- add new authoring patterns

Minimum recommended context for AI:

- `README.md`
- `docs/VizzyAuthoringGuide.md`
- `docs/VizzyBlocksMegaGuide.md`
- `docs/AiRepairContextGuide.md`
- `docs/RawPreservationGuide.md`
- `docs/ExportValidationAndCoverageGuide.md`

Strongly recommended when the task is about general Vizzy behavior, not only this converter:

- `docs/Mastering Vizzy _ A Complete Guide - Early Access 08.07.25.md`

If the task is fidelity-sensitive, also include the original working XML and the current `.cs`.

If the task involves `RawXml*`, preserved metadata, or newly handwritten replacements inside an imported mission, also include:

- which region is still preserved
- which region was rewritten manually
- whether repo CLI and VS Code plugin CLI currently export byte-identical XML
- whether export validation currently passes or fails
- the current validator error text if validation fails

The app supports two distinct ways to use Claude Code, Gemini CLI, Codex CLI, and OpenCode.

1. In-app headless chat  
   Uses the agent CLI in non-interactive mode from the current working directory.

2. Open CLI  
   Launches the real native agent CLI in a terminal window from the current working directory.
   This is the mode to use for approvals, consent prompts, slash commands, session resume, agent management, and the full native feature set of each agent.

This split is intentional. The official CLIs do not all expose the same non-interactive approval protocol, so VizzyCode does not pretend they do.

## Current Provider Behavior

### Claude

- CLI mode uses `claude -p --verbose --output-format stream-json --include-partial-messages`
- interactive mode is opened through the `Open CLI` button
- default model is `sonnet`
- agent switching is supported in headless mode for `build`, `plan`, and `general`

### Gemini

- CLI mode uses the real `gemini` binary in headless mode
- interactive mode is opened through the `Open CLI` button
- default model is `gemini-2.5-pro`
- API mode remains available with a Google AI Studio key

### OpenAI / Codex

- CLI mode uses the real `codex` binary through `codex exec --json`
- interactive mode is opened through the `Open CLI` button
- default model is `gpt-5-codex`
- API mode remains available with an OpenAI API key

### OpenCode

- CLI mode uses `opencode run --format json`
- interactive mode is opened through the `Open CLI` button
- if you configure Base URL, VizzyCode can also use an OpenAI-compatible API endpoint
- for CLI mode, model names should follow the provider/model format expected by OpenCode

## Settings

AI settings are stored in:

`%AppData%\VizzyCode\ai-settings.json`

The app persists provider, mode, model, and API settings between runs.

## Working Directory

Agent CLIs run from the directory of the currently opened file when possible. If no file is open, they run from the app directory.

That avoids launching external CLIs without reliable project context.

## Writing New Vizzy Scripts

If you are writing new Vizzy programs by hand, do not assume that arbitrary C#-like helper syntax will import cleanly.

Use these references first:

- [Vizzy Authoring Guide](docs/VizzyAuthoringGuide.md)
- [Vizzy Blocks Mega Guide](docs/VizzyBlocksMegaGuide.md)
- [AI Repair Context Guide](docs/AiRepairContextGuide.md)
- [Raw Preservation Guide](docs/RawPreservationGuide.md)
- [Mastering Vizzy - A Complete Guide](docs/Mastering%20Vizzy%20_%20A%20Complete%20Guide%20-%20Early%20Access%2008.07.25.md)
- `Vizzy examples/orbiting maybe.xml`
- `Vizzy examples/orbiting maybe starter.cs`
- `Vizzy examples/Auto Orbit authoring-safe.cs`
- `Vizzy examples/T.T.cs`

Those files define the current safe and tested authoring subset.

## What The Current Safety Net Really Means

VizzyCode is now in a much better place than “export and hope”.

The practical safety net is:

1. importer and authoring parser
2. clean-view plus metadata sidecar restoration
3. export validation
4. repository-wide coverage verification

That means current failures are much easier to localize.

It does **not** honestly mean that every possible Vizzy construct in existence is guaranteed forever.

The correct statement is:

- current real examples and current known failure classes are covered much more strongly than before
- the app and CLI now fail early on several known Juno-breaking XML shapes
- future newly discovered failure patterns should be added to the validator and the docs instead of being silently tolerated

For AI-generated Vizzy code, do not ask the AI to work from code alone. Give it the documentation above as context first.

For handwritten top-level layout control, VizzyCode also supports:

```csharp
// VZPOS x=1200 y=-300
```

Place it before a top-level event, custom instruction, or custom expression to carry explicit positions into exported Vizzy XML.

If high-level authoring syntax is not enough, prefer:

1. normal authoring syntax
2. `RawXml*` readable preservation
3. old base64 `Raw*` only if you explicitly want compact opaque payloads

## Build

```powershell
dotnet build VizzyCode.csproj -c Release
```

## Publish EXE

```powershell
dotnet publish VizzyCode.csproj -c Release -r win-x64 -p:PublishSingleFile=true --self-contained false
```

Published output:

`bin\Release\net9.0-windows\win-x64\publish\VizzyCode.exe`

## Publish Standalone EXE

To build the self-contained Windows standalone version that does not require a separate .NET installation:

```powershell
dotnet publish VizzyCode.csproj -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true -o publish_standalone_win64
```

Standalone output:

`publish_standalone_win64\VizzyCode.exe`
