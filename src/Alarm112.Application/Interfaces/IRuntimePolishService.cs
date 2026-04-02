using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IRuntimePolishService
{
    Task<LiveRouteRuntimeDto> GetLiveRouteRuntimeAsync(string? missionId, CancellationToken cancellationToken);
    Task<RoundTimerDto> GetRoundTimerAsync(string? missionId, CancellationToken cancellationToken);
    Task<IReadOnlyList<ChainEscalationStateDto>> GetChainEscalationRuntimeAsync(string? missionId, CancellationToken cancellationToken);
    Task<DemoMissionPolishDto> GetDemoMissionPolishAsync(string? missionId, CancellationToken cancellationToken);
}
