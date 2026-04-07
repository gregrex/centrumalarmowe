using System.Collections.Concurrent;
using System.Text.Json;
using Alarm112.Application.Interfaces;

namespace Alarm112.Application.Services;

public sealed class JsonContentBundleLoader : IContentBundleLoader
{
    private readonly ConcurrentDictionary<string, object> _cache = new();

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public JsonContentBundleLoader(string dataRoot)
    {
        DataRoot = dataRoot;
    }

    public string DataRoot { get; }

    public Task<T> LoadContentAsync<T>(string fileName, CancellationToken cancellationToken = default)
        => LoadAsync<T>(Path.Combine(DataRoot, "content", fileName), cancellationToken);

    public Task<T> LoadConfigAsync<T>(string fileName, CancellationToken cancellationToken = default)
        => LoadAsync<T>(Path.Combine(DataRoot, "config", fileName), cancellationToken);

    public Task<T> LoadReferenceAsync<T>(string fileName, CancellationToken cancellationToken = default)
        => LoadAsync<T>(Path.Combine(DataRoot, "reference", fileName), cancellationToken);

    private async Task<T> LoadAsync<T>(string fullPath, CancellationToken cancellationToken)
    {
        var key = fullPath.ToLowerInvariant();

        if (_cache.TryGetValue(key, out var cached))
            return (T)cached;

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Content bundle not found: {fullPath}");

        await using var stream = File.OpenRead(fullPath);
        var result = await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions, cancellationToken)
            ?? throw new InvalidOperationException($"Failed to deserialize {fullPath} as {typeof(T).Name}");

        _cache.TryAdd(key, result);
        return result;
    }
}
