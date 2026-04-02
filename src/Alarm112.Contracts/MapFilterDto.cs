namespace Alarm112.Contracts;

public sealed record MapFilterDto(
    string FilterId,
    string LabelKey,
    string Category,
    bool EnabledByDefault,
    IReadOnlyCollection<string> Tags);
