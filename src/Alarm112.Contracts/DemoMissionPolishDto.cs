namespace Alarm112.Contracts;

public sealed record DemoMissionPolishDto(
    string MissionId,
    string Theme,
    string ScenePackId,
    string AudioPackId,
    IReadOnlyList<string> HeroObjects,
    IReadOnlyList<string> ContinuityTags);
