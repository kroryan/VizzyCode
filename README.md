# VizzyCode

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

## VS Code Integration

This repository now includes a complete VS Code workflow built on top of the same converter:

- [VizzyCode.Cli](VizzyCode.Cli)
- [vscode-extension](vscode-extension)
- [install-vscode-integration.ps1](scripts/install-vscode-integration.ps1)

The VS Code integration gives you:

- `VizzyCode: Import XML to Code`
- `VizzyCode: Export Code to XML`
- `VizzyCode: Round-Trip XML`

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
- `vizzycode-tools-0.0.55.vsix`

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

Examples:

```powershell
VizzyCode.Cli.exe import "Vizzy examples\orbiting maybe.xml" -o "orbiting maybe.vizzy.cs"
VizzyCode.Cli.exe export "Vizzy examples\Auto Orbit authoring-safe.cs" -o "autorbit.xml"
VizzyCode.Cli.exe roundtrip "Vizzy examples\T.T. Mission Program.xml" -o "_tt_rt.xml" -c "_tt_rt.vizzy.cs"
```

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
- `vizzycode-tools-0.0.55.vsix`

## How To Create A Distributable Extension Bundle

If you want to distribute only the extension without the whole repository:

1. run the repository installer once:

```powershell
.\scripts\install-vscode-integration.ps1
```

2. take one of these outputs:

- `vscode-extension-dist\`
- `vizzycode-tools-0.0.55.vsix`

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
code --install-extension .\vizzycode-tools-0.0.55.vsix --force
```

or from inside VS Code:

1. open Extensions
2. open the `...` menu
3. choose `Install from VSIX...`
4. select `vizzycode-tools-0.0.55.vsix`

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

Strongly recommended when the task is about general Vizzy behavior, not only this converter:

- `docs/Mastering Vizzy _ A Complete Guide - Early Access 08.07.25.md`

If the task is fidelity-sensitive, also include the original working XML and the current `.cs`.

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
- [Mastering Vizzy - A Complete Guide](docs/Mastering%20Vizzy%20_%20A%20Complete%20Guide%20-%20Early%20Access%2008.07.25.md)
- `Vizzy examples/orbiting maybe.xml`
- `Vizzy examples/orbiting maybe starter.cs`
- `Vizzy examples/Auto Orbit authoring-safe.cs`
- `Vizzy examples/T.T.cs`

Those files define the current safe and tested authoring subset.

For AI-generated Vizzy code, do not ask the AI to work from code alone. Give it the documentation above as context first.

For handwritten top-level layout control, VizzyCode also supports:

```csharp
// VZPOS x=1200 y=-300
```

Place it before a top-level event, custom instruction, or custom expression to carry explicit positions into exported Vizzy XML.

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
