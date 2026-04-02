namespace Alarm112.Contracts;

public sealed record SettingsBundleDto(
    IReadOnlyList<SettingsSectionDto> Sections);
