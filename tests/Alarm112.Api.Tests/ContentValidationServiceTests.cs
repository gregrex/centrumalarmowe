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
            WriteBaselineBundles(dataRoot);

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
            Assert.Contains(result.Issues, issue => issue.Source == "reference/reference-data.json");
            Assert.Contains(result.Issues, issue => issue.Source == "config/roles.json");
        }
        finally
        {
            Directory.Delete(dataRoot, recursive: true);
        }
    }

    [Fact]
    public async Task ValidateAsync_InvalidJsonFile_IsReportedWithRelativePath()
    {
        var dataRoot = CreateTempDataRoot();

        try
        {
            WriteBaselineBundles(dataRoot);
            File.WriteAllText(Path.Combine(dataRoot, "content", "broken.json"), "{ invalid ");

            var loader = new JsonContentBundleLoader(dataRoot);
            var service = new ContentValidationService(loader);

            var result = await service.ValidateAsync(default);

            Assert.False(result.IsValid);
            Assert.Contains(result.Issues, issue => issue.Source == "content/broken.json");
            Assert.Contains(result.Issues, issue => issue.Message.StartsWith("Invalid JSON:", StringComparison.Ordinal));
        }
        finally
        {
            Directory.Delete(dataRoot, recursive: true);
        }
    }

    private static string CreateTempDataRoot()
    {
        var root = Path.Combine(Path.GetTempPath(), "alarm112-content-" + Guid.NewGuid().ToString("N"));
        foreach (var directory in ContentValidationCatalog.RequiredDirectories)
            Directory.CreateDirectory(Path.Combine(root, directory));
        return root;
    }

    private static void WriteBaselineBundles(string dataRoot)
    {
        WriteJson(Path.Combine(dataRoot, "reference", "reference-data.json"), new { ok = true });
        WriteJson(Path.Combine(dataRoot, "reference", "reference-data.extended.json"), new { ok = true });
        WriteJson(Path.Combine(dataRoot, "config", "roles.json"), new { roles = Array.Empty<object>() });
        WriteJson(Path.Combine(dataRoot, "config", "bot_profiles.json"), new { profiles = Array.Empty<object>() });
        WriteJson(Path.Combine(dataRoot, "content", "home-hub.v1.json"), new { cards = Array.Empty<object>() });
        WriteJson(Path.Combine(dataRoot, "ui", "home_cards.v1.json"), new { cards = Array.Empty<object>() });
        WriteJson(Path.Combine(dataRoot, "art", "icon_catalog.json"), new { icons = Array.Empty<object>() });
        WriteJson(Path.Combine(dataRoot, "art", "sprite_atlas_manifest.json"), new { atlases = Array.Empty<object>() });
        WriteJson(Path.Combine(dataRoot, "audio", "role_audio_priorities.json"), new { priorities = Array.Empty<object>() });
    }

    private static void WriteJson(string path, object payload)
    {
        File.WriteAllText(path, JsonSerializer.Serialize(payload));
    }
}
