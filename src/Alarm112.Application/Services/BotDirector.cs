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
            // Pick a pending incident and simulate a dispatch action
            var incident = snapshot.Incidents.FirstOrDefault(i => i.Status == "pending");
            if (incident is null) continue;

            var unit = snapshot.Units.FirstOrDefault(u => u.Status == "available");
            if (unit is null) continue;

            var action = new SessionActionDto
            {
                SessionId = sessionId,
                ActorId = $"bot:{role.Role}",
                Role = role.Role,
                ActionType = "dispatch",
                PayloadJson = JsonSerializer.Serialize(new { incidentId = incident.IncidentId, unitId = unit.UnitId, botProfileId = profile.Id }),
                CorrelationId = Guid.NewGuid().ToString("N")
            };

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

    private sealed record BotProfilesJson(BotProfileConfig[] Profiles);
    private sealed record BotProfileConfig(string Id, string Style, int CooldownMs, double RiskTolerance);
}
