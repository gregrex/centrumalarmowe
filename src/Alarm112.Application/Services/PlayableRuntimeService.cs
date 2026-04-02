using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class PlayableRuntimeService : IPlayableRuntimeService
{
    public Task<PlayableRuntimeMapDto> GetPlayableRuntimeMapAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.01.05" : missionId;
        var payload = new PlayableRuntimeMapDto(
            resolvedMissionId,
            "zone.west_bridge",
            57,
            new[]
            {
                new ActiveIncidentDto("inc.med.010", "medical", "incident.medical.010", "critical", "zone.west_bridge", "queued", "operator", "unit.ems.alpha", 35, 40, false, Array.Empty<string>()),
                new ActiveIncidentDto("inc.fire.011", "fire", "incident.fire.011", "high", "zone.mall", "queued", "dispatcher", "unit.fire.bravo", 60, 30, false, Array.Empty<string>()),
                new ActiveIncidentDto("inc.pol.012", "police", "incident.pol.012", "medium", "zone.center", "queued", "operator", "unit.pol.delta", 40, 20, false, Array.Empty<string>())
            },
            new[]
            {
                new UnitRuntimeDto("unit.ems.alpha", "EMS-A", "ambulance", "available", "zone.hospital", 0, 35, true, false),
                new UnitRuntimeDto("unit.fire.bravo", "FIRE-B", "fire_engine", "busy", "zone.mall", 0, 60, false, false),
                new UnitRuntimeDto("unit.pol.delta", "POL-D", "patrol", "available", "zone.policehub", 0, 40, true, false)
            },
            new[]
            {
                new RouteOverlaySegmentDto("route.001", "zone.hospital", "zone.west_bridge", "clear", false, 1),
                new RouteOverlaySegmentDto("route.002", "zone.firehub", "zone.mall", "delayed", true, 3),
                new RouteOverlaySegmentDto("route.003", "zone.policehub", "zone.center", "clear", false, 1)
            });

        return Task.FromResult(payload);
    }

    public Task<IReadOnlyList<ObjectiveStateTransitionDto>> GetObjectiveStateMachineAsync(string? missionId, CancellationToken cancellationToken)
    {
        IReadOnlyList<ObjectiveStateTransitionDto> payload = new[]
        {
            new ObjectiveStateTransitionDto("obj.primary.01", "active", "progress", "dispatch.medical.success", 25),
            new ObjectiveStateTransitionDto("obj.primary.01", "progress", "completed", "incident.resolved", 75),
            new ObjectiveStateTransitionDto("obj.secondary.01", "active", "at_risk", "time.exceeded.critical", -20),
            new ObjectiveStateTransitionDto("obj.secondary.01", "at_risk", "failed", "chain_event.started", -80)
        };
        return Task.FromResult(payload);
    }

    public Task<DispatcherLoopDto> GetDispatcherLoopAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.01.05" : missionId;
        var payload = new DispatcherLoopDto(
            resolvedMissionId,
            new[] { "assign", "reinforce", "reroute", "hold", "cancel" },
            new[]
            {
                new RuntimeDispatchOutcomeDto("inc.med.010", "dispatch.success.fast", 18, 6, "dispatch.success"),
                new RuntimeDispatchOutcomeDto("inc.fire.011", "dispatch.delayed", -6, -3, "dispatch.delayed"),
                new RuntimeDispatchOutcomeDto("inc.pol.012", "dispatch.standard", 4, 0, "dispatch.standard")
            },
            "assign");
        return Task.FromResult(payload);
    }

    public Task<CityStatusDto> GetCityPressureRuntimeAsync(string? missionId, CancellationToken cancellationToken)
    {
        var payload = new CityStatusDto(57, "critical", "traffic_heavy", "weather_rain", "media_spike", 5);
        return Task.FromResult(payload);
    }

    public Task<ReportProgressionDto> GetReportProgressionAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.01.05" : missionId;
        var payload = new ReportProgressionDto(
            resolvedMissionId,
            "A",
            2,
            68,
            new[] { "reward.credit.01", "reward.badge.response", "reward.cosmetic.banner" },
            new[] { "continue", "retry", "back_home" });
        return Task.FromResult(payload);
    }
}
