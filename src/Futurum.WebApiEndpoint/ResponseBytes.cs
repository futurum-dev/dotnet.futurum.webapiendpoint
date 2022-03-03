using System.Net.Mime;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for bytes
/// </summary>
public record ResponseBytes(byte[] Bytes, string? FileName = null, string ContentType = MediaTypeNames.Application.Octet);

/// <summary>
/// Response dto for bytes
/// </summary>
public record ResponseBytesDto;