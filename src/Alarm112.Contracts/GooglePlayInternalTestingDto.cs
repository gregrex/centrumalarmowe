namespace Alarm112.Contracts;

public sealed record GooglePlayInternalTestingDto(
    string Track,
    IReadOnlyList<GooglePlayChecklistItemDto> Items,
    string Notes);
