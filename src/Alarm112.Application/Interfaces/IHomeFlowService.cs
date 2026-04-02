using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IHomeFlowService
{
    Task<HomeHubDto> GetHomeHubAsync(CancellationToken cancellationToken);
    Task<CampaignOverviewDto> GetCampaignOverviewAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<DailyChallengeDto>> GetDailyChallengesAsync(CancellationToken cancellationToken);
    Task<SettingsBundleDto> GetSettingsBundleAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<ScreenAudioRouteDto>> GetScreenAudioRoutesAsync(CancellationToken cancellationToken);
}
