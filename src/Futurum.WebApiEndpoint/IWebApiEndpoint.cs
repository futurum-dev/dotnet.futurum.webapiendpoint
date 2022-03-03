using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Interface for all WebApiEndpoints
/// </summary>
public interface IWebApiEndpoint
{
}


internal interface IWebApiEndpoint<TRequestDto, TResponseDto, TRequest, TResponse, TRequestMapper, TResponseMapper> : IWebApiEndpoint
    where TRequestMapper : IWebApiEndpointRequestMapper<TRequest>
    where TResponseMapper : IWebApiEndpointResponseMapper<TResponse, TResponseDto>
{
    /// <summary>
    /// Execute the WebApiEndpoint
    /// <para>This method is called once for each request received</para>
    /// </summary>
    Task<Result<TResponse>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}