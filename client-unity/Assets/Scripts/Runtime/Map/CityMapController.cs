using UnityEngine;

namespace Alarm112.Client.Runtime.Map;

public sealed class CityMapController : MonoBehaviour
{
    [SerializeField] private TextAsset? mapJsonAsset;
    [SerializeField] private string fallbackPath = "data/content/city-map.v1.json";
    [SerializeField] private int nodeCount;

    public int NodeCount => nodeCount;

    public void LoadMap()
    {
        var json = Alarm112.Client.Runtime.Config.JsonConfigLoader.LoadTextAssetOrFile(mapJsonAsset, fallbackPath);
        nodeCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(1, json.Split("nodeId").Length - 1);
        Debug.Log($"[CityMap] loaded city map, approx node count={nodeCount}");
    }
}
