using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Middleware;

internal class DisabledWebApiEndpointMiddlewareExecutor<TRequest, TResponse> : IWebApiEndpointMiddlewareExecutor<TRequest, TResponse>
{
    public Task<Result<TResponse>> ExecuteAsync(HttpContext httpContext, TRequest request, Func<TRequest, CancellationToken, Task<Result<TResponse>>> apiEndpointFunc, CancellationToken cancellationToken) =>
        apiEndpointFunc(request, cancellationToken);
}