using UnityEngine;

namespace Alarm112.Client.Runtime.Map;

public sealed class UnitRuntimeListController : MonoBehaviour
{
    [SerializeField] private TextAsset? unitsRuntimeJsonAsset;
    [SerializeField] private string fallbackPath = "data/content/unit-cooldowns.v1.json";
    [SerializeField] private int unitCount;
    [SerializeField] private int availableCount;

    public int UnitCount => unitCount;
    public int AvailableCount => availableCount;

    public void LoadRuntime()
    {
        var json = Alarm112.Client.Runtime.Config.JsonConfigLoader.LoadTextAssetOrFile(unitsRuntimeJsonAsset, fallbackPath);
        unitCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(0, json.Split("unitId").Length - 1);
        availableCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(0, json.Split('"available": true').Length - 1);
        Debug.Log($"[UnitRuntimeListController] units={unitCount}, available={availableCount}");
    }
}
