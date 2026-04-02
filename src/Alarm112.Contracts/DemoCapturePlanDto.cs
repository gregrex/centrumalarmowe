namespace Alarm112.Contracts;

public sealed record DemoCapturePlanDto(
    string PlanId,
    IReadOnlyList<string> Shots,
    IReadOnlyList<string> Notes,
    bool CaptureModeEnabled);
