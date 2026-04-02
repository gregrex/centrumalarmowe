namespace Alarm112.Contracts;

public sealed record RoleSelectionSlotDto(
    string RoleId,
    string State,
    string? OccupantId,
    string Difficulty,
    bool IsRecommended,
    bool IsLocked);
