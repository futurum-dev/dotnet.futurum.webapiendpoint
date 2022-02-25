namespace Futurum.WebApiEndpoint.Metadata;

/// <summary>
/// Metadata definition for a WebApiEndpoint Route Parameter
/// </summary>
public record MetadataRouteParameterDefinition(string Name, MetadataRouteParameterDefinitionType ParameterDefinitionType, Type Type);

public enum MetadataRouteParameterDefinitionType
{
    Path,
    Query,
    Header,
    Cookie
}