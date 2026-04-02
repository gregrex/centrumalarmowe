namespace Alarm112.Contracts;

public sealed record RewardRevealStateDto(
    string StateId,
    string Title,
    bool IsFinal,
    IReadOnlyList<string> Rewards);
