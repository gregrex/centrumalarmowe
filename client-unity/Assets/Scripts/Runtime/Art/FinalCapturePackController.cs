namespace Alarm112.ClientUnity.Runtime.Art;

public sealed class FinalCapturePackController
{
    public string ActiveSafeFrame { get; private set; } = "mobile_portrait_safe";

    public void SetSafeFrame(string safeFrame)
    {
        ActiveSafeFrame = safeFrame;
    }
}
