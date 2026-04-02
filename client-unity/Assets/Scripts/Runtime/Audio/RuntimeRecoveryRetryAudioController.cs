namespace Alarm112.Client.Runtime.Audio;

public sealed class RuntimeRecoveryRetryAudioController
{
    public string CurrentState { get; private set; } = "runtime_low";

    public void SetState(string state)
    {
        CurrentState = state;
    }
}