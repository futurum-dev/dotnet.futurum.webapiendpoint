namespace Futurum.WebApiEndpoint.Metadata;

/// <summary>
/// Metadata definition for a WebApiEndpoint
/// </summary>
public record MetadataDefinition(MetadataRouteDefinition MetadataRouteDefinition, MetadataTypeDefinition MetadataTypeDefinition, MetadataMapFromDefinition? MetadataMapFromDefinition);