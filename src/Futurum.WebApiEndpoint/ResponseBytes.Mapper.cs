using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

internal class ResponseBytesMapper<TApiEndpoint> : IWebApiEndpointResponseMapper<ResponseBytes<TApiEndpoint>, ResponseBytesDto>
{
    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, ResponseBytes<TApiEndpoint> domain, CancellationToken cancellation) =>
        httpContext.Response.TrySendResponseBytesAsync(domain.Bytes, metadataRouteDefinition.SuccessStatusCode, domain.FileName, domain.Bytes.Length, domain.ContentType, cancellation);
}