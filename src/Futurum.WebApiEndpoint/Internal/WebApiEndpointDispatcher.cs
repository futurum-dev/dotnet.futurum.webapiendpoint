using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

namespace Futurum.WebApiEndpoint.Internal;

internal interface IWebApiEndpointDispatcher
{
    Task<Result> ExecuteAsync(MetadataDefinition metadataDefinition, HttpContext httpContext, IWebApiEndpointMiddlewareExecutor middlewareExecutor, IWebApiEndpoint apiEndpoint,
                              CancellationToken cancellationToken);
}

internal class WebApiEndpointDispatcher<TRequestDto, TResponseDto, TRequest, TResponse, TRequestMapper, TResponseMapper> : IWebApiEndpointDispatcher
    where TRequestDto : class
    where TRequestMapper : IWebApiEndpointRequestMapper<TRequest>
    where TResponseMapper : IWebApiEndpointResponseMapper<TResponse, TResponseDto>
{
    private readonly IWebApiEndpointLogger _logger;
    private readonly TRequestMapper _requestMapper;
    private readonly TResponseMapper _responseMapper;
    private readonly IWebApiEndpointHttpContextDispatcher _httpContextDispatcher;

    public WebApiEndpointDispatcher(IWebApiEndpointLogger logger,
                                    TRequestMapper requestMapper,
                                    TResponseMapper responseMapper,
                                    IWebApiEndpointHttpContextDispatcher httpContextDispatcher)
    {
        _logger = logger;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _httpContextDispatcher = httpContextDispatcher;
    }

    public Task<Result> ExecuteAsync(MetadataDefinition metadataDefinition, HttpContext httpContext, IWebApiEndpointMiddlewareExecutor middlewareExecutor, IWebApiEndpoint apiEndpoint,
                                     CancellationToken cancellationToken)
    {
        var middlewareExecutorTyped = middlewareExecutor as IWebApiEndpointMiddlewareExecutor<TRequest, TResponse>;
        var apiEndpointTyped = apiEndpoint as IWebApiEndpoint<TRequestDto, TResponseDto, TRequest, TResponse, TRequestMapper, TResponseMapper>;

        return _requestMapper.MapAsync(httpContext, metadataDefinition, cancellationToken)
                             .DoAsync(command => _logger.RequestReceived<TRequest, TResponse>(command))
                             .ThenAsync(command => middlewareExecutorTyped.ExecuteAsync(httpContext, command, apiEndpointTyped.ExecuteAsync, cancellationToken))
                             .SwitchAsync(response => _responseMapper.MapAsync(httpContext, metadataDefinition.MetadataRouteDefinition, response, cancellationToken),
                                          error =>
                                          {
                                              _logger.Error(httpContext.Request.Path, error);
                                                  
                                              return _httpContextDispatcher.HandleFailedResponseAsync(httpContext, error, metadataDefinition.MetadataRouteDefinition, cancellationToken);
                                          });
    }
}