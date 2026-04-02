namespace Alarm112.Client.Runtime.UI.Reports;

public sealed class RuntimeScoreboardController
{
    public string CurrentState { get; private set; } = "hidden";

    public void ShowScoreboard() => CurrentState = "scoreboard";
}
