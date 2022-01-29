using System.Text.Json.Nodes;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint.Internal;

internal interface IWebApiEndpointHttpContextDispatcher
{
    Task<Result<TRequestDto>> ReadRequestAsync<TRequestDto>(HttpContext httpContext, MetadataMapFromDefinition? metadataMapFromDefinition, CancellationToken cancellationToken)
        where TRequestDto : class;

    Task<Result> HandleSuccessResponseAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, CancellationToken cancellation = default);
    Task<Result> HandleSuccessResponseAsync<TResponseDto>(HttpContext httpContext, TResponseDto response, MetadataRouteDefinition metadataRouteDefinition, CancellationToken cancellation = default);
    Task<Result> HandleFailedResponseAsync(HttpContext httpContext, IResultError error, MetadataRouteDefinition metadataRouteDefinition, CancellationToken cancellation = default);
}

internal class WebApiEndpointHttpContextDispatcher : IWebApiEndpointHttpContextDispatcher
{
    private readonly IOptions<JsonOptions> _serializationOptions;

    public WebApiEndpointHttpContextDispatcher(IOptions<JsonOptions> serializationOptions)
    {
        _serializationOptions = serializationOptions;
    }

    public Task<Result<TRequestDto>> ReadRequestAsync<TRequestDto>(HttpContext httpContext, MetadataMapFromDefinition? metadataMapFromDefinition, CancellationToken cancellationToken)
        where TRequestDto : class
    {
        if (typeof(TRequestDto) == typeof(RequestPlainTextDto))
        {
            return httpContext.Request.ReadPlainTextBodyAsync()
                              .MapAsync(body => new RequestPlainTextDto(body) as TRequestDto);
        }
        
        if (typeof(TRequestDto) == typeof(RequestUploadFilesDto))
        {
            return httpContext.Request.ReadUploadFilesAsync()
                              .MapAsync(files => new RequestUploadFilesDto(files) as TRequestDto);
        }

        if (httpContext.Request.HasJsonContentType() && httpContext.Request.ContentLength > 0)
        {
            if (metadataMapFromDefinition == null)
            {
                return ReadFromJsonAsync<TRequestDto>(httpContext, cancellationToken);
            }

            return ReadFromJsonAndMapFromAsync<TRequestDto>(httpContext, cancellationToken);
        }

        if (metadataMapFromDefinition != null)
        {
            return MapFromRequestMapper<TRequestDto>.Map(httpContext).ToResultAsync();
        }

        if (typeof(TRequestDto) == typeof(EmptyRequestDto))
        {
            return (new EmptyRequestDto() as TRequestDto).ToResultOkAsync();
        }

        return Result.FailAsync<TRequestDto>($"Unable to read request, unknown type : '{typeof(TRequestDto).FullName}'");
    }

    private Task<Result<TRequestDto?>> ReadFromJsonAsync<TRequestDto>(HttpContext httpContext, CancellationToken cancellationToken)
    {
        ValueTask<TRequestDto?> Execute() =>
            httpContext.Request.ReadFromJsonAsync<TRequestDto>(_serializationOptions.Value.JsonSerializerOptions, cancellationToken);

        return Result.TryAsync(Execute, () => "Failed to deserialize request as json");
    }

    private Task<Result<TRequestDto>> ReadFromJsonAndMapFromAsync<TRequestDto>(HttpContext httpContext, CancellationToken cancellationToken)
        where TRequestDto : class =>
        ReadFromJsonAsync<TRequestDto>(httpContext, cancellationToken)
            .ThenAsync(requestDto => MapFromRequestMapper<TRequestDto>.Map(httpContext, requestDto));

    public Task<Result> HandleSuccessResponseAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, CancellationToken cancellation = default)
    {
        httpContext.Response.StatusCode = metadataRouteDefinition.SuccessStatusCode;

        return Result.OkAsync();
    }

    public Task<Result> HandleSuccessResponseAsync<TResponseDto>(HttpContext httpContext, TResponseDto response, MetadataRouteDefinition metadataRouteDefinition, CancellationToken cancellation = default)
    {
        if (response is ResponseStreamDto responseStreamDto)
            return httpContext.Response.SendResponseStreamAsync(responseStreamDto.Stream, metadataRouteDefinition.SuccessStatusCode, responseStreamDto.FileName, responseStreamDto.FileLengthBytes,
                                                                responseStreamDto.ContentType, cancellation);
        if (response is ResponseFileStreamDto responseFileStreamDto)
            return httpContext.Response.SendResponseFileAsync(responseFileStreamDto.FileInfo, metadataRouteDefinition.SuccessStatusCode, responseFileStreamDto.ContentType, cancellation);
        if (response is ResponseBytesDto responseBytesDto)
            return httpContext.Response.SendResponseBytesAsync(responseBytesDto.Bytes, metadataRouteDefinition.SuccessStatusCode, responseBytesDto.FileName, responseBytesDto.Bytes.Length,
                                                               responseBytesDto.ContentType, cancellation);
        if (response.GetType().IsGenericType && response.GetType().GetGenericTypeDefinition() == typeof(ResponseAsyncEnumerableDto<>))
        {
            var value = response.GetType().GetProperty(nameof(ResponseAsyncEnumerableDto<int>.AsyncEnumerable)).GetValue(response, null);
            var type = typeof(IAsyncEnumerable<>).MakeGenericType(response.GetType().GetGenericArguments()[0]);

            return httpContext.Response.WriteAsyncEnumerableAsJsonAsync(value, type, _serializationOptions.Value.JsonSerializerOptions, metadataRouteDefinition.SuccessStatusCode, cancellation);
        }

        if (response is ResponseEmptyJsonDto)
            return httpContext.Response.WriteAsJsonAsync(new JsonObject(), _serializationOptions.Value.JsonSerializerOptions, 200, cancellation);

        return httpContext.Response.WriteAsJsonAsync(response, _serializationOptions.Value.JsonSerializerOptions, metadataRouteDefinition.SuccessStatusCode, cancellation);
    }

    public Task<Result> HandleFailedResponseAsync(HttpContext httpContext, IResultError error, MetadataRouteDefinition metadataRouteDefinition, CancellationToken cancellation = default)
    {
        var errorResponse = error.ToErrorStructure();

        return httpContext.Response.WriteAsJsonAsync(errorResponse, _serializationOptions.Value.JsonSerializerOptions, metadataRouteDefinition.FailedStatusCode, cancellation);
    }
}