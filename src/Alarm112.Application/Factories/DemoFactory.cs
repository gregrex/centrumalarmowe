using Alarm112.Contracts;
using Alarm112.Domain;

namespace Alarm112.Application.Factories;

public static class DemoFactory
{
    public static SessionSnapshotDto Create(string sessionId)
    {
        return new SessionSnapshotDto(
            SessionId: sessionId,
            SessionCode: "DEMO112",
            State: SessionState.Active.ToString(),
            Roles: new[]
            {
                new RoleSlotDto("Operator", true, false, "operator-1"),
                new RoleSlotDto("Dispatcher", false, true, null),
                new RoleSlotDto("Coordinator", true, false, "coord-1"),
                new RoleSlotDto("CrisisOfficer", false, true, null)
            },
            Incidents: new[]
            {
                new IncidentDto("INC-001", "Bus driver unconscious", "Medical", "Critical", "Transit", "New"),
                new IncidentDto("INC-002", "Kitchen fire", "Fire", "Medium", "HousingA", "Assigned")
            },
            Units: new[]
            {
                new DispatchUnitDto("AMB-01", "Ambulance", "Available", "Hospital1"),
                new DispatchUnitDto("FIRE-03", "FireTruck", "OnRoute", "HousingA"),
                new DispatchUnitDto("POL-02", "Police", "Busy", "Downtown")
            },
            Alerts: new[]
            {
                new HudAlertDto("ALT-01", "Critical case incoming", "Critical"),
                new HudAlertDto("ALT-02", "AI has taken Dispatcher slot", "Info")
            });
    }
}
