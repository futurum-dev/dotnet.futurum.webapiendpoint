using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

namespace Futurum.WebApiEndpoint.Internal;

internal interface IWebApiEndpointDispatcher
{
    Task<Result> ExecuteAsync(MetadataDefinition metadataDefinition, HttpContext httpContext, CancellationToken cancellationToken);
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
    private readonly IWebApiEndpointMiddlewareExecutor<TRequest, TResponse> _middlewareExecutor;
    private readonly IWebApiEndpoint<TRequestDto, TResponseDto, TRequest, TResponse, TRequestMapper, TResponseMapper> _webApiEndpoint;

    public WebApiEndpointDispatcher(IWebApiEndpointLogger logger,
                                    TRequestMapper requestMapper,
                                    TResponseMapper responseMapper,
                                    IWebApiEndpointHttpContextDispatcher httpContextDispatcher,
                                    IWebApiEndpointMiddlewareExecutor<TRequest, TResponse> middlewareExecutor,
                                    IWebApiEndpoint<TRequestDto, TResponseDto, TRequest, TResponse, TRequestMapper, TResponseMapper> webApiEndpoint)
    {
        _logger = logger;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _httpContextDispatcher = httpContextDispatcher;
        _middlewareExecutor = middlewareExecutor;
        _webApiEndpoint = webApiEndpoint;
    }

    public Task<Result> ExecuteAsync(MetadataDefinition metadataDefinition, HttpContext httpContext, CancellationToken cancellationToken) =>
        _requestMapper.MapAsync(httpContext, metadataDefinition, cancellationToken)
                      .DoAsync(command => _logger.RequestReceived<TRequest, TResponse>(command))
                      .ThenAsync(command => _middlewareExecutor.ExecuteAsync(httpContext, command, _webApiEndpoint.ExecuteAsync, cancellationToken))
                      .DoAsync(response => _logger.ResponseSent<TRequest, TResponse>(response))
                      .SwitchAsync(response => _responseMapper.MapAsync(httpContext, metadataDefinition.MetadataRouteDefinition, response, cancellationToken),
                                   error =>
                                   {
                                       _logger.Error(httpContext.Request.Path, error);

                                       return _httpContextDispatcher.HandleFailedResponseAsync(httpContext, error, metadataDefinition.MetadataRouteDefinition, cancellationToken);
                                   });
}