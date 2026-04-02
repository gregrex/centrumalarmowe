using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IMissionRuntimeService
{
    Task<MissionRuntimeStateDto> GetMissionRuntimeAsync(string? missionId, CancellationToken cancellationToken);
    Task<IReadOnlyList<RuntimeDispatchOutcomeDto>> GetDispatchOutcomesAsync(string? missionId, CancellationToken cancellationToken);
    Task<MissionCompleteGateDto> GetMissionCompleteGateAsync(string? missionId, CancellationToken cancellationToken);
    Task<ObjectiveTrackerDto> GetObjectiveTrackerAsync(string? missionId, CancellationToken cancellationToken);
    Task<MissionScriptDto> GetMissionScriptAsync(string? missionId, CancellationToken cancellationToken);
}
