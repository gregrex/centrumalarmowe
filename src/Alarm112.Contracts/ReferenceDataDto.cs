namespace Alarm112.Contracts;

public sealed record ReferenceDataDto(
    IReadOnlyCollection<DictionaryEntryDto> Entries,
    IReadOnlyDictionary<string, string> Texts);
