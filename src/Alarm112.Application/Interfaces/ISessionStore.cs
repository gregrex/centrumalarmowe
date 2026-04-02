using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface ISessionStore
{
    SessionSnapshotDto GetOrAdd(string sessionId, Func<string, SessionSnapshotDto> factory);
    void Save(SessionSnapshotDto snapshot);
    IReadOnlyList<string> GetActiveSessionIds();
}
