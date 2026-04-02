$Root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$required = @(
  'docs/implementation/36_V20_SHOWCASE_DEMO_PACKAGE_SCOPE.md',
  'docs/ui/56_FIRST_TIME_USER_EXPERIENCE_AND_ONBOARDING.md',
  'docs/ui/57_SHOWCASE_DEMO_FLOW_AND_CAPTURE_PLAN.md',
  'docs/ui/58_PLAYER_FACING_POLISH_PACK.md',
  'data/content/showcase-mission.v1.json',
  'data/content/onboarding-flow.v1.json',
  'src/Alarm112.Contracts/ShowcaseMissionDto.cs',
  'src/Alarm112.Application/Services/ShowcaseDemoService.cs'
)
foreach ($item in $required) {
  $full = Join-Path $Root $item
  if (!(Test-Path $full)) { throw "Missing required file: $item" }
}
Write-Host 'V20 smoke passed.'
