using System.Text.Json;
using Alarm112.Application.Factories;
using Alarm112.Application.Interfaces;
using Alarm112.Contracts;
using Microsoft.Extensions.Logging;

namespace Alarm112.Application.Services;

public sealed class SessionService : ISessionService
{
    private readonly ISessionStore _store;
    private readonly ILogger<SessionService> _logger;

    public SessionService(ISessionStore store, ILogger<SessionService> logger)
    {
        _store = store;
        _logger = logger;
    }

    public Task<SessionSnapshotDto?> GetSnapshotAsync(string sessionId, CancellationToken cancellationToken)
    {
        var snapshot = _store.TryGet(sessionId);
        return Task.FromResult(snapshot);
    }

    public Task<SessionSnapshotDto> CreateDemoSessionAsync(CancellationToken cancellationToken)
    {
        var sessionId = Guid.NewGuid().ToString("N");
        var snapshot = DemoFactory.Create(sessionId);
        _store.Save(snapshot);
        return Task.FromResult(snapshot);
    }

    public Task<SessionActionResultDto> ApplyActionAsync(string sessionId, SessionActionDto action, CancellationToken cancellationToken)
    {
        var snapshot = _store.GetOrAdd(sessionId, id => DemoFactory.Create(id));

        snapshot = action.ActionType switch
        {
            "dispatch" => ApplyDispatch(snapshot, action),
            "escalate" => ApplyEscalation(snapshot, action),
            "resolve" => ApplyResolve(snapshot, action),
            _ => snapshot
        };

        _store.Save(snapshot);
        return Task.FromResult(new SessionActionResultDto(true, sessionId, action.ActionType, $"Action '{action.ActionType}' applied by {action.ActorId}."));
    }

    private SessionSnapshotDto ApplyDispatch(SessionSnapshotDto snapshot, SessionActionDto action)
    {
        if (action.PayloadJson is null) return snapshot;

        string? incidentId = null;
        string? unitId = null;
        try
        {
            var doc = JsonDocument.Parse(action.PayloadJson);
            if (doc.RootElement.TryGetProperty("incidentId", out var incElem) &&
                incElem.ValueKind == JsonValueKind.String)
            {
                incidentId = incElem.GetString();
            }

            if (doc.RootElement.TryGetProperty("unitId", out var unitElem) &&
                unitElem.ValueKind == JsonValueKind.String)
            {
                unitId = unitElem.GetString();
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Dispatch: failed to parse PayloadJson for actor {ActorId}", action.ActorId);
            return snapshot;
        }

        if (incidentId is null || unitId is null) return snapshot;

        var updatedIncidents = snapshot.Incidents
            .Select(i => i.IncidentId == incidentId && i.Status == "pending"
                ? i with { Status = "dispatched" }
                : i)
            .ToList();

        var updatedUnits = snapshot.Units
            .Select(u => u.UnitId == unitId && u.Status == "available"
                ? u with { Status = "dispatched" }
                : u)
            .ToList();

        return snapshot with { Incidents = updatedIncidents, Units = updatedUnits };
    }

    private SessionSnapshotDto ApplyEscalation(SessionSnapshotDto snapshot, SessionActionDto action)
    {
        if (action.PayloadJson is null) return snapshot;
        string? incidentId = null;
        try
        {
            var doc = JsonDocument.Parse(action.PayloadJson);
            if (doc.RootElement.TryGetProperty("incidentId", out var incElem) &&
                incElem.ValueKind == JsonValueKind.String)
            {
                incidentId = incElem.GetString();
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Escalate: failed to parse PayloadJson for actor {ActorId}", action.ActorId);
            return snapshot;
        }

        if (incidentId is null) return snapshot;

        var updatedIncidents = snapshot.Incidents
            .Select(i => i.IncidentId == incidentId
                ? i with { Status = "escalated" }
                : i)
            .ToList();

        return snapshot with { Incidents = updatedIncidents };
    }

    private SessionSnapshotDto ApplyResolve(SessionSnapshotDto snapshot, SessionActionDto action)
    {
        if (action.PayloadJson is null) return snapshot;
        string? incidentId = null;
        string? unitId = null;
        try
        {
            var doc = JsonDocument.Parse(action.PayloadJson);
            if (doc.RootElement.TryGetProperty("incidentId", out var incElem) &&
                incElem.ValueKind == JsonValueKind.String)
            {
                incidentId = incElem.GetString();
            }

            if (doc.RootElement.TryGetProperty("unitId", out var unitElem) &&
                unitElem.ValueKind == JsonValueKind.String)
            {
                unitId = unitElem.GetString();
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Resolve: failed to parse PayloadJson for actor {ActorId}", action.ActorId);
            return snapshot;
        }

        var updatedIncidents = snapshot.Incidents
            .Select(i => i.IncidentId == incidentId
                ? i with { Status = "resolved" }
                : i)
            .ToList();

        var updatedUnits = snapshot.Units
            .Select(u => u.UnitId == unitId
                ? u with { Status = "available" }
                : u)
            .ToList();

        return snapshot with { Incidents = updatedIncidents, Units = updatedUnits };
    }
}
