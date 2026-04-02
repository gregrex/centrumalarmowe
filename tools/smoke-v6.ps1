$root = Split-Path -Parent $PSScriptRoot

$required = @(
    "docs/implementation/08_V6_CITYMAP_REALTIME_SCOPE.md",
    "docs/ui/12_CITYMAP_LIVE_UNITS_AND_INCIDENT_CARD.md",
    "docs/gameplay/13_CITYMAP_DISPATCH_RUNTIME.md",
    "docs/art/13_PREMIUM_2D_CITYMAP_AND_MARKERS.md",
    "data/content/city-map.v1.json",
    "data/content/unit-roster.live.v1.json",
    "data/content/incident-actions.v1.json",
    "data/content/report-timeline-demo.v1.json",
    "src/Alarm112.Contracts/CityMapDto.cs",
    "src/Alarm112.Contracts/DispatchCommandDto.cs",
    "src/Alarm112.Application/Interfaces/ICityMapService.cs",
    "src/Alarm112.Application/Services/CityMapService.cs",
    "client-unity/Assets/Scripts/Runtime/Map/CityMapController.cs",
    "client-unity/Assets/Scripts/Runtime/Map/LiveUnitListController.cs",
    "client-unity/Assets/Scripts/Runtime/Map/IncidentCardController.cs",
    "client-unity/Assets/Scripts/Runtime/Map/RoundTimelineController.cs"
)

foreach ($item in $required) {
    $path = Join-Path $root $item
    if (-not (Test-Path $path)) {
        throw "Missing required file: $path"
    }
}

Write-Host "V6 smoke passed."
