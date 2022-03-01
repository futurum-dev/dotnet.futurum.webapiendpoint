using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

internal class RequestUploadFilesMapper<TApiEndpoint> : IWebApiEndpointRequestMapper<RequestUploadFiles<TApiEndpoint>>
{
    public Task<Result<RequestUploadFiles<TApiEndpoint>>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
        httpContext.Request.TryReadUploadFilesAsync(cancellationToken)
                   .MapAsync(files => new RequestUploadFiles<TApiEndpoint>(files))
                   .EnhanceWithErrorAsync(() => "Failed to read upload files");
}