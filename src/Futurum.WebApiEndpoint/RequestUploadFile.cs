namespace Futurum.WebApiEndpoint;

/// <summary>
/// Request domain for upload file
/// </summary>
public record RequestUploadFile(IFormFile File);

/// <summary>
/// Request dto for upload file
/// </summary>
public record RequestUploadFileDto;