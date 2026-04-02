namespace Alarm112.Contracts;

public sealed record TeamReadinessDto(
    string MissionId,
    string Mode,
    IReadOnlyList<TeamReadinessSlotDto> Slots,
    string BotFillMode,
    bool CanStart,
    IReadOnlyList<string> Warnings,
    int TeamScore);
