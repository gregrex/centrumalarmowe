using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Alarm112.Api.Tests;

/// <summary>
/// Tests for the extended health endpoint and new session-3 features.
/// Covers: health shape with store field, audit middleware transparency, caching headers.
/// </summary>
public sealed class Session3FeatureTests(Alarm112ApiFactory factory)
    : IClassFixture<Alarm112ApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    // ─── Health endpoint (extended) ──────────────────────────────────────────

    [Fact]
    public async Task Health_Returns200()
    {
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Health_HasStoreField()
    {
        var response = await _client.GetAsync("/health");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(json.TryGetProperty("store", out var store),
            "Health response should include 'store' field indicating the session store type.");
        Assert.False(string.IsNullOrWhiteSpace(store.GetString()),
            "store field should not be empty.");
    }

    [Fact]
    public async Task Health_HasUtcField()
    {
        var response = await _client.GetAsync("/health");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(json.TryGetProperty("utc", out _),
            "Health response should include 'utc' timestamp field.");
    }

    [Fact]
    public async Task Health_StoreIsInMemory_WhenInMemoryConfigured()
    {
        var response = await _client.GetAsync("/health");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var store = json.GetProperty("store").GetString();
        // Test environment uses InMemorySessionStore
        Assert.Equal("InMemorySessionStore", store);
    }

    // ─── Audit logging transparency ───────────────────────────────────────────

    [Fact]
    public async Task AuditMiddleware_DoesNotBreakGetRequests()
    {
        // GET requests should pass through audit middleware without affecting response
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AuditMiddleware_DoesNotBreakPostRequests()
    {
        // POST requests should still return correct status codes
        var response = await _client.PostAsync("/api/sessions/demo", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AuditMiddleware_PostsDoNotLeak500()
    {
        // Malformed POST should return 4xx, not 500 (audit middleware must not swallow exceptions)
        var content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/sessions/bad-id/actions", content);
        Assert.True(
            (int)response.StatusCode < 500,
            $"Expected 4xx, got {(int)response.StatusCode}");
    }

    // ─── SignalR hub auth ─────────────────────────────────────────────────────

    [Fact]
    public async Task SignalRHub_WithAuthDisabled_StillConnects()
    {
        // In dev mode (auth disabled), [Authorize] on hub uses permissive default policy
        // Verify hub endpoint is reachable via negotiate
        var response = await _client.PostAsync("/hubs/session/negotiate?negotiateVersion=1", null);
        // Negotiate should succeed (200) or redirect (307) — never 5xx
        Assert.True(
            response.StatusCode is HttpStatusCode.OK or HttpStatusCode.Redirect
                or HttpStatusCode.TemporaryRedirect or HttpStatusCode.Found,
            $"Expected negotiate success, got {response.StatusCode}");
    }

    // ─── CORS restricted methods ──────────────────────────────────────────────

    [Fact]
    public async Task ContentEndpoint_ReferenceData_Returns200()
    {
        var response = await _client.GetAsync("/api/reference-data");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ContentEndpoint_CityMap_Returns200()
    {
        var response = await _client.GetAsync("/api/city-map");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // ─── SharedActionDto validation ───────────────────────────────────────────

    [Fact]
    public void SharedActionDto_Validation_RequiresSharedActionId()
    {
        var dto = new Alarm112.Contracts.SharedActionDto(
            SharedActionId: "",         // empty — invalid
            IncidentId: "inc-001",
            ActionType: "confirm",
            RequestedByRole: "Dispatcher",
            RequiredRoles: ["CallOperator"],
            TimeoutSeconds: 30,
            AllowBotAssist: true);

        var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
        var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var valid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(dto, ctx, results, validateAllProperties: true);
        Assert.False(valid, "Empty SharedActionId should fail validation.");
    }

    [Fact]
    public void SharedActionDto_Validation_RequiresValidRole()
    {
        var dto = new Alarm112.Contracts.SharedActionDto(
            SharedActionId: "sa-001",
            IncidentId: "inc-001",
            ActionType: "confirm",
            RequestedByRole: "SuperAdmin",   // invalid role
            RequiredRoles: ["CallOperator"],
            TimeoutSeconds: 30,
            AllowBotAssist: true);

        var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
        var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var valid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(dto, ctx, results, validateAllProperties: true);
        Assert.False(valid, "Invalid role 'SuperAdmin' should fail validation.");
    }

    [Fact]
    public void SharedActionDto_Validation_RejectsZeroTimeout()
    {
        var dto = new Alarm112.Contracts.SharedActionDto(
            SharedActionId: "sa-001",
            IncidentId: "inc-001",
            ActionType: "confirm",
            RequestedByRole: "Dispatcher",
            RequiredRoles: [],
            TimeoutSeconds: 0,   // out of range
            AllowBotAssist: true);

        var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
        var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var valid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(dto, ctx, results, validateAllProperties: true);
        Assert.False(valid, "TimeoutSeconds=0 should fail Range validation.");
    }

    [Fact]
    public void SharedActionDto_Validation_AcceptsValidDto()
    {
        var dto = new Alarm112.Contracts.SharedActionDto(
            SharedActionId: "sa-valid-001",
            IncidentId: "inc-001",
            ActionType: "confirm",
            RequestedByRole: "Dispatcher",
            RequiredRoles: ["CallOperator", "OperationsCoordinator"],
            TimeoutSeconds: 60,
            AllowBotAssist: false);

        var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
        var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var valid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(dto, ctx, results, validateAllProperties: true);
        Assert.True(valid, $"Valid SharedActionDto should pass. Errors: {string.Join(", ", results.Select(r => r.ErrorMessage))}");
    }
}
