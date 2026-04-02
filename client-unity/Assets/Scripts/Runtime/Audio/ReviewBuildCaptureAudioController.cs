namespace Alarm112.ClientUnity.Runtime.Audio;

public sealed class ReviewBuildCaptureAudioController
{
    public string MixState { get; private set; } = "home_clean";

    public void SetMixState(string mixState)
    {
        if (!string.IsNullOrWhiteSpace(mixState))
        {
            MixState = mixState;
        }
    }
}
