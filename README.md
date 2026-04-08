# VizzyCode

VizzyCode is a Windows Forms editor for opening Vizzy XML, converting it to C#-style VizzyBuilder code, editing it, and exporting back to Vizzy XML.

## Documentation Language Policy

All repository documentation must be written in English.

This rule applies to:
- `README` files
- Markdown documentation
- Text documentation and guides
- User-facing developer notes
- Documentation comments added for future maintenance

If an agent updates or creates documentation, it must keep the documentation in English or translate it to English before finishing the task.

## Documentation

Start here if you want to write or maintain Vizzy scripts with this project:

- [Vizzy Authoring Guide](docs/VizzyAuthoringGuide.md)
- [Vizzy Blocks Mega Guide](docs/VizzyBlocksMegaGuide.md)

Useful reference examples:

- [orbiting maybe.xml](Vizzy%20examples/orbiting%20maybe.xml)
- [orbiting maybe starter.cs](Vizzy%20examples/orbiting%20maybe%20starter.cs)
- [Auto Orbit authoring-safe.cs](Vizzy%20examples/Auto%20Orbit%20authoring-safe.cs)

## AI Integration

The app now supports two distinct ways to use Claude Code, Gemini CLI, Codex CLI, and OpenCode:

<img width="1397" height="833" alt="image" src="https://github.com/user-attachments/assets/b5557aa8-66c0-4c7a-b04e-79450e9b0f61" />

1. In-app headless chat
   Uses the agent CLI in non-interactive mode from the current working directory.

2. Open CLI
   Launches the real native agent CLI in a terminal window from the current working directory.
   This is the mode to use for approvals, consent prompts, slash commands, session resume, agent management, and the full native feature set of each agent.

This split is intentional. The official CLIs do not all expose the same non-interactive approval protocol, so VizzyCode now stops pretending they do.

## Current Provider Behavior

### Claude

- CLI mode uses `claude -p --verbose --output-format stream-json --include-partial-messages`
- Interactive mode is opened through the `Open CLI` button
- Default model is `sonnet`
- Agent switching supported in headless mode for `build`, `plan`, and `general`

Official references:
- https://docs.anthropic.com/en/docs/claude-code/overview
- https://docs.anthropic.com/en/docs/claude-code/sdk

### Gemini

- CLI mode uses the real `gemini` binary in headless mode
- Interactive mode is opened through the `Open CLI` button
- Default model is `gemini-2.5-pro`
- API mode remains available with a Google AI Studio key

Official references:
- https://github.com/google-gemini/gemini-cli
- https://geminicli.com/docs

### OpenAI / Codex

- CLI mode uses the real `codex` binary through `codex exec --json`
- Interactive mode is opened through the `Open CLI` button
- Default model is `gpt-5-codex`
- API mode remains available with an OpenAI API key

Official references:
- https://github.com/openai/codex
- https://developers.openai.com/codex

### OpenCode

- CLI mode uses `opencode run --format json`
- Interactive mode is opened through the `Open CLI` button
- If you configure Base URL, VizzyCode can also use an OpenAI-compatible API endpoint
- For CLI mode, model names should follow the provider/model format expected by OpenCode

Official references:
- https://github.com/sst/opencode
- https://opencode.ai/docs

## Settings

AI settings are stored in:

`%AppData%\VizzyCode\ai-settings.json`

The app now persists provider, mode, model, and API settings between runs.

## Working Directory

Agent CLIs now run from the directory of the currently opened file when possible. If no file is open, they run from the app directory.

That fixes a major integration bug in the previous implementation, where CLIs were launched without reliable project context.

## Writing New Vizzy Scripts

If you are writing new Vizzy programs by hand, do not assume that arbitrary C#-like helper syntax will import cleanly.

Use these references first:

- [Vizzy Authoring Guide](docs/VizzyAuthoringGuide.md)
- [Vizzy Blocks Mega Guide](docs/VizzyBlocksMegaGuide.md)
- `Vizzy examples/orbiting maybe.xml`
- `Vizzy examples/orbiting maybe starter.cs`

Those examples define the current safe authoring subset for `code -> XML` generation.

Additional safe template:

- `Vizzy examples/Auto Orbit authoring-safe.cs`

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
