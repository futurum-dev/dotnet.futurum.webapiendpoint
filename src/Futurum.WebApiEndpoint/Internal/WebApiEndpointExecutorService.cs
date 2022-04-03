using System.Net;
using System.Text.Json;

using Futurum.Core.Result;
using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Internal;

internal static class WebApiEndpointExecutorService
{
    public static Task ExecuteAsync(HttpContext httpContext, MetadataDefinition? metadataDefinition, string routePath, CancellationToken cancellationToken) =>
        metadataDefinition != null
            ? WebApiEndpointAsync(httpContext, metadataDefinition, routePath, cancellationToken)
            : WebApiEndpointNotFoundAsync(httpContext, routePath, cancellationToken);

    private static async Task WebApiEndpointAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, string routePath, CancellationToken cancellationToken)
    {
        try
        {
            await CallWebApiEndpointAsync(httpContext, metadataDefinition, cancellationToken).UnwrapAsync();
        }
        catch (Exception exception)
        {
            const HttpStatusCode internalServerError = HttpStatusCode.InternalServerError;

            string requestPath = httpContext.Request.Path;

            var webApiEndpointLogger = httpContext.RequestServices.GetService<IWebApiEndpointLogger>();
            webApiEndpointLogger.ErrorUnhandled(exception, routePath, requestPath, "Internal Server Error", (int)internalServerError, exception.Message);

            var errorResponse = internalServerError.ToResultError(exception.ToResultError())
                                                   .ToProblemDetails((int)internalServerError, requestPath);

            httpContext.Response.StatusCode = (int)internalServerError;

            httpContext.Response.ContentType = WebApiEndpointContentType.ProblemJson;

            await JsonSerializer.SerializeAsync(httpContext.Response.Body, errorResponse, (JsonSerializerOptions)null, cancellationToken);
        }
    }

    private static Task<Result<IWebApiEndpointDispatcher>> CallWebApiEndpointAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
        httpContext.RequestServices.TryGetService<IWebApiEndpointDispatcher>(metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType)
                   .ThenAsync(webApiEndpointDispatcher => webApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext, cancellationToken));

    private static Task WebApiEndpointNotFoundAsync(HttpContext httpContext, string routePath, CancellationToken cancellationToken)
    {
        const HttpStatusCode failedStatusCode = HttpStatusCode.BadRequest;

        string requestPath = httpContext.Request.Path;

        var webApiEndpointLogger = httpContext.RequestServices.GetService<IWebApiEndpointLogger>();
        webApiEndpointLogger.ErrorWebApiEndpointNotFound(routePath, httpContext.Request.Method);

        var errorResponse = $"WebApiEndpoint - Unable to find WebApiEndpoint for route : '{routePath}'".ToResultError()
                                                                                                       .ToProblemDetails((int)failedStatusCode, requestPath);

        httpContext.Response.StatusCode = (int)failedStatusCode;

        httpContext.Response.ContentType = WebApiEndpointContentType.ProblemJson;

        return JsonSerializer.SerializeAsync(httpContext.Response.Body, errorResponse, (JsonSerializerOptions)null, cancellationToken);
    }
}