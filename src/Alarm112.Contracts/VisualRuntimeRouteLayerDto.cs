namespace Alarm112.Contracts;

public sealed record VisualRuntimeRouteLayerDto(
    string MissionId,
    IReadOnlyList<LiveRouteSegmentStateDto> Segments,
    string FocusSegmentId,
    bool ShowDirectionArrows,
    bool ShowSeverityBadges);
