using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for RequestUploadFileWithPayload
/// </summary>
public class RequestUploadFileWithPayloadMapper<TPayloadDto, TPayload, TRequestPayloadMapper> : IWebApiEndpointRequestMapper<RequestUploadFileWithPayload<TPayload>>
    where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
{
    private readonly TRequestPayloadMapper _payloadMapper;

    public RequestUploadFileWithPayloadMapper(TRequestPayloadMapper payloadMapper)
    {
        _payloadMapper = payloadMapper;
    }

    public Task<Result<RequestUploadFileWithPayload<TPayload>>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken)
    {
        var dto = new RequestUploadFileWithPayloadDto<TPayloadDto>();
        
        return MapFromRequestMultipartMapper<RequestUploadFileWithPayloadDto<TPayloadDto>>
               .MapAsync(httpContext, dto, cancellationToken)
               .ThenAsync(() => _payloadMapper.Map(dto.Payload)
                                              .Map(payload => new RequestUploadFileWithPayload<TPayload>(dto.File, payload)));
    }
}