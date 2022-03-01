using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

public class ResponseStreamMapper<TApiEndpoint> : IWebApiEndpointResponseMapper<ResponseStream<TApiEndpoint>, ResponseStreamDto>
{
    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, ResponseStream<TApiEndpoint> domain, CancellationToken cancellation) => 
        httpContext.Response.TrySendResponseStreamAsync(domain.Stream, metadataRouteDefinition.SuccessStatusCode, domain.FileName, domain.FileLengthBytes, domain.ContentType, cancellation);
}