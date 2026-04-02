namespace Alarm112.Contracts;

public sealed record AndroidPreviewBuildDto(
    string BuildId,
    string Version,
    string MissionId,
    string Status,
    IReadOnlyList<string> Artifacts,
    IReadOnlyList<string> DeviceTargets);
