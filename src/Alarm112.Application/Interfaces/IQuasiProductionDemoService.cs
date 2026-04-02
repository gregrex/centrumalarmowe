using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IQuasiProductionDemoService
{
    Task<VisualRuntimeRouteLayerDto> GetVisualRuntimeRouteLayerAsync(string? missionId, CancellationToken cancellationToken);
    Task<IReadOnlyList<RecoveryDecisionCardDto>> GetRecoveryDecisionCardsAsync(string? missionId, CancellationToken cancellationToken);
    Task<IReadOnlyList<MissionFailBranchDto>> GetMissionFailBranchesAsync(string? missionId, CancellationToken cancellationToken);
    Task<ReportRoomPolishDto> GetReportRoomPolishAsync(string? missionId, CancellationToken cancellationToken);
    Task<QuasiProductionDemoFlowDto> GetQuasiProductionDemoFlowAsync(string? missionId, CancellationToken cancellationToken);
}
