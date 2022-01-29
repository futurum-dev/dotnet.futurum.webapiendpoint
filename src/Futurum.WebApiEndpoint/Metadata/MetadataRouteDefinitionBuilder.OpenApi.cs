namespace Futurum.WebApiEndpoint.Metadata;

public partial class MetadataRouteDefinitionBuilder
{
    /// <summary>
    /// Configure OpenApi <paramref name="summary"/>
    /// <remarks>
    /// A short summary of what the operation does. For maximum readability in the swagger-ui,
    /// this field SHOULD be less than 120 characters.
    /// </remarks>
    /// </summary>
    public MetadataRouteDefinitionBuilder Summary(string summary)
    {
        if (_metadataRouteDefinition.OpenApiOperation == null)
        {
            _metadataRouteDefinition = _metadataRouteDefinition with
            {
                OpenApiOperation = new(summary, string.Empty)
            };
        }
        else
        {
            _metadataRouteDefinition = _metadataRouteDefinition with
            {
                OpenApiOperation = _metadataRouteDefinition.OpenApiOperation with { Summary = summary }
            };
        }

        return this;
    }

    /// <summary>
    /// Configure OpenApi <paramref name="description"/>
    /// <remarks>
    /// A verbose explanation of the operation behavior. GFM syntax can be used for rich text representation.
    /// </remarks>
    /// </summary>
    public MetadataRouteDefinitionBuilder Description(string description)
    {
        if (_metadataRouteDefinition.OpenApiOperation == null)
        {
            _metadataRouteDefinition = _metadataRouteDefinition with
            {
                OpenApiOperation = new(string.Empty, description)
            };
        }
        else
        {
            _metadataRouteDefinition = _metadataRouteDefinition with
            {
                OpenApiOperation = _metadataRouteDefinition.OpenApiOperation with { Description = description }
            };
        }

        return this;
    }
}