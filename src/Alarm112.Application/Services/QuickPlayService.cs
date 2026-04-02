
using Alarm112.Application.Factories;
using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class QuickPlayService : IQuickPlayService
{
    public Task<QuickPlayBootstrapDto> GetBootstrapAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(VerticalSliceFactory.CreateBootstrap());
    }

    public Task<SessionSnapshotDto> StartAsync(QuickPlayStartRequestDto request, CancellationToken cancellationToken)
    {
        var sessionId = Guid.NewGuid().ToString("N");
        return Task.FromResult(VerticalSliceFactory.CreateSession(sessionId, request.PreferredRole));
    }

    public Task<SessionReportDto> GetReportAsync(string sessionId, CancellationToken cancellationToken)
    {
        return Task.FromResult(VerticalSliceFactory.CreateReport(sessionId));
    }
}
