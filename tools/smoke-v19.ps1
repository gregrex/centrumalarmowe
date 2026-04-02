$root = Split-Path -Parent $PSScriptRoot
$files = @(
  'docs/implementation/34_V19_SCOREBOARD_REWARDS_RETRYPREP_SCOPE.md',
  'docs/ui/53_RUNTIME_SCOREBOARD_AND_REWARD_REVEAL.md',
  'docs/ui/54_RETRY_PREPARATION_AND_NEXT_MISSION_HANDOFF.md',
  'docs/ui/55_NEAR_FINAL_VERTICAL_SLICE_FLOW.md',
  'docs/art/54_NEAR_FINAL_2D_OBJECTS_SCENES_V6.md',
  'docs/audio/37_RUNTIME_SCOREBOARD_REWARD_AUDIO.md',
  'docs/gameplay/48_RUNTIME_SCOREBOARD_AND_REWARD_REVEAL_RULES.md',
  'docs/network/18_SCOREBOARD_REWARD_RETRY_SYNC.md',
  'docs/backend/16_SCOREBOARD_REWARD_RETRYPREP_API.md',
  'docs/data/36_SCOREBOARD_REWARD_RETRY_DICTIONARIES.md'
)
foreach ($file in $files) {
  $full = Join-Path $root $file
  if (-not (Test-Path $full)) { throw "Missing required file: $file" }
}
$program = Get-Content (Join-Path $root 'src/Alarm112.Api/Program.cs') -Raw
@('/api/runtime-scoreboard/demo','/api/reward-reveal/demo','/api/retry-preparation/demo','/api/next-mission-handoff/demo','/api/near-final-slice-flow/demo') | ForEach-Object {
  if ($program -notmatch [regex]::Escape($_)) { throw "Missing endpoint $_" }
}
Write-Host 'V19 smoke passed.'
