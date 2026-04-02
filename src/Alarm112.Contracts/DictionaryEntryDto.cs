namespace Alarm112.Contracts;

public sealed record DictionaryEntryDto(
    string Id,
    string Category,
    string Value,
    int Version);
