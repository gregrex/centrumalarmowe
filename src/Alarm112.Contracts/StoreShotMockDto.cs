namespace Alarm112.Contracts;

public sealed record StoreShotMockDto(
    string Key,
    string Title,
    string Format,
    string SceneVariant,
    IReadOnlyList<string> FocusTags);
