namespace Alarm112.Contracts;

public sealed record VerticalSliceScenePackDto(
    string SceneId,
    string LayerPack,
    IReadOnlyList<string> HeroObjects,
    string AudioState);
