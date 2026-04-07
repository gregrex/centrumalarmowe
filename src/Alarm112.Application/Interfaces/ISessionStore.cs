using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface ISessionStore
{
    /// <summary>Returns the session if it exists, otherwise null. Does not create.</summary>
    SessionSnapshotDto? TryGet(string sessionId);

    SessionSnapshotDto GetOrAdd(string sessionId, Func<string, SessionSnapshotDto> factory);
    void Save(SessionSnapshotDto snapshot);
    IReadOnlyList<string> GetActiveSessionIds();
}
