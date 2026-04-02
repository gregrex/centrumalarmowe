namespace Alarm112.Contracts;

public sealed record RuntimeScoreboardRowDto(
    string Id,
    string LabelKey,
    string DisplayValue,
    string Tier);
