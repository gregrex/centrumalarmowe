namespace Alarm112.ClientUnity.Runtime.Session;

public sealed class ShowcaseMissionFlowController
{
    public string FlowState { get; private set; } = "home";

    public void BeginShowcase() => FlowState = "briefing";
    public void EnterRuntime() => FlowState = "runtime";
    public void CompleteMission() => FlowState = "report";
}
