using System.Net.Mime;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for file-stream
/// </summary>
public record ResponseFileStream(FileInfo FileInfo, string ContentType = MediaTypeNames.Application.Octet);

/// <summary>
/// Response dto for file-stream
/// </summary>
public record ResponseFileStreamDto(FileInfo FileInfo, string ContentType);