namespace Alarm112.Contracts;

public sealed record TeamReadinessSlotDto(
    string RoleId,
    string State,
    string? OccupantId,
    bool Ready,
    string NetworkQuality);
