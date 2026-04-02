$Root = Split-Path -Parent $PSScriptRoot
$checks = @(
  "docs/implementation/44_V24_INTERNAL_TEST_AND_LIVEOPS_SCOPE.md",
  "src/Alarm112.Application/Services/InternalTestService.cs",
  "src/Alarm112.Contracts/InternalTestPackDto.cs",
  "data/content/internal-test-pack.v1.json",
  "data/reference/reference-data.v24.internaltest.json"
)
foreach ($item in $checks) {
  if (-not (Test-Path (Join-Path $Root $item))) { throw "Missing $item" }
}
Write-Host "smoke-v24 ok"
