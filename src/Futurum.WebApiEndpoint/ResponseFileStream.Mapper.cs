using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for ResponseFileStream
/// </summary>
public class ResponseFileStreamMapper : IWebApiEndpointResponseMapper<ResponseFileStream, ResponseFileStreamDto>
{
    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, ResponseFileStream domain, CancellationToken cancellation) =>
        httpContext.Response.TrySendResponseFileAsync(domain.FileInfo, domain.Range, metadataRouteDefinition.SuccessStatusCode, domain.ContentType, cancellation);
}