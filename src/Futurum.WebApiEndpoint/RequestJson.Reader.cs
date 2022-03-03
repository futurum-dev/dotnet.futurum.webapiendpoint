using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint;

internal interface IRequestJsonReader<TRequestDto>
    where TRequestDto : class
{
    Task<Result<TRequestDto>> ExecuteAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken);
}

internal class RequestJsonReader<TRequestDto> : IRequestJsonReader<TRequestDto>
    where TRequestDto : class
{
    private readonly IOptions<JsonOptions> _serializationOptions;

    public RequestJsonReader(IOptions<JsonOptions> serializationOptions)
    {
        _serializationOptions = serializationOptions;
    }

    public Task<Result<TRequestDto>> ExecuteAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
        ReadRequestDto(httpContext, metadataDefinition.MetadataMapFromDefinition, metadataDefinition.MetadataMapFromMultipartDefinition, cancellationToken)
            .ThenAsync(requestDto => ApplyMapFromMappingsAsync(httpContext, metadataDefinition.MetadataMapFromDefinition, requestDto, cancellationToken)
                           .ThenAsync(() => ApplyMapFromMultipartMappingsAsyncAsync(httpContext, requestDto, metadataDefinition.MetadataMapFromMultipartDefinition, cancellationToken)));

    private Task<Result<TRequestDto>> ReadRequestDto(HttpContext httpContext, MetadataMapFromDefinition? metadataMapFromDefinition,
                                                     MetadataMapFromMultipartDefinition? metadataMapFromMultipartDefinition, CancellationToken cancellationToken)
    {
        if (httpContext.Request.HasJsonContentType() && httpContext.Request.ContentLength > 0)
        {
            return ReadFromJsonAsync(httpContext, cancellationToken);
        }

        if (metadataMapFromDefinition != null || metadataMapFromMultipartDefinition != null)
        {
            return typeof(TRequestDto).GetConstructor(Type.EmptyTypes) != null
                ? (Activator.CreateInstance(typeof(TRequestDto)) as TRequestDto).ToResultOkAsync()
                : Result.FailAsync<TRequestDto>($"RequestDto type : '{typeof(TRequestDto).FullName}', does not have an empty constructor");
        }

        return Result.FailAsync<TRequestDto>($"Unable to read request, unknown type : '{typeof(TRequestDto).FullName}'");
    }

    private static Result ApplyMapFromMappingsAsync(HttpContext httpContext, MetadataMapFromDefinition? metadataMapFromDefinition, TRequestDto requestDto, CancellationToken cancellationToken) =>
        metadataMapFromDefinition != null
            ? MapFromRequestMapper<TRequestDto>.Map(httpContext, requestDto, cancellationToken)
            : Result.Ok();

    private static Task<Result> ApplyMapFromMultipartMappingsAsyncAsync(HttpContext httpContext, TRequestDto requestDto, MetadataMapFromMultipartDefinition? metadataMapFromMultipartDefinition,
                                                                        CancellationToken cancellationToken) =>
        metadataMapFromMultipartDefinition != null
            ? MapFromRequestMultipartMapper<TRequestDto>.MapAsync(httpContext, requestDto, cancellationToken)
            : Result.OkAsync();

    private Task<Result<TRequestDto?>> ReadFromJsonAsync(HttpContext httpContext, CancellationToken cancellationToken)
    {
        ValueTask<TRequestDto?> Execute() =>
            httpContext.Request.ReadFromJsonAsync<TRequestDto>(_serializationOptions.Value.JsonSerializerOptions, cancellationToken);

        return Result.TryAsync(Execute, () => "Failed to deserialize request as json");
    }
}