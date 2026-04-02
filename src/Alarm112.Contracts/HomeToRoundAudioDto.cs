namespace Alarm112.Contracts;

public sealed record HomeToRoundAudioDto(
    string FromScreen,
    string ToScreen,
    string MusicStateId,
    string StingerId);
