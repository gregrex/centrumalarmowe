using UnityEngine;

namespace Alarm112.Client.Runtime.Map;

public sealed class LiveIncidentDeltaFeedController : MonoBehaviour
{
    [SerializeField] private TextAsset? deltasJsonAsset;
    [SerializeField] private string fallbackPath = "data/content/live-incident-deltas.v1.json";
    [SerializeField] private int deltaCount;
    [SerializeField] private int needsAttentionCount;

    public int DeltaCount => deltaCount;
    public int NeedsAttentionCount => needsAttentionCount;

    public void LoadDeltas()
    {
        var json = Alarm112.Client.Runtime.Config.JsonConfigLoader.LoadTextAssetOrFile(deltasJsonAsset, fallbackPath);
        deltaCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(0, json.Split("deltaId").Length - 1);
        needsAttentionCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(0, json.Split('"needsAttention": true').Length - 1);
        Debug.Log($"[LiveIncidentDeltaFeedController] deltas={deltaCount}, urgent={needsAttentionCount}");
    }
}
