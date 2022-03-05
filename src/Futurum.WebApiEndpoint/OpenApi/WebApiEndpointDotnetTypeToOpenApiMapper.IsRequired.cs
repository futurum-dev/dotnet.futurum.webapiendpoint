using Futurum.Core.Option;
using Futurum.WebApiEndpoint.Internal;

namespace Futurum.WebApiEndpoint.OpenApi;

public  static class WebApiEndpointDotnetTypeToOpenApiIsRequiredMapper
{
    public static bool Execute(Type type) =>
        !type.IsClosedTypeOf(typeof(Option<>));
}