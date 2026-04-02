namespace Alarm112.Client.Runtime.UI.Reports;

public sealed class FailRetryNextFlowController
{
    public string ResultState { get; private set; } = "partial";
    public int StepCount { get; private set; }

    public void Bind(string resultState, int stepCount)
    {
        ResultState = resultState;
        StepCount = stepCount;
    }
}