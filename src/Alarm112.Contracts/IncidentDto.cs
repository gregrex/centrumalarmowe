namespace Alarm112.Contracts;

public sealed record IncidentDto(
    string IncidentId,
    string Title,
    string Category,
    string Severity,
    string Zone,
    string Status);
