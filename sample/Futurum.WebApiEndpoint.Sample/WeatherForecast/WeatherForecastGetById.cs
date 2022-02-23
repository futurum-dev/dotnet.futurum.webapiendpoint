using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.WeatherForecast;

public static class WeatherForecastGetById
{
    public record QueryDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Query(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<QueryDto, Query>.WithResponse<WeatherForecastDto, WeatherForecast>.WithMapper<Mapper>
    {
        private static readonly string[] Summaries = { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        protected override Task<Result<WeatherForecast>> ExecuteAsync(Query query, CancellationToken cancellationToken) =>
            Enumerable.Range(1, 5)
                      .Select(index => new WeatherForecast(DateTime.Now.AddDays(index), Random.Shared.Next(-20, 55), $"{query.Id}-{Summaries[Random.Shared.Next(Summaries.Length)]}"))
                      .First()
                      .ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<QueryDto, Query>, IWebApiEndpointResponseMapper<WeatherForecast, WeatherForecastDto>
    {
        public Result<Query> Map(HttpContext httpContext, QueryDto dto) =>
            new Query(dto.Id).ToResultOk();

        public WeatherForecastDto Map(WeatherForecast domain) =>
            WeatherForecastMapper.MapToDto(domain);
    }
}