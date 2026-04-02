using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IReleaseCandidateService
{
    Task<ReleaseCandidatePackageDto> GetReleaseCandidatePackageAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<BugBashChecklistItemDto>> GetBugBashChecklistAsync(CancellationToken cancellationToken);
    Task<ReleaseNotesDraftDto> GetReleaseNotesDraftAsync(CancellationToken cancellationToken);
    Task<AndroidRcPipelineDto> GetAndroidRcPipelineAsync(CancellationToken cancellationToken);
    Task<FinalPromoMediaDto> GetFinalPromoMediaAsync(CancellationToken cancellationToken);
}
