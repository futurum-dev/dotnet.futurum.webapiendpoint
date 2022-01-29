namespace Futurum.WebApiEndpoint.Metadata;

public partial class MetadataRouteDefinitionBuilder
{
    /// <summary>
    /// Configure allow file uploads
    /// </summary>
    public MetadataRouteDefinitionBuilder AllowFileUploads()
    {
        _metadataRouteDefinition = _metadataRouteDefinition with
        {
            AllowFileUploads = true
        };

        return this;
    }
}