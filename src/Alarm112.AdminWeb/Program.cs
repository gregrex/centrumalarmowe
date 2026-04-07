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
                new AdminDashboardContentDto("auth-not-configured", null, "Skonfiguruj ApiAuth:Jwt:* w AdminWeb.", null)));
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var sessions = await FetchSessionsAsync(client, logger, cancellationToken);
        var content = await FetchContentAsync(client, logger, cancellationToken);

        return Results.Ok(new AdminDashboardSummaryDto(health, sessions, content));
    });

app.MapGet("/", () => Results.Content($$"""
<!DOCTYPE html>
<html lang="pl">
<head>
  <meta charset="utf-8"/>
  <meta name="viewport" content="width=device-width,initial-scale=1"/>
  <title>112 Centrum Alarmowe — Panel Admina</title>
  <link rel="stylesheet" href="/css/admin.css"/>
</head>
<body>
<div class="scanline-overlay"></div>

<!-- ── HEADER ─────────────────────────────────────────────── -->
<header class="header">
  <a class="header-logo" href="/">
    <span class="logo-badge">112</span>
    <span class="logo-name">Centrum Alarmowe <span>Panel Admina</span></span>
  </a>
  <div class="header-spacer"></div>
  <div class="header-status">
    <span class="status-dot" id="api-dot"></span>
    <span id="api-status-text">sprawdzam…</span>
  </div>
  <span class="header-ver">v26</span>
</header>

<!-- ── LAYOUT ─────────────────────────────────────────────── -->
<div class="layout">

  <!-- ── SIDEBAR ─────────────────────────────────────────── -->
  <nav class="sidebar">
    <div class="nav-section"><span class="nav-label">System</span></div>
    <a class="nav-item active" href="/">
      <span class="nav-icon">🏠</span> Dashboard
    </a>
    <a class="nav-item" href="{{apiBase}}/swagger" target="_blank">
      <span class="nav-icon">📘</span> Swagger UI
    </a>
    <a class="nav-item" href="{{apiBase}}/health" target="_blank">
      <span class="nav-icon">❤️</span> Health Check
    </a>
    <hr class="nav-divider"/>

    <div class="nav-section"><span class="nav-label">Content</span></div>
    <a class="nav-item" href="{{apiBase}}/api/reference-data" target="_blank">
      <span class="nav-icon">📦</span> Reference Data
    </a>
    <a class="nav-item" href="{{apiBase}}/api/content/validate" target="_blank">
      <span class="nav-icon">✅</span> Walidacja
    </a>
    <hr class="nav-divider"/>

    <div class="nav-section"><span class="nav-label">Sesje</span></div>
    <a class="nav-item" href="{{apiBase}}/api/home-hub" target="_blank">
      <span class="nav-icon">🏘️</span> Home Hub
    </a>
    <a class="nav-item" href="{{apiBase}}/api/city-map" target="_blank">
      <span class="nav-icon">🗺️</span> City Map
    </a>
    <a class="nav-item" href="{{apiBase}}/api/mission-briefing/demo" target="_blank">
      <span class="nav-icon">📋</span> Mission Briefing
    </a>
    <a class="nav-item" href="{{apiBase}}/api/quickplay/bootstrap" target="_blank">
      <span class="nav-icon">⚡</span> Quickplay
    </a>
    <hr class="nav-divider"/>

    <div class="nav-section"><span class="nav-label">Demo</span></div>
    <a class="nav-item" href="{{apiBase}}/api/showcase-demo/demo" target="_blank">
      <span class="nav-icon">🎬</span> Showcase Demo
    </a>
    <a class="nav-item" href="{{apiBase}}/api/real-android-build/demo" target="_blank">
      <span class="nav-icon">📱</span> Android Build
    </a>

    <div class="sidebar-footer">
      API: <a href="{{apiBase}}" target="_blank">{{apiBase}}</a>
    </div>
  </nav>

  <!-- ── MAIN ─────────────────────────────────────────────── -->
  <main class="main">
    <div class="page-header">
      <div class="page-title">Dashboard operacyjny</div>
      <div class="page-subtitle" id="page-subtitle">Centrum Alarmowe v26 · oczekiwanie na dane operacyjne</div>
    </div>

    <!-- METRYKI -->
    <div class="metrics">
      <div class="metric" style="--m-color:var(--ok)">
        <div class="metric-label">Status API</div>
        <div class="metric-value" id="metric-api-status">—</div>
        <div class="metric-sub" id="metric-api-sub">oczekiwanie…</div>
        <div class="metric-icon">🛰️</div>
      </div>
      <div class="metric" style="--m-color:var(--role-op)">
        <div class="metric-label">Sesje aktywne</div>
        <div class="metric-value" id="metric-sessions">—</div>
        <div class="metric-sub" id="metric-sessions-sub">oczekiwanie…</div>
        <div class="metric-icon">🎮</div>
      </div>
      <div class="metric" style="--m-color:var(--role-dis)">
        <div class="metric-label">Content bundle</div>
        <div class="metric-value" id="metric-bundle">—</div>
        <div class="metric-sub" id="metric-bundle-sub">reference-data v26</div>
        <div class="metric-icon">📦</div>
      </div>
      <div class="metric" style="--m-color:var(--role-coord)">
        <div class="metric-label">SignalR hub</div>
        <div class="metric-value">LIVE</div>
        <div class="metric-sub">/hubs/session</div>
        <div class="metric-icon">📡</div>
      </div>
      <div class="metric" style="--m-color:var(--role-crisis)">
        <div class="metric-label">Bot Director</div>
        <div class="metric-value">4 role</div>
        <div class="metric-sub">AI fallback aktywny</div>
        <div class="metric-icon">🤖</div>
      </div>
    </div>

    <!-- ROLE STATUS -->
    <div class="section-label">Role w systemie</div>
    <div class="roles-grid">
      <div class="role-card" style="--r-color:var(--role-op)">
        <div class="role-name">Call Operator</div>
        <div class="role-status"><span class="role-dot"></span>AI fallback gotowy</div>
      </div>
      <div class="role-card" style="--r-color:var(--role-dis)">
        <div class="role-name">Dispatcher</div>
        <div class="role-status"><span class="role-dot"></span>AI fallback gotowy</div>
      </div>
      <div class="role-card" style="--r-color:var(--role-coord)">
        <div class="role-name">Ops Coordinator</div>
        <div class="role-status"><span class="role-dot"></span>AI fallback gotowy</div>
      </div>
      <div class="role-card" style="--r-color:var(--role-crisis)">
        <div class="role-name">Crisis Officer</div>
        <div class="role-status"><span class="role-dot"></span>AI fallback gotowy</div>
      </div>
    </div>

    <!-- ENDPOINTS -->
    <div class="section-label">Endpointy API</div>
    <div class="grid">

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--ok)"></div>
        <div class="card-header">
          <div class="card-icon">❤️</div>
          <div class="card-title">Health Check</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Status serwera backend. Zwraca JSON z polem <code>ok</code> i wersją.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/health" target="_blank">GET /health</a>
          <a class="btn btn-link" href="{{apiBase}}/swagger" target="_blank">→ Swagger UI</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--role-dis)"></div>
        <div class="card-header">
          <div class="card-icon">✅</div>
          <div class="card-title">Walidacja contentu</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Sprawdza poprawność wszystkich JSON bundli content pipeline.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/api/content/validate" target="_blank">Waliduj</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--role-crisis)"></div>
        <div class="card-header">
          <div class="card-icon">🎮</div>
          <div class="card-title">Sesje demo</div>
          <span class="card-tag">POST</span>
        </div>
        <div class="card-desc">Utwórz lub przeglądaj sesje demo. Pełne POST dostępne w Swagger.</div>
        <div class="card-actions">
          <a class="btn btn-ghost" href="{{apiBase}}/api/sessions/demo" target="_blank">GET /sessions/demo</a>
          <a class="btn btn-link" href="{{apiBase}}/swagger#/Sessions" target="_blank">→ Swagger</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--role-op)"></div>
        <div class="card-header">
          <div class="card-icon">📦</div>
          <div class="card-title">Reference Data</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Pełny bundle danych referencyjnych v26: role, incydenty, jednostki, mapy.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/api/reference-data" target="_blank">GET /reference-data</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--role-coord)"></div>
        <div class="card-header">
          <div class="card-icon">🏘️</div>
          <div class="card-title">Home Hub</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Dane ekranu głównego gracza: profil, kampania, quickplay, powiadomienia.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/api/home-hub" target="_blank">GET /home-hub</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--role-op)"></div>
        <div class="card-header">
          <div class="card-icon">🗺️</div>
          <div class="card-title">City Map</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Mapa miasta z węzłami, połączeniami i aktualnymi incydentami.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/api/city-map" target="_blank">GET /city-map</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--warn)"></div>
        <div class="card-header">
          <div class="card-icon">📋</div>
          <div class="card-title">Mission Briefing</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Briefing misji showcase — pełny pakiet danych briefingu misji demo.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/api/mission-briefing/demo" target="_blank">GET /mission-briefing</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--role-coord)"></div>
        <div class="card-header">
          <div class="card-icon">⚡</div>
          <div class="card-title">Quickplay Lobby</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Bootstrap quickplay session — natychmiastowy start z AI botami.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/api/quickplay/bootstrap" target="_blank">Bootstrap</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--text-dim)"></div>
        <div class="card-header">
          <div class="card-icon">📱</div>
          <div class="card-title">Android Build Status</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Status buildu Android preview v26 — RC build metadata.</div>
        <div class="card-actions">
          <a class="btn btn-ghost" href="{{apiBase}}/api/real-android-build/demo" target="_blank">GET status</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--accent)"></div>
        <div class="card-header">
          <div class="card-icon">🎬</div>
          <div class="card-title">Showcase Demo Flow</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Kompletny flow showcase — misja demo operator + dispatcher end-to-end.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/api/showcase-demo/demo" target="_blank">GET showcase</a>
        </div>
      </div>

    </div><!-- /grid -->

    <!-- LOG FEED -->
    <div class="section-label">Log systemowy</div>
    <div class="log-feed">
      <div class="log-header">
        <span class="log-header-dot"></span>
        Live feed — aktywność panelu
      </div>
      <div class="log-body" id="log-body">
        <div class="log-row">
          <span class="log-time" id="ts-boot"></span>
          <span class="log-tag info">[BOOT]</span>
          <span class="log-msg">AdminWeb v26 załadowany</span>
        </div>
        <div class="log-row">
          <span class="log-time"></span>
          <span class="log-tag info">[SYS]</span>
          <span class="log-msg">Sprawdzanie statusu API: {{apiBase}}/health</span>
        </div>
      </div>
    </div>

  </main>
</div><!-- /layout -->

<script>window.API_BASE = '{{apiBase}}';</script>
<script src="/js/admin.js"></script>
</body>
</html>
""", "text/html"));

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

internal sealed record AdminApiAuthOptions(
    string? SigningKey,
    string Issuer,
    string Audience,
    string Subject,
    string Role);

internal sealed record AdminDashboardSummaryDto(
    AdminDashboardApiDto Api,
    AdminDashboardSessionsDto Sessions,
    AdminDashboardContentDto Content);

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
