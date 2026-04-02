using UnityEngine;

namespace Alarm112.Client.Menu
{
    public sealed class SettingsBundleController : MonoBehaviour
    {
        [SerializeField] private string activeSectionId = "settings.audio";

        public void OpenSection(string sectionId)
        {
            activeSectionId = sectionId;
            Debug.Log($"[SettingsBundleController] section => {activeSectionId}");
        }
    }
}
