using System.Net;
using System.Text.Json;

using Futurum.Core.Result;
using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Metadata;

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

            var errorResponse = HttpStatusCode.InternalServerError.ToResultError(exception.ToResultError())
                                         .ToProblemDetails((int)HttpStatusCode.InternalServerError, httpContext.Request.Path);

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            httpContext.Response.ContentType = WebApiEndpointContentType.ProblemJson;

            await JsonSerializer.SerializeAsync(httpContext.Response.Body, errorResponse, (JsonSerializerOptions)null, cancellationToken);
        }
    }

    private static Task<Result<IWebApiEndpointDispatcher>> CallWebApiEndpointAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
        httpContext.RequestServices.TryGetService<IWebApiEndpointDispatcher>(metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType)
                   .ThenAsync(webApiEndpointDispatcher => webApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext,  cancellationToken));

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