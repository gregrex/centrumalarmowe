#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"

require_file () {
  if [ ! -f "$1" ]; then
    echo "Missing required file: $1" >&2
    exit 1
  fi
}

require_file "$ROOT/docs/implementation/16_V10_HOME_FLOW_RENDERER_SCOPE.md"
require_file "$ROOT/docs/implementation/17_NEXT_450_TASKS_HOME_FLOW_AUDIO_RENDERER.md"
require_file "$ROOT/docs/ui/23_HOME_HUB_PROTOTYPE.md"
require_file "$ROOT/docs/ui/24_CAMPAIGN_QUICKPLAY_COOP_PROFILE_SETTINGS_FLOW.md"
require_file "$ROOT/docs/ui/25_SETTINGS_AUDIO_ACCESSIBILITY_PANELS.md"
require_file "$ROOT/docs/art/23_MAIN_MENU_SCENE_RENDERER_PACK.md"
require_file "$ROOT/docs/art/24_OBJECT_LIBRARY_EXPANDED_300.md"
require_file "$ROOT/docs/audio/10_SCREEN_AUDIO_ROUTING_AND_STINGERS.md"
require_file "$ROOT/docs/audio/11_COOP_LOBBY_AND_SESSION_MUSIC_PACK.md"
require_file "$ROOT/docs/audio/12_VOICE_BARKS_RADIO_CALLS_PACK.md"
require_file "$ROOT/data/content/home-hub.v1.json"
require_file "$ROOT/data/content/campaign-overview.v1.json"
require_file "$ROOT/data/content/daily-challenges.v1.json"
require_file "$ROOT/data/content/settings-bundle.v1.json"
require_file "$ROOT/data/art/main_menu_renderer_layers.v1.json"
require_file "$ROOT/data/audio/screen_audio_routing.v1.json"
require_file "$ROOT/data/reference/reference-data.v10.homeflow.json"
require_file "$ROOT/src/Alarm112.Contracts/HomeHubDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/CampaignOverviewDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/SettingsBundleDto.cs"
require_file "$ROOT/src/Alarm112.Application/Interfaces/IHomeFlowService.cs"
require_file "$ROOT/src/Alarm112.Application/Services/HomeFlowService.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Menu/HomeHubController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Menu/CampaignOverviewController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Menu/SettingsBundleController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Art/MainMenuRendererController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Audio/ScreenAudioRouter.cs"

grep -q '/api/home-hub' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/campaign-overview/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/daily-challenges/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/settings-bundle' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/audio-routes' "$ROOT/src/Alarm112.Api/Program.cs"

echo "V10 smoke passed."
