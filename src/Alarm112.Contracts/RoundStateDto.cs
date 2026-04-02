namespace Alarm112.Contracts;

public sealed record RoundStateDto(
    string SessionId,
    string RoundId,
    int Tick,
    int ElapsedSeconds,
    string Phase,
    int ActiveHumanPlayers,
    int ActiveBotPlayers,
    int OpenIncidents,
    int SharedActionsPending,
    IReadOnlyCollection<RolePanelStateDto> RolePanels);
