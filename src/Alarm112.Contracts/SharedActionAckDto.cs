namespace Alarm112.Contracts;

public sealed record SharedActionAckDto(
    string SharedActionId,
    string Role,
    bool Accepted,
    bool IsBot,
    string ResultCode);
