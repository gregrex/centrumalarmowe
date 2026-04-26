using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Alarm112.Api.Tests;

public sealed class HealthEndpointTests(Alarm112ApiFactory factory)
    : IClassFixture<Alarm112ApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Health_Returns200()
    {
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Health_ReturnsOkTrue()
    {
        var response = await _client.GetAsync("/health");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(json.GetProperty("ok").GetBoolean());
    }

    [Fact]
    public async Task Health_ReturnsServiceName()
    {
        var response = await _client.GetAsync("/health");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Alarm112.Api", json.GetProperty("service").GetString());
    }

    [Fact]
    public async Task Health_ReturnsV26()
    {
        var response = await _client.GetAsync("/health");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("v26", json.GetProperty("version").GetString());
    }

    [Fact]
    public async Task HealthLive_ReturnsLiveStatus()
    {
        var response = await _client.GetAsync("/health/live");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("live", json.GetProperty("status").GetString());
    }

    [Fact]
    public async Task HealthReady_ReturnsReadinessChecks()
    {
        var response = await _client.GetAsync("/health/ready");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(json.GetProperty("ok").GetBoolean());
        Assert.Equal("ready", json.GetProperty("status").GetString());
        Assert.True(json.GetProperty("checks").GetArrayLength() >= 3);
    }
}
