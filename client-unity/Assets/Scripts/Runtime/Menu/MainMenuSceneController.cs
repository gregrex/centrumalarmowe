using UnityEngine;

namespace Alarm112.Client.Menu
{
    public sealed class MainMenuSceneController : MonoBehaviour
    {
        [SerializeField] private string defaultThemeId = "scene.menu.city_night.default";
        [SerializeField] private string defaultHeroObjectId = "hero.vehicle.ambulance.alpha";

        public string CurrentThemeId { get; private set; } = string.Empty;
        public string CurrentHeroObjectId { get; private set; } = string.Empty;

        private void Awake()
        {
            CurrentThemeId = defaultThemeId;
            CurrentHeroObjectId = defaultHeroObjectId;
            Debug.Log($"[MainMenuSceneController] theme={CurrentThemeId}, hero={CurrentHeroObjectId}");
        }

        public void ApplyTheme(string themeId, string heroObjectId)
        {
            CurrentThemeId = themeId;
            CurrentHeroObjectId = heroObjectId;
            Debug.Log($"[MainMenuSceneController] ApplyTheme => {themeId} / {heroObjectId}");
        }
    }
}
