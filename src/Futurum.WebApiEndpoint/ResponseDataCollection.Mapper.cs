using System.Text.Json;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for ResponseDataCollection
/// </summary>
public class ResponseDataCollectionMapper<TData, TDataDto, TResponseDataMapper> : IWebApiEndpointResponseMapper<ResponseDataCollection<TData>, ResponseDataCollectionDto<TDataDto>>
    where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly TResponseDataMapper _dataMapper;

    public ResponseDataCollectionMapper(IOptions<JsonOptions> serializationOptions,
                                        TResponseDataMapper dataMapper)
    {
        _jsonSerializerOptions = serializationOptions.Value.SerializerOptions;
        _dataMapper = dataMapper;
    }

    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, ResponseDataCollection<TData> domain, CancellationToken cancellation)
    {
        var data = domain.Data.Select(_dataMapper.Map)
                         .ToList();

        var dto = new ResponseDataCollectionDto<TDataDto>(data);

        return httpContext.Response.TryWriteAsJsonAsync(dto, _jsonSerializerOptions, metadataRouteDefinition.SuccessStatusCode, cancellation);
    }
}