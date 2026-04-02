namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class InternalTestPackController
{
    public string BuildId { get; private set; } = "Alarm112-Internal-001";
    public string Status { get; private set; } = "draft";

    public void Apply(string buildId, string status)
    {
        BuildId = buildId;
        Status = status;
    }
}
