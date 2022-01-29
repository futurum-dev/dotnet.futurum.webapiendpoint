namespace Futurum.WebApiEndpoint.Metadata;

public partial class MetadataRouteDefinitionBuilder
{
    /// <summary>
    /// Configure escape hatch
    /// </summary>
    public MetadataRouteDefinitionBuilder ExtendedOptions(Action<RouteHandlerBuilder> builder)
    {
        _metadataRouteDefinition = _metadataRouteDefinition with
        {
            ExtendedOptions = builder
        };

        return this;
    }
}