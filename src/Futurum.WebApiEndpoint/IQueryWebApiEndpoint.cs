using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Interface for all Query WebApiEndpoints
/// </summary>
public interface IQueryWebApiEndpoint : IWebApiEndpoint
{
}

internal interface IQueryWebApiEndpoint<TResponseDto, TResponse> : IQueryWebApiEndpoint
{
    Task<Result<TResponse>> ExecuteQueryAsync(CancellationToken cancellationToken);
}

internal interface IQueryWebApiEndpoint<TResponseDto, TQuery, TResponse> : IQueryWebApiEndpoint
{
    Task<Result<TResponse>> ExecuteQueryAsync(TQuery query, CancellationToken cancellationToken);
}

internal interface IQueryWebApiEndpoint<TQueryDto, TResponseDto, TQuery, TResponse> : IQueryWebApiEndpoint
{
    Task<Result<TResponse>> ExecuteQueryAsync(TQuery query, CancellationToken cancellationToken);
}