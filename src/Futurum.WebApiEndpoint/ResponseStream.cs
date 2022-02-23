using System.Net.Mime;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for stream with <typeparamref name="TApiEndpoint"/>
/// </summary>
public record ResponseStream<TApiEndpoint>(Stream Stream, string? FileName = null, long? FileLengthBytes = null, string ContentType = MediaTypeNames.Application.Octet);

/// <summary>
/// Response domain for stream
/// </summary>
public record ResponseStream(Stream Stream, string? FileName = null, long? FileLengthBytes = null, string ContentType = MediaTypeNames.Application.Octet)
{
    internal ResponseStream<TApiEndpoint> ToApiEndpoint<TApiEndpoint>() => new(Stream, FileName, FileLengthBytes, ContentType);
}

/// <summary>
/// Response dto for stream
/// </summary>
public record ResponseStreamDto(Stream Stream, string ContentType, string? FileName = null, long? FileLengthBytes = null) : IResponseStreamDto;

internal class ResponseStreamMapper<TApiEndpoint> : IWebApiEndpointResponseMapper<ResponseStream<TApiEndpoint>, ResponseStreamDto>
{
    public ResponseStreamDto Map(ResponseStream<TApiEndpoint> domain) => 
        new(domain.Stream, domain.ContentType, domain.FileName, domain.FileLengthBytes);
}