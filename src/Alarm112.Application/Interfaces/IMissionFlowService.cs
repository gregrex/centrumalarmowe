using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IMissionFlowService
{
    Task<MissionBriefingDto> GetMissionBriefingAsync(string? missionId, CancellationToken cancellationToken);
    Task<TeamReadinessDto> GetTeamReadinessAsync(string? missionId, CancellationToken cancellationToken);
    Task<PostRoundReportDto> GetPostRoundReportAsync(string? missionId, CancellationToken cancellationToken);
    Task<MissionCompleteFlowDto> GetMissionCompleteFlowAsync(string? missionId, CancellationToken cancellationToken);
}
