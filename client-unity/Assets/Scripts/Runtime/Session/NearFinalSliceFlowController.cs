namespace Alarm112.Client.Runtime.Session;

public sealed class NearFinalSliceFlowController
{
    public string CurrentStep { get; private set; } = "home";

    public void Advance(string nextStep) => CurrentStep = nextStep;
}
