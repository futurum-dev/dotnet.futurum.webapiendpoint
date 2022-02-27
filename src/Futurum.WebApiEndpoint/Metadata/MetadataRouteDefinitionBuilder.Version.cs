using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Metadata;

public partial class MetadataRouteDefinitionBuilder
{
    /// <summary>
    /// Configure <see cref="ApiVersion"/>
    /// <para>If not specified it will use <see cref="WebApiEndpointConfiguration.DefaultApiVersion"/></para>
    /// </summary>
    public MetadataRouteDefinitionBuilder Version(params ApiVersion[] versions)
    {
        _apiVersions.AddRange(versions);

        return this;
    }
}