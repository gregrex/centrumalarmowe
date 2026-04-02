namespace Alarm112.ClientUnity.Runtime.Audio;

public sealed class OnboardingShowcaseAudioController
{
    public string CurrentAudioState { get; private set; } = "onboarding_soft";

    public void SetState(string stateId)
    {
        CurrentAudioState = stateId;
    }
}
