namespace Alarm112.Contracts;

public sealed record ReviewFeedbackItemDto(
    string Reviewer,
    string Severity,
    string Area,
    string Note);
