using Futurum.ApiEndpoint;

namespace Futurum.WebApiEndpoint.Benchmark.WebApiEndpoint;

public class ApiEndpointDefinition : IApiEndpointDefinition
{
    public void Configure(ApiEndpointDefinitionBuilder definitionBuilder)
    {
        definitionBuilder.Web()
                         .Command<TestWebApiEndpoint.ApiEndpoint>(builder => builder.Post("benchmark/{id}", ("id", typeof(string))).Version(WebApiEndpointVersions.V1_0));
    }
}