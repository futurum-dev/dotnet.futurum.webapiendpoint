using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Middleware;

/// <summary>
/// WebApiEndpoint pre-processor middleware
/// </summary>
public interface IWebApiEndpointPreProcessorMiddleware<in TRequest>
{
    /// <summary>
    /// Execute the middleware
    /// </summary>
    Task<Result> ExecuteAsync(HttpContext httpContext, TRequest request, CancellationToken cancellationToken);
}