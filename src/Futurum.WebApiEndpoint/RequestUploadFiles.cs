using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Request domain for upload files with <typeparamref name="TApiEndpoint"/>
/// </summary>
public record RequestUploadFiles<TApiEndpoint>(IEnumerable<IFormFile> Files)
{
    internal RequestUploadFiles ToNonApiEndpoint() => new(Files);
}

/// <summary>
/// Request domain for upload files
/// </summary>
public record RequestUploadFiles(IEnumerable<IFormFile> Files);

/// <summary>
/// Request dto for upload files
/// </summary>
public record RequestUploadFilesDto(IEnumerable<IFormFile> Files);

internal class RequestUploadFilesMapper<TApiEndpoint> : IWebApiEndpointRequestMapper<RequestUploadFilesDto, RequestUploadFiles<TApiEndpoint>>
{
    public Result<RequestUploadFiles<TApiEndpoint>> Map(HttpContext httpContext, RequestUploadFilesDto dto) =>
        new RequestUploadFiles<TApiEndpoint>(dto.Files).ToResultOk();
}