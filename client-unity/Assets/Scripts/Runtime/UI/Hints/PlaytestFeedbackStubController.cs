namespace Alarm112.ClientUnity.Runtime.UI.Hints;

public sealed class PlaytestFeedbackStubController
{
    public bool ScreenshotAttached { get; private set; }

    public void AttachScreenshot() => ScreenshotAttached = true;
    public void ClearScreenshot() => ScreenshotAttached = false;
}
