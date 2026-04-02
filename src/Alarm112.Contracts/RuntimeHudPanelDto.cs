namespace Alarm112.Contracts;

public sealed record RuntimeHudPanelDto(
    string Id,
    string PanelType,
    string Priority,
    string Anchor,
    bool Visible);