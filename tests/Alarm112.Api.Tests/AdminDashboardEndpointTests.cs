using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

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
    public async Task UserDashboard_ReturnsLiveApiData()
    {
        await using var stubApi = await StubApiHost.StartAsync(requireAuth: true);
        await using var factory = new AdminWebFactory(stubApi.BaseUrl, ApiSigningKey);
        using var client = factory.CreateClient();

        var response = await client.GetFromJsonAsync<UserDashboardResponseDto>("/api/user/dashboard");

        Assert.NotNull(response);
        Assert.Equal("online", response!.Api.Status);
        Assert.Equal("ok", response.Home.Status);
        Assert.Equal("dispatch.home", response.Home.DefaultScreen);
        Assert.Equal(2, response.Home.Cards.Count);
        Assert.Equal("ok", response.Chapters.Status);
        Assert.Equal(2, response.Chapters.ChapterCount);
        Assert.Equal(3, response.Chapters.MissionNodeCount);
        Assert.Equal("ok", response.MissionEntry.Status);
        Assert.Equal("mission.demo.01", response.MissionEntry.MissionId);
        Assert.Equal("Dispatcher", response.MissionEntry.RecommendedRole);
        Assert.Equal("ok", response.Briefing.Status);
        Assert.Equal(18, response.Briefing.EstimatedMinutes);
        Assert.Equal("ok", response.QuickPlay.Status);
        Assert.Equal(3, response.QuickPlay.IncidentCount);
        Assert.Equal("ok", response.Showcase.Status);
        Assert.Equal(3, response.Showcase.Steps.Count);
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
    public async Task RootPage_ReturnsRenderedLandingTemplateWithoutAuth()
    {
        await using var stubApi = await StubApiHost.StartAsync(requireAuth: false);
        await using var factory = new AdminWebFactory(stubApi.BaseUrl, apiSigningKey: ApiSigningKey);
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/");
        var body = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("Nowoczesna dyspozytornia", body);
        Assert.Contains("/app", body);
        Assert.Contains("/admin", body);
    }

    [Fact]
    public async Task AdminPage_RequiresBasicAuth()
    {
        await using var stubApi = await StubApiHost.StartAsync(requireAuth: false);
        await using var factory = new AdminWebFactory(stubApi.BaseUrl, apiSigningKey: ApiSigningKey);
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/admin");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AdminPage_WithBasicAuth_ReturnsRenderedDashboardTemplate()
    {
        await using var stubApi = await StubApiHost.StartAsync(requireAuth: false);
        await using var factory = new AdminWebFactory(stubApi.BaseUrl, apiSigningKey: ApiSigningKey);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = CreateBasicAuthHeader();

        var response = await client.GetAsync("/admin");
        var body = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("Dashboard operacyjny", body);
        Assert.Contains(stubApi.BaseUrl, body);
        Assert.Contains("/js/admin.js", body);
    }

    [Fact]
    public async Task LandingPage_HasSecurityHeaders()
    {
        await using var stubApi = await StubApiHost.StartAsync(requireAuth: false);
        await using var factory = new AdminWebFactory(stubApi.BaseUrl, apiSigningKey: ApiSigningKey);
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/");

        Assert.True(response.Headers.Contains("X-Frame-Options"));
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").First());
        Assert.True(response.Headers.Contains("X-Content-Type-Options"));
        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").First());
        Assert.True(response.Headers.Contains("Content-Security-Policy"));
    }

    [Fact]
    public async Task ContactEndpoint_IsRateLimited()
    {
        await using var stubApi = await StubApiHost.StartAsync(requireAuth: false);
        await using var factory = new AdminWebFactory(stubApi.BaseUrl, apiSigningKey: ApiSigningKey);
        using var client = factory.CreateClient();

        var codes = new List<HttpStatusCode>();
        for (var i = 0; i < 12; i++)
        {
            var response = await client.PostAsJsonAsync("/api/public/contact", new
            {
                name = $"Lead {i}",
                email = $"lead{i}@example.com",
                company = "Alarm112",
                message = "Chce zobaczyc demo produktu."
            });

            codes.Add(response.StatusCode);
        }

        Assert.Contains(HttpStatusCode.Accepted, codes);
        Assert.Contains(HttpStatusCode.TooManyRequests, codes);
    }

    [Fact]
    public async Task ContactEndpoint_InvalidEmail_ReturnsValidationProblem()
    {
        await using var stubApi = await StubApiHost.StartAsync(requireAuth: false);
        await using var factory = new AdminWebFactory(stubApi.BaseUrl, apiSigningKey: ApiSigningKey);
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/public/contact", new
        {
            name = "Lead Invalid",
            email = "not-an-email",
            company = "Alarm112",
            message = "Prosze o demo."
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload!.GetProperty("errors").TryGetProperty("email", out _));
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

            app.MapGet("/api/home-hub", (HttpContext context) =>
            {
                if (requireAuth && !HasBearerToken(context))
                    return Results.Unauthorized();

                return Results.Ok(new
                {
                    defaultScreen = "dispatch.home",
                    continueSessionId = "demo-session-1",
                    continueSummary = "1 aktywna zmiana i 2 alerty priorytetowe.",
                    cards = new[]
                    {
                        new { id = "card-1", type = "campaign", labelKey = "home.card.campaign", state = "ready", route = "/campaign" },
                        new { id = "card-2", type = "quickplay", labelKey = "home.card.quickplay", state = "hot", route = "/quickplay" }
                    }
                });
            });

            app.MapGet("/api/campaign-chapters/demo", (HttpContext context) =>
            {
                if (requireAuth && !HasBearerToken(context))
                    return Results.Unauthorized();

                return Results.Ok(new[]
                {
                    new
                    {
                        chapterId = "chapter-1",
                        titleKey = "chapter.one",
                        themeId = "urban",
                        progress = 0.5,
                        nodes = new[]
                        {
                            new { missionId = "mission.demo.01", nodeKind = "mission", state = "ready", x = 0.1, y = 0.3, titleKey = "mission.one" },
                            new { missionId = "mission.demo.02", nodeKind = "mission", state = "locked", x = 0.4, y = 0.6, titleKey = "mission.two" }
                        }
                    },
                    new
                    {
                        chapterId = "chapter-2",
                        titleKey = "chapter.two",
                        themeId = "storm",
                        progress = 0.2,
                        nodes = new[]
                        {
                            new { missionId = "mission.demo.03", nodeKind = "mission", state = "ready", x = 0.8, y = 0.4, titleKey = "mission.three" }
                        }
                    }
                });
            });

            app.MapGet("/api/mission-entry/demo", (HttpContext context) =>
            {
                if (requireAuth && !HasBearerToken(context))
                    return Results.Unauthorized();

                return Results.Ok(new
                {
                    missionId = "mission.demo.01",
                    titleKey = "mission.entry.demo",
                    chapterId = "chapter-1",
                    estimatedMinutes = 18,
                    difficulty = "high",
                    recommendedRole = "Dispatcher",
                    weatherPreset = "rain",
                    timeOfDay = "night",
                    startingUnits = new[] { "unit-1", "unit-2" },
                    riskTags = new[] { "traffic", "weather" },
                    rewards = new[] { "xp", "unlock" },
                    availableSlots = 4,
                    defaultBotFillMode = "fill"
                });
            });

            app.MapGet("/api/mission-briefing/demo", (HttpContext context) =>
            {
                if (requireAuth && !HasBearerToken(context))
                    return Results.Unauthorized();

                return Results.Ok(new
                {
                    missionId = "mission.demo.01",
                    titleKey = "briefing.demo.one",
                    difficulty = "high",
                    estimatedMinutes = 18,
                    weatherPreset = "rain",
                    timeOfDay = "night",
                    primaryObjectives = new[] { "Stabilizuj ruch", "Zabezpiecz ofiary" },
                    secondaryObjectives = new[] { "Utrzymaj KPI" },
                    riskTags = new[] { "traffic", "weather" },
                    recommendedRoles = new[] { "Dispatcher", "CallOperator" },
                    suggestedUnits = new[] { "unit-1", "unit-2" },
                    speakerPortraitId = "portrait-1",
                    speakerLineKey = "speaker.line.demo",
                    hotspots = new[] { "hotspot-1" }
                });
            });

            app.MapGet("/api/quickplay/bootstrap", (HttpContext context) =>
            {
                if (requireAuth && !HasBearerToken(context))
                    return Results.Unauthorized();

                return Results.Ok(new
                {
                    scenarioId = "quickplay.demo",
                    difficulty = "high",
                    preferredRole = "Dispatcher",
                    autoFillBots = true,
                    incidentIds = new[] { "inc-1", "inc-2", "inc-3" },
                    recommendedRoles = new[] { "Dispatcher", "OperationsCoordinator" }
                });
            });

            app.MapGet("/api/showcase-mission/demo", (HttpContext context) =>
            {
                if (requireAuth && !HasBearerToken(context))
                    return Results.Unauthorized();

                return Results.Ok(new
                {
                    missionId = "showcase-1",
                    title = "Operator / Dispatcher showcase",
                    recommendedRole = "Dispatcher",
                    estimatedDurationSeconds = 240,
                    steps = new[]
                    {
                        new { stepId = "s1", title = "Receive alert", description = "Incoming emergency", order = 1, isMandatory = true },
                        new { stepId = "s2", title = "Dispatch units", description = "Assign resources", order = 2, isMandatory = true },
                        new { stepId = "s3", title = "Recover operations", description = "Close out the shift", order = 3, isMandatory = false }
                    }
                });
            });

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

    private sealed record UserDashboardResponseDto(
        AdminDashboardApiResponseDto Api,
        UserDashboardHomeResponseDto Home,
        UserDashboardChaptersResponseDto Chapters,
        UserDashboardMissionEntryResponseDto MissionEntry,
        UserDashboardBriefingResponseDto Briefing,
        UserDashboardQuickPlayResponseDto QuickPlay,
        UserDashboardShowcaseResponseDto Showcase);

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

    private sealed record UserDashboardHomeResponseDto(
        string Status,
        string? DefaultScreen,
        string? ContinueSummary,
        string? ContinueSessionId,
        IReadOnlyCollection<UserDashboardHomeCardResponseDto> Cards,
        string? Error);

    private sealed record UserDashboardHomeCardResponseDto(
        string Id,
        string Type,
        string LabelKey,
        string State,
        string Route);

    private sealed record UserDashboardChaptersResponseDto(
        string Status,
        int? ChapterCount,
        int? MissionNodeCount,
        IReadOnlyCollection<object> Items,
        string? Error);

    private sealed record UserDashboardMissionEntryResponseDto(
        string Status,
        string? MissionId,
        string? Title,
        string? Difficulty,
        string? RecommendedRole,
        IReadOnlyCollection<string> RiskTags,
        string? Error);

    private sealed record UserDashboardBriefingResponseDto(
        string Status,
        string? Title,
        string? Difficulty,
        int? EstimatedMinutes,
        IReadOnlyCollection<string> PrimaryObjectives,
        IReadOnlyCollection<string> RecommendedRoles,
        string? Error);

    private sealed record UserDashboardQuickPlayResponseDto(
        string Status,
        string? ScenarioId,
        string? Difficulty,
        string? PreferredRole,
        int? IncidentCount,
        IReadOnlyCollection<string> RecommendedRoles,
        string? Error);

    private sealed record UserDashboardShowcaseResponseDto(
        string Status,
        string? MissionId,
        string? Title,
        string? RecommendedRole,
        IReadOnlyCollection<UserDashboardShowcaseStepResponseDto> Steps,
        string? Error);

    private sealed record UserDashboardShowcaseStepResponseDto(
        string StepId,
        string Title,
        string Description,
        int Order,
        bool IsMandatory);
}
