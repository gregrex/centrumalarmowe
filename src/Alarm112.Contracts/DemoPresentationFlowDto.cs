namespace Alarm112.Contracts;

public sealed record DemoPresentationFlowDto(
    string FlowId,
    IReadOnlyList<string> Shots,
    string SuggestedAspectRatio);
