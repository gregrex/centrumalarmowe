using Alarm112.Application.Interfaces;

namespace Alarm112.Api.Endpoints;

public static class CampaignEndpoints
{
    public static WebApplication MapCampaignEndpoints(this WebApplication app)
    {
        app.MapGet("/api/home-hub",
            async (IHomeFlowService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetHomeHubAsync(ct)));

        app.MapGet("/api/campaign-overview/demo",
            async (IHomeFlowService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetCampaignOverviewAsync(ct)));

        app.MapGet("/api/daily-challenges/demo",
            async (IHomeFlowService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetDailyChallengesAsync(ct)));

        app.MapGet("/api/settings-bundle",
            async (IHomeFlowService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetSettingsBundleAsync(ct)));

        app.MapGet("/api/audio-routes",
            async (IHomeFlowService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetScreenAudioRoutesAsync(ct)));

        app.MapGet("/api/campaign-chapters/demo",
            async (ICampaignEntryService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetCampaignChaptersAsync(ct)));

        app.MapGet("/api/mission-entry/demo",
            async (string? missionId, ICampaignEntryService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetMissionEntryAsync(missionId, ct)));

        app.MapGet("/api/profile-cosmetics/demo",
            async (ICampaignEntryService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetProfileCosmeticsAsync(ct)));

        app.MapGet("/api/player-identity/demo",
            async (ICampaignEntryService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetPlayerIdentityAsync(ct)));

        app.MapGet("/api/home-to-round-audio",
            async (ICampaignEntryService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetHomeToRoundAudioAsync(ct)));

        return app;
    }
}
