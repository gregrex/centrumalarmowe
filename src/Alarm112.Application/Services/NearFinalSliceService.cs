using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class NearFinalSliceService : INearFinalSliceService
{
    public Task<RuntimeScoreboardDto> GetRuntimeScoreboardAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.demo.19" : missionId;
        var payload = new RuntimeScoreboardDto(
            resolvedMissionId,
            "partial",
            8420,
            2,
            new[]
            {
                new RuntimeScoreboardRowDto("score.total", "score.total", "8420", "gold"),
                new RuntimeScoreboardRowDto("score.objectives", "score.objectives", "3 / 4", "green"),
                new RuntimeScoreboardRowDto("score.city_pressure", "score.city_pressure", "Stable", "blue"),
                new RuntimeScoreboardRowDto("score.recovery", "score.recovery_quality", "Good", "purple")
            });
        return Task.FromResult(payload);
    }

    public Task<IReadOnlyList<RewardRevealStateDto>> GetRewardRevealStatesAsync(string? missionId, CancellationToken cancellationToken)
    {
        IReadOnlyList<RewardRevealStateDto> payload = new[]
        {
            new RewardRevealStateDto("xp_reveal", "XP Revealed", false, new[] { "xp:120" }),
            new RewardRevealStateDto("star_reveal", "Stars Awarded", false, new[] { "star:2" }),
            new RewardRevealStateDto("badge_reveal", "Badge Unlocked", false, new[] { "badge:route_stabilizer" }),
            new RewardRevealStateDto("unlock_teaser", "Next Unlock Teaser", true, new[] { "teaser:chapter02_dispatch_boost" })
        };
        return Task.FromResult(payload);
    }

    public Task<RetryPreparationDto> GetRetryPreparationAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.demo.19" : missionId;
        var payload = new RetryPreparationDto(
            resolvedMissionId,
            "dispatcher",
            new[] { "keep one EMS reserve", "trigger recovery card earlier", "reroute before yellow threshold" },
            new[] { "operator-bot", "crisis-bot" });
        return Task.FromResult(payload);
    }

    public Task<NextMissionHandoffDto> GetNextMissionHandoffAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.demo.19" : missionId;
        var payload = new NextMissionHandoffDto(
            resolvedMissionId,
            "chapter02.mission03",
            "Blackout in the Old Town",
            "Storm damage threatens the power grid and ambulance routing.",
            new[] { "ems.alpha", "fire.bravo", "police.delta" });
        return Task.FromResult(payload);
    }

    public Task<NearFinalSliceFlowDto> GetNearFinalSliceFlowAsync(CancellationToken cancellationToken)
    {
        var payload = new NearFinalSliceFlowDto(
            "slice.v19",
            new[] { "home", "briefing", "runtime", "scoreboard", "reward_reveal", "retry_or_next" },
            "dispatcher");
        return Task.FromResult(payload);
    }
}
