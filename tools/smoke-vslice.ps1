
$Root = Split-Path -Parent $PSScriptRoot
Write-Host "[smoke-vslice] validating vertical slice files"
$files = @(
  "data/content/vertical-slice.quickplay.json",
  "data/content/incident-type-catalog.v2.json",
  "data/content/dispatch-unit-roster.json",
  "data/content/status-effects.json",
  "data/content/mission-pack-vslice.json",
  "docs/implementation/06_V5_PLAYABLE_VERTICAL_SLICE_SCOPE.md",
  "docs/ui/10_HOME_QUICKPLAY_ENDREPORT_SCREENS.md",
  "docs/gameplay/11_FIRST_3_INCIDENT_TYPES_E2E.md",
  "src/Alarm112.Contracts/SessionReportDto.cs",
  "src/Alarm112.Application/Services/QuickPlayService.cs",
  "client-unity/Assets/Scripts/Runtime/Menu/HomeMenuController.cs"
)
foreach ($rel in $files) {
  $path = Join-Path $Root $rel
  if (!(Test-Path $path)) { throw "Missing $rel" }
}
Write-Host "[smoke-vslice] success"
