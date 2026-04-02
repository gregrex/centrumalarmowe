#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
[ -f "$ROOT/docs/implementation/48_V26_REAL_ANDROID_BUILD_AND_BUGFIX_FREEZE_SCOPE.md" ]
[ -f "$ROOT/src/Alarm112.Application/Services/RealAndroidBuildService.cs" ]
[ -f "$ROOT/src/Alarm112.Contracts/RealAndroidBuildDto.cs" ]
[ -f "$ROOT/data/content/real-android-build.v1.json" ]
[ -f "$ROOT/data/reference/reference-data.v26.realandroid.json" ]
echo "smoke-v26 ok"
