namespace Alarm112.Contracts;

public sealed record ReviewBuildPackageDto(
    string BuildId,
    string BuildName,
    string MissionId,
    bool CaptureModeEnabled,
    IReadOnlyList<ReviewBuildChecklistItemDto> Checklist,
    IReadOnlyList<string> Channels);
