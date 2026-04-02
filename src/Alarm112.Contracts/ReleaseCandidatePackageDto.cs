namespace Alarm112.Contracts;

public sealed record ReleaseCandidatePackageDto(
    string BuildId,
    string Version,
    string MissionId,
    IReadOnlyList<string> Channels,
    IReadOnlyList<string> ReleaseGates,
    IReadOnlyList<string> Artifacts);
