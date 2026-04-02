namespace Alarm112.Client.Runtime.Map;

public sealed class VisualRuntimeRouteLayerController
{
    public string MissionId { get; private set; } = "mission.demo.17";
    public string FocusSegmentId { get; private set; } = "seg.fire.01";
    public int SegmentCount { get; private set; }

    public void Bind(string missionId, string focusSegmentId, int segmentCount)
    {
        MissionId = missionId;
        FocusSegmentId = focusSegmentId;
        SegmentCount = segmentCount;
    }
}
