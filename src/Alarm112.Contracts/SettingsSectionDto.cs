namespace Alarm112.Contracts;

public sealed record SettingsSectionDto(
    string Id,
    string LabelKey,
    IReadOnlyList<string> Options);
