# VizzyCode Tools Install Guide

This package can be installed in two ways.

## Option 1: Install from the Bundle Folder

This is the easiest method if you downloaded the self-contained extension folder.

1. Extract the ZIP file.
2. Open the extracted `vizzycode-tools` folder.
3. Run:

```powershell
.\install.ps1
```

That script builds a local `.vsix` from the folder contents and installs it into VS Code.

After installation:

1. Restart VS Code or run `Developer: Reload Window`.
2. Open the Command Palette.
3. Search for:
   - `VizzyCode Import XML`
   - `VizzyCode Export Code`
   - `VizzyCode Round-Trip`

If the extension is active, a `VizzyCode` status bar item appears in the lower-left corner.

Important:

- imported XML now produces:
  - a clean visible `*.vizzy.cs`
  - a matching `*.vizzy.meta.json` sidecar
- keep both files together if you want the best export fidelity
- the sidecar stores the exact imported metadata while the visible code stays cleaner for editing
- exports now pass through the same XML validation gate as the desktop app and CLI
- if export fails with a validation error, treat that as a real structural XML problem, not just a warning

## Option 2: Install from the VSIX File

If the release includes `vizzycode-tools-0.0.60.vsix`, install it with either:

```powershell
code --install-extension .\vizzycode-tools-0.0.60.vsix --force
```

or inside VS Code:

1. Open Extensions.
2. Open the `...` menu.
3. Choose `Install from VSIX...`
4. Select `vizzycode-tools-0.0.60.vsix`

## What This Package Contains

- `extension.js`
- `package.json`
- `README.md`
- `INSTALL.md`
- `install.ps1`
- `bin\win-x64\VizzyCode.Cli.exe`

The bundled CLI is included so the extension works without requiring the full VizzyCode repository.
