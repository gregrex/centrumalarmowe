$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
Write-Host "[content-verify] checking json payloads and basic dictionary rules"

$files = @(
  'data/reference/reference-data.json',
  'data/reference/reference-data.extended.json',
  'data/reference/reference-data.citymap.v1.json',
  'data/reference/reference-data.v7.routing.json',
  'data/reference/reference-data.v8.rounds.json',
  'data/ui/ui-texts.pl-PL.json',
  'data/ui/role_hud_panels.v3.json',
  'data/ui/splitscreen_panels.v1.json',
  'data/balance/spawn_weights.json',
  'data/art/icon_catalog.json',
  'data/art/sprite_atlas_manifest.json',
  'data/art/route_overlay_prompt_pack.json',
  'data/art/route_heatmap_prompt_pack.json',
  'data/content/vertical-slice.quickplay.json',
  'data/content/incident-type-catalog.v2.json',
  'data/content/dispatch-unit-roster.json',
  'data/content/status-effects.json',
  'data/content/city-map.v1.json',
  'data/content/unit-roster.live.v1.json',
  'data/content/incident-actions.v1.json',
  'data/content/report-timeline-demo.v1.json',
  'data/content/active-incidents.v1.json',
  'data/content/map-filters.v1.json',
  'data/content/shared-actions.v1.json',
  'data/content/route-preview-demo.v1.json',
  'data/content/role-bot-profiles.v2.json',
  'data/content/route-overlay.v1.json',
  'data/content/live-incident-deltas.v1.json',
  'data/content/coop-round-loop.v1.json',
  'data/content/unit-cooldowns.v1.json',
  'data/audio/role_audio_priorities.json',
  'data/audio/shared_action_audio.json',
  'data/audio/round_tension_states.json',
  'data/content/mission-briefing.v1.json',
  'data/content/team-readiness.v1.json',
  'data/content/postround-report.v1.json',
  'data/content/reward-track.v1.json',
  'data/content/mission-complete-flow.v1.json',
  'data/art/briefing_prompt_pack.json',
  'data/art/role_cards_team_prompt_pack.v2.json',
  'data/audio/mission_briefing_audio.v1.json',
  'data/audio/postround_report_audio.v1.json',
  'data/audio/dialogue_pack_v3.json',
  'data/ui/mission_briefing_panels.v1.json',
  'data/ui/postround_report_panels.v1.json',
  'data/reference/reference-data.v13.briefingreport.json'
)

foreach ($f in $files) {
  $path = Join-Path $root $f
  if (-not (Test-Path $path)) { throw "Missing file: $path" }
  Get-Content $path -Raw | ConvertFrom-Json | Out-Null
  Write-Host "OK $(Split-Path $path -Leaf)"
}

Write-Host "[content-verify] success"
