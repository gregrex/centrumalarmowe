$Root = Split-Path -Parent $PSScriptRoot
$required = @(
  "docs/implementation/16_V10_HOME_FLOW_RENDERER_SCOPE.md",
  "docs/implementation/17_NEXT_450_TASKS_HOME_FLOW_AUDIO_RENDERER.md",
  "docs/ui/23_HOME_HUB_PROTOTYPE.md",
  "docs/audio/10_SCREEN_AUDIO_ROUTING_AND_STINGERS.md",
  "data/content/home-hub.v1.json",
  "src/Alarm112.Application/Services/HomeFlowService.cs"
)
foreach ($file in $required) {
  $full = Join-Path $Root $file
  if (-not (Test-Path $full)) { throw "Missing required file: $full" }
}
Write-Host "V10 smoke passed."
