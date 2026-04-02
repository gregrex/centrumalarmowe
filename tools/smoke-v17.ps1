$Root = Split-Path -Parent $PSScriptRoot
$files = @(
  "docs/implementation/30_V17_QUASI_PRODUCTION_DEMO_SCOPE.md",
  "docs/ui/46_VISUAL_RUNTIME_ROUTE_LAYER_AND_RECOVERY_CARDS.md",
  "docs/art/48_VISUAL_RUNTIME_ROUTE_LAYER_V4.md",
  "docs/audio/31_RECOVERY_FAILSTATE_REPORT_ROOM_AUDIO.md",
  "docs/gameplay/42_RECOVERY_DECISION_CARDS_AND_FAIL_BRANCH_RULES.md",
  "docs/backend/14_RECOVERY_FAIL_REPORT_API.md",
  "data/content/visual-runtime-route-layer.v1.json",
  "data/content/recovery-decision-cards.v1.json",
  "data/content/mission-fail-branches.v1.json",
  "data/content/report-room-polish.v1.json",
  "data/content/quasi-production-demo-flow.v1.json",
  "data/reference/reference-data.v17.quasiprod.json",
  "src/Alarm112.Contracts/VisualRuntimeRouteLayerDto.cs",
  "src/Alarm112.Contracts/RecoveryDecisionCardDto.cs",
  "src/Alarm112.Contracts/MissionFailBranchDto.cs",
  "src/Alarm112.Contracts/ReportRoomPolishDto.cs",
  "src/Alarm112.Contracts/QuasiProductionDemoFlowDto.cs",
  "src/Alarm112.Application/Interfaces/IQuasiProductionDemoService.cs",
  "src/Alarm112.Application/Services/QuasiProductionDemoService.cs",
  "client-unity/Assets/Scripts/Runtime/Map/VisualRuntimeRouteLayerController.cs",
  "client-unity/Assets/Scripts/Runtime/Session/RecoveryDecisionCardsController.cs",
  "client-unity/Assets/Scripts/Runtime/Session/MissionFailBranchController.cs",
  "client-unity/Assets/Scripts/Runtime/UI/Reports/ReportRoomPolishController.cs",
  "client-unity/Assets/Scripts/Runtime/Session/QuasiProductionDemoFlowController.cs"
)
foreach ($file in $files) {
  $full = Join-Path $Root $file
  if (!(Test-Path $full)) { throw "Missing required file: $file" }
}
$program = Get-Content (Join-Path $Root "src/Alarm112.Api/Program.cs") -Raw
if ($program -notmatch "/api/visual-runtime-route-layer/demo") { throw "Missing route-layer endpoint" }
if ($program -notmatch "/api/recovery-decision-cards/demo") { throw "Missing recovery-cards endpoint" }
if ($program -notmatch "/api/mission-fail-branches/demo") { throw "Missing fail-branches endpoint" }
if ($program -notmatch "/api/report-room-polish/demo") { throw "Missing report-room endpoint" }
if ($program -notmatch "/api/quasi-production-demo-flow/demo") { throw "Missing demo-flow endpoint" }
Write-Host "V17 smoke passed."
