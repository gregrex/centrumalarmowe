namespace Alarm112.Contracts;

public sealed record CityNodeDto(
    string NodeId,
    string LabelKey,
    string TypeId,
    double X,
    double Y);
