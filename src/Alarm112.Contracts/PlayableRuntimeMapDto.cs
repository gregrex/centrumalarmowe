namespace Alarm112.Contracts;

public sealed record PlayableRuntimeMapDto(
    string MissionId,
    string CurrentZoneId,
    int CityPressure,
    IReadOnlyList<ActiveIncidentDto> Incidents,
    IReadOnlyList<UnitRuntimeDto> Units,
    IReadOnlyList<RouteOverlaySegmentDto> Routes);
