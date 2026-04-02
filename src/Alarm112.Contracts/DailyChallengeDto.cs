namespace Alarm112.Contracts;

public sealed record DailyChallengeDto(
    string Id,
    string Role,
    string Difficulty,
    string Reward);
