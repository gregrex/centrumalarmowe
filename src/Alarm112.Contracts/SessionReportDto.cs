
namespace Alarm112.Contracts;

public sealed record SessionReportDto(
    string SessionId,
    string Grade,
    int PressurePeak,
    IReadOnlyCollection<SessionMetricDto> Metrics,
    string BestMoment,
    string BiggestRisk,
    int BotTakeovers);
