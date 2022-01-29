using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Metadata;

public partial class MetadataRouteDefinitionBuilder
{
    private readonly List<ApiVersion> _apiVersions = new();

    private readonly Type _apiEndpointType;
    private MetadataRouteDefinition _metadataRouteDefinition;

    public MetadataRouteDefinitionBuilder(Type apiEndpointType,
                                          MetadataRouteDefinition metadataRouteDefinition)
    {
        _apiEndpointType = apiEndpointType;
        _metadataRouteDefinition = metadataRouteDefinition;
    }

    public MetadataRouteDefinitionBuilder SuccessStatusCode(int httpStatusCode)
    {
        _metadataRouteDefinition = _metadataRouteDefinition with
        {
            SuccessStatusCode = httpStatusCode
        };

        return this;
    }

    /// <summary>
    /// Configure success <paramref name="httpStatusCode"/>
    /// </summary>
    public MetadataRouteDefinitionBuilder SuccessStatusCode(HttpStatusCode httpStatusCode)
    {
        _metadataRouteDefinition = _metadataRouteDefinition with
        {
            SuccessStatusCode = (int)httpStatusCode
        };

        return this;
    }

    /// <summary>
    /// Configure failed <paramref name="httpStatusCode"/>
    /// </summary>
    public MetadataRouteDefinitionBuilder FailedStatusCode(int httpStatusCode)
    {
        _metadataRouteDefinition = _metadataRouteDefinition with
        {
            FailedStatusCode = httpStatusCode
        };

        return this;
    }

    /// <summary>
    /// Configure failed <paramref name="httpStatusCode"/>
    /// </summary>
    public MetadataRouteDefinitionBuilder FailedStatusCode(HttpStatusCode httpStatusCode)
    {
        _metadataRouteDefinition = _metadataRouteDefinition with
        {
            FailedStatusCode = (int)httpStatusCode
        };

        return this;
    }
}