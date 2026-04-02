namespace Alarm112.Contracts;

public sealed record ObjectiveTrackerDto(
    string MissionId,
    IReadOnlyList<ObjectiveTrackerItemDto> Objectives,
    IReadOnlyList<EventFeedItemDto> EventFeed);
