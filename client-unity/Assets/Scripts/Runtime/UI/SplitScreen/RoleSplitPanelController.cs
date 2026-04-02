using UnityEngine;

namespace Alarm112.Client.Runtime.UI.SplitScreen;

public sealed class RoleSplitPanelController : MonoBehaviour
{
    [SerializeField] private TextAsset? splitPanelsJsonAsset;
    [SerializeField] private string fallbackPath = "data/ui/splitscreen_panels.v1.json";
    [SerializeField] private int layoutCount;

    public int LayoutCount => layoutCount;

    public void LoadLayouts()
    {
        var json = Alarm112.Client.Runtime.Config.JsonConfigLoader.LoadTextAssetOrFile(splitPanelsJsonAsset, fallbackPath);
        layoutCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(0, json.Split("layoutId").Length - 1);
        Debug.Log($"[RoleSplitPanelController] layouts={layoutCount}");
    }
}
