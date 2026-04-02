using Alarm112.Application.Interfaces;

namespace Alarm112.Api.Endpoints;

public static class ContentEndpoints
{
    public static WebApplication MapContentEndpoints(this WebApplication app)
    {
        app.MapGet("/api/reference-data",
            async (IReferenceDataService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetReferenceDataAsync(ct)));

        app.MapGet("/api/theme-pack",
            async (IThemePackService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetThemePackAsync(ct)));

        app.MapGet("/api/menu-flow",
            async (IThemePackService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetMenuFlowAsync(ct)));

        app.MapGet("/api/meta-progression/demo",
            async (IThemePackService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetDemoMetaProgressionAsync(ct)));

        app.MapGet("/api/city-map",
            async (ICityMapService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetCityMapAsync(ct)));

        app.MapGet("/api/map-filters",
            async (IOperationsBoardService svc, CancellationToken ct) =>
                Results.Ok(await svc.GetMapFiltersAsync(ct)));

        app.MapGet("/api/content/validate",
            async (IContentValidationService svc, CancellationToken ct) =>
                Results.Ok(await svc.ValidateAsync(ct)));

        return app;
    }
}
