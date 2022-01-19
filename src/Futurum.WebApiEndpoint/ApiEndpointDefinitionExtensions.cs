using Futurum.ApiEndpoint;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Extension methods for <see cref="ApiEndpointDefinitionBuilder"/>
/// </summary>
public static class ApiEndpointDefinitionExtensions
{
    /// <summary>
    /// Configure a WebApiEndpoint
    /// </summary>
    public static IWebApiEndpointDefinition Web(this ApiEndpointDefinitionBuilder apiEndpointDefinitionBuilder)
    {
        var webApiEndpointDefinition = new WebApiEndpointDefinition();
        
        apiEndpointDefinitionBuilder.Add(webApiEndpointDefinition);

        return webApiEndpointDefinition;
    }
}