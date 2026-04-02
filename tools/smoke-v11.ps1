$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot

$files = @(
  "docs/implementation/18_V11_CAMPAIGN_ENTRY_BOTFILL_SCOPE.md",
  "docs/ui/26_CAMPAIGN_CHAPTER_MAP_AND_MISSION_ENTRY.md",
  "data/content/campaign-chapters.v1.json",
  "data/content/mission-entry-flow.v1.json",
  "src/Alarm112.Contracts/CampaignChapterDto.cs",
  "src/Alarm112.Application/Services/CampaignEntryService.cs",
  "client-unity/Assets/Scripts/Runtime/Menu/CampaignChapterMapController.cs"
)

foreach ($file in $files) {
  $full = Join-Path $root $file
  if (-not (Test-Path $full)) { throw "Missing required file: $file" }
}

$program = Get-Content (Join-Path $root "src/Alarm112.Api/Program.cs") -Raw
foreach ($pattern in @("/api/campaign-chapters/demo","/api/mission-entry/demo","/api/profile-cosmetics/demo","/api/player-identity/demo","/api/home-to-round-audio")) {
  if ($program -notmatch [regex]::Escape($pattern)) { throw "Missing endpoint $pattern" }
}

Write-Host "V11 smoke passed."
