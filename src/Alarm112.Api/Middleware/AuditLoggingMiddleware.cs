namespace Alarm112.Api.Middleware;

/// <summary>
/// Logs all mutating (non-GET) requests: method, path, status, actor identity, duration.
/// Satisfies SEC-14 audit trail requirement.
/// </summary>
public sealed class AuditLoggingMiddleware(RequestDelegate next, ILogger<AuditLoggingMiddleware> logger)
{
    private static readonly HashSet<string> AuditMethods =
        new(StringComparer.OrdinalIgnoreCase) { "POST", "PUT", "PATCH", "DELETE" };

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        await next(context);

        sw.Stop();

        if (!AuditMethods.Contains(context.Request.Method))
            return;

        var userId = context.User?.Identity?.Name ?? "anonymous";
        var role = context.User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "-";
        var status = context.Response.StatusCode;
        var path = context.Request.Path.Value ?? "/";
        var method = context.Request.Method;
        var ms = sw.ElapsedMilliseconds;

        var level = status >= 500 ? LogLevel.Error
                  : status >= 400 ? LogLevel.Warning
                  : LogLevel.Information;

        logger.Log(level,
            "[AUDIT] {Method} {Path} → {Status} | actor={Actor} role={Role} | {Ms}ms",
            method, path, status, userId, role, ms);
    }
}

public static class AuditLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseAuditLogging(this IApplicationBuilder app)
        => app.UseMiddleware<AuditLoggingMiddleware>();
}
