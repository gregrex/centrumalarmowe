using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Alarm112.Api.Tests;

/// <summary>
/// Tests for the paginated sessions list endpoint: GET /api/sessions?page=&pageSize=
/// </summary>
public sealed class PaginationTests(Alarm112ApiFactory factory)
    : IClassFixture<Alarm112ApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetSessions_Returns200()
    {
        var response = await _client.GetAsync("/api/sessions");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetSessions_HasPaginationShape()
    {
        var response = await _client.GetAsync("/api/sessions?page=1&pageSize=10");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.True(json.TryGetProperty("page", out _), "Missing 'page' field.");
        Assert.True(json.TryGetProperty("pageSize", out _), "Missing 'pageSize' field.");
        Assert.True(json.TryGetProperty("totalCount", out _), "Missing 'totalCount' field.");
        Assert.True(json.TryGetProperty("totalPages", out _), "Missing 'totalPages' field.");
        Assert.True(json.TryGetProperty("items", out var items), "Missing 'items' field.");
        Assert.Equal(JsonValueKind.Array, items.ValueKind);
    }

    [Fact]
    public async Task GetSessions_DefaultPageSizeIs20()
    {
        var response = await _client.GetAsync("/api/sessions");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(20, json.GetProperty("pageSize").GetInt32());
    }

    [Fact]
    public async Task GetSessions_RespectsCustomPageSize()
    {
        // Create some demo sessions first to have data
        await _client.PostAsync("/api/sessions/demo", null);
        await _client.PostAsync("/api/sessions/demo", null);

        var response = await _client.GetAsync("/api/sessions?page=1&pageSize=1");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(1, json.GetProperty("pageSize").GetInt32());
        // Items array should have at most pageSize elements
        var itemCount = json.GetProperty("items").GetArrayLength();
        Assert.True(itemCount <= 1, $"Expected at most 1 item, got {itemCount}.");
    }

    [Fact]
    public async Task GetSessions_PageSizeCappedAt100()
    {
        var response = await _client.GetAsync("/api/sessions?pageSize=999");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(json.GetProperty("pageSize").GetInt32() <= 100,
            "pageSize should be capped at 100.");
    }

    [Fact]
    public async Task GetSessions_NegativePageDefaultsTo1()
    {
        var response = await _client.GetAsync("/api/sessions?page=-5");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(1, json.GetProperty("page").GetInt32());
    }

    [Fact]
    public async Task GetSessions_AfterCreatingSession_TotalCountIncreases()
    {
        var before = await _client.GetAsync("/api/sessions");
        var beforeJson = await before.Content.ReadFromJsonAsync<JsonElement>();
        var countBefore = beforeJson.GetProperty("totalCount").GetInt32();

        // Create a new session
        await _client.PostAsync("/api/sessions/demo", null);

        var after = await _client.GetAsync("/api/sessions");
        var afterJson = await after.Content.ReadFromJsonAsync<JsonElement>();
        var countAfter = afterJson.GetProperty("totalCount").GetInt32();

        Assert.True(countAfter > countBefore,
            $"Expected totalCount to increase after creating a session. Was {countBefore}, still {countAfter}.");
    }
}
