namespace Alarm112.Contracts;

public sealed record CampaignChapterDto(
    string ChapterId,
    string TitleKey,
    string ThemeId,
    double Progress,
    IReadOnlyList<CampaignMissionNodeDto> Nodes);
