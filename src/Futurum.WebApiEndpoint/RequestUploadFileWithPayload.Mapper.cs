using System.Text.Json;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for RequestUploadFileWithPayload
/// </summary>
public class RequestUploadFileWithPayloadMapper<TPayloadDto, TPayload, TRequestPayloadMapper> : IWebApiEndpointRequestMapper<RequestUploadFileWithPayload<TPayload>>
    where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly TRequestPayloadMapper _payloadMapper;

    public RequestUploadFileWithPayloadMapper(IOptions<JsonOptions> serializationOptions,
                                              TRequestPayloadMapper payloadMapper)
    {
        _payloadMapper = payloadMapper;
        _jsonSerializerOptions = serializationOptions.Value.SerializerOptions;
    }

    public Task<Result<RequestUploadFileWithPayload<TPayload>>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken)
    {
        var dto = new RequestUploadFileWithPayloadDto<TPayloadDto>();
        
        return MapFromRequestMultipartMapper<RequestUploadFileWithPayloadDto<TPayloadDto>>
               .MapAsync(httpContext, _jsonSerializerOptions, dto, cancellationToken)
               .ThenAsync(() => _payloadMapper.Map(dto.Payload)
                                              .Map(payload => new RequestUploadFileWithPayload<TPayload>(dto.File, payload)));
    }
}