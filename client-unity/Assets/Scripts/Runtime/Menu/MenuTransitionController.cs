using UnityEngine;

namespace Alarm112.Client.Menu
{
    public sealed class MenuTransitionController : MonoBehaviour
    {
        [SerializeField] private float transitionDuration = 0.2f;

        public void GoToScreen(string screenId)
        {
            Debug.Log($"[MenuTransitionController] Transition to {screenId} in {transitionDuration:0.00}s");
        }
    }
}
