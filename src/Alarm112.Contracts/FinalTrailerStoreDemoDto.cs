namespace Alarm112.Contracts;

public sealed record FinalTrailerStoreDemoDto(
    IReadOnlyList<CaptureShotDto> Shots,
    string AudioLock,
    string PresentationState);
