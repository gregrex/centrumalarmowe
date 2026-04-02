namespace Alarm112.Client.Runtime.Session;

public sealed class ObjectiveStateMachineController
{
    public string LastTransition { get; private set; } = "none";

    public void ApplyTransition(string objectiveId, string fromState, string toState)
    {
        LastTransition = $"{objectiveId}:{fromState}->{toState}";
    }
}
