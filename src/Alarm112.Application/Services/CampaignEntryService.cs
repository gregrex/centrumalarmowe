using Alarm112.Application.Interfaces;
using Alarm112.Application.Models;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class CampaignEntryService : ICampaignEntryService
{
    private readonly IContentBundleLoader _loader;

    public CampaignEntryService(IContentBundleLoader loader) => _loader = loader;

    public async Task<IReadOnlyList<CampaignChapterDto>> GetCampaignChaptersAsync(CancellationToken cancellationToken)
    {
        var json = await _loader.LoadContentAsync<CampaignChaptersJson>("campaign-chapters.v1.json", cancellationToken);
        IReadOnlyList<CampaignChapterDto> result = json.Chapters.Select((c, i) =>
        {
            var nodes = c.NodeIds.Select((nodeId, idx) => new CampaignMissionNodeDto(
                nodeId,
                i == 0 && idx < 2 ? "tutorial" : "standard",
                i == 0 && idx < 2 ? "completed" : (i == 0 && idx == 2 ? "active" : "locked"),
                Math.Round(0.10 + idx * 0.18, 2),
                Math.Round(0.62 - idx * 0.07, 2),
                $"{nodeId}.title"
            )).ToArray();
            return new CampaignChapterDto(c.Id, c.TitleKey, c.ThemeId, c.Progress, nodes);
        }).ToArray();
        return result;
    }

    public async Task<CampaignMissionEntryDto> GetMissionEntryAsync(string? missionId, CancellationToken cancellationToken)
    {
        var json = await _loader.LoadContentAsync<MissionEntryJson>("mission-entry-flow.v1.json", cancellationToken);
        var resolved = string.IsNullOrWhiteSpace(missionId) ? json.MissionId : missionId;
        return new CampaignMissionEntryDto(
            resolved,
            $"{resolved}.title",
            json.ChapterId,
            json.EstimatedMinutes,
            json.Difficulty,
            json.RecommendedRole,
            json.WeatherPreset,
            json.TimeOfDay,
            json.StartingUnits,
            json.RiskTags,
            json.Rewards,
            json.AvailableSlots,
            json.BotFillSummary?.DefaultMode ?? "balanced");
    }

    public Task<IReadOnlyList<ProfileCosmeticDto>> GetProfileCosmeticsAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ProfileCosmeticDto> payload = new[]
        {
            new ProfileCosmeticDto("portrait.dispatcher.steel", "portrait", "common", "starter"),
            new ProfileCosmeticDto("frame.blackout.pulse", "frame", "rare", "chapter.03.complete"),
            new ProfileCosmeticDto("badge.fast_filter", "badge", "uncommon", "daily.challenge"),
            new ProfileCosmeticDto("title.city_anchor", "title", "heroic", "coop.milestone.10")
        };
        return Task.FromResult(payload);
    }

    public Task<PlayerIdentityDto> GetPlayerIdentityAsync(CancellationToken cancellationToken)
    {
        var payload = new PlayerIdentityDto(
            "player.demo.001",
            "Dispatcher Zero",
            "dispatcher",
            "portrait.dispatcher.steel",
            "frame.blackout.pulse",
            "badge.fast_filter",
            "title.city_anchor");
        return Task.FromResult(payload);
    }

    public Task<IReadOnlyList<HomeToRoundAudioDto>> GetHomeToRoundAudioAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<HomeToRoundAudioDto> payload = new[]
        {
            new HomeToRoundAudioDto("menu.home", "menu.campaign", "menu.campaign.focus", "stinger.campaign.enter"),
            new HomeToRoundAudioDto("menu.campaign", "menu.mission_entry", "menu.mission.tight", "stinger.mission.open"),
            new HomeToRoundAudioDto("menu.mission_entry", "session.round", "session.prep.rise", "stinger.round.start")
        };
        return Task.FromResult(payload);
    }
}
