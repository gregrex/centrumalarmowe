using UnityEngine;

namespace Alarm112.Client.Runtime.Map;

public sealed class RoundTimelineController : MonoBehaviour
{
    [SerializeField] private TextAsset? timelineJsonAsset;
    [SerializeField] private string fallbackPath = "data/content/report-timeline-demo.v1.json";
    [SerializeField] private int timelineCount;

    public int TimelineCount => timelineCount;

    public void LoadTimeline()
    {
        var json = Alarm112.Client.Runtime.Config.JsonConfigLoader.LoadTextAssetOrFile(timelineJsonAsset, fallbackPath);
        timelineCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(1, json.Split("timelineItemId").Length - 1);
        Debug.Log($"[RoundTimeline] loaded approx count={timelineCount}");
    }
}
