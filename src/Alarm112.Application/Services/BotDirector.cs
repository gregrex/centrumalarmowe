using System.Text.Json;
using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

/// <summary>
/// Loads bot profiles from data/config/bot_profiles.json and executes bot actions
/// for any role slots that have no human player in the given session.
/// </summary>
public sealed class BotDirector : IBotDirector
{
    private readonly ISessionStore _store;
    private readonly ISessionService _sessionService;
    private readonly IContentBundleLoader _loader;
    private BotProfileConfig[]? _profiles;

    public BotDirector(ISessionStore store, ISessionService sessionService, IContentBundleLoader loader)
    {
        _store = store;
        _sessionService = sessionService;
        _loader = loader;
    }

    public async Task ExecuteBotTickAsync(string sessionId, CancellationToken cancellationToken)
    {
        var profiles = await GetProfilesAsync(cancellationToken);
        if (profiles.Length == 0) return;

        var snapshot = _store.GetOrAdd(sessionId, _ => null!);
        if (snapshot is null) return;

        var botRoles = snapshot.Roles.Where(r => !r.HasHuman && r.HasBot).ToList();
        if (botRoles.Count == 0) return;

        var profile = profiles[Math.Abs(sessionId.GetHashCode()) % profiles.Length];

        foreach (var role in botRoles)
        {
            cancellationToken.ThrowIfCancellationRequested();

            snapshot = _store.TryGet(sessionId) ?? snapshot;
            if (CreateBotAction(sessionId, role.Role, profile, snapshot) is not { } action)
                continue;

            await _sessionService.ApplyActionAsync(sessionId, action, cancellationToken);
        }
    }

    private async Task<BotProfileConfig[]> GetProfilesAsync(CancellationToken cancellationToken)
    {
        if (_profiles is not null) return _profiles;
        var raw = await _loader.LoadConfigAsync<BotProfilesJson>("bot_profiles.json", cancellationToken);
        _profiles = raw?.Profiles ?? [];
        return _profiles;
    }

    private static SessionActionDto? CreateBotAction(
        string sessionId,
        string role,
        BotProfileConfig profile,
        SessionSnapshotDto snapshot)
    {
        var pendingIncident = snapshot.Incidents.FirstOrDefault(i => string.Equals(i.Status, "pending", StringComparison.OrdinalIgnoreCase));
        var availableUnit = snapshot.Units.FirstOrDefault(u => string.Equals(u.Status, "available", StringComparison.OrdinalIgnoreCase));

        if (pendingIncident is not null && availableUnit is not null)
        {
            return BuildAction(
                sessionId,
                role,
                "dispatch",
                new { incidentId = pendingIncident.IncidentId, unitId = availableUnit.UnitId, botProfileId = profile.Id });
        }

        var dispatchedIncident = snapshot.Incidents.FirstOrDefault(i =>
            string.Equals(i.Status, "dispatched", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(i.Status, "escalated", StringComparison.OrdinalIgnoreCase));
        var engagedUnit = snapshot.Units.FirstOrDefault(u => string.Equals(u.Status, "dispatched", StringComparison.OrdinalIgnoreCase));

        if (dispatchedIncident is not null && engagedUnit is not null)
        {
            return BuildAction(
                sessionId,
                role,
                "resolve",
                new { incidentId = dispatchedIncident.IncidentId, unitId = engagedUnit.UnitId, botProfileId = profile.Id });
        }

        if (pendingIncident is not null)
        {
            return BuildAction(
                sessionId,
                role,
                "escalate",
                new { incidentId = pendingIncident.IncidentId, botProfileId = profile.Id });
        }

        return null;
    }

    private static SessionActionDto BuildAction(string sessionId, string role, string actionType, object payload) =>
        new()
        {
            SessionId = sessionId,
            ActorId = $"bot:{role}",
            Role = role,
            ActionType = actionType,
            PayloadJson = JsonSerializer.Serialize(payload),
            CorrelationId = Guid.NewGuid().ToString("N")
        };

    private sealed record BotProfilesJson(BotProfileConfig[] Profiles);
    private sealed record BotProfileConfig(string Id, string Style, int CooldownMs, double RiskTolerance);
}
