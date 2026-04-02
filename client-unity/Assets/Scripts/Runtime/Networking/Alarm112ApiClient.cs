using System.Net.Http;
using System.Threading.Tasks;

namespace Alarm112.Client.Runtime.Networking;

public sealed class Alarm112ApiClient
{
    private readonly HttpClient _httpClient;

    public Alarm112ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> CreateDemoSessionAsync()
    {
        var response = await _httpClient.PostAsync("/api/sessions/demo", null);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetCityMapAsync()
    {
        var response = await _httpClient.GetAsync("/api/city-map");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetTimelineAsync(string sessionId)
    {
        var response = await _httpClient.GetAsync($"/api/sessions/{sessionId}/timeline");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> DispatchRawAsync(string sessionId, string json)
    {
        using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"/api/sessions/{sessionId}/dispatch", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
