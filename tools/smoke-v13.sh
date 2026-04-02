#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"

require_file () {
  if [ ! -f "$1" ]; then
    echo "Missing required file: $1" >&2
    exit 1
  fi
}

require_file "$ROOT/docs/implementation/22_V13_BRIEFING_READINESS_REPORT_SCOPE.md"
require_file "$ROOT/docs/implementation/23_NEXT_600_TASKS_BRIEFING_REPORT_FINALFLOW.md"
require_file "$ROOT/docs/ui/33_MISSION_BRIEFING_SCREEN.md"
require_file "$ROOT/docs/ui/34_TEAM_READINESS_ROLE_CARDS.md"
require_file "$ROOT/docs/ui/35_POSTROUND_GRADING_REPORT_SCREEN.md"
require_file "$ROOT/docs/art/35_MISSION_BRIEFING_SCENES_AND_BRIEFING_OBJECTS.md"
require_file "$ROOT/docs/audio/19_MISSION_BRIEFING_AND_READYCHECK_AUDIO.md"
require_file "$ROOT/docs/gameplay/30_PREMISSION_BRIEFING_READYCHECK_FLOW.md"
require_file "$ROOT/docs/backend/10_BRIEFING_READYCHECK_REPORT_API.md"
require_file "$ROOT/data/content/mission-briefing.v1.json"
require_file "$ROOT/data/content/team-readiness.v1.json"
require_file "$ROOT/data/content/postround-report.v1.json"
require_file "$ROOT/data/content/mission-complete-flow.v1.json"
require_file "$ROOT/data/reference/reference-data.v13.briefingreport.json"
require_file "$ROOT/src/Alarm112.Contracts/MissionBriefingDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/TeamReadinessDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/PostRoundReportDto.cs"
require_file "$ROOT/src/Alarm112.Application/Interfaces/IMissionFlowService.cs"
require_file "$ROOT/src/Alarm112.Application/Services/MissionFlowService.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Menu/MissionBriefingController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Menu/TeamReadinessController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/UI/Reports/PostRoundReportController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Session/MissionCompleteFlowController.cs"

grep -q '/api/mission-briefing/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/team-readiness/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/postround-report/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/mission-complete-flow/demo' "$ROOT/src/Alarm112.Api/Program.cs"

echo "V13 smoke passed."
