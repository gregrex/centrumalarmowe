using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Alarm112.Client.Runtime.UI.Theme;

public sealed class IconCatalogLoader
{
    public Dictionary<string, string> AssetToPath { get; } = new();

    public void LoadFromJson(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning($"[IconCatalogLoader] Missing file: {path}");
            return;
        }

        var json = File.ReadAllText(path);
        var wrapper = JsonUtility.FromJson<IconCatalogWrapper>(json);
        if (wrapper == null || wrapper.icons == null)
        {
            return;
        }

        AssetToPath.Clear();
        foreach (var icon in wrapper.icons)
        {
            AssetToPath[icon.assetId] = icon.placeholderPath;
        }
    }

    [System.Serializable]
    private sealed class IconCatalogWrapper
    {
        public IconCatalogItem[] icons;
    }

    [System.Serializable]
    private sealed class IconCatalogItem
    {
        public string assetId;
        public string placeholderPath;
    }
}
