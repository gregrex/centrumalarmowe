#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
[ -f "$ROOT/docs/implementation/40_V22_RELEASE_CANDIDATE_SHOWCASE_SCOPE.md" ]
[ -f "$ROOT/src/Alarm112.Application/Services/ReleaseCandidateService.cs" ]
[ -f "$ROOT/src/Alarm112.Contracts/ReleaseCandidatePackageDto.cs" ]
[ -f "$ROOT/data/content/release-candidate-package.v1.json" ]
[ -f "$ROOT/data/reference/reference-data.v22.releasecandidate.json" ]
echo "smoke-v22 ok"
