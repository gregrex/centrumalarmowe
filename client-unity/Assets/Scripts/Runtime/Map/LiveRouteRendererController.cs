namespace Alarm112.Client.Runtime.Map;

public sealed class LiveRouteRendererController
{
    public string MissionId { get; private set; } = "mission.demo.16";
    public int RouteCount { get; private set; }

    public void Bind(string missionId, int routeCount)
    {
        MissionId = missionId;
        RouteCount = routeCount;
    }
}
