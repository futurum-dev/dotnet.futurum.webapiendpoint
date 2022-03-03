using System.Text.Json.Nodes;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for ResponseEmptyJson
/// </summary>
public class ResponseEmptyJsonMapper : IWebApiEndpointResponseMapper<ResponseEmptyJson, ResponseEmptyJsonDto>
{
    private readonly IOptions<JsonOptions> _serializationOptions;

    public ResponseEmptyJsonMapper(IOptions<JsonOptions> serializationOptions)
    {
        _serializationOptions = serializationOptions;
    }
    
    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, ResponseEmptyJson domain, CancellationToken cancellation) => 
        httpContext.Response.TryWriteAsJsonAsync(new JsonObject(), _serializationOptions.Value.JsonSerializerOptions, 200, cancellation);
}