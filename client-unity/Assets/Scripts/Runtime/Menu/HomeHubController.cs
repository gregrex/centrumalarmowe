using UnityEngine;

namespace Alarm112.Client.Menu
{
    public sealed class HomeHubController : MonoBehaviour
    {
        [SerializeField] private string currentScreenId = "menu.home";
        [SerializeField] private string continueSummary = "2 aktywne incydenty, 3 wolne jednostki";

        public void ApplyHome(string screenId, string summary)
        {
            currentScreenId = screenId;
            continueSummary = summary;
            Debug.Log($"[HomeHubController] screen={currentScreenId}, summary={continueSummary}");
        }

        public void OpenRoute(string routeId)
        {
            Debug.Log($"[HomeHubController] OpenRoute => {routeId}");
        }
    }
}
