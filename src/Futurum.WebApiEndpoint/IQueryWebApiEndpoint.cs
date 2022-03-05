namespace Futurum.WebApiEndpoint;

/// <summary>
/// Interface for all Query WebApiEndpoints
/// </summary>
public interface IQueryWebApiEndpoint : IWebApiEndpoint
{
}

public interface IQueryWebApiEndpoint<TRequestDto, TResponseDto, TRequest, TResponse, TRequestMapper, TResponseMapper>
    : IWebApiEndpoint<TRequestDto, TResponseDto, TRequest, TResponse, TRequestMapper, TResponseMapper>,
      IQueryWebApiEndpoint
    where TRequestMapper : IWebApiEndpointRequestMapper<TRequest>
    where TResponseMapper : IWebApiEndpointResponseMapper<TResponse, TResponseDto>
{
}