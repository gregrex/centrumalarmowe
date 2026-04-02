namespace Alarm112.Contracts;

public sealed record RoundTimerDto(
    string MissionId,
    int TotalSeconds,
    int SecondsRemaining,
    string Phase,
    IReadOnlyList<int> Thresholds);
