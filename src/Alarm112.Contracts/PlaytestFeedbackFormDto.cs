namespace Alarm112.Contracts;

public sealed record PlaytestFeedbackFormDto(
    string FormId,
    IReadOnlyList<PlaytestFeedbackFieldDto> Fields,
    IReadOnlyList<string> Tags,
    bool AllowScreenshotUpload);
