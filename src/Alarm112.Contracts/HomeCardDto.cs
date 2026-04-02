namespace Alarm112.Contracts;

public sealed record HomeCardDto(
    string Id,
    string Type,
    string LabelKey,
    string State,
    string Route);
