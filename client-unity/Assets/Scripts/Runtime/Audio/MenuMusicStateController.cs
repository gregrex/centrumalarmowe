using UnityEngine;

namespace Alarm112.Client.Audio
{
    public sealed class MenuMusicStateController : MonoBehaviour
    {
        [SerializeField] private string currentState = "audio.menu.calm.v1";

        public void SetState(string stateId)
        {
            currentState = stateId;
            Debug.Log($"[MenuMusicStateController] state => {currentState}");
        }
    }
}
