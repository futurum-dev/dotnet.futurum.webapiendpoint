using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Middleware;

internal interface IWebApiEndpointMiddlewareExecutor
{
}

internal interface IWebApiEndpointMiddlewareExecutor<TRequest, TResponse> : IWebApiEndpointMiddlewareExecutor
{
    Task<Result<TResponse>> ExecuteAsync(HttpContext httpContext, TRequest request, Func<TRequest, CancellationToken, Task<Result<TResponse>>> apiEndpointFunc, CancellationToken cancellationToken);
}

internal class WebApiEndpointMiddlewareExecutor<TRequest, TResponse> : IWebApiEndpointMiddlewareExecutor<TRequest, TResponse>
{
    private readonly IWebApiEndpointMiddleware<TRequest, TResponse>[] _middleware;

    public WebApiEndpointMiddlewareExecutor(IWebApiEndpointMiddleware<TRequest, TResponse>[] middleware)
    {
        _middleware = middleware.Reverse().ToArray();
    }

    public Task<Result<TResponse>> ExecuteAsync(HttpContext httpContext, TRequest request, Func<TRequest, CancellationToken, Task<Result<TResponse>>> apiEndpointFunc, CancellationToken cancellationToken)
    {
        if (_middleware.Length == 0)
            return apiEndpointFunc(request, cancellationToken);
            
        return _middleware.Aggregate(apiEndpointFunc, (next, middleware) => (r, ct) => middleware.ExecuteAsync(httpContext, r, next, ct))(request, cancellationToken);
    }
}