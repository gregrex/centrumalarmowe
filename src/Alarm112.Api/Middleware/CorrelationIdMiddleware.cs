namespace Alarm112.Api.Middleware;

/// <summary>
/// Reads or generates an X-Correlation-Id header per request.
/// Propagates the correlation ID into the log scope so every log entry for
/// that request is tagged with it automatically.
/// The correlation ID is also echoed back in the response header.
/// </summary>
public sealed class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-Id";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Use existing header from client (e.g., forwarded from load balancer) or generate a new one.
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(correlationId) || correlationId.Length > 128)
            correlationId = Guid.NewGuid().ToString("N");

        // Store in HttpContext.Items for access by handlers and middlewares
        context.Items[HeaderName] = correlationId;

        // Echo back so clients can correlate responses with their requests
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        // Push into log scope so every log line for this request carries the correlation ID
        using (_logger.BeginScope(new Dictionary<string, object> { [HeaderName] = correlationId }))
        {
            await _next(context);
        }
    }
}

public static class CorrelationIdMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        => app.UseMiddleware<CorrelationIdMiddleware>();
}
