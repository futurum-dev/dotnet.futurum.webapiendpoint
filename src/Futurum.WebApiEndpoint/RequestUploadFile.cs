using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Request domain for upload file with <typeparamref name="TApiEndpoint"/>
/// </summary>
public record RequestUploadFile<TApiEndpoint>(IFormFile File)
{
    internal RequestUploadFile ToNonApiEndpoint() => new(File);
}

/// <summary>
/// Request domain for upload file
/// </summary>
public record RequestUploadFile(IFormFile File);

/// <summary>
/// Request dto for upload file
/// </summary>
public record RequestUploadFileDto(IFormFile File);

internal class RequestUploadFileMapper<TApiEndpoint> : IWebApiEndpointRequestMapper<RequestUploadFileDto, RequestUploadFile<TApiEndpoint>>
{
    public Result<RequestUploadFile<TApiEndpoint>> Map(HttpContext httpContext, RequestUploadFileDto dto) =>
        new RequestUploadFile<TApiEndpoint>(dto.File).ToResultOk();
}