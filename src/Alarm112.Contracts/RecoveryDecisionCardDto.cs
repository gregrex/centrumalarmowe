namespace Alarm112.Contracts;

public sealed record RecoveryDecisionCardDto(
    string Id,
    string Trigger,
    string Title,
    string Severity,
    IReadOnlyList<RecoveryDecisionCardOptionDto> Options);
