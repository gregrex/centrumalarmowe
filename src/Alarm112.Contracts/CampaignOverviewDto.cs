namespace Alarm112.Contracts;

public sealed record CampaignOverviewDto(
    string ChapterId,
    string ActiveNodeId,
    IReadOnlyList<CampaignNodeDto> Nodes,
    IReadOnlyList<string> RewardsPreview);
