namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class ReleaseCandidatePackageController
{
    public string BuildId { get; private set; } = "Alarm112-RC-001";
    public bool IsRcReady { get; private set; }

    public void MarkReady(string buildId)
    {
        BuildId = buildId;
        IsRcReady = true;
    }
}
