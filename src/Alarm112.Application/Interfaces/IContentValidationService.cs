using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IContentValidationService
{
    Task<ContentValidationResultDto> ValidateAsync(CancellationToken cancellationToken);
}
