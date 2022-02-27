using Futurum.Core.Option;

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
                OpenApiOperation = new(summary, string.Empty, Option<bool>.None, null)
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
                OpenApiOperation = new(string.Empty, description, Option<bool>.None, null)
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

    /// <summary>
    /// Configure OpenApi <paramref name="deprecated"/>
    /// <remarks>
    /// Declares this operation to be deprecated. Consumers SHOULD refrain from usage of the declared operation.
    /// </remarks>
    /// </summary>
    public MetadataRouteDefinitionBuilder Deprecated(bool deprecated)
    {
        if (_metadataRouteDefinition.OpenApiOperation == null)
        {
            _metadataRouteDefinition = _metadataRouteDefinition with
            {
                OpenApiOperation = new(string.Empty, string.Empty, deprecated, null)
            };
        }
        else
        {
            _metadataRouteDefinition = _metadataRouteDefinition with
            {
                OpenApiOperation = _metadataRouteDefinition.OpenApiOperation with { Deprecated = deprecated }
            };
        }

        return this;
    }

    /// <summary>
    /// Configure OpenApi ExternalDocs <paramref name="externalDocsDescription"/>
    /// <remarks>
    /// A short description of the target documentation.
    /// </remarks>
    /// </summary>
    public MetadataRouteDefinitionBuilder ExternalDocsDescription(string externalDocsDescription)
    {
        if (_metadataRouteDefinition.OpenApiOperation == null)
        {
            _metadataRouteDefinition = _metadataRouteDefinition with
            {
                OpenApiOperation = new(string.Empty, string.Empty, Option<bool>.None, new MetadataRouteOpenApiExternalDocs(externalDocsDescription, null))
            };
        }
        else
        {
            if (_metadataRouteDefinition.OpenApiOperation.ExternalDocs != null)
            {
                _metadataRouteDefinition = _metadataRouteDefinition with
                {
                    OpenApiOperation = _metadataRouteDefinition.OpenApiOperation with
                    {
                        ExternalDocs = _metadataRouteDefinition.OpenApiOperation.ExternalDocs with
                        {
                            Description = externalDocsDescription
                        }
                    }
                };
            }
            else
            {
                _metadataRouteDefinition = _metadataRouteDefinition with
                {
                    OpenApiOperation = _metadataRouteDefinition.OpenApiOperation with
                    {
                        ExternalDocs = new MetadataRouteOpenApiExternalDocs(externalDocsDescription, null)
                    }
                };
            }
        }

        return this;
    }

    /// <summary>
    /// Configure OpenApi ExternalDocs <paramref name="externalDocsUrl"/>
    /// <remarks>
    /// REQUIRED. The URL for the target documentation. Value MUST be in the format of a URL.
    /// </remarks>
    /// </summary>
    public MetadataRouteDefinitionBuilder ExternalDocsUrl(Uri externalDocsUrl)
    {
        if (_metadataRouteDefinition.OpenApiOperation == null)
        {
            _metadataRouteDefinition = _metadataRouteDefinition with
            {
                OpenApiOperation = new(string.Empty, string.Empty, Option<bool>.None, new MetadataRouteOpenApiExternalDocs(string.Empty, externalDocsUrl))
            };
        }
        else
        {
            if (_metadataRouteDefinition.OpenApiOperation.ExternalDocs != null)
            {
                _metadataRouteDefinition = _metadataRouteDefinition with
                {
                    OpenApiOperation = _metadataRouteDefinition.OpenApiOperation with
                    {
                        ExternalDocs = _metadataRouteDefinition.OpenApiOperation.ExternalDocs with
                        {
                            Url = externalDocsUrl
                        }
                    }
                };
            }
            else
            {
                _metadataRouteDefinition = _metadataRouteDefinition with
                {
                    OpenApiOperation = _metadataRouteDefinition.OpenApiOperation with
                    {
                        ExternalDocs = new MetadataRouteOpenApiExternalDocs(string.Empty, externalDocsUrl)
                    }
                };
            }
        }

        return this;
    }
}