namespace Alarm112.Contracts;

public sealed record RoundBootstrapDto(
    string MissionId,
    string Mode,
    string SelectedRole,
    string ScenePreset,
    string WeatherPreset,
    string MusicState,
    IReadOnlyList<string> StartingUnits,
    IReadOnlyList<string> RiskTags,
    IReadOnlyList<RoleSelectionSlotDto> TeamSlots,
    IReadOnlyList<string> Objectives);
