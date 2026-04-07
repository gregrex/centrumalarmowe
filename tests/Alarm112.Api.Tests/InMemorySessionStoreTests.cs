using Alarm112.Application.Interfaces;
using Alarm112.Contracts;
using Alarm112.Infrastructure.Persistence;

namespace Alarm112.Api.Tests;

public class InMemorySessionStoreTests
{
    private static SessionSnapshotDto MakeSnapshot(string id) =>
        new SessionSnapshotDto(
            SessionId: id,
            SessionCode: "CODE1",
            State: "Active",
            Roles: [],
            Incidents: [],
            Units: [],
            Alerts: []
        );

    [Fact]
    public void GetOrAdd_NewSession_CreatesAndReturns()
    {
        var store = new InMemorySessionStore();
        var result = store.GetOrAdd("session-1", id => MakeSnapshot(id));

        Assert.Equal("session-1", result.SessionId);
    }

    [Fact]
    public void GetOrAdd_ExistingSession_ReturnsExisting()
    {
        var store = new InMemorySessionStore();
        var first = store.GetOrAdd("sess", id => MakeSnapshot(id));
        var second = store.GetOrAdd("sess", id => MakeSnapshot("should-not-replace"));

        Assert.Equal("sess", first.SessionId);
        Assert.Equal("sess", second.SessionId);
    }

    [Fact]
    public void Save_UpdatesExistingSession()
    {
        var store = new InMemorySessionStore();
        store.GetOrAdd("sess", id => MakeSnapshot(id));

        var updated = MakeSnapshot("sess") with { State = "Summary" };
        store.Save(updated);

        var retrieved = store.GetOrAdd("sess", _ => throw new InvalidOperationException("should not be called"));
        Assert.Equal("Summary", retrieved.State);
    }

    [Fact]
    public void GetActiveSessionIds_ReturnsAllSavedIds()
    {
        var store = new InMemorySessionStore();
        store.GetOrAdd("a", id => MakeSnapshot(id));
        store.GetOrAdd("b", id => MakeSnapshot(id));
        store.GetOrAdd("c", id => MakeSnapshot(id));

        var ids = store.GetActiveSessionIds();

        Assert.Contains("a", ids);
        Assert.Contains("b", ids);
        Assert.Contains("c", ids);
        Assert.Equal(3, ids.Count);
    }

    [Fact]
    public void GetActiveSessionIds_EmptyStore_ReturnsEmpty()
    {
        var store = new InMemorySessionStore();
        var ids = store.GetActiveSessionIds();
        Assert.Empty(ids);
    }

    [Fact]
    public async Task ConcurrentGetOrAdd_ThreadSafe_NoExceptions()
    {
        var store = new InMemorySessionStore();
        var tasks = Enumerable.Range(0, 20).Select(i =>
            Task.Run(() => store.GetOrAdd($"session-{i % 5}", id => MakeSnapshot(id)))
        );

        await Task.WhenAll(tasks);
    }

    [Fact]
    public void TryGet_ExistingSession_ReturnsSnapshot()
    {
        var store = new InMemorySessionStore();
        store.Save(MakeSnapshot("existing-id"));
        var result = store.TryGet("existing-id");
        Assert.NotNull(result);
        Assert.Equal("existing-id", result!.SessionId);
    }

    [Fact]
    public void TryGet_NonExistentSession_ReturnsNull()
    {
        var store = new InMemorySessionStore();
        var result = store.TryGet("does-not-exist");
        Assert.Null(result);
    }

    [Fact]
    public void TryGet_AfterGetOrAdd_ReturnsSameData()
    {
        var store = new InMemorySessionStore();
        store.GetOrAdd("via-getOradd", id => MakeSnapshot(id));
        var result = store.TryGet("via-getOradd");
        Assert.NotNull(result);
        Assert.Equal("via-getOradd", result!.SessionId);
    }
}
