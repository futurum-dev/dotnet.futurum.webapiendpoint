using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// <see cref="WebApiEndpoint"/> request mapper
/// </summary>
public interface IWebApiEndpointRequestMapper<TRequest>
{
    /// <summary>
    /// Map from <see cref="HttpContext"/> to request domain
    /// </summary>
    Result<TRequest> Map(HttpContext httpContext);
}

/// <summary>
/// <see cref="WebApiEndpoint"/> request mapper
/// </summary>
public interface IWebApiEndpointRequestMapper<TRequestDto, TRequest>
{
    /// <summary>
    /// Map from <see cref="HttpContext"/> and request dto to request domain
    /// </summary>
    Result<TRequest> Map(HttpContext httpContext, TRequestDto dto);
}