namespace Alarm112.Contracts;

public sealed record HomeHubDto(
    string DefaultScreen,
    string? ContinueSessionId,
    string ContinueSummary,
    IReadOnlyList<HomeCardDto> Cards);
