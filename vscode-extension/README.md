# VizzyCode Tools for VS Code

This extension adds a practical VS Code workflow for VizzyCode:

- import Vizzy XML into code
- export code back to Vizzy XML
- run round-trip tests directly from VS Code

It is built on the same converter core as the desktop app, so the important concepts are the same:

- normal authoring mode for new handwritten scripts
- fidelity mode for existing imported Vizzy XML
- clean-view imported code plus metadata sidecar
- readable raw preservation through `RawXml*`
- top-level layout hints through `// VZPOS x=... y=...`
- export-time XML validation before save

## Commands

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

When the extension is active, it also shows a `VizzyCode` item in the VS Code status bar.

If the optional VizzyCode Juno mod is running, the extension also shows a Juno status item and can import/export Vizzy directly through the local bridge at `http://127.0.0.1:7842/`.

The telemetry and snapshot JSON commands save exact raw data from the running game.

The telemetry and snapshot report commands save Markdown summaries designed to be readable by humans and AI agents. These reports are the recommended first context files for craft-specific Vizzy work. Keep the JSON beside them when exact raw fields are needed.

## CLI Requirement

The extension calls `VizzyCode.Cli`.

When distributed through the bundled package, the CLI is included directly inside the extension folder:

- `bin/win-x64/VizzyCode.Cli.exe`

For development inside the repository, the extension can also find the CLI automatically in these common workspace locations:

- `publish_standalone_win64/VizzyCode.Cli.exe`
- `VizzyCode.Cli/bin/Debug/net9.0/VizzyCode.Cli.exe`
- `VizzyCode.Cli/bin/Release/net9.0/VizzyCode.Cli.exe`

You can also set `vizzycode.cliPath` in VS Code settings.

## Suggested Workflow

1. Open a Vizzy XML file.
2. Run `VizzyCode: Import XML to Code`.
3. Edit the generated `.vizzy.cs` file in VS Code.
4. Run `VizzyCode: Export Code to XML`.
5. Test the resulting XML in Juno.

Important:

- export now runs the same XML validation gate used by the desktop app and CLI
- if the extension reports an export validation failure, do not ignore it and try the XML anyway
- that means VizzyCode already detected a structural pattern known to break Juno loading

The import command now writes:

- a visible clean code file: `name.vizzy.cs`
- a metadata sidecar: `name.vizzy.meta.json`

The visible file is intentionally cleaner than the exact imported structure:

- most `VZTOPBLOCK`, `VZBLOCK`, and `VZEL` comment noise is hidden
- many preserved variables and constants are simplified for readability
- fidelity-sensitive lines are restored from the sidecar during export when unchanged

Keep the sidecar file next to the `.vizzy.cs` if you want maximum export fidelity for imported missions.

If the imported code still contains:

- `VZTOPBLOCK`
- `VZBLOCK`
- `VZEL`
- `Vz.RawXml*`

do not treat those as noise. They are part of how VizzyCode preserves exact XML structure.

For fidelity-sensitive files, also run:

- `VizzyCode: Round-Trip XML`

For craft-aware AI work, also run:

- `VizzyCode: Save Full Juno Craft Report for Humans/AI` before writing or repairing the script
- `VizzyCode: Save Juno Telemetry Report for Humans/AI` at the point where the mission fails or behaves incorrectly
- `VizzyCode: Save Full Juno Craft Snapshot JSON` when exact raw fields are needed
- `VizzyCode: Save Juno Telemetry JSON` when exact raw runtime values are needed

Give the Markdown report files to the AI together with the repository docs and the `.vizzy.cs` file. Add the JSON files only when the AI needs exact raw fields, vectors, or data not shown in the readable report.

If the AI agent cannot trigger VS Code commands directly, generate these reports yourself from VS Code or from the standalone app first. For automated conversion tasks, the AI should use `VizzyCode.Cli.exe` directly instead of depending on the VS Code command UI.

That generates:

- a round-tripped XML file
- a generated code file

If you are using AI on imported mission files, also provide the AI with:

- the repository `README.md`
- `docs/VizzyAuthoringGuide.md`
- `docs/VizzyBlocksMegaGuide.md`
- `docs/AiRepairContextGuide.md`
- `docs/RawPreservationGuide.md`
- `docs/ExportValidationAndCoverageGuide.md`

## Installation

There are two supported install paths.

### End User Install

If you have the self-contained extension bundle, run:

```powershell
.\install.ps1
```

That package contains:

- the extension files
- the bundled CLI in `bin/win-x64`
- the local installer script
- the quick install guide in `INSTALL.md`

### Repository Install

From the repository root, run:

```powershell
.\scripts\install-vscode-integration.ps1
```

That script:

- publishes the bundled CLI
- creates a self-contained extension bundle in `vscode-extension-dist`
- creates a local `.vsix`
- installs the extension into VS Code

This is the recommended install path because it follows the official VS Code VSIX flow instead of relying on a manually copied folder.

After that, restart VS Code.

## How The Extension Is Built

The extension is not a standalone JavaScript-only package. It depends on the bundled `VizzyCode.Cli`.

The repository build pipeline is:

1. publish `VizzyCode.Cli` as a standalone Windows binary (`publish_cli_<ver>\VizzyCode.Cli.exe`)
2. copy the CLI into `vscode-extension\bin\win-x64\VizzyCode.Cli.exe`
3. run `_make_release_<ver>.ps1` — this builds the VSIX and all three release zips

Manual VSIX build:

```powershell
cd vscode-extension
npx @vscode/vsce package --out ../vizzycode-tools-0.0.62.vsix
```

Output:

- `vizzycode-tools-0.0.62.vsix` (includes CLI inside `bin/win-x64/`)

## Self-Contained Bundle Layout

The distributable extension folder is expected to contain:

- `extension.js`
- `package.json`
- `README.md`
- `INSTALL.md`
- `install.ps1`
- `bin\win-x64\VizzyCode.Cli.exe`

This is why users can install the extension from the bundle folder alone without needing the whole repository.

## Manual Build Notes

If you only want the CLI binary for development, you can build it separately:

```powershell
dotnet build ..\VizzyCode.Cli\VizzyCode.Cli.csproj -c Release
```

or publish it standalone:

```powershell
dotnet publish ..\VizzyCode.Cli\VizzyCode.Cli.csproj -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true -o ..\publish_cli_win64
```

The extension can also use a manually specified CLI path through the `vizzycode.cliPath` setting.

## Where The Commands Appear

After installation and a VS Code restart, the commands are available in:

- the Command Palette
- the Explorer file context menu for `.xml` and `.cs` files
- the editor title toolbar for open `.xml` and `.cs` files

Commands:

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

## Troubleshooting

If the extension appears installed but the commands are not visible:

1. Reload VS Code with `Developer: Reload Window`.
2. Check that the extension is enabled.
3. Confirm that the `VizzyCode` status bar item appears.
4. Search the Command Palette for:
   - `VizzyCode Import XML`
   - `VizzyCode Export Code`
   - `VizzyCode Round-Trip`

This extension explicitly declares support for Workspace Trust / Restricted Mode, so those commands should still be available in standard restricted workspaces.

If VS Code passes a stale file path to a command, the extension now falls back to the active editor or prompts for a file again instead of failing with a raw `FileNotFoundException`.
