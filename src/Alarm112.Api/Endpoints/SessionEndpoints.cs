using Alarm112.Application.Interfaces;
using Alarm112.Api.Hubs;
using Alarm112.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace Alarm112.Api.Endpoints;

public static class SessionEndpoints
{
    private static readonly System.Text.RegularExpressions.Regex SessionIdRegex =
        new(@"^[a-zA-Z0-9\-_]{1,128}$", System.Text.RegularExpressions.RegexOptions.Compiled);

    private static bool IsValidSessionId(string id) => SessionIdRegex.IsMatch(id);

    public static WebApplication MapSessionEndpoints(this WebApplication app)
    {
        app.MapPost("/api/sessions/demo",
            async (ISessionService sessionService, CancellationToken cancellationToken) =>
            {
                var snapshot = await sessionService.CreateDemoSessionAsync(cancellationToken);
                return Results.Ok(snapshot);
            }).RequireRateLimiting("fixed");

        app.MapGet("/api/sessions/{sessionId}",
            async (string sessionId, ISessionService sessionService, CancellationToken cancellationToken) =>
            {
                if (!IsValidSessionId(sessionId))
                    return Results.BadRequest(new { error = "Invalid sessionId format." });
                var snapshot = await sessionService.GetSnapshotAsync(sessionId, cancellationToken);
                return Results.Ok(snapshot);
            });

        app.MapPost("/api/sessions/{sessionId}/actions",
            async (string sessionId, SessionActionDto? action, ISessionService sessionService,
                   IHubContext<SessionHub> hub, CancellationToken cancellationToken) =>
            {
                if (action is null)
                    return Results.BadRequest(new { error = "Request body is required." });
                if (!IsValidSessionId(sessionId))
                    return Results.BadRequest(new { error = "Invalid sessionId format." });

                var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(action);
                if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(action, ctx, validationResults, true))
                {
                    var errors = validationResults
                        .GroupBy(v => v.MemberNames.FirstOrDefault() ?? "general")
                        .ToDictionary(g => g.Key, g => g.Select(v => v.ErrorMessage ?? "Invalid value.").ToArray());
                    return Results.ValidationProblem(errors);
                }

                var result = await sessionService.ApplyActionAsync(sessionId, action, cancellationToken);
                var envelope = new RealtimeEnvelopeDto(
                    Guid.NewGuid().ToString("N"), sessionId, "session.snapshot.delta",
                    DateTimeOffset.UtcNow, action.Role, action.ActionType, "1.0",
                    new { sessionId, action = action.ActionType });
                await hub.Clients.Group(sessionId).SendAsync("session.envelope", envelope, cancellationToken);
                return Results.Ok(result);
            }).RequireRateLimiting("fixed");

        app.MapGet("/api/sessions/{sessionId}/timeline",
            async (string sessionId, ICityMapService cityMapService, CancellationToken cancellationToken) =>
            {
                var payload = await cityMapService.GetTimelineAsync(sessionId, cancellationToken);
                return Results.Ok(payload);
            });

        app.MapPost("/api/sessions/{sessionId}/dispatch",
            async (string sessionId, DispatchCommandDto command, ICityMapService cityMapService,
                   IHubContext<SessionHub> hub, CancellationToken cancellationToken) =>
            {
                if (!IsValidSessionId(sessionId))
                    return Results.BadRequest(new { error = "Invalid sessionId format." });

                var payload = await cityMapService.DispatchAsync(sessionId, command, cancellationToken);
                var envelope = new RealtimeEnvelopeDto(
                    Guid.NewGuid().ToString("N"), sessionId, "session.dispatch.result",
                    DateTimeOffset.UtcNow, command.ActorRole, command.ActionId, "1.0",
                    new { sessionId, command.IncidentId, command.UnitId, payload.ResultCode });
                await hub.Clients.Group(sessionId).SendAsync("session.envelope", envelope, cancellationToken);
                return Results.Ok(payload);
            }).RequireRateLimiting("fixed");

        app.MapGet("/api/sessions/{sessionId}/active-incidents",
            async (string sessionId, IOperationsBoardService ops, CancellationToken cancellationToken) =>
            {
                var payload = await ops.GetActiveIncidentsAsync(sessionId, cancellationToken);
                return Results.Ok(payload);
            });

        app.MapPost("/api/sessions/{sessionId}/route-preview",
            async (string sessionId, RoutePreviewRequestDto request,
                   IOperationsBoardService ops, CancellationToken cancellationToken) =>
            {
                var payload = await ops.PreviewRouteAsync(sessionId, request, cancellationToken);
                return Results.Ok(payload);
            }).RequireRateLimiting("fixed");

        app.MapPost("/api/sessions/{sessionId}/shared-actions",
            async (string sessionId, SharedActionDto action, IOperationsBoardService ops,
                   IHubContext<SessionHub> hub, CancellationToken cancellationToken) =>
            {
                var payload = await ops.ResolveSharedActionAsync(sessionId, action, cancellationToken);
                var envelope = new RealtimeEnvelopeDto(
                    Guid.NewGuid().ToString("N"), sessionId, "session.shared-action.resolved",
                    DateTimeOffset.UtcNow, payload.Role, action.ActionType, "1.0",
                    new { sessionId, action.SharedActionId, payload.ResultCode, payload.Accepted, payload.IsBot });
                await hub.Clients.Group(sessionId).SendAsync("session.envelope", envelope, cancellationToken);
                return Results.Ok(payload);
            }).RequireRateLimiting("fixed");

        app.MapGet("/api/sessions/{sessionId}/route-overlay",
            async (string sessionId, string incidentId, string unitId,
                   IRoundRuntimeService rrt, CancellationToken cancellationToken) =>
                Results.Ok(await rrt.GetRouteOverlayAsync(sessionId, incidentId, unitId, cancellationToken)));

        app.MapGet("/api/sessions/{sessionId}/round-state",
            async (string sessionId, IRoundRuntimeService rrt, CancellationToken cancellationToken) =>
                Results.Ok(await rrt.GetRoundStateAsync(sessionId, cancellationToken)));

        app.MapGet("/api/sessions/{sessionId}/live-deltas",
            async (string sessionId, IRoundRuntimeService rrt, CancellationToken cancellationToken) =>
                Results.Ok(await rrt.GetLiveDeltasAsync(sessionId, cancellationToken)));

        app.MapGet("/api/sessions/{sessionId}/units/runtime",
            async (string sessionId, IRoundRuntimeService rrt, CancellationToken cancellationToken) =>
                Results.Ok(await rrt.GetUnitsRuntimeAsync(sessionId, cancellationToken)));

        app.MapGet("/api/sessions/{sessionId}/report",
            async (string sessionId, IQuickPlayService qp, CancellationToken cancellationToken) =>
                Results.Ok(await qp.GetReportAsync(sessionId, cancellationToken)));

        return app;
    }
}
