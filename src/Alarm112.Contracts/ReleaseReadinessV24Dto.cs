namespace Alarm112.Contracts;

public sealed record ReleaseReadinessV24Dto(
    IReadOnlyList<ReleaseReadinessChecklistItemDto> Checklist,
    string Status,
    string RecommendedNextAction);
