#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"

require_file () {
  if [ ! -f "$1" ]; then
    echo "Missing required file: $1" >&2
    exit 1
  fi
}

require_file "$ROOT/docs/implementation/08_V6_CITYMAP_REALTIME_SCOPE.md"
require_file "$ROOT/docs/ui/12_CITYMAP_LIVE_UNITS_AND_INCIDENT_CARD.md"
require_file "$ROOT/docs/gameplay/13_CITYMAP_DISPATCH_RUNTIME.md"
require_file "$ROOT/docs/art/13_PREMIUM_2D_CITYMAP_AND_MARKERS.md"
require_file "$ROOT/data/content/city-map.v1.json"
require_file "$ROOT/data/content/unit-roster.live.v1.json"
require_file "$ROOT/data/content/incident-actions.v1.json"
require_file "$ROOT/data/content/report-timeline-demo.v1.json"
require_file "$ROOT/src/Alarm112.Contracts/CityMapDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/DispatchCommandDto.cs"
require_file "$ROOT/src/Alarm112.Application/Interfaces/ICityMapService.cs"
require_file "$ROOT/src/Alarm112.Application/Services/CityMapService.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Map/CityMapController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Map/LiveUnitListController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Map/IncidentCardController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Map/RoundTimelineController.cs"

echo "V6 smoke passed."
