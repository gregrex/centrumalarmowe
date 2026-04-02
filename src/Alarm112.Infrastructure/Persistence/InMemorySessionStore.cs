using System.Collections.Concurrent;
using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Infrastructure.Persistence;

public sealed class InMemorySessionStore : ISessionStore
{
    private readonly ConcurrentDictionary<string, SessionSnapshotDto> _sessions = new();

    public SessionSnapshotDto GetOrAdd(string sessionId, Func<string, SessionSnapshotDto> factory)
        => _sessions.GetOrAdd(sessionId, factory);

    public void Save(SessionSnapshotDto snapshot)
        => _sessions[snapshot.SessionId] = snapshot;

    public IReadOnlyList<string> GetActiveSessionIds()
        => _sessions.Keys.ToList();
}
