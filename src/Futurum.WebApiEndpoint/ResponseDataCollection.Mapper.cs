using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint;

internal class ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper> : IWebApiEndpointResponseMapper<ResponseDataCollection<TApiEndpoint, TData>, ResponseDataCollectionDto<TDataDto>>
    where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
{
    private readonly IOptions<JsonOptions> _serializationOptions;
    private readonly TResponseDataMapper _dataMapper;

    public ResponseDataCollectionMapper(IOptions<JsonOptions> serializationOptions,
                                        TResponseDataMapper dataMapper)
    {
        _serializationOptions = serializationOptions;
        _dataMapper = dataMapper;
    }

    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, ResponseDataCollection<TApiEndpoint, TData> domain, CancellationToken cancellation)
    {
        var data = domain.Data.Select(_dataMapper.Map)
                         .ToList();

        var dto = new ResponseDataCollectionDto<TDataDto>(data);

        return httpContext.Response.TryWriteAsJsonAsync(dto, _serializationOptions.Value.JsonSerializerOptions, metadataRouteDefinition.SuccessStatusCode, cancellation);
    }
}