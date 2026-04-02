namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class DemoPresentationFlowController
{
    public bool CaptureModeEnabled { get; private set; }
    public string AspectRatio { get; private set; } = "9:16";

    public void EnableCaptureMode() => CaptureModeEnabled = true;
    public void DisableCaptureMode() => CaptureModeEnabled = false;
}
