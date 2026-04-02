namespace Alarm112.Contracts;

public sealed record UnitRuntimeDto(
    string UnitId,
    string CallSign,
    string UnitType,
    string Status,
    string CurrentNodeId,
    int CooldownSeconds,
    int EtaSeconds,
    bool Available,
    bool IsBotBackfilled);
