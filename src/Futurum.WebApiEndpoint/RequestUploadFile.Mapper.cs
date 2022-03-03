using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for RequestUploadFile
/// </summary>
public class RequestUploadFileMapper : IWebApiEndpointRequestMapper<RequestUploadFile>
{
    public Task<Result<RequestUploadFile>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
        httpContext.Request.TryReadUploadFilesAsync(cancellationToken)
                   .MapAsync(files => new RequestUploadFile(files.FirstOrDefault()))
                   .EnhanceWithErrorAsync(() => "Failed to read upload file");
}