namespace Alarm112.Contracts;

public sealed record ObjectiveGradeDto(
    string GradeId,
    int ScoreMin,
    int Stars,
    IReadOnlyList<string> RewardKeys);
