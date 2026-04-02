namespace Alarm112.Contracts;

public sealed record DispatcherLoopDto(
    string MissionId,
    IReadOnlyList<string> AllowedActions,
    IReadOnlyList<RuntimeDispatchOutcomeDto> Outcomes,
    string RecommendedAction);
