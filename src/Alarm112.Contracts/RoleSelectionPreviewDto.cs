namespace Alarm112.Contracts;

public sealed record RoleSelectionPreviewDto(
    string MissionId,
    string RecommendedRole,
    IReadOnlyList<RoleSelectionSlotDto> Slots,
    BotFillPreviewDto BotPreview);
