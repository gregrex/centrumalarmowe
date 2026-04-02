using UnityEngine;

namespace Alarm112.Client.Audio
{
    public sealed class ScreenAudioRouter : MonoBehaviour
    {
        [SerializeField] private string currentScreen = "menu.home";
        [SerializeField] private string currentMusicState = "menu.home.calm";

        public void Route(string screenId, string musicState)
        {
            currentScreen = screenId;
            currentMusicState = musicState;
            Debug.Log($"[ScreenAudioRouter] {currentScreen} => {currentMusicState}");
        }
    }
}
