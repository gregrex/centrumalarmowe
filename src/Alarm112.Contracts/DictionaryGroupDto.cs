namespace Alarm112.Contracts;

public sealed record DictionaryGroupDto(
    string GroupId,
    IReadOnlyCollection<DictionaryEntryDto> Entries);
