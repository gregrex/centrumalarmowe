using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Alarm112.Client.Runtime.Lobby;

public sealed class LobbyApiClient
{
    private readonly string _baseUrl;

    public LobbyApiClient(string baseUrl)
    {
        _baseUrl = baseUrl.TrimEnd('/');
    }

    public async Task<string> CreateDemoLobbyRawAsync()
    {
        using var request = UnityWebRequest.Post($"{_baseUrl}/api/lobbies/demo", string.Empty, "application/json");
        await request.SendWebRequest();
        return request.downloadHandler.text;
    }
}
