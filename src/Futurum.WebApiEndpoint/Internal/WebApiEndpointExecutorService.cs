using System.Net;
using System.Net.Mime;
using System.Text.Json;

using Futurum.Core.Result;
using Futurum.Microsoft.Extensions.DependencyInjection;
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
            var errorData = new IWebApiEndpointLogger.WebApiRouteErrorData(routePath, httpContext.Request.Path, "Internal Server Error", (int)HttpStatusCode.InternalServerError, exception.Message);

            var webApiEndpointLogger = httpContext.RequestServices.GetService<IWebApiEndpointLogger>();
            webApiEndpointLogger.Error(exception, errorData);

            var errorResponse = exception.ToResultError("WebApiEndpoint - Internal Server Error").ToErrorStructure();

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            
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

        var errorResponse = $"WebApiEndpoint - Unable to find WebApiEndpoint for route : '{routePath}'".ToResultError().ToErrorStructure();

        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            
        return JsonSerializer.SerializeAsync(httpContext.Response.Body, errorResponse, (JsonSerializerOptions)null, cancellationToken);
    }
}