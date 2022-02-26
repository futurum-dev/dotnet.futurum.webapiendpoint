using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

namespace Futurum.WebApiEndpoint.Internal.Dispatcher;

internal class CommandWebApiEndpointDispatcher<TCommandDto, TResponseDto, TCommand, TResponse, TRequestMapper, TResponseMapper> : IWebApiEndpointDispatcher
    where TCommandDto : class
    where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommand>
    where TResponseMapper : IWebApiEndpointResponseMapper<TResponse, TResponseDto>
{
    private readonly IWebApiEndpointLogger _logger;
    private readonly IWebApiEndpointHttpContextDispatcher _httpContextDispatcher;
    private readonly IWebApiEndpointRequestValidation<TCommandDto> _requestValidation;
    private readonly TRequestMapper _requestMapper;
    private readonly TResponseMapper _responseMapper;

    public CommandWebApiEndpointDispatcher(IWebApiEndpointLogger logger,
                                           IWebApiEndpointHttpContextDispatcher httpContextDispatcher,
                                           IWebApiEndpointRequestValidation<TCommandDto> requestValidation,
                                           TRequestMapper requestMapper,
                                           TResponseMapper responseMapper)
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
        var apiEndpointTyped = apiEndpoint as ICommandWebApiEndpoint<TCommandDto, TResponseDto, TCommand, TResponse, TRequestMapper, TResponseMapper>;

        return _httpContextDispatcher.ReadRequestAsync<TCommandDto>(httpContext, metadataDefinition.MetadataMapFromDefinition, metadataDefinition.MetadataMapFromMultipartDefinition, cancellationToken)
                                     .ThenAsync(commandDto => _requestValidation.ExecuteAsync(commandDto))
                                     .ThenAsync(commandDto => _requestMapper.Map(httpContext, commandDto))
                                     .DoAsync(command => _logger.RequestReceived<TCommand, TResponse>(command))
                                     .ThenAsync(command => middlewareExecutorTyped.ExecuteAsync(httpContext, command, (c, ct) => apiEndpointTyped.ExecuteCommandAsync(c, ct), cancellationToken))
                                     .MapAsync(response => _responseMapper.Map(httpContext, response))
                                     .DoWhenFailureAsync(error => _logger.Error(httpContext.Request.Path, error))
                                     .SwitchAsync(responseDto => _httpContextDispatcher.HandleSuccessResponseAsync(httpContext, responseDto, metadataDefinition.MetadataRouteDefinition, cancellationToken),
                                                  error => _httpContextDispatcher.HandleFailedResponseAsync(httpContext, error, metadataDefinition.MetadataRouteDefinition, cancellationToken));
    }
}