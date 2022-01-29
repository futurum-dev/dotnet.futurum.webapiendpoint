using System.Security.Claims;

using FluentAssertions;

using Futurum.WebApiEndpoint.Internal.Authorization;

using Microsoft.AspNetCore.Authorization;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.Internal.Authorization;

public class CustomClaimCheckerTests
{
    [Fact]
    public void correct_handler_is_called()
    {
        var wasCalled = true;

        var handler = (AuthorizationHandlerContext authorizationHandlerContext) =>
        {
            wasCalled = true;
            return true;
        };

        var equalityClaimChecker = new CustomClaimChecker(handler);

        var authorizationHandlerContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement>(),
                                                                          new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>())),
                                                                          null);

        var result = equalityClaimChecker.Execute(authorizationHandlerContext);

        wasCalled.Should().BeTrue();
    }
}