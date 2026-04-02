namespace Alarm112.Contracts;

public sealed record ReleaseReadinessChecklistItemDto(
    string Id,
    string Label,
    string State,
    string Severity);
