using Alarm112.Application.Interfaces;
using Alarm112.Application.Services;
using Alarm112.Infrastructure.Persistence;

namespace Alarm112.Api;

internal static class OperationalHealth
{
    private const string ServiceName = "Alarm112.Api";
    private const string Version = "v26";

    public static ApiHealthReportDto CreateLivenessReport()
        => new(
            true,
            ServiceName,
            Version,
            "live",
            null,
            DateTimeOffset.UtcNow,
            []);

    public static ApiHealthReportDto CreateReadinessReport(ISessionStore store, IContentBundleLoader contentBundleLoader)
    {
        var checks = new List<ApiHealthCheckDto>();
        var dataRoot = contentBundleLoader.DataRoot;

        checks.Add(BuildContentRootCheck(dataRoot));
        checks.Add(BuildContentDirectoriesCheck(dataRoot));
        checks.Add(BuildContentFilesCheck(dataRoot));
        checks.Add(BuildSessionStoreCheck(store));

        var ok = checks.All(check => string.Equals(check.Status, "ok", StringComparison.OrdinalIgnoreCase));

        return new ApiHealthReportDto(
            ok,
            ServiceName,
            Version,
            ok ? "ready" : "degraded",
            store.GetType().Name,
            DateTimeOffset.UtcNow,
            checks);
    }

    private static ApiHealthCheckDto BuildContentRootCheck(string dataRoot)
    {
        var ok = Directory.Exists(dataRoot);
        return new ApiHealthCheckDto(
            "content-root",
            ok ? "ok" : "error",
            "data",
            ok ? "Content root available." : "Configured data root does not exist.");
    }

    private static ApiHealthCheckDto BuildContentDirectoriesCheck(string dataRoot)
    {
        var missingDirectories = ContentValidationCatalog.RequiredDirectories
            .Where(directory => !Directory.Exists(Path.Combine(dataRoot, directory)))
            .ToArray();

        return new ApiHealthCheckDto(
            "content-directories",
            missingDirectories.Length == 0 ? "ok" : "error",
            "critical-json-directories",
            missingDirectories.Length == 0
                ? "All critical content directories are present."
                : $"Missing directories: {string.Join(", ", missingDirectories)}");
    }

    private static ApiHealthCheckDto BuildContentFilesCheck(string dataRoot)
    {
        var missingFiles = ContentValidationCatalog.RequiredFiles
            .Where(file => !File.Exists(Path.Combine(dataRoot, file)))
            .Select(file => file.Replace('\\', '/'))
            .ToArray();

        return new ApiHealthCheckDto(
            "content-bundles",
            missingFiles.Length == 0 ? "ok" : "error",
            "critical-json-files",
            missingFiles.Length == 0
                ? "All required seed bundles are present."
                : $"Missing files: {string.Join(", ", missingFiles)}");
    }

    private static ApiHealthCheckDto BuildSessionStoreCheck(ISessionStore store)
    {
        var storeName = store.GetType().Name;
        if (store is PostgresSessionStore postgresStore)
        {
            var ok = postgresStore.CanConnect(out var error);
            return new ApiHealthCheckDto(
                "session-store",
                ok ? "ok" : "error",
                storeName,
                ok ? "Database connectivity verified." : $"Database connectivity failed: {error}");
        }

        return new ApiHealthCheckDto(
            "session-store",
            "ok",
            storeName,
            "In-memory store available.");
    }
}

internal sealed record ApiHealthReportDto(
    bool Ok,
    string Service,
    string Version,
    string Status,
    string? Store,
    DateTimeOffset Utc,
    IReadOnlyList<ApiHealthCheckDto> Checks);

internal sealed record ApiHealthCheckDto(
    string Name,
    string Status,
    string Target,
    string Message);
