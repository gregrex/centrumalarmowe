using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class ReleaseCandidateService : IReleaseCandidateService
{
    public Task<ReleaseCandidatePackageDto> GetReleaseCandidatePackageAsync(CancellationToken cancellationToken)
    {
        var payload = new ReleaseCandidatePackageDto(
            "Alarm112-RC-001",
            "0.1.0-rc1",
            "showcase.mission.01",
            new[] { "internal_review", "android_rc" },
            new[] { "flow_complete", "bugbash_ready", "media_pack_ready", "audio_lock" },
            new[] { "Alarm112-RC.apk", "Alarm112-RC.aab", "release_notes_draft.md", "promo_media_pack.zip" });
        return Task.FromResult(payload);
    }

    public Task<IReadOnlyList<BugBashChecklistItemDto>> GetBugBashChecklistAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<BugBashChecklistItemDto> payload = new[]
        {
            new BugBashChecklistItemDto("bb.home.cta", "Home CTA działa", "P1", true, true),
            new BugBashChecklistItemDto("bb.onboarding.flow", "Onboarding nie blokuje startu misji", "P1", true, true),
            new BugBashChecklistItemDto("bb.runtime.hud", "Runtime HUD jest czytelny", "P1", true, true),
            new BugBashChecklistItemDto("bb.report.room", "Report room ma poprawne CTA", "P1", true, true),
            new BugBashChecklistItemDto("bb.audio.lock", "Audio lock potwierdzony", "P2", false, true)
        };
        return Task.FromResult(payload);
    }

    public Task<ReleaseNotesDraftDto> GetReleaseNotesDraftAsync(CancellationToken cancellationToken)
    {
        var sections = new[]
        {
            new ReleaseNotesSectionDto("highlights", "Highlights", new[] { "Showcase mission RC", "Onboarding + hint system", "Report room polish" }),
            new ReleaseNotesSectionDto("fixes", "Fixes", new[] { "Retry flow stabilized", "HUD readability improved", "Audio transitions aligned" }),
            new ReleaseNotesSectionDto("known_issues", "Known Issues", new[] { "Non-showcase content still contains placeholders" })
        };
        var payload = new ReleaseNotesDraftDto("Alarm112 RC1 Release Notes", sections, "Alarm112-RC-001");
        return Task.FromResult(payload);
    }

    public Task<AndroidRcPipelineDto> GetAndroidRcPipelineAsync(CancellationToken cancellationToken)
    {
        var payload = new AndroidRcPipelineDto(
            "android.rc.mock",
            new[] { "restore", "build_client", "package_apk", "package_aab", "smoke", "handoff" },
            new[] { "Alarm112-RC.apk", "Alarm112-RC.aab", "build_manifest.json" },
            "mock_keystore",
            "artifacts/android_rc");
        return Task.FromResult(payload);
    }

    public Task<FinalPromoMediaDto> GetFinalPromoMediaAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<StoreShotMockDto> shots = new[]
        {
            new StoreShotMockDto("promo.home.hero", "Home Hero", "9:16", "night_console", new[] { "branding", "hero", "menu" }),
            new StoreShotMockDto("promo.runtime.route", "Runtime Route", "9:16", "route_alert", new[] { "runtime", "route", "dispatch" }),
            new StoreShotMockDto("promo.report.success", "Report Success", "16:9", "report_room_success", new[] { "report", "reward", "next" })
        };
        var payload = new FinalPromoMediaDto(shots, true, new[] { "store", "preview", "capture" });
        return Task.FromResult(payload);
    }
}
