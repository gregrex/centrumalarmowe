namespace Alarm112.Contracts;

public sealed record QuasiProductionDemoFlowDto(
    string MissionId,
    IReadOnlyList<QuasiProductionDemoFlowStepDto> Steps);
