using Futurum.ApiEndpoint;

namespace Futurum.WebApiEndpoint.Sample.OpenApiInformation;

public class ApiDefinition : IApiEndpointDefinition
{
    public void Configure(ApiEndpointDefinitionBuilder definitionBuilder)
    {
        definitionBuilder.Web()
                         .Query<OpenApiInformation.ApiEndpoint>(builder => builder.Route("open-api-information")
                                                                                  .Summary("OpenApi Summary")
                                                                                  .Description("OpenApi Description")
                                                                                  .Deprecated(true)
                                                                                  .ExternalDocsDescription("OpenApi ExternalDocs Description")
                                                                                  .ExternalDocsUrl(new Uri("http://www.google.com")));
    }
}