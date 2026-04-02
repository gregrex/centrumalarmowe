
using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IQuickPlayService
{
    Task<QuickPlayBootstrapDto> GetBootstrapAsync(CancellationToken cancellationToken);
    Task<SessionSnapshotDto> StartAsync(QuickPlayStartRequestDto request, CancellationToken cancellationToken);
    Task<SessionReportDto> GetReportAsync(string sessionId, CancellationToken cancellationToken);
}
