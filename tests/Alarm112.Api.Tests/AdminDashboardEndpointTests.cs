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
                    pageSize = 1,
                    totalCount = 7,
                    totalPages = 7,
                    items = new[] { "demo" }
                });
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
        AdminDashboardContentResponseDto Content);

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
}
