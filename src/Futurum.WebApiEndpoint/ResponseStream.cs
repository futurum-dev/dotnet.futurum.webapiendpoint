using System.Net.Mime;

using Futurum.Core.Option;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for stream
/// </summary>
public record ResponseStream(Stream Stream)
{
    public Option<Range> Range { get; set; } = default;
    public string? FileName { get; set; } = null;
    public long? FileLengthBytes { get; set; } = null;
    public string ContentType { get; set; } = MediaTypeNames.Application.Octet;
}

/// <summary>
/// Response dto for stream
/// </summary>
public record ResponseStreamDto;