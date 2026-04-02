using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IFinalHandoffService
{
    Task<FinalHandoffPackDto> GetFinalHandoffPackAsync(CancellationToken cancellationToken);
    Task<StoreComplianceDto> GetStoreComplianceAsync(CancellationToken cancellationToken);
    Task<InternalDemoPackageDto> GetInternalDemoPackageAsync(CancellationToken cancellationToken);
    Task<PlaytestReleaseLiveopsLoopDto> GetPlaytestReleaseLiveopsLoopAsync(CancellationToken cancellationToken);
    Task<ReleaseReadinessV25Dto> GetReleaseReadinessV25Async(CancellationToken cancellationToken);
}
