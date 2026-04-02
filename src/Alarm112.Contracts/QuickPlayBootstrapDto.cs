
namespace Alarm112.Contracts;

public sealed record QuickPlayBootstrapDto(
    string ScenarioId,
    string Difficulty,
    string PreferredRole,
    bool AutoFillBots,
    IReadOnlyCollection<string> IncidentIds,
    IReadOnlyCollection<string> RecommendedRoles);
