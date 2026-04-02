using UnityEngine;

namespace Alarm112.Client.Runtime.Map;

public sealed class LiveUnitListController : MonoBehaviour
{
    [SerializeField] private TextAsset? liveUnitsJsonAsset;
    [SerializeField] private string fallbackPath = "data/content/unit-roster.live.v1.json";
    [SerializeField] private int unitCount;

    public int UnitCount => unitCount;

    public void LoadUnits()
    {
        var json = Alarm112.Client.Runtime.Config.JsonConfigLoader.LoadTextAssetOrFile(liveUnitsJsonAsset, fallbackPath);
        unitCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(1, json.Split("unitId").Length - 1);
        Debug.Log($"[LiveUnits] loaded, approx unit count={unitCount}");
    }
}
