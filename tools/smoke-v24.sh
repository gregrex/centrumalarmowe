#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
[ -f "$ROOT/docs/implementation/44_V24_INTERNAL_TEST_AND_LIVEOPS_SCOPE.md" ]
[ -f "$ROOT/src/Alarm112.Application/Services/InternalTestService.cs" ]
[ -f "$ROOT/src/Alarm112.Contracts/InternalTestPackDto.cs" ]
[ -f "$ROOT/data/content/internal-test-pack.v1.json" ]
[ -f "$ROOT/data/reference/reference-data.v24.internaltest.json" ]
echo "smoke-v24 ok"
