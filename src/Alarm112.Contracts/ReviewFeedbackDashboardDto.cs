namespace Alarm112.Contracts;

public sealed record ReviewFeedbackDashboardDto(
    IReadOnlyList<ReviewFeedbackItemDto> Reviewers,
    ReviewFeedbackSummaryDto Summary);
