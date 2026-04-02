using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.Session;

public sealed class RoundBootstrapController : MonoBehaviour
{
    [SerializeField] private string missionId = "mission.01.03";
    [SerializeField] private string mode = "offline_solo_with_bots";
    [SerializeField] private string roleId = "dispatcher";
    [SerializeField] private string scenePreset = "dispatch_hall_storm";

    public string MissionId => missionId;
    public string Mode => mode;
    public string RoleId => roleId;
    public string ScenePreset => scenePreset;

    public void Configure(string newMissionId, string newMode, string newRoleId)
    {
        missionId = newMissionId;
        mode = newMode;
        roleId = newRoleId;
    }
}
