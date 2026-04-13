$ErrorActionPreference = "Stop"

$repoRoot      = Split-Path -Parent $PSScriptRoot
$cliProject    = Join-Path $repoRoot "VizzyCode.Cli\VizzyCode.Cli.csproj"
$cliOut        = Join-Path $repoRoot "vscode-extension\bin\win-x64"
$extSource     = Join-Path $repoRoot "vscode-extension"
$version       = (Get-Content (Join-Path $extSource "package.json") | ConvertFrom-Json).version
$vsixPath      = Join-Path $repoRoot "vizzycode-tools-$version.vsix"

Write-Host "Building VizzyCode.Cli $version..."
dotnet publish $cliProject -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true -o $cliOut

Write-Host "Building VSIX..."
Push-Location $extSource
npx --yes @vscode/vsce package --out $vsixPath
Pop-Location

Write-Host "Installing extension..."
code --install-extension $vsixPath --force

Write-Host ""
Write-Host "Done. VSIX: $vsixPath"
Write-Host "Reload VS Code (Ctrl+Shift+P > Developer: Reload Window) to activate."
