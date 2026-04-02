namespace Alarm112.Contracts;

public sealed record SessionSnapshotDto(
    string SessionId,
    string SessionCode,
    string State,
    IReadOnlyCollection<RoleSlotDto> Roles,
    IReadOnlyCollection<IncidentDto> Incidents,
    IReadOnlyCollection<DispatchUnitDto> Units,
    IReadOnlyCollection<HudAlertDto> Alerts);
