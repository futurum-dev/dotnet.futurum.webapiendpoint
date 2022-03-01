using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// <see cref="WebApiEndpoint"/> response mapper
/// </summary>
public interface IWebApiEndpointResponseMapper<TResponse>
{
    /// <summary>
    /// Map from response domain to response dto
    /// </summary>
    Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, TResponse domain, CancellationToken cancellation);
}

/// <summary>
/// <see cref="WebApiEndpoint"/> response mapper
/// </summary>
public interface IWebApiEndpointResponseMapper<TResponse, TResponseDto>
{
    /// <summary>
    /// Map from response domain to response dto
    /// </summary>
    Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, TResponse domain, CancellationToken cancellation);
}