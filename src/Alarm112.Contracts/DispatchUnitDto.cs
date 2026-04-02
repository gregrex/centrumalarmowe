namespace Alarm112.Contracts;

public sealed record DispatchUnitDto(
    string UnitId,
    string UnitType,
    string Status,
    string Zone);
