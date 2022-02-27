namespace Futurum.WebApiEndpoint.Metadata;

public record MetadataRouteOpenApiOperation(string Summary, string Description, bool Deprecated, MetadataRouteOpenApiExternalDocs? ExternalDocs);