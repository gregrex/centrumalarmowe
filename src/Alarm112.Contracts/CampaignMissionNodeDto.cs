namespace Alarm112.Contracts;

public sealed record CampaignMissionNodeDto(
    string MissionId,
    string NodeKind,
    string State,
    double X,
    double Y,
    string TitleKey);
