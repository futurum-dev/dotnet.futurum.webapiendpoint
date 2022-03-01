using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

public class ResponseFileStreamMapper<TApiEndpoint> : IWebApiEndpointResponseMapper<ResponseFileStream<TApiEndpoint>, ResponseFileStreamDto>
{
    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, ResponseFileStream<TApiEndpoint> domain, CancellationToken cancellation) =>
        httpContext.Response.TrySendResponseFileAsync(domain.FileInfo, metadataRouteDefinition.SuccessStatusCode, domain.ContentType, cancellation);
}