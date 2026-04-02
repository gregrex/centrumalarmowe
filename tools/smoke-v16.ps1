$Root = Split-Path -Parent $PSScriptRoot
$required = @(
  "docs/implementation/28_V16_LIVE_ROUTE_OBJECTIVE_TIMER_SCOPE.md",
  "docs/ui/43_LIVE_ROUTE_RENDERER_AND_ROUND_TIMER.md",
  "docs/art/45_LIVE_ROUTE_RENDERER_MARKERS_TRAILS_V3.md",
  "docs/audio/28_LIVE_ROUTE_TIMER_CHAIN_ALERT_AUDIO.md",
  "docs/gameplay/39_LIVE_ROUTE_RENDERER_AND_ROUND_TIMER_RULES.md",
  "docs/backend/13_LIVE_ROUTE_TIMER_CHAIN_API.md",
  "data/content/live-route-runtime.v1.json",
  "data/content/round-timer.v1.json",
  "data/content/chain-escalation-runtime.v1.json",
  "data/content/demo-mission-polish.v1.json",
  "data/reference/reference-data.v16.liveroute.json",
  "src/Alarm112.Contracts/LiveRouteRuntimeDto.cs",
  "src/Alarm112.Contracts/LiveRouteSegmentStateDto.cs",
  "src/Alarm112.Contracts/RoundTimerDto.cs",
  "src/Alarm112.Contracts/ChainEscalationStateDto.cs",
  "src/Alarm112.Contracts/DemoMissionPolishDto.cs",
  "src/Alarm112.Application/Interfaces/IRuntimePolishService.cs",
  "src/Alarm112.Application/Services/RuntimePolishService.cs",
  "client-unity/Assets/Scripts/Runtime/Map/LiveRouteRendererController.cs",
  "client-unity/Assets/Scripts/Runtime/Session/RoundTimerController.cs",
  "client-unity/Assets/Scripts/Runtime/Session/ChainEscalationController.cs",
  "client-unity/Assets/Scripts/Runtime/UI/ObjectiveTransitionHudController.cs",
  "client-unity/Assets/Scripts/Runtime/Art/DemoMissionPolishController.cs"
)
foreach ($item in $required) { if (-not (Test-Path (Join-Path $Root $item))) { throw "Missing required file: $item" } }
$program = Get-Content (Join-Path $Root "src/Alarm112.Api/Program.cs") -Raw
foreach ($endpoint in @("/api/live-route-runtime/demo","/api/round-timer/demo","/api/chain-escalation-runtime/demo","/api/demo-mission-polish/demo")) {
  if ($program -notmatch [regex]::Escape($endpoint)) { throw "Missing endpoint: $endpoint" }
}
Write-Host "V16 smoke passed."
