using Futurum.Core.Option;

namespace Futurum.WebApiEndpoint.Metadata;

public record MetadataRouteOpenApiOperation(string Summary, string Description, Option<bool> Deprecated, MetadataRouteOpenApiExternalDocs? ExternalDocs);