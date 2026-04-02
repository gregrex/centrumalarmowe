namespace Alarm112.Contracts;

public sealed record RecoveryHudTriggerDto(
    string Id,
    string TriggerKey,
    string Severity,
    string Role,
    bool RequiresImmediateAttention);