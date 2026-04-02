namespace Alarm112.Contracts;

public sealed record BugfixFreezeDto(
    string FreezeLabel,
    IReadOnlyList<BugfixFreezeChecklistItemDto> Checklist,
    IReadOnlyList<string> AllowedChanges,
    IReadOnlyList<string> BlockedChanges,
    string RecommendedNextAction);
