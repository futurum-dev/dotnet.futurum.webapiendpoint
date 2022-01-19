using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Interface for all Command WebApiEndpoints
/// </summary>
public interface ICommandWebApiEndpoint : IWebApiEndpoint
{
}

internal interface ICommandWebApiEndpoint<TCommandDto, TResponseDto, TCommand, TResponse> : ICommandWebApiEndpoint
{
    Task<Result<TResponse>> ExecuteCommandAsync(TCommand command, CancellationToken cancellationToken);
}

internal interface ICommandWebApiEndpoint<TResponseDto, TCommand, TResponse> : ICommandWebApiEndpoint
{
    Task<Result<TResponse>> ExecuteCommandAsync(TCommand command, CancellationToken cancellationToken);
}

internal interface ICommandWebApiEndpoint<TCommandDto, TCommand> : ICommandWebApiEndpoint
{
    Task<Result> ExecuteCommandAsync(TCommand command, CancellationToken cancellationToken);
}

internal interface ICommandWebApiEndpoint<TCommand> : ICommandWebApiEndpoint
{
    Task<Result> ExecuteCommandAsync(TCommand command, CancellationToken cancellationToken);
}