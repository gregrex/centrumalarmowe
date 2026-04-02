namespace Alarm112.Contracts;

public sealed record ReleaseNotesDraftDto(
    string Title,
    IReadOnlyList<ReleaseNotesSectionDto> Sections,
    string BuildId);
