namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class GooglePlayInternalTestingController
{
    public string Track { get; private set; } = "internal";

    public void Apply(string track)
    {
        Track = track;
    }
}
