namespace Alarm112.Contracts;

public sealed record MissionCompleteGateDto(
    string MissionId,
    string GateState,
    string ReasonKey,
    string Grade,
    int Stars,
    int CityStability,
    IReadOnlyList<string> NextActions);
