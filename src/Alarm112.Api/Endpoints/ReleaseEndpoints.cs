using Alarm112.Application.Interfaces;

namespace Alarm112.Api.Endpoints;

public static class ReleaseEndpoints
{
    public static WebApplication MapReleaseEndpoints(this WebApplication app)
    {
        // Review build
        app.MapGet("/api/review-build-package/demo",
            async (IReviewBuildService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetReviewBuildPackageAsync(ct)));
        app.MapGet("/api/test-build-deploy/demo",
            async (IReviewBuildService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetTestBuildDeployAsync(ct)));
        app.MapGet("/api/store-shot-mocks/demo",
            async (IReviewBuildService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetStoreShotMocksAsync(ct)));
        app.MapGet("/api/playtest-feedback-form/demo",
            async (IReviewBuildService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetPlaytestFeedbackFormAsync(ct)));
        app.MapGet("/api/review-build-checklist/demo",
            async (IReviewBuildService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetReviewBuildChecklistAsync(ct)));

        // Release candidate
        app.MapGet("/api/release-candidate-package/demo",
            async (IReleaseCandidateService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetReleaseCandidatePackageAsync(ct)));
        app.MapGet("/api/bug-bash-checklist/demo",
            async (IReleaseCandidateService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetBugBashChecklistAsync(ct)));
        app.MapGet("/api/release-notes-draft/demo",
            async (IReleaseCandidateService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetReleaseNotesDraftAsync(ct)));
        app.MapGet("/api/android-rc-pipeline/demo",
            async (IReleaseCandidateService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetAndroidRcPipelineAsync(ct)));
        app.MapGet("/api/final-promo-media/demo",
            async (IReleaseCandidateService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetFinalPromoMediaAsync(ct)));

        // Android preview
        app.MapGet("/api/android-preview-build/demo",
            async (IAndroidPreviewService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetAndroidPreviewBuildAsync(ct)));
        app.MapGet("/api/release-readiness-checklist/demo",
            async (IAndroidPreviewService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetReleaseReadinessChecklistAsync(ct)));
        app.MapGet("/api/telemetry-dashboard/demo",
            async (IAndroidPreviewService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetTelemetryDashboardAsync(ct)));
        app.MapGet("/api/review-feedback-dashboard/demo",
            async (IAndroidPreviewService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetReviewFeedbackDashboardAsync(ct)));
        app.MapGet("/api/final-capture-pack/demo",
            async (IAndroidPreviewService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetFinalCapturePackAsync(ct)));

        // Internal test
        app.MapGet("/api/internal-test-pack/demo",
            async (IInternalTestService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetInternalTestPackAsync(ct)));
        app.MapGet("/api/google-play-internal-testing/demo",
            async (IInternalTestService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetGooglePlayInternalTestingAsync(ct)));
        app.MapGet("/api/liveops-review-panel/demo",
            async (IInternalTestService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetLiveopsReviewPanelAsync(ct)));
        app.MapGet("/api/final-trailer-store-demo/demo",
            async (IInternalTestService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetFinalTrailerStoreDemoAsync(ct)));
        app.MapGet("/api/release-readiness-v24/demo",
            async (IInternalTestService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetReleaseReadinessV24Async(ct)));

        // Final handoff
        app.MapGet("/api/final-handoff-pack/demo",
            async (IFinalHandoffService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetFinalHandoffPackAsync(ct)));
        app.MapGet("/api/store-compliance/demo",
            async (IFinalHandoffService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetStoreComplianceAsync(ct)));
        app.MapGet("/api/internal-demo-package/demo",
            async (IFinalHandoffService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetInternalDemoPackageAsync(ct)));
        app.MapGet("/api/playtest-release-liveops-loop/demo",
            async (IFinalHandoffService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetPlaytestReleaseLiveopsLoopAsync(ct)));
        app.MapGet("/api/release-readiness-v25/demo",
            async (IFinalHandoffService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetReleaseReadinessV25Async(ct)));

        // Real Android build
        app.MapGet("/api/real-android-build/demo",
            async (IRealAndroidBuildService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetRealAndroidBuildAsync(ct)));
        app.MapGet("/api/bugfix-freeze/demo",
            async (IRealAndroidBuildService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetBugfixFreezeAsync(ct)));
        app.MapGet("/api/operator-dispatcher-showcase/demo",
            async (IRealAndroidBuildService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetOperatorDispatcherShowcaseAsync(ct)));
        app.MapGet("/api/final-polish-pack/demo",
            async (IRealAndroidBuildService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetFinalPolishPackAsync(ct)));
        app.MapGet("/api/release-feedback-loop-v2/demo",
            async (IRealAndroidBuildService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetReleaseFeedbackLoopV2Async(ct)));

        return app;
    }
}
