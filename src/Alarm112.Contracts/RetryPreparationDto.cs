namespace Alarm112.Contracts;

public sealed record RetryPreparationDto(
    string MissionId,
    string SuggestedRole,
    IReadOnlyList<string> Recommendations,
    IReadOnlyList<string> SuggestedBotFill);
