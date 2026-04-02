namespace Alarm112.Contracts;

public sealed record BotFillPreviewDto(
    string Style,
    IReadOnlyList<string> ExpectedWeaknesses,
    string SummaryKey);
