namespace Alarm112.Contracts;

public sealed record ReviewFeedbackSummaryDto(
    int OpenBlockers,
    int P1,
    int P2,
    int P3);
