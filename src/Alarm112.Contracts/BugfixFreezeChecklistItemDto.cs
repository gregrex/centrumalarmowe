namespace Alarm112.Contracts;

public sealed record BugfixFreezeChecklistItemDto(
    string Id,
    string Title,
    string Status,
    string Severity,
    string Owner);
