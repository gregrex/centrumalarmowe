using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface ICampaignEntryService
{
    Task<IReadOnlyList<CampaignChapterDto>> GetCampaignChaptersAsync(CancellationToken cancellationToken);
    Task<CampaignMissionEntryDto> GetMissionEntryAsync(string? missionId, CancellationToken cancellationToken);
    Task<IReadOnlyList<ProfileCosmeticDto>> GetProfileCosmeticsAsync(CancellationToken cancellationToken);
    Task<PlayerIdentityDto> GetPlayerIdentityAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<HomeToRoundAudioDto>> GetHomeToRoundAudioAsync(CancellationToken cancellationToken);
}
