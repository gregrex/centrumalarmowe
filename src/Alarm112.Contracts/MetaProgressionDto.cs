namespace Alarm112.Contracts;

public sealed record MetaProgressionDto(
    int AccountLevel,
    int CurrentXp,
    int NextLevelXp,
    IReadOnlyDictionary<string, int> RoleMastery,
    IReadOnlyList<string> UnlockedRewards);
