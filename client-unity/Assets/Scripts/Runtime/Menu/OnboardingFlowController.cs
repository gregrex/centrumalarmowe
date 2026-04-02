namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class OnboardingFlowController
{
    public string CurrentStepId { get; private set; } = "intro";
    public bool IsCompleted { get; private set; }

    public void Advance(string nextStepId)
    {
        CurrentStepId = nextStepId;
        IsCompleted = nextStepId == "report";
    }
}
