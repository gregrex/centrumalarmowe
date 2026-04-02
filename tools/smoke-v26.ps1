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
Write-Host "smoke-v26 ok"
