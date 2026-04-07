using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface ISessionService
{
    Task<SessionSnapshotDto?> GetSnapshotAsync(string sessionId, CancellationToken cancellationToken);
    Task<SessionSnapshotDto> CreateDemoSessionAsync(CancellationToken cancellationToken);
    Task<SessionActionResultDto> ApplyActionAsync(string sessionId, SessionActionDto action, CancellationToken cancellationToken);
}
