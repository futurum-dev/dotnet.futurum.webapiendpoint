using System.Net;
using System.Text.Json;

using Futurum.Core.Result;
using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

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
            var failedStatusCode = (int)HttpStatusCode.InternalServerError;
            
            var errorData = new IWebApiEndpointLogger.WebApiRouteErrorData(routePath, httpContext.Request.Path, "Internal Server Error", failedStatusCode, exception.Message);

            var webApiEndpointLogger = httpContext.RequestServices.GetService<IWebApiEndpointLogger>();
            webApiEndpointLogger.Error(exception, errorData);

            var errorResponse = exception.ToResultError("WebApiEndpoint - Internal Server Error")
                                         .ToProblemDetails(failedStatusCode, httpContext.Request.Path);

            httpContext.Response.StatusCode = failedStatusCode;

            httpContext.Response.ContentType = WebApiEndpointContentType.ProblemJson;

            await JsonSerializer.SerializeAsync(httpContext.Response.Body, errorResponse, (JsonSerializerOptions)null, cancellationToken);
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
        var eventData = new IWebApiEndpointLogger.WebApiEndpointNotFoundData(routePath, httpContext.Request.Method);

        var webApiEndpointLogger = httpContext.RequestServices.GetService<IWebApiEndpointLogger>();
        webApiEndpointLogger.Error(eventData);

        var failedStatusCode = (int)HttpStatusCode.BadRequest;

        var errorResponse = $"WebApiEndpoint - Unable to find WebApiEndpoint for route : '{routePath}'"
                            .ToResultError()
                            .ToProblemDetails(failedStatusCode, httpContext.Request.Path);
        
        httpContext.Response.StatusCode = failedStatusCode;

        httpContext.Response.ContentType = WebApiEndpointContentType.ProblemJson;

        return JsonSerializer.SerializeAsync(httpContext.Response.Body, errorResponse, (JsonSerializerOptions)null, cancellationToken);
    }
}