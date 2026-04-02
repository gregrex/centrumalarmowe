namespace Alarm112.Client.Runtime.Session;

public sealed class LiveCityStatusController
{
    public string PressureBand { get; private set; } = "critical";
    public string TrafficState { get; private set; } = "traffic_heavy";

    public void Update(string pressureBand, string trafficState)
    {
        PressureBand = pressureBand;
        TrafficState = trafficState;
    }
}
