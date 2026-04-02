namespace Alarm112.Contracts;

public sealed record MenuFlowDto(
    string DefaultScreen,
    IReadOnlyList<string> Screens,
    IReadOnlyList<string> PrimaryWidgets,
    IReadOnlyList<string> SecondaryWidgets);
