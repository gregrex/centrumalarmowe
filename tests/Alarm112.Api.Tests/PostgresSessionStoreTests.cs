using Alarm112.Contracts;
using Alarm112.Infrastructure.Persistence;
using Microsoft.Extensions.Logging.Abstractions;

namespace Alarm112.Api.Tests;

/// <summary>
/// Unit tests for PostgresSessionStore that exercise the in-memory cache layer
/// without requiring a real PostgreSQL instance.
///
/// Strategy: provide an invalid connection string so all DB operations throw
/// (caught internally) — allowing us to test cache-hit paths, factory fallback,
/// and GetActiveSessionIds cache fallback in full isolation.
/// </summary>
public sealed class PostgresSessionStoreTests : IDisposable
{
    private const string InvalidConnStr = "Host=localhost;Port=9;Database=noexist;Username=nobody;Password=bad;CommandTimeout=1";
    private readonly PostgresSessionStore _store;

    public PostgresSessionStoreTests()
    {
        _store = new PostgresSessionStore(InvalidConnStr, NullLogger<PostgresSessionStore>.Instance);
    }

    public void Dispose() => _store.Dispose();

    private static SessionSnapshotDto MakeSnapshot(string id) =>
        new(id, "CODE1", "Lobby", [], [], [], []);

    [Fact]
    public void GetOrAdd_CacheMiss_CallsFactory()
    {
        var factoryCalled = 0;
        var result = _store.GetOrAdd("s1", id => { factoryCalled++; return MakeSnapshot(id); });
        Assert.Equal(1, factoryCalled);
        Assert.Equal("s1", result.SessionId);
    }

    [Fact]
    public void GetOrAdd_CacheHit_DoesNotCallFactory()
    {
        _store.GetOrAdd("s2", MakeSnapshot);
        var factoryCalled = 0;
        _store.GetOrAdd("s2", id => { factoryCalled++; return MakeSnapshot(id); });
        Assert.Equal(0, factoryCalled);
    }

    [Fact]
    public void GetOrAdd_ReturnsSameInstanceOnCacheHit()
    {
        var first = _store.GetOrAdd("s3", MakeSnapshot);
        var second = _store.GetOrAdd("s3", MakeSnapshot);
        Assert.Same(first, second);
    }

    [Fact]
    public void GetOrAdd_DifferentSessions_AreIndependent()
    {
        var a = _store.GetOrAdd("sa", MakeSnapshot);
        var b = _store.GetOrAdd("sb", MakeSnapshot);
        Assert.NotEqual(a.SessionId, b.SessionId);
    }

    [Fact]
    public void Save_PopulatesCache_SubsequentGetOrAddIsHit()
    {
        _store.Save(MakeSnapshot("s4"));
        var factoryCalled = 0;
        _store.GetOrAdd("s4", id => { factoryCalled++; return MakeSnapshot(id); });
        Assert.Equal(0, factoryCalled);
    }

    [Fact]
    public void Save_OverwritesExistingCacheEntry()
    {
        _store.GetOrAdd("s5", MakeSnapshot);
        _store.Save(new SessionSnapshotDto("s5", "CODE2", "Active", [], [], [], []));
        var result = _store.GetOrAdd("s5", MakeSnapshot);
        Assert.Equal("Active", result.State);
    }

    [Fact]
    public void GetActiveSessionIds_WhenDbUnavailable_ReturnsCacheKeys()
    {
        _store.GetOrAdd("id1", MakeSnapshot);
        _store.GetOrAdd("id2", MakeSnapshot);
        var ids = _store.GetActiveSessionIds();
        Assert.Contains("id1", ids);
        Assert.Contains("id2", ids);
    }

    [Fact]
    public void GetActiveSessionIds_EmptyCache_ReturnsEmptyList()
    {
        using var emptyStore = new PostgresSessionStore(InvalidConnStr, NullLogger<PostgresSessionStore>.Instance);
        Assert.Empty(emptyStore.GetActiveSessionIds());
    }
}
