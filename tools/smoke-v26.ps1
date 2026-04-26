$root = Split-Path -Parent $PSScriptRoot
$checks = @(
  "docs/implementation/48_V26_REAL_ANDROID_BUILD_AND_BUGFIX_FREEZE_SCOPE.md",
  "src/Alarm112.Application/Services/RealAndroidBuildService.cs",
  "src/Alarm112.Contracts/RealAndroidBuildDto.cs",
  "data/content/real-android-build.v1.json",
  "data/reference/reference-data.v26.realandroid.json"
)
foreach ($check in $checks) {
  if (-not (Test-Path (Join-Path $root $check))) { throw "Missing $check" }
}

$apiBase = $env:SMOKE_API_BASE
if ([string]::IsNullOrWhiteSpace($apiBase)) {
  $defaultApi = "http://localhost:5080"
  try {
    Invoke-RestMethod "$defaultApi/health/live" -TimeoutSec 2 -ErrorAction Stop | Out-Null
    $apiBase = $defaultApi
  } catch {
    $apiBase = $null
  }
}

if (-not [string]::IsNullOrWhiteSpace($apiBase)) {
  $live = Invoke-RestMethod "$apiBase/health/live" -TimeoutSec 5
  $ready = Invoke-RestMethod "$apiBase/health/ready" -TimeoutSec 5
  $content = Invoke-RestMethod "$apiBase/api/content/validate" -TimeoutSec 10

  if (-not $live.ok) { throw "Live health check failed at $apiBase/health/live" }
  if (-not $ready.ok) { throw "Readiness health check failed at $apiBase/health/ready" }
  if (-not ($content.PSObject.Properties.Name -contains "isValid")) { throw "Content validation payload missing isValid." }

  Write-Host "smoke-v26 ok (live HTTP checks against $apiBase)"
} else {
  Write-Host "smoke-v26 ok (static checks only; API not running)"
}
