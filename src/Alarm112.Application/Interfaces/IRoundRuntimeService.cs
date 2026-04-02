using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IRoundRuntimeService
{
    Task<RouteOverlayDto> GetRouteOverlayAsync(string sessionId, string incidentId, string unitId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<IncidentDeltaDto>> GetLiveDeltasAsync(string sessionId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<UnitRuntimeDto>> GetUnitsRuntimeAsync(string sessionId, CancellationToken cancellationToken);
    Task<RoundStateDto> GetRoundStateAsync(string sessionId, CancellationToken cancellationToken);
}
