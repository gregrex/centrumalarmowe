namespace Alarm112.Contracts;

public sealed record ActiveIncidentBoardDto(
    string SessionId,
    int ActiveCount,
    int CriticalCount,
    IReadOnlyCollection<ActiveIncidentDto> Items);
