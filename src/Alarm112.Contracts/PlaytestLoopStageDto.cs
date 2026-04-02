namespace Alarm112.Contracts;

public sealed record PlaytestLoopStageDto(
    string Id,
    string Name,
    string Owner,
    string Status,
    string Output);
