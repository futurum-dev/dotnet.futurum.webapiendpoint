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
    Task<Result<TResponse>> ExecuteCommandAsync(TRequest command, CancellationToken cancellationToken);
}