namespace Alarm112.Contracts;

public sealed record NearFinalSliceFlowDto(
    string SliceId,
    IReadOnlyList<string> Steps,
    string RecommendedEntryRole);
