namespace Alarm112.Contracts;

public sealed record RuntimeHudDto(
    string MissionId,
    IReadOnlyList<RuntimeHudPanelDto> Panels,
    IReadOnlyList<string> ActiveBanners,
    string ActiveRecoverySlotId);