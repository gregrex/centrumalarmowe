namespace Alarm112.Client.Runtime.Session;

public sealed class RoundTimerController
{
    public int SecondsRemaining { get; private set; }
    public string Phase { get; private set; } = "active";

    public void Bind(int secondsRemaining, string phase)
    {
        SecondsRemaining = secondsRemaining;
        Phase = phase;
    }
}
