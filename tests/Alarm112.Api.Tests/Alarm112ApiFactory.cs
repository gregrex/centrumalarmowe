using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace Alarm112.Api.Tests;

/// <summary>
/// Custom WebApplicationFactory that overrides ContentRootPath
/// so the API can locate data/ bundles during test runs.
/// </summary>
public sealed class Alarm112ApiFactory : WebApplicationFactory<Alarm112.Api.Program>
{
    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        // Point DataRoot to the solution's data/ folder
        var solutionRoot = FindSolutionRoot();
        builder.UseSetting("ContentBundles:DataRoot", Path.Combine(solutionRoot, "data"));
        builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development");
        // Explicitly disable auth for tests
        builder.UseSetting("Security:RequireAuth", "false");
        // Provide a test JWT signing key (appsettings.json has empty key — production pattern)
        builder.UseSetting("Security:Jwt:SigningKey", "test-key-exactly-32-chars-minimum!!");
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
