namespace Alarm112.Contracts;

public sealed record MissionBriefingDto(
    string MissionId,
    string TitleKey,
    string Difficulty,
    int EstimatedMinutes,
    string WeatherPreset,
    string TimeOfDay,
    IReadOnlyList<string> PrimaryObjectives,
    IReadOnlyList<string> SecondaryObjectives,
    IReadOnlyList<string> RiskTags,
    IReadOnlyList<string> RecommendedRoles,
    IReadOnlyList<string> SuggestedUnits,
    string SpeakerPortraitId,
    string SpeakerLineKey,
    IReadOnlyList<string> Hotspots);
