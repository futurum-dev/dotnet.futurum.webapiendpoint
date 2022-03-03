using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.WeatherForecast;

public static class WeatherForecastGetById
{
    public record QueryDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Query(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.Request<QueryDto, Query>.Response<WeatherForecastDto, WeatherForecast>.Mapper<Mapper>
    {
        private static readonly string[] Summaries = { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        public override Task<Result<WeatherForecast>> ExecuteAsync(Query request, CancellationToken cancellationToken) =>
            Enumerable.Range(1, 5)
                      .Select(index => new WeatherForecast(DateTime.Now.AddDays(index), Random.Shared.Next(-20, 55), $"{request.Id}-{Summaries[Random.Shared.Next(Summaries.Length)]}"))
                      .First()
                      .ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<QueryDto, Query>, IWebApiEndpointResponseDtoMapper<WeatherForecast, WeatherForecastDto>
    {
        public Task<Result<Query>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, QueryDto dto, CancellationToken cancellationToken) =>
            new Query(dto.Id).ToResultOkAsync();

        public WeatherForecastDto Map(WeatherForecast domain) =>
            WeatherForecastMapper.MapToDto(domain);
    }
}