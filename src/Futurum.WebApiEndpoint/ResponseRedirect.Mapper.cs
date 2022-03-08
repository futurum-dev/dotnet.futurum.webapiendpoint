using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Mapper for ResponseRedirect
/// </summary>
public class ResponseRedirectMapper : IWebApiEndpointResponseMapper<ResponseRedirect, ResponseRedirectDto>
{
    public async Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, ResponseRedirect domain, CancellationToken cancellation)
    {
        httpContext.Response.Redirect(domain.Location, domain.Permanent);

        await httpContext.Response.StartAsync(cancellation);

        return Result.Ok();
    }
}