$Root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$files = @(
  'docs/implementation/10_V7_HUD_FILTERS_ROUTE_SCOPE.md',
  'docs/ui/14_OPERATOR_DISPATCHER_REAL_HUD.md',
  'docs/ui/15_ACTIVE_INCIDENT_BOARD_AND_FILTERS.md',
  'docs/gameplay/16_SHARED_COOP_ACTIONS_AND_CONFIRMATIONS.md',
  'docs/art/15_PREMIUM_HUD_V3_AND_ROLE_FOCUSED_READABILITY.md',
  'data/content/active-incidents.v1.json',
  'data/content/map-filters.v1.json',
  'data/content/shared-actions.v1.json',
  'data/content/route-preview-demo.v1.json',
  'src/Alarm112.Contracts/ActiveIncidentDto.cs',
  'src/Alarm112.Contracts/RoutePreviewDto.cs',
  'src/Alarm112.Application/Interfaces/IOperationsBoardService.cs',
  'src/Alarm112.Application/Services/OperationsBoardService.cs',
  'client-unity/Assets/Scripts/Runtime/Map/ActiveIncidentBoardController.cs',
  'client-unity/Assets/Scripts/Runtime/Map/MapFilterBarController.cs',
  'client-unity/Assets/Scripts/Runtime/Map/RoutePreviewController.cs',
  'client-unity/Assets/Scripts/Runtime/Coop/SharedActionPanelController.cs'
)
foreach ($file in $files) {
  $full = Join-Path $Root $file
  if (-not (Test-Path $full)) { throw "Missing required file: $file" }
}
$program = Get-Content (Join-Path $Root 'src/Alarm112.Api/Program.cs') -Raw
if ($program -notmatch '/api/sessions/\{sessionId\}/active-incidents') { throw 'Missing active-incidents endpoint' }
if ($program -notmatch '/api/map-filters') { throw 'Missing map-filters endpoint' }
if ($program -notmatch '/api/sessions/\{sessionId\}/route-preview') { throw 'Missing route-preview endpoint' }
if ($program -notmatch '/api/sessions/\{sessionId\}/shared-actions') { throw 'Missing shared-actions endpoint' }
Write-Host 'V7 smoke passed.'
