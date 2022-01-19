using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Internal;

internal interface IEndpointRouteSecurityBuilder
{
    void Configure(RouteHandlerBuilder routeHandlerBuilder, WebApiEndpointConfiguration webApiEndpointConfiguration, MetadataDefinition metadataDefinition);
}

internal class EndpointRouteSecurityBuilder : IEndpointRouteSecurityBuilder
{
    public void Configure(RouteHandlerBuilder routeHandlerBuilder, WebApiEndpointConfiguration webApiEndpointConfiguration, MetadataDefinition metadataDefinition)
    {
        metadataDefinition.MetadataRouteDefinition.SecurityDefinition
                          .DoSwitch(_ => routeHandlerBuilder.RequireAuthorization(AuthorizationExtensions.ToAuthorizationPolicy(metadataDefinition)),
                                    () =>
                                    {
                                        if (webApiEndpointConfiguration.SecureByDefault)
                                        {
                                            routeHandlerBuilder.RequireAuthorization();
                                        }
                                        else
                                        {
                                            routeHandlerBuilder.AllowAnonymous();
                                        }
                                    });
    }
}