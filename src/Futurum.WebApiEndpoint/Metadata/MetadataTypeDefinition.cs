namespace Futurum.WebApiEndpoint.Metadata;

/// <summary>
/// Metadata definition for WebApiEndpoint types
/// </summary>
public record MetadataTypeDefinition(Type RequestDtoType, Type UnderlyingRequestDtoType, Type ResponseDtoType, Type UnderlyingResponseDtoType, Type WebApiEndpointType, Type WebApiEndpointInterfaceType, Type MiddlewareExecutorType, Type WebApiEndpointExecutorServiceType, IEnumerable<Type> MapperTypes, Type? ResponseType = null);