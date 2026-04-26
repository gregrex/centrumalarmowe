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
            PreferredRole: "CallOperator",
            AutoFillBots: true,
            IncidentIds:
            [
                "incident.medical.busdriver_unconscious",
                "incident.fire.kitchen_apartment",
                "incident.police.domestic_violence"
            ],
            RecommendedRoles:
            [
                "CallOperator",
                "Dispatcher",
                "OperationsCoordinator",
                "CrisisOfficer"
            ]);
    }

    public static SessionSnapshotDto CreateSession(string sessionId, string preferredRole)
    {
        var normalizedPreferredRole = NormalizePreferredRole(preferredRole);

        return new SessionSnapshotDto(
            SessionId: sessionId,
            SessionCode: "QP112",
            State: SessionState.Active.ToString(),
            Roles:
            [
                new RoleSlotDto("CallOperator", normalizedPreferredRole == "CallOperator", normalizedPreferredRole != "CallOperator", normalizedPreferredRole == "CallOperator" ? "player.local" : null),
                new RoleSlotDto("Dispatcher", normalizedPreferredRole == "Dispatcher", normalizedPreferredRole != "Dispatcher", normalizedPreferredRole == "Dispatcher" ? "player.local" : null),
                new RoleSlotDto("OperationsCoordinator", normalizedPreferredRole == "OperationsCoordinator", normalizedPreferredRole != "OperationsCoordinator", normalizedPreferredRole == "OperationsCoordinator" ? "player.local" : null),
                new RoleSlotDto("CrisisOfficer", normalizedPreferredRole == "CrisisOfficer", normalizedPreferredRole != "CrisisOfficer", normalizedPreferredRole == "CrisisOfficer" ? "player.local" : null)
            ],
            Incidents:
            [
                new IncidentDto("INC-VS-001", "Bus driver unconscious", "Medical", "Critical", "Transit", "pending"),
                new IncidentDto("INC-VS-002", "Kitchen fire in apartment", "Fire", "Medium", "HousingA", "pending"),
                new IncidentDto("INC-VS-003", "Domestic violence call", "Police", "High", "HousingB", "pending")
            ],
            Units:
            [
                new DispatchUnitDto("AMB-01", "Ambulance", "available", "Hospital1"),
                new DispatchUnitDto("AMB-02", "AdvancedAmbulance", "available", "Hospital1"),
                new DispatchUnitDto("FIRE-01", "FireTruck", "available", "NorthStation"),
                new DispatchUnitDto("POL-01", "Police", "available", "Central")
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

    private static string NormalizePreferredRole(string preferredRole) =>
        preferredRole switch
        {
            "role.operator" or "CallOperator" => "CallOperator",
            "role.dispatcher" or "Dispatcher" => "Dispatcher",
            "role.coordinator" or "OperationsCoordinator" => "OperationsCoordinator",
            "role.crisis_officer" or "CrisisOfficer" => "CrisisOfficer",
            _ => "CallOperator"
        };
}
