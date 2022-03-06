using System.Text.Json;
using System.Text.Json.Nodes;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for ResponseEmptyJson
/// </summary>
public class ResponseEmptyJsonMapper : IWebApiEndpointResponseMapper<ResponseEmptyJson, ResponseEmptyJsonDto>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ResponseEmptyJsonMapper(IOptions<JsonOptions> serializationOptions)
    {
        _jsonSerializerOptions = serializationOptions.Value.SerializerOptions;
    }
    
    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, ResponseEmptyJson domain, CancellationToken cancellation) => 
        httpContext.Response.TryWriteAsJsonAsync(new JsonObject(), _jsonSerializerOptions, 200, cancellation);
}