using Futurum.Core.Functional;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

namespace Futurum.WebApiEndpoint.Internal.Dispatcher;

internal class QueryWebApiEndpointDispatcher<TResponseDto, TResponse> : IWebApiEndpointDispatcher
{
    private readonly IWebApiEndpointLogger _logger;
    private readonly IWebApiEndpointHttpContextDispatcher _httpContextDispatcher;
    private readonly IWebApiEndpointResponseMapper<TResponse, TResponseDto> _responseMapper;

    public QueryWebApiEndpointDispatcher(IWebApiEndpointLogger logger,
                                         IWebApiEndpointHttpContextDispatcher httpContextDispatcher,
                                         IWebApiEndpointResponseMapper<TResponse, TResponseDto> responseMapper)
    {
        _logger = logger;
        _httpContextDispatcher = httpContextDispatcher;
        _responseMapper = responseMapper;
    }

    public Task<Result> ExecuteAsync(MetadataDefinition metadataDefinition, HttpContext httpContext, IWebApiEndpointMiddlewareExecutor middlewareExecutor,
                                     IWebApiEndpoint apiEndpoint, CancellationToken cancellationToken)
    {
        var middlewareExecutorTyped = middlewareExecutor as IWebApiEndpointMiddlewareExecutor<Unit, TResponse>;
        var apiEndpointTyped = apiEndpoint as IQueryWebApiEndpoint<TResponseDto, TResponse>;

        return middlewareExecutorTyped.ExecuteAsync(httpContext, Unit.Value, (_, ct) => apiEndpointTyped.ExecuteQueryAsync(ct), cancellationToken)
                                      .ThenAsync(response => _responseMapper.Map(response))
                                      .DoWhenFailureAsync(error => _logger.Error(httpContext.Request.Path, error))
                                      .SwitchAsync(
                                          responseDto => _httpContextDispatcher.HandleSuccessResponseAsync(httpContext, responseDto, metadataDefinition.MetadataRouteDefinition, cancellationToken),
                                          error => _httpContextDispatcher.HandleFailedResponseAsync(httpContext, error, metadataDefinition.MetadataRouteDefinition, cancellationToken));
    }
}