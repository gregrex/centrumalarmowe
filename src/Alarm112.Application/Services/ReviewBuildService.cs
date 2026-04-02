using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class ReviewBuildService : IReviewBuildService
{
    public Task<ReviewBuildPackageDto> GetReviewBuildPackageAsync(CancellationToken cancellationToken)
    {
        var checklist = GetChecklist();
        var payload = new ReviewBuildPackageDto(
            "review-build.v21",
            "Alarm112 Showcase Review Build",
            "showcase.mission.01",
            true,
            checklist,
            new[] { "local", "device_internal" });
        return Task.FromResult(payload);
    }

    public Task<TestBuildDeployDto> GetTestBuildDeployAsync(CancellationToken cancellationToken)
    {
        var payload = new TestBuildDeployDto(
            "review-build.v21",
            "review",
            "internal-review",
            "http://localhost:8080",
            new[] { "health", "showcase_seed", "capture_mode", "review_checklist" },
            true);
        return Task.FromResult(payload);
    }

    public Task<IReadOnlyList<StoreShotMockDto>> GetStoreShotMocksAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<StoreShotMockDto> payload = new[]
        {
            new StoreShotMockDto("shot.home.hero", "Home Hero", "portrait_store", "night_rain", new[] { "home", "hero_console", "branding" }),
            new StoreShotMockDto("shot.runtime.action", "Runtime Dispatch", "9:16", "runtime_alert", new[] { "route", "incident", "dispatch" }),
            new StoreShotMockDto("shot.report.reward", "Report Reward", "16:9", "report_room_success", new[] { "reward", "summary", "next" })
        };
        return Task.FromResult(payload);
    }

    public Task<PlaytestFeedbackFormDto> GetPlaytestFeedbackFormAsync(CancellationToken cancellationToken)
    {
        var fields = new[]
        {
            new PlaytestFeedbackFieldDto("device", "Device Model", "text", true),
            new PlaytestFeedbackFieldDto("orientation", "Orientation", "select", true),
            new PlaytestFeedbackFieldDto("role", "Role Used", "select", true),
            new PlaytestFeedbackFieldDto("notes", "Notes", "multiline", true),
            new PlaytestFeedbackFieldDto("timestamp", "Timestamp or Step", "text", false)
        };
        var payload = new PlaytestFeedbackFormDto(
            "playtest.v21",
            fields,
            new[] { "readability", "audio", "retry", "bug", "confusion" },
            true);
        return Task.FromResult(payload);
    }

    public Task<IReadOnlyList<ReviewBuildChecklistItemDto>> GetReviewBuildChecklistAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ReviewBuildChecklistItemDto> payload = GetChecklist();
        return Task.FromResult(payload);
    }

    private static IReadOnlyList<ReviewBuildChecklistItemDto> GetChecklist() => new[]
    {
        new ReviewBuildChecklistItemDto("home.flow", "Home flow działa", "blocker", true, true),
        new ReviewBuildChecklistItemDto("capture.mode", "Capture mode czyści overlaye", "major", true, true),
        new ReviewBuildChecklistItemDto("runtime.showcase", "Showcase mission kończy się raportem", "blocker", true, true),
        new ReviewBuildChecklistItemDto("report.retry", "Retry i Next mają poprawne CTA", "major", true, true),
        new ReviewBuildChecklistItemDto("store.shots", "Store shot mocks gotowe", "minor", false, true)
    };
}
