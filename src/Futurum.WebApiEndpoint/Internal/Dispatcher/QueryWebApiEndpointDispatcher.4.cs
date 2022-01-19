using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

namespace Futurum.WebApiEndpoint.Internal.Dispatcher;

internal class QueryWebApiEndpointDispatcher<TQueryDto, TResponseDto, TQuery, TResponse> : IWebApiEndpointDispatcher
    where TQueryDto : class
{
    private readonly IWebApiEndpointLogger _logger;
    private readonly IWebApiEndpointHttpContextDispatcher _httpContextDispatcher;
    private readonly IWebApiEndpointRequestValidation<TQueryDto> _requestValidation;
    private readonly IWebApiEndpointRequestMapper<TQueryDto, TQuery> _requestMapper;
    private readonly IWebApiEndpointResponseMapper<TResponse, TResponseDto> _responseMapper;

    public QueryWebApiEndpointDispatcher(IWebApiEndpointLogger logger,
                                         IWebApiEndpointHttpContextDispatcher httpContextDispatcher,
                                         IWebApiEndpointRequestValidation<TQueryDto> requestValidation,
                                         IWebApiEndpointRequestMapper<TQueryDto, TQuery> requestMapper,
                                         IWebApiEndpointResponseMapper<TResponse, TResponseDto> responseMapper)
    {
        _logger = logger;
        _httpContextDispatcher = httpContextDispatcher;
        _requestValidation = requestValidation;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
    }

    public Task<Result> ExecuteAsync(MetadataDefinition metadataDefinition, HttpContext httpContext, IWebApiEndpointMiddlewareExecutor middlewareExecutor,
                                     IWebApiEndpoint apiEndpoint, CancellationToken cancellationToken)
    {
        var middlewareExecutorTyped = middlewareExecutor as IWebApiEndpointMiddlewareExecutor<TQuery, TResponse>;
        var apiEndpointTyped = apiEndpoint as IQueryWebApiEndpoint<TQueryDto, TResponseDto, TQuery, TResponse>;

        return _httpContextDispatcher.ReadRequestAsync<TQueryDto>(httpContext, metadataDefinition.MetadataMapFromDefinition, cancellationToken)
                                     .ThenAsync(commandDto => _requestValidation.ExecuteAsync(commandDto))
                                     .ThenAsync(commandDto => _requestMapper.Map(httpContext, commandDto))
                                     .DoAsync(command => _logger.RequestReceived<TQuery, TResponse>(command))
                                     .ThenAsync(query => middlewareExecutorTyped.ExecuteAsync(httpContext, query, (q, ct) => apiEndpointTyped.ExecuteQueryAsync(q, ct), cancellationToken))
                                     .ThenAsync(response => _responseMapper.Map(response))
                                     .DoWhenFailureAsync(error => _logger.Error(httpContext.Request.Path, error))
                                     .SwitchAsync(
                                         responseDto => _httpContextDispatcher.HandleSuccessResponseAsync(httpContext, responseDto, metadataDefinition.MetadataRouteDefinition, cancellationToken),
                                         error => _httpContextDispatcher.HandleFailedResponseAsync(httpContext, error, metadataDefinition.MetadataRouteDefinition, cancellationToken));
    }
}