namespace Alarm112.Contracts;

public sealed record OnboardingHintDto(
    string HintId,
    string Trigger,
    string Message,
    string Severity);
