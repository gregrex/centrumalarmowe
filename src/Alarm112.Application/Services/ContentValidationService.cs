using System.Text.Json;
using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class ContentValidationService : IContentValidationService
{
    private readonly IContentBundleLoader _contentBundleLoader;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public ContentValidationService(IContentBundleLoader contentBundleLoader)
    {
        _contentBundleLoader = contentBundleLoader;
    }

    public async Task<ContentValidationResultDto> ValidateAsync(CancellationToken cancellationToken)
    {
        var issues = new List<ContentValidationIssueDto>();
        var dataRoot = _contentBundleLoader.DataRoot;

        if (!Directory.Exists(dataRoot))
        {
            issues.Add(new("Error", "data", "Data root missing."));
            return new ContentValidationResultDto(false, issues);
        }

        foreach (var directory in ContentValidationCatalog.RequiredDirectories)
        {
            var fullDirectory = Path.Combine(dataRoot, directory);
            if (!Directory.Exists(fullDirectory))
                issues.Add(new("Error", directory, "Directory missing."));
        }

        foreach (var requiredFile in ContentValidationCatalog.RequiredFiles)
        {
            var file = Path.Combine(dataRoot, requiredFile);
            if (!File.Exists(file))
                issues.Add(new("Error", ContentValidationCatalog.ToRelativePath(dataRoot, file), "File missing."));
        }

        foreach (var file in ContentValidationCatalog.EnumerateCandidateFiles(dataRoot))
        {
            var relativePath = ContentValidationCatalog.ToRelativePath(dataRoot, file);

            try
            {
                await using var stream = File.OpenRead(file);
                _ = await JsonSerializer.DeserializeAsync<JsonElement>(
                    stream,
                    JsonOptions,
                    cancellationToken);
            }
            catch (JsonException ex)
            {
                issues.Add(new("Error", relativePath, $"Invalid JSON: {ex.Message}"));
            }
        }

        return new ContentValidationResultDto(issues.Count == 0, issues);
    }
}
