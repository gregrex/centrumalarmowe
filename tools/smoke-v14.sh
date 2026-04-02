#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"

require_file () {
  if [ ! -f "$1" ]; then
    echo "Missing required file: $1" >&2
    exit 1
  fi
}

require_file "$ROOT/docs/implementation/24_V14_FULL_MISSION_VERTICAL_SLICE_SCOPE.md"
require_file "$ROOT/docs/ui/37_FULL_MISSION_RUNTIME_DISPATCH_SCREEN.md"
require_file "$ROOT/docs/art/39_FULL_MISSION_RUNTIME_SCENES_AND_RUNTIME_OBJECTS.md"
require_file "$ROOT/docs/audio/22_FULL_MISSION_RUNTIME_AUDIO_STACK.md"
require_file "$ROOT/docs/gameplay/33_FULL_MISSION_VERTICAL_SLICE_RUNTIME.md"
require_file "$ROOT/docs/backend/11_FULL_MISSION_RUNTIME_API.md"
require_file "$ROOT/data/content/full-mission-runtime.v1.json"
require_file "$ROOT/data/content/mission-complete-gate.v1.json"
require_file "$ROOT/data/content/runtime-dispatch-outcomes.v1.json"
require_file "$ROOT/data/content/objective-tracker.v1.json"
require_file "$ROOT/data/content/mission-script.01.full.json"
require_file "$ROOT/data/reference/reference-data.v14.fullmission.json"
require_file "$ROOT/src/Alarm112.Contracts/MissionRuntimeStateDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/MissionCompleteGateDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/ObjectiveTrackerDto.cs"
require_file "$ROOT/src/Alarm112.Application/Interfaces/IMissionRuntimeService.cs"
require_file "$ROOT/src/Alarm112.Application/Services/MissionRuntimeService.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Session/MissionRuntimeHudController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Session/MissionCompleteGateController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/UI/ObjectiveTrackerController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/UI/EventFeedController.cs"

grep -q '/api/mission-runtime/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/mission-complete-gate/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/runtime-dispatch-outcomes/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/objective-tracker/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/mission-script/demo' "$ROOT/src/Alarm112.Api/Program.cs"

echo "V14 smoke passed."
