using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

namespace Futurum.WebApiEndpoint.Internal.Dispatcher;

internal interface IWebApiEndpointDispatcher
{
    Task<Result> ExecuteAsync(MetadataDefinition metadataDefinition, HttpContext httpContext, IWebApiEndpointMiddlewareExecutor middlewareExecutor, IWebApiEndpoint apiEndpoint,
                              CancellationToken cancellationToken);
}