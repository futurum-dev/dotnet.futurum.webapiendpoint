using System.Net.Mime;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for bytes with <typeparamref name="TApiEndpoint"/>
/// </summary>
public record ResponseBytes<TApiEndpoint>(byte[] Bytes, string? FileName = null, string ContentType = MediaTypeNames.Application.Octet);

/// <summary>
/// Response domain for bytes
/// </summary>
public record ResponseBytes(byte[] Bytes, string? FileName = null, string ContentType = MediaTypeNames.Application.Octet)
{
    internal ResponseBytes<TApiEndpoint> ToApiEndpoint<TApiEndpoint>() => new(Bytes, FileName, ContentType);
}

/// <summary>
/// Response dto for bytes
/// </summary>
public record ResponseBytesDto(byte[] Bytes, string ContentType, string? FileName = null) : IResponseStreamDto;

internal class ResponseBytesMapper<TApiEndpoint> : IWebApiEndpointResponseMapper<ResponseBytes<TApiEndpoint>, ResponseBytesDto>
{
    public Result<ResponseBytesDto> Map(ResponseBytes<TApiEndpoint> domain) =>
        new ResponseBytesDto(domain.Bytes, domain.ContentType, domain.FileName).ToResultOk();
}