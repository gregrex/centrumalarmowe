using UnityEngine;

namespace Alarm112.Client.Runtime.Coop;

public sealed class SharedActionPanelController : MonoBehaviour
{
    [SerializeField] private TextAsset? sharedActionsJsonAsset;
    [SerializeField] private string fallbackPath = "data/content/shared-actions.v1.json";
    [SerializeField] private int pendingActions;

    public int PendingActions => pendingActions;

    public void LoadSharedActions()
    {
        var json = Alarm112.Client.Runtime.Config.JsonConfigLoader.LoadTextAssetOrFile(sharedActionsJsonAsset, fallbackPath);
        pendingActions = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(1, json.Split("sharedActionId").Length - 1);
        Debug.Log($"[SharedActionPanel] pending={pendingActions}");
    }
}
