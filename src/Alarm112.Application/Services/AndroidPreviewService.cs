using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class AndroidPreviewService : IAndroidPreviewService
{
    public Task<AndroidPreviewBuildDto> GetAndroidPreviewBuildAsync(CancellationToken cancellationToken)
    {
        var payload = new AndroidPreviewBuildDto(
            "Alarm112-Preview-001",
            "0.1.0-preview1",
            "showcase.mission.01",
            "ready_for_internal_preview",
            new[] { "Alarm112-Preview.apk", "Alarm112-Preview.aab", "preview_manifest.json" },
            new[] { "android_mid_720p", "android_hd_1080p", "android_tablet_debug" });
        return Task.FromResult(payload);
    }

    public Task<IReadOnlyList<ReleaseReadinessChecklistItemDto>> GetReleaseReadinessChecklistAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ReleaseReadinessChecklistItemDto> payload = new[]
        {
            new ReleaseReadinessChecklistItemDto("rr.preview.smoke", "Smoke preview build", "done", "P1"),
            new ReleaseReadinessChecklistItemDto("rr.capture.pack", "Capture pack complete", "in_progress", "P2"),
            new ReleaseReadinessChecklistItemDto("rr.telemetry.mock", "Telemetry dashboard ready", "done", "P2"),
            new ReleaseReadinessChecklistItemDto("rr.blockers.none", "No blocker bugs open", "pending", "P1")
        };
        return Task.FromResult(payload);
    }

    public Task<TelemetryDashboardDto> GetTelemetryDashboardAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<TelemetryMetricDto> kpis = new[]
        {
            new TelemetryMetricDto("mission_complete_rate", "Mission complete rate", 0.84, "up"),
            new TelemetryMetricDto("retry_rate", "Retry rate", 0.26, "down"),
            new TelemetryMetricDto("avg_route_confirm_sec", "Avg route confirm sec", 8.4, "flat"),
            new TelemetryMetricDto("hint_trigger_density", "Hint trigger density", 0.42, "down")
        };
        IReadOnlyList<TelemetryHeatmapPointDto> heatmap = new[]
        {
            new TelemetryHeatmapPointDto("north", "medium"),
            new TelemetryHeatmapPointDto("downtown", "high")
        };
        var payload = new TelemetryDashboardDto("showcase.mission.01", kpis, heatmap, "demo_mock");
        return Task.FromResult(payload);
    }

    public Task<ReviewFeedbackDashboardDto> GetReviewFeedbackDashboardAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ReviewFeedbackItemDto> items = new[]
        {
            new ReviewFeedbackItemDto("QA-1", "P1", "runtime_hud", "route overlay still busy on 720p"),
            new ReviewFeedbackItemDto("Design-1", "P2", "report_room", "reward reveal needs slower pacing"),
            new ReviewFeedbackItemDto("Audio-1", "P2", "mix", "city pressure sting slightly loud")
        };
        var summary = new ReviewFeedbackSummaryDto(1, 1, 2, 0);
        var payload = new ReviewFeedbackDashboardDto(items, summary);
        return Task.FromResult(payload);
    }

    public Task<FinalCapturePackDto> GetFinalCapturePackAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<CaptureShotDto> shots = new[]
        {
            new CaptureShotDto("cap.home.hero", "store_shot", "9:16", "menu_night_console"),
            new CaptureShotDto("cap.briefing", "demo_shot", "16:9", "briefing_room"),
            new CaptureShotDto("cap.runtime.route", "trailer_shot", "9:16", "city_route_overlay"),
            new CaptureShotDto("cap.report.success", "store_shot", "9:16", "report_room_success")
        };
        var payload = new FinalCapturePackDto(shots, "preview_capture_lock", "mobile_portrait_safe");
        return Task.FromResult(payload);
    }
}
