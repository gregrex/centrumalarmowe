namespace Alarm112.Contracts;

public sealed record ReportRoomPolishDto(
    string MissionId,
    IReadOnlyList<ReportRoomVariantDto> Variants);
