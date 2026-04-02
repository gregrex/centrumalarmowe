using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class RoundRuntimeService : IRoundRuntimeService
{
    public Task<RouteOverlayDto> GetRouteOverlayAsync(string sessionId, string incidentId, string unitId, CancellationToken cancellationToken)
    {
        var payload = new RouteOverlayDto(
            sessionId,
            incidentId,
            unitId,
            "heatmap.core.traffic-heavy",
            new[]
            {
                new RouteOverlaySegmentDto("seg.001", "station.fire.1", "cross.central.1", "overlay.style.normal", false, 2),
                new RouteOverlaySegmentDto("seg.002", "cross.central.1", "bridge.north", "overlay.style.warning", false, 4),
                new RouteOverlaySegmentDto("seg.003", "bridge.north", "zone.residential.north", "overlay.style.critical", true, 5)
            },
            new[] { "route.warning.traffic", "route.warning.narrow-access" });

        return Task.FromResult(payload);
    }

    public Task<IReadOnlyCollection<IncidentDeltaDto>> GetLiveDeltasAsync(string sessionId, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<IncidentDeltaDto> payload = new[]
        {
            new IncidentDeltaDto("delta.001", "inc.fire.002", "delta.type.escalated", "critical", "active", -30, true, DateTimeOffset.UtcNow.AddSeconds(-8)),
            new IncidentDeltaDto("delta.002", "inc.medical.001", "delta.type.timeout-risk", "high", "active", -20, true, DateTimeOffset.UtcNow.AddSeconds(-6)),
            new IncidentDeltaDto("delta.003", "inc.medical.004", "delta.type.shared-action-needed", "critical", "awaiting-ack", -10, true, DateTimeOffset.UtcNow.AddSeconds(-4)),
            new IncidentDeltaDto("delta.004", "inc.medical.001", "delta.type.resolved", "normal", "resolved", 0, false, DateTimeOffset.UtcNow.AddSeconds(-1))
        };

        return Task.FromResult(payload);
    }

    public Task<IReadOnlyCollection<UnitRuntimeDto>> GetUnitsRuntimeAsync(string sessionId, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<UnitRuntimeDto> payload = new[]
        {
            new UnitRuntimeDto("unit.ambulance.01", "A-01", "medical", "returning", "zone.central", 35, 90, true, false),
            new UnitRuntimeDto("unit.ambulance.02", "A-02", "medical", "engaged", "zone.industrial.east", 120, 210, false, false),
            new UnitRuntimeDto("unit.fire.01", "F-01", "fire", "available", "station.fire.1", 0, 160, true, false),
            new UnitRuntimeDto("unit.police.01", "P-01", "police", "bot-assigned", "district.stadium", 20, 110, true, true)
        };

        return Task.FromResult(payload);
    }

    public Task<RoundStateDto> GetRoundStateAsync(string sessionId, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<RolePanelStateDto> panels = new[]
        {
            new RolePanelStateDto("role.operator", "split.panel.layout.operator-dispatcher", true, true, "inc.medical.001", 2, 3, "alert.loaded"),
            new RolePanelStateDto("role.dispatcher", "split.panel.layout.operator-dispatcher", true, true, "inc.fire.002", 1, 4, "alert.warning"),
            new RolePanelStateDto("role.coordinator", "split.panel.layout.coordinator-crisis", false, true, "inc.medical.004", 1, 2, "alert.critical"),
            new RolePanelStateDto("role.crisis-officer", "split.panel.layout.coordinator-crisis", false, true, "inc.fire.002", 0, 1, "alert.loaded")
        };

        var payload = new RoundStateDto(sessionId, "round.demo.001", 12, 44, "round.phase.dispatch-window", 2, 2, 3, 1, panels);
        return Task.FromResult(payload);
    }
}
