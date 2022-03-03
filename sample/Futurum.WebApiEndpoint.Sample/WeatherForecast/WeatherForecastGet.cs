using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.WeatherForecast;

public static class WeatherForecastGet
{
    public class ApiEndpoint : QueryWebApiEndpoint.NoRequest.ResponseDataCollection<WeatherForecastDto, WeatherForecast>.Mapper<WeatherForecastDataMapper>
    {
        private static readonly string[] Summaries = { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        protected override Task<Result<ResponseDataCollection<WeatherForecast>>> ExecuteAsync(CancellationToken cancellationToken) =>
            Enumerable.Range(1, 5)
                      .Select(index => new WeatherForecast(DateTime.Now.AddDays(index), Random.Shared.Next(-20, 55), Summaries[Random.Shared.Next(Summaries.Length)]))
                      .ToResultOkAsync()
                      .ToResponseDataCollectionAsync();
    }
}