$ErrorActionPreference = "Stop"

$base = "C:\Users\adryl\AppData\Local\Temp\VizzyCode"
$rel  = "$base\release"

# ── VS Code plugin release ─────────────────────────────────────────────────────
$plugDir = "$rel\vizzycode-tools-0.00059-vscode-plugin"
if (Test-Path $plugDir) { Remove-Item -Recurse -Force $plugDir }
New-Item -ItemType Directory -Force -Path $plugDir | Out-Null

Copy-Item "$base\README.md"                                            "$plugDir\README.md"
Copy-Item "$base\vscode-extension\INSTALL.md"                         "$plugDir\VSCode-INSTALL.md"
Copy-Item "$base\vscode-extension\README.md"                          "$plugDir\VSCode-README.md"
Copy-Item -Recurse "$base\docs"                                        "$plugDir\docs"
Copy-Item -Recurse "$base\Vizzy examples"                              "$plugDir\Vizzy examples"
Copy-Item -Recurse "$base\vscode-extension-dist"                       "$plugDir\vizzycode-tools"
Copy-Item "$base\vizzycode-tools-0.0.59.vsix"                          "$plugDir\vizzycode-tools-0.0.59.vsix"

Write-Host "Plugin folder assembled: $plugDir"

# ── Standalone desktop release ─────────────────────────────────────────────────
$stanDir = "$rel\VizzyCode-0.00059-standalone-win64"
if (Test-Path $stanDir) { Remove-Item -Recurse -Force $stanDir }
New-Item -ItemType Directory -Force -Path $stanDir | Out-Null

Copy-Item "$base\README.md"                                            "$stanDir\README.md"
Copy-Item "$base\vscode-extension\INSTALL.md"                         "$stanDir\VSCode-INSTALL.md"
Copy-Item "$base\vscode-extension\README.md"                          "$stanDir\VSCode-README.md"
Copy-Item -Recurse "$base\docs"                                        "$stanDir\docs"
Copy-Item -Recurse "$base\Vizzy examples"                              "$stanDir\Vizzy examples"
Copy-Item "$base\publish_059_win64\VizzyCode.exe"                      "$stanDir\VizzyCode.exe"
Copy-Item "$base\publish_059_win64\VizzyCode.pdb"                      "$stanDir\VizzyCode.pdb"
Copy-Item "$base\publish_cli_059\VizzyCode.Cli.exe"                    "$stanDir\VizzyCode.Cli.exe"
Copy-Item "$base\publish_cli_059\VizzyCode.Cli.pdb"                    "$stanDir\VizzyCode.Cli.pdb"

Write-Host "Standalone folder assembled: $stanDir"

# ── Zip both ──────────────────────────────────────────────────────────────────
$plugZip = "$rel\vizzycode-tools-0.00059-vscode-plugin.zip"
$stanZip = "$rel\VizzyCode-0.00059-standalone-win64.zip"

if (Test-Path $plugZip) { Remove-Item -Force $plugZip }
if (Test-Path $stanZip) { Remove-Item -Force $stanZip }

Compress-Archive -Path "$plugDir\*" -DestinationPath $plugZip -Force
Write-Host "Plugin zip: $plugZip"

Compress-Archive -Path "$stanDir\*" -DestinationPath $stanZip -Force
Write-Host "Standalone zip: $stanZip"

Write-Host ""
Write-Host "Release 0.0.59 ready."
