namespace Alarm112.Contracts;

public sealed record PostRoundReportDto(
    string MissionId,
    string GradeId,
    int Score,
    int Stars,
    IReadOnlyList<MissionBriefingObjectiveDto> Objectives,
    IReadOnlyList<PostRoundMetricDto> Metrics,
    IReadOnlyList<string> Mistakes,
    IReadOnlyList<string> Rewards,
    IReadOnlyList<string> NextActions);
