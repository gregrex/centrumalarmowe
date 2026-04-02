namespace Alarm112.Contracts;

public sealed record PlaytestReleaseLiveopsLoopDto(
    IReadOnlyList<PlaytestLoopStageDto> Stages,
    string CurrentStage,
    string NextAction);
