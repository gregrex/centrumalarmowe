namespace Alarm112.Contracts;

public sealed record InternalTestPackDto(
    string BuildId,
    string MissionId,
    string Status,
    IReadOnlyList<string> Artifacts,
    IReadOnlyList<string> TesterGroups,
    IReadOnlyList<InternalTestBuildStepDto> Steps);
