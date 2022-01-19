using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

namespace Futurum.WebApiEndpoint.Internal.Dispatcher;

internal class CommandWebApiEndpointDispatcher<TCommandDto, TResponseDto, TCommand, TResponse> : IWebApiEndpointDispatcher
    where TCommandDto : class
{
    private readonly IWebApiEndpointLogger _logger;
    private readonly IWebApiEndpointHttpContextDispatcher _httpContextDispatcher;
    private readonly IWebApiEndpointRequestValidation<TCommandDto> _requestValidation;
    private readonly IWebApiEndpointRequestMapper<TCommandDto, TCommand> _requestMapper;
    private readonly IWebApiEndpointResponseMapper<TResponse, TResponseDto> _responseMapper;

    public CommandWebApiEndpointDispatcher(IWebApiEndpointLogger logger,
                                           IWebApiEndpointHttpContextDispatcher httpContextDispatcher,
                                           IWebApiEndpointRequestValidation<TCommandDto> requestValidation,
                                           IWebApiEndpointRequestMapper<TCommandDto, TCommand> requestMapper,
                                           IWebApiEndpointResponseMapper<TResponse, TResponseDto> responseMapper)
    {
        _logger = logger;
        _httpContextDispatcher = httpContextDispatcher;
        _requestValidation = requestValidation;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
    }

    public Task<Result> ExecuteAsync(MetadataDefinition metadataDefinition, HttpContext httpContext, IWebApiEndpointMiddlewareExecutor middlewareExecutor, IWebApiEndpoint apiEndpoint,
                                     CancellationToken cancellationToken)
    {
        var middlewareExecutorTyped = middlewareExecutor as IWebApiEndpointMiddlewareExecutor<TCommand, TResponse>;
        var apiEndpointTyped = apiEndpoint as ICommandWebApiEndpoint<TCommandDto, TResponseDto, TCommand, TResponse>;

        return _httpContextDispatcher.ReadRequestAsync<TCommandDto>(httpContext, metadataDefinition.MetadataMapFromDefinition, cancellationToken)
                                     .ThenAsync(commandDto => _requestValidation.ExecuteAsync(commandDto))
                                     .ThenAsync(commandDto => _requestMapper.Map(httpContext, commandDto))
                                     .DoAsync(command => _logger.RequestReceived<TCommand, TResponse>(command))
                                     .ThenAsync(command => middlewareExecutorTyped.ExecuteAsync(httpContext, command, (c, ct) => apiEndpointTyped.ExecuteCommandAsync(c, ct), cancellationToken))
                                     .ThenAsync(response => _responseMapper.Map(response))
                                     .DoWhenFailureAsync(error => _logger.Error(httpContext.Request.Path, error))
                                     .SwitchAsync(
                                         responseDto => _httpContextDispatcher.HandleSuccessResponseAsync(httpContext, responseDto, metadataDefinition.MetadataRouteDefinition, cancellationToken),
                                         error => _httpContextDispatcher.HandleFailedResponseAsync(httpContext, error, metadataDefinition.MetadataRouteDefinition, cancellationToken));
    }
}