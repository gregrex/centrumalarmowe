using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IAndroidPreviewService
{
    Task<AndroidPreviewBuildDto> GetAndroidPreviewBuildAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<ReleaseReadinessChecklistItemDto>> GetReleaseReadinessChecklistAsync(CancellationToken cancellationToken);
    Task<TelemetryDashboardDto> GetTelemetryDashboardAsync(CancellationToken cancellationToken);
    Task<ReviewFeedbackDashboardDto> GetReviewFeedbackDashboardAsync(CancellationToken cancellationToken);
    Task<FinalCapturePackDto> GetFinalCapturePackAsync(CancellationToken cancellationToken);
}
