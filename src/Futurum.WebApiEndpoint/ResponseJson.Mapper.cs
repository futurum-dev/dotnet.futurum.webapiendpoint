using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint;

internal class ResponseJsonMapper<TResponse, TResponseDto, TMapper> : IWebApiEndpointResponseMapper<TResponse, ResponseJsonDto<TResponseDto>>
    where TMapper : IWebApiEndpointResponseDtoMapper<TResponse, TResponseDto>
{
    private readonly IOptions<JsonOptions> _serializationOptions;
    private readonly TMapper _mapper;

    public ResponseJsonMapper(IOptions<JsonOptions> serializationOptions,
                              TMapper mapper)
    {
        _serializationOptions = serializationOptions;
        _mapper = mapper;
    }

    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, TResponse domain, CancellationToken cancellation)
    {
        var dto = _mapper.Map(domain);
        
        return httpContext.Response.TryWriteAsJsonAsync(dto, _serializationOptions.Value.JsonSerializerOptions, metadataRouteDefinition.SuccessStatusCode, cancellation);
    }
}