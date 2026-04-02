using System.Net;

namespace Alarm112.Api.Tests;

/// <summary>
/// Tests verifying that security response headers are present on all API responses.
/// </summary>
public sealed class SecurityHeaderTests(Alarm112ApiFactory factory)
    : IClassFixture<Alarm112ApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Health_Response_HasXFrameOptions()
    {
        var response = await _client.GetAsync("/health");
        Assert.True(response.Headers.Contains("X-Frame-Options"),
            "X-Frame-Options header is missing.");
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").First());
    }

    [Fact]
    public async Task Health_Response_HasXContentTypeOptions()
    {
        var response = await _client.GetAsync("/health");
        Assert.True(response.Headers.Contains("X-Content-Type-Options"),
            "X-Content-Type-Options header is missing.");
        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").First());
    }

    [Fact]
    public async Task Health_Response_HasReferrerPolicy()
    {
        var response = await _client.GetAsync("/health");
        Assert.True(response.Headers.Contains("Referrer-Policy"),
            "Referrer-Policy header is missing.");
    }

    [Fact]
    public async Task Health_Response_HasXXssProtection()
    {
        var response = await _client.GetAsync("/health");
        Assert.True(response.Headers.Contains("X-XSS-Protection"),
            "X-XSS-Protection header is missing.");
    }

    [Fact]
    public async Task Health_Response_HasPermissionsPolicy()
    {
        var response = await _client.GetAsync("/health");
        Assert.True(response.Headers.Contains("Permissions-Policy"),
            "Permissions-Policy header is missing.");
    }

    [Fact]
    public async Task ApiEndpoint_Response_HasSecurityHeaders()
    {
        var response = await _client.GetAsync("/api/reference-data");
        Assert.True(response.Headers.Contains("X-Frame-Options"),
            "X-Frame-Options missing from API response.");
        Assert.True(response.Headers.Contains("X-Content-Type-Options"),
            "X-Content-Type-Options missing from API response.");
    }
}
