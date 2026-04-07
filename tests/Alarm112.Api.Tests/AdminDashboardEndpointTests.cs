using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Alarm112.Api.Tests;

public sealed class AdminDashboardEndpointTests
{
    private const string ApiSigningKey = "test-key-exactly-32-chars-minimum!!";

    [Fact]
    public async Task DashboardSummary_ReturnsLiveApiData()
    {
        await using var stubApi = await StubApiHost.StartAsync(requireAuth: true);
        await using var factory = new AdminWebFactory(stubApi.BaseUrl, ApiSigningKey);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = CreateBasicAuthHeader();

        var response = await client.GetFromJsonAsync<AdminDashboardResponseDto>("/api/admin/dashboard");

        Assert.NotNull(response);
        Assert.Equal("online", response!.Api.Status);
        Assert.Equal("Alarm112.Api", response.Api.Service);
        Assert.Equal("RedisSessionStore", response.Api.Store);
        Assert.Equal("ok", response.Sessions.Status);
        Assert.Equal(7, response.Sessions.TotalCount);
        Assert.Equal("invalid", response.Content.Status);
        Assert.Equal(2, response.Content.IssueCount);
        Assert.Equal("ok", response.Incidents.Status);
        Assert.Equal(5, response.Incidents.TotalCount);
        Assert.Equal(2, response.Incidents.CriticalCount);
        Assert.Equal("ok", response.Units.Status);
        Assert.Equal(5, response.Units.TotalCount);
        Assert.Equal(3, response.Units.AvailableCount);
        Assert.Equal(1, response.Units.BotBackfillCount);
    }

    [Fact]
    public async Task DashboardSummary_WithoutApiTokenConfig_ReturnsAuthConfigurationState()
    {
        await using var stubApi = await StubApiHost.StartAsync(requireAuth: false);
        await using var factory = new AdminWebFactory(stubApi.BaseUrl, apiSigningKey: null);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = CreateBasicAuthHeader();

        var response = await client.GetFromJsonAsync<AdminDashboardResponseDto>("/api/admin/dashboard");

        Assert.NotNull(response);
        Assert.Equal("online", response!.Api.Status);
        Assert.Equal("auth-not-configured", response.Sessions.Status);
        Assert.Equal("auth-not-configured", response.Content.Status);
        Assert.Equal("auth-not-configured", response.Incidents.Status);
        Assert.Equal("auth-not-configured", response.Units.Status);
    }

    [Fact]
    public async Task RootPage_ReturnsRenderedDashboardTemplate()
    {
        await using var stubApi = await StubApiHost.StartAsync(requireAuth: false);
        await using var factory = new AdminWebFactory(stubApi.BaseUrl, apiSigningKey: ApiSigningKey);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = CreateBasicAuthHeader();

        var response = await client.GetAsync("/");
        var body = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("Alarm112", body);
        Assert.Contains(stubApi.BaseUrl, body);
        Assert.Contains("/js/admin.js", body);
    }

    private static AuthenticationHeaderValue CreateBasicAuthHeader()
    {
        var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:test-admin-pass"));
        return new AuthenticationHeaderValue("Basic", encoded);
    }

    private sealed class StubApiHost : IAsyncDisposable
    {
        private readonly WebApplication _app;

        private StubApiHost(WebApplication app, string baseUrl)
        {
            _app = app;
            BaseUrl = baseUrl;
        }

        public string BaseUrl { get; }

        public static async Task<StubApiHost> StartAsync(bool requireAuth)
        {
            var port = GetFreeTcpPort();
            var baseUrl = $"http://127.0.0.1:{port}";

            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls(baseUrl);

            var app = builder.Build();

            app.MapGet("/health", () => Results.Ok(new
            {
                ok = true,
                service = "Alarm112.Api",
                version = "v26",
                store = "RedisSessionStore"
            }));

            app.MapGet("/api/sessions", (HttpContext context) =>
            {
                if (requireAuth && !HasBearerToken(context))
                    return Results.Unauthorized();

                return Results.Ok(new
                {
                    page = 1,
                    pageSize = 100,
                    totalCount = 7,
                    totalPages = 1,
                    items = new[] { "demo-a", "demo-b" }
                });
            });

            app.MapGet("/api/sessions/{sessionId}/active-incidents", (string sessionId, HttpContext context) =>
            {
                if (requireAuth && !HasBearerToken(context))
                    return Results.Unauthorized();

                return sessionId switch
                {
                    "demo-a" => Results.Ok(new
                    {
                        sessionId,
                        activeCount = 3,
                        criticalCount = 1,
                        items = Array.Empty<object>()
                    }),
                    "demo-b" => Results.Ok(new
                    {
                        sessionId,
                        activeCount = 2,
                        criticalCount = 1,
                        items = Array.Empty<object>()
                    }),
                    _ => Results.NotFound()
                };
            });

            app.MapGet("/api/sessions/{sessionId}/units/runtime", (string sessionId, HttpContext context) =>
            {
                if (requireAuth && !HasBearerToken(context))
                    return Results.Unauthorized();

                return sessionId switch
                {
                    "demo-a" => Results.Ok(new[]
                    {
                        new { unitId = "unit-a1", callSign = "A-1", unitType = "medical", status = "available", currentNodeId = "n1", cooldownSeconds = 0, etaSeconds = 15, available = true, isBotBackfilled = false },
                        new { unitId = "unit-a2", callSign = "A-2", unitType = "fire", status = "engaged", currentNodeId = "n2", cooldownSeconds = 10, etaSeconds = 30, available = false, isBotBackfilled = false }
                    }),
                    "demo-b" => Results.Ok(new[]
                    {
                        new { unitId = "unit-b1", callSign = "B-1", unitType = "police", status = "available", currentNodeId = "n3", cooldownSeconds = 0, etaSeconds = 20, available = true, isBotBackfilled = false },
                        new { unitId = "unit-b2", callSign = "B-2", unitType = "medical", status = "bot-assigned", currentNodeId = "n4", cooldownSeconds = 5, etaSeconds = 25, available = true, isBotBackfilled = true },
                        new { unitId = "unit-b3", callSign = "B-3", unitType = "fire", status = "returning", currentNodeId = "n5", cooldownSeconds = 20, etaSeconds = 40, available = false, isBotBackfilled = false }
                    }),
                    _ => Results.NotFound()
                };
            });

            app.MapGet("/api/content/validate", (HttpContext context) =>
            {
                if (requireAuth && !HasBearerToken(context))
                    return Results.Unauthorized();

                return Results.Ok(new
                {
                    isValid = false,
                    issues = new[]
                    {
                        new { severity = "Error", source = "file-a.json", message = "Broken json." },
                        new { severity = "Error", source = "file-b.json", message = "Missing field." }
                    }
                });
            });

            await app.StartAsync();
            return new StubApiHost(app, baseUrl);
        }

        public ValueTask DisposeAsync() => _app.DisposeAsync();

        private static bool HasBearerToken(HttpContext context) =>
            context.Request.Headers.Authorization.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase);

        private static int GetFreeTcpPort()
        {
            using var listener = new TcpListener(System.Net.IPAddress.Loopback, 0);
            listener.Start();
            return ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
        }
    }

    private sealed record AdminDashboardResponseDto(
        AdminDashboardApiResponseDto Api,
        AdminDashboardSessionsResponseDto Sessions,
        AdminDashboardContentResponseDto Content,
        AdminDashboardIncidentsResponseDto Incidents,
        AdminDashboardUnitsResponseDto Units);

    private sealed record AdminDashboardApiResponseDto(
        string Status,
        string? Version,
        string? Service,
        string? Store,
        string? Error);

    private sealed record AdminDashboardSessionsResponseDto(
        string Status,
        int? TotalCount,
        string? Summary,
        string? Error);

    private sealed record AdminDashboardContentResponseDto(
        string Status,
        int? IssueCount,
        string? Summary,
        string? Error);

    private sealed record AdminDashboardIncidentsResponseDto(
        string Status,
        int? TotalCount,
        int? CriticalCount,
        string? Summary,
        string? Error);

    private sealed record AdminDashboardUnitsResponseDto(
        string Status,
        int? TotalCount,
        int? AvailableCount,
        int? BotBackfillCount,
        string? Summary,
        string? Error);
}
