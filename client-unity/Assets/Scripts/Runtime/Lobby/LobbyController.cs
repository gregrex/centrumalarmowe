using UnityEngine;

namespace Alarm112.Client.Runtime.Lobby;

public sealed class LobbyController : MonoBehaviour
{
    [SerializeField] private string _apiBaseUrl = "http://localhost:8080";

    public async void CreateDemoLobby()
    {
        var client = new LobbyApiClient(_apiBaseUrl);
        var raw = await client.CreateDemoLobbyRawAsync();
        Debug.Log($"[LobbyController] Demo lobby payload: {raw}");
    }
}
