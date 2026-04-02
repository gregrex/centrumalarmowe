namespace Alarm112.Contracts;

public sealed record BugBashChecklistItemDto(
    string Id,
    string Label,
    string Severity,
    bool IsBlocking,
    bool IsPassed);
