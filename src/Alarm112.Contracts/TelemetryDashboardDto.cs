namespace Alarm112.Contracts;

public sealed record TelemetryDashboardDto(
    string MissionId,
    IReadOnlyList<TelemetryMetricDto> Kpis,
    IReadOnlyList<TelemetryHeatmapPointDto> Heatmap,
    string Status);
