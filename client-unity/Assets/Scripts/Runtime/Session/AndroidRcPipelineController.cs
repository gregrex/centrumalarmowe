namespace Alarm112.ClientUnity.Runtime.Session;

public sealed class AndroidRcPipelineController
{
    public string PipelineState { get; private set; } = "idle";

    public void StartMockPipeline() => PipelineState = "building";
    public void CompleteMockPipeline() => PipelineState = "done";
}
