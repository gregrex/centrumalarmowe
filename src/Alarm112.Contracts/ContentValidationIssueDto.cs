namespace Alarm112.Contracts;

public sealed record ContentValidationIssueDto(
    string Severity,
    string Source,
    string Message);
