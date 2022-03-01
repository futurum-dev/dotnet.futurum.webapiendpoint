using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

internal class RequestUploadFileMapper<TApiEndpoint> : IWebApiEndpointRequestMapper<RequestUploadFile<TApiEndpoint>>
{
    public Task<Result<RequestUploadFile<TApiEndpoint>>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
        httpContext.Request.TryReadUploadFilesAsync(cancellationToken)
                   .MapAsync(files => new RequestUploadFile<TApiEndpoint>(files.FirstOrDefault()))
                   .EnhanceWithErrorAsync(() => "Failed to read upload file");
}