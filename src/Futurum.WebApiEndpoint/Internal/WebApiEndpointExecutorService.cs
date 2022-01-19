using System.Collections.Concurrent;
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
    private static readonly ConcurrentDictionary<Type, Result<IWebApiEndpointDispatcher>> WebApiEndpointDispatcherCache = new();
    private static readonly ConcurrentDictionary<Type, Result<IWebApiEndpointMiddlewareExecutor>> WebApiEndpointMiddlewareExecutorCache = new();

    public static async Task ExecuteAsync(HttpContext httpContext, MetadataDefinition? metadataDefinition, WebApiEndpointConfiguration configuration, string routePath, CancellationToken cancellationToken)
    {
        try
        {
            if (metadataDefinition != null)
                await CallWebApiEndpointAsync(httpContext, metadataDefinition, configuration, cancellationToken).UnwrapAsync();
            else
                await WebApiEndpointNotFoundAsync(httpContext, routePath);
        }
        catch (Exception exception)
        {
            var errorData = new WebApiRouteErrorData(routePath, httpContext.Request.Path, "Internal Server Error", (int)HttpStatusCode.InternalServerError, exception.Message);

            Log.Logger.Error(exception, "WebApiEndpoint error - {@eventData}", errorData);

            var errorResponse = $"WebApiEndpoint error for route : '{routePath}'".ToResultError().ToErrorStructure();

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(errorResponse, typeof(ResultErrorStructure), CancellationToken.None);
        }
    }

    private static Task<Result<(IWebApiEndpointDispatcher, IWebApiEndpointMiddlewareExecutor, IWebApiEndpoint)>> CallWebApiEndpointAsync(
        HttpContext httpContext, MetadataDefinition metadataDefinition, WebApiEndpointConfiguration configuration, CancellationToken cancellationToken)
    {
        var webApiEndpointDispatcher = WebApiEndpointDispatcherCache.GetOrAdd(metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType,
                                                                              webApiEndpointDispatcher =>
                                                                                  httpContext.RequestServices.TryGetService<IWebApiEndpointDispatcher>(webApiEndpointDispatcher));

        var middlewareExecutor = configuration.EnableMiddleware
            ? httpContext.RequestServices.TryGetService<IWebApiEndpointMiddlewareExecutor>(metadataDefinition.MetadataTypeDefinition.MiddlewareExecutorType)
            : WebApiEndpointMiddlewareExecutorCache.GetOrAdd(metadataDefinition.MetadataTypeDefinition.MiddlewareExecutorType,
                                                             middlewareExecutorType => httpContext.RequestServices.TryGetService<IWebApiEndpointMiddlewareExecutor>(middlewareExecutorType));

        var apiEndpoint = httpContext.RequestServices.TryGetService<IWebApiEndpoint>(metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType);

        return Result.CombineAll(webApiEndpointDispatcher, middlewareExecutor, apiEndpoint)
                     .ThenAsync(x => x.Item1.ExecuteAsync(metadataDefinition, httpContext, x.Item2, x.Item3, cancellationToken));
    }

    private static Task WebApiEndpointNotFoundAsync(HttpContext httpContext, string? routePath)
    {
        var eventData = new WebApiEndpointNotFoundData(routePath, httpContext.Request.Method);
        Log.Logger.Error("Unable to find WebApiEndpoint - {@eventData}", eventData);

        var errorResponse = $"WebApiEndpoint - Unable to find WebApiEndpoint for route : '{routePath}'".ToResultError().ToErrorStructure();

        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return httpContext.Response.WriteAsJsonAsync(errorResponse, typeof(ResultErrorStructure), CancellationToken.None);
    }

    private record struct WebApiEndpointNotFoundData(string RoutePath, string HttpMethod);

    private record struct WebApiRouteErrorData(string Route, string Path, string Status, int StatusCode, string Reason);
}