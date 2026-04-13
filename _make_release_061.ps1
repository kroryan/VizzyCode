$ErrorActionPreference = "Stop"

$base = "C:\Users\adryl\AppData\Local\Temp\VizzyCode"
$releaseRoot = Join-Path $base "release"
$version = "0.00061"
$pluginVersion = "0.0.61"

$pluginDir = Join-Path $releaseRoot "vizzycode-tools-$version-vscode-plugin"
$appDir = Join-Path $releaseRoot "VizzyCode-$version-standalone-win64"
$pluginZip = "$pluginDir.zip"
$appZip = "$appDir.zip"
$strayRel = Join-Path $base '$rel'

$releaseRootResolved = (Resolve-Path -LiteralPath $releaseRoot).Path
$baseResolved = (Resolve-Path -LiteralPath $base).Path

function Remove-SafePath([string]$path, [string]$allowedRoot) {
    if (-not (Test-Path -LiteralPath $path)) { return }
    $full = (Resolve-Path -LiteralPath $path).Path
    if (-not $full.StartsWith($allowedRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "Refusing to remove path outside allowed root: $full"
    }
    Remove-Item -LiteralPath $full -Recurse -Force
}

Remove-SafePath $pluginDir $releaseRootResolved
Remove-SafePath $appDir $releaseRootResolved
Remove-SafePath $pluginZip $releaseRootResolved
Remove-SafePath $appZip $releaseRootResolved
Remove-SafePath $strayRel $baseResolved

New-Item -ItemType Directory -Force -Path $pluginDir, $appDir | Out-Null

$sharedItems = @("README.md", "docs", "Vizzy examples", "Mod Assets")
foreach ($item in $sharedItems) {
    $src = Join-Path $base $item
    if (-not (Test-Path -LiteralPath $src)) { throw "Missing shared release item: $src" }
    Copy-Item -LiteralPath $src -Destination $pluginDir -Recurse -Force
    Copy-Item -LiteralPath $src -Destination $appDir -Recurse -Force
}

$pluginBundle = Join-Path $pluginDir "vizzycode-tools"
New-Item -ItemType Directory -Force -Path $pluginBundle | Out-Null
Copy-Item -Path (Join-Path $base "vscode-extension-dist\*") -Destination $pluginBundle -Recurse -Force

$vsix = Join-Path $base "vizzycode-tools-$pluginVersion.vsix"
if (-not (Test-Path -LiteralPath $vsix)) { throw "Missing VSIX: $vsix" }
Copy-Item -LiteralPath $vsix -Destination $pluginDir -Force

$vsReadme = Join-Path $base "vscode-extension\README.md"
if (Test-Path -LiteralPath $vsReadme) {
    Copy-Item -LiteralPath $vsReadme -Destination (Join-Path $pluginDir "VSCode-README.md") -Force
}

Copy-Item -Path (Join-Path $base "publish_061_win64\*") -Destination $appDir -Recurse -Force

$cliExe = Join-Path $base "publish_cli_061\VizzyCode.Cli.exe"
if (-not (Test-Path -LiteralPath $cliExe)) { throw "Missing CLI executable: $cliExe" }
Copy-Item -LiteralPath $cliExe -Destination $appDir -Force

$cliPdb = Join-Path $base "publish_cli_061\VizzyCode.Cli.pdb"
if (Test-Path -LiteralPath $cliPdb) {
    Copy-Item -LiteralPath $cliPdb -Destination $appDir -Force
}

Compress-Archive -Path (Join-Path $pluginDir "*") -DestinationPath $pluginZip -Force
Compress-Archive -Path (Join-Path $appDir "*") -DestinationPath $appZip -Force

Get-Item -LiteralPath $pluginZip, $appZip |
    Select-Object FullName, Length |
    Format-Table -AutoSize
