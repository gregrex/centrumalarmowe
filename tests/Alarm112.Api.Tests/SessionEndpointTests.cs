using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Alarm112.Api.Tests;

public sealed class SessionEndpointTests(Alarm112ApiFactory factory)
    : IClassFixture<Alarm112ApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateDemoSession_Returns200()
    {
        var response = await _client.PostAsync("/api/sessions/demo", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateDemoSession_ReturnsSessionId()
    {
        var response = await _client.PostAsync("/api/sessions/demo", null);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var sessionId = json.GetProperty("sessionId").GetString();
        Assert.False(string.IsNullOrEmpty(sessionId));
    }

    [Fact]
    public async Task GetDemoSession_ReturnsSnapshot()
    {
        // Create session first
        var createResp = await _client.PostAsync("/api/sessions/demo", null);
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var sessionId = created.GetProperty("sessionId").GetString()!;

        // Retrieve it
        var getResp = await _client.GetAsync($"/api/sessions/{sessionId}");
        Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);
        var snap = await getResp.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(sessionId, snap.GetProperty("sessionId").GetString());
    }

    [Fact]
    public async Task GetSession_UnknownId_Returns404()
    {
        var response = await _client.GetAsync("/api/sessions/non-existent-session");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetSession_UnknownId_ReturnsProblemDetails()
    {
        var response = await _client.GetAsync("/api/sessions/non-existent-session");
        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(404, payload.GetProperty("status").GetInt32());
        Assert.Equal("Session not found.", payload.GetProperty("title").GetString());
    }

    [Fact]
    public async Task ApplyDispatchAction_Returns200()
    {
        // Create session
        var createResp = await _client.PostAsync("/api/sessions/demo", null);
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var sessionId = created.GetProperty("sessionId").GetString()!;

        // Get current state to find a pending incident and available unit
        var snapResp = await _client.GetAsync($"/api/sessions/{sessionId}");
        var snap = await snapResp.Content.ReadFromJsonAsync<JsonElement>();

        string incidentId = "inc-demo-1";
        string unitId = "unit-1";

        foreach (var i in snap.GetProperty("incidents").EnumerateArray())
        {
            if (i.GetProperty("status").GetString() == "pending")
            {
                incidentId = i.GetProperty("incidentId").GetString() ?? incidentId;
                break;
            }
        }

        foreach (var u in snap.GetProperty("units").EnumerateArray())
        {
            if (u.GetProperty("status").GetString() == "available")
            {
                unitId = u.GetProperty("unitId").GetString() ?? unitId;
                break;
            }
        }

        var action = new
        {
            sessionId,
            actorId = "player-1",
            role = "Dispatcher",
            actionType = "dispatch",
            payloadJson = $"{{\"incidentId\":\"{incidentId}\",\"unitId\":\"{unitId}\"}}",
            correlationId = Guid.NewGuid().ToString("N")
        };

        var response = await _client.PostAsJsonAsync($"/api/sessions/{sessionId}/actions", action);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ApplyDispatchAction_ReturnsSuccess()
    {
        var createResp = await _client.PostAsync("/api/sessions/demo", null);
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var sessionId = created.GetProperty("sessionId").GetString()!;

        var snapshot = await _client.GetFromJsonAsync<JsonElement>($"/api/sessions/{sessionId}");
        var incidentId = snapshot.GetProperty("incidents").EnumerateArray().First(i => i.GetProperty("status").GetString() == "pending").GetProperty("incidentId").GetString()!;
        var unitId = snapshot.GetProperty("units").EnumerateArray().First(u => u.GetProperty("status").GetString() == "available").GetProperty("unitId").GetString()!;

        var action = new
        {
            sessionId,
            actorId = "player-1",
            role = "Dispatcher",
            actionType = "dispatch",
            payloadJson = $"{{\"incidentId\":\"{incidentId}\",\"unitId\":\"{unitId}\"}}",
            correlationId = Guid.NewGuid().ToString("N")
        };

        var response = await _client.PostAsJsonAsync($"/api/sessions/{sessionId}/actions", action);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(result.GetProperty("success").GetBoolean());
    }

    [Fact]
    public async Task ApplyDispatchAction_MutatesSnapshotState()
    {
        var createResp = await _client.PostAsync("/api/sessions/demo", null);
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var sessionId = created.GetProperty("sessionId").GetString()!;

        var snapshot = await _client.GetFromJsonAsync<JsonElement>($"/api/sessions/{sessionId}");
        var incidentId = snapshot.GetProperty("incidents").EnumerateArray().First(i => i.GetProperty("status").GetString() == "pending").GetProperty("incidentId").GetString()!;
        var unitId = snapshot.GetProperty("units").EnumerateArray().First(u => u.GetProperty("status").GetString() == "available").GetProperty("unitId").GetString()!;

        var response = await _client.PostAsJsonAsync($"/api/sessions/{sessionId}/actions", new
        {
            sessionId,
            actorId = "player-1",
            role = "Dispatcher",
            actionType = "dispatch",
            payloadJson = $"{{\"incidentId\":\"{incidentId}\",\"unitId\":\"{unitId}\"}}",
            correlationId = Guid.NewGuid().ToString("N")
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updated = await _client.GetFromJsonAsync<JsonElement>($"/api/sessions/{sessionId}");
        var incident = updated.GetProperty("incidents").EnumerateArray().First(i => i.GetProperty("incidentId").GetString() == incidentId);
        var unit = updated.GetProperty("units").EnumerateArray().First(u => u.GetProperty("unitId").GetString() == unitId);

        Assert.Equal("dispatched", incident.GetProperty("status").GetString());
        Assert.Equal("dispatched", unit.GetProperty("status").GetString());
    }

    [Fact]
    public async Task ApplyDispatchAction_ReplayedCorrelationId_ReturnsDuplicate()
    {
        var createResp = await _client.PostAsync("/api/sessions/demo", null);
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var sessionId = created.GetProperty("sessionId").GetString()!;
        var snapshot = await _client.GetFromJsonAsync<JsonElement>($"/api/sessions/{sessionId}");
        var incidentId = snapshot.GetProperty("incidents").EnumerateArray().First(i => i.GetProperty("status").GetString() == "pending").GetProperty("incidentId").GetString()!;
        var unitId = snapshot.GetProperty("units").EnumerateArray().First(u => u.GetProperty("status").GetString() == "available").GetProperty("unitId").GetString()!;
        var correlationId = Guid.NewGuid().ToString("N");
        var action = new
        {
            sessionId,
            actorId = "player-1",
            role = "Dispatcher",
            actionType = "dispatch",
            payloadJson = $"{{\"incidentId\":\"{incidentId}\",\"unitId\":\"{unitId}\"}}",
            correlationId
        };

        var first = await _client.PostAsJsonAsync($"/api/sessions/{sessionId}/actions", action);
        var second = await _client.PostAsJsonAsync($"/api/sessions/{sessionId}/actions", action);
        var result = await second.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        Assert.Equal(HttpStatusCode.OK, second.StatusCode);
        Assert.True(result.GetProperty("duplicate").GetBoolean());
    }

    [Fact]
    public async Task ApplyResolveAction_MutatesSnapshotState()
    {
        var createResp = await _client.PostAsync("/api/sessions/demo", null);
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var sessionId = created.GetProperty("sessionId").GetString()!;

        var snapshot = await _client.GetFromJsonAsync<JsonElement>($"/api/sessions/{sessionId}");
        var incidentId = snapshot.GetProperty("incidents").EnumerateArray().First(i => i.GetProperty("status").GetString() == "pending").GetProperty("incidentId").GetString()!;
        var unitId = snapshot.GetProperty("units").EnumerateArray().First(u => u.GetProperty("status").GetString() == "available").GetProperty("unitId").GetString()!;

        await _client.PostAsJsonAsync($"/api/sessions/{sessionId}/actions", new
        {
            sessionId,
            actorId = "player-1",
            role = "Dispatcher",
            actionType = "dispatch",
            payloadJson = $"{{\"incidentId\":\"{incidentId}\",\"unitId\":\"{unitId}\"}}",
            correlationId = Guid.NewGuid().ToString("N")
        });

        var resolveResponse = await _client.PostAsJsonAsync($"/api/sessions/{sessionId}/actions", new
        {
            sessionId,
            actorId = "player-2",
            role = "OperationsCoordinator",
            actionType = "resolve",
            payloadJson = $"{{\"incidentId\":\"{incidentId}\",\"unitId\":\"{unitId}\"}}",
            correlationId = Guid.NewGuid().ToString("N")
        });

        Assert.Equal(HttpStatusCode.OK, resolveResponse.StatusCode);

        var updated = await _client.GetFromJsonAsync<JsonElement>($"/api/sessions/{sessionId}");
        var incident = updated.GetProperty("incidents").EnumerateArray().First(i => i.GetProperty("incidentId").GetString() == incidentId);
        var unit = updated.GetProperty("units").EnumerateArray().First(u => u.GetProperty("unitId").GetString() == unitId);

        Assert.Equal("resolved", incident.GetProperty("status").GetString());
        Assert.Equal("available", unit.GetProperty("status").GetString());
    }
}
