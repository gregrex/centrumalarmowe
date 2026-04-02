namespace Alarm112.Contracts;

public sealed record AndroidRcPipelineDto(
    string PipelineId,
    IReadOnlyList<string> Stages,
    IReadOnlyList<string> Artifacts,
    string SigningMode,
    string OutputFolder);
