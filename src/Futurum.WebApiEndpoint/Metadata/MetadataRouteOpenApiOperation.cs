namespace Futurum.WebApiEndpoint.Metadata;

public class MetadataRouteOpenApiOperation
{
    /// <summary>
    /// A short summary of what the operation does. For maximum readability in the swagger-ui,
    /// this field SHOULD be less than 120 characters.
    /// </summary>
    public string Summary { get; set; }

    /// <summary>
    /// A verbose explanation of the operation behavior. GFM syntax can be used for rich text representation.
    /// </summary>
    public string Description { get; set; }
}