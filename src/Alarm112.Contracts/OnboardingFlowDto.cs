namespace Alarm112.Contracts;

public sealed record OnboardingFlowDto(
    string FlowId,
    IReadOnlyList<string> StepOrder,
    IReadOnlyList<OnboardingHintDto> Hints);
