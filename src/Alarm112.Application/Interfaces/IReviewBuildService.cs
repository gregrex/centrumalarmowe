using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IReviewBuildService
{
    Task<ReviewBuildPackageDto> GetReviewBuildPackageAsync(CancellationToken cancellationToken);
    Task<TestBuildDeployDto> GetTestBuildDeployAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<StoreShotMockDto>> GetStoreShotMocksAsync(CancellationToken cancellationToken);
    Task<PlaytestFeedbackFormDto> GetPlaytestFeedbackFormAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<ReviewBuildChecklistItemDto>> GetReviewBuildChecklistAsync(CancellationToken cancellationToken);
}
