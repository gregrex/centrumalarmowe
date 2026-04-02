namespace Alarm112.Client.Runtime.Session;

public sealed class PlayableRuntimeMapController
{
    public string CurrentMissionId { get; private set; } = "mission.01.05";
    public int CityPressure { get; private set; }

    public void Bind(string missionId, int cityPressure)
    {
        CurrentMissionId = missionId;
        CityPressure = cityPressure;
    }
}
