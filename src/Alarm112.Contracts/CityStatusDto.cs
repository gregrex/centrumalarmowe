namespace Alarm112.Contracts;

public sealed record CityStatusDto(
    int Pressure,
    string PressureBand,
    string TrafficState,
    string WeatherState,
    string MediaState,
    int AvailableUnits);
