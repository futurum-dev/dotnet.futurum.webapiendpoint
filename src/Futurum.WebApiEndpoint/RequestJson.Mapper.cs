using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for RequestJson
/// </summary>
public class RequestJsonMapper<TRequestDto, TRequest, TMapper> : IWebApiEndpointRequestMapper<TRequest>
    where TRequestDto : class
    where TMapper : IWebApiEndpointRequestMapper<TRequestDto, TRequest>
{
    private readonly IRequestJsonReader<TRequestDto> _jsonReader;
    private readonly TMapper _mapper;
    private readonly IWebApiEndpointRequestValidation<TRequestDto> _requestValidation;

    public RequestJsonMapper(IRequestJsonReader<TRequestDto> jsonReader,
                             TMapper mapper,
                             IWebApiEndpointRequestValidation<TRequestDto> requestValidation)
    {
        _jsonReader = jsonReader;
        _mapper = mapper;
        _requestValidation = requestValidation;
    }

    public Task<Result<TRequest>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
        _jsonReader.ExecuteAsync(httpContext, metadataDefinition, cancellationToken)
                   .ThenAsync(dto => _requestValidation.ExecuteAsync(dto))
                   .ThenAsync(dto => _mapper.MapAsync(httpContext, metadataDefinition, dto, cancellationToken));
}