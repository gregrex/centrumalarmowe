
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
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Content bundle loader — resolves data/ relative to solution root
var dataRoot = Path.GetFullPath(
    Path.Combine(builder.Environment.ContentRootPath,
        builder.Configuration["ContentBundles:DataRoot"] ?? "../../data"));
builder.Services.AddSingleton<IContentBundleLoader>(_ => new JsonContentBundleLoader(dataRoot));

// Session store (in-memory; swap for DB-backed impl when ready)
builder.Services.AddSingleton<ISessionStore, InMemorySessionStore>();

builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Security (optional): JWT auth can be enabled per-environment via configuration.
var requireAuth = builder.Configuration.GetValue<bool>("Security:RequireAuth");

// IMPORTANT: Always register authorization policies and services, even when auth is disabled.
// This allows endpoints to use .RequireAuthorization() without errors.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtIssuer = builder.Configuration["Security:Jwt:Issuer"] ?? "Alarm112.Api";
        var jwtAudience = builder.Configuration["Security:Jwt:Audience"] ?? "Alarm112.Client";
        var jwtSigningKey = builder.Configuration["Security:Jwt:SigningKey"]
            ?? "dev-only-signing-key-change-me-to-32-plus-chars";

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
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
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

// Security headers — must be first in pipeline
app.UseSecurityHeaders();

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

app.MapGet("/health", () => Results.Ok(new { ok = true, service = "Alarm112.Api", version = "v26" }));

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
