namespace Alarm112.Contracts;

public sealed record FinalHandoffPackDto(
    string Version,
    string BuildTarget,
    string ShowcaseMissionId,
    IReadOnlyList<string> Artifacts,
    string Status,
    string RecommendedNextAction);
