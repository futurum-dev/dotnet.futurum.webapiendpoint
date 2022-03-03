using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

internal class RequestPlainTextMapper : IWebApiEndpointRequestMapper<RequestPlainText>
{
    public Task<Result<RequestPlainText>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
        httpContext.Request.TryReadPlainTextBodyAsync()
                   .MapAsync(body => new RequestPlainText(body));
}