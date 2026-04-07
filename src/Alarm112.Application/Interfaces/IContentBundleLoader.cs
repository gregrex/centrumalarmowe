namespace Alarm112.Application.Interfaces;

/// <summary>
/// Loads and caches typed content bundles from JSON files in the data directory.
/// </summary>
public interface IContentBundleLoader
{
    string DataRoot { get; }

    /// <summary>Loads a file from data/content/ by filename (e.g. "home-hub.v1.json").</summary>
    Task<T> LoadContentAsync<T>(string fileName, CancellationToken cancellationToken = default);

    /// <summary>Loads a file from data/config/ by filename (e.g. "roles.json").</summary>
    Task<T> LoadConfigAsync<T>(string fileName, CancellationToken cancellationToken = default);

    /// <summary>Loads a file from data/reference/ by filename (e.g. "reference-data.json").</summary>
    Task<T> LoadReferenceAsync<T>(string fileName, CancellationToken cancellationToken = default);
}
