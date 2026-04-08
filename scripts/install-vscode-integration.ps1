$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$cliProject = Join-Path $repoRoot "VizzyCode.Cli\VizzyCode.Cli.csproj"
$publishDir = Join-Path $repoRoot "vscode-extension-dist\bin\win-x64"
$extensionSource = Join-Path $repoRoot "vscode-extension"
$extensionsRoot = Join-Path $env:USERPROFILE ".vscode\extensions"
$distRoot = Join-Path $repoRoot "vscode-extension-dist"
$vsixStageRoot = Join-Path $repoRoot "vscode-extension-vsix"
$publisher = "vizzycode"
$name = "vizzycode-tools"
$displayName = "VizzyCode Tools"
$description = "Import Vizzy XML to code, export code back to Vizzy XML, and run round-trip tests from VS Code."
$version = "0.0.1"
$engine = "^1.85.0"
$extensionId = "$publisher.$name"
$vsixPath = Join-Path $repoRoot "$name-$version.vsix"
$extensionTarget = Join-Path $extensionsRoot "$extensionId-$version"
$legacyExtensionTarget = Join-Path $extensionsRoot "$name-$version"

if (Test-Path $distRoot) {
    Remove-Item -Recurse -Force $distRoot
}

if (Test-Path $vsixStageRoot) {
    Remove-Item -Recurse -Force $vsixStageRoot
}

if (Test-Path $vsixPath) {
    Remove-Item -Force $vsixPath
}

New-Item -ItemType Directory -Force -Path $distRoot | Out-Null

Write-Host "Publishing VizzyCode.Cli..."
dotnet publish $cliProject -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true -o $publishDir

Write-Host "Preparing self-contained VS Code extension..."
Copy-Item -Recurse -Force (Join-Path $extensionSource "*") $distRoot

Write-Host "Building VSIX package..."
New-Item -ItemType Directory -Force -Path (Join-Path $vsixStageRoot "extension") | Out-Null
Copy-Item -Recurse -Force (Join-Path $distRoot "*") (Join-Path $vsixStageRoot "extension")

$vsixManifest = @"
<?xml version="1.0" encoding="utf-8"?>
<PackageManifest Version="2.0.0" xmlns="http://schemas.microsoft.com/developer/vsx-schema/2011">
  <Metadata>
    <Identity Language="en-US" Id="$extensionId" Version="$version" Publisher="$publisher" />
    <DisplayName>$displayName</DisplayName>
    <Description xml:space="preserve">$description</Description>
    <Tags>vizzy xml juno</Tags>
    <Categories>Other</Categories>
    <Properties>
      <Property Id="Microsoft.VisualStudio.Code.Engine" Value="$engine" />
    </Properties>
  </Metadata>
  <Installation>
    <InstallationTarget Id="Microsoft.VisualStudio.Code" />
  </Installation>
  <Dependencies />
  <Assets>
    <Asset Type="Microsoft.VisualStudio.Code.Manifest" Path="extension/package.json" />
    <Asset Type="Microsoft.VisualStudio.Services.Content.Details" Path="extension/README.md" />
  </Assets>
</PackageManifest>
"@

$contentTypes = @"
<?xml version="1.0" encoding="utf-8"?>
<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
  <Default Extension="json" ContentType="application/json" />
  <Default Extension="js" ContentType="application/javascript" />
  <Default Extension="md" ContentType="text/markdown" />
  <Default Extension="exe" ContentType="application/octet-stream" />
  <Default Extension="pdb" ContentType="application/octet-stream" />
  <Default Extension="xml" ContentType="application/xml" />
  <Override PartName="/extension.vsixmanifest" ContentType="text/xml" />
</Types>
"@

Set-Content -LiteralPath (Join-Path $vsixStageRoot "extension.vsixmanifest") -Value $vsixManifest -Encoding UTF8
Set-Content -LiteralPath (Join-Path $vsixStageRoot "[Content_Types].xml") -Value $contentTypes -Encoding UTF8

$zipPath = "$vsixPath.zip"
if (Test-Path $zipPath) {
    Remove-Item -Force $zipPath
}

Compress-Archive -Path (Join-Path $vsixStageRoot "*") -DestinationPath $zipPath -Force
Move-Item -Force $zipPath $vsixPath

if (Test-Path $extensionTarget) {
    Remove-Item -Recurse -Force $extensionTarget
}

if (Test-Path $legacyExtensionTarget) {
    Remove-Item -Recurse -Force $legacyExtensionTarget
}

New-Item -ItemType Directory -Force -Path $extensionsRoot | Out-Null
Copy-Item -Recurse -Force $distRoot $extensionTarget
code --install-extension $vsixPath --force | Out-Null

Write-Host ""
Write-Host "VS Code integration installed."
Write-Host "Extension bundle:      $distRoot"
Write-Host "VSIX package:          $vsixPath"
Write-Host "Extension directory:   $extensionTarget"
Write-Host ""
Write-Host "Restart VS Code to load the extension."
