using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

namespace Futurum.WebApiEndpoint.Internal.Dispatcher;

internal class CommandWebApiEndpointDispatcher<TResponseDto, TCommand, TResponse, TRequestMapper, TResponseMapper> : IWebApiEndpointDispatcher
    where TRequestMapper : IWebApiEndpointRequestMapper<TCommand>
    where TResponseMapper : IWebApiEndpointResponseMapper<TResponse, TResponseDto>
{
    private readonly IWebApiEndpointLogger _logger;
    private readonly IWebApiEndpointHttpContextDispatcher _httpContextDispatcher;
    private readonly TRequestMapper _requestMapper;
    private readonly TResponseMapper _responseMapper;

    public CommandWebApiEndpointDispatcher(IWebApiEndpointLogger logger,
                                           IWebApiEndpointHttpContextDispatcher httpContextDispatcher,
                                           TRequestMapper requestMapper,
                                           TResponseMapper responseMapper)
    {
        _logger = logger;
        _httpContextDispatcher = httpContextDispatcher;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
    }

    public Task<Result> ExecuteAsync(MetadataDefinition metadataDefinition, HttpContext httpContext, IWebApiEndpointMiddlewareExecutor middlewareExecutor, IWebApiEndpoint apiEndpoint,
                                     CancellationToken cancellationToken)
    {
        var middlewareExecutorTyped = middlewareExecutor as IWebApiEndpointMiddlewareExecutor<TCommand, TResponse>;
        var apiEndpointTyped = apiEndpoint as ICommandWebApiEndpoint<TResponseDto, TCommand, TResponse, TRequestMapper, TResponseMapper>;

        return _requestMapper.Map(httpContext)
                      .Do(command => _logger.RequestReceived<TCommand, TResponse>(command))
                      .ThenAsync(command => middlewareExecutorTyped.ExecuteAsync(httpContext, command, (c, ct) => apiEndpointTyped.ExecuteCommandAsync(c, ct), cancellationToken))
                      .ThenAsync(response => _responseMapper.Map(response))
                      .DoWhenFailureAsync(error => _logger.Error(httpContext.Request.Path, error))
                      .SwitchAsync(responseDto => _httpContextDispatcher.HandleSuccessResponseAsync(httpContext, responseDto, metadataDefinition.MetadataRouteDefinition, cancellationToken),
                                   error => _httpContextDispatcher.HandleFailedResponseAsync(httpContext, error, metadataDefinition.MetadataRouteDefinition, cancellationToken));
    }
}