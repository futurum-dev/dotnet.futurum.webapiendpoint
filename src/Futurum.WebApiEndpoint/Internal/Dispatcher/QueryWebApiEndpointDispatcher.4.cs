using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

namespace Futurum.WebApiEndpoint.Internal.Dispatcher;

internal class QueryWebApiEndpointDispatcher<TQueryDto, TResponseDto, TQuery, TResponse, TRequestMapper, TResponseMapper> : IWebApiEndpointDispatcher
    where TQueryDto : class
    where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQuery>
    where TResponseMapper : IWebApiEndpointResponseMapper<TResponse, TResponseDto>
{
    private readonly IWebApiEndpointLogger _logger;
    private readonly IWebApiEndpointHttpContextDispatcher _httpContextDispatcher;
    private readonly IWebApiEndpointRequestValidation<TQueryDto> _requestValidation;
    private readonly TRequestMapper _requestMapper;
    private readonly TResponseMapper _responseMapper;

    public QueryWebApiEndpointDispatcher(IWebApiEndpointLogger logger,
                                         IWebApiEndpointHttpContextDispatcher httpContextDispatcher,
                                         IWebApiEndpointRequestValidation<TQueryDto> requestValidation,
                                         TRequestMapper requestMapper,
                                         TResponseMapper responseMapper)
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
        var apiEndpointTyped = apiEndpoint as IQueryWebApiEndpoint<TQueryDto, TResponseDto, TQuery, TResponse, TRequestMapper, TResponseMapper>;

        return _httpContextDispatcher.ReadRequestAsync<TQueryDto>(httpContext, metadataDefinition.MetadataMapFromDefinition, metadataDefinition.MetadataMapFromMultipartDefinition, cancellationToken)
                                     .ThenAsync(commandDto => _requestValidation.ExecuteAsync(commandDto))
                                     .ThenAsync(commandDto => _requestMapper.Map(httpContext, commandDto))
                                     .DoAsync(command => _logger.RequestReceived<TQuery, TResponse>(command))
                                     .ThenAsync(query => middlewareExecutorTyped.ExecuteAsync(httpContext, query, (q, ct) => apiEndpointTyped.ExecuteQueryAsync(q, ct), cancellationToken))
                                     .MapAsync(response => _responseMapper.Map(httpContext, response))
                                     .DoWhenFailureAsync(error => _logger.Error(httpContext.Request.Path, error))
                                     .SwitchAsync(responseDto => _httpContextDispatcher.HandleSuccessResponseAsync(httpContext, responseDto, metadataDefinition.MetadataRouteDefinition, cancellationToken),
                                                  error => _httpContextDispatcher.HandleFailedResponseAsync(httpContext, error, metadataDefinition.MetadataRouteDefinition, cancellationToken));
    }
}