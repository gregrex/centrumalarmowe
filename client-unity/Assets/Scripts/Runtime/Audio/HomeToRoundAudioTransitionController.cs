using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.Audio;

public sealed class HomeToRoundAudioTransitionController : MonoBehaviour
{
    [SerializeField] private string currentRoute = "menu.home";
    [SerializeField] private string currentMusicState = "menu.home.calm";

    public void Transition(string fromScreen, string toScreen, string musicState, string stingerId)
    {
        currentRoute = $"{fromScreen}->{toScreen}";
        currentMusicState = musicState;
        Debug.Log($"[AudioTransition] {currentRoute} music={musicState} stinger={stingerId}");
    }
}
