using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// <see cref="WebApiEndpoint"/> response mapper
/// </summary>
public interface IWebApiEndpointResponseMapper<TResponse, TResponseDto>
{
    /// <summary>
    /// Map from response domain to response dto
    /// </summary>
    Result<TResponseDto> Map(TResponse domain);
}