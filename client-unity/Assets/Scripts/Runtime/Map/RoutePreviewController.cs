using UnityEngine;

namespace Alarm112.Client.Runtime.Map;

public sealed class RoutePreviewController : MonoBehaviour
{
    [SerializeField] private TextAsset? routeJsonAsset;
    [SerializeField] private string fallbackPath = "data/content/route-preview-demo.v1.json";
    [SerializeField] private int warningCount;
    [SerializeField] private float etaSeconds;

    public int WarningCount => warningCount;
    public float EtaSeconds => etaSeconds;

    public void LoadPreview()
    {
        var json = Alarm112.Client.Runtime.Config.JsonConfigLoader.LoadTextAssetOrFile(routeJsonAsset, fallbackPath);
        warningCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(0, json.Split("route.warning").Length - 1);
        etaSeconds = json.Contains('"etaSeconds": 240') ? 240f : 180f;
        Debug.Log($"[RoutePreview] eta={etaSeconds}, warnings={warningCount}");
    }
}
