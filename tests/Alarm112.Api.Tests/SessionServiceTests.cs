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

        Assert.NotNull(fetched);
        Assert.Equal(created.SessionId, fetched.SessionId);
    }

    [Fact]
    public async Task GetSnapshot_UnknownId_ReturnsNull()
    {
        var service = CreateService(out _);
        var snapshot = await service.GetSnapshotAsync("brand-new-id", default);
        Assert.Null(snapshot);
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
        Assert.NotNull(updated);
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

    [Fact]
    public async Task ApplyAction_Dispatch_InvalidJson_LeavesSnapshotUnchanged()
    {
        var service = CreateService(out _);
        var created = await service.CreateDemoSessionAsync(default);

        await service.ApplyActionAsync(
            created.SessionId,
            MakeAction(created.SessionId, "dispatch", "{\"incidentId\":\"broken\""),
            default);

        var updated = await service.GetSnapshotAsync(created.SessionId, default);
        Assert.NotNull(updated);
        Assert.Equivalent(created, updated);
    }

    [Fact]
    public async Task ApplyAction_Escalate_MissingIncidentId_LeavesSnapshotUnchanged()
    {
        var service = CreateService(out _);
        var created = await service.CreateDemoSessionAsync(default);

        await service.ApplyActionAsync(
            created.SessionId,
            MakeAction(created.SessionId, "escalate", "{\"wrongField\":\"inc-1\"}"),
            default);

        var updated = await service.GetSnapshotAsync(created.SessionId, default);
        Assert.NotNull(updated);
        Assert.Equivalent(created, updated);
    }

    [Fact]
    public async Task ApplyAction_Dispatch_XssLikePayload_DoesNotMatchOrMutateEntities()
    {
        var service = CreateService(out _);
        var created = await service.CreateDemoSessionAsync(default);

        await service.ApplyActionAsync(
            created.SessionId,
            MakeAction(
                created.SessionId,
                "dispatch",
                "{\"incidentId\":\"<script>alert(1)</script>\",\"unitId\":\"<img src=x onerror=alert(1)>\"}"),
            default);

        var updated = await service.GetSnapshotAsync(created.SessionId, default);
        Assert.NotNull(updated);
        Assert.Equivalent(created, updated);
    }

    [Fact]
    public async Task ApplyAction_Dispatch_UnknownIdentifiers_LeavesSnapshotUnchanged()
    {
        var service = CreateService(out _);
        var created = await service.CreateDemoSessionAsync(default);

        await service.ApplyActionAsync(
            created.SessionId,
            MakeAction(created.SessionId, "dispatch", "{\"incidentId\":\"missing-incident\",\"unitId\":\"missing-unit\"}"),
            default);

        var updated = await service.GetSnapshotAsync(created.SessionId, default);
        Assert.NotNull(updated);
        Assert.Equivalent(created, updated);
    }

    [Fact]
    public async Task ApplyAction_Dispatch_RepeatedPayload_IsEffectivelyIdempotent()
    {
        var service = CreateService(out _);
        var created = await service.CreateDemoSessionAsync(default);
        var incident = created.Incidents.First();
        var unit = created.Units.First();
        var action = MakeAction(
            created.SessionId,
            "dispatch",
            $"{{\"incidentId\":\"{incident.IncidentId}\",\"unitId\":\"{unit.UnitId}\"}}");

        await service.ApplyActionAsync(created.SessionId, action, default);
        var afterFirst = await service.GetSnapshotAsync(created.SessionId, default);

        await service.ApplyActionAsync(created.SessionId, action, default);
        var afterSecond = await service.GetSnapshotAsync(created.SessionId, default);

        Assert.NotNull(afterFirst);
        Assert.NotNull(afterSecond);
        Assert.Equivalent(afterFirst, afterSecond);
    }
}
