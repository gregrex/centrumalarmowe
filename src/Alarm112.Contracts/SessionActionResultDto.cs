namespace Alarm112.Contracts;

public sealed record SessionActionResultDto(bool Success, string SessionId, string ActionType, string Message);
