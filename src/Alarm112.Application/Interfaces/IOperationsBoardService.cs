using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IOperationsBoardService
{
    Task<ActiveIncidentBoardDto> GetActiveIncidentsAsync(string sessionId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<MapFilterDto>> GetMapFiltersAsync(CancellationToken cancellationToken);
    Task<RoutePreviewDto> PreviewRouteAsync(string sessionId, RoutePreviewRequestDto request, CancellationToken cancellationToken);
    Task<SharedActionAckDto> ResolveSharedActionAsync(string sessionId, SharedActionDto action, CancellationToken cancellationToken);
}
