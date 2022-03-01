using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

internal class RequestPlainTextMapper<TApiEndpoint> : IWebApiEndpointRequestMapper<RequestPlainText<TApiEndpoint>>
{
    public Task<Result<RequestPlainText<TApiEndpoint>>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
        httpContext.Request.TryReadPlainTextBodyAsync()
                   .MapAsync(body => new RequestPlainText<TApiEndpoint>(body));
}