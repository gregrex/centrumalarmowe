using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class ThemePackService : IThemePackService
{
    public Task<ThemePackDto> GetThemePackAsync(CancellationToken cancellationToken)
    {
        var payload = new ThemePackDto(
            "scene.menu.city_night.default",
            "hero.vehicle.ambulance.alpha",
            "audio.menu.calm.v1",
            new[]
            {
                "scene.menu.city_day.default",
                "scene.menu.city_night.default",
                "scene.menu.rain_alert.default",
                "scene.menu.blackout.default"
            },
            new[]
            {
                "hero.vehicle.ambulance.alpha",
                "hero.vehicle.firetruck.compact",
                "hero.prop.dispatch_console.wall"
            });
        return Task.FromResult(payload);
    }

    public Task<MenuFlowDto> GetMenuFlowAsync(CancellationToken cancellationToken)
    {
        var payload = new MenuFlowDto(
            "menu.home",
            new[]
            {
                "menu.home",
                "menu.quickplay",
                "menu.campaign",
                "menu.coop",
                "menu.profile",
                "menu.settings"
            },
            new[] { "widget.continue_session", "widget.quick_play" },
            new[] { "widget.campaign_progress", "widget.coop_party", "widget.daily_challenge", "widget.reward_inbox" });
        return Task.FromResult(payload);
    }

    public Task<MetaProgressionDto> GetDemoMetaProgressionAsync(CancellationToken cancellationToken)
    {
        var payload = new MetaProgressionDto(
            4,
            520,
            820,
            new Dictionary<string, int>
            {
                ["operator"] = 140,
                ["dispatcher"] = 110,
                ["coordinator"] = 75,
                ["crisis_officer"] = 95
            },
            new[]
            {
                "reward.theme.city_day",
                "reward.theme.city_night",
                "reward.ui_skin.dark_blue",
                "reward.badge.steady_hands"
            });
        return Task.FromResult(payload);
    }
}
