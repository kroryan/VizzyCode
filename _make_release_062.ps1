$ErrorActionPreference = "Stop"

$base = "C:\Users\adryl\AppData\Local\Temp\VizzyCode"
$releaseRoot = Join-Path $base "release"
$version = "0.00062"
$pluginVersion = "0.0.62"

$pluginDir = Join-Path $releaseRoot "vizzycode-tools-$version-vscode-plugin"
$appDir    = Join-Path $releaseRoot "VizzyCode-$version-standalone-win64"
$modDir    = Join-Path $releaseRoot "VizzyCode-$version-juno-mod"
$pluginZip = "$pluginDir.zip"
$appZip    = "$appDir.zip"
$modZip    = "$modDir.zip"
$strayRel  = Join-Path $base '$rel'

$releaseRootResolved = (Resolve-Path -LiteralPath $releaseRoot).Path
$baseResolved        = (Resolve-Path -LiteralPath $base).Path

function Remove-SafePath([string]$path, [string]$allowedRoot) {
    if (-not (Test-Path -LiteralPath $path)) { return }
    $full = (Resolve-Path -LiteralPath $path).Path
    if (-not $full.StartsWith($allowedRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "Refusing to remove path outside allowed root: $full"
    }
    Remove-Item -LiteralPath $full -Recurse -Force
}

Remove-SafePath $pluginDir $releaseRootResolved
Remove-SafePath $appDir    $releaseRootResolved
Remove-SafePath $modDir    $releaseRootResolved
Remove-SafePath $pluginZip $releaseRootResolved
Remove-SafePath $appZip    $releaseRootResolved
Remove-SafePath $modZip    $releaseRootResolved
Remove-SafePath $strayRel  $baseResolved

New-Item -ItemType Directory -Force -Path $pluginDir, $appDir, $modDir | Out-Null

# ── Shared items for plugin + standalone ──────────────────────────────────────
$sharedItems = @("README.md", "docs", "Vizzy examples", "Mod Assets")
foreach ($item in $sharedItems) {
    $src = Join-Path $base $item
    if (-not (Test-Path -LiteralPath $src)) { throw "Missing shared release item: $src" }
    Copy-Item -LiteralPath $src -Destination $pluginDir -Recurse -Force
    Copy-Item -LiteralPath $src -Destination $appDir    -Recurse -Force
}

# ── VS Code plugin ─────────────────────────────────────────────────────────────
$pluginBundle = Join-Path $pluginDir "vizzycode-tools"
New-Item -ItemType Directory -Force -Path $pluginBundle | Out-Null
Copy-Item -Path (Join-Path $base "vscode-extension\*") -Destination $pluginBundle -Recurse -Force

$vsix = Join-Path $base "vizzycode-tools-$pluginVersion.vsix"
if (-not (Test-Path -LiteralPath $vsix)) { throw "Missing VSIX: $vsix" }
Copy-Item -LiteralPath $vsix -Destination $pluginDir -Force

$vsReadme = Join-Path $base "vscode-extension\README.md"
if (Test-Path -LiteralPath $vsReadme) {
    Copy-Item -LiteralPath $vsReadme -Destination (Join-Path $pluginDir "VSCode-README.md") -Force
}

# ── Standalone app ────────────────────────────────────────────────────────────
Copy-Item -Path (Join-Path $base "publish_062_win64\*") -Destination $appDir -Recurse -Force

$cliExe = Join-Path $base "publish_cli_062\VizzyCode.Cli.exe"
if (-not (Test-Path -LiteralPath $cliExe)) { throw "Missing CLI executable: $cliExe" }
Copy-Item -LiteralPath $cliExe -Destination $appDir -Force

$cliPdb = Join-Path $base "publish_cli_062\VizzyCode.Cli.pdb"
if (Test-Path -LiteralPath $cliPdb) {
    Copy-Item -LiteralPath $cliPdb -Destination $appDir -Force
}

# ── Juno mod ──────────────────────────────────────────────────────────────────
$sharedModItems = @("README.md", "docs")
foreach ($item in $sharedModItems) {
    $src = Join-Path $base $item
    if (-not (Test-Path -LiteralPath $src)) { throw "Missing mod item: $src" }
    Copy-Item -LiteralPath $src -Destination $modDir -Recurse -Force
}

$junoReadme = Join-Path $base "release\VizzyCode-0.00060-juno-mod\JUNO-MOD-README.md"
if (Test-Path -LiteralPath $junoReadme) {
    Copy-Item -LiteralPath $junoReadme -Destination $modDir -Force
}

# Copy the compiled .sr2-mod (built by Unity)
$sr2mod = Join-Path $base "Mod Assets\VizzyCode.sr2-mod"
if (-not (Test-Path -LiteralPath $sr2mod)) {
    # Fallback: look for it in common Unity build output locations
    $sr2mod = Join-Path $base "release\VizzyCode-0.00060-juno-mod\VizzyCode.sr2-mod"
    Write-Warning "VizzyCode.sr2-mod not found in Mod Assets - using 0.0060 copy as placeholder. Replace with freshly built mod before distributing!"
}
Copy-Item -LiteralPath $sr2mod -Destination $modDir -Force

# Copy source scripts
$sourceDir = Join-Path $modDir "source\Assets"
New-Item -ItemType Directory -Force -Path $sourceDir | Out-Null
$modAssets = Join-Path $base "Mod Assets"
Copy-Item -LiteralPath (Join-Path $modAssets "ModData.asset") -Destination $sourceDir -Force -ErrorAction SilentlyContinue
$scriptsDir = Join-Path $modAssets "Scripts"
if (Test-Path -LiteralPath $scriptsDir) {
    Copy-Item -LiteralPath $scriptsDir -Destination $sourceDir -Recurse -Force
}

# ── Zip all three ─────────────────────────────────────────────────────────────
Compress-Archive -Path (Join-Path $pluginDir "*") -DestinationPath $pluginZip -Force
Compress-Archive -Path (Join-Path $appDir    "*") -DestinationPath $appZip    -Force
Compress-Archive -Path (Join-Path $modDir    "*") -DestinationPath $modZip    -Force

Get-Item -LiteralPath $pluginZip, $appZip, $modZip |
    Select-Object FullName, @{N='Size';E={[math]::Round($_.Length/1KB,1).ToString()+'KB'}} |
    Format-Table -AutoSize
