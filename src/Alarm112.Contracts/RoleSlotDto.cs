namespace Alarm112.Contracts;

public sealed record RoleSlotDto(string Role, bool HasHuman, bool HasBot, string? OccupantId);
