using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class MissionBriefingController : MonoBehaviour
{
    [SerializeField] private string missionId = "mission.01.03";
    [SerializeField] private string weatherPreset = "storm_night";
    [SerializeField] private string speakerPortraitId = "portrait.commander.helena";

    public string MissionId => missionId;
    public string WeatherPreset => weatherPreset;
    public string SpeakerPortraitId => speakerPortraitId;

    public void Configure(string newMissionId, string newWeatherPreset, string newSpeakerPortraitId)
    {
        missionId = newMissionId;
        weatherPreset = newWeatherPreset;
        speakerPortraitId = newSpeakerPortraitId;
    }
}
