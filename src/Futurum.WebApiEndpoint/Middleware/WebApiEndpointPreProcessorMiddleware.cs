using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Middleware;

internal class WebApiEndpointPreProcessorMiddleware<TRequest, TResponse> : IWebApiEndpointMiddleware<TRequest, TResponse>
{
    private readonly IWebApiEndpointPreProcessorMiddleware<TRequest>[] _middleware;

    public WebApiEndpointPreProcessorMiddleware(IEnumerable<IWebApiEndpointPreProcessorMiddleware<TRequest>> middleware)
    {
        _middleware = middleware.ToArray();
    }

    public Task<Result<TResponse>> ExecuteAsync(HttpContext httpContext, TRequest request, Func<TRequest, CancellationToken, Task<Result<TResponse>>> next, CancellationToken cancellationToken)
    {
        Task<Result> ExecuteMiddleware(IWebApiEndpointPreProcessorMiddleware<TRequest> middleware) =>
            middleware.ExecuteAsync(httpContext, request, cancellationToken);

        if (_middleware.Length == 0)
            return next(request, cancellationToken);

        return _middleware.FlatMapSequentialUntilFailureAsync(ExecuteMiddleware)
                          .ThenAsync(() => next(request, cancellationToken));
    }
}