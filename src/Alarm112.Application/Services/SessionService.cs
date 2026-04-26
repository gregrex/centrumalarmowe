using System.Text.Json;
using Alarm112.Application.Factories;
using Alarm112.Application.Interfaces;
using Alarm112.Contracts;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Alarm112.Application.Services;

public sealed class SessionService : ISessionService
{
    private readonly ISessionStore _store;
    private readonly ILogger<SessionService> _logger;
    private readonly ConcurrentDictionary<string, SessionActionResultDto> _processedActions = new(StringComparer.Ordinal);
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _sessionLocks = new(StringComparer.Ordinal);

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

    public async Task<SessionActionResultDto> ApplyActionAsync(string sessionId, SessionActionDto action, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!string.Equals(action.SessionId, sessionId, StringComparison.Ordinal))
        {
            return new SessionActionResultDto(
                false,
                sessionId,
                action.ActionType,
                "SessionId in the request body must match the route sessionId.");
        }

        var actionKey = $"{sessionId}:{action.CorrelationId}";
        if (_processedActions.TryGetValue(actionKey, out var cachedResult))
            return cachedResult with { Duplicate = true, Message = $"Action '{action.ActionType}' already processed for correlation '{action.CorrelationId}'." };

        var sessionLock = _sessionLocks.GetOrAdd(sessionId, _ => new SemaphoreSlim(1, 1));
        await sessionLock.WaitAsync(cancellationToken);
        try
        {
            if (_processedActions.TryGetValue(actionKey, out cachedResult))
                return cachedResult with { Duplicate = true, Message = $"Action '{action.ActionType}' already processed for correlation '{action.CorrelationId}'." };

            var snapshot = NormalizeSnapshot(_store.GetOrAdd(sessionId, id => DemoFactory.Create(id)));
            var outcome = action.ActionType switch
            {
                "dispatch" => ApplyDispatch(snapshot, action),
                "escalate" => ApplyEscalation(snapshot, action),
                "resolve" => ApplyResolve(snapshot, action),
                _ => new ActionOutcome(snapshot, false, $"Unsupported action type '{action.ActionType}'.")
            };

            _store.Save(outcome.Snapshot);

            var result = new SessionActionResultDto(
                outcome.Success,
                sessionId,
                action.ActionType,
                outcome.Message);

            _processedActions[actionKey] = result;
            return result;
        }
        finally
        {
            sessionLock.Release();
        }
    }

    private SessionSnapshotDto NormalizeSnapshot(SessionSnapshotDto snapshot)
    {
        var normalizedRoles = snapshot.Roles
            .Select(role => role with { Role = NormalizeRole(role.Role) })
            .ToList();

        var normalizedIncidents = snapshot.Incidents
            .Select(incident => incident with { Status = NormalizeIncidentStatus(incident.Status) })
            .ToList();

        var normalizedUnits = snapshot.Units
            .Select(unit => unit with { Status = NormalizeUnitStatus(unit.Status) })
            .ToList();

        return snapshot with
        {
            Roles = normalizedRoles,
            Incidents = normalizedIncidents,
            Units = normalizedUnits
        };
    }

    private ActionOutcome ApplyDispatch(SessionSnapshotDto snapshot, SessionActionDto action)
    {
        if (!TryParsePayload(action, "dispatch", out var payload, out var parseFailure))
            return new ActionOutcome(snapshot, false, parseFailure);

        if (string.IsNullOrWhiteSpace(payload.IncidentId) || string.IsNullOrWhiteSpace(payload.UnitId))
        {
            return new ActionOutcome(snapshot, false, "Dispatch requires both incidentId and unitId in payloadJson.");
        }

        var incident = snapshot.Incidents.FirstOrDefault(i => i.IncidentId == payload.IncidentId);
        if (incident is null)
            return new ActionOutcome(snapshot, false, $"Incident '{payload.IncidentId}' was not found.");

        if (!string.Equals(incident.Status, "pending", StringComparison.OrdinalIgnoreCase))
        {
            return new ActionOutcome(snapshot, false, $"Incident '{payload.IncidentId}' is not dispatchable from status '{incident.Status}'.");
        }

        var unit = snapshot.Units.FirstOrDefault(u => u.UnitId == payload.UnitId);
        if (unit is null)
            return new ActionOutcome(snapshot, false, $"Unit '{payload.UnitId}' was not found.");

        if (!string.Equals(unit.Status, "available", StringComparison.OrdinalIgnoreCase))
            return new ActionOutcome(snapshot, false, $"Unit '{payload.UnitId}' is not available from status '{unit.Status}'.");

        var updatedIncidents = snapshot.Incidents
            .Select(i => i.IncidentId == payload.IncidentId
                ? i with { Status = "dispatched" }
                : i)
            .ToList();

        var updatedUnits = snapshot.Units
            .Select(u => u.UnitId == payload.UnitId
                ? u with { Status = "dispatched" }
                : u)
            .ToList();

        return new ActionOutcome(
            AppendAlert(snapshot with { Incidents = updatedIncidents, Units = updatedUnits }, action, $"Dispatch assigned {payload.UnitId} to {payload.IncidentId}.", "Info"),
            true,
            $"Action 'dispatch' applied by {action.ActorId}.");
    }

    private ActionOutcome ApplyEscalation(SessionSnapshotDto snapshot, SessionActionDto action)
    {
        if (!TryParsePayload(action, "escalate", out var payload, out var parseFailure))
            return new ActionOutcome(snapshot, false, parseFailure);

        if (string.IsNullOrWhiteSpace(payload.IncidentId))
            return new ActionOutcome(snapshot, false, "Escalate requires incidentId in payloadJson.");

        var incident = snapshot.Incidents.FirstOrDefault(i => i.IncidentId == payload.IncidentId);
        if (incident is null)
            return new ActionOutcome(snapshot, false, $"Incident '{payload.IncidentId}' was not found.");

        if (string.Equals(incident.Status, "resolved", StringComparison.OrdinalIgnoreCase))
            return new ActionOutcome(snapshot, false, $"Incident '{payload.IncidentId}' is already resolved.");

        if (string.Equals(incident.Status, "escalated", StringComparison.OrdinalIgnoreCase))
        {
            return new ActionOutcome(snapshot, false, $"Incident '{payload.IncidentId}' is already escalated.");
        }

        var updatedIncidents = snapshot.Incidents
            .Select(i => i.IncidentId == payload.IncidentId
                ? i with { Status = "escalated" }
                : i)
            .ToList();

        return new ActionOutcome(
            AppendAlert(snapshot with { Incidents = updatedIncidents }, action, $"Incident {payload.IncidentId} escalated.", "Warning"),
            true,
            $"Action 'escalate' applied by {action.ActorId}.");
    }

    private ActionOutcome ApplyResolve(SessionSnapshotDto snapshot, SessionActionDto action)
    {
        if (!TryParsePayload(action, "resolve", out var payload, out var parseFailure))
            return new ActionOutcome(snapshot, false, parseFailure);

        if (string.IsNullOrWhiteSpace(payload.IncidentId) || string.IsNullOrWhiteSpace(payload.UnitId))
        {
            return new ActionOutcome(snapshot, false, "Resolve requires both incidentId and unitId in payloadJson.");
        }

        var incident = snapshot.Incidents.FirstOrDefault(i => i.IncidentId == payload.IncidentId);
        if (incident is null)
            return new ActionOutcome(snapshot, false, $"Incident '{payload.IncidentId}' was not found.");

        if (string.Equals(incident.Status, "resolved", StringComparison.OrdinalIgnoreCase))
        {
            return new ActionOutcome(snapshot, false, $"Incident '{payload.IncidentId}' is already resolved.");
        }

        var unit = snapshot.Units.FirstOrDefault(u => u.UnitId == payload.UnitId);
        if (unit is null)
            return new ActionOutcome(snapshot, false, $"Unit '{payload.UnitId}' was not found.");

        var updatedIncidents = snapshot.Incidents
            .Select(i => i.IncidentId == payload.IncidentId
                ? i with { Status = "resolved" }
                : i)
            .ToList();

        var updatedUnits = snapshot.Units
            .Select(u => u.UnitId == payload.UnitId
                ? u with { Status = "available" }
                : u)
            .ToList();

        return new ActionOutcome(
            AppendAlert(snapshot with { Incidents = updatedIncidents, Units = updatedUnits }, action, $"Incident {payload.IncidentId} resolved by {payload.UnitId}.", "Info"),
            true,
            $"Action 'resolve' applied by {action.ActorId}.");
    }

    private bool TryParsePayload(SessionActionDto action, string actionName, out ActionPayload payload, out string failureMessage)
    {
        payload = new ActionPayload(null, null);

        if (string.IsNullOrWhiteSpace(action.PayloadJson))
        {
            failureMessage = $"Action '{actionName}' requires payloadJson.";
            return false;
        }

        try
        {
            using var doc = JsonDocument.Parse(action.PayloadJson);
            payload = new ActionPayload(
                doc.RootElement.TryGetProperty("incidentId", out var incidentId) && incidentId.ValueKind == JsonValueKind.String
                    ? incidentId.GetString()
                    : null,
                doc.RootElement.TryGetProperty("unitId", out var unitId) && unitId.ValueKind == JsonValueKind.String
                    ? unitId.GetString()
                    : null);
            failureMessage = string.Empty;
            return true;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "{ActionName}: failed to parse PayloadJson for actor {ActorId}", actionName, action.ActorId);
            failureMessage = $"Action '{actionName}' payloadJson is invalid JSON.";
            return false;
        }
    }

    private static string NormalizeRole(string role) =>
        role switch
        {
            "Operator" or "CallOperator" or "role.operator" => "CallOperator",
            "Dispatcher" or "role.dispatcher" => "Dispatcher",
            "Coordinator" or "OperationsCoordinator" or "role.coordinator" => "OperationsCoordinator",
            "CrisisOfficer" or "role.crisis_officer" => "CrisisOfficer",
            _ => role
        };

    private static string NormalizeIncidentStatus(string status) =>
        status switch
        {
            "New" or "new" or "Queued" or "queued" => "pending",
            "Assigned" or "assigned" or "OnRoute" or "onroute" => "dispatched",
            "Resolved" => "resolved",
            "Escalated" => "escalated",
            _ => status.ToLowerInvariant()
        };

    private static string NormalizeUnitStatus(string status) =>
        status switch
        {
            "Available" => "available",
            "OnRoute" or "Busy" or "Assigned" => "dispatched",
            _ => status.ToLowerInvariant()
        };

    private static SessionSnapshotDto AppendAlert(SessionSnapshotDto snapshot, SessionActionDto action, string message, string severity)
    {
        var alerts = snapshot.Alerts.ToList();
        alerts.Add(new HudAlertDto($"ACT-{action.CorrelationId}", message, severity));
        return snapshot with { Alerts = alerts.TakeLast(10).ToArray() };
    }

    private sealed record ActionPayload(string? IncidentId, string? UnitId);
    private sealed record ActionOutcome(SessionSnapshotDto Snapshot, bool Success, string Message);
}
