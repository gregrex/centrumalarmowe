namespace Alarm112.Contracts;

public sealed record ObjectiveStateTransitionDto(
    string ObjectiveId,
    string FromState,
    string ToState,
    string Trigger,
    int ProgressDelta);
