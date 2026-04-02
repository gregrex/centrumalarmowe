namespace Alarm112.Contracts;

public sealed record ShowcaseMissionDto(
    string MissionId,
    string Title,
    string RecommendedRole,
    int EstimatedDurationSeconds,
    IReadOnlyList<ShowcaseMissionStepDto> Steps);
