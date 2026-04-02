namespace Alarm112.Contracts;

public sealed record InternalDemoPackageDto(
    string DemoId,
    IReadOnlyList<string> Flow,
    IReadOnlyList<string> TargetAudience,
    string PresenterNotes);
