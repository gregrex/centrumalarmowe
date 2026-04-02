using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface INearFinalSliceService
{
    Task<RuntimeScoreboardDto> GetRuntimeScoreboardAsync(string? missionId, CancellationToken cancellationToken);
    Task<IReadOnlyList<RewardRevealStateDto>> GetRewardRevealStatesAsync(string? missionId, CancellationToken cancellationToken);
    Task<RetryPreparationDto> GetRetryPreparationAsync(string? missionId, CancellationToken cancellationToken);
    Task<NextMissionHandoffDto> GetNextMissionHandoffAsync(string? missionId, CancellationToken cancellationToken);
    Task<NearFinalSliceFlowDto> GetNearFinalSliceFlowAsync(CancellationToken cancellationToken);
}
