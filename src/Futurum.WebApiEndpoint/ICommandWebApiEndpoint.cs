namespace Futurum.WebApiEndpoint;

/// <summary>
/// Interface for all Command WebApiEndpoints
/// </summary>
public interface ICommandWebApiEndpoint : IWebApiEndpoint
{
}

internal interface ICommandWebApiEndpoint<TRequestDto, TResponseDto, TRequest, TResponse, TRequestMapper, TResponseMapper>
    : IWebApiEndpoint<TRequestDto, TResponseDto, TRequest, TResponse, TRequestMapper, TResponseMapper>,
      ICommandWebApiEndpoint
    where TRequestMapper : IWebApiEndpointRequestMapper<TRequest>
    where TResponseMapper : IWebApiEndpointResponseMapper<TResponse, TResponseDto>
{
}