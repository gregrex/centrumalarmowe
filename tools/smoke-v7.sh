#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"

require_file () {
  if [ ! -f "$1" ]; then
    echo "Missing required file: $1" >&2
    exit 1
  fi
}

require_file "$ROOT/docs/implementation/10_V7_HUD_FILTERS_ROUTE_SCOPE.md"
require_file "$ROOT/docs/ui/14_OPERATOR_DISPATCHER_REAL_HUD.md"
require_file "$ROOT/docs/ui/15_ACTIVE_INCIDENT_BOARD_AND_FILTERS.md"
require_file "$ROOT/docs/gameplay/16_SHARED_COOP_ACTIONS_AND_CONFIRMATIONS.md"
require_file "$ROOT/docs/art/15_PREMIUM_HUD_V3_AND_ROLE_FOCUSED_READABILITY.md"
require_file "$ROOT/data/content/active-incidents.v1.json"
require_file "$ROOT/data/content/map-filters.v1.json"
require_file "$ROOT/data/content/shared-actions.v1.json"
require_file "$ROOT/data/content/route-preview-demo.v1.json"
require_file "$ROOT/src/Alarm112.Contracts/ActiveIncidentDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/RoutePreviewDto.cs"
require_file "$ROOT/src/Alarm112.Application/Interfaces/IOperationsBoardService.cs"
require_file "$ROOT/src/Alarm112.Application/Services/OperationsBoardService.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Map/ActiveIncidentBoardController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Map/MapFilterBarController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Map/RoutePreviewController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Coop/SharedActionPanelController.cs"

grep -q '/api/sessions/{sessionId}/active-incidents' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/map-filters' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/sessions/{sessionId}/route-preview' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/sessions/{sessionId}/shared-actions' "$ROOT/src/Alarm112.Api/Program.cs"

echo "V7 smoke passed."
