$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
if (!(Test-Path (Join-Path $root "docs/implementation/40_V22_RELEASE_CANDIDATE_SHOWCASE_SCOPE.md"))) { throw "missing scope doc" }
if (!(Test-Path (Join-Path $root "src/Alarm112.Application/Services/ReleaseCandidateService.cs"))) { throw "missing service" }
if (!(Test-Path (Join-Path $root "src/Alarm112.Contracts/ReleaseCandidatePackageDto.cs"))) { throw "missing contract" }
if (!(Test-Path (Join-Path $root "data/content/release-candidate-package.v1.json"))) { throw "missing data" }
if (!(Test-Path (Join-Path $root "data/reference/reference-data.v22.releasecandidate.json"))) { throw "missing ref data" }
Write-Host "smoke-v22 ok"
