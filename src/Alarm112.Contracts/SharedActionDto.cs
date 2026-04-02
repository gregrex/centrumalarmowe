namespace Alarm112.Contracts;

public sealed record SharedActionDto(
    string SharedActionId,
    string IncidentId,
    string ActionType,
    string RequestedByRole,
    IReadOnlyCollection<string> RequiredRoles,
    int TimeoutSeconds,
    bool AllowBotAssist);
