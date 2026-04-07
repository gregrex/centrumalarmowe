using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var apiBase = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5080";
if (!Uri.TryCreate(apiBase, UriKind.Absolute, out var apiBaseUri))
    throw new InvalidOperationException("ApiBaseUrl must be an absolute URI.");

var apiAuth = new AdminApiAuthOptions(
    builder.Configuration["ApiAuth:Jwt:SigningKey"],
    builder.Configuration["ApiAuth:Jwt:Issuer"] ?? "Alarm112.Api",
    builder.Configuration["ApiAuth:Jwt:Audience"] ?? "Alarm112.Client",
    builder.Configuration["ApiAuth:Subject"] ?? "admin-dashboard",
    builder.Configuration["ApiAuth:Role"] ?? "CallOperator");

// Basic Auth credentials — MUST be provided via environment variables
// Set AdminAuth__Username and AdminAuth__Password env vars (never use defaults in production)
var adminUser = builder.Configuration["AdminAuth:Username"]
    ?? throw new InvalidOperationException("AdminAuth:Username is required. Set the AdminAuth__Username environment variable.");
var adminPass = builder.Configuration["AdminAuth:Password"]
    ?? throw new InvalidOperationException("AdminAuth:Password is required. Set the AdminAuth__Password environment variable.");

if (adminPass.Length < 12)
    throw new InvalidOperationException("AdminAuth:Password must be at least 12 characters.");

builder.Services.AddHttpClient();

var app = builder.Build();
var dashboardHtml = LoadDashboardHtml(app.Environment.ContentRootPath, apiBase);

// Basic Auth middleware for all non-health routes
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? string.Empty;

    // Health check is always public
    if (path.StartsWith("/health", StringComparison.OrdinalIgnoreCase))
    {
        await next();
        return;
    }

    // Check Authorization header
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
        // Malformed Base64 in Authorization header — reject with 400
        var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
            .CreateLogger("AdminAuth");
        logger.LogWarning("Malformed Base64 in Authorization header from {RemoteIp}",
            context.Connection.RemoteIpAddress);
        context.Response.StatusCode = 400;
        return;
    }

    await next();
});

app.UseStaticFiles();

app.MapGet("/health", () => Results.Ok(new { ok = true, service = "Alarm112.AdminWeb", version = "v26" }));

app.MapGet("/api/admin/dashboard",
    async (IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory, CancellationToken cancellationToken) =>
    {
        var logger = loggerFactory.CreateLogger("AdminDashboard");
        using var client = httpClientFactory.CreateClient();
        client.BaseAddress = apiBaseUri;
        client.Timeout = TimeSpan.FromSeconds(5);

        var health = await FetchHealthAsync(client, logger, cancellationToken);

        if (TryCreateApiAccessToken(apiAuth) is not { } accessToken)
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
    });

app.MapGet("/", () => Results.Content(dashboardHtml, "text/html"));

app.Run();

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
        logger.LogWarning(ex, "Admin dashboard could not read API health.");
        return new AdminDashboardApiDto("offline", null, null, null, ex.Message);
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

        var payload = await response.Content.ReadFromJsonAsync<Alarm112.Contracts.ContentValidationResultDto>(cancellationToken);
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
            return await response.Content.ReadFromJsonAsync<Alarm112.Contracts.ActiveIncidentBoardDto>(cancellationToken);
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
            return await response.Content.ReadFromJsonAsync<IReadOnlyCollection<Alarm112.Contracts.UnitRuntimeDto>>(cancellationToken);
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

static string LoadDashboardHtml(string contentRootPath, string apiBase)
{
    var candidatePaths = new[]
    {
        Path.Combine(contentRootPath, "Templates", "Dashboard.html"),
        Path.Combine(AppContext.BaseDirectory, "Templates", "Dashboard.html")
    };

    var templatePath = candidatePaths.FirstOrDefault(File.Exists)
        ?? throw new FileNotFoundException("Admin dashboard template not found.", candidatePaths[0]);

    return File.ReadAllText(templatePath)
        .Replace("{{apiBase}}", apiBase, StringComparison.Ordinal);
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
