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
        ReadRequestDto<TRequestDto>(httpContext, metadataDefinition.MetadataMapFromDefinition, metadataDefinition.MetadataMapFromMultipartDefinition, cancellationToken)
            .ThenAsync(requestDto => ApplyMapFromMappingsAsync(httpContext, metadataDefinition.MetadataMapFromDefinition, requestDto, cancellationToken)
                           .ThenAsync(requestDto => ApplyMapFromMultipartMappingsAsyncAsync(httpContext, requestDto, metadataDefinition.MetadataMapFromMultipartDefinition, cancellationToken)));


    private Task<Result<TRequestDto>> ReadRequestDto<TRequestDto>(HttpContext httpContext, MetadataMapFromDefinition? metadataMapFromDefinition,
                                                                  MetadataMapFromMultipartDefinition? metadataMapFromMultipartDefinition, CancellationToken cancellationToken)
        where TRequestDto : class
    {
        if (httpContext.Request.HasJsonContentType() && httpContext.Request.ContentLength > 0)
        {
            return ReadFromJsonAsync<TRequestDto>(httpContext, cancellationToken);
        }

        if (metadataMapFromDefinition != null || metadataMapFromMultipartDefinition != null)
        {
            return typeof(TRequestDto).GetConstructor(Type.EmptyTypes) != null
                ? (Activator.CreateInstance(typeof(TRequestDto)) as TRequestDto).ToResultOkAsync()
                : Result.FailAsync<TRequestDto>($"RequestDto type : '{typeof(TRequestDto).FullName}', does not have an empty constructor");
        }

        return Result.FailAsync<TRequestDto>($"Unable to read request, unknown type : '{typeof(TRequestDto).FullName}'");
    }

    private static Result<TRequestDto> ApplyMapFromMappingsAsync<TRequestDto>(HttpContext httpContext, MetadataMapFromDefinition? metadataMapFromDefinition, TRequestDto requestDto,
                                                                              CancellationToken cancellationToken)
        where TRequestDto : class =>
        metadataMapFromDefinition != null
            ? MapFromRequestMapper<TRequestDto>.Map(httpContext, requestDto, cancellationToken)
            : requestDto.ToResultOk();

    private static async Task<Result<TRequestDto>> ApplyMapFromMultipartMappingsAsyncAsync<TRequestDto>(HttpContext httpContext, TRequestDto requestDto,
                                                                                                        MetadataMapFromMultipartDefinition? metadataMapFromMultipartDefinition,
                                                                                                        CancellationToken cancellationToken)
        where TRequestDto : class =>
        metadataMapFromMultipartDefinition != null
            ? await MapFromRequestMultipartMapper<TRequestDto>.MapAsync(httpContext, requestDto, cancellationToken)
            : requestDto.ToResultOk();

    private Task<Result<TRequestDto?>> ReadFromJsonAsync<TRequestDto>(HttpContext httpContext, CancellationToken cancellationToken)
    {
        ValueTask<TRequestDto?> Execute() =>
            httpContext.Request.ReadFromJsonAsync<TRequestDto>(_serializationOptions.Value.JsonSerializerOptions, cancellationToken);

        return Result.TryAsync(Execute, () => "Failed to deserialize request as json");
    }
}