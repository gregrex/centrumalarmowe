namespace Alarm112.Contracts;

public sealed record TelemetryMetricDto(
    string Id,
    string Label,
    double Value,
    string Trend);
