using System.Collections.Generic;

namespace Alarm112.Client.Art
{
    public sealed class ThemePackCatalog
    {
        public string ActiveThemeId { get; set; } = "scene.menu.city_night.default";
        public List<string> Themes { get; } = new()
        {
            "scene.menu.city_day.default",
            "scene.menu.city_night.default",
            "scene.menu.rain_alert.default",
            "scene.menu.blackout.default"
        };
    }
}
