# Run once per PowerShell session before building/running (from any folder):
#   . .\use-d-drive.ps1
#
# Points NuGet, CLI home, and temp folders to D: so C: is not filled by builds.

$ErrorActionPreference = "Stop"

$root = "D:\InternoDevCache"
@(
  "$root\nuget",
  "$root\dotnet",
  "$root\temp",
  "$root\aspnet",
  "$root\http"
) | ForEach-Object {
  New-Item -ItemType Directory -Force -Path $_ | Out-Null
}

$env:NUGET_PACKAGES = "$root\nuget"
$env:DOTNET_CLI_HOME = "$root\dotnet"
$env:TEMP = "$root\temp"
$env:TMP = "$root\temp"
$env:ASPNETCORE_TEMP = "$root\aspnet"
$env:DOTNET_BUNDLE_EXTRACT_BASE_DIR = "$root\dotnet\bundles"

Write-Host "Dev caches redirected to $root"
Write-Host "  NUGET_PACKAGES=$env:NUGET_PACKAGES"
Write-Host "  DOTNET_CLI_HOME=$env:DOTNET_CLI_HOME"
Write-Host "  TEMP/TMP=$env:TEMP"
Write-Host ""
Write-Host "Optional permanent (User) env vars:"
Write-Host "  [System.Environment]::SetEnvironmentVariable('NUGET_PACKAGES', '$root\nuget', 'User')"
Write-Host "  [System.Environment]::SetEnvironmentVariable('DOTNET_CLI_HOME', '$root\dotnet', 'User')"
Write-Host "  [System.Environment]::SetEnvironmentVariable('TEMP', '$root\temp', 'User')"
Write-Host "  [System.Environment]::SetEnvironmentVariable('TMP', '$root\temp', 'User')"
