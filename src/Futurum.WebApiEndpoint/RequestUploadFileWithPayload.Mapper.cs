using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

internal class RequestUploadFileWithPayloadMapper<TPayloadDto, TPayload, TRequestPayloadMapper> : IWebApiEndpointRequestMapper<RequestUploadFileWithPayload<TPayload>>
    where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
{
    private readonly TRequestPayloadMapper _payloadMapper;

    public RequestUploadFileWithPayloadMapper(TRequestPayloadMapper payloadMapper)
    {
        _payloadMapper = payloadMapper;
    }

    public Task<Result<RequestUploadFileWithPayload<TPayload>>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
        MapFromRequestMultipartMapper<RequestUploadFileWithPayloadDto<TPayloadDto>>
            .MapAsync(httpContext, new RequestUploadFileWithPayloadDto<TPayloadDto>(), cancellationToken)
            .ThenAsync(dto => _payloadMapper.Map(dto.Payload)
                                            .Map(payload => new RequestUploadFileWithPayload<TPayload>(dto.File, payload)));
}