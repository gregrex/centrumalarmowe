using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Alarm112.Api.Tests;

public sealed class ContentEndpointTests(Alarm112ApiFactory factory)
    : IClassFixture<Alarm112ApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ReferenceData_Returns200()
    {
        var response = await _client.GetAsync("/api/reference-data");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ContentValidate_Returns200()
    {
        var response = await _client.GetAsync("/api/content/validate");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task HomeHub_Returns200()
    {
        var response = await _client.GetAsync("/api/home-hub");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CampaignChapters_Returns200()
    {
        var response = await _client.GetAsync("/api/campaign-chapters/demo");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task MissionBriefing_Returns200()
    {
        var response = await _client.GetAsync("/api/mission-briefing/demo");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CityMap_Returns200()
    {
        var response = await _client.GetAsync("/api/city-map");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task QuickPlayBootstrap_Returns200()
    {
        var response = await _client.GetAsync("/api/quickplay/bootstrap");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ThemePack_Returns200()
    {
        var response = await _client.GetAsync("/api/theme-pack");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task MenuFlow_Returns200()
    {
        var response = await _client.GetAsync("/api/menu-flow");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task RoleSelection_Returns200()
    {
        var response = await _client.GetAsync("/api/role-selection/demo");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task MissionRuntime_Returns200()
    {
        var response = await _client.GetAsync("/api/mission-runtime/demo");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostRoundReport_Returns200()
    {
        var response = await _client.GetAsync("/api/postround-report/demo");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
