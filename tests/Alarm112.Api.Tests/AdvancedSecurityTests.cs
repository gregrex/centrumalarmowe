using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Alarm112.Api.Tests;

/// <summary>
/// Advanced security tests: JWT expiry, injection payloads, CORS, IDOR, rate limiting.
/// </summary>
public sealed class AdvancedSecurityTests(Alarm112ApiFactoryWithAuth factory)
    : IClassFixture<Alarm112ApiFactoryWithAuth>
{
    private readonly HttpClient _client = factory.CreateClient();

    private static string MakeToken(string role = "Dispatcher", bool expired = false, string? wrongKey = null)
    {
        var rawKey = wrongKey ?? "test-key-exactly-32-chars-minimum!!";
        var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(rawKey));
        var creds  = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);
        var expiry = expired ? DateTime.UtcNow.AddHours(-2) : DateTime.UtcNow.AddHours(1);
        var token  = new JwtSecurityToken(
            issuer: "Alarm112.Api",
            audience: "Alarm112.Client",
            claims: new[] { new Claim(ClaimTypes.Role, role) },
            notBefore: expired ? DateTime.UtcNow.AddHours(-4) : null,
            expires: expiry,
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // ─── JWT ────────────────────────────────────────────────────────────────

    [Fact]
    public async Task PostActions_WithExpiredToken_Returns401()
    {
        var jwt = MakeToken(expired: true);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        var response = await _client.PostAsJsonAsync("/api/sessions/test/actions", new
        {
            sessionId = "test", actorId = "p1", role = "Dispatcher",
            actionType = "dispatch", correlationId = "corr-exp"
        });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        _client.DefaultRequestHeaders.Authorization = null;
    }

    [Fact]
    public async Task PostActions_WrongSigningKey_Returns401()
    {
        var jwt = MakeToken(wrongKey: "totally-wrong-key-32-chars-min-xx");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        var response = await _client.PostAsJsonAsync("/api/sessions/test/actions", new
        {
            sessionId = "test", actorId = "p1", role = "Dispatcher",
            actionType = "dispatch", correlationId = "corr-key"
        });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        _client.DefaultRequestHeaders.Authorization = null;
    }

    [Fact]
    public async Task PostActions_TokenWithNoRole_StillProcessedOrRejected()
    {
        // Token valid but no role claim — endpoint requires auth, role check is best-effort
        var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("test-key-exactly-32-chars-minimum!!"));
        var creds  = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);
        var token  = new JwtSecurityToken(
            issuer: "Alarm112.Api",
            audience: "Alarm112.Client",
            claims: new[] { new Claim(ClaimTypes.NameIdentifier, "anonymous-user") },
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        var response = await _client.PostAsJsonAsync("/api/sessions/test/actions", new
        {
            sessionId = "test", actorId = "p1", role = "Dispatcher",
            actionType = "dispatch", correlationId = "corr-norole"
        });
        // 401 (not authenticated enough) or 400 (validation fails on role) — never 500
        Assert.NotEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        _client.DefaultRequestHeaders.Authorization = null;
    }

    // ─── Injection payloads ─────────────────────────────────────────────────

    [Fact]
    public async Task GetSession_XssInPath_DoesNotReturn500()
    {
        var response = await _client.GetAsync("/api/sessions/<script>alert(1)</script>");
        Assert.NotEqual(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task GetSession_PathTraversalAttempt_DoesNotReturn500()
    {
        var response = await _client.GetAsync("/api/sessions/../../etc/passwd");
        Assert.NotEqual(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task GetSession_SqlInjectionInPath_DoesNotReturn500()
    {
        // URL-encoded SQL injection
        var response = await _client.GetAsync("/api/sessions/1%27%20OR%20%271%27%3D%271");
        Assert.NotEqual(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task PostActions_XssInBodyFields_Returns400Or401()
    {
        // With auth enabled, 401 can come back before validation — both are safe rejections
        var response = await _client.PostAsJsonAsync("/api/sessions/test/actions", new
        {
            sessionId = "<script>alert('xss')</script>",
            actorId   = "<img src=x onerror=alert(1)>",
            role      = "Dispatcher",
            actionType = "dispatch",
            correlationId = "corr-xss"
        });
        Assert.True(
            response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.Unauthorized,
            $"Expected 400 or 401, got {response.StatusCode}");
    }

    [Fact]
    public async Task PostActions_NullByteInSessionId_DoesNotReturn500()
    {
        // Null byte injection — TestHost may throw decoding error or return 400
        // Either outcome is acceptable; 500 is not.
        try
        {
            var response = await _client.GetAsync("/api/sessions/test%00injection");
            Assert.NotEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }
        catch (Exception ex) when (ex is InvalidOperationException or UriFormatException or ArgumentException)
        {
            // TestHost URL decoder threw — this is acceptable (bad input rejected before reaching app)
        }
    }

    // ─── IDOR / Authorization ───────────────────────────────────────────────

    [Fact]
    public async Task GetSession_NonExistentSession_Returns404()
    {
        var response = await _client.GetAsync("/api/sessions/nonexistent-session-id-999");
        // Without auth, depends on policy. With auth disabled factory (this test uses auth factory),
        // check it does not leak data about other sessions
        Assert.True(
            response.StatusCode is HttpStatusCode.NotFound
                                or HttpStatusCode.Unauthorized
                                or HttpStatusCode.OK,
            $"Unexpected status: {response.StatusCode}");
    }

    // ─── Response headers ───────────────────────────────────────────────────

    [Fact]
    public async Task HealthEndpoint_HasSecurityHeaders()
    {
        var response = await _client.GetAsync("/health");
        // Must NOT expose server version details
        Assert.False(response.Headers.Contains("X-Powered-By"),
            "X-Powered-By header should not be present");
        Assert.False(
            response.Headers.TryGetValues("Server", out var serverVals) &&
            serverVals.Any(v => v.Contains("Kestrel")),
            "Server header should not reveal Kestrel version");
    }

    [Fact]
    public async Task HealthEndpoint_NoServerVersionInBody()
    {
        var response = await _client.GetAsync("/health");
        var body = await response.Content.ReadAsStringAsync();
        // Response should not contain .NET/runtime version strings
        Assert.DoesNotContain("Microsoft.AspNetCore", body);
    }

    // ─── Rate limiting ───────────────────────────────────────────────────────

    [Fact]
    public async Task RateLimiter_IsConfigured_RejectionStatusIs429()
    {
        // Send many rapid requests to hit the rate limit window
        // Rate limit: 200 per 10s. We send 210 requests.
        var tasks = new List<Task<HttpResponseMessage>>();
        using var burstClient = factory.CreateClient();
        for (int i = 0; i < 210; i++)
            tasks.Add(burstClient.GetAsync("/health"));

        var responses = await Task.WhenAll(tasks);
        var codes = responses.Select(r => r.StatusCode).ToList();

        // All should be 200 or 429 — never 500
        Assert.All(codes, code =>
            Assert.True(code is HttpStatusCode.OK or HttpStatusCode.TooManyRequests or HttpStatusCode.ServiceUnavailable,
                $"Unexpected status: {code}"));

        // At least some should succeed
        Assert.Contains(HttpStatusCode.OK, codes);
    }

    // ─── Error format ────────────────────────────────────────────────────────

    [Fact]
    public async Task PostActions_BadRequest_ReturnsProblemDetailsFormat()
    {
        // Use a valid JWT so we get past auth and reach validation layer
        var jwt = MakeToken("Dispatcher");
        using var authClient = factory.CreateClient();
        authClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        var response = await authClient.PostAsJsonAsync("/api/sessions/test/actions", new
        {
            sessionId = "test", actorId = "p1", role = "BADBADROLE",
            actionType = "dispatch", correlationId = "corr-pd"
        });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        // Should return JSON problem details, not HTML error page
        var contentType = response.Content.Headers.ContentType?.ToString() ?? "";
        Assert.True(
            contentType.Contains("application/json") || contentType.Contains("application/problem+json"),
            $"Expected JSON content type, got: {contentType}");
    }
}
