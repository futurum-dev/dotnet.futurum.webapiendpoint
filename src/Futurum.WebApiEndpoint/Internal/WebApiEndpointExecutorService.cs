using System.Net;

using Futurum.Core.Result;
using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Internal.Dispatcher;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

using Serilog;

namespace Futurum.WebApiEndpoint.Internal;

internal static class WebApiEndpointExecutorService
{
    public static async Task ExecuteAsync(HttpContext httpContext, MetadataDefinition? metadataDefinition, string routePath, CancellationToken cancellationToken)
    {
        try
        {
            if (metadataDefinition != null)
                await CallWebApiEndpointAsync(httpContext, metadataDefinition, cancellationToken).UnwrapAsync();
            else
                await WebApiEndpointNotFoundAsync(httpContext, routePath, cancellationToken);
        }
        catch (Exception exception)
        {
            var errorData = new WebApiRouteErrorData(routePath, httpContext.Request.Path, "Internal Server Error", (int)HttpStatusCode.InternalServerError, exception.Message);

            Log.Logger.Error(exception, "WebApiEndpoint error - {@eventData}", errorData);

            var errorResponse = $"WebApiEndpoint error for route : '{routePath}'".ToResultError().ToErrorStructure();

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(errorResponse, typeof(ResultErrorStructure), cancellationToken);
        }
    }

    private static Task<Result<(IWebApiEndpointDispatcher, IWebApiEndpointMiddlewareExecutor, IWebApiEndpoint)>> CallWebApiEndpointAsync(
        HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken)
    {
        var webApiEndpointDispatcher = httpContext.RequestServices.TryGetService<IWebApiEndpointDispatcher>(metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType);

        var middlewareExecutor = httpContext.RequestServices.TryGetService<IWebApiEndpointMiddlewareExecutor>(metadataDefinition.MetadataTypeDefinition.MiddlewareExecutorType);

        var apiEndpoint = httpContext.RequestServices.TryGetService<IWebApiEndpoint>(metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType);

        return Result.CombineAll(webApiEndpointDispatcher, middlewareExecutor, apiEndpoint,
                         (webApiEndpointDispatcher, middlewareExecutor, apiEndpoint) => (webApiEndpointDispatcher, middlewareExecutor, apiEndpoint))
                     .ThenAsync(x => x.webApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext, x.middlewareExecutor, x.apiEndpoint, cancellationToken));
    }

    private static Task WebApiEndpointNotFoundAsync(HttpContext httpContext, string routePath, CancellationToken cancellationToken)
    {
        var eventData = new WebApiEndpointNotFoundData(routePath, httpContext.Request.Method);
        Log.Logger.Error("Unable to find WebApiEndpoint - {@eventData}", eventData);

        var errorResponse = $"WebApiEndpoint - Unable to find WebApiEndpoint for route : '{routePath}'".ToResultError().ToErrorStructure();

        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return httpContext.Response.WriteAsJsonAsync(errorResponse, typeof(ResultErrorStructure), cancellationToken);
    }

    private record struct WebApiEndpointNotFoundData(string RoutePath, string HttpMethod);

    private record struct WebApiRouteErrorData(string Route, string Path, string Status, int StatusCode, string Reason);
}