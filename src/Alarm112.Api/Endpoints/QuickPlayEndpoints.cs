using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Api.Endpoints;

public static class QuickPlayEndpoints
{
    public static WebApplication MapQuickPlayEndpoints(this WebApplication app)
    {
        app.MapGet("/api/quickplay/bootstrap",
            async (IQuickPlayService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetBootstrapAsync(ct)));

        app.MapPost("/api/quickplay/start",
            async (QuickPlayStartRequestDto request, IQuickPlayService svc, CancellationToken ct) =>
            {
                var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(request);
                if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(request, ctx, validationResults, true))
                {
                    var errors = validationResults
                        .GroupBy(v => v.MemberNames.FirstOrDefault() ?? "general")
                        .ToDictionary(g => g.Key, g => g.Select(v => v.ErrorMessage ?? "Invalid value.").ToArray());
                    return Results.ValidationProblem(errors);
                }
                return Results.Ok(await svc.StartAsync(request, ct));
            }).RequireRateLimiting("fixed");

        app.MapPost("/api/lobbies/demo",
            async (ILobbyService svc, CancellationToken ct) =>
                Results.Ok(await svc.CreateDemoLobbyAsync(ct))).RequireRateLimiting("fixed");

        app.MapGet("/api/lobbies/{lobbyId}",
            async (string lobbyId, ILobbyService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetLobbyAsync(lobbyId, ct)));

        return app;
    }
}
