namespace Alarm112.Client.Runtime.Menu;

public sealed class NextMissionHandoffController
{
    public string? NextMissionId { get; private set; }

    public void SetNextMission(string missionId) => NextMissionId = missionId;
}
