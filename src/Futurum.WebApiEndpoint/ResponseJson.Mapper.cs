using System.Text.Json;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for ResponseJson
/// </summary>
public class ResponseJsonMapper<TResponse, TResponseDto, TMapper> : IWebApiEndpointResponseMapper<TResponse, ResponseJsonDto<TResponseDto>>
    where TMapper : IWebApiEndpointResponseDtoMapper<TResponse, TResponseDto>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly TMapper _mapper;

    public ResponseJsonMapper(IOptions<JsonOptions> serializationOptions,
                              TMapper mapper)
    {
        _jsonSerializerOptions = serializationOptions.Value.SerializerOptions;
        _mapper = mapper;
    }

    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, TResponse domain, CancellationToken cancellation)
    {
        var dto = _mapper.Map(domain);
        
        return httpContext.Response.TryWriteAsJsonAsync(dto, _jsonSerializerOptions, metadataRouteDefinition.SuccessStatusCode, cancellation);
    }
}