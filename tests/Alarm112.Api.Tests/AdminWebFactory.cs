using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Alarm112.Api.Tests;

public sealed class AdminWebFactory(string apiBaseUrl, string? apiSigningKey) : WebApplicationFactory<Alarm112.AdminWeb.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development");
        builder.UseSetting("ApiBaseUrl", apiBaseUrl);
        builder.UseSetting("AdminAuth:Username", "admin");
        builder.UseSetting("AdminAuth:Password", "test-admin-pass");
        builder.UseSetting("ApiAuth:Jwt:Issuer", "Alarm112.Api");
        builder.UseSetting("ApiAuth:Jwt:Audience", "Alarm112.Client");

        if (!string.IsNullOrWhiteSpace(apiSigningKey))
            builder.UseSetting("ApiAuth:Jwt:SigningKey", apiSigningKey);
    }
}
