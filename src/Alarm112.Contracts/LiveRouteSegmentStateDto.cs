namespace Alarm112.Contracts;

public sealed record LiveRouteSegmentStateDto(
    string RouteId,
    string FromNodeId,
    string ToNodeId,
    string State,
    int EtaSeconds,
    string VisualStyle);
