namespace Alarm112.Contracts;

public sealed record FinalPromoMediaDto(
    IReadOnlyList<StoreShotMockDto> Shots,
    bool CaptureModeEnabled,
    IReadOnlyList<string> Tags);
