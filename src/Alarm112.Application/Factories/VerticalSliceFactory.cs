using Alarm112.Contracts;
using Alarm112.Domain;

namespace Alarm112.Application.Factories;

public static class VerticalSliceFactory
{
    public static QuickPlayBootstrapDto CreateBootstrap()
    {
        return new QuickPlayBootstrapDto(
            ScenarioId: "scenario.verticalslice.quickplay",
            Difficulty: "normal",
            PreferredRole: "role.operator",
            AutoFillBots: true,
            IncidentIds:
            [
                "incident.medical.busdriver_unconscious",
                "incident.fire.kitchen_apartment",
                "incident.police.domestic_violence"
            ],
            RecommendedRoles:
            [
                "role.operator",
                "role.dispatcher",
                "role.coordinator",
                "role.crisis_officer"
            ]);
    }

    public static SessionSnapshotDto CreateSession(string sessionId, string preferredRole)
    {
        return new SessionSnapshotDto(
            SessionId: sessionId,
            SessionCode: "QP112",
            State: SessionState.Active.ToString(),
            Roles:
            [
                new RoleSlotDto("Operator", preferredRole == "role.operator", preferredRole != "role.operator", preferredRole == "role.operator" ? "player.local" : null),
                new RoleSlotDto("Dispatcher", preferredRole == "role.dispatcher", preferredRole != "role.dispatcher", preferredRole == "role.dispatcher" ? "player.local" : null),
                new RoleSlotDto("Coordinator", preferredRole == "role.coordinator", preferredRole != "role.coordinator", preferredRole == "role.coordinator" ? "player.local" : null),
                new RoleSlotDto("CrisisOfficer", preferredRole == "role.crisis_officer", preferredRole != "role.crisis_officer", preferredRole == "role.crisis_officer" ? "player.local" : null)
            ],
            Incidents:
            [
                new IncidentDto("INC-VS-001", "Bus driver unconscious", "Medical", "Critical", "Transit", "Queued"),
                new IncidentDto("INC-VS-002", "Kitchen fire in apartment", "Fire", "Medium", "HousingA", "Queued"),
                new IncidentDto("INC-VS-003", "Domestic violence call", "Police", "High", "HousingB", "Queued")
            ],
            Units:
            [
                new DispatchUnitDto("AMB-01", "Ambulance", "Available", "Hospital1"),
                new DispatchUnitDto("AMB-02", "AdvancedAmbulance", "Available", "Hospital1"),
                new DispatchUnitDto("FIRE-01", "FireTruck", "Available", "NorthStation"),
                new DispatchUnitDto("POL-01", "Police", "Available", "Central")
            ],
            Alerts:
            [
                new HudAlertDto("ALT-V5-01", "Quick Play session started", "Info"),
                new HudAlertDto("ALT-V5-02", "Missing roles auto-filled by AI", "Info")
            ]);
    }

    public static SessionReportDto CreateReport(string sessionId)
    {
        return new SessionReportDto(
            SessionId: sessionId,
            Grade: "A",
            PressurePeak: 61,
            Metrics:
            [
                new SessionMetricDto("metric.response.time", "Pierwsza reakcja", "00:18", "Up"),
                new SessionMetricDto("metric.dispatch.time", "Dispatch", "00:24", "Stable"),
                new SessionMetricDto("metric.incidents.resolved", "Rozwiązane incydenty", "3", "Up"),
                new SessionMetricDto("metric.incidents.failed", "Porażki", "0", "Up"),
                new SessionMetricDto("metric.incidents.escalated", "Eskalacje", "1", "Down"),
                new SessionMetricDto("metric.false.dispatch", "Błędne wysłania", "0", "Up"),
                new SessionMetricDto("metric.bot.takeovers", "Przejęcia BOT", "2", "Stable"),
                new SessionMetricDto("metric.avg.pressure", "Średnie przeciążenie", "43", "Stable")
            ],
            BestMoment: "Szybkie rozdzielenie karetki i straży w pierwszych 30 sekundach.",
            BiggestRisk: "Późna reakcja na eskalację incydentu policyjnego.",
            BotTakeovers: 2);
    }
}
