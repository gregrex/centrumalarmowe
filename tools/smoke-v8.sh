#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"

require_file () {
  if [ ! -f "$1" ]; then
    echo "Missing required file: $1" >&2
    exit 1
  fi
}

require_file "$ROOT/docs/implementation/12_V8_ROUTE_OVERLAY_LIVE_DELTAS_SCOPE.md"
require_file "$ROOT/docs/ui/17_ROUTE_OVERLAY_ON_MAP.md"
require_file "$ROOT/docs/ui/18_SPLITSCREEN_ROLE_PANELS.md"
require_file "$ROOT/docs/gameplay/18_HALF_PLAYABLE_COOP_ROUND_LOOP.md"
require_file "$ROOT/docs/network/09_LIVE_INCIDENT_DELTAS_AND_ROUND_TICKS.md"
require_file "$ROOT/docs/art/17_ROUTE_OVERLAY_SPLITPANELS_AND_HEATMAP_PROMPTS.md"
require_file "$ROOT/data/content/route-overlay.v1.json"
require_file "$ROOT/data/content/live-incident-deltas.v1.json"
require_file "$ROOT/data/content/coop-round-loop.v1.json"
require_file "$ROOT/data/content/unit-cooldowns.v1.json"
require_file "$ROOT/data/reference/reference-data.v8.rounds.json"
require_file "$ROOT/data/ui/splitscreen_panels.v1.json"
require_file "$ROOT/src/Alarm112.Contracts/RouteOverlayDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/IncidentDeltaDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/UnitRuntimeDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/RoundStateDto.cs"
require_file "$ROOT/src/Alarm112.Application/Interfaces/IRoundRuntimeService.cs"
require_file "$ROOT/src/Alarm112.Application/Services/RoundRuntimeService.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Map/RouteOverlayMapController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Map/LiveIncidentDeltaFeedController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Map/UnitRuntimeListController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/UI/SplitScreen/RoleSplitPanelController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Session/HalfPlayableRoundController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Bots/RoleBotDeltaResponder.cs"

grep -q '/api/sessions/{sessionId}/route-overlay' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/sessions/{sessionId}/round-state' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/sessions/{sessionId}/live-deltas' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/sessions/{sessionId}/units/runtime' "$ROOT/src/Alarm112.Api/Program.cs"

echo "V8 smoke passed."
