namespace Alarm112.Contracts;

public sealed record MissionCompleteFlowDto(
    IReadOnlyList<string> Steps,
    string AudioState,
    string ScenePreset);
