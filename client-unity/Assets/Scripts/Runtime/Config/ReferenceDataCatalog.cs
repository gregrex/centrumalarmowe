using System.Collections.Generic;

namespace Alarm112.Client.Runtime.Config;

public sealed class ReferenceDataCatalog
{
    public Dictionary<string, string> Texts { get; } = new();
    public Dictionary<string, string> Values { get; } = new();

    public string ResolveText(string key)
    {
        return Texts.TryGetValue(key, out var value) ? value : key;
    }
}
