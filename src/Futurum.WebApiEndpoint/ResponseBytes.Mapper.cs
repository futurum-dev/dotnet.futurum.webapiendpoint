using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for ResponseBytes
/// </summary>
public class ResponseBytesMapper : IWebApiEndpointResponseMapper<ResponseBytes, ResponseBytesDto>
{
    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, ResponseBytes domain, CancellationToken cancellation) =>
        httpContext.Response.TrySendResponseBytesAsync(domain.Bytes, domain.Range, metadataRouteDefinition.SuccessStatusCode, domain.FileName, domain.Bytes.Length, domain.ContentType, cancellation);
}