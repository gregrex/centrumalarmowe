using System.IO;
using UnityEngine;

namespace Alarm112.Client.Runtime.Config;

public static class JsonConfigLoader
{
    public static string LoadTextAssetOrFile(TextAsset? asset, string fallbackPath)
    {
        if (asset != null)
        {
            return asset.text;
        }

        return File.Exists(fallbackPath)
            ? File.ReadAllText(fallbackPath)
            : "{}";
    }
}
