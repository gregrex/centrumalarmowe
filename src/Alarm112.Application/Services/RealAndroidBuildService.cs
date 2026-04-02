using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class RealAndroidBuildService : IRealAndroidBuildService
{
    public Task<RealAndroidBuildDto> GetRealAndroidBuildAsync(CancellationToken cancellationToken)
    {
        var payload = new RealAndroidBuildDto(
            "android.real.v26.preview01",
            "0.1.0-preview26",
            "showcase.mission.01",
            "android_internal_preview",
            new[] { "operator_runtime", "dispatcher_runtime", "bugfix_freeze", "capture_refresh" },
            new[] { "android_mid_720p", "android_hd_1080p", "android_tablet_debug" },
            "candidate_for_real_build");
        return Task.FromResult(payload);
    }

    public Task<BugfixFreezeDto> GetBugfixFreezeAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<BugfixFreezeChecklistItemDto> checklist = new[]
        {
            new BugfixFreezeChecklistItemDto("bf26_01", "No blocker crash on showcase mission", "in_progress", "P0", "tech_lead"),
            new BugfixFreezeChecklistItemDto("bf26_02", "Operator HUD readable on 720p", "todo", "P1", "ux"),
            new BugfixFreezeChecklistItemDto("bf26_03", "Dispatcher route overlay readable on 1080p", "todo", "P1", "ux"),
            new BugfixFreezeChecklistItemDto("bf26_04", "Audio mix validated for retry flow", "todo", "P2", "audio"),
        };

        var payload = new BugfixFreezeDto(
            "v26_bugfix_freeze",
            checklist,
            new[] { "p0_bugfix", "p1_readability_fix", "audio_mix_fix", "blocking_input_fix" },
            new[] { "new_mode", "new_mission", "network_refactor", "ui_relayout_large" },
            "Finish P0/P1 items, then cut Android preview build.");
        return Task.FromResult(payload);
    }

    public Task<OperatorDispatcherShowcaseDto> GetOperatorDispatcherShowcaseAsync(CancellationToken cancellationToken)
    {
        var payload = new OperatorDispatcherShowcaseDto(
            "showcase.mission.01",
            new[] { "operator", "dispatcher" },
            new[] { "incoming_call", "priority_assignment", "dispatch", "escalation", "recovery_decision", "report" },
            new[] { "clean_success", "partial_success" },
            new[] { "timeout_fail", "wrong_unit_fail" },
            "ready_for_playtest_focus");
        return Task.FromResult(payload);
    }

    public Task<FinalPolishPackDto> GetFinalPolishPackAsync(CancellationToken cancellationToken)
    {
        var payload = new FinalPolishPackDto(
            "one_mission_showcase",
            new[] { "route_overlay", "incident_marker_contrast", "report_room_reward_reveal" },
            new[] { "assign_confirm", "critical_alert_balance", "retry_recommendation_stinger" },
            new[] { "operator_card_spacing", "dispatcher_unit_eta_badges", "touch_target_consistency" },
            "Apply polish only if it improves clarity or preview build quality.");
        return Task.FromResult(payload);
    }

    public Task<ReleaseFeedbackLoopV2Dto> GetReleaseFeedbackLoopV2Async(CancellationToken cancellationToken)
    {
        IReadOnlyList<PlaytestLoopStageDto> stages = new[]
        {
            new PlaytestLoopStageDto("pl26_01", "Preview build", "engineer", "active", "android_preview"),
            new PlaytestLoopStageDto("pl26_02", "Focused playtest", "qa", "queued", "one_mission_showcase"),
            new PlaytestLoopStageDto("pl26_03", "Feedback triage", "producer", "queued", "severity_board"),
            new PlaytestLoopStageDto("pl26_04", "Freeze patch", "tech_lead", "queued", "approved_fix_scope")
        };

        var payload = new ReleaseFeedbackLoopV2Dto(
            stages,
            new[] { "readability", "dispatch_confidence", "route_clarity", "audio_fatigue", "retry_value" },
            "preview_build",
            "Stabilize one mission showcase for first real Android preview build.");
        return Task.FromResult(payload);
    }
}
