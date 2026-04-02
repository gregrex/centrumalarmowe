using UnityEngine;

namespace Alarm112.Client.Runtime.Map;

public sealed class ActiveIncidentBoardController : MonoBehaviour
{
    [SerializeField] private TextAsset? boardJsonAsset;
    [SerializeField] private string fallbackPath = "data/content/active-incidents.v1.json";
    [SerializeField] private int activeCount;
    [SerializeField] private int criticalCount;

    public int ActiveCount => activeCount;
    public int CriticalCount => criticalCount;

    public void LoadBoard()
    {
        var json = Alarm112.Client.Runtime.Config.JsonConfigLoader.LoadTextAssetOrFile(boardJsonAsset, fallbackPath);
        activeCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(1, json.Split("incidentId").Length - 1);
        criticalCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(0, json.Split(""critical"").Length - 1);
        Debug.Log($"[ActiveIncidentBoard] active={activeCount}, critical={criticalCount}");
    }
}
