#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"

require_file () {
  if [ ! -f "$1" ]; then
    echo "Missing required file: $1" >&2
    exit 1
  fi
}

require_file "$ROOT/docs/implementation/18_V11_CAMPAIGN_ENTRY_BOTFILL_SCOPE.md"
require_file "$ROOT/docs/implementation/19_NEXT_500_TASKS_CAMPAIGN_ENTRY_ONLINE_OFFLINE.md"
require_file "$ROOT/docs/ui/26_CAMPAIGN_CHAPTER_MAP_AND_MISSION_ENTRY.md"
require_file "$ROOT/docs/ui/27_HOME_TO_ROUND_ENTRY_FLOW.md"
require_file "$ROOT/docs/ui/28_PORTRAITS_COSMETICS_AND_PLAYER_IDENTITY.md"
require_file "$ROOT/docs/art/27_ANIMATED_MENU_DAY_NIGHT_WEATHER_RENDERER.md"
require_file "$ROOT/docs/art/28_CHAPTER_MAP_SCENES_AND_NODE_OBJECTS.md"
require_file "$ROOT/docs/art/29_PROFILE_COSMETICS_PORTRAITS_NAMEPLATES.md"
require_file "$ROOT/docs/audio/13_HOME_TO_ROUND_AUDIO_TRANSITIONS.md"
require_file "$ROOT/docs/audio/14_CAMPAIGN_MAP_MUSIC_LAYERING.md"
require_file "$ROOT/docs/audio/15_PORTRAIT_UI_COSMETIC_SFX.md"
require_file "$ROOT/data/content/campaign-chapters.v1.json"
require_file "$ROOT/data/content/mission-entry-flow.v1.json"
require_file "$ROOT/data/content/profile-cosmetics.v1.json"
require_file "$ROOT/data/content/portrait-packs.v1.json"
require_file "$ROOT/data/content/animated-menu-weather.v1.json"
require_file "$ROOT/data/content/chapter-map-nodes.v1.json"
require_file "$ROOT/data/audio/home_to_round_audio.v1.json"
require_file "$ROOT/data/audio/campaign_map_music_layers.v1.json"
require_file "$ROOT/data/ui/chapter_map_panels.v1.json"
require_file "$ROOT/data/reference/reference-data.v11.campaignentry.json"
require_file "$ROOT/src/Alarm112.Contracts/CampaignChapterDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/CampaignMissionEntryDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/ProfileCosmeticDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/PlayerIdentityDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/HomeToRoundAudioDto.cs"
require_file "$ROOT/src/Alarm112.Application/Interfaces/ICampaignEntryService.cs"
require_file "$ROOT/src/Alarm112.Application/Services/CampaignEntryService.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Menu/CampaignChapterMapController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Menu/MissionEntryFlowController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Menu/PlayerIdentityPanelController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Art/AnimatedMenuWeatherController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Audio/HomeToRoundAudioTransitionController.cs"

grep -q '/api/campaign-chapters/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/mission-entry/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/profile-cosmetics/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/player-identity/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/home-to-round-audio' "$ROOT/src/Alarm112.Api/Program.cs"

echo "V11 smoke passed."
