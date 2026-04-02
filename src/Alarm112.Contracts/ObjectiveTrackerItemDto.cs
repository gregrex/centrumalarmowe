namespace Alarm112.Contracts;

public sealed record ObjectiveTrackerItemDto(
    string Id,
    string LabelKey,
    string Status,
    int Progress,
    string Priority);
