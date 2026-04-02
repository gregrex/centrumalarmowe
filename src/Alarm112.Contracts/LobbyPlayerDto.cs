namespace Alarm112.Contracts;

public sealed record LobbyPlayerDto(
    string PlayerId,
    string DisplayName,
    string RoleId,
    bool IsBot,
    bool IsReady,
    bool IsConnected);
