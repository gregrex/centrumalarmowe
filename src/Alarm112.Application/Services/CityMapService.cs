using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class CityMapService : ICityMapService
{
    public Task<CityMapDto> GetCityMapAsync(CancellationToken cancellationToken)
    {
        var map = new CityMapDto(
            "city.nowogrod",
            "map.v1.quickplay",
            new[]
            {
                new CityNodeDto("zone.central", "zone.central", "district.center", 0.50, 0.36),
                new CityNodeDto("zone.residential.north", "zone.residential.north", "residential.cluster", 0.32, 0.18),
                new CityNodeDto("zone.industrial.east", "zone.industrial.east", "industrial.site", 0.78, 0.42),
                new CityNodeDto("station.medical.1", "station.medical.1", "station.medical", 0.44, 0.52),
                new CityNodeDto("station.fire.1", "station.fire.1", "station.fire", 0.67, 0.56),
                new CityNodeDto("station.police.1", "station.police.1", "station.police", 0.58, 0.22)
            },
            new[]
            {
                new CityConnectionDto("c1", "station.medical.1", "zone.central", "road.primary"),
                new CityConnectionDto("c2", "station.fire.1", "zone.industrial.east", "road.primary"),
                new CityConnectionDto("c3", "station.police.1", "zone.central", "road.secondary"),
                new CityConnectionDto("c4", "zone.central", "zone.residential.north", "road.secondary"),
                new CityConnectionDto("c5", "zone.central", "zone.industrial.east", "road.primary")
            });

        return Task.FromResult(map);
    }

    public Task<IReadOnlyCollection<SessionTimelineItemDto>> GetTimelineAsync(string sessionId, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<SessionTimelineItemDto> items = new[]
        {
            new SessionTimelineItemDto("t1", DateTimeOffset.UtcNow.AddSeconds(-45), "info", "role.operator", "Call classified: chest pain in central district.", false),
            new SessionTimelineItemDto("t2", DateTimeOffset.UtcNow.AddSeconds(-30), "warning", "role.dispatcher", "Ambulance AMB-01 dispatched.", false),
            new SessionTimelineItemDto("t3", DateTimeOffset.UtcNow.AddSeconds(-10), "critical", "role.coordinator", "Secondary response delayed. Pressure increased.", true)
        };

        return Task.FromResult(items);
    }

    public Task<DispatchResultDto> DispatchAsync(string sessionId, DispatchCommandDto command, CancellationToken cancellationToken)
    {
        var accepted = !string.IsNullOrWhiteSpace(command.UnitId) && !string.IsNullOrWhiteSpace(command.IncidentId);
        var resultCode = accepted ? "dispatch.accepted" : "dispatch.rejected";
        var eta = accepted ? 180 : 0;
        var pressureDelta = accepted ? -2 : 3;
        var warningCode = accepted ? string.Empty : "dispatch.invalid";
        var timeline = new SessionTimelineItemDto(
            Guid.NewGuid().ToString("N"),
            DateTimeOffset.UtcNow,
            accepted ? "info" : "warning",
            command.ActorRole,
            accepted
                ? $"Dispatch sent: {command.UnitId} -> {command.IncidentId} via {command.ActionId}."
                : $"Dispatch rejected: {command.UnitId} / {command.IncidentId}.",
            command.IsBot);

        return Task.FromResult(new DispatchResultDto(
            accepted,
            resultCode,
            eta,
            pressureDelta,
            warningCode,
            timeline));
    }
}
