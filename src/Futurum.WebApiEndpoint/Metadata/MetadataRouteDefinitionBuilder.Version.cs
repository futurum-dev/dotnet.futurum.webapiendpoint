using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Metadata;

public partial class MetadataRouteDefinitionBuilder
{
    /// <summary>
    /// Configure <paramref name="versions"/>
    /// </summary>
    public MetadataRouteDefinitionBuilder Version(params ApiVersion[] versions)
    {
        _apiVersions.AddRange(versions);

        return this;
    }
}