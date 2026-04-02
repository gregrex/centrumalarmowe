namespace Alarm112.Contracts;

public sealed record OperatorDispatcherShowcaseDto(
    string MissionId,
    IReadOnlyList<string> Roles,
    IReadOnlyList<string> Beats,
    IReadOnlyList<string> SuccessStates,
    IReadOnlyList<string> FailStates,
    string Status);
