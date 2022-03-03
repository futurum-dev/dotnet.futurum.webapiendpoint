using System.Net.Mime;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for stream
/// </summary>
public record ResponseStream(Stream Stream, string? FileName = null, long? FileLengthBytes = null, string ContentType = MediaTypeNames.Application.Octet);

/// <summary>
/// Response dto for stream
/// </summary>
public record ResponseStreamDto;