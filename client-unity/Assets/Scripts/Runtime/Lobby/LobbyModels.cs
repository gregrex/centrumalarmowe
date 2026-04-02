using System;
using System.Collections.Generic;

namespace Alarm112.Client.Runtime.Lobby;

[Serializable]
public sealed class LobbyDto
{
    public string LobbyId = string.Empty;
    public string LobbyCode = string.Empty;
    public string State = string.Empty;
    public string ScenarioId = string.Empty;
    public bool BotFillEnabled;
    public List<LobbyPlayerDto> Players = new();
}

[Serializable]
public sealed class LobbyPlayerDto
{
    public string PlayerId = string.Empty;
    public string DisplayName = string.Empty;
    public string RoleId = string.Empty;
    public bool IsBot;
    public bool IsReady;
    public bool IsConnected;
}
