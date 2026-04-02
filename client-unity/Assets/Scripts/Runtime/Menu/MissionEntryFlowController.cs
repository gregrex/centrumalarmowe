using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class MissionEntryFlowController : MonoBehaviour
{
    [SerializeField] private string missionId = "mission.01.03";
    [SerializeField] private string selectedMode = "offline_solo";
    [SerializeField] private string botFillMode = "balanced";

    public string MissionId => missionId;
    public string SelectedMode => selectedMode;
    public string BotFillMode => botFillMode;

    public void ConfigureMission(string newMissionId)
    {
        missionId = newMissionId;
    }

    public void SelectMode(string modeId)
    {
        selectedMode = modeId;
    }

    public void SelectBotFillMode(string modeId)
    {
        botFillMode = modeId;
    }
}
