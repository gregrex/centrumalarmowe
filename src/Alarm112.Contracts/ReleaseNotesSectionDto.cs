namespace Alarm112.Contracts;

public sealed record ReleaseNotesSectionDto(
    string Id,
    string Title,
    IReadOnlyList<string> Items);
