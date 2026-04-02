using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class QuasiProductionDemoService : IQuasiProductionDemoService
{
    public Task<VisualRuntimeRouteLayerDto> GetVisualRuntimeRouteLayerAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.demo.17" : missionId;
        var payload = new VisualRuntimeRouteLayerDto(
            resolvedMissionId,
            new[]
            {
                new LiveRouteSegmentStateDto("seg.ems.01", "hub.ems", "bridge.accident", "clear", 36, "pulse_blue"),
                new LiveRouteSegmentStateDto("seg.fire.01", "hub.fire", "bridge.smoke", "blocked", 74, "pulse_red"),
                new LiveRouteSegmentStateDto("seg.police.01", "hub.police", "bridge.traffic", "rerouted", 58, "pulse_amber")
            },
            "seg.fire.01",
            true,
            true);

        return Task.FromResult(payload);
    }

    public Task<IReadOnlyList<RecoveryDecisionCardDto>> GetRecoveryDecisionCardsAsync(string? missionId, CancellationToken cancellationToken)
    {
        IReadOnlyList<RecoveryDecisionCardDto> payload = new[]
        {
            new RecoveryDecisionCardDto(
                "recovery.card.traffic_reroute",
                "chain.blocked_fire_lane",
                "Reroute traffic corridor",
                "high",
                new[]
                {
                    new RecoveryDecisionCardOptionDto("opt.open_temp_lane", "Open temporary lane", "reduce_eta_fire", "medium"),
                    new RecoveryDecisionCardOptionDto("opt.reassign_police", "Reassign police escort", "stabilize_route", "low")
                }),
            new RecoveryDecisionCardDto(
                "recovery.card.ems_reinforce",
                "objective.patient_at_risk",
                "Reinforce EMS coverage",
                "critical",
                new[]
                {
                    new RecoveryDecisionCardOptionDto("opt.call_mutual_aid", "Call mutual aid", "add_unit", "resource_cost"),
                    new RecoveryDecisionCardOptionDto("opt.delay_minor_case", "Delay minor case", "free_unit", "public_frustration")
                })
        };

        return Task.FromResult(payload);
    }

    public Task<IReadOnlyList<MissionFailBranchDto>> GetMissionFailBranchesAsync(string? missionId, CancellationToken cancellationToken)
    {
        IReadOnlyList<MissionFailBranchDto> payload = new[]
        {
            new MissionFailBranchDto("fail.primary_objective", "objective_critical_fail", "Primary objective failed", "Prioritize EMS route stabilization earlier."),
            new MissionFailBranchDto("fail.city_pressure", "system_overload", "City pressure collapsed", "Use recovery cards before the second escalation peak.")
        };

        return Task.FromResult(payload);
    }

    public Task<ReportRoomPolishDto> GetReportRoomPolishAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.demo.17" : missionId;
        var payload = new ReportRoomPolishDto(
            resolvedMissionId,
            new[]
            {
                new ReportRoomVariantDto("report.success", "success", "report_room.clean_night", "badge_responder_bronze", "debrief_resolve"),
                new ReportRoomVariantDto("report.partial", "partial", "report_room.neutral_night", "token_ops_small", "debrief_neutral"),
                new ReportRoomVariantDto("report.fail", "fail", "report_room.dim_alert", "none", "debrief_fail")
            });

        return Task.FromResult(payload);
    }

    public Task<QuasiProductionDemoFlowDto> GetQuasiProductionDemoFlowAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.demo.17" : missionId;
        var payload = new QuasiProductionDemoFlowDto(
            resolvedMissionId,
            new[]
            {
                new QuasiProductionDemoFlowStepDto("flow.01", "home", "Home"),
                new QuasiProductionDemoFlowStepDto("flow.02", "chapter_map", "Chapter Map"),
                new QuasiProductionDemoFlowStepDto("flow.03", "briefing", "Mission Briefing"),
                new QuasiProductionDemoFlowStepDto("flow.04", "readiness", "Ready Check"),
                new QuasiProductionDemoFlowStepDto("flow.05", "runtime", "Runtime Start"),
                new QuasiProductionDemoFlowStepDto("flow.06", "recovery", "Recovery Card"),
                new QuasiProductionDemoFlowStepDto("flow.07", "resolve_or_fail", "Resolve or Fail"),
                new QuasiProductionDemoFlowStepDto("flow.08", "report_room", "Report Room"),
                new QuasiProductionDemoFlowStepDto("flow.09", "post_choice", "Retry or Next")
            });

        return Task.FromResult(payload);
    }
}
