using System.Net.Mime;

using Futurum.Core.Option;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for file-stream
/// </summary>
public record ResponseFileStream(FileInfo FileInfo)
{
    public Option<Range> Range { get; set; } = default;
    public string ContentType { get; set; } = MediaTypeNames.Application.Octet;
}

/// <summary>
/// Response dto for file-stream
/// </summary>
public record ResponseFileStreamDto;