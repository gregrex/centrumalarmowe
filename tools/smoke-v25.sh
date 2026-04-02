#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
[ -f "$ROOT/docs/implementation/46_V25_FINAL_HANDOFF_ANDROID_BUILD_SCOPE.md" ]
[ -f "$ROOT/src/Alarm112.Application/Services/FinalHandoffService.cs" ]
[ -f "$ROOT/src/Alarm112.Contracts/FinalHandoffPackDto.cs" ]
[ -f "$ROOT/data/content/final-handoff-pack.v1.json" ]
[ -f "$ROOT/data/reference/reference-data.v25.finalhandoff.json" ]
echo "smoke-v25 ok"
