namespace Alarm112.Contracts;

public sealed record NextMissionHandoffDto(
    string CurrentMissionId,
    string NextMissionId,
    string Title,
    string Hook,
    IReadOnlyList<string> RecommendedUnits);
