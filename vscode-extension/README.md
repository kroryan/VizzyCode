# VizzyCode Tools for VS Code

This extension adds a practical VS Code workflow for VizzyCode:

- import Vizzy XML into code
- export code back to Vizzy XML
- run round-trip tests directly from VS Code

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

For fidelity-sensitive files, also run:

- `VizzyCode: Round-Trip XML`

That generates:

- a round-tripped XML file
- a generated code file

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
4. generate `vizzycode-tools-0.0.1.vsix`
5. install that `.vsix` into VS Code

Repository command:

```powershell
.\scripts\install-vscode-integration.ps1
```

Outputs:

- `vscode-extension-dist\`
- `vizzycode-tools-0.0.1.vsix`

## Self-Contained Bundle Layout

The distributable extension folder is expected to contain:

- `extension.js`
- `package.json`
- `README.md`
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
