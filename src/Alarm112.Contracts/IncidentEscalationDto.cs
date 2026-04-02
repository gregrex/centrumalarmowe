namespace Alarm112.Contracts;

public sealed record IncidentEscalationDto(
    string IncidentId,
    string EscalationType,
    int PressureDelta,
    string ResultState);
