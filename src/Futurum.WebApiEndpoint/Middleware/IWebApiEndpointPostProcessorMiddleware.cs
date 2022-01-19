using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Middleware;

/// <summary>
/// WebApiEndpoint post-processor middleware
/// </summary>
public interface IWebApiEndpointPostProcessorMiddleware<in TRequest, in TResponse>
{
    /// <summary>
    /// Execute the middleware
    /// </summary>
    Task<Result> ExecuteAsync(HttpContext httpContext, TRequest request, TResponse response, CancellationToken cancellationToken);
}