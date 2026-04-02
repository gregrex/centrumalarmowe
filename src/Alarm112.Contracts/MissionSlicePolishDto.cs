namespace Alarm112.Contracts;

public sealed record MissionSlicePolishDto(
    string MissionId,
    IReadOnlyList<string> SceneKeys,
    IReadOnlyList<string> ObjectKeys,
    IReadOnlyList<string> AudioKeys);