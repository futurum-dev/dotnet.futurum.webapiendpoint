using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for RequestEmpty
/// </summary>
public class RequestEmptyMapper : IWebApiEndpointRequestMapper<RequestEmpty>
{
    public Task<Result<RequestEmpty>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
        RequestEmpty.Default.ToResultOkAsync();
}

/// <summary>
/// Mapper for RequestEmpty
/// </summary>
public class RequestEmptyMapper<TRequest, TMapper> : IWebApiEndpointRequestMapper<TRequest>
    where TMapper : IWebApiEndpointRequestMapper<TRequest>
{
    private readonly TMapper _mapper;

    public RequestEmptyMapper(TMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<Result<TRequest>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
        _mapper.MapAsync(httpContext, metadataDefinition, cancellationToken);
}