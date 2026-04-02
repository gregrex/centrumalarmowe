using System;

namespace Alarm112.Contracts;

public sealed record SessionTimelineItemDto(
    string TimelineItemId,
    DateTimeOffset Timestamp,
    string Severity,
    string ActorRole,
    string Message,
    bool IsBot);
