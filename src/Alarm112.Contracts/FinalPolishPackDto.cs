namespace Alarm112.Contracts;

public sealed record FinalPolishPackDto(
    string Scope,
    IReadOnlyList<string> VisualFocus,
    IReadOnlyList<string> AudioFocus,
    IReadOnlyList<string> UiFocus,
    string RecommendedNextAction);
