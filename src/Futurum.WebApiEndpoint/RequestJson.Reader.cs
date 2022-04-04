using System.Text.Json;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint;

public interface IRequestJsonReader<TRequestDto>
    where TRequestDto : class
{
    Task<Result<TRequestDto>> ExecuteAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken);
}

internal class RequestJsonReader<TRequestDto> : IRequestJsonReader<TRequestDto>
    where TRequestDto : class
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public RequestJsonReader(IOptions<JsonOptions> serializationOptions)
    {
        _jsonSerializerOptions = serializationOptions.Value.SerializerOptions;
    }

    public Task<Result<TRequestDto>> ExecuteAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
        ReadRequestDto(httpContext, metadataDefinition.MetadataMapFromDefinition, cancellationToken)
            .ThenAsync(requestDto => ApplyMapFromMappingsAsync(httpContext, metadataDefinition.MetadataMapFromDefinition, requestDto, cancellationToken));

    private Task<Result<TRequestDto>> ReadRequestDto(HttpContext httpContext, MetadataMapFromDefinition? metadataMapFromDefinition, CancellationToken cancellationToken)
    {
        if (httpContext.Request.HasJsonContentType() && httpContext.Request.ContentLength > 0)
        {
            return ReadFromJsonAsync(httpContext, cancellationToken);
        }

        if (metadataMapFromDefinition != null)
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

    private Task<Result<TRequestDto?>> ReadFromJsonAsync(HttpContext httpContext, CancellationToken cancellationToken)
    {
        ValueTask<TRequestDto?> Execute() =>
            JsonSerializer.DeserializeAsync<TRequestDto>(httpContext.Request.Body, _jsonSerializerOptions, cancellationToken);

        return Result.TryAsync(Execute, "Failed to deserialize request as json");
    }
}