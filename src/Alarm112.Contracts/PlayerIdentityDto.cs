namespace Alarm112.Contracts;

public sealed record PlayerIdentityDto(
    string PlayerId,
    string DisplayName,
    string PreferredRole,
    string PortraitId,
    string FrameId,
    string BadgeId,
    string TitleId);
