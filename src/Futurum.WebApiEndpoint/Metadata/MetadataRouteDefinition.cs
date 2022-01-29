using Futurum.ApiEndpoint;
using Futurum.Core.Option;

using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Metadata;

/// <summary>
/// Metadata definition for a WebApiEndpoint Route
/// </summary>
public record MetadataRouteDefinition(MetadataRouteHttpMethod HttpMethod, string RouteTemplate, ApiVersion? ApiVersion, List<MetadataRouteParameterDefinition> ParameterDefinitions,
                                      MetadataRouteOpenApiOperation? OpenApiOperation, int SuccessStatusCode, int FailedStatusCode, 
                                      Option<Action<RouteHandlerBuilder>> ExtendedOptions, Option<MetadataSecurityDefinition> SecurityDefinition) : IMetadataDefinition;