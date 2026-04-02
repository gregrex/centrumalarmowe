using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Alarm112.Api.Tests;

public sealed class LobbyEndpointTests(Alarm112ApiFactory factory)
    : IClassFixture<Alarm112ApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateDemoLobby_Returns200()
    {
        var response = await _client.PostAsync("/api/lobbies/demo", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateDemoLobby_ReturnsLobbyId()
    {
        var response = await _client.PostAsync("/api/lobbies/demo", null);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var lobbyId = json.GetProperty("lobbyId").GetString();
        Assert.False(string.IsNullOrEmpty(lobbyId));
    }

    [Fact]
    public async Task GetLobby_ReturnsLobby()
    {
        var createResp = await _client.PostAsync("/api/lobbies/demo", null);
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var lobbyId = created.GetProperty("lobbyId").GetString()!;

        var getResp = await _client.GetAsync($"/api/lobbies/{lobbyId}");
        Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);
    }
}
