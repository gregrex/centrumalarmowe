using System.Text.Json;
using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class ContentValidationService : IContentValidationService
{
    private readonly IContentBundleLoader _contentBundleLoader;

    public ContentValidationService(IContentBundleLoader contentBundleLoader)
    {
        _contentBundleLoader = contentBundleLoader;
    }

    public async Task<ContentValidationResultDto> ValidateAsync(CancellationToken cancellationToken)
    {
        var issues = new List<ContentValidationIssueDto>();
        var dataRoot = _contentBundleLoader.DataRoot;
        var files = new[]
        {
            Path.Combine(dataRoot, "reference", "reference-data.json"),
            Path.Combine(dataRoot, "reference", "reference-data.extended.json"),
            Path.Combine(dataRoot, "art", "icon_catalog.json"),
            Path.Combine(dataRoot, "art", "sprite_atlas_manifest.json")
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
}
