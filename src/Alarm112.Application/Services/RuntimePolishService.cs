using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class RuntimePolishService : IRuntimePolishService
{
    public Task<LiveRouteRuntimeDto> GetLiveRouteRuntimeAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.demo.16" : missionId;
        var payload = new LiveRouteRuntimeDto(
            resolvedMissionId,
            new[]
            {
                new LiveRouteSegmentStateDto("route.demo.01", "hub.ems", "incident.bridge", "clear", 42, "pulse_blue"),
                new LiveRouteSegmentStateDto("route.demo.02", "hub.fire", "incident.mall", "delayed", 67, "pulse_amber"),
                new LiveRouteSegmentStateDto("route.demo.03", "hub.police", "incident.bridge", "rerouted", 58, "pulse_red")
            },
            "obj.primary.bridge_patient",
            "pressure_peak");

        return Task.FromResult(payload);
    }

    public Task<RoundTimerDto> GetRoundTimerAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.demo.16" : missionId;
        var payload = new RoundTimerDto(resolvedMissionId, 240, 87, "pressure_peak", new[] { 180, 90, 30, 10 });
        return Task.FromResult(payload);
    }

    public Task<IReadOnlyList<ChainEscalationStateDto>> GetChainEscalationRuntimeAsync(string? missionId, CancellationToken cancellationToken)
    {
        IReadOnlyList<ChainEscalationStateDto> payload = new[]
        {
            new ChainEscalationStateDto("chain.bridge.01", "route.demo.01.delayed", "high", new[] { "traffic_spike", "patient_deterioration", "objective_at_risk" }, new[] { "reroute", "reinforce" })
        };
        return Task.FromResult(payload);
    }

    public Task<DemoMissionPolishDto> GetDemoMissionPolishAsync(string? missionId, CancellationToken cancellationToken)
    {
        var resolvedMissionId = string.IsNullOrWhiteSpace(missionId) ? "mission.demo.16" : missionId;
        var payload = new DemoMissionPolishDto(resolvedMissionId, "rainy_evening", "scene_pack.demo.01", "audio_pack.demo.01", new[] { "ambulance.hero", "bridge.hero", "dispatcher_console.hero" }, new[] { "menu_city_night", "runtime_rain", "report_room_afterglow" });
        return Task.FromResult(payload);
    }
}
