using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface ICityMapService
{
    Task<CityMapDto> GetCityMapAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<SessionTimelineItemDto>> GetTimelineAsync(string sessionId, CancellationToken cancellationToken);
    Task<DispatchResultDto> DispatchAsync(string sessionId, DispatchCommandDto command, CancellationToken cancellationToken);
}
