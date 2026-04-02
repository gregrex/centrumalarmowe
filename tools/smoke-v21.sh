#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
[ -f "$ROOT/docs/implementation/38_V21_QUASI_FINAL_SHOWCASE_BUILD_SCOPE.md" ]
[ -f "$ROOT/src/Alarm112.Application/Services/ReviewBuildService.cs" ]
[ -f "$ROOT/src/Alarm112.Contracts/ReviewBuildPackageDto.cs" ]
[ -f "$ROOT/data/content/review-build-flow.v1.json" ]
[ -f "$ROOT/tools/content-verify.sh" ]
echo "smoke-v21 ok"
