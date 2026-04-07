using System.Text.Json;
using Alarm112.Application.Services;

namespace Alarm112.Api.Tests;

public sealed class ContentValidationServiceTests
{
    [Fact]
    public async Task ValidateAsync_UsesConfiguredDataRoot()
    {
        var dataRoot = CreateTempDataRoot();

        try
        {
            WriteJson(Path.Combine(dataRoot, "reference", "reference-data.json"), new { ok = true });
            WriteJson(Path.Combine(dataRoot, "reference", "reference-data.extended.json"), new { ok = true });
            WriteJson(Path.Combine(dataRoot, "art", "icon_catalog.json"), new { icons = Array.Empty<object>() });
            WriteJson(Path.Combine(dataRoot, "art", "sprite_atlas_manifest.json"), new { atlases = Array.Empty<object>() });

            var loader = new JsonContentBundleLoader(dataRoot);
            var service = new ContentValidationService(loader);

            var result = await service.ValidateAsync(default);

            Assert.True(result.IsValid);
            Assert.Empty(result.Issues);
        }
        finally
        {
            Directory.Delete(dataRoot, recursive: true);
        }
    }

    [Fact]
    public async Task ValidateAsync_MissingFiles_ReportConfiguredDataRootPaths()
    {
        var dataRoot = CreateTempDataRoot();

        try
        {
            var loader = new JsonContentBundleLoader(dataRoot);
            var service = new ContentValidationService(loader);

            var result = await service.ValidateAsync(default);

            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Issues);
            Assert.All(result.Issues, issue => Assert.StartsWith(dataRoot, issue.Source, StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            Directory.Delete(dataRoot, recursive: true);
        }
    }

    private static string CreateTempDataRoot()
    {
        var root = Path.Combine(Path.GetTempPath(), "alarm112-content-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(root, "reference"));
        Directory.CreateDirectory(Path.Combine(root, "art"));
        return root;
    }

    private static void WriteJson(string path, object payload)
    {
        File.WriteAllText(path, JsonSerializer.Serialize(payload));
    }
}
