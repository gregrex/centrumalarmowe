namespace Alarm112.Client.Runtime.Art;

public sealed class MissionSlicePolishController
{
    public int SceneCount { get; private set; }
    public int AudioCueCount { get; private set; }

    public void Apply(int sceneCount, int audioCueCount)
    {
        SceneCount = sceneCount;
        AudioCueCount = audioCueCount;
    }
}