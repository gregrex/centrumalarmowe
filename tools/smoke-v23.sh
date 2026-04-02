#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
[ -f "$ROOT/docs/implementation/42_V23_ANDROID_PREVIEW_AND_RELEASE_READINESS_SCOPE.md" ]
[ -f "$ROOT/src/Alarm112.Application/Services/AndroidPreviewService.cs" ]
[ -f "$ROOT/src/Alarm112.Contracts/AndroidPreviewBuildDto.cs" ]
[ -f "$ROOT/data/content/android-preview-build.v1.json" ]
[ -f "$ROOT/data/reference/reference-data.v23.androidpreview.json" ]
echo "smoke-v23 ok"
