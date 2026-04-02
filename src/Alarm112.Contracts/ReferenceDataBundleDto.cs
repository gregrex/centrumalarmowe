namespace Alarm112.Contracts;

public sealed record ReferenceDataBundleDto(
    string Version,
    IReadOnlyCollection<DictionaryGroupDto> Groups,
    IReadOnlyDictionary<string, string> Texts);
