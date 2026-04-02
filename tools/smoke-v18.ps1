$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
function Require-File([string]$path) { if (!(Test-Path $path)) { throw "Missing required file: $path" } }
Require-File "$root/docs/implementation/32_V18_RUNTIME_UI_RECOVERY_SCOPE.md"
Require-File "$root/docs/ui/49_RUNTIME_MISSION_HUD_V2.md"
Require-File "$root/docs/ui/50_RECOVERY_CARDS_IN_HUD.md"
Require-File "$root/docs/ui/51_FAIL_RETRY_NEXT_FLOW.md"
Require-File "$root/data/content/runtime-hud.v2.json"
Require-File "$root/src/Alarm112.Contracts/RuntimeHudDto.cs"
Require-File "$root/src/Alarm112.Application/Services/RuntimeUiFlowService.cs"
Require-File "$root/client-unity/Assets/Scripts/Runtime/UI/RuntimeHudV2Controller.cs"
$program = Get-Content "$root/src/Alarm112.Api/Program.cs" -Raw
if ($program -notmatch '/api/runtime-hud/demo') { throw 'Missing runtime-hud endpoint' }
if ($program -notmatch '/api/recovery-hud-triggers/demo') { throw 'Missing recovery-hud-triggers endpoint' }
if ($program -notmatch '/api/fail-retry-next/demo') { throw 'Missing fail-retry-next endpoint' }
if ($program -notmatch '/api/mission-slice-polish/demo') { throw 'Missing mission-slice-polish endpoint' }
Write-Host 'V18 smoke passed.'
