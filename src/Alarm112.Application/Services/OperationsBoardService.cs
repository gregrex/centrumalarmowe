using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class OperationsBoardService : IOperationsBoardService
{
    public Task<ActiveIncidentBoardDto> GetActiveIncidentsAsync(string sessionId, CancellationToken cancellationToken)
    {
        var items = new[]
        {
            new ActiveIncidentDto("inc.medical.001", "incident.medical.chest-pain", "incident.medical.chest-pain.title", "high", "zone.central", "active", "role.dispatcher", "unit.ambulance.01", 180, 7, false, new [] { "medical", "high-priority" }),
            new ActiveIncidentDto("inc.fire.002", "incident.fire.apartment", "incident.fire.apartment.title", "critical", "zone.residential.north", "active", "role.coordinator", "unit.fire.01", 240, 9, true, new [] { "fire", "shared-action" }),
            new ActiveIncidentDto("inc.medical.004", "incident.medical.unconscious", "incident.medical.unconscious.title", "critical", "zone.industrial.east", "active", "role.dispatcher", "unit.ambulance.02", 300, 10, true, new [] { "medical", "industrial", "shared-action" })
        };

        return Task.FromResult(new ActiveIncidentBoardDto(sessionId, items.Length, items.Count(x => x.Severity == "critical"), items));
    }

    public Task<IReadOnlyCollection<MapFilterDto>> GetMapFiltersAsync(CancellationToken cancellationToken)
    {
        IReadOnlyCollection<MapFilterDto> filters = new[]
        {
            new MapFilterDto("filter.severity.critical", "filter.severity.critical", "severity", true, new[] { "critical" }),
            new MapFilterDto("filter.service.medical", "filter.service.medical", "service", true, new[] { "medical" }),
            new MapFilterDto("filter.service.fire", "filter.service.fire", "service", true, new[] { "fire" }),
            new MapFilterDto("filter.state.shared-required", "filter.state.shared-required", "state", false, new[] { "shared-action" })
        };

        return Task.FromResult(filters);
    }

    public Task<RoutePreviewDto> PreviewRouteAsync(string sessionId, RoutePreviewRequestDto request, CancellationToken cancellationToken)
    {
        var warningCodes = request.UnitId.Contains("fire", StringComparison.OrdinalIgnoreCase)
            ? new[] { "route.warning.traffic" }
            : Array.Empty<string>();

        var route = new RoutePreviewDto(
            Guid.NewGuid().ToString("N"),
            request.IncidentId,
            request.UnitId,
            warningCodes.Length > 0 ? 240 : 180,
            warningCodes.Length > 0 ? 4.8 : 3.2,
            new[] { "station.medical.1", "zone.central", "zone.residential.north" },
            warningCodes,
            warningCodes.Length > 0 ? "route.line.warning" : "route.line.normal");

        return Task.FromResult(route);
    }

    public Task<SharedActionAckDto> ResolveSharedActionAsync(string sessionId, SharedActionDto action, CancellationToken cancellationToken)
    {
        var accepted = action.AllowBotAssist || action.RequiredRoles.Count <= 1;
        var result = new SharedActionAckDto(
            action.SharedActionId,
            action.RequiredRoles.FirstOrDefault() ?? action.RequestedByRole,
            accepted,
            action.AllowBotAssist,
            accepted ? "shared-action.accepted" : "shared-action.pending");

        return Task.FromResult(result);
    }
}
