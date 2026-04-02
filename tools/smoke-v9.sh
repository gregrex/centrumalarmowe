#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"

require_file () {
  if [ ! -f "$1" ]; then
    echo "Missing required file: $1" >&2
    exit 1
  fi
}

require_file "$ROOT/docs/implementation/14_V9_MENU_ART_AUDIO_SCOPE.md"
require_file "$ROOT/docs/ui/20_MAIN_MENU_META_PROGRESS_AND_SCENE_FLOW.md"
require_file "$ROOT/docs/ui/21_MENU_WIDGETS_CARDS_AND_TRANSITIONS.md"
require_file "$ROOT/docs/ui/22_PROFILE_LOADOUT_SETTINGS_AND_ACCESSIBILITY.md"
require_file "$ROOT/docs/art/19_MAIN_MENU_SCENES_AND_HERO_OBJECTS.md"
require_file "$ROOT/docs/art/20_OBJECT_LIBRARY_VEHICLES_BUILDINGS_PROPS.md"
require_file "$ROOT/docs/art/21_2D_SCENE_TILES_BACKGROUND_PARALLAX.md"
require_file "$ROOT/docs/audio/07_MENU_AUDIO_THEME_AND_MUSIC_STATES.md"
require_file "$ROOT/docs/audio/08_SFX_PACK_MENUS_UI_RADIO_CITY.md"
require_file "$ROOT/docs/audio/09_MUSIC_CUE_SHEET_AND_ADAPTIVE_RULES.md"
require_file "$ROOT/data/content/menu-flow.v1.json"
require_file "$ROOT/data/content/meta-progression.v1.json"
require_file "$ROOT/data/content/scene-variants.v1.json"
require_file "$ROOT/data/art/menu_scene_prompt_pack.json"
require_file "$ROOT/data/art/object_library_prompt_pack.json"
require_file "$ROOT/data/art/parallax_layers.v1.json"
require_file "$ROOT/data/audio/menu_music_states.v1.json"
require_file "$ROOT/data/audio/sfx_pack_menu_radio_city.v1.json"
require_file "$ROOT/data/audio/music_cue_sheet.v1.json"
require_file "$ROOT/data/reference/reference-data.v9.menuartaudio.json"
require_file "$ROOT/src/Alarm112.Contracts/ThemePackDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/MenuFlowDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/MetaProgressionDto.cs"
require_file "$ROOT/src/Alarm112.Application/Interfaces/IThemePackService.cs"
require_file "$ROOT/src/Alarm112.Application/Services/ThemePackService.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Menu/MainMenuSceneController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Menu/MenuTransitionController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Menu/ProfileAndLoadoutController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Menu/MetaProgressionPanelController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Audio/MenuMusicStateController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Audio/SfxRouter.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Art/ParallaxSceneController.cs"

grep -q '/api/theme-pack' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/menu-flow' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/meta-progression/demo' "$ROOT/src/Alarm112.Api/Program.cs"

echo "V9 smoke passed."
