using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class RuntimeUiFlowService : IRuntimeUiFlowService
{
    public Task<RuntimeHudDto> GetRuntimeHudAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.demo.18" : missionId;
        var payload = new RuntimeHudDto(
            resolvedMissionId,
            new[]
            {
                new RuntimeHudPanelDto("panel.timer", "timer", "critical", "top", true),
                new RuntimeHudPanelDto("panel.incidents", "incident_list", "high", "left", true),
                new RuntimeHudPanelDto("panel.objectives", "objective_tracker", "high", "right", true),
                new RuntimeHudPanelDto("panel.recovery", "recovery_slot", "critical", "right", true),
                new RuntimeHudPanelDto("panel.actions", "dispatch_actions", "high", "bottom", true)
            },
            new[] { "banner.city_pressure", "banner.route_blocked" },
            "recovery.slot.primary");

        return Task.FromResult(payload);
    }

    public Task<IReadOnlyList<RecoveryHudTriggerDto>> GetRecoveryHudTriggersAsync(string? missionId, CancellationToken cancellationToken)
    {
        IReadOnlyList<RecoveryHudTriggerDto> payload = new[]
        {
            new RecoveryHudTriggerDto("trigger.route_blocked", "route.blocked", "critical", "dispatcher", true),
            new RecoveryHudTriggerDto("trigger.unit_delayed", "unit.delayed", "high", "coordinator", false),
            new RecoveryHudTriggerDto("trigger.objective_risk", "objective.at_risk", "critical", "operator", true)
        };

        return Task.FromResult(payload);
    }

    public Task<FailRetryNextFlowDto> GetFailRetryNextFlowAsync(string? missionId, string? resultState, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.demo.18" : missionId;
        var resolvedResultState = string.IsNullOrWhiteSpace(resultState) ? "partial" : resultState;

        var steps = resolvedResultState switch
        {
            "success" => new[] { "report.success", "reward.reveal", "mission.next_gate" },
            "fail" => new[] { "report.fail", "retry.hints", "choice.retry_or_home" },
            _ => new[] { "report.partial", "choice.retry_or_next" }
        };

        var payload = new FailRetryNextFlowDto(
            resolvedMissionId,
            resolvedResultState,
            steps,
            new[] { "Improve route stabilization earlier.", "Use recovery cards before timer threshold.", "Keep one backup EMS unit free." });

        return Task.FromResult(payload);
    }

    public Task<MissionSlicePolishDto> GetMissionSlicePolishAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.demo.18" : missionId;
        var payload = new MissionSlicePolishDto(
            resolvedMissionId,
            new[] { "runtime.day.alert", "fail.dim_room", "retry.prep_room", "report.success_warm" },
            new[] { "ems_van_hero", "route_blocked_cluster", "recovery_card_frame", "retry_gate_panel" },
            new[] { "runtime_peak", "recovery_focus", "retry_rebuild", "report_release" });

        return Task.FromResult(payload);
    }
}