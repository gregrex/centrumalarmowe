using UnityEngine;

namespace Alarm112.Client.Runtime.Map;

public sealed class RouteOverlayMapController : MonoBehaviour
{
    [SerializeField] private TextAsset? routeOverlayJsonAsset;
    [SerializeField] private string fallbackPath = "data/content/route-overlay.v1.json";
    [SerializeField] private int segmentCount;
    [SerializeField] private int criticalSegmentCount;

    public int SegmentCount => segmentCount;
    public int CriticalSegmentCount => criticalSegmentCount;

    public void LoadOverlay()
    {
        var json = Alarm112.Client.Runtime.Config.JsonConfigLoader.LoadTextAssetOrFile(routeOverlayJsonAsset, fallbackPath);
        segmentCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(0, json.Split("segmentId").Length - 1);
        criticalSegmentCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(0, json.Split('"isCritical": true').Length - 1);
        Debug.Log($"[RouteOverlayMapController] segments={segmentCount}, critical={criticalSegmentCount}");
    }
}
