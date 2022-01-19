using System.Net;

using Futurum.ApiEndpoint;

namespace Futurum.WebApiEndpoint.Sample.Security;

public class ApiEndpointDefinition : IApiEndpointDefinition
{
    public void Configure(ApiEndpointDefinitionBuilder definitionBuilder)
    {
        definitionBuilder.Web()
                         .Command<Login.ApiEndpoint>(builder => builder.Post("login").FailedStatusCode(HttpStatusCode.Unauthorized)
                                                                       .Version(WebApiEndpointVersions.V1_0, WebApiEndpointVersions.V2_0)
                                                                       .Summary("Login and get JWT Token").Description("Login and get JWT Token"));
    }
}