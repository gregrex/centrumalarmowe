
using UnityEngine;

namespace Alarm112.Client.Runtime.Menu;

public sealed class HomeMenuController : MonoBehaviour
{
    [SerializeField] private string defaultSceneId = "scene.quickplay.verticalslice";

    public string DefaultSceneId => defaultSceneId;

    public void StartQuickPlay()
    {
        Debug.Log($"[HomeMenu] Start quick play -> {defaultSceneId}");
    }
}
