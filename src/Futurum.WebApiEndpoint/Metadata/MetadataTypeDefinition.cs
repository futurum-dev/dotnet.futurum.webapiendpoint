namespace Futurum.WebApiEndpoint.Metadata;

/// <summary>
/// Metadata definition for WebApiEndpoint types
/// </summary>
public record MetadataTypeDefinition(Type RequestDtoType, Type ResponseDtoType, Type WebApiEndpointType, Type WebApiEndpointInterfaceType, Type MiddlewareExecutorType, Type WebApiEndpointExecutorServiceType, IEnumerable<Type> MapperTypes, Type? ResponseType = null);