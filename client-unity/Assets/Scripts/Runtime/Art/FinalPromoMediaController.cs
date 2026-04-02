namespace Alarm112.ClientUnity.Runtime.Art;

public sealed class FinalPromoMediaController
{
    public bool CaptureModeEnabled { get; private set; }

    public void SetCaptureMode(bool enabled) => CaptureModeEnabled = enabled;
}
