namespace Alarm112.Contracts;

public sealed record RolePanelStateDto(
    string RoleId,
    string LayoutKey,
    bool IsHuman,
    bool IsActive,
    string FocusIncidentId,
    int UnreadCount,
    int PrimaryCounter,
    string AlertState);
