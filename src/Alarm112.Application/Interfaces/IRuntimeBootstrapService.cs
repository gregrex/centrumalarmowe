using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IRuntimeBootstrapService
{
    Task<IReadOnlyList<CampaignNodeRuntimeDto>> GetChapterRuntimeAsync(CancellationToken cancellationToken);
    Task<RoleSelectionPreviewDto> GetRoleSelectionPreviewAsync(CancellationToken cancellationToken);
    Task<RoundBootstrapDto> GetRoundBootstrapAsync(string? missionId, string? mode, string? role, CancellationToken cancellationToken);
    Task<IReadOnlyList<ObjectiveGradeDto>> GetObjectiveGradesAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<VerticalSliceScenePackDto>> GetFinalVerticalSliceScenesAsync(CancellationToken cancellationToken);
}
