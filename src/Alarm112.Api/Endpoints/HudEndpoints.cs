using Alarm112.Application.Interfaces;

namespace Alarm112.Api.Endpoints;

public static class HudEndpoints
{
    public static WebApplication MapHudEndpoints(this WebApplication app)
    {
        // Playable runtime
        app.MapGet("/api/playable-runtime-map/demo",
            async (string? missionId, IPlayableRuntimeService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetPlayableRuntimeMapAsync(missionId, ct)));

        app.MapGet("/api/objective-state-machine/demo",
            async (string? missionId, IPlayableRuntimeService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetObjectiveStateMachineAsync(missionId, ct)));

        app.MapGet("/api/dispatcher-loop/demo",
            async (string? missionId, IPlayableRuntimeService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetDispatcherLoopAsync(missionId, ct)));

        app.MapGet("/api/city-pressure-runtime/demo",
            async (string? missionId, IPlayableRuntimeService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetCityPressureRuntimeAsync(missionId, ct)));

        app.MapGet("/api/report-progression/demo",
            async (string? missionId, IPlayableRuntimeService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetReportProgressionAsync(missionId, ct)));

        // Runtime polish
        app.MapGet("/api/live-route-runtime/demo",
            async (string? missionId, IRuntimePolishService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetLiveRouteRuntimeAsync(missionId, ct)));

        app.MapGet("/api/round-timer/demo",
            async (string? missionId, IRuntimePolishService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetRoundTimerAsync(missionId, ct)));

        app.MapGet("/api/chain-escalation-runtime/demo",
            async (string? missionId, IRuntimePolishService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetChainEscalationRuntimeAsync(missionId, ct)));

        app.MapGet("/api/demo-mission-polish/demo",
            async (string? missionId, IRuntimePolishService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetDemoMissionPolishAsync(missionId, ct)));

        // HUD UI flow
        app.MapGet("/api/runtime-hud/demo",
            async (string? missionId, IRuntimeUiFlowService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetRuntimeHudAsync(missionId, ct)));

        app.MapGet("/api/recovery-hud-triggers/demo",
            async (string? missionId, IRuntimeUiFlowService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetRecoveryHudTriggersAsync(missionId, ct)));

        app.MapGet("/api/fail-retry-next/demo",
            async (string? missionId, string? resultState, IRuntimeUiFlowService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetFailRetryNextFlowAsync(missionId, resultState, ct)));

        app.MapGet("/api/mission-slice-polish/demo",
            async (string? missionId, IRuntimeUiFlowService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetMissionSlicePolishAsync(missionId, ct)));

        // Near-final slice
        app.MapGet("/api/runtime-scoreboard/demo",
            async (string? missionId, INearFinalSliceService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetRuntimeScoreboardAsync(missionId, ct)));

        app.MapGet("/api/reward-reveal/demo",
            async (string? missionId, INearFinalSliceService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetRewardRevealStatesAsync(missionId, ct)));

        app.MapGet("/api/retry-preparation/demo",
            async (string? missionId, INearFinalSliceService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetRetryPreparationAsync(missionId, ct)));

        app.MapGet("/api/next-mission-handoff/demo",
            async (string? missionId, INearFinalSliceService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetNextMissionHandoffAsync(missionId, ct)));

        app.MapGet("/api/near-final-slice-flow/demo",
            async (INearFinalSliceService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetNearFinalSliceFlowAsync(ct)));

        return app;
    }
}
