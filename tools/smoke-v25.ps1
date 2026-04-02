$root = Split-Path -Parent $PSScriptRoot
$checks = @(
  "docs/implementation/46_V25_FINAL_HANDOFF_ANDROID_BUILD_SCOPE.md",
  "src/Alarm112.Application/Services/FinalHandoffService.cs",
  "src/Alarm112.Contracts/FinalHandoffPackDto.cs",
  "data/content/final-handoff-pack.v1.json",
  "data/reference/reference-data.v25.finalhandoff.json"
)
foreach ($check in $checks) {
  if (-not (Test-Path (Join-Path $root $check))) { throw "Missing $check" }
}
Write-Host "smoke-v25 ok"
