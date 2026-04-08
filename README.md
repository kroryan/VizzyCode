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

Useful reference examples:

- [orbiting maybe.xml](Vizzy%20examples/orbiting%20maybe.xml)
- [orbiting maybe starter.cs](Vizzy%20examples/orbiting%20maybe%20starter.cs)
- [Auto Orbit authoring-safe.cs](Vizzy%20examples/Auto%20Orbit%20authoring-safe.cs)
- [T.T. Mission Program.xml](Vizzy%20examples/T.T.%20Mission%20Program.xml)
- [T.T.cs](Vizzy%20examples/T.T.cs)

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
- `Vizzy examples/orbiting maybe.xml`
- `Vizzy examples/orbiting maybe starter.cs`
- `Vizzy examples/Auto Orbit authoring-safe.cs`
- `Vizzy examples/T.T.cs`

Those files define the current safe and tested authoring subset.

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
