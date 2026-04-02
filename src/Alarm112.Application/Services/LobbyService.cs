using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class LobbyService : ILobbyService
{
    public Task<LobbyDto> CreateDemoLobbyAsync(CancellationToken cancellationToken)
    {
        var lobby = Build("LOBBY-DEMO");
        return Task.FromResult(lobby);
    }

    public Task<LobbyDto> GetLobbyAsync(string lobbyId, CancellationToken cancellationToken)
    {
        var lobby = Build(lobbyId);
        return Task.FromResult(lobby);
    }

    private static LobbyDto Build(string lobbyId)
    {
        IReadOnlyCollection<LobbyPlayerDto> players =
        [
            new("player.local", "Gracz lokalny", "role.operator", false, true, true),
            new("bot.dispatcher", "BOT Dispatcher", "role.dispatcher", true, true, true),
            new("bot.coordinator", "BOT Coordinator", "role.coordinator", true, true, true),
            new("bot.crisis", "BOT Crisis", "role.crisis_officer", true, true, true)
        ];

        return new LobbyDto(
            lobbyId,
            "112ABC",
            "WaitingForPlayers",
            "scenario.verticalslice.quickplay",
            players,
            true);
    }
}
