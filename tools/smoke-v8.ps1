$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot

$files = @(
  'docs/implementation/12_V8_ROUTE_OVERLAY_LIVE_DELTAS_SCOPE.md',
  'docs/ui/17_ROUTE_OVERLAY_ON_MAP.md',
  'docs/ui/18_SPLITSCREEN_ROLE_PANELS.md',
  'docs/gameplay/18_HALF_PLAYABLE_COOP_ROUND_LOOP.md',
  'docs/network/09_LIVE_INCIDENT_DELTAS_AND_ROUND_TICKS.md',
  'docs/art/17_ROUTE_OVERLAY_SPLITPANELS_AND_HEATMAP_PROMPTS.md',
  'data/content/route-overlay.v1.json',
  'data/content/live-incident-deltas.v1.json',
  'data/content/coop-round-loop.v1.json',
  'data/content/unit-cooldowns.v1.json',
  'data/reference/reference-data.v8.rounds.json',
  'data/ui/splitscreen_panels.v1.json',
  'src/Alarm112.Contracts/RouteOverlayDto.cs',
  'src/Alarm112.Contracts/IncidentDeltaDto.cs',
  'src/Alarm112.Contracts/UnitRuntimeDto.cs',
  'src/Alarm112.Contracts/RoundStateDto.cs',
  'src/Alarm112.Application/Interfaces/IRoundRuntimeService.cs',
  'src/Alarm112.Application/Services/RoundRuntimeService.cs',
  'client-unity/Assets/Scripts/Runtime/Map/RouteOverlayMapController.cs',
  'client-unity/Assets/Scripts/Runtime/Map/LiveIncidentDeltaFeedController.cs',
  'client-unity/Assets/Scripts/Runtime/Map/UnitRuntimeListController.cs',
  'client-unity/Assets/Scripts/Runtime/UI/SplitScreen/RoleSplitPanelController.cs',
  'client-unity/Assets/Scripts/Runtime/Session/HalfPlayableRoundController.cs',
  'client-unity/Assets/Scripts/Runtime/Bots/RoleBotDeltaResponder.cs'
)

foreach ($f in $files) {
  $path = Join-Path $root $f
  if (-not (Test-Path $path)) { throw "Missing required file: $path" }
}

$program = Get-Content (Join-Path $root 'src/Alarm112.Api/Program.cs') -Raw
if ($program -notmatch '/api/sessions/\{sessionId\}/route-overlay') { throw 'Missing route-overlay endpoint' }
if ($program -notmatch '/api/sessions/\{sessionId\}/round-state') { throw 'Missing round-state endpoint' }
if ($program -notmatch '/api/sessions/\{sessionId\}/live-deltas') { throw 'Missing live-deltas endpoint' }
if ($program -notmatch '/api/sessions/\{sessionId\}/units/runtime') { throw 'Missing units/runtime endpoint' }

Write-Host 'V8 smoke passed.'
