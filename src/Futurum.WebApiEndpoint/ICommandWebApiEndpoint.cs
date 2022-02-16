using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Interface for all Command WebApiEndpoints
/// </summary>
public interface ICommandWebApiEndpoint : IWebApiEndpoint
{
}

internal interface ICommandWebApiEndpoint<TCommandDto, TResponseDto, TCommand, TResponse, TRequestMapper, TResponseMapper> : ICommandWebApiEndpoint
    where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommand>
    where TResponseMapper : IWebApiEndpointResponseMapper<TResponse, TResponseDto>
{
    Task<Result<TResponse>> ExecuteCommandAsync(TCommand command, CancellationToken cancellationToken);
}

internal interface ICommandWebApiEndpoint<TResponseDto, TCommand, TResponse, TRequestMapper, TResponseMapper> : ICommandWebApiEndpoint
    where TRequestMapper : IWebApiEndpointRequestMapper<TCommand>
    where TResponseMapper : IWebApiEndpointResponseMapper<TResponse, TResponseDto>
{
    Task<Result<TResponse>> ExecuteCommandAsync(TCommand command, CancellationToken cancellationToken);
}

internal interface ICommandWebApiEndpoint<TCommandDto, TCommand, TRequestMapper> : ICommandWebApiEndpoint
    where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommand>
{
    Task<Result> ExecuteCommandAsync(TCommand command, CancellationToken cancellationToken);
}

internal interface ICommandWebApiEndpoint<TCommand, TRequestMapper> : ICommandWebApiEndpoint
    where TRequestMapper : IWebApiEndpointRequestMapper<TCommand>
{
    Task<Result> ExecuteCommandAsync(TCommand command, CancellationToken cancellationToken);
}