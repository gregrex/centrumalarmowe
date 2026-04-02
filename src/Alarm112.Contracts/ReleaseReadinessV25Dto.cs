namespace Alarm112.Contracts;

public sealed record ReleaseReadinessV25Dto(
    IReadOnlyList<ReleaseReadinessChecklistItemDto> Checklist,
    string Status,
    string RecommendedNextAction);
