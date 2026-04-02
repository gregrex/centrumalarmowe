using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface ILobbyService
{
    Task<LobbyDto> CreateDemoLobbyAsync(CancellationToken cancellationToken);
    Task<LobbyDto> GetLobbyAsync(string lobbyId, CancellationToken cancellationToken);
}
