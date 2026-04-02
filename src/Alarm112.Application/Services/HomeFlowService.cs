using Alarm112.Application.Interfaces;
using Alarm112.Application.Models;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class HomeFlowService : IHomeFlowService
{
    private readonly IContentBundleLoader _loader;

    public HomeFlowService(IContentBundleLoader loader) => _loader = loader;

    public async Task<HomeHubDto> GetHomeHubAsync(CancellationToken cancellationToken)
    {
        var json = await _loader.LoadContentAsync<HomeHubJson>("home-hub.v1.json", cancellationToken);
        var cards = json.Cards.Select(c => new HomeCardDto(c.Id, c.Type, c.LabelKey, c.State, c.Route)).ToArray();
        return new HomeHubDto(
            json.DefaultScreen,
            json.ContinueSession?.SessionId ?? "DEMO112",
            json.ContinueSession?.Summary ?? "",
            cards);
    }

    public Task<CampaignOverviewDto> GetCampaignOverviewAsync(CancellationToken cancellationToken)
    {
        var payload = new CampaignOverviewDto(
            "chapter.01",
            "mission.01.03",
            new[]
            {
                new CampaignNodeDto("mission.01.01", "tutorial", "completed", "Pierwsze zgloszenia"),
                new CampaignNodeDto("mission.01.02", "standard", "completed", "Narastajace przeciazenie"),
                new CampaignNodeDto("mission.01.03", "critical", "active", "Burza i blackout"),
                new CampaignNodeDto("mission.01.04", "challenge", "locked", "Nocny sztorm")
            },
            new[] { "reward.badge.steady_hands", "reward.theme.city_storm" });
        return Task.FromResult(payload);
    }

    public Task<IReadOnlyList<DailyChallengeDto>> GetDailyChallengesAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<DailyChallengeDto> payload = new[]
        {
            new DailyChallengeDto("daily.route_efficiency", "dispatcher", "normal", "xp.120"),
            new DailyChallengeDto("daily.multi_call_triage", "operator", "hard", "badge.fast_filter"),
            new DailyChallengeDto("daily.party_anchor", "coordinator", "hard", "cosmetic.frame.coop")
        };
        return Task.FromResult(payload);
    }

    public Task<SettingsBundleDto> GetSettingsBundleAsync(CancellationToken cancellationToken)
    {
        var payload = new SettingsBundleDto(new[]
        {
            new SettingsSectionDto("settings.audio", "settings.audio", new[] { "master", "music", "sfx", "radio", "ducking", "haptics" }),
            new SettingsSectionDto("settings.accessibility", "settings.accessibility", new[] { "fontScale", "highContrast", "colorblind", "reducedMotion", "leftHandMode", "captions" }),
            new SettingsSectionDto("settings.controls", "settings.controls", new[] { "tapSize", "confirmDispatch", "dragMode", "holdToConfirm" }),
            new SettingsSectionDto("settings.data", "settings.data", new[] { "analytics", "cloudSync", "clearCache" })
        });
        return Task.FromResult(payload);
    }

    public Task<IReadOnlyList<ScreenAudioRouteDto>> GetScreenAudioRoutesAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ScreenAudioRouteDto> payload = new[]
        {
            new ScreenAudioRouteDto("menu.home", "menu.home.calm", "stinger.home.enter"),
            new ScreenAudioRouteDto("menu.campaign", "menu.campaign.focus", "stinger.campaign.enter"),
            new ScreenAudioRouteDto("menu.coop", "menu.coop.anticipation", "stinger.coop.enter"),
            new ScreenAudioRouteDto("menu.profile", "menu.profile.light", "stinger.profile.enter"),
            new ScreenAudioRouteDto("menu.settings", "menu.settings.neutral", "stinger.settings.enter")
        };
        return Task.FromResult(payload);
    }
}
