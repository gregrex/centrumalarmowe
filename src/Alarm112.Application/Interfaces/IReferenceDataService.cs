using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IReferenceDataService
{
    Task<ReferenceDataDto> GetReferenceDataAsync(CancellationToken cancellationToken);
}
