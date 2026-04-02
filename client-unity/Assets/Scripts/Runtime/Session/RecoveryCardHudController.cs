namespace Alarm112.Client.Runtime.Session;

public sealed class RecoveryCardHudController
{
    public string ActiveTriggerId { get; private set; } = "trigger.route_blocked";
    public bool IsCritical { get; private set; }

    public void Show(string triggerId, bool isCritical)
    {
        ActiveTriggerId = triggerId;
        IsCritical = isCritical;
    }
}