using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IInternalTestService
{
    Task<InternalTestPackDto> GetInternalTestPackAsync(CancellationToken cancellationToken);
    Task<GooglePlayInternalTestingDto> GetGooglePlayInternalTestingAsync(CancellationToken cancellationToken);
    Task<LiveopsReviewPanelDto> GetLiveopsReviewPanelAsync(CancellationToken cancellationToken);
    Task<FinalTrailerStoreDemoDto> GetFinalTrailerStoreDemoAsync(CancellationToken cancellationToken);
    Task<ReleaseReadinessV24Dto> GetReleaseReadinessV24Async(CancellationToken cancellationToken);
}
