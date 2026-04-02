using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IPlayableRuntimeService
{
    Task<PlayableRuntimeMapDto> GetPlayableRuntimeMapAsync(string? missionId, CancellationToken cancellationToken);
    Task<IReadOnlyList<ObjectiveStateTransitionDto>> GetObjectiveStateMachineAsync(string? missionId, CancellationToken cancellationToken);
    Task<DispatcherLoopDto> GetDispatcherLoopAsync(string? missionId, CancellationToken cancellationToken);
    Task<CityStatusDto> GetCityPressureRuntimeAsync(string? missionId, CancellationToken cancellationToken);
    Task<ReportProgressionDto> GetReportProgressionAsync(string? missionId, CancellationToken cancellationToken);
}
