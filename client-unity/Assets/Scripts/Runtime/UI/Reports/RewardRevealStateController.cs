namespace Alarm112.Client.Runtime.UI.Reports;

public sealed class RewardRevealStateController
{
    public string CurrentRevealState { get; private set; } = "idle";

    public void SetState(string state) => CurrentRevealState = state;
}
