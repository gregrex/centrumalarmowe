$root = Split-Path -Parent $PSScriptRoot
$files = @(
  'docs/implementation/20_V12_CHAPTER_RUNTIME_ROLE_SELECTION_SCOPE.md',
  'docs/implementation/21_NEXT_550_TASKS_RUNTIME_BOOTSTRAP_FINAL_VSLICE.md',
  'data/content/chapter-runtime.v1.json',
  'data/content/role-selection-botfill.v1.json',
  'data/content/round-bootstrap.v1.json',
  'src/Alarm112.Contracts/RoundBootstrapDto.cs',
  'src/Alarm112.Application/Services/RuntimeBootstrapService.cs',
  'client-unity/Assets/Scripts/Runtime/Menu/ChapterRuntimeController.cs'
)
foreach ($file in $files) {
  $full = Join-Path $root $file
  if (-not (Test-Path $full)) { throw "Missing required file: $file" }
}
Write-Host 'V12 smoke passed.'
