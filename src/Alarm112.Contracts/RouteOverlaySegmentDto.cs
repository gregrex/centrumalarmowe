namespace Alarm112.Contracts;

public sealed record RouteOverlaySegmentDto(
    string SegmentId,
    string FromNodeId,
    string ToNodeId,
    string Style,
    bool IsCritical,
    int TrafficLevel);
