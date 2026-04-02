namespace Alarm112.Client.Runtime.Session;

public sealed class QuasiProductionDemoFlowController
{
    public string MissionId { get; private set; } = "mission.demo.17";
    public int StepCount { get; private set; }

    public void Bind(string missionId, int stepCount)
    {
        MissionId = missionId;
        StepCount = stepCount;
    }
}
