namespace Alarm112.Contracts;

public sealed record ActiveIncidentDto(
    string IncidentId,
    string IncidentTypeId,
    string TitleKey,
    string Severity,
    string NodeId,
    string Status,
    string PrimaryRequiredRole,
    string RecommendedUnitId,
    int RecommendedEtaSeconds,
    int Pressure,
    bool SharedActionRequired,
    IReadOnlyCollection<string> Tags);
