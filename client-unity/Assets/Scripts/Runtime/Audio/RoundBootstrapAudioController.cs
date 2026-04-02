using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.Audio;

public sealed class RoundBootstrapAudioController : MonoBehaviour
{
    [SerializeField] private string musicState = "bootstrap_rise";
    [SerializeField] private string lastCue = string.Empty;

    public string MusicState => musicState;
    public string LastCue => lastCue;

    public void SetMusicState(string stateId)
    {
        musicState = stateId;
    }

    public void PlayCue(string cueId)
    {
        lastCue = cueId;
    }
}
