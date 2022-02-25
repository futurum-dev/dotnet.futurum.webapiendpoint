using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Internal;

internal partial interface IWebApiEndpointHttpContextDispatcher
{
    Task<Result<TRequestDto>> ReadRequestAsync<TRequestDto>(HttpContext httpContext, MetadataMapFromDefinition? metadataMapFromDefinition,
                                                            MetadataMapFromMultipartDefinition? metadataMapFromMultipartDefinition, CancellationToken cancellationToken)
        where TRequestDto : class;
}

internal partial class WebApiEndpointHttpContextDispatcher : IWebApiEndpointHttpContextDispatcher
{
    public Task<Result<TRequestDto>> ReadRequestAsync<TRequestDto>(HttpContext httpContext, MetadataMapFromDefinition? metadataMapFromDefinition,
                                                                   MetadataMapFromMultipartDefinition? metadataMapFromMultipartDefinition, CancellationToken cancellationToken)
        where TRequestDto : class
    {
        if (typeof(TRequestDto) == typeof(RequestPlainTextDto))
        {
            return httpContext.Request.ReadPlainTextBodyAsync()
                              .MapAsync(body => new RequestPlainTextDto(body) as TRequestDto);
        }

        if (typeof(TRequestDto) == typeof(RequestUploadFilesDto))
        {
            return httpContext.Request.ReadUploadFilesAsync(cancellationToken)
                              .MapAsync(files => new RequestUploadFilesDto(files) as TRequestDto);
        }

        if (typeof(TRequestDto) == typeof(RequestUploadFileDto))
        {
            return httpContext.Request.ReadUploadFilesAsync(cancellationToken)
                              .MapAsync(files => new RequestUploadFileDto(files.FirstOrDefault()) as TRequestDto)
                              .EnhanceWithErrorAsync(() => "Failed to read upload file");
        }

        if (typeof(TRequestDto) == typeof(EmptyRequestDto))
        {
            return (new EmptyRequestDto() as TRequestDto).ToResultOkAsync();
        }

        return ReadRequestDto<TRequestDto>(httpContext, metadataMapFromDefinition, metadataMapFromMultipartDefinition, cancellationToken)
            .ThenAsync(requestDto => ApplyMapFromMappingsAsync(httpContext, metadataMapFromDefinition, requestDto, cancellationToken)
                           .ThenAsync(requestDto => ApplyMapFromMultipartMappingsAsyncAsync(httpContext, requestDto, metadataMapFromMultipartDefinition, cancellationToken)));
    }

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