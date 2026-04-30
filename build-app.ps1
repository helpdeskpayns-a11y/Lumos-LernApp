$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectPath = Join-Path $root "SpieleLernApp.csproj"
$dotnetPath = (Get-Command dotnet -ErrorAction Stop).Source

& $dotnetPath build $projectPath
