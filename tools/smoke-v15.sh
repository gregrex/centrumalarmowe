#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"

require_file () {
  if [ ! -f "$1" ]; then
    echo "Missing required file: $1" >&2
    exit 1
  fi
}

require_file "$ROOT/docs/implementation/26_V15_PLAYABLE_RUNTIME_MAP_SCOPE.md"
require_file "$ROOT/docs/ui/40_PLAYABLE_RUNTIME_MAP_AND_DISPATCH_LOOP.md"
require_file "$ROOT/docs/art/42_PLAYABLE_RUNTIME_MAP_OBJECTS_AND_ZONES.md"
require_file "$ROOT/docs/audio/25_PLAYABLE_RUNTIME_MUSIC_AND_ALERT_PRIORITIES.md"
require_file "$ROOT/docs/gameplay/38_FIRST_PLAYABLE_MISSION_FLOW_END_TO_END.md"
require_file "$ROOT/docs/backend/12_PLAYABLE_RUNTIME_MAP_API.md"
require_file "$ROOT/data/content/playable-runtime-map.v1.json"
require_file "$ROOT/data/content/objective-state-machine.v1.json"
require_file "$ROOT/data/content/dispatcher-loop.v1.json"
require_file "$ROOT/data/content/city-pressure-runtime.v1.json"
require_file "$ROOT/data/content/report-progression.v1.json"
require_file "$ROOT/data/reference/reference-data.v15.playableflow.json"
require_file "$ROOT/src/Alarm112.Contracts/PlayableRuntimeMapDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/CityStatusDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/ObjectiveStateTransitionDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/DispatcherLoopDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/ReportProgressionDto.cs"
require_file "$ROOT/src/Alarm112.Application/Interfaces/IPlayableRuntimeService.cs"
require_file "$ROOT/src/Alarm112.Application/Services/PlayableRuntimeService.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Session/PlayableRuntimeMapController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Session/DispatcherLoopController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Session/LiveCityStatusController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Session/ObjectiveStateMachineController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/UI/Reports/ReportProgressionController.cs"

grep -q '/api/playable-runtime-map/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/objective-state-machine/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/dispatcher-loop/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/city-pressure-runtime/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/report-progression/demo' "$ROOT/src/Alarm112.Api/Program.cs"

echo "V15 smoke passed."
