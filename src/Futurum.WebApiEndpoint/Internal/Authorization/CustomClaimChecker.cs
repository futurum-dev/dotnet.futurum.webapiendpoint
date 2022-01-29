using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Authorization;

namespace Futurum.WebApiEndpoint.Internal.Authorization;

internal class CustomClaimChecker : IClaimChecker
{
    public CustomClaimChecker(Func<AuthorizationHandlerContext, bool> handler)
    {
        Handler = handler;
    }

    public Func<AuthorizationHandlerContext, bool> Handler { get; }

    public bool Execute(AuthorizationHandlerContext authorizationHandlerContext) =>
        Handler(authorizationHandlerContext);
}