namespace Alarm112.Contracts;

public sealed record RuntimeScoreboardDto(
    string MissionId,
    string ResultState,
    int TotalScore,
    int Stars,
    IReadOnlyList<RuntimeScoreboardRowDto> Rows);
