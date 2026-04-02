namespace Alarm112.Contracts;

public sealed record MissionRuntimeStateDto(
    string MissionId,
    string TitleKey,
    string Difficulty,
    string RuntimeState,
    int CityStability,
    string PressureState,
    IReadOnlyList<ActiveIncidentDto> ActiveIncidents,
    IReadOnlyList<UnitRuntimeDto> AvailableUnits);
