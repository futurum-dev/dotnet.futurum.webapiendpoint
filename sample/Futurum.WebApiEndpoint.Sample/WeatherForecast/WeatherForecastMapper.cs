using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.WeatherForecast;

public class WeatherForecastDataMapper : IWebApiEndpointResponseDataMapper<WeatherForecast, WeatherForecastDto>
{
    public WeatherForecastDto Map(WeatherForecast data) => 
        new(data.Date, data.TemperatureC, data.TemperatureF, data.Summary);
}

public static class WeatherForecastMapper
{
    public static WeatherForecastDto MapToDto(WeatherForecast domain) => 
        new(domain.Date, domain.TemperatureC, domain.TemperatureF, domain.Summary);

    public static Result<WeatherForecast> MapToDomain(WeatherForecastDto dto) =>
        new WeatherForecast(dto.Date, dto.TemperatureC, dto.Summary).ToResultOk();
}