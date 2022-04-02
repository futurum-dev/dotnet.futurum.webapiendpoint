using System.Net.Mime;

using Futurum.Core.Option;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for bytes
/// </summary>
public record ResponseBytes(byte[] Bytes)
{
    public Option<Range> Range { get; set; } = default;
    public string? FileName { get; set; } = null;
    public string ContentType { get; set; } = MediaTypeNames.Application.Octet;
}

/// <summary>
/// Response dto for bytes
/// </summary>
public record ResponseBytesDto;