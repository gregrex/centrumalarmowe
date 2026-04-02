namespace Alarm112.Contracts;

public sealed record RuntimeDispatchOutcomeDto(
    string IncidentId,
    string Outcome,
    int ScoreDelta,
    int CityStabilityDelta,
    string AudioState);
