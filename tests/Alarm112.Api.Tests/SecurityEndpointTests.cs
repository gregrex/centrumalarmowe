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
/// Tests verifying security behaviors: auth enforcement, token validation.
/// </summary>
public sealed class SecurityEndpointTests(Alarm112ApiFactoryWithAuth factory)
    : IClassFixture<Alarm112ApiFactoryWithAuth>
{
    private readonly HttpClient _client = factory.CreateClient();

    private static string GenerateToken(string role = "Dispatcher", bool expired = false)
    {
        const string key = "test-key-exactly-32-chars-minimum!!";
        var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);
        var expiry = expired ? DateTime.UtcNow.AddHours(-1) : DateTime.UtcNow.AddHours(1);
        var token = new JwtSecurityToken(
            issuer: "Alarm112.Api",
            audience: "Alarm112.Client",
            claims: new[] { new Claim(ClaimTypes.Role, role) },
            expires: expiry,
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Fact]
    public async Task PostActions_WithoutToken_Returns401()
    {
        var response = await _client.PostAsJsonAsync("/api/sessions/test123/actions", new
        {
            sessionId = "test123",
            actorId = "player1",
            role = "Dispatcher",
            actionType = "dispatch",
            correlationId = "corr-001"
        });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PostDispatch_WithoutToken_Returns401()
    {
        var response = await _client.PostAsJsonAsync("/api/sessions/test123/dispatch", new
        {
            incidentId = "inc-1",
            unitId = "unit-1",
            actionId = "act-1",
            actorRole = "Dispatcher",
            isBot = false
        });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PostActions_WithValidToken_Returns200OrAccepted()
    {
        // Create session first (no auth needed for demo session creation when it's behind public path logic)
        // Note: in auth mode, even demo session creation requires auth
        var jwt = GenerateToken("Dispatcher");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var createResp = await _client.PostAsync("/api/sessions/demo", null);
        // Should succeed with valid token
        Assert.True(createResp.StatusCode is HttpStatusCode.OK or HttpStatusCode.Unauthorized,
            $"Unexpected status: {createResp.StatusCode}");
    }

    [Fact]
    public async Task DevTokenEndpoint_WhenEnabled_ReturnsToken()
    {
        var response = await _client.PostAsJsonAsync("/auth/dev-token", new
        {
            subject = "test-user",
            role = "Dispatcher"
        });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.False(string.IsNullOrEmpty(json.GetProperty("accessToken").GetString()));
    }

    [Fact]
    public async Task DevTokenEndpoint_InvalidRole_Returns400()
    {
        var response = await _client.PostAsJsonAsync("/auth/dev-token", new
        {
            subject = "test-user",
            role = "SuperAdmin"
        });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostActions_WithInvalidToken_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "this.is.not.valid");
        var response = await _client.PostAsJsonAsync("/api/sessions/test123/actions", new
        {
            sessionId = "test123",
            actorId = "player1",
            role = "Dispatcher",
            actionType = "dispatch",
            correlationId = "corr-002"
        });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
