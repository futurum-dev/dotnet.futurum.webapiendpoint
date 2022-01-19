using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Middleware;

/// <summary>
/// WebApiEndpoint middleware
/// </summary>
public interface IWebApiEndpointMiddleware<TRequest, TResponse>
{
    /// <summary>
    /// Execute the middleware
    /// </summary>
    Task<Result<TResponse>> ExecuteAsync(HttpContext httpContext, TRequest request, Func<TRequest, CancellationToken, Task<Result<TResponse>>> next, CancellationToken cancellationToken);
}