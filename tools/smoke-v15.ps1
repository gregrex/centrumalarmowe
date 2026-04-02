$ErrorActionPreference = "Stop"
$Root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$required = @(
  "docs/implementation/26_V15_PLAYABLE_RUNTIME_MAP_SCOPE.md",
  "docs/ui/40_PLAYABLE_RUNTIME_MAP_AND_DISPATCH_LOOP.md",
  "docs/art/42_PLAYABLE_RUNTIME_MAP_OBJECTS_AND_ZONES.md",
  "docs/audio/25_PLAYABLE_RUNTIME_MUSIC_AND_ALERT_PRIORITIES.md",
  "docs/gameplay/38_FIRST_PLAYABLE_MISSION_FLOW_END_TO_END.md",
  "docs/backend/12_PLAYABLE_RUNTIME_MAP_API.md",
  "data/content/playable-runtime-map.v1.json",
  "data/content/objective-state-machine.v1.json",
  "data/content/dispatcher-loop.v1.json",
  "data/content/city-pressure-runtime.v1.json",
  "data/content/report-progression.v1.json",
  "data/reference/reference-data.v15.playableflow.json",
  "src/Alarm112.Contracts/PlayableRuntimeMapDto.cs",
  "src/Alarm112.Contracts/CityStatusDto.cs",
  "src/Alarm112.Contracts/ObjectiveStateTransitionDto.cs",
  "src/Alarm112.Contracts/DispatcherLoopDto.cs",
  "src/Alarm112.Contracts/ReportProgressionDto.cs",
  "src/Alarm112.Application/Interfaces/IPlayableRuntimeService.cs",
  "src/Alarm112.Application/Services/PlayableRuntimeService.cs",
  "client-unity/Assets/Scripts/Runtime/Session/PlayableRuntimeMapController.cs",
  "client-unity/Assets/Scripts/Runtime/Session/DispatcherLoopController.cs",
  "client-unity/Assets/Scripts/Runtime/Session/LiveCityStatusController.cs",
  "client-unity/Assets/Scripts/Runtime/Session/ObjectiveStateMachineController.cs",
  "client-unity/Assets/Scripts/Runtime/UI/Reports/ReportProgressionController.cs"
)
foreach ($rel in $required) {
  $full = Join-Path $Root $rel
  if (-not (Test-Path $full)) { throw "Missing required file: $full" }
}
$program = Get-Content (Join-Path $Root "src/Alarm112.Api/Program.cs") -Raw
foreach ($needle in @('/api/playable-runtime-map/demo','/api/objective-state-machine/demo','/api/dispatcher-loop/demo','/api/city-pressure-runtime/demo','/api/report-progression/demo')) {
  if ($program -notmatch [regex]::Escape($needle)) { throw "Missing endpoint: $needle" }
}
Write-Host "V15 smoke passed."
