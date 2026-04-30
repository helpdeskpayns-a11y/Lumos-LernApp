$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectPath = Join-Path $root "SpieleLernApp.csproj"
$installerScript = Join-Path $root "Installer\Lumos-LernApp.iss"
$dotnetPath = (Get-Command dotnet -ErrorAction Stop).Source
$isccPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
$publishDir = Join-Path $root "artifacts\publish\win-x64"
$outputDir = Join-Path $root "artifacts\installer"
$version = "1.0.0"

if (-not (Test-Path $isccPath)) {
    Write-Error "Inno Setup wurde nicht gefunden."
    exit 1
}

New-Item -ItemType Directory -Force -Path $publishDir | Out-Null
New-Item -ItemType Directory -Force -Path $outputDir | Out-Null

& $dotnetPath publish $projectPath `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=false `
    -o $publishDir

if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

& $isccPath "/DMyAppVersion=$version" "/DPublishDir=$publishDir" "/DOutputDir=$outputDir" $installerScript

if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

Write-Host "Setup erstellt in $outputDir"
