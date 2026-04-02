namespace Alarm112.Contracts;

public sealed record ReportProgressionDto(
    string MissionId,
    string Grade,
    int Stars,
    int RankProgress,
    IReadOnlyList<string> Rewards,
    IReadOnlyList<string> NextActions);
