namespace Alarm112.Contracts;

public sealed record FinalCapturePackDto(
    IReadOnlyList<CaptureShotDto> Shots,
    string AudioProfile,
    string SafeFrame);
