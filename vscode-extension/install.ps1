$ErrorActionPreference = "Stop"

$extensionRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$publisher = "vizzycode"
$name = "vizzycode-tools"
$version = "0.0.5"
$displayName = "VizzyCode Tools"
$description = "Import Vizzy XML to code, export code back to Vizzy XML, and run round-trip tests from VS Code."
$engine = "^1.85.0"
$extensionId = "$publisher.$name"
$vsixStageRoot = Join-Path $extensionRoot "_vsix_stage"
$vsixPath = Join-Path $extensionRoot "$name-$version.vsix"

if (-not (Test-Path (Join-Path $extensionRoot "bin\win-x64\VizzyCode.Cli.exe"))) {
    throw "Bundled CLI not found. This installer expects bin\win-x64\VizzyCode.Cli.exe inside the extension folder."
}

if (Test-Path $vsixStageRoot) {
    Remove-Item -Recurse -Force $vsixStageRoot
}

if (Test-Path $vsixPath) {
    Remove-Item -Force $vsixPath
}

New-Item -ItemType Directory -Force -Path (Join-Path $vsixStageRoot "extension") | Out-Null

Get-ChildItem $extensionRoot -Force |
    Where-Object { $_.Name -notin @("_vsix_stage", "$name-$version.vsix") } |
    ForEach-Object {
        Copy-Item -Recurse -Force $_.FullName (Join-Path $vsixStageRoot "extension")
    }

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

code --install-extension $vsixPath --force

Write-Host ""
Write-Host "Installed VS Code extension: $extensionId@$version"
Write-Host "Package: $vsixPath"
Write-Host "Restart VS Code if the commands are not visible yet."
