$Root = Split-Path -Parent $PSScriptRoot
$required = @(
  "docs/implementation/42_V23_ANDROID_PREVIEW_AND_RELEASE_READINESS_SCOPE.md",
  "src/Alarm112.Application/Services/AndroidPreviewService.cs",
  "src/Alarm112.Contracts/AndroidPreviewBuildDto.cs",
  "data/content/android-preview-build.v1.json",
  "data/reference/reference-data.v23.androidpreview.json"
)
foreach ($rel in $required) {
  $path = Join-Path $Root $rel
  if (!(Test-Path $path)) { throw "Missing required file: $rel" }
}
Write-Host "smoke-v23 ok"
