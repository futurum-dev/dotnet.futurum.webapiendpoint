using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for RequestUploadFiles
/// </summary>
public class RequestUploadFilesMapper : IWebApiEndpointRequestMapper<RequestUploadFiles>
{
    public Task<Result<RequestUploadFiles>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
        httpContext.Request.TryReadUploadFilesAsync(cancellationToken)
                   .MapAsync(files => new RequestUploadFiles(files))
                   .EnhanceWithErrorAsync("Failed to read upload files");
}