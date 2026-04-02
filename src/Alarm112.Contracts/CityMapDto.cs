using System.Collections.Generic;

namespace Alarm112.Contracts;

public sealed record CityMapDto(
    string CityId,
    string PresetId,
    IReadOnlyCollection<CityNodeDto> Nodes,
    IReadOnlyCollection<CityConnectionDto> Connections);
