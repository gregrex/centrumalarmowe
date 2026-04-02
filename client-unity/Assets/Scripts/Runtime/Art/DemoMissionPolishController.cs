namespace Alarm112.Client.Runtime.Art;

public sealed class DemoMissionPolishController
{
    public string Theme { get; private set; } = "rainy_evening";
    public string ScenePackId { get; private set; } = "scene_pack.demo.01";

    public void Bind(string theme, string scenePackId)
    {
        Theme = theme;
        ScenePackId = scenePackId;
    }
}
