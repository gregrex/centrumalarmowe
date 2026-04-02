namespace Alarm112.Contracts;

public sealed record ContentValidationResultDto(
    bool IsValid,
    IReadOnlyCollection<ContentValidationIssueDto> Issues);
