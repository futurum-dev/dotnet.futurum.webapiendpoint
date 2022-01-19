using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.WeatherForecast;

public class WeatherForecastDataMapper : IWebApiEndpointDataMapper<WeatherForecast, WeatherForecastDto>
{
    public WeatherForecastDto Map(WeatherForecast data) => 
        new(data.Date, data.TemperatureC, data.TemperatureF, data.Summary);
}

public static class WeatherForecastMapper
{
    public static Result<WeatherForecastDto> MapToDto(WeatherForecast domain) => 
        new WeatherForecastDto(domain.Date, domain.TemperatureC, domain.TemperatureF, domain.Summary).ToResultOk();

    public static Result<WeatherForecast> MapToDomain(WeatherForecastDto dto) =>
        new WeatherForecast(dto.Date, dto.TemperatureC, dto.Summary).ToResultOk();
}