namespace Alarm112.ClientUnity.Runtime.UI.Hints;

public sealed class TelemetryDashboardStubController
{
    public string MissionId { get; private set; } = "showcase.mission.01";

    public void BindMission(string missionId)
    {
        MissionId = missionId;
    }
}
