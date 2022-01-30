using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Middleware;

internal class WebApiEndpointPostProcessorMiddleware<TRequest, TResponse> : IWebApiEndpointMiddleware<TRequest, TResponse>
{
    private readonly IWebApiEndpointPostProcessorMiddleware<TRequest, TResponse>[] _middleware;

    public WebApiEndpointPostProcessorMiddleware(IEnumerable<IWebApiEndpointPostProcessorMiddleware<TRequest, TResponse>> middleware)
    {
        _middleware = middleware.ToArray();
    }

    public Task<Result<TResponse>> ExecuteAsync(HttpContext httpContext, TRequest request, Func<TRequest, CancellationToken, Task<Result<TResponse>>> next, CancellationToken cancellationToken)
    {
        Task<Result<TResponse>> ExecuteNext() =>
            next(request, cancellationToken);

        Task<Result> ExecuteMiddleware(IWebApiEndpointPostProcessorMiddleware<TRequest, TResponse> middleware, TResponse response) =>
            middleware.ExecuteAsync(httpContext, request, response, cancellationToken);

        if (_middleware.Length == 0)
            return next(request, cancellationToken);
        
        return ExecuteNext()
            .ThenAsync(response => _middleware.FlatMapSequentialUntilFailureAsync(middleware => ExecuteMiddleware(middleware, response)));
    }
}