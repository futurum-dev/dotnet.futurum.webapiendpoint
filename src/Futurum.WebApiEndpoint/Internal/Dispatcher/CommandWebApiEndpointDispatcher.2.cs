using Futurum.Core.Functional;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

namespace Futurum.WebApiEndpoint.Internal.Dispatcher;

internal class CommandWebApiEndpointDispatcher<TCommandDto, TCommand, TRequestMapper> : IWebApiEndpointDispatcher
    where TCommandDto : class
    where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommand>
{
    private readonly IWebApiEndpointLogger _logger;
    private readonly IWebApiEndpointHttpContextDispatcher _httpContextDispatcher;
    private readonly IWebApiEndpointRequestValidation<TCommandDto> _requestValidation;
    private readonly TRequestMapper _requestMapper;

    public CommandWebApiEndpointDispatcher(IWebApiEndpointLogger logger,
                                           IWebApiEndpointHttpContextDispatcher httpContextDispatcher,
                                           IWebApiEndpointRequestValidation<TCommandDto> requestValidation,
                                           TRequestMapper requestMapper)
    {
        _logger = logger;
        _httpContextDispatcher = httpContextDispatcher;
        _requestValidation = requestValidation;
        _requestMapper = requestMapper;
    }

    public Task<Result> ExecuteAsync(MetadataDefinition metadataDefinition, HttpContext httpContext, IWebApiEndpointMiddlewareExecutor middlewareExecutor, IWebApiEndpoint apiEndpoint,
                                     CancellationToken cancellationToken)
    {
        var middlewareExecutorTyped = middlewareExecutor as IWebApiEndpointMiddlewareExecutor<TCommand, Unit>;
        var apiEndpointTyped = apiEndpoint as ICommandWebApiEndpoint<TCommandDto, TCommand, TRequestMapper>;

        return _httpContextDispatcher.ReadRequestAsync<TCommandDto>(httpContext, metadataDefinition.MetadataMapFromDefinition, cancellationToken)
                                     .ThenAsync(commandDto => _requestValidation.ExecuteAsync(commandDto))
                                     .ThenAsync(commandDto => _requestMapper.Map(httpContext, commandDto))
                                     .DoAsync(command => _logger.RequestReceived<TCommand, Unit>(command))
                                     .ThenAsync(command => middlewareExecutorTyped.ExecuteAsync(httpContext, command, (c, ct) => apiEndpointTyped.ExecuteCommandAsync(c, ct).MapAsync(Unit.Value),
                                                                                                cancellationToken))
                                     .DoWhenFailureAsync(error => _logger.Error(httpContext.Request.Path, error))
                                     .SwitchAsync(_ => _httpContextDispatcher.HandleSuccessResponseAsync(httpContext, metadataDefinition.MetadataRouteDefinition, cancellationToken),
                                                  error => _httpContextDispatcher.HandleFailedResponseAsync(httpContext, error, metadataDefinition.MetadataRouteDefinition, cancellationToken));
    }
}