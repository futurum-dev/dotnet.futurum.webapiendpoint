using System.Net.Mime;

using Futurum.WebApiEndpoint.Internal;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for file-stream with <typeparamref name="TApiEndpoint"/>
/// </summary>
public record ResponseFileStream<TApiEndpoint>(FileInfo FileInfo, string ContentType = MediaTypeNames.Application.Octet);

/// <summary>
/// Response domain for file-stream
/// </summary>
public record ResponseFileStream(FileInfo FileInfo, string ContentType = MediaTypeNames.Application.Octet)
{
    internal ResponseFileStream<TApiEndpoint> ToApiEndpoint<TApiEndpoint>() => new(FileInfo, ContentType);
}

/// <summary>
/// Response dto for file-stream
/// </summary>
public record ResponseFileStreamDto(FileInfo FileInfo, string ContentType) : IResponseStreamDto;

internal class ResponseFileStreamMapper<TApiEndpoint> : IWebApiEndpointResponseMapper<ResponseFileStream<TApiEndpoint>, ResponseFileStreamDto>
{
    public ResponseFileStreamDto Map(ResponseFileStream<TApiEndpoint> domain) => 
        new(domain.FileInfo, domain.ContentType);
}