namespace Alarm112.Contracts;

public sealed record MissionScriptDto(
    string MissionId,
    IReadOnlyList<MissionScriptStepDto> Timeline);
