#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"

require_file () {
  if [ ! -f "$1" ]; then
    echo "Missing required file: $1" >&2
    exit 1
  fi
}

require_file "$ROOT/docs/implementation/20_V12_CHAPTER_RUNTIME_ROLE_SELECTION_SCOPE.md"
require_file "$ROOT/docs/implementation/21_NEXT_550_TASKS_RUNTIME_BOOTSTRAP_FINAL_VSLICE.md"
require_file "$ROOT/docs/ui/29_CHAPTER_MAP_RUNTIME_AND_NODE_STATES.md"
require_file "$ROOT/docs/ui/30_ROLE_SELECTION_BOT_FILL_PREVIEW.md"
require_file "$ROOT/docs/ui/31_ROUND_BOOTSTRAP_ONLINE_OFFLINE_SWITCH.md"
require_file "$ROOT/docs/art/31_FINAL_VERTICAL_SLICE_OBJECT_SCENE_LIBRARY.md"
require_file "$ROOT/docs/art/32_PREMIUM_2D_ROUND_BOOTSTRAP_AND_ROLE_CARDS.md"
require_file "$ROOT/docs/audio/16_ROUND_BOOTSTRAP_AND_ROLE_LOCK_AUDIO.md"
require_file "$ROOT/docs/audio/17_FINAL_VERTICAL_SLICE_MUSIC_STACK.md"
require_file "$ROOT/data/content/chapter-runtime.v1.json"
require_file "$ROOT/data/content/role-selection-botfill.v1.json"
require_file "$ROOT/data/content/round-bootstrap.v1.json"
require_file "$ROOT/data/content/objectives-grading.v1.json"
require_file "$ROOT/data/content/final-vertical-slice-scenes.v1.json"
require_file "$ROOT/data/audio/round_bootstrap_audio.v1.json"
require_file "$ROOT/data/audio/final_vertical_slice_music.v1.json"
require_file "$ROOT/data/ui/role_selection_panels.v1.json"
require_file "$ROOT/data/reference/reference-data.v12.runtimebootstrap.json"
require_file "$ROOT/src/Alarm112.Contracts/CampaignNodeRuntimeDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/RoleSelectionSlotDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/BotFillPreviewDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/RoundBootstrapDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/ObjectiveGradeDto.cs"
require_file "$ROOT/src/Alarm112.Contracts/VerticalSliceScenePackDto.cs"
require_file "$ROOT/src/Alarm112.Application/Interfaces/IRuntimeBootstrapService.cs"
require_file "$ROOT/src/Alarm112.Application/Services/RuntimeBootstrapService.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Menu/ChapterRuntimeController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Menu/RoleSelectionBotFillController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Session/RoundBootstrapController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Session/OfflineOnlineSwitchController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Art/FinalVerticalSliceScenePackController.cs"
require_file "$ROOT/client-unity/Assets/Scripts/Runtime/Audio/RoundBootstrapAudioController.cs"

grep -q '/api/chapter-runtime/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/role-selection/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/round-bootstrap/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/objectives-grading/demo' "$ROOT/src/Alarm112.Api/Program.cs"
grep -q '/api/final-vertical-slice/scenes' "$ROOT/src/Alarm112.Api/Program.cs"

echo "V12 smoke passed."
