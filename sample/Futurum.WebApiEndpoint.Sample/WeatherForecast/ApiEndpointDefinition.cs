using Futurum.ApiEndpoint;

namespace Futurum.WebApiEndpoint.Sample.WeatherForecast;

public class ApiEndpointDefinition : IApiEndpointDefinition
{
    public void Configure(ApiEndpointDefinitionBuilder definitionBuilder)
    {
        definitionBuilder.Web()
                         .Query<WeatherForecastGetById.ApiEndpoint>(builder => builder.Route("weather-forecast/{Id}")
                                                                                      .Version(WebApiEndpointVersions.V1_0, WebApiEndpointVersions.V2_0, WebApiEndpointVersions.V3_0)
                                                                                      .Summary("Get WeatherForecast by Id").Description("Get WeatherForecast by Id"))
                         .Query<WeatherForecastGet.ApiEndpoint>(builder => builder.Route("weather-forecast")
                                                                                  .Version(WebApiEndpointVersions.V1_0, WebApiEndpointVersions.V2_0, WebApiEndpointVersions.V3_0)
                                                                                  .Summary("Get WeatherForecast").Description("Get WeatherForecast"));
    }
}