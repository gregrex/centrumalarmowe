using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Alarm112.Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
var apiBase = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5080";
if (!Uri.TryCreate(apiBase, UriKind.Absolute, out var apiBaseUri))
    throw new InvalidOperationException("ApiBaseUrl must be an absolute URI.");

var adminApiAuth = new AdminApiAuthOptions(
    builder.Configuration["ApiAuth:Jwt:SigningKey"],
    builder.Configuration["ApiAuth:Jwt:Issuer"] ?? "Alarm112.Api",
    builder.Configuration["ApiAuth:Jwt:Audience"] ?? "Alarm112.Client",
    builder.Configuration["ApiAuth:Subject"] ?? "admin-dashboard",
    builder.Configuration["ApiAuth:Role"] ?? "CallOperator");

var demoUserApiAuth = adminApiAuth with
{
    Subject = builder.Configuration["ApiAuth:DemoSubject"] ?? "demo-user-dashboard",
    Role = builder.Configuration["ApiAuth:DemoRole"] ?? "Dispatcher"
};

var adminUser = builder.Configuration["AdminAuth:Username"]
    ?? throw new InvalidOperationException("AdminAuth:Username is required. Set the AdminAuth__Username environment variable.");
var adminPass = builder.Configuration["AdminAuth:Password"]
    ?? throw new InvalidOperationException("AdminAuth:Password is required. Set the AdminAuth__Password environment variable.");

if (adminPass.Length < 12)
    throw new InvalidOperationException("AdminAuth:Password must be at least 12 characters.");

builder.Services.AddHttpClient();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost;
    ResetForwardedHeaderTrust(options);
});
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("public", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            GetRateLimitPartitionKey(context, "public"),
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 120,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            }));

    options.AddPolicy("contact", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            GetRateLimitPartitionKey(context, "contact"),
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            }));

    options.AddPolicy("admin", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            GetRateLimitPartitionKey(context, "admin"),
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 240,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            }));
});

var app = builder.Build();
var landingHtml = LoadTemplateHtml(app.Environment.ContentRootPath, "Landing.html", apiBase);
var dashboardHtml = LoadTemplateHtml(app.Environment.ContentRootPath, "Dashboard.html", apiBase);
var userDashboardHtml = LoadTemplateHtml(app.Environment.ContentRootPath, "UserDashboard.html", apiBase);

app.UseForwardedHeaders();

app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;
    headers["X-Frame-Options"] = "DENY";
    headers["X-Content-Type-Options"] = "nosniff";
    headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
    headers["Content-Security-Policy"] =
        "default-src 'self'; img-src 'self' data:; style-src 'self' 'unsafe-inline'; script-src 'self'; connect-src 'self'; form-action 'self'; base-uri 'self'; frame-ancestors 'none'";
    headers.Remove("Server");
    headers.Remove("X-Powered-By");

    await next();
});

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? string.Empty;

    if (IsPublicPath(path))
    {
        await next();
        return;
    }

    if (RequiresAdminAuth(path))
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader) ||
            !authHeader.ToString().StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"112 Admin Panel\", charset=\"UTF-8\"";
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Authentication required.");
            return;
        }

        try
        {
            var encoded = authHeader.ToString().Substring("Basic ".Length).Trim();
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
            var parts = decoded.Split(':', 2);
            if (parts.Length != 2 || parts[0] != adminUser || parts[1] != adminPass)
            {
                context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"112 Admin Panel\", charset=\"UTF-8\"";
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid credentials.");
                return;
            }
        }
        catch (FormatException)
        {
            var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("AdminAuth");
            logger.LogWarning("Malformed Base64 in Authorization header from {RemoteIp}", context.Connection.RemoteIpAddress);
            context.Response.StatusCode = 400;
            return;
        }
    }

    await next();
});

app.UseStaticFiles();
app.UseRateLimiter();

app.MapGet("/health", () => Results.Ok(new { ok = true, service = "Alarm112.AdminWeb", version = "v26" }));

app.MapPost("/api/public/contact",
    (ContactLeadRequestDto request, ILoggerFactory loggerFactory, CancellationToken cancellationToken) =>
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Message))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["general"] = ["Name, email and message are required."]
            });
        }

        if (request.Name.Length > 120 || request.Email.Length > 200 || request.Company?.Length > 120 || request.Message.Length > 2000)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["general"] = ["One or more fields exceed the allowed length."]
            });
        }

        if (!new EmailAddressAttribute().IsValid(request.Email))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["email"] = ["Email must be a valid email address."]
            });
        }

        var logger = loggerFactory.CreateLogger("LandingContact");
        logger.LogInformation("Landing contact lead captured for {Email} from {Company}", request.Email, request.Company ?? "n/a");

        return Results.Accepted("/app", new ContactLeadResponseDto(
            "accepted",
            "Dziekujemy. Zgloszenie demo zostalo zapisane do follow-up.",
            DateTimeOffset.UtcNow));
    }).RequireRateLimiting("contact");

app.MapGet("/api/user/dashboard",
    async (IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory, CancellationToken cancellationToken) =>
    {
        var logger = loggerFactory.CreateLogger("UserDashboard");
        using var client = httpClientFactory.CreateClient();
        client.BaseAddress = apiBaseUri;
        client.Timeout = TimeSpan.FromSeconds(5);

        var health = await FetchHealthAsync(client, logger, cancellationToken);
        ApplyApiTokenIfConfigured(client, demoUserApiAuth);

        var home = await FetchHomeHubAsync(client, logger, cancellationToken);
        var chapters = await FetchCampaignChaptersAsync(client, logger, cancellationToken);
        var missionEntry = await FetchMissionEntryAsync(client, logger, cancellationToken);
        var briefing = await FetchMissionBriefingAsync(client, logger, cancellationToken);
        var quickPlay = await FetchQuickPlayBootstrapAsync(client, logger, cancellationToken);
        var showcase = await FetchShowcaseMissionAsync(client, logger, cancellationToken);

        return Results.Ok(new UserDashboardSummaryDto(
            health,
            home,
            chapters,
            missionEntry,
            briefing,
            quickPlay,
            showcase,
            new UserDashboardCallToActionDto(
                "Uruchom demo",
                "/app",
                "Otworz panel operatora i scenariusz showcase od razu po starcie stacka.")));
    }).RequireRateLimiting("public");

app.MapGet("/api/admin/dashboard",
    async (IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory, CancellationToken cancellationToken) =>
    {
        var logger = loggerFactory.CreateLogger("AdminDashboard");
        using var client = httpClientFactory.CreateClient();
        client.BaseAddress = apiBaseUri;
        client.Timeout = TimeSpan.FromSeconds(5);

        var health = await FetchHealthAsync(client, logger, cancellationToken);

        if (TryCreateApiAccessToken(adminApiAuth) is not { } accessToken)
        {
            return Results.Ok(new AdminDashboardSummaryDto(
                health,
                new AdminDashboardSessionsDto("auth-not-configured", null, "Skonfiguruj ApiAuth:Jwt:* w AdminWeb.", null),
                new AdminDashboardContentDto("auth-not-configured", null, "Skonfiguruj ApiAuth:Jwt:* w AdminWeb.", null),
                new AdminDashboardIncidentsDto("auth-not-configured", null, null, "Skonfiguruj ApiAuth:Jwt:* w AdminWeb.", null),
                new AdminDashboardUnitsDto("auth-not-configured", null, null, null, "Skonfiguruj ApiAuth:Jwt:* w AdminWeb.", null)));
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var sessions = await FetchSessionsAsync(client, logger, cancellationToken);
        var content = await FetchContentAsync(client, logger, cancellationToken);
        var sessionIds = await FetchSessionIdsAsync(client, logger, cancellationToken);
        var incidents = await FetchIncidentsAsync(client, logger, sessionIds, cancellationToken);
        var units = await FetchUnitsAsync(client, logger, sessionIds, cancellationToken);

        return Results.Ok(new AdminDashboardSummaryDto(health, sessions, content, incidents, units));
    }).RequireRateLimiting("admin");

app.MapGet("/", () => Results.Content(landingHtml, "text/html")).RequireRateLimiting("public");
app.MapGet("/app", () => Results.Content(userDashboardHtml, "text/html")).RequireRateLimiting("public");
app.MapGet("/admin", () => Results.Content(dashboardHtml, "text/html")).RequireRateLimiting("admin");

app.Run();

static bool IsPublicPath(string path) =>
    path.Equals("/", StringComparison.OrdinalIgnoreCase) ||
    path.Equals("/app", StringComparison.OrdinalIgnoreCase) ||
    path.Equals("/health", StringComparison.OrdinalIgnoreCase) ||
    path.StartsWith("/health/", StringComparison.OrdinalIgnoreCase) ||
    path.StartsWith("/css/", StringComparison.OrdinalIgnoreCase) ||
    path.StartsWith("/js/", StringComparison.OrdinalIgnoreCase) ||
    path.StartsWith("/api/public/", StringComparison.OrdinalIgnoreCase) ||
    path.Equals("/api/user/dashboard", StringComparison.OrdinalIgnoreCase);

static bool RequiresAdminAuth(string path) =>
    path.Equals("/admin", StringComparison.OrdinalIgnoreCase) ||
    path.StartsWith("/api/admin/", StringComparison.OrdinalIgnoreCase);

static string GetRateLimitPartitionKey(HttpContext context, string policyName) =>
    $"{policyName}:{context.Connection.RemoteIpAddress?.ToString() ?? "unknown"}";

static void ApplyApiTokenIfConfigured(HttpClient client, AdminApiAuthOptions options)
{
    if (TryCreateApiAccessToken(options) is { } accessToken)
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
}

static async Task<AdminDashboardApiDto> FetchHealthAsync(
    HttpClient client,
    ILogger logger,
    CancellationToken cancellationToken)
{
    try
    {
        using var response = await client.GetAsync("/health", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return new AdminDashboardApiDto(
                "offline",
                null,
                $"HTTP {(int)response.StatusCode}",
                null,
                $"Backend returned {(int)response.StatusCode}.");
        }

        var payload = await response.Content.ReadFromJsonAsync<AdminHealthResponseDto>(cancellationToken);
        return new AdminDashboardApiDto(
            "online",
            payload?.Version ?? "v26",
            payload?.Service ?? "Alarm112.Api",
            payload?.Store,
            null);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Dashboard could not read API health.");
        return new AdminDashboardApiDto("offline", null, null, null, ex.Message);
    }
}

static async Task<UserDashboardHomeDto> FetchHomeHubAsync(
    HttpClient client,
    ILogger logger,
    CancellationToken cancellationToken)
{
    try
    {
        using var response = await client.GetAsync("/api/home-hub", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return new UserDashboardHomeDto("unavailable", null, null, null, [], $"HTTP {(int)response.StatusCode}");
        }

        var payload = await response.Content.ReadFromJsonAsync<HomeHubDto>(cancellationToken);
        return new UserDashboardHomeDto(
            "ok",
            payload?.DefaultScreen,
            payload?.ContinueSummary,
            payload?.ContinueSessionId,
            payload?.Cards ?? [],
            null);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "User dashboard could not read home hub.");
        return new UserDashboardHomeDto("unavailable", null, null, null, [], ex.Message);
    }
}

static async Task<UserDashboardChaptersDto> FetchCampaignChaptersAsync(
    HttpClient client,
    ILogger logger,
    CancellationToken cancellationToken)
{
    try
    {
        using var response = await client.GetAsync("/api/campaign-chapters/demo", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return new UserDashboardChaptersDto("unavailable", null, null, [], $"HTTP {(int)response.StatusCode}");
        }

        var payload = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<CampaignChapterDto>>(cancellationToken);
        var chapters = payload ?? [];
        var missionCount = chapters.Sum(chapter => chapter.Nodes.Count);

        return new UserDashboardChaptersDto(
            "ok",
            chapters.Count,
            missionCount,
            chapters,
            null);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "User dashboard could not read campaign chapters.");
        return new UserDashboardChaptersDto("unavailable", null, null, [], ex.Message);
    }
}

static async Task<UserDashboardMissionEntryDto> FetchMissionEntryAsync(
    HttpClient client,
    ILogger logger,
    CancellationToken cancellationToken)
{
    try
    {
        using var response = await client.GetAsync("/api/mission-entry/demo", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return new UserDashboardMissionEntryDto("unavailable", null, null, null, null, [], $"HTTP {(int)response.StatusCode}");
        }

        var payload = await response.Content.ReadFromJsonAsync<CampaignMissionEntryDto>(cancellationToken);
        return new UserDashboardMissionEntryDto(
            "ok",
            payload?.MissionId,
            payload?.TitleKey,
            payload?.Difficulty,
            payload?.RecommendedRole,
            payload?.RiskTags ?? [],
            null);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "User dashboard could not read mission entry.");
        return new UserDashboardMissionEntryDto("unavailable", null, null, null, null, [], ex.Message);
    }
}

static async Task<UserDashboardBriefingDto> FetchMissionBriefingAsync(
    HttpClient client,
    ILogger logger,
    CancellationToken cancellationToken)
{
    try
    {
        using var response = await client.GetAsync("/api/mission-briefing/demo", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return new UserDashboardBriefingDto("unavailable", null, null, null, [], [], $"HTTP {(int)response.StatusCode}");
        }

        var payload = await response.Content.ReadFromJsonAsync<MissionBriefingDto>(cancellationToken);
        return new UserDashboardBriefingDto(
            "ok",
            payload?.TitleKey,
            payload?.Difficulty,
            payload?.EstimatedMinutes,
            payload?.PrimaryObjectives ?? [],
            payload?.RecommendedRoles ?? [],
            null);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "User dashboard could not read mission briefing.");
        return new UserDashboardBriefingDto("unavailable", null, null, null, [], [], ex.Message);
    }
}

static async Task<UserDashboardQuickPlayDto> FetchQuickPlayBootstrapAsync(
    HttpClient client,
    ILogger logger,
    CancellationToken cancellationToken)
{
    try
    {
        using var response = await client.GetAsync("/api/quickplay/bootstrap", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return new UserDashboardQuickPlayDto("unavailable", null, null, null, null, [], $"HTTP {(int)response.StatusCode}");
        }

        var payload = await response.Content.ReadFromJsonAsync<QuickPlayBootstrapDto>(cancellationToken);
        return new UserDashboardQuickPlayDto(
            "ok",
            payload?.ScenarioId,
            payload?.Difficulty,
            payload?.PreferredRole,
            payload?.IncidentIds.Count,
            payload?.RecommendedRoles ?? [],
            null);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "User dashboard could not read quick play bootstrap.");
        return new UserDashboardQuickPlayDto("unavailable", null, null, null, null, [], ex.Message);
    }
}

static async Task<UserDashboardShowcaseDto> FetchShowcaseMissionAsync(
    HttpClient client,
    ILogger logger,
    CancellationToken cancellationToken)
{
    try
    {
        using var response = await client.GetAsync("/api/showcase-mission/demo", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return new UserDashboardShowcaseDto("unavailable", null, null, null, [], $"HTTP {(int)response.StatusCode}");
        }

        var payload = await response.Content.ReadFromJsonAsync<ShowcaseMissionDto>(cancellationToken);
        return new UserDashboardShowcaseDto(
            "ok",
            payload?.MissionId,
            payload?.Title,
            payload?.RecommendedRole,
            payload?.Steps ?? [],
            null);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "User dashboard could not read showcase mission.");
        return new UserDashboardShowcaseDto("unavailable", null, null, null, [], ex.Message);
    }
}

static async Task<AdminDashboardSessionsDto> FetchSessionsAsync(
    HttpClient client,
    ILogger logger,
    CancellationToken cancellationToken)
{
    try
    {
        using var response = await client.GetAsync("/api/sessions?page=1&pageSize=1", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return new AdminDashboardSessionsDto(
                "unavailable",
                null,
                $"HTTP {(int)response.StatusCode}",
                $"Session endpoint returned {(int)response.StatusCode}.");
        }

        var payload = await response.Content.ReadFromJsonAsync<AdminSessionsResponseDto>(cancellationToken);
        return new AdminDashboardSessionsDto("ok", payload?.TotalCount ?? 0, "Aktywne sesje z API.", null);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Admin dashboard could not read session summary.");
        return new AdminDashboardSessionsDto("unavailable", null, null, ex.Message);
    }
}

static async Task<AdminDashboardContentDto> FetchContentAsync(
    HttpClient client,
    ILogger logger,
    CancellationToken cancellationToken)
{
    try
    {
        using var response = await client.GetAsync("/api/content/validate", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return new AdminDashboardContentDto(
                "unavailable",
                null,
                $"HTTP {(int)response.StatusCode}",
                $"Content validation returned {(int)response.StatusCode}.");
        }

        var payload = await response.Content.ReadFromJsonAsync<ContentValidationResultDto>(cancellationToken);
        var status = payload?.IsValid == true ? "valid" : "invalid";
        var summary = payload?.IsValid == true
            ? "Brak problemow walidacji."
            : "Wykryto problemy wymagajace reakcji.";

        return new AdminDashboardContentDto(status, payload?.Issues.Count ?? 0, summary, null);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Admin dashboard could not read content validation summary.");
        return new AdminDashboardContentDto("unavailable", null, null, ex.Message);
    }
}

static async Task<IReadOnlyCollection<string>> FetchSessionIdsAsync(
    HttpClient client,
    ILogger logger,
    CancellationToken cancellationToken)
{
    var sessionIds = new List<string>();
    var page = 1;
    var totalPages = 1;

    try
    {
        while (page <= totalPages)
        {
            using var response = await client.GetAsync($"/api/sessions?page={page}&pageSize=100", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Admin dashboard could not enumerate sessions. HTTP {StatusCode}", (int)response.StatusCode);
                return [];
            }

            var payload = await response.Content.ReadFromJsonAsync<AdminSessionsResponseDto>(cancellationToken);
            if (payload is null)
                return [];

            sessionIds.AddRange(payload.Items);
            totalPages = Math.Max(payload.TotalPages, 1);
            page++;
        }

        return sessionIds;
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Admin dashboard could not enumerate sessions.");
        return [];
    }
}

static async Task<AdminDashboardIncidentsDto> FetchIncidentsAsync(
    HttpClient client,
    ILogger logger,
    IReadOnlyCollection<string> sessionIds,
    CancellationToken cancellationToken)
{
    if (sessionIds.Count == 0)
        return new AdminDashboardIncidentsDto("ok", 0, 0, "Brak aktywnych sesji.", null);

    try
    {
        var boards = await Task.WhenAll(sessionIds.Select(async sessionId =>
        {
            using var response = await client.GetAsync($"/api/sessions/{sessionId}/active-incidents", cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ActiveIncidentBoardDto>(cancellationToken);
        }));

        var totalCount = boards.Sum(board => board?.ActiveCount ?? 0);
        var criticalCount = boards.Sum(board => board?.CriticalCount ?? 0);
        var summary = criticalCount > 0
            ? $"{totalCount} aktywne / {criticalCount} krytyczne."
            : $"{totalCount} aktywne / brak krytycznych.";

        return new AdminDashboardIncidentsDto("ok", totalCount, criticalCount, summary, null);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Admin dashboard could not aggregate incident metrics.");
        return new AdminDashboardIncidentsDto("unavailable", null, null, null, ex.Message);
    }
}

static async Task<AdminDashboardUnitsDto> FetchUnitsAsync(
    HttpClient client,
    ILogger logger,
    IReadOnlyCollection<string> sessionIds,
    CancellationToken cancellationToken)
{
    if (sessionIds.Count == 0)
        return new AdminDashboardUnitsDto("ok", 0, 0, 0, "Brak aktywnych sesji.", null);

    try
    {
        var unitsPerSession = await Task.WhenAll(sessionIds.Select(async sessionId =>
        {
            using var response = await client.GetAsync($"/api/sessions/{sessionId}/units/runtime", cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IReadOnlyCollection<UnitRuntimeDto>>(cancellationToken);
        }));

        var units = unitsPerSession
            .Where(collection => collection is not null)
            .SelectMany(collection => collection!)
            .ToList();

        var totalCount = units.Count;
        var availableCount = units.Count(unit => unit.Available);
        var botBackfillCount = units.Count(unit => unit.IsBotBackfilled);
        var summary = $"{availableCount} dostepne z {totalCount} runtime, boty: {botBackfillCount}.";

        return new AdminDashboardUnitsDto("ok", totalCount, availableCount, botBackfillCount, summary, null);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Admin dashboard could not aggregate unit metrics.");
        return new AdminDashboardUnitsDto("unavailable", null, null, null, null, ex.Message);
    }
}

static string? TryCreateApiAccessToken(AdminApiAuthOptions options)
{
    if (string.IsNullOrWhiteSpace(options.SigningKey) || options.SigningKey.Length < 32)
        return null;

    var credentials = new SigningCredentials(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey)),
        SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        options.Issuer,
        options.Audience,
        [
            new Claim(JwtRegisteredClaimNames.Sub, options.Subject),
            new Claim(ClaimTypes.Name, options.Subject),
            new Claim(ClaimTypes.Role, options.Role)
        ],
        expires: DateTime.UtcNow.AddMinutes(5),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
}

static string LoadTemplateHtml(string contentRootPath, string templateName, string apiBase)
{
    var candidatePaths = new[]
    {
        Path.Combine(contentRootPath, "Templates", templateName),
        Path.Combine(AppContext.BaseDirectory, "Templates", templateName)
    };

    var templatePath = candidatePaths.FirstOrDefault(File.Exists)
        ?? throw new FileNotFoundException($"AdminWeb template not found: {templateName}", candidatePaths[0]);

    return File.ReadAllText(templatePath)
        .Replace("{{apiBase}}", apiBase, StringComparison.Ordinal)
        .Replace("{{year}}", DateTime.UtcNow.Year.ToString(), StringComparison.Ordinal);
}

static void ResetForwardedHeaderTrust(ForwardedHeadersOptions options)
{
    ClearForwardedHeadersCollection(options, "KnownIPNetworks");
    ClearForwardedHeadersCollection(options, "KnownNetworks");
    ClearForwardedHeadersCollection(options, "KnownProxies");
}

static void ClearForwardedHeadersCollection(ForwardedHeadersOptions options, string propertyName)
{
    var property = typeof(ForwardedHeadersOptions).GetProperty(propertyName);
    var collection = property?.GetValue(options);
    var clearMethod = collection?.GetType().GetMethod("Clear", Type.EmptyTypes);
    clearMethod?.Invoke(collection, null);
}

internal sealed record AdminApiAuthOptions(
    string? SigningKey,
    string Issuer,
    string Audience,
    string Subject,
    string Role);

internal sealed record AdminDashboardSummaryDto(
    AdminDashboardApiDto Api,
    AdminDashboardSessionsDto Sessions,
    AdminDashboardContentDto Content,
    AdminDashboardIncidentsDto Incidents,
    AdminDashboardUnitsDto Units);

internal sealed record UserDashboardSummaryDto(
    AdminDashboardApiDto Api,
    UserDashboardHomeDto Home,
    UserDashboardChaptersDto Chapters,
    UserDashboardMissionEntryDto MissionEntry,
    UserDashboardBriefingDto Briefing,
    UserDashboardQuickPlayDto QuickPlay,
    UserDashboardShowcaseDto Showcase,
    UserDashboardCallToActionDto CallToAction);

internal sealed record UserDashboardHomeDto(
    string Status,
    string? DefaultScreen,
    string? ContinueSummary,
    string? ContinueSessionId,
    IReadOnlyCollection<HomeCardDto> Cards,
    string? Error);

internal sealed record UserDashboardChaptersDto(
    string Status,
    int? ChapterCount,
    int? MissionNodeCount,
    IReadOnlyCollection<CampaignChapterDto> Items,
    string? Error);

internal sealed record UserDashboardMissionEntryDto(
    string Status,
    string? MissionId,
    string? Title,
    string? Difficulty,
    string? RecommendedRole,
    IReadOnlyCollection<string> RiskTags,
    string? Error);

internal sealed record UserDashboardBriefingDto(
    string Status,
    string? Title,
    string? Difficulty,
    int? EstimatedMinutes,
    IReadOnlyCollection<string> PrimaryObjectives,
    IReadOnlyCollection<string> RecommendedRoles,
    string? Error);

internal sealed record UserDashboardQuickPlayDto(
    string Status,
    string? ScenarioId,
    string? Difficulty,
    string? PreferredRole,
    int? IncidentCount,
    IReadOnlyCollection<string> RecommendedRoles,
    string? Error);

internal sealed record UserDashboardShowcaseDto(
    string Status,
    string? MissionId,
    string? Title,
    string? RecommendedRole,
    IReadOnlyCollection<ShowcaseMissionStepDto> Steps,
    string? Error);

internal sealed record UserDashboardCallToActionDto(
    string Label,
    string Route,
    string Description);

internal sealed record AdminDashboardApiDto(
    string Status,
    string? Version,
    string? Service,
    string? Store,
    string? Error);

internal sealed record AdminDashboardSessionsDto(
    string Status,
    int? TotalCount,
    string? Summary,
    string? Error);

internal sealed record AdminDashboardContentDto(
    string Status,
    int? IssueCount,
    string? Summary,
    string? Error);

internal sealed record AdminDashboardIncidentsDto(
    string Status,
    int? TotalCount,
    int? CriticalCount,
    string? Summary,
    string? Error);

internal sealed record AdminDashboardUnitsDto(
    string Status,
    int? TotalCount,
    int? AvailableCount,
    int? BotBackfillCount,
    string? Summary,
    string? Error);

internal sealed record AdminHealthResponseDto(
    bool Ok,
    string Service,
    string Version,
    string? Store);

internal sealed record AdminSessionsResponseDto(
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages,
    IReadOnlyCollection<string> Items);

internal sealed record ContactLeadRequestDto(
    string Name,
    string Email,
    string? Company,
    string Message);

internal sealed record ContactLeadResponseDto(
    string Status,
    string Message,
    DateTimeOffset Utc);

public partial class Program { }
