namespace Alarm112.Contracts;

public sealed record GooglePlayChecklistItemDto(
    string Id,
    string Label,
    string Status,
    string Owner);
