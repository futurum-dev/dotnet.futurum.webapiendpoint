using System.Text.Json;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for ResponseAsyncEnumerable
/// </summary>
public class ResponseAsyncEnumerableMapper<TData, TDataDto, TResponseDataMapper> : IWebApiEndpointResponseMapper<ResponseAsyncEnumerable<TData>, ResponseAsyncEnumerableDto<TDataDto>>
    where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly TResponseDataMapper _dataMapper;

    public ResponseAsyncEnumerableMapper(IOptions<JsonOptions> serializationOptions,
                                         TResponseDataMapper dataMapper)
    {
        _jsonSerializerOptions = serializationOptions.Value.SerializerOptions;
        _dataMapper = dataMapper;
    }

    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, ResponseAsyncEnumerable<TData> domain, CancellationToken cancellation)
    {
        var data = domain.Data.Select(_dataMapper.Map);

        return httpContext.Response.TryWriteAsyncEnumerableAsJsonAsync(data, _jsonSerializerOptions, metadataRouteDefinition.SuccessStatusCode, cancellation);
    }
}