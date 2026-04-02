using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IRealAndroidBuildService
{
    Task<RealAndroidBuildDto> GetRealAndroidBuildAsync(CancellationToken cancellationToken);
    Task<BugfixFreezeDto> GetBugfixFreezeAsync(CancellationToken cancellationToken);
    Task<OperatorDispatcherShowcaseDto> GetOperatorDispatcherShowcaseAsync(CancellationToken cancellationToken);
    Task<FinalPolishPackDto> GetFinalPolishPackAsync(CancellationToken cancellationToken);
    Task<ReleaseFeedbackLoopV2Dto> GetReleaseFeedbackLoopV2Async(CancellationToken cancellationToken);
}
