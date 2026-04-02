using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class InternalTestService : IInternalTestService
{
    public Task<InternalTestPackDto> GetInternalTestPackAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<InternalTestBuildStepDto> steps = new[]
        {
            new InternalTestBuildStepDto("it01", "Freeze showcase mission data", "done", "P1"),
            new InternalTestBuildStepDto("it02", "Run smoke and content verify", "done", "P1"),
            new InternalTestBuildStepDto("it03", "Prepare internal tester notes", "in_progress", "P2")
        };

        var payload = new InternalTestPackDto(
            "Alarm112-Internal-001",
            "showcase.mission.01",
            "ready_for_google_play_internal_testing",
            new[] { "Alarm112-internal.apk", "Alarm112-internal.aab", "release-notes.md", "known-issues.md" },
            new[] { "qa_core", "design_review", "business_demo" },
            steps);
        return Task.FromResult(payload);
    }

    public Task<GooglePlayInternalTestingDto> GetGooglePlayInternalTestingAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<GooglePlayChecklistItemDto> items = new[]
        {
            new GooglePlayChecklistItemDto("gp01", "Upload AAB to Internal track", "todo", "release_engineer"),
            new GooglePlayChecklistItemDto("gp02", "Attach release notes and tester instructions", "todo", "producer"),
            new GooglePlayChecklistItemDto("gp03", "Invite internal testers", "todo", "qa_lead")
        };
        var payload = new GooglePlayInternalTestingDto("internal", items, "Demo pack for one showcase mission.");
        return Task.FromResult(payload);
    }

    public Task<LiveopsReviewPanelDto> GetLiveopsReviewPanelAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<LiveopsReviewWidgetDto> widgets = new[]
        {
            new LiveopsReviewWidgetDto("retry_rate", "metric", "Retry rate", "24%", "flat"),
            new LiveopsReviewWidgetDto("fail_hotspot", "heatmap", "Fail hotspot", "downtown", "up"),
            new LiveopsReviewWidgetDto("hint_density", "metric", "Hint density", "0.38", "down")
        };
        var payload = new LiveopsReviewPanelDto("LiveOps Review", widgets, "draft_demo");
        return Task.FromResult(payload);
    }

    public Task<FinalTrailerStoreDemoDto> GetFinalTrailerStoreDemoAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<CaptureShotDto> shots = new[]
        {
            new CaptureShotDto("cap24.home", "store_shot", "9:16", "menu_night_console"),
            new CaptureShotDto("cap24.runtime", "trailer_shot", "16:9", "city_route_overlay"),
            new CaptureShotDto("cap24.report", "demo_shot", "9:16", "report_room_success")
        };
        var payload = new FinalTrailerStoreDemoDto(shots, "v24_final_audio_lock", "internal_test_capture_ready");
        return Task.FromResult(payload);
    }

    public Task<ReleaseReadinessV24Dto> GetReleaseReadinessV24Async(CancellationToken cancellationToken)
    {
        IReadOnlyList<ReleaseReadinessChecklistItemDto> checklist = new[]
        {
            new ReleaseReadinessChecklistItemDto("v24_rr_01", "Internal test smoke passed", "done", "P1"),
            new ReleaseReadinessChecklistItemDto("v24_rr_02", "Google Play Internal Testing checklist drafted", "done", "P1"),
            new ReleaseReadinessChecklistItemDto("v24_rr_03", "LiveOps review mock ready", "in_progress", "P2")
        };
        var payload = new ReleaseReadinessV24Dto(checklist, "internal_test_candidate", "Run first internal playtest group and collect feedback.");
        return Task.FromResult(payload);
    }
}
