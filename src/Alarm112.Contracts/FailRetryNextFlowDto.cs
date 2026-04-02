namespace Alarm112.Contracts;

public sealed record FailRetryNextFlowDto(
    string MissionId,
    string ResultState,
    IReadOnlyList<string> Steps,
    IReadOnlyList<string> RetryHints);