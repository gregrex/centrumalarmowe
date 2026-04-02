namespace Alarm112.Contracts;

public sealed record CampaignNodeRuntimeDto(
    string MissionId,
    string State,
    int Stars,
    bool IsFocused);
