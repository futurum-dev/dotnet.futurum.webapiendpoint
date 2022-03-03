using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint;

internal class ResponseAsyncEnumerableMapper<TData, TDataDto, TResponseDataMapper> : 
    IWebApiEndpointResponseMapper<ResponseAsyncEnumerable<TData>, ResponseAsyncEnumerableDto<TDataDto>>
    where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
{
    private readonly IOptions<JsonOptions> _serializationOptions;
    private readonly TResponseDataMapper _dataMapper;

    public ResponseAsyncEnumerableMapper(IOptions<JsonOptions> serializationOptions,
                                         TResponseDataMapper dataMapper)
    {
        _serializationOptions = serializationOptions;
        _dataMapper = dataMapper;
    }

    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, ResponseAsyncEnumerable<TData> domain, CancellationToken cancellation)
    {
        var data = domain.Data.Select(_dataMapper.Map);

        return httpContext.Response.TryWriteAsyncEnumerableAsJsonAsync(data, typeof(IAsyncEnumerable<TDataDto>), _serializationOptions.Value.JsonSerializerOptions,
                                                                       metadataRouteDefinition.SuccessStatusCode, cancellation);
    }
}