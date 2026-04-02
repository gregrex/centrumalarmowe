using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IRuntimeUiFlowService
{
    Task<RuntimeHudDto> GetRuntimeHudAsync(string? missionId, CancellationToken cancellationToken);
    Task<IReadOnlyList<RecoveryHudTriggerDto>> GetRecoveryHudTriggersAsync(string? missionId, CancellationToken cancellationToken);
    Task<FailRetryNextFlowDto> GetFailRetryNextFlowAsync(string? missionId, string? resultState, CancellationToken cancellationToken);
    Task<MissionSlicePolishDto> GetMissionSlicePolishAsync(string? missionId, CancellationToken cancellationToken);
}