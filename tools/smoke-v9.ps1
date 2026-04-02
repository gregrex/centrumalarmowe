$Root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$required = @(
  "docs/implementation/14_V9_MENU_ART_AUDIO_SCOPE.md",
  "docs/ui/20_MAIN_MENU_META_PROGRESS_AND_SCENE_FLOW.md",
  "docs/art/19_MAIN_MENU_SCENES_AND_HERO_OBJECTS.md",
  "docs/audio/07_MENU_AUDIO_THEME_AND_MUSIC_STATES.md",
  "data/content/menu-flow.v1.json",
  "data/audio/menu_music_states.v1.json",
  "src/Alarm112.Contracts/ThemePackDto.cs",
  "src/Alarm112.Application/Services/ThemePackService.cs",
  "client-unity/Assets/Scripts/Runtime/Menu/MainMenuSceneController.cs",
  "client-unity/Assets/Scripts/Runtime/Audio/MenuMusicStateController.cs"
)
foreach ($item in $required) {
  $path = Join-Path $Root $item
  if (-not (Test-Path $path)) { throw "Missing required file: $path" }
}
Write-Host "V9 smoke passed."
