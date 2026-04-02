namespace Alarm112.Api.Middleware;

/// <summary>
/// Adds security response headers to all API responses.
/// Implements OWASP recommended headers.
/// </summary>
public sealed class SecurityHeadersMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var headers = context.Response.Headers;

        // Prevent clickjacking
        headers["X-Frame-Options"] = "DENY";

        // Prevent MIME-type sniffing
        headers["X-Content-Type-Options"] = "nosniff";

        // Legacy XSS protection (modern browsers use CSP instead)
        headers["X-XSS-Protection"] = "1; mode=block";

        // Control referrer information
        headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

        // Restrict browser features
        headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";

        // Content Security Policy — API only, no HTML rendering
        headers["Content-Security-Policy"] = "default-src 'none'; frame-ancestors 'none'";

        // Remove server identification
        headers.Remove("Server");
        headers.Remove("X-Powered-By");

        await next(context);
    }
}

public static class SecurityHeadersMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
        => app.UseMiddleware<SecurityHeadersMiddleware>();
}
