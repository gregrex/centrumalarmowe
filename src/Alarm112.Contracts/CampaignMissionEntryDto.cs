namespace Alarm112.Contracts;

public sealed record CampaignMissionEntryDto(
    string MissionId,
    string TitleKey,
    string ChapterId,
    int EstimatedMinutes,
    string Difficulty,
    string RecommendedRole,
    string WeatherPreset,
    string TimeOfDay,
    IReadOnlyList<string> StartingUnits,
    IReadOnlyList<string> RiskTags,
    IReadOnlyList<string> Rewards,
    int AvailableSlots,
    string DefaultBotFillMode);
