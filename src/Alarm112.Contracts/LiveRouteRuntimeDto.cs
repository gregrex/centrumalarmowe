namespace Alarm112.Contracts;

public sealed record LiveRouteRuntimeDto(
    string MissionId,
    IReadOnlyList<LiveRouteSegmentStateDto> Routes,
    string ActiveObjectiveId,
    string TimerPhase);
