using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// <see cref="WebApiEndpoint"/> request mapper
/// </summary>
public interface IWebApiEndpointRequestMapper<TRequest>
{
    /// <summary>
    /// Map from <see cref="HttpContext"/> to request domain
    /// </summary>
    Task<Result<TRequest>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken);
}

/// <summary>
/// <see cref="WebApiEndpoint"/> request mapper
/// </summary>
public interface IWebApiEndpointRequestMapper<TRequestDto, TRequest>
{
    /// <summary>
    /// Map from <see cref="HttpContext"/> and request dto to request domain
    /// </summary>
    Task<Result<TRequest>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, TRequestDto dto, CancellationToken cancellationToken);
}