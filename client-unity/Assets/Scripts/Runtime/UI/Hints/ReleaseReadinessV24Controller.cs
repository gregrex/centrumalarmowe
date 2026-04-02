namespace Alarm112.ClientUnity.Runtime.UI.Hints;

public sealed class ReleaseReadinessV24Controller
{
    public string Status { get; private set; } = "draft";

    public void Apply(string status)
    {
        Status = status;
    }
}
