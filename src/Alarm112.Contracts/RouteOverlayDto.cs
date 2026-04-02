namespace Alarm112.Contracts;

public sealed record RouteOverlayDto(
    string SessionId,
    string IncidentId,
    string UnitId,
    string HeatmapPreset,
    IReadOnlyCollection<RouteOverlaySegmentDto> Segments,
    IReadOnlyCollection<string> WarningCodes);
