namespace Alarm112.Contracts;

public sealed record DispatchResultDto(
    bool Accepted,
    string ResultCode,
    int EtaSeconds,
    int PressureDelta,
    string WarningCode,
    SessionTimelineItemDto TimelineItem);
