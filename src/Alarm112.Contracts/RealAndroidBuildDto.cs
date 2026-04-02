namespace Alarm112.Contracts;

public sealed record RealAndroidBuildDto(
    string BuildId,
    string Version,
    string MissionId,
    string Target,
    IReadOnlyList<string> FocusAreas,
    IReadOnlyList<string> Devices,
    string Status);
