namespace Futurum.WebApiEndpoint;

/// <summary>
/// Request domain for upload file with payload
/// </summary>
public record RequestUploadFileWithPayload<TPayload>(IFormFile File, TPayload Payload);

/// <summary>
/// Request dto for upload file with payload
/// </summary>
public record RequestUploadFileWithPayloadDto<TPayload>
{
    [MapFromMultipartFile(0)] public IFormFile File { get; set; }

    [MapFromMultipartJson(1)] public TPayload Payload { get; set; }
}