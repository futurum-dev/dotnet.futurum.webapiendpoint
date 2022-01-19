namespace Futurum.WebApiEndpoint.Sample.WeatherForecast;

public record WeatherForecastDto(DateTime Date, int TemperatureC, int TemperatureF, string? Summary);