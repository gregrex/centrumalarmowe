using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;

namespace Alarm112.Api.Tests;

/// <summary>
/// WebApplicationFactory variant with auth ENABLED for security testing.
/// </summary>
public sealed class Alarm112ApiFactoryWithAuth : WebApplicationFactory<Alarm112.Api.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var solutionRoot = FindSolutionRoot();
        builder.UseSetting("ContentBundles:DataRoot", Path.Combine(solutionRoot, "data"));
        builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development");
        builder.UseSetting("Security:RequireAuth", "true");
        builder.UseSetting("Security:EnableDevTokenEndpoint", "true");
        builder.UseSetting("Security:Jwt:SigningKey", "test-key-exactly-32-chars-minimum!!");
        builder.UseSetting("Security:Jwt:Issuer", "Alarm112.Api");
        builder.UseSetting("Security:Jwt:Audience", "Alarm112.Client");
    }

    private static string FindSolutionRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Alarm112.sln")))
                return dir.FullName;
            dir = dir.Parent!;
        }
        return AppContext.BaseDirectory;
    }
}
