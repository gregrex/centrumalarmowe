using UnityEngine;

namespace Alarm112.Client.Runtime.Map;

public sealed class MapFilterBarController : MonoBehaviour
{
    [SerializeField] private TextAsset? filtersJsonAsset;
    [SerializeField] private string fallbackPath = "data/content/map-filters.v1.json";
    [SerializeField] private int filterCount;

    public int FilterCount => filterCount;

    public void LoadFilters()
    {
        var json = Alarm112.Client.Runtime.Config.JsonConfigLoader.LoadTextAssetOrFile(filtersJsonAsset, fallbackPath);
        filterCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(1, json.Split("filterId").Length - 1);
        Debug.Log($"[MapFilterBar] filters={filterCount}");
    }
}
