namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class ReviewBuildChecklistController
{
    public bool CaptureModeEnabled { get; private set; }
    public string BuildName { get; private set; } = "Alarm112 Showcase Review Build";

    public void EnableCaptureMode() => CaptureModeEnabled = true;
    public void DisableCaptureMode() => CaptureModeEnabled = false;
}
