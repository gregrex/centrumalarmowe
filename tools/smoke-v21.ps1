$Root = Split-Path -Parent $PSScriptRoot
$Checks = @(
  "docs/implementation/38_V21_QUASI_FINAL_SHOWCASE_BUILD_SCOPE.md",
  "src/Alarm112.Application/Services/ReviewBuildService.cs",
  "src/Alarm112.Contracts/ReviewBuildPackageDto.cs",
  "data/content/review-build-flow.v1.json"
)
foreach ($Check in $Checks) {
  if (-not (Test-Path (Join-Path $Root $Check))) { throw "Missing $Check" }
}
Write-Host "smoke-v21 ok"
