using System.Text.Json.Nodes;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Internal;

internal partial interface IWebApiEndpointHttpContextDispatcher
{
    Task<Result> HandleSuccessResponseAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, CancellationToken cancellation = default);
    Task<Result> HandleSuccessResponseAsync<TResponseDto>(HttpContext httpContext, TResponseDto response, MetadataRouteDefinition metadataRouteDefinition, CancellationToken cancellation = default);
}

internal partial class WebApiEndpointHttpContextDispatcher : IWebApiEndpointHttpContextDispatcher
{
    public Task<Result> HandleSuccessResponseAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, CancellationToken cancellation = default)
    {
        httpContext.Response.StatusCode = metadataRouteDefinition.SuccessStatusCode;

        return Result.OkAsync();
    }

    public Task<Result> HandleSuccessResponseAsync<TResponseDto>(HttpContext httpContext, TResponseDto response, MetadataRouteDefinition metadataRouteDefinition,
                                                                 CancellationToken cancellation = default)
    {
        switch (response)
        {
            case ResponseStreamDto(var stream, var contentType, var fileName, var fileLengthBytes):
                return httpContext.Response.SendResponseStreamAsync(stream, metadataRouteDefinition.SuccessStatusCode, fileName, fileLengthBytes, contentType, cancellation);
            case ResponseFileStreamDto(var fileInfo, var contentType):
                return httpContext.Response.SendResponseFileAsync(fileInfo, metadataRouteDefinition.SuccessStatusCode, contentType, cancellation);
            case ResponseBytesDto(var bytes, var contentType, var fileName):
                return httpContext.Response.SendResponseBytesAsync(bytes, metadataRouteDefinition.SuccessStatusCode, fileName, bytes.Length, contentType, cancellation);
            case ResponseEmptyJsonDto:
                return httpContext.Response.WriteAsJsonAsync(new JsonObject(), _serializationOptions.Value.JsonSerializerOptions, 200, cancellation);
        }

        if (response.GetType().IsGenericType && response.GetType().GetGenericTypeDefinition() == typeof(ResponseAsyncEnumerableDto<>))
        {
            var value = response.GetType().GetProperty(nameof(ResponseAsyncEnumerableDto<int>.AsyncEnumerable)).GetValue(response, null);
            var type = typeof(IAsyncEnumerable<>).MakeGenericType(response.GetType().GetGenericArguments()[0]);

            return httpContext.Response.WriteAsyncEnumerableAsJsonAsync(value, type, _serializationOptions.Value.JsonSerializerOptions, metadataRouteDefinition.SuccessStatusCode, cancellation);
        }

        return httpContext.Response.WriteAsJsonAsync(response, _serializationOptions.Value.JsonSerializerOptions, metadataRouteDefinition.SuccessStatusCode, cancellation);
    }
}