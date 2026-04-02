$Root = Split-Path -Parent $PSScriptRoot
$files = @(
  'docs/implementation/22_V13_BRIEFING_READINESS_REPORT_SCOPE.md',
  'docs/implementation/23_NEXT_600_TASKS_BRIEFING_REPORT_FINALFLOW.md',
  'docs/ui/33_MISSION_BRIEFING_SCREEN.md',
  'docs/ui/34_TEAM_READINESS_ROLE_CARDS.md',
  'docs/ui/35_POSTROUND_GRADING_REPORT_SCREEN.md',
  'docs/art/35_MISSION_BRIEFING_SCENES_AND_BRIEFING_OBJECTS.md',
  'docs/audio/19_MISSION_BRIEFING_AND_READYCHECK_AUDIO.md',
  'docs/gameplay/30_PREMISSION_BRIEFING_READYCHECK_FLOW.md',
  'docs/backend/10_BRIEFING_READYCHECK_REPORT_API.md',
  'data/content/mission-briefing.v1.json',
  'data/content/team-readiness.v1.json',
  'data/content/postround-report.v1.json',
  'data/content/mission-complete-flow.v1.json',
  'data/reference/reference-data.v13.briefingreport.json',
  'src/Alarm112.Contracts/MissionBriefingDto.cs',
  'src/Alarm112.Contracts/TeamReadinessDto.cs',
  'src/Alarm112.Contracts/PostRoundReportDto.cs',
  'src/Alarm112.Application/Interfaces/IMissionFlowService.cs',
  'src/Alarm112.Application/Services/MissionFlowService.cs',
  'client-unity/Assets/Scripts/Runtime/Menu/MissionBriefingController.cs',
  'client-unity/Assets/Scripts/Runtime/Menu/TeamReadinessController.cs',
  'client-unity/Assets/Scripts/Runtime/UI/Reports/PostRoundReportController.cs',
  'client-unity/Assets/Scripts/Runtime/Session/MissionCompleteFlowController.cs'
)
foreach ($file in $files) {
  $full = Join-Path $Root $file
  if (-not (Test-Path $full)) { throw "Missing required file: $full" }
}

$program = Get-Content (Join-Path $Root 'src/Alarm112.Api/Program.cs') -Raw
if ($program -notmatch '/api/mission-briefing/demo') { throw 'Missing mission briefing endpoint' }
if ($program -notmatch '/api/team-readiness/demo') { throw 'Missing team readiness endpoint' }
if ($program -notmatch '/api/postround-report/demo') { throw 'Missing postround report endpoint' }
if ($program -notmatch '/api/mission-complete-flow/demo') { throw 'Missing mission complete flow endpoint' }

Write-Host 'V13 smoke passed.'
