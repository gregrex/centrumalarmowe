namespace Alarm112.Contracts;

public sealed record ShowcaseMissionStepDto(
    string StepId,
    string Title,
    string Description,
    int Order,
    bool IsMandatory);
