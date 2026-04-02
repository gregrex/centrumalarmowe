using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IThemePackService
{
    Task<ThemePackDto> GetThemePackAsync(CancellationToken cancellationToken);
    Task<MenuFlowDto> GetMenuFlowAsync(CancellationToken cancellationToken);
    Task<MetaProgressionDto> GetDemoMetaProgressionAsync(CancellationToken cancellationToken);
}
