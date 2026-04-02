using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.Session;

public sealed class MissionCompleteFlowController : MonoBehaviour
{
    [SerializeField] private string scenePreset = "report_room_success";
    [SerializeField] private string audioState = "report_success_mild";

    public string ScenePreset => scenePreset;
    public string AudioState => audioState;

    public void Configure(string newScenePreset, string newAudioState)
    {
        scenePreset = newScenePreset;
        audioState = newAudioState;
    }
}
