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

## Commands

- `VizzyCode: Import XML to Code`
- `VizzyCode: Export Code to XML`
- `VizzyCode: Round-Trip XML`

When the extension is active, it also shows a `VizzyCode` item in the VS Code status bar.

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

That generates:

- a round-tripped XML file
- a generated code file

If you are using AI on imported mission files, also provide the AI with:

- the repository `README.md`
- `docs/VizzyAuthoringGuide.md`
- `docs/VizzyBlocksMegaGuide.md`
- `docs/AiRepairContextGuide.md`
- `docs/RawPreservationGuide.md`

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

1. publish `VizzyCode.Cli` as a standalone Windows binary
2. copy the extension files into `vscode-extension-dist`
3. place the CLI in `vscode-extension-dist\bin\win-x64`
4. generate `vizzycode-tools-0.0.55.vsix`
5. install that `.vsix` into VS Code

Repository command:

```powershell
.\scripts\install-vscode-integration.ps1
```

Outputs:

- `vscode-extension-dist\`
- `vizzycode-tools-0.0.55.vsix`

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
