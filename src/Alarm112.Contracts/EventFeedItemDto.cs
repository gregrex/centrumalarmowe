namespace Alarm112.Contracts;

public sealed record EventFeedItemDto(
    string Id,
    string Priority,
    string LabelKey);
