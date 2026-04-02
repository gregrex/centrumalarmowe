namespace Alarm112.Contracts;

public sealed record ReleaseFeedbackLoopV2Dto(
    IReadOnlyList<PlaytestLoopStageDto> Stages,
    IReadOnlyList<string> FeedbackFields,
    string CurrentStage,
    string Goal);
