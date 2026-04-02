namespace Alarm112.Client.Runtime.UI;

public sealed class RuntimeHudV2Controller
{
    public string MissionId { get; private set; } = "mission.demo.18";
    public int VisiblePanelCount { get; private set; }

    public void Bind(string missionId, int visiblePanelCount)
    {
        MissionId = missionId;
        VisiblePanelCount = visiblePanelCount;
    }
}