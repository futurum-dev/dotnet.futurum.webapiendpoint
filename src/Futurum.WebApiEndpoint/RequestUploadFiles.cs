namespace Futurum.WebApiEndpoint;

/// <summary>
/// Request domain for upload files
/// </summary>
public record RequestUploadFiles(IEnumerable<IFormFile> Files);

/// <summary>
/// Request dto for upload files
/// </summary>
public record RequestUploadFilesDto(IEnumerable<IFormFile> Files);