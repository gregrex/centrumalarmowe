namespace Alarm112.Contracts;

public sealed record IncidentDeltaDto(
    string DeltaId,
    string IncidentId,
    string ChangeType,
    string Severity,
    string Status,
    int TimerDeltaSeconds,
    bool NeedsAttention,
    DateTimeOffset Timestamp);
