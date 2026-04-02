namespace Alarm112.Contracts;

public sealed record LobbyDto(
    string LobbyId,
    string LobbyCode,
    string State,
    string ScenarioId,
    IReadOnlyCollection<LobbyPlayerDto> Players,
    bool BotFillEnabled);
