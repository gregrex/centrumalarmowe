namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class AndroidPreviewBuildController
{
    public string BuildId { get; private set; } = "Alarm112-Preview-001";
    public string Status { get; private set; } = "draft";

    public void ApplyPreview(string buildId, string status)
    {
        BuildId = buildId;
        Status = status;
    }
}
