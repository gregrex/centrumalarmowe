using Alarm112.Application.Interfaces;
using Microsoft.AspNetCore.OutputCaching;

namespace Alarm112.Api.Endpoints;

public static class ContentEndpoints
{
    public static WebApplication MapContentEndpoints(this WebApplication app)
    {
        // Reference data is static — cache for 5 minutes
        app.MapGet("/api/reference-data",
            async (IReferenceDataService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetReferenceDataAsync(ct)))
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(5)));

        app.MapGet("/api/theme-pack",
            async (IThemePackService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetThemePackAsync(ct)))
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(5)));

        app.MapGet("/api/menu-flow",
            async (IThemePackService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetMenuFlowAsync(ct)))
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(5)));

        app.MapGet("/api/meta-progression/demo",
            async (IThemePackService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetDemoMetaProgressionAsync(ct)));

        app.MapGet("/api/city-map",
            async (ICityMapService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetCityMapAsync(ct)))
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(1)));

        app.MapGet("/api/map-filters",
            async (IOperationsBoardService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetMapFiltersAsync(ct)))
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(5)));

        // Content validation is heavier — no cache so it always reflects current disk state
        app.MapGet("/api/content/validate",
            async (IContentValidationService svc, CancellationToken ct) =>
                Results.Ok(await svc.ValidateAsync(ct)));

        return app;
    }
}
