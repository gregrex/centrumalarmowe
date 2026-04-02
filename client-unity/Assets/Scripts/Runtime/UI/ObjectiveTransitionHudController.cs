namespace Alarm112.Client.Runtime.UI;

public sealed class ObjectiveTransitionHudController
{
    public string ObjectiveId { get; private set; } = "obj.primary.bridge_patient";
    public string State { get; private set; } = "active";

    public void Bind(string objectiveId, string state)
    {
        ObjectiveId = objectiveId;
        State = state;
    }
}
