using UnityEngine;

namespace Alarm112.Client.Session;

public sealed class MissionRuntimeHudController : MonoBehaviour
{
    [SerializeField] private string missionId = "mission.01.04";

    public string MissionId => missionId;

    public void BootstrapRuntime()
    {
        Debug.Log($"[MissionRuntimeHudController] Bootstrap runtime for {missionId}");
    }
}
