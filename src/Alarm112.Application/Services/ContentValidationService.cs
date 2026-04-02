using System.Text.Json;
using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class ContentValidationService : IContentValidationService
{
    public async Task<ContentValidationResultDto> ValidateAsync(CancellationToken cancellationToken)
    {
        var issues = new List<ContentValidationIssueDto>();
        var root = FindProjectRoot();
        var files = new[]
        {
            Path.Combine(root, "data", "reference", "reference-data.json"),
            Path.Combine(root, "data", "reference", "reference-data.extended.json"),
            Path.Combine(root, "data", "art", "icon_catalog.json"),
            Path.Combine(root, "data", "art", "sprite_atlas_manifest.json")
        };

        foreach (var file in files)
        {
            if (!File.Exists(file))
            {
                issues.Add(new("Error", file, "File missing."));
                continue;
            }

            try
            {
                await using var stream = File.OpenRead(file);
                _ = await JsonSerializer.DeserializeAsync<JsonElement>(stream, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                issues.Add(new("Error", file, $"Invalid JSON: {ex.Message}"));
            }
        }

        return new ContentValidationResultDto(issues.Count == 0, issues);
    }

    private static string FindProjectRoot()
    {
        var current = AppContext.BaseDirectory;
        var dir = new DirectoryInfo(current);

        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Alarm112.sln")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        return Directory.GetCurrentDirectory();
    }
}
