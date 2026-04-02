using Alarm112.Application.Interfaces;
using Alarm112.Application.Models;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class MissionFlowService : IMissionFlowService
{
    private readonly IContentBundleLoader _loader;

    public MissionFlowService(IContentBundleLoader loader) => _loader = loader;

    public async Task<MissionBriefingDto> GetMissionBriefingAsync(string? missionId, CancellationToken cancellationToken)
    {
        var json = await _loader.LoadContentAsync<MissionBriefingJson>("mission-briefing.v1.json", cancellationToken);
        var resolved = string.IsNullOrWhiteSpace(missionId) ? json.MissionId : missionId;
        return new MissionBriefingDto(
            resolved,
            json.TitleKey,
            json.Difficulty,
            json.EstimatedMinutes,
            json.WeatherPreset,
            json.TimeOfDay,
            json.PrimaryObjectives,
            json.SecondaryObjectives,
            json.RiskTags,
            json.RecommendedRoles,
            json.SuggestedUnits,
            json.Speaker?.PortraitId ?? "portrait.commander.helena",
            json.Speaker?.LineKey ?? "briefing.line.001",
            json.Hotspots);
    }

    public async Task<TeamReadinessDto> GetTeamReadinessAsync(string? missionId, CancellationToken cancellationToken)
    {
        var json = await _loader.LoadContentAsync<TeamReadinessJson>("team-readiness.v1.json", cancellationToken);
        var resolved = string.IsNullOrWhiteSpace(missionId) ? json.MissionId : missionId;
        var slots = json.Slots.Select(s => new TeamReadinessSlotDto(
            s.RoleId, s.State, s.OccupantId, s.Ready, s.NetworkQuality)).ToArray();
        return new TeamReadinessDto(
            resolved, json.Mode, slots, json.BotFillMode, json.CanStart, json.Warnings, json.TeamScore);
    }

    public async Task<PostRoundReportDto> GetPostRoundReportAsync(string? missionId, CancellationToken cancellationToken)
    {
        var json = await _loader.LoadContentAsync<PostRoundReportJson>("postround-report.v1.json", cancellationToken);
        var resolved = string.IsNullOrWhiteSpace(missionId) ? json.MissionId : missionId;
        var objectives = json.Objectives.Select(o =>
            new MissionBriefingObjectiveDto(o.ObjectiveId, "primary", o.State)).ToArray();
        var metrics = json.Metrics.Select(m => new PostRoundMetricDto(m.MetricId, m.Value)).ToArray();
        return new PostRoundReportDto(
            resolved, json.GradeId, json.Score, json.Stars,
            objectives, metrics, json.Mistakes, json.Rewards, json.NextActions);
    }

    public async Task<MissionCompleteFlowDto> GetMissionCompleteFlowAsync(string? missionId, CancellationToken cancellationToken)
    {
        var json = await _loader.LoadContentAsync<MissionCompleteFlowJson>("mission-complete-flow.v1.json", cancellationToken);
        return new MissionCompleteFlowDto(json.Steps, json.AudioState, json.ScenePreset);
    }
}
