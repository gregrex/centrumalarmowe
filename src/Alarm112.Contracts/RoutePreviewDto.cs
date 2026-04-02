namespace Alarm112.Contracts;

public sealed record RoutePreviewDto(
    string RouteId,
    string IncidentId,
    string UnitId,
    int EtaSeconds,
    double DistanceKm,
    IReadOnlyCollection<string> NodeIds,
    IReadOnlyCollection<string> WarningCodes,
    string LineStyle);
