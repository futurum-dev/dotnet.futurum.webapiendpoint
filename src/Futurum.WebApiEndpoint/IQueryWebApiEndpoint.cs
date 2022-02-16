using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Interface for all Query WebApiEndpoints
/// </summary>
public interface IQueryWebApiEndpoint : IWebApiEndpoint
{
}

internal interface IQueryWebApiEndpoint<TResponseDto, TResponse, TResponseMapper> : IQueryWebApiEndpoint
    where TResponseMapper : IWebApiEndpointResponseMapper<TResponse, TResponseDto>
{
    Task<Result<TResponse>> ExecuteQueryAsync(CancellationToken cancellationToken);
}

internal interface IQueryWebApiEndpoint<TResponseDto, TQuery, TResponse, TRequestMapper, TResponseMapper> : IQueryWebApiEndpoint
    where TRequestMapper : IWebApiEndpointRequestMapper<TQuery>
    where TResponseMapper : IWebApiEndpointResponseMapper<TResponse, TResponseDto>
{
    Task<Result<TResponse>> ExecuteQueryAsync(TQuery query, CancellationToken cancellationToken);
}

internal interface IQueryWebApiEndpoint<TQueryDto, TResponseDto, TQuery, TResponse, TRequestMapper, TResponseMapper> : IQueryWebApiEndpoint
    where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQuery>
    where TResponseMapper : IWebApiEndpointResponseMapper<TResponse, TResponseDto>
{
    Task<Result<TResponse>> ExecuteQueryAsync(TQuery query, CancellationToken cancellationToken);
}