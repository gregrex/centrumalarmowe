using Alarm112.Application.Interfaces;

namespace Alarm112.Api.Endpoints;

public static class MissionEndpoints
{
    public static WebApplication MapMissionEndpoints(this WebApplication app)
    {
        // Bootstrap / entry
        app.MapGet("/api/chapter-runtime/demo",
            async (IRuntimeBootstrapService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetChapterRuntimeAsync(ct)));

        app.MapGet("/api/role-selection/demo",
            async (IRuntimeBootstrapService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetRoleSelectionPreviewAsync(ct)));

        app.MapGet("/api/round-bootstrap/demo",
            async (string? missionId, string? mode, string? role, IRuntimeBootstrapService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetRoundBootstrapAsync(missionId, mode, role, ct)));

        app.MapGet("/api/objectives-grading/demo",
            async (IRuntimeBootstrapService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetObjectiveGradesAsync(ct)));

        app.MapGet("/api/final-vertical-slice/scenes",
            async (IRuntimeBootstrapService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetFinalVerticalSliceScenesAsync(ct)));

        // Mission flow
        app.MapGet("/api/mission-briefing/demo",
            async (string? missionId, IMissionFlowService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetMissionBriefingAsync(missionId, ct)));

        app.MapGet("/api/team-readiness/demo",
            async (string? missionId, IMissionFlowService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetTeamReadinessAsync(missionId, ct)));

        app.MapGet("/api/postround-report/demo",
            async (string? missionId, IMissionFlowService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetPostRoundReportAsync(missionId, ct)));

        app.MapGet("/api/mission-complete-flow/demo",
            async (string? missionId, IMissionFlowService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetMissionCompleteFlowAsync(missionId, ct)));

        // Mission runtime
        app.MapGet("/api/mission-runtime/demo",
            async (string? missionId, IMissionRuntimeService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetMissionRuntimeAsync(missionId, ct)));

        app.MapGet("/api/mission-complete-gate/demo",
            async (string? missionId, IMissionRuntimeService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetMissionCompleteGateAsync(missionId, ct)));

        app.MapGet("/api/runtime-dispatch-outcomes/demo",
            async (string? missionId, IMissionRuntimeService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetDispatchOutcomesAsync(missionId, ct)));

        app.MapGet("/api/objective-tracker/demo",
            async (string? missionId, IMissionRuntimeService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetObjectiveTrackerAsync(missionId, ct)));

        app.MapGet("/api/mission-script/demo",
            async (string? missionId, IMissionRuntimeService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetMissionScriptAsync(missionId, ct)));

        return app;
    }
}
