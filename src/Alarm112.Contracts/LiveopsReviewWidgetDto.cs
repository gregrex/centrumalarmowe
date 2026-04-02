namespace Alarm112.Contracts;

public sealed record LiveopsReviewWidgetDto(
    string Id,
    string Type,
    string Label,
    string Value,
    string Trend);
