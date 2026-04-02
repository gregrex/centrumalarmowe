namespace Alarm112.Contracts;

public sealed record ThemePackDto(
    string ActiveThemeId,
    string ActiveHeroObjectId,
    string ActiveAudioStateId,
    IReadOnlyList<string> AvailableThemes,
    IReadOnlyList<string> AvailableHeroObjects);
