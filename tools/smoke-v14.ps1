$Root = Split-Path -Parent $PSScriptRoot
$files = @(
  'docs/implementation/24_V14_FULL_MISSION_VERTICAL_SLICE_SCOPE.md',
  'docs/ui/37_FULL_MISSION_RUNTIME_DISPATCH_SCREEN.md',
  'docs/art/39_FULL_MISSION_RUNTIME_SCENES_AND_RUNTIME_OBJECTS.md',
  'docs/audio/22_FULL_MISSION_RUNTIME_AUDIO_STACK.md',
  'docs/gameplay/33_FULL_MISSION_VERTICAL_SLICE_RUNTIME.md',
  'docs/backend/11_FULL_MISSION_RUNTIME_API.md',
  'data/content/full-mission-runtime.v1.json',
  'data/content/mission-complete-gate.v1.json',
  'data/content/runtime-dispatch-outcomes.v1.json',
  'data/content/objective-tracker.v1.json',
  'data/content/mission-script.01.full.json',
  'data/reference/reference-data.v14.fullmission.json',
  'src/Alarm112.Contracts/MissionRuntimeStateDto.cs',
  'src/Alarm112.Contracts/MissionCompleteGateDto.cs',
  'src/Alarm112.Contracts/ObjectiveTrackerDto.cs',
  'src/Alarm112.Application/Interfaces/IMissionRuntimeService.cs',
  'src/Alarm112.Application/Services/MissionRuntimeService.cs',
  'client-unity/Assets/Scripts/Runtime/Session/MissionRuntimeHudController.cs',
  'client-unity/Assets/Scripts/Runtime/Session/MissionCompleteGateController.cs',
  'client-unity/Assets/Scripts/Runtime/UI/ObjectiveTrackerController.cs',
  'client-unity/Assets/Scripts/Runtime/UI/EventFeedController.cs'
)
foreach ($file in $files) {
  $full = Join-Path $Root $file
  if (-not (Test-Path $full)) { throw "Missing required file: $full" }
}

$program = Get-Content (Join-Path $Root 'src/Alarm112.Api/Program.cs') -Raw
if ($program -notmatch '/api/mission-runtime/demo') { throw 'Missing mission runtime endpoint' }
if ($program -notmatch '/api/mission-complete-gate/demo') { throw 'Missing mission complete gate endpoint' }
if ($program -notmatch '/api/runtime-dispatch-outcomes/demo') { throw 'Missing runtime dispatch outcomes endpoint' }
if ($program -notmatch '/api/objective-tracker/demo') { throw 'Missing objective tracker endpoint' }
if ($program -notmatch '/api/mission-script/demo') { throw 'Missing mission script endpoint' }

Write-Host 'V14 smoke passed.'
