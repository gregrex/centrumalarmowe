namespace Alarm112.Contracts;

public sealed record ChainEscalationStateDto(
    string ChainId,
    string Trigger,
    string Severity,
    IReadOnlyList<string> ActiveSteps,
    IReadOnlyList<string> RecoveryOptions);
