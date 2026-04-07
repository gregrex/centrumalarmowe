
using Alarm112.Application.Interfaces;
using Alarm112.Application.Services;
using Alarm112.Api.Endpoints;
using Alarm112.Api.Hubs;
using Alarm112.Api.Middleware;
using Alarm112.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Formatting.Compact;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();

    if (context.HostingEnvironment.IsProduction())
    {
        loggerConfiguration.WriteTo.Console(new CompactJsonFormatter());
    }
    else
    {
        loggerConfiguration.WriteTo.Console();
    }
});

// Content bundle loader — resolves data/ relative to solution root
var dataRoot = Path.GetFullPath(
    Path.Combine(builder.Environment.ContentRootPath,
        builder.Configuration["ContentBundles:DataRoot"] ?? "../../data"));
builder.Services.AddSingleton<IContentBundleLoader>(_ => new JsonContentBundleLoader(dataRoot));

// Session store — use PostgreSQL when ConnectionStrings:Main is configured, else InMemory.
// In production: set ConnectionStrings__Main env var to activate PostgresSessionStore.
var pgConnStr = builder.Configuration.GetConnectionString("Main");
var usingPostgres = !string.IsNullOrWhiteSpace(pgConnStr);
if (usingPostgres)
{
    builder.Services.AddSingleton<ISessionStore>(sp =>
        new PostgresSessionStore(pgConnStr!, sp.GetRequiredService<ILogger<PostgresSessionStore>>()));
}
else
{
    builder.Services.AddSingleton<ISessionStore, InMemorySessionStore>();
}

builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOutputCache();

// Security (optional): JWT auth can be enabled per-environment via configuration.
var requireAuth = builder.Configuration.GetValue<bool>("Security:RequireAuth");

// IMPORTANT: Always register authorization policies and services, even when auth is disabled.
// This allows endpoints to use .RequireAuthorization() without errors.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtIssuer = builder.Configuration["Security:Jwt:Issuer"] ?? "Alarm112.Api";
        var jwtAudience = builder.Configuration["Security:Jwt:Audience"] ?? "Alarm112.Client";
        // If no key is configured (dev/test), generate a random one per startup.
        // This avoids any hardcoded known key in source. Production requires an env var key ≥32 chars.
        var configuredKey = builder.Configuration["Security:Jwt:SigningKey"] ?? string.Empty;
        var jwtSigningKey = configuredKey.Length >= 32
            ? configuredKey
            : Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32));

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey))
        };
    });

// RBAC policies for the 4 roles — always registered
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DispatcherOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Dispatcher"));

    options.AddPolicy("CoordinatorOrAbove", policy =>
        policy.RequireClaim(ClaimTypes.Role, "OperationsCoordinator", "CrisisOfficer"));

    options.AddPolicy("AdminOperations", policy =>
        policy.RequireClaim(ClaimTypes.Role, "CallOperator"));

    options.AddPolicy("Authenticated", policy =>
        policy.RequireAuthenticatedUser());
    // Default policy for development: allow everything when auth is disabled
    // This allows .RequireAuthorization() calls on endpoints without breaking tests
    if (!requireAuth)
    {
        options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAssertion(_ => true)
            .Build();
    }
});
// CORS — allow Unity client and admin panel cross-origin access
var allowedOrigins = builder.Configuration["Cors:AllowedOrigins"]?.Split(',', StringSplitOptions.RemoveEmptyEntries)
    ?? new[] { "http://localhost:3000", "http://localhost:5081", "http://localhost:5090" };
var allowedMethods = builder.Configuration["Cors:AllowedMethods"]?.Split(',', StringSplitOptions.RemoveEmptyEntries)
    ?? null; // null = AllowAnyMethod (dev default)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var p = policy.WithOrigins(allowedOrigins)
                      .AllowAnyHeader()
                      .AllowCredentials();
        if (allowedMethods is not null && allowedMethods.Length > 0)
            p.WithMethods(allowedMethods);
        else
            p.AllowAnyMethod();
    });
});

// Rate limiting — 200 requests per 10 seconds per IP
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", limiter =>
    {
        limiter.Window = TimeSpan.FromSeconds(10);
        limiter.PermitLimit = 200;
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiter.QueueLimit = 10;
    });
    options.RejectionStatusCode = 429;
});

// Problem details for structured error responses
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctx =>
    {
        if (ctx.Exception is Microsoft.AspNetCore.Http.BadHttpRequestException badReq)
        {
            ctx.ProblemDetails.Status = badReq.StatusCode;
            ctx.HttpContext.Response.StatusCode = badReq.StatusCode;
        }
    };
});
builder.Services.AddSingleton<ISessionService, SessionService>();
builder.Services.AddSingleton<IBotDirector, BotDirector>();
builder.Services.AddSingleton<IReferenceDataService, ReferenceDataService>();
builder.Services.AddSingleton<ILobbyService, LobbyService>();
builder.Services.AddSingleton<IContentValidationService, ContentValidationService>();
builder.Services.AddSingleton<IQuickPlayService, QuickPlayService>();
builder.Services.AddSingleton<ICityMapService, CityMapService>();
builder.Services.AddSingleton<IOperationsBoardService, OperationsBoardService>();
builder.Services.AddSingleton<IRoundRuntimeService, RoundRuntimeService>();
builder.Services.AddSingleton<IThemePackService, ThemePackService>();
builder.Services.AddSingleton<IHomeFlowService, HomeFlowService>();
builder.Services.AddSingleton<ICampaignEntryService, CampaignEntryService>();
builder.Services.AddSingleton<IRuntimeBootstrapService, RuntimeBootstrapService>();
builder.Services.AddSingleton<IMissionFlowService, MissionFlowService>();
builder.Services.AddSingleton<IMissionRuntimeService, MissionRuntimeService>();
builder.Services.AddSingleton<IPlayableRuntimeService, PlayableRuntimeService>();
builder.Services.AddSingleton<IRuntimePolishService, RuntimePolishService>();
builder.Services.AddSingleton<IQuasiProductionDemoService, QuasiProductionDemoService>();
builder.Services.AddSingleton<IRuntimeUiFlowService, RuntimeUiFlowService>();
builder.Services.AddSingleton<INearFinalSliceService, NearFinalSliceService>();
builder.Services.AddSingleton<IShowcaseDemoService, ShowcaseDemoService>();
builder.Services.AddSingleton<IReviewBuildService, ReviewBuildService>();
builder.Services.AddSingleton<IReleaseCandidateService, ReleaseCandidateService>();
builder.Services.AddSingleton<IAndroidPreviewService, AndroidPreviewService>();
builder.Services.AddSingleton<IInternalTestService, InternalTestService>();
builder.Services.AddSingleton<IFinalHandoffService, FinalHandoffService>();
builder.Services.AddSingleton<IRealAndroidBuildService, RealAndroidBuildService>();
builder.Services.AddHostedService<Alarm112.Application.Services.BotTickHostedService>();

var app = builder.Build();

// Log which session store is active (visible in container logs and dev output)
var startupLogger = app.Services.GetRequiredService<ILogger<Microsoft.AspNetCore.Builder.WebApplication>>();
startupLogger.LogInformation("Session store: {StoreType}",
    usingPostgres ? "PostgresSessionStore" : "InMemorySessionStore");

// Startup validation — fail fast if production config is incomplete
var isProduction = app.Environment.IsProduction();
var jwtKeyAtRuntime = app.Configuration["Security:Jwt:SigningKey"];
if (isProduction && string.IsNullOrWhiteSpace(jwtKeyAtRuntime))
    throw new InvalidOperationException(
        "Security:Jwt:SigningKey is required in production. Set the Security__Jwt__SigningKey environment variable.");
if (isProduction && (jwtKeyAtRuntime?.Length ?? 0) < 32)
    throw new InvalidOperationException(
        "Security:Jwt:SigningKey must be at least 32 characters in production.");

// Security headers — must be first in pipeline
app.UseSecurityHeaders();

// Correlation ID — assign/propagate X-Correlation-Id for every request
app.UseCorrelationId();

// Audit logging for all mutating requests (POST/PUT/PATCH/DELETE)
app.UseAuditLogging();

// Global error handler — structured ProblemDetails responses
app.UseExceptionHandler();
app.UseStatusCodePages();

// Swagger — only in Development (not production)
if (app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseRateLimiter();
app.UseOutputCache();

// Always enable auth/authz services (but only enforce if requireAuth=true)
app.UseAuthentication();
app.UseAuthorization();

if (requireAuth)
{
    app.Use(async (context, next) =>
    {
        var path = context.Request.Path.Value ?? string.Empty;

        // Keep health and docs public; enforce auth for API and SignalR traffic.
        var isPublicPath =
            path.StartsWith("/health", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/auth", StringComparison.OrdinalIgnoreCase);

        var requiresAuthPath =
            path.StartsWith("/api", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/hubs", StringComparison.OrdinalIgnoreCase);

        if (requiresAuthPath && !isPublicPath && context.User.Identity?.IsAuthenticated != true)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "Unauthorized" });
            return;
        }

        await next();
    });
}

app.MapGet("/health", (ISessionStore store, IContentValidationService contentSvc) =>
{
    var storeType = store.GetType().Name;
    return Results.Ok(new
    {
        ok = true,
        service = "Alarm112.Api",
        version = "v26",
        store = storeType,
        utc = DateTimeOffset.UtcNow
    });
});

// Endpoint groups — each group in its own file under Endpoints/
app.MapAuthEndpoints();
app.MapSessionEndpoints();
app.MapContentEndpoints();
app.MapCampaignEndpoints();
app.MapMissionEndpoints();
app.MapQuickPlayEndpoints();
app.MapHudEndpoints();
app.MapShowcaseEndpoints();
app.MapReleaseEndpoints();

app.MapHub<SessionHub>("/hubs/session");

app.Run();
