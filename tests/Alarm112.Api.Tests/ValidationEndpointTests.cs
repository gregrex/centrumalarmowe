using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Alarm112.Api.Tests;

/// <summary>
/// Tests for input validation on API endpoints.
/// Verifies that invalid input returns 400/422 with proper error details.
/// </summary>
public sealed class ValidationEndpointTests(Alarm112ApiFactory factory)
    : IClassFixture<Alarm112ApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PostActions_NullBody_Returns400()
    {
        var content = new StringContent("", System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/sessions/test123/actions", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostActions_InvalidActionType_Returns400()
    {
        var response = await _client.PostAsJsonAsync("/api/sessions/test123/actions", new
        {
            sessionId = "test123",
            actorId = "player1",
            role = "Dispatcher",
            actionType = "INVALID_ACTION",
            correlationId = "corr-001"
        });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostActions_InvalidRole_Returns400()
    {
        var response = await _client.PostAsJsonAsync("/api/sessions/test123/actions", new
        {
            sessionId = "test123",
            actorId = "player1",
            role = "SuperAdmin",
            actionType = "dispatch",
            correlationId = "corr-001"
        });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostActions_OversizedPayloadJson_Returns400()
    {
        var oversized = new string('x', 2000);
        var response = await _client.PostAsJsonAsync("/api/sessions/test123/actions", new
        {
            sessionId = "test123",
            actorId = "player1",
            role = "Dispatcher",
            actionType = "dispatch",
            payloadJson = oversized,
            correlationId = "corr-001"
        });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetSession_InvalidSessionIdFormat_Returns400()
    {
        // Session ID with SQL injection attempt
        var response = await _client.GetAsync("/api/sessions/'; DROP TABLE sessions; --");
        // Should return 400 (invalid format) or 404, never 500
        Assert.NotEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.True(
            response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.NotFound,
            $"Expected 400 or 404, got {response.StatusCode}");
    }

    [Fact]
    public async Task PostActions_MissingSessionId_StillValidates()
    {
        // SessionId in body differs from URL (body matters for binding)
        var response = await _client.PostAsJsonAsync("/api/sessions/test123/actions", new
        {
            sessionId = (string?)null,
            actorId = "player1",
            role = "Dispatcher",
            actionType = "dispatch",
            correlationId = "corr-001"
        });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostQuickPlayStart_InvalidDifficulty_Returns400()
    {
        var response = await _client.PostAsJsonAsync("/api/quickplay/start", new
        {
            scenarioId = "scenario-1",
            difficulty = "IMPOSSIBLE",
            preferredRole = "Dispatcher",
            autoFillBots = true
        });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostQuickPlayStart_InvalidPreferredRole_Returns400()
    {
        var response = await _client.PostAsJsonAsync("/api/quickplay/start", new
        {
            scenarioId = "scenario-1",
            difficulty = "normal",
            preferredRole = "<script>alert(1)</script>",
            autoFillBots = true
        });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
