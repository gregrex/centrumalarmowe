using Alarm112.Application.Interfaces;

namespace Alarm112.Api.Endpoints;

public static class ShowcaseEndpoints
{
    public static WebApplication MapShowcaseEndpoints(this WebApplication app)
    {
        app.MapGet("/api/showcase-mission/demo",
            async (IShowcaseDemoService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetShowcaseMissionAsync(ct)));

        app.MapGet("/api/onboarding-flow/demo",
            async (IShowcaseDemoService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetOnboardingFlowAsync(ct)));

        app.MapGet("/api/hint-system/demo",
            async (IShowcaseDemoService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetHintSystemAsync(ct)));

        app.MapGet("/api/demo-presentation-flow/demo",
            async (IShowcaseDemoService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetDemoPresentationFlowAsync(ct)));

        app.MapGet("/api/player-facing-polish/demo",
            async (IShowcaseDemoService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetPlayerFacingPolishAsync(ct)));

        app.MapGet("/api/demo-capture-plan/demo",
            async (IShowcaseDemoService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetDemoCapturePlanAsync(ct)));

        // Quasi-production demo
        app.MapGet("/api/visual-runtime-route-layer/demo",
            async (string? missionId, IQuasiProductionDemoService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetVisualRuntimeRouteLayerAsync(missionId, ct)));

        app.MapGet("/api/recovery-decision-cards/demo",
            async (string? missionId, IQuasiProductionDemoService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetRecoveryDecisionCardsAsync(missionId, ct)));

        app.MapGet("/api/mission-fail-branches/demo",
            async (string? missionId, IQuasiProductionDemoService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetMissionFailBranchesAsync(missionId, ct)));

        app.MapGet("/api/report-room-polish/demo",
            async (string? missionId, IQuasiProductionDemoService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetReportRoomPolishAsync(missionId, ct)));

        app.MapGet("/api/quasi-production-demo-flow/demo",
            async (string? missionId, IQuasiProductionDemoService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetQuasiProductionDemoFlowAsync(missionId, ct)));

        return app;
    }
}
