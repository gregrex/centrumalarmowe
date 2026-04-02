using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.Art;

public sealed class FinalVerticalSliceScenePackController : MonoBehaviour
{
    [SerializeField] private string scenePreset = "dispatch_hall_storm";

    public string ScenePreset => scenePreset;

    public void ApplyPreset(string presetId)
    {
        scenePreset = presetId;
    }
}
