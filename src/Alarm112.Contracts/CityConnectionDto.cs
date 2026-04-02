namespace Alarm112.Contracts;

public sealed record CityConnectionDto(
    string ConnectionId,
    string FromNodeId,
    string ToNodeId,
    string TypeId);
