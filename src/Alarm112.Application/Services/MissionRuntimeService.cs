using Alarm112.Application.Interfaces;
using Alarm112.Application.Models;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class MissionRuntimeService : IMissionRuntimeService
{
    private readonly IContentBundleLoader _loader;

    public MissionRuntimeService(IContentBundleLoader loader) => _loader = loader;

    public async Task<MissionRuntimeStateDto> GetMissionRuntimeAsync(string? missionId, CancellationToken cancellationToken)
    {
        var json = await _loader.LoadContentAsync<FullMissionRuntimeJson>("full-mission-runtime.v1.json", cancellationToken);
        var resolved = string.IsNullOrWhiteSpace(missionId) ? json.MissionId : missionId;

        var incidents = json.ActiveIncidents.Select(i => new ActiveIncidentDto(
            i.Id, i.Type, $"incident.{i.Type}.{i.Id}", i.Priority, i.Node,
            "queued", "operator", i.AssignedUnitId, 60, 40, false, Array.Empty<string>())).ToArray();

        var units = json.AvailableUnits.Select(u => new UnitRuntimeDto(
            u.Id, u.Id.ToUpperInvariant(), u.Type, u.Status, u.Id,
            u.CooldownSeconds, u.EtaSeconds, u.Status == "available", false)).ToArray();

        return new MissionRuntimeStateDto(
            resolved, json.TitleKey, json.Difficulty, json.RuntimeState,
            json.CityStability, json.PressureState, incidents, units);
    }

    public Task<IReadOnlyList<RuntimeDispatchOutcomeDto>> GetDispatchOutcomesAsync(string? missionId, CancellationToken cancellationToken)
    {
        IReadOnlyList<RuntimeDispatchOutcomeDto> payload = new[]
        {
            new RuntimeDispatchOutcomeDto("inc.med.001", "dispatch.success", 15, 2, "dispatch.success"),
            new RuntimeDispatchOutcomeDto("inc.fire.002", "dispatch.delayed", -8, -5, "dispatch.delayed"),
            new RuntimeDispatchOutcomeDto("inc.pol.003", "dispatch.rerouted", 4, 0, "dispatch.rerouted")
        };
        return Task.FromResult(payload);
    }

    public Task<MissionCompleteGateDto> GetMissionCompleteGateAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolved = string.IsNullOrWhiteSpace(missionId) ? "mission.01.04" : missionId;
        var payload = new MissionCompleteGateDto(
            resolved, "gate.success", "gate.success.reason.objectives_met",
            "A-", 2, 61, new[] { "continue_to_report", "retry", "back_home" });
        return Task.FromResult(payload);
    }

    public Task<ObjectiveTrackerDto> GetObjectiveTrackerAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolved = string.IsNullOrWhiteSpace(missionId) ? "mission.01.04" : missionId;
        var payload = new ObjectiveTrackerDto(
            resolved,
            new[]
            {
                new ObjectiveTrackerItemDto("obj.primary.01", "obj.primary.01", "completed", 100, "critical"),
                new ObjectiveTrackerItemDto("obj.primary.02", "obj.primary.02", "active", 60, "high"),
                new ObjectiveTrackerItemDto("obj.secondary.01", "obj.secondary.01", "active", 45, "medium")
            },
            new[]
            {
                new EventFeedItemDto("feed.001", "critical", "feed.dispatch.success.medical"),
                new EventFeedItemDto("feed.002", "high", "feed.route.blocked"),
                new EventFeedItemDto("feed.003", "medium", "feed.objective.progress")
            });
        return Task.FromResult(payload);
    }

    public async Task<MissionScriptDto> GetMissionScriptAsync(string? missionId, CancellationToken cancellationToken)
    {
        var json = await _loader.LoadContentAsync<MissionScriptJson>("mission-script.01.full.json", cancellationToken);
        var resolved = string.IsNullOrWhiteSpace(missionId) ? json.MissionId : missionId;
        var steps = json.Timeline.Select(t => new MissionScriptStepDto(t.AtSeconds, t.Event)).ToArray();
        return new MissionScriptDto(resolved, steps);
    }
}
