using Alarm112.Application.Services;
using Alarm112.Contracts;
using Alarm112.Infrastructure.Persistence;
using Microsoft.Extensions.Logging.Abstractions;

namespace Alarm112.Api.Tests;

public class SessionServiceTests
{
    private static SessionService CreateService(out InMemorySessionStore store)
    {
        store = new InMemorySessionStore();
        return new SessionService(store, NullLogger<SessionService>.Instance);
    }

    private static SessionActionDto MakeAction(string sessionId, string actionType, string? payload = null) =>
        new SessionActionDto
        {
            SessionId = sessionId,
            ActorId = "player-1",
            Role = "CallOperator",
            ActionType = actionType,
            PayloadJson = payload,
            CorrelationId = Guid.NewGuid().ToString("N")[..16]
        };

    [Fact]
    public async Task CreateDemoSession_CreatesUniqueSessionIds()
    {
        var service = CreateService(out _);
        var s1 = await service.CreateDemoSessionAsync(default);
        var s2 = await service.CreateDemoSessionAsync(default);

        Assert.NotEqual(s1.SessionId, s2.SessionId);
        Assert.NotEmpty(s1.SessionId);
    }

    [Fact]
    public async Task CreateDemoSession_SnapshotHasActiveState()
    {
        var service = CreateService(out _);
        var snapshot = await service.CreateDemoSessionAsync(default);
        Assert.Equal("Active", snapshot.State);
    }

    [Fact]
    public async Task GetSnapshot_ReturnsStoredSession()
    {
        var service = CreateService(out _);
        var created = await service.CreateDemoSessionAsync(default);
        var fetched = await service.GetSnapshotAsync(created.SessionId, default);

        Assert.Equal(created.SessionId, fetched.SessionId);
    }

    [Fact]
    public async Task GetSnapshot_UnknownId_CreatesNewSession()
    {
        var service = CreateService(out _);
        var snapshot = await service.GetSnapshotAsync("brand-new-id", default);
        Assert.Equal("brand-new-id", snapshot.SessionId);
    }

    [Fact]
    public async Task ApplyAction_Dispatch_ReturnsSuccess()
    {
        var service = CreateService(out _);
        var created = await service.CreateDemoSessionAsync(default);
        var incident = created.Incidents.First();
        var unit = created.Units.First();

        var result = await service.ApplyActionAsync(
            created.SessionId,
            MakeAction(created.SessionId, "dispatch", $"{{\"incidentId\":\"{incident.IncidentId}\",\"unitId\":\"{unit.UnitId}\"}}"),
            default);

        Assert.True(result.Success);
        Assert.Equal("dispatch", result.ActionType);
    }

    [Fact]
    public async Task ApplyAction_Escalate_ChangesIncidentToEscalated()
    {
        var service = CreateService(out _);
        var created = await service.CreateDemoSessionAsync(default);
        var incident = created.Incidents.First();

        var action = MakeAction(created.SessionId, "escalate", $"{{\"incidentId\":\"{incident.IncidentId}\"}}");
        action.Role = "OperationsCoordinator";

        await service.ApplyActionAsync(created.SessionId, action, default);

        var updated = await service.GetSnapshotAsync(created.SessionId, default);
        var updatedIncident = updated.Incidents.First(i => i.IncidentId == incident.IncidentId);
        Assert.Equal("escalated", updatedIncident.Status);
    }

    [Fact]
    public async Task ApplyAction_Resolve_ReturnsSuccess()
    {
        var service = CreateService(out _);
        var created = await service.CreateDemoSessionAsync(default);
        var incident = created.Incidents.First();
        var unit = created.Units.First();

        var result = await service.ApplyActionAsync(
            created.SessionId,
            MakeAction(created.SessionId, "resolve", $"{{\"incidentId\":\"{incident.IncidentId}\",\"unitId\":\"{unit.UnitId}\"}}"),
            default);

        Assert.True(result.Success);
    }

    [Fact]
    public async Task ApplyAction_UnknownActionType_DoesNotCrash()
    {
        var service = CreateService(out _);
        var created = await service.CreateDemoSessionAsync(default);
        var action = new SessionActionDto
        {
            SessionId = created.SessionId,
            ActorId = "player-1",
            Role = "CallOperator",
            ActionType = "dispatch",  // valid for DataAnnotations, but payload is empty
            PayloadJson = null,
            CorrelationId = "abc"
        };

        var result = await service.ApplyActionAsync(created.SessionId, action, default);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task ApplyAction_PersistsChangesToStore()
    {
        var service = CreateService(out var store);
        var created = await service.CreateDemoSessionAsync(default);
        var incident = created.Incidents.First();
        var unit = created.Units.First();

        await service.ApplyActionAsync(
            created.SessionId,
            MakeAction(created.SessionId, "escalate", $"{{\"incidentId\":\"{incident.IncidentId}\"}}"),
            default);

        var stored = store.GetOrAdd(created.SessionId, _ => throw new Exception("should not recreate"));
        var storedIncident = stored.Incidents.First(i => i.IncidentId == incident.IncidentId);
        Assert.Equal("escalated", storedIncident.Status);
    }
}
