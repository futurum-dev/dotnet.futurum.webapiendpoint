using System.Text.Json;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for MapFromMultipart
/// </summary>
public class RequestMapFromMultipartMapper<TRequestDto, TRequest, TMapper> : IWebApiEndpointRequestMapper<TRequest>
    where TRequestDto : class, new()
    where TMapper : IWebApiEndpointRequestMapper<TRequestDto, TRequest>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly TMapper _mapper;

    public RequestMapFromMultipartMapper(IOptions<JsonOptions> serializationOptions,
                                         TMapper mapper)
    {
        _jsonSerializerOptions = serializationOptions.Value.SerializerOptions;
        _mapper = mapper;
    }

    public Task<Result<TRequest>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken)
    {
        var dto = new TRequestDto();

        return MapFromRequestMultipartMapper<TRequestDto>
               .MapAsync(httpContext, _jsonSerializerOptions, dto, cancellationToken)
               .ThenAsync(() => _mapper.MapAsync(httpContext, metadataDefinition, dto, cancellationToken));
    }
}