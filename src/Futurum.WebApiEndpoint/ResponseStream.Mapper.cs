using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for ResponseStream
/// </summary>
public class ResponseStreamMapper : IWebApiEndpointResponseMapper<ResponseStream, ResponseStreamDto>
{
    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, ResponseStream domain, CancellationToken cancellation) => 
        httpContext.Response.TrySendResponseStreamAsync(domain.Stream, metadataRouteDefinition.SuccessStatusCode, domain.FileName, domain.FileLengthBytes, domain.ContentType, cancellation);
}