namespace Alarm112.Application.Services;

public static class ContentValidationCatalog
{
    public static IReadOnlyList<string> RequiredDirectories { get; } =
    [
        "reference",
        "config",
        "content",
        "ui",
        "art",
        "audio"
    ];

    public static IReadOnlyList<string> RequiredFiles { get; } =
    [
        Path.Combine("reference", "reference-data.json"),
        Path.Combine("reference", "reference-data.extended.json"),
        Path.Combine("config", "roles.json"),
        Path.Combine("config", "bot_profiles.json"),
        Path.Combine("content", "home-hub.v1.json"),
        Path.Combine("ui", "home_cards.v1.json"),
        Path.Combine("art", "icon_catalog.json"),
        Path.Combine("art", "sprite_atlas_manifest.json"),
        Path.Combine("audio", "role_audio_priorities.json")
    ];

    public static IReadOnlyList<string> EnumerateCandidateFiles(string dataRoot)
    {
        var files = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var directory in RequiredDirectories)
        {
            var fullDirectory = Path.Combine(dataRoot, directory);
            if (!Directory.Exists(fullDirectory))
                continue;

            foreach (var file in Directory.EnumerateFiles(fullDirectory, "*.json", SearchOption.AllDirectories))
                files.Add(file);
        }

        foreach (var requiredFile in RequiredFiles.Select(file => Path.Combine(dataRoot, file)))
        {
            if (File.Exists(requiredFile))
                files.Add(requiredFile);
        }

        return files.ToList();
    }

    public static string ToRelativePath(string dataRoot, string path)
    {
        var relative = Path.GetRelativePath(dataRoot, path);
        return relative.Replace('\\', '/');
    }
}
